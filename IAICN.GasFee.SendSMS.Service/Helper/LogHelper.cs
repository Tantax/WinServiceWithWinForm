using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAICN.GasFee.SendSMS.Service
{
    /// <summary>
    /// Log4net使用帮助类
    /// </summary>
    public class LogHelper
    {

        /// <summary>
        /// 选择<logger name="loginfo">的配置 
        /// </summary>
        public static readonly ILog loginfo = LogManager.GetLogger("loginfo");

        /// <summary>
        /// 选择<logger name="logerror">的配置 
        /// </summary>
        public static readonly ILog logerror = LogManager.GetLogger("logerror");

        /// <summary>
        /// 配置初始化（一定要调用后才能使用日志）
        /// </summary>
        public static void SetConfig()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// 配置初始化
        /// </summary>
        /// <param name="configFile">配置文件</param>
        public static void SetConfig(FileInfo configFile)
        {
            XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 写入信息日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        /// <summary>
        /// 写入异常日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);
            }
        }
    }
}
