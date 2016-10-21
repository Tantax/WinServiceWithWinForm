using IAICN.GasFee.Infrastructure;
using IAICN.GasFee.Model;
using IAICN.GasFee.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using Newtonsoft.Json;
using IAICN.GasFee.DTO.SendSMS;

namespace IAICN.GasFee.SendSMS.Service
{
    public partial class SendSMSService : ServiceBase
    {
        public SendSMSService()
        {
            InitializeComponent();
            LogHelper.SetConfig();
        }

        protected override void OnStart(string[] args)
        {
            this.timer1.Start();
            LogHelper.WriteLog("服务启动，定时器开始计时");
            timeTick();
        }

        protected override void OnStop()
        {
            this.timer1.Stop();
            LogHelper.WriteLog("服务停止，定时器关闭");
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 更新时间到            
            timeTick();
        }

        /// <summary>
        /// 定时时间到
        /// </summary>
        private void timeTick()
        {
            LogHelper.WriteLog("-------------------------------开始执行任务----------------------------");
            ConfigurationHelper config = new ConfigurationHelper();
            string msgContentFormat = config.GetAppSetting("MsgContent");
            if (string.IsNullOrEmpty(config.GetAppSetting("SendTime")))
            {
                // 即刻发送短信
                LogHelper.WriteLog("即刻发送短信");
                QueryIsArrear(msgContentFormat);
            }
            else
            {
                int timeInterval = GetSendMsgIntervalDays();
                if (timeInterval != -1)
                {
                    // 检查上一次发送时间
                    DateTime sendTime = Convert.ToDateTime(config.GetAppSetting("SendTime"));
                    TimeSpan time = DateTime.Now - sendTime;
                    // 距离上一次发短信间隔天数
                    int intervalDays = time.Days;
                    LogHelper.WriteLog("上次发送时间：" + sendTime + ";发送短信间隔天数：" + timeInterval + ";");
                    if (intervalDays >= timeInterval)
                    {
                        QueryIsArrear(msgContentFormat);
                    }
                }
            }
            LogHelper.WriteLog("-------------------------------执行任务完毕----------------------------");
        }

        /// <summary>
        /// 查询是否欠费并发送短信
        /// </summary>
        private void QueryIsArrear(string msgContentFormat)
        {
            List<GasMeter> list = GetGasMeterList();
            LogHelper.WriteLog("开通短信通知的燃气表数：" + (list == null ? 0 : list.Count));
            if (list != null && list.Count > 0)
            {
                int arrearsInformDays = GetBeginInformArrearDays();
                LogHelper.WriteLog("开始通知欠费天数:" + arrearsInformDays);
                if (arrearsInformDays != -1)
                {
                    LogHelper.WriteLog("------循环遍历燃气表开始------");
                    foreach (GasMeter item in list)
                    {
                        // 所欠气量
                        decimal arrearVolume = (item.TotalAddGasNo.HasValue ? item.TotalAddGasNo.Value : 0) + item.TotalGasNo - item.TotalUseNo;
                        if (item.ArrearsDate.HasValue && item.ArrearsDate.Value != DateTime.MinValue && arrearVolume < 0 && item.ResidualMoney < 0) // 欠费
                        {
                            LogHelper.WriteLog("所欠气量：" + arrearVolume + ";欠费金额：" + item.ResidualMoney + ";欠费日期：" + item.ArrearsDate.Value + ";");
                            TimeSpan timeSpan = DateTime.Now - item.ArrearsDate.Value;
                            if (timeSpan.Days >= arrearsInformDays)// 超过欠费通知天数
                            {
                                string msg = string.Format(msgContentFormat, item.ResidualMoney.Value.ToString("F2"));
                                try
                                {
                                    SendMsg(msg, HttpHelper.GetInternetIP(), item);
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.WriteLog(ex.Message, ex);
                                }
                            }
                        }
                    }
                    LogHelper.WriteLog("------循环遍历燃气表结束------");
                }
            }
        }

        /// <summary>
        /// 查询发送短信间隔天数
        /// </summary>
        private int GetSendMsgIntervalDays()
        {
            try
            {
                Arrears queryModel = new Arrears();
                queryModel.ArrsearName = ConfigurationHelper.GetAppSettings("ArrsearNameSendMsgInterval");
                queryModel.ArrsearKey = ConfigurationHelper.GetAppSettings("ArrsearKeySendMsgInterval");
                return Convert.ToInt32(GetArrears(queryModel).ArrsearValue);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("查询发送短信间隔天数错误，错误信息：" + ex.Message, ex);
                return -1;
            }
        }

