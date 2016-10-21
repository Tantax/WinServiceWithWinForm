using IAICN.GasFee.SendSMS.Service;
using System;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Windows.Forms;

namespace IAICN.GasFee.SendSMS
{
    public partial class Index : Form
    {
        private readonly string _svcName = "IAI_SendSMSService";
        private readonly string _svcAppName = "IAICN.GasFee.SendSMS.Service.exe";

        public Index()
        {
            InitializeComponent();
            LogHelper.SetConfig();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            this.textBox_ServiceName.Text = _svcName;

            if (!ServiceIsExisted(_svcName))
            {
                DialogResult result = MessageBox.Show(this.textBox_ServiceName.Text + "服务不存在，是否马上安装服务？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (!InstallService())
                    {
                        Quit();
                    }
                }
                else
                {
                    Quit();
                }
            }
            else
            {
                this.label_ServiceState.Text = "当前服务状态为:" + GetStateForService(this.textBox_ServiceName.Text);
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_StartService_Click(object sender, EventArgs e)
        {
            RunService(this.textBox_ServiceName.Text);           
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_StopService_Click(object sender, EventArgs e)
        {
            KillService(this.textBox_ServiceName.Text);
        }
       
        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_PauseService_Click(object sender, EventArgs e)
        {
            PauseService(this.textBox_ServiceName.Text);
        }

        /// <summary>
        /// 恢复服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RecoverService_Click(object sender, EventArgs e)
        {
            ResumeService(this.textBox_ServiceName.Text);
        }

        /// <summary>
        /// 当前服务状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CurrentState_Click(object sender, EventArgs e)
        {
            this.label_ServiceState.Text = "当前服务状态为:" + GetStateForService(this.textBox_ServiceName.Text);
        }

        /// <summary>
        /// 退出管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Quit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否卸载服务？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                KillService(_svcName);
                UninstallService();
            }
            Quit();
        }       

        #region Methods

        private bool InstallService()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { _svcAppName });
                MessageBox.Show("服务安装成功！");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private void RunService(string servername)
        {
            ServiceController sc = new ServiceController(servername);
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.Stopped:
                    sc.Start();
                    break;
                default: break;
            }
            sc.WaitForStatus(ServiceControllerStatus.Running);
            st = sc.Status;//再次获取服务状态  
            if (st == ServiceControllerStatus.Running)
            {
                this.label_ServiceState.Text = "服务  " + sc.ServiceName + "  已经启动！";
            }
        }
        private void KillService(string servername)
        {
            ServiceController sc = new ServiceController(servername);
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.ContinuePending:
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    break;
            }
            st = sc.Status;//再次获取服务状态  
            if (st == ServiceControllerStatus.Stopped)
            {
                this.label_ServiceState.Text = "服务　" + sc.ServiceName + "  已经停止！";
            }
        }
        private void PauseService(string servername)
        {
            ServiceController sc = new ServiceController(servername);
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                    sc.Pause();
                    sc.WaitForStatus(ServiceControllerStatus.Paused);
                    break;
            }
            st = sc.Status;//再次获取服务状态  
            if (st == ServiceControllerStatus.Paused)
            {
                this.label_ServiceState.Text = "服务　" + sc.ServiceName + "  已经暂停！";
            }
        }
        private void ResumeService(string servername)
        {
            ServiceController sc = new ServiceController(servername);
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.PausePending:
                    sc.Continue();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    break;
            }
            st = sc.Status;//再次获取服务状态  
            if (st == ServiceControllerStatus.Running)
            {
                this.label_ServiceState.Text = "服务　" + sc.ServiceName + "  已经继续！";
            }
        }
        private void Quit()
        {
            this.Close();
            Application.Exit();
        }
        private bool UninstallService()
        {
            try
            {
                if (ServiceIsExisted(_svcName))
                {
                    TransactedInstaller transactedInstaller = new TransactedInstaller();
                    AssemblyInstaller assemblyInstaller = new AssemblyInstaller(_svcAppName, null);
                    transactedInstaller.Installers.Add(assemblyInstaller);
                    transactedInstaller.Uninstall(null);
                    MessageBox.Show("卸载服务成功！");
                    return true;
                }
                else
                {
                    MessageBox.Show("服务不存在");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private string GetStateForService(string servername)
        {
            string serviceState = string.Empty;
            ServiceController sc = new ServiceController(servername);
            ServiceControllerStatus st = sc.Status;
            switch (st)
            {
                case ServiceControllerStatus.PausePending:
                case ServiceControllerStatus.Paused:
                    serviceState = "已暂停";
                    break;
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.Running:
                    serviceState = "正在运行";
                    break;
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.Stopped:
                    serviceState = "未运行";
                    break;
                default:
                    serviceState = "请重试";
                    break;
            }
            return serviceState;
        }

        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <param name="svcName"></param>
        /// <returns></returns>
        private bool ServiceIsExisted(string svcName)
        {
            ServiceController[] services = ServiceController.GetServices();
            for (int i = 0; i < services.Length; i++)
            {
                if (services[i].ServiceName.Equals(this.textBox_ServiceName.Text))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion


        
        
    }
}
