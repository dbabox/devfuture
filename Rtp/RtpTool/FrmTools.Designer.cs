using System.Windows.Forms;
namespace RtpTool
{
    partial class FrmTools
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
            if (string.IsNullOrEmpty(textBoxScriptCyc.Text) == false
               && System.IO.File.Exists(textBoxScriptCyc.Text))
            {
                Application.UserAppDataRegistry.SetValue(USER_REG_KEY_LAST_SCRIPT_FILE, textBoxScriptCyc.Text, Microsoft.Win32.RegistryValueKind.String);
            }

            if (string.IsNullOrEmpty(textBoxScriptOnece.Text) == false
           && System.IO.File.Exists(textBoxScriptOnece.Text))
            {
                Application.UserAppDataRegistry.SetValue(USER_REG_KEY_LAST_ENV_FILE, textBoxScriptOnece.Text, Microsoft.Win32.RegistryValueKind.String);
            }

            rf.Close();

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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxScriptOnece = new System.Windows.Forms.TextBox();
            this.btnFileScriptOnece = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxScriptCyc = new System.Windows.Forms.TextBox();
            this.btnFileScriptCyc = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudCycTimes = new System.Windows.Forms.NumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelExe = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCycTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "环境设置脚本";
            // 
            // textBoxScriptOnece
            // 
            this.textBoxScriptOnece.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScriptOnece.Location = new System.Drawing.Point(85, 4);
            this.textBoxScriptOnece.Name = "textBoxScriptOnece";
            this.textBoxScriptOnece.Size = new System.Drawing.Size(508, 21);
            this.textBoxScriptOnece.TabIndex = 1;
            // 
            // btnFileScriptOnece
            // 
            this.btnFileScriptOnece.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileScriptOnece.Location = new System.Drawing.Point(599, 4);
            this.btnFileScriptOnece.Name = "btnFileScriptOnece";
            this.btnFileScriptOnece.Size = new System.Drawing.Size(44, 23);
            this.btnFileScriptOnece.TabIndex = 2;
            this.btnFileScriptOnece.Text = "...";
            this.btnFileScriptOnece.UseVisualStyleBackColor = true;
            this.btnFileScriptOnece.Click += new System.EventHandler(this.btnFileScriptOnece_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "循环执行脚本";
            // 
            // textBoxScriptCyc
            // 
            this.textBoxScriptCyc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScriptCyc.Location = new System.Drawing.Point(85, 36);
            this.textBoxScriptCyc.Name = "textBoxScriptCyc";
            this.textBoxScriptCyc.Size = new System.Drawing.Size(508, 21);
            this.textBoxScriptCyc.TabIndex = 3;
            // 
            // btnFileScriptCyc
            // 
            this.btnFileScriptCyc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileScriptCyc.Location = new System.Drawing.Point(599, 36);
            this.btnFileScriptCyc.Name = "btnFileScriptCyc";
            this.btnFileScriptCyc.Size = new System.Drawing.Size(44, 23);
            this.btnFileScriptCyc.TabIndex = 4;
            this.btnFileScriptCyc.Text = "...";
            this.btnFileScriptCyc.UseVisualStyleBackColor = true;
            this.btnFileScriptCyc.Click += new System.EventHandler(this.btnFileScriptCyc_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "循环速度(毫秒)";
            // 
            // nudInterval
            // 
            this.nudInterval.Location = new System.Drawing.Point(97, 63);
            this.nudInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudInterval.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(48, 21);
            this.nudInterval.TabIndex = 5;
            this.nudInterval.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(4, 95);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(644, 216);
            this.textBoxLog.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(151, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "循环次数";
            // 
            // nudCycTimes
            // 
            this.nudCycTimes.Location = new System.Drawing.Point(210, 63);
            this.nudCycTimes.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudCycTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudCycTimes.Name = "nudCycTimes";
            this.nudCycTimes.Size = new System.Drawing.Size(48, 21);
            this.nudCycTimes.TabIndex = 6;
            this.nudCycTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(344, 61);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始执行";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(431, 61);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 7;
            this.btnPause.Text = "暂停执行";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(518, 62);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "中止执行";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelExe
            // 
            this.labelExe.AutoSize = true;
            this.labelExe.Location = new System.Drawing.Point(274, 67);
            this.labelExe.Name = "labelExe";
            this.labelExe.Size = new System.Drawing.Size(35, 12);
            this.labelExe.TabIndex = 7;
            this.labelExe.Text = "/次数";
            // 
            // FrmTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 317);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.labelExe);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.nudCycTimes);
            this.Controls.Add(this.nudInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFileScriptCyc);
            this.Controls.Add(this.textBoxScriptCyc);
            this.Controls.Add(this.btnFileScriptOnece);
            this.Controls.Add(this.textBoxScriptOnece);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmTools";
            this.Text = "RFID 脚本执行工具";
            this.Load += new System.EventHandler(this.FrmTools_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCycTimes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxScriptOnece;
        private System.Windows.Forms.Button btnFileScriptOnece;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxScriptCyc;
        private System.Windows.Forms.Button btnFileScriptCyc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudCycTimes;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timer1;
        private Label labelExe;
    }
}