        /// <summary>
        /// 查询开始通知欠费天数
        /// </summary>
        /// <returns></returns>
        private int GetBeginInformArrearDays()
        {
            try
            {
                Arrears queryModel = new Arrears();
                queryModel.ArrsearName = ConfigurationHelper.GetAppSettings("ArrsearNameSendMsg");
                queryModel.ArrsearKey = ConfigurationHelper.GetAppSettings("ArrsearKeySendMsg");
                return Convert.ToInt32(GetArrears(queryModel).ArrsearValue);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("查询开始通知欠费天数错误，错误信息：" + ex.Message, ex);
                return -1;
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        /// <param name="model"></param>
        private void SendMsg(string msg, string ip, GasMeter model)
        {
            string sendMsgApi = ConfigurationHelper.GetAppSettings("SendMsgApi");
            string sendMgsApiUrl = TokenHelper.MsgCenterUrl + sendMsgApi + "?access_token=" + TokenHelper.Token; ;
            string param = JsonConvert.SerializeObject(new
            {
                sms = new
                {
                    type = "general",
                    reply = false,
                    general = new
                    {
                        content = msg
                    },
                    to = model.Tel,
                    totype = "id",
                    toname = model.CustomerName,
                    schedule = ""
                },
                @event = "DEFAULT",
                from = "System",
                cid = model.CompanyId,
                cname = "成都九门科技有限公司",
                ip = ip
            });
            string result = HttpHelper.HttpPost(sendMgsApiUrl, param);
            var DTO = JsonConvert.DeserializeObject<SendMessageResponseDTO>(result);
            if (DTO.errcode == 0) // 成功
            {
                // 把发送时间写入配置
                ConfigurationHelper config = new ConfigurationHelper();
                config.SetAppSetting("SendTime", DateTime.Now.ToString());
                // 写入日志                
                LogHelper.WriteLog("发送成功，发送成功的手机号：" + model.Tel + ";");
            }
            else
            {
                // 写入日志
                LogHelper.WriteLog("发送失败，发送失败的手机号：" + model.Tel + ";失败信息如下：" + DTO.errmsg);
            }
        }

        /// <summary>
        /// 获取欠费信息配置表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        private Arrears GetArrears(Arrears queryModel)
        {
            try
            {
                ArrearsRepository arrearsRepository = new ArrearsRepository();
                #region SQL
                string sql = @"SELECT 
	                                ID,
	                                CompanyId,
	                                ArrsearName,
	                                ArrsearKey,
	                                ArrsearValue,
	                                Remark
                                FROM T_Arrears
                                WHERE CompanyId = @CompanyId AND ArrsearName = @ArrsearName AND ArrsearKey = @ArrsearKey";
                #endregion
                List<SqlParameter> parList = new List<SqlParameter>();
                parList.Add(new SqlParameter("@CompanyId", ConfigurationHelper.GetAppSettings("CompanyId")));
                parList.Add(new SqlParameter("@ArrsearName", queryModel.ArrsearName));
                parList.Add(new SqlParameter("@ArrsearKey", queryModel.ArrsearKey));
                return arrearsRepository.Get(sql, parList);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取欠费信息配置表失败，错误信息：" + ex.Message);
                throw new Exception("获取欠费信息配置表失败", ex);
            }
        }

        /// <summary>
        /// 获取开通短信欠费通知的燃气表
        /// </summary>
        /// <returns></returns>
        private List<GasMeter> GetGasMeterList()
        {
            try
            {
                GasMeterRepository gasMeterRepository = new GasMeterRepository();
                #region SQL
                string sql = @"SELECT 
	                        ID,
	                        CompanyId,
	                        GasNo,
	                        GasTitle,
	                        IsOpen,
	                        CustomerName,
	                        Tel,
	                        Factory,
	                        GasType,
	                        BaseNo,
	                        LeftOrRigh,
	                        SelfClosing,
	                        GasBrand,
	                        GasVersion,
	                        CommunityName,
	                        GasAddress,
	                        GasPricePlan,
	                        OpenDate,
	                        ConnectionDate,
	                        BuildDate,
	                        GasStatus,
	                        LimitNo,
	                        TotalAddGasNo,
	                        TotalGasNo,
	                        TotalUseNo,
	                        ReceivableAmount,
	                        PaidAmount,
	                        NowGasNo,
	                        NowGasMoney,
	                        IsArrears,
	                        LastDate,
	                        LastNo,
	                        LastMoney,
	                        Province,
	                        City,
	                        County,
	                        UserName,
	                        TrueName,
	                        CreateDate,
	                        IsDelete,
	                        ArrearsDate,
	                        ResidualMoney,
	                        TotalAddGasMoney,
	                        LastReadMeterDate
                        FROM T_GasMeter
                        WHERE  CompanyId = @CompanyId AND IsDelete = 0 
                            AND GasStatus = 1 
                            AND IsArrears = 1 
                            AND ResidualMoney < 0 ";
                #endregion
                List<SqlParameter> parList = new List<SqlParameter>();
                parList.Add(new SqlParameter("@CompanyId", ConfigurationHelper.GetAppSettings("CompanyId")));
                return gasMeterRepository.GetAll(sql, parList).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取开通短信欠费通知燃气表失败，错误信息：" + ex.Message);
                throw new Exception("获取开通短信欠费通知燃气表失败", ex);
            }
        }



    }
}
