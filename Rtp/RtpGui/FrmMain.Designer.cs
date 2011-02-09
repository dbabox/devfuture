using System.Windows.Forms;
namespace Rtp.Gui
{
    partial class FrmMain
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

            if (string.IsNullOrEmpty(scriptFile)==false
                && System.IO.File.Exists(scriptFile))
            {
                Application.UserAppDataRegistry.SetValue(USER_REG_KEY_LAST_SCRIPT_FILE, scriptFile, Microsoft.Win32.RegistryValueKind.String);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnExcute = new System.Windows.Forms.Button();
            this.textBoxCmd = new System.Windows.Forms.TextBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslRequest = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslResponse = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAsciiResponse = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnSaveCmd = new System.Windows.Forms.Button();
            this.btnViewCmdLog = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCommand = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRun = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiViewCmdHis = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScriptHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExcute
            // 
            this.btnExcute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcute.Location = new System.Drawing.Point(84, 405);
            this.btnExcute.Name = "btnExcute";
            this.btnExcute.Size = new System.Drawing.Size(69, 33);
            this.btnExcute.TabIndex = 1;
            this.btnExcute.Text = "执行\r\n(F5)";
            this.btnExcute.UseVisualStyleBackColor = true;
            this.btnExcute.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // textBoxCmd
            // 
            this.textBoxCmd.BackColor = System.Drawing.Color.Teal;
            this.textBoxCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCmd.ForeColor = System.Drawing.Color.Yellow;
            this.textBoxCmd.Location = new System.Drawing.Point(0, 0);
            this.textBoxCmd.Multiline = true;
            this.textBoxCmd.Name = "textBoxCmd";
            this.textBoxCmd.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCmd.Size = new System.Drawing.Size(692, 251);
            this.textBoxCmd.TabIndex = 0;
            // 
            // textBoxLog
            // 
            this.textBoxLog.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLog.Location = new System.Drawing.Point(0, 0);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(692, 119);
            this.textBoxLog.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslRequest,
            this.tsslResponse,
            this.tsslAsciiResponse});
            this.statusStrip1.Location = new System.Drawing.Point(0, 441);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(696, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslRequest
            // 
            this.tsslRequest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslRequest.Name = "tsslRequest";
            this.tsslRequest.Size = new System.Drawing.Size(47, 17);
            this.tsslRequest.Text = "命令...";
            // 
            // tsslResponse
            // 
            this.tsslResponse.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsslResponse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslResponse.Name = "tsslResponse";
            this.tsslResponse.Size = new System.Drawing.Size(533, 17);
            this.tsslResponse.Spring = true;
            this.tsslResponse.Text = "响应:....";
            // 
            // tsslAsciiResponse
            // 
            this.tsslAsciiResponse.Name = "tsslAsciiResponse";
            this.tsslAsciiResponse.Size = new System.Drawing.Size(101, 17);
            this.tsslAsciiResponse.Text = "ASCII格式响应...";
            this.tsslAsciiResponse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiCommand,
            this.tsmiHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(696, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxCmd);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxLog);
            this.splitContainer1.Size = new System.Drawing.Size(692, 374);
            this.splitContainer1.SplitterDistance = 251;
            this.splitContainer1.TabIndex = 7;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearLog.Location = new System.Drawing.Point(312, 405);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(69, 33);
            this.btnClearLog.TabIndex = 8;
            this.btnClearLog.Text = "清除日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // btnSaveCmd
            // 
            this.btnSaveCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveCmd.Location = new System.Drawing.Point(160, 405);
            this.btnSaveCmd.Name = "btnSaveCmd";
            this.btnSaveCmd.Size = new System.Drawing.Size(69, 33);
            this.btnSaveCmd.TabIndex = 8;
            this.btnSaveCmd.Text = "保存脚本\r\n(F4)";
            this.btnSaveCmd.UseVisualStyleBackColor = true;
            this.btnSaveCmd.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // btnViewCmdLog
            // 
            this.btnViewCmdLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewCmdLog.Location = new System.Drawing.Point(387, 405);
            this.btnViewCmdLog.Name = "btnViewCmdLog";
            this.btnViewCmdLog.Size = new System.Drawing.Size(69, 33);
            this.btnViewCmdLog.TabIndex = 8;
            this.btnViewCmdLog.Text = "COS命令\r\n历史";
            this.btnViewCmdLog.UseVisualStyleBackColor = true;
            this.btnViewCmdLog.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenFile.Location = new System.Drawing.Point(9, 405);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(69, 33);
            this.btnOpenFile.TabIndex = 8;
            this.btnOpenFile.Text = "打开文件\r\n(&Open)";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveAs.Location = new System.Drawing.Point(235, 405);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(69, 33);
            this.btnSaveAs.TabIndex = 8;
            this.btnSaveAs.Text = "另存脚本\r\n";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpen,
            this.tsmiSave,
            this.toolStripSeparator1,
            this.tsmiSaveAs});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(77, 20);
            this.tsmiFile.Text = "文件(&File)";
            // 
            // tsmiCommand
            // 
            this.tsmiCommand.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRun,
            this.tsmiClearLog,
            this.tsmiViewCmdHis});
            this.tsmiCommand.Name = "tsmiCommand";
            this.tsmiCommand.Size = new System.Drawing.Size(95, 20);
            this.tsmiCommand.Text = "命令(&Command)";
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.Name = "tsmiOpen";
            this.tsmiOpen.Size = new System.Drawing.Size(152, 22);
            this.tsmiOpen.Text = "打开(&Open)";
            this.tsmiOpen.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.Size = new System.Drawing.Size(152, 22);
            this.tsmiSave.Text = "保存脚本(F4)";
            this.tsmiSave.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            this.tsmiSaveAs.Size = new System.Drawing.Size(152, 22);
            this.tsmiSaveAs.Text = "另存为...";
            this.tsmiSaveAs.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiRun
            // 
            this.tsmiRun.Name = "tsmiRun";
            this.tsmiRun.Size = new System.Drawing.Size(152, 22);
            this.tsmiRun.Text = "执行(F5)";
            this.tsmiRun.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiClearLog
            // 
            this.tsmiClearLog.Name = "tsmiClearLog";
            this.tsmiClearLog.Size = new System.Drawing.Size(152, 22);
            this.tsmiClearLog.Text = "清除日志";
            this.tsmiClearLog.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiViewCmdHis
            // 
            this.tsmiViewCmdHis.Name = "tsmiViewCmdHis";
            this.tsmiViewCmdHis.Size = new System.Drawing.Size(152, 22);
            this.tsmiViewCmdHis.Text = "COS命令历史";
            this.tsmiViewCmdHis.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiScriptHelp,
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(77, 20);
            this.tsmiHelp.Text = "帮助(&Help)";
            // 
            // tsmiScriptHelp
            // 
            this.tsmiScriptHelp.Name = "tsmiScriptHelp";
            this.tsmiScriptHelp.Size = new System.Drawing.Size(152, 22);
            this.tsmiScriptHelp.Text = "脚本语法";
            this.tsmiScriptHelp.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(152, 22);
            this.tsmiAbout.Text = "关于";
            this.tsmiAbout.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // FrmMain
            // 
            this.ClientSize = new System.Drawing.Size(696, 463);
            this.Controls.Add(this.btnViewCmdLog);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.btnSaveCmd);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnExcute);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "RFID 脚本环境";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExcute;
        private System.Windows.Forms.TextBox textBoxCmd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.ToolStripStatusLabel tsslRequest;
        private System.Windows.Forms.ToolStripStatusLabel tsslResponse;
        private System.Windows.Forms.ToolStripStatusLabel tsslAsciiResponse;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnSaveCmd;
        private System.Windows.Forms.Button btnViewCmdLog;
        private System.Windows.Forms.Button btnOpenFile;
        private Button btnSaveAs;
        private ToolStripMenuItem tsmiFile;
        private ToolStripMenuItem tsmiOpen;
        private ToolStripMenuItem tsmiSave;
        private ToolStripMenuItem tsmiSaveAs;
        private ToolStripMenuItem tsmiCommand;
        private ToolStripMenuItem tsmiRun;
        private ToolStripMenuItem tsmiClearLog;
        private ToolStripMenuItem tsmiViewCmdHis;
        private ToolStripMenuItem tsmiHelp;
        private ToolStripMenuItem tsmiScriptHelp;
        private ToolStripMenuItem tsmiAbout;
        private ToolStripSeparator toolStripSeparator1;
    }
}

