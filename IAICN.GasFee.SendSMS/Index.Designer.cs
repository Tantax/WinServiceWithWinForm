namespace IAICN.GasFee.SendSMS
{
    partial class Index
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Label_ServiceName = new System.Windows.Forms.Label();
            this.textBox_ServiceName = new System.Windows.Forms.TextBox();
            this.button_StartService = new System.Windows.Forms.Button();
            this.button_StopService = new System.Windows.Forms.Button();
            this.button_PauseService = new System.Windows.Forms.Button();
            this.button_RecoverService = new System.Windows.Forms.Button();
            this.button_CurrentState = new System.Windows.Forms.Button();
            this.button_Quit = new System.Windows.Forms.Button();
            this.label_ServiceState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label_ServiceName
            // 
            this.Label_ServiceName.AutoSize = true;
            this.Label_ServiceName.Location = new System.Drawing.Point(38, 16);
            this.Label_ServiceName.Name = "Label_ServiceName";
            this.Label_ServiceName.Size = new System.Drawing.Size(65, 12);
            this.Label_ServiceName.TabIndex = 0;
            this.Label_ServiceName.Text = "服务名称：";
            // 
            // textBox_ServiceName
            // 
            this.textBox_ServiceName.Location = new System.Drawing.Point(109, 13);
            this.textBox_ServiceName.Name = "textBox_ServiceName";
            this.textBox_ServiceName.ReadOnly = true;
            this.textBox_ServiceName.Size = new System.Drawing.Size(156, 21);
            this.textBox_ServiceName.TabIndex = 1;
            // 
            // button_StartService
            // 
            this.button_StartService.Location = new System.Drawing.Point(40, 59);
            this.button_StartService.Name = "button_StartService";
            this.button_StartService.Size = new System.Drawing.Size(96, 31);
            this.button_StartService.TabIndex = 5;
            this.button_StartService.Text = "启动服务";
            this.button_StartService.UseVisualStyleBackColor = true;
            this.button_StartService.Click += new System.EventHandler(this.button_StartService_Click);
            // 
            // button_StopService
            // 
            this.button_StopService.Location = new System.Drawing.Point(169, 59);
            this.button_StopService.Name = "button_StopService";
            this.button_StopService.Size = new System.Drawing.Size(96, 31);
            this.button_StopService.TabIndex = 6;
            this.button_StopService.Text = "停止服务";
            this.button_StopService.UseVisualStyleBackColor = true;
            this.button_StopService.Click += new System.EventHandler(this.button_StopService_Click);
            // 
            // button_PauseService
            // 
            this.button_PauseService.Location = new System.Drawing.Point(40, 96);
            this.button_PauseService.Name = "button_PauseService";
            this.button_PauseService.Size = new System.Drawing.Size(96, 31);
            this.button_PauseService.TabIndex = 7;
            this.button_PauseService.Text = "暂停服务";
            this.button_PauseService.UseVisualStyleBackColor = true;
            this.button_PauseService.Visible = false;
            this.button_PauseService.Click += new System.EventHandler(this.button_PauseService_Click);
            // 
            // button_RecoverService
            // 
            this.button_RecoverService.Location = new System.Drawing.Point(169, 96);
            this.button_RecoverService.Name = "button_RecoverService";
            this.button_RecoverService.Size = new System.Drawing.Size(96, 31);
            this.button_RecoverService.TabIndex = 8;
            this.button_RecoverService.Text = "恢复服务";
            this.button_RecoverService.UseVisualStyleBackColor = true;
            this.button_RecoverService.Visible = false;
            this.button_RecoverService.Click += new System.EventHandler(this.button_RecoverService_Click);
            // 
            // button_CurrentState
            // 
            this.button_CurrentState.Location = new System.Drawing.Point(40, 171);
            this.button_CurrentState.Name = "button_CurrentState";
            this.button_CurrentState.Size = new System.Drawing.Size(96, 27);
            this.button_CurrentState.TabIndex = 9;
            this.button_CurrentState.Text = "当前服务状态";
            this.button_CurrentState.UseVisualStyleBackColor = true;
            this.button_CurrentState.Click += new System.EventHandler(this.button_CurrentState_Click);
            // 
            // button_Quit
            // 
            this.button_Quit.Location = new System.Drawing.Point(169, 171);
            this.button_Quit.Name = "button_Quit";
            this.button_Quit.Size = new System.Drawing.Size(96, 27);
            this.button_Quit.TabIndex = 10;
            this.button_Quit.Text = "退出管理";
            this.button_Quit.UseVisualStyleBackColor = true;
            this.button_Quit.Click += new System.EventHandler(this.button_Quit_Click);
            // 
            // label_ServiceState
            // 
            this.label_ServiceState.AutoSize = true;
            this.label_ServiceState.Location = new System.Drawing.Point(40, 135);
            this.label_ServiceState.Name = "label_ServiceState";
            this.label_ServiceState.Size = new System.Drawing.Size(77, 12);
            this.label_ServiceState.TabIndex = 11;
            this.label_ServiceState.Text = "服务当前状态";
            // 
            // Index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 237);
            this.Controls.Add(this.label_ServiceState);
            this.Controls.Add(this.button_Quit);
            this.Controls.Add(this.button_CurrentState);
            this.Controls.Add(this.button_RecoverService);
            this.Controls.Add(this.button_PauseService);
            this.Controls.Add(this.button_StopService);
            this.Controls.Add(this.button_StartService);
            this.Controls.Add(this.textBox_ServiceName);
            this.Controls.Add(this.Label_ServiceName);
            this.Name = "Index";
            this.Text = "短信服务管理工具";
            this.Load += new System.EventHandler(this.Index_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_ServiceName;
        private System.Windows.Forms.TextBox textBox_ServiceName;
        private System.Windows.Forms.Button button_StartService;
        private System.Windows.Forms.Button button_StopService;
        private System.Windows.Forms.Button button_PauseService;
        private System.Windows.Forms.Button button_RecoverService;
        private System.Windows.Forms.Button button_CurrentState;
        private System.Windows.Forms.Button button_Quit;
        private System.Windows.Forms.Label label_ServiceState;
    }
}

