namespace LiveTV
{
    partial class frmMMSServer
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
            this.textBoxPingLog = new System.Windows.Forms.TextBox();
            this.btnTestServer = new System.Windows.Forms.Button();
            this.textBoxBase_Url = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxVieo_Url = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboBoxProto = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelSampleLink = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxPingLog
            // 
            this.textBoxPingLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPingLog.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxPingLog.ForeColor = System.Drawing.SystemColors.Info;
            this.textBoxPingLog.Location = new System.Drawing.Point(2, 185);
            this.textBoxPingLog.Multiline = true;
            this.textBoxPingLog.Name = "textBoxPingLog";
            this.textBoxPingLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPingLog.Size = new System.Drawing.Size(447, 184);
            this.textBoxPingLog.TabIndex = 2;
            // 
            // btnTestServer
            // 
            this.btnTestServer.Location = new System.Drawing.Point(2, 156);
            this.btnTestServer.Name = "btnTestServer";
            this.btnTestServer.Size = new System.Drawing.Size(144, 23);
            this.btnTestServer.TabIndex = 3;
            this.btnTestServer.Text = "测试服务器是否联通";
            this.btnTestServer.UseVisualStyleBackColor = true;
            this.btnTestServer.Click += new System.EventHandler(this.btnTestServer_Click);
            // 
            // textBoxBase_Url
            // 
            this.textBoxBase_Url.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxBase_Url.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxBase_Url.Location = new System.Drawing.Point(93, 12);
            this.textBoxBase_Url.Name = "textBoxBase_Url";
            this.textBoxBase_Url.Size = new System.Drawing.Size(345, 23);
            this.textBoxBase_Url.TabIndex = 7;
            this.textBoxBase_Url.Text = "192.168.0.11";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "服务地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "广播发布点";
            // 
            // textBoxVieo_Url
            // 
            this.textBoxVieo_Url.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVieo_Url.Location = new System.Drawing.Point(93, 63);
            this.textBoxVieo_Url.Name = "textBoxVieo_Url";
            this.textBoxVieo_Url.Size = new System.Drawing.Size(345, 23);
            this.textBoxVieo_Url.TabIndex = 10;
            this.textBoxVieo_Url.Text = "/LiveTV/Show.asf";
            this.textBoxVieo_Url.Leave += new System.EventHandler(this.textBoxVieo_Url_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "例如：192.168.0.1/LiveTV/Show.asf";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(91, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(347, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "请输入服务器名称或IP地址,如果是虚拟目录，请输入完整虚拟目录地址。eg: 192.168.0.1:8080/pmps";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(363, 117);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(363, 156);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // comboBoxProto
            // 
            this.comboBoxProto.FormattingEnabled = true;
            this.comboBoxProto.Items.AddRange(new object[] {
            "MMS",
            "RTSP",
            "HTTP",
            "FILE"});
            this.comboBoxProto.Location = new System.Drawing.Point(93, 114);
            this.comboBoxProto.Name = "comboBoxProto";
            this.comboBoxProto.Size = new System.Drawing.Size(121, 20);
            this.comboBoxProto.TabIndex = 12;
            this.comboBoxProto.SelectedIndexChanged += new System.EventHandler(this.comboBoxProto_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "服务器协议";
            // 
            // labelSampleLink
            // 
            this.labelSampleLink.AutoSize = true;
            this.labelSampleLink.Location = new System.Drawing.Point(91, 137);
            this.labelSampleLink.Name = "labelSampleLink";
            this.labelSampleLink.Size = new System.Drawing.Size(203, 12);
            this.labelSampleLink.TabIndex = 9;
            this.labelSampleLink.Text = "例如：192.168.0.1/LiveTV/Show.asf";
            // 
            // frmMMSServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(450, 370);
            this.Controls.Add(this.comboBoxProto);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.textBoxVieo_Url);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelSampleLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxBase_Url);
            this.Controls.Add(this.btnTestServer);
            this.Controls.Add(this.textBoxPingLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMMSServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "默认媒体发布点";
            this.Load += new System.EventHandler(this.frmMMSServer_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMMSServer_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPingLog;
        private System.Windows.Forms.Button btnTestServer;
        private System.Windows.Forms.TextBox textBoxBase_Url;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxVieo_Url;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboBoxProto;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSampleLink;
    }
}