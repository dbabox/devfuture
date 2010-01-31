namespace LiveTV
{
    partial class frmLiveTV
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLiveTV));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPlayMMSServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPause_Continue = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSMServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Player = new AxWMPLib.AxWindowsMediaPlayer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslCommon = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsmiPlayList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Player)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiPlay,
            this.tsmiConfig,
            this.tsmiAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(546, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpen,
            this.tsmiClose,
            this.toolStripSeparator2,
            this.tsmiPlayMMSServer,
            this.tsmiPlayList,
            this.toolStripSeparator3,
            this.tsmiExit});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(77, 20);
            this.tsmiFile.Text = "文件(&File)";
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.Name = "tsmiOpen";
            this.tsmiOpen.Size = new System.Drawing.Size(154, 22);
            this.tsmiOpen.Text = "打开(&Open)";
            this.tsmiOpen.Click += new System.EventHandler(this.tsmiOpen_Click);
            // 
            // tsmiClose
            // 
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(154, 22);
            this.tsmiClose.Text = "关闭(&Close)";
            this.tsmiClose.Click += new System.EventHandler(this.tsmiClose_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(151, 6);
            // 
            // tsmiPlayMMSServer
            // 
            this.tsmiPlayMMSServer.Name = "tsmiPlayMMSServer";
            this.tsmiPlayMMSServer.Size = new System.Drawing.Size(154, 22);
            this.tsmiPlayMMSServer.Text = "播放服务器广播";
            this.tsmiPlayMMSServer.Click += new System.EventHandler(this.tsmiPlayMMSServer_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(151, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(154, 22);
            this.tsmiExit.Text = "退出(&Exit)";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiPlay
            // 
            this.tsmiPlay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPause_Continue,
            this.tsmiStop,
            this.toolStripSeparator1,
            this.tsmiFullScreen});
            this.tsmiPlay.Name = "tsmiPlay";
            this.tsmiPlay.Size = new System.Drawing.Size(77, 20);
            this.tsmiPlay.Text = "播放(&Play)";
            // 
            // tsmiPause_Continue
            // 
            this.tsmiPause_Continue.Name = "tsmiPause_Continue";
            this.tsmiPause_Continue.Size = new System.Drawing.Size(152, 22);
            this.tsmiPause_Continue.Text = "暂停/继续(&C)";
            this.tsmiPause_Continue.Click += new System.EventHandler(this.tsmiPause_Continue_Click);
            // 
            // tsmiStop
            // 
            this.tsmiStop.Name = "tsmiStop";
            this.tsmiStop.Size = new System.Drawing.Size(152, 22);
            this.tsmiStop.Text = "停止(&Stop)";
            this.tsmiStop.Click += new System.EventHandler(this.tsmiStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmiFullScreen
            // 
            this.tsmiFullScreen.Name = "tsmiFullScreen";
            this.tsmiFullScreen.Size = new System.Drawing.Size(152, 22);
            this.tsmiFullScreen.Text = "全屏(&Full)";
            this.tsmiFullScreen.Click += new System.EventHandler(this.tsmiFullScreen_Click);
            // 
            // tsmiConfig
            // 
            this.tsmiConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSMServer});
            this.tsmiConfig.Name = "tsmiConfig";
            this.tsmiConfig.Size = new System.Drawing.Size(89, 20);
            this.tsmiConfig.Text = "设置(&Option)";
            // 
            // tsmiSMServer
            // 
            this.tsmiSMServer.Name = "tsmiSMServer";
            this.tsmiSMServer.Size = new System.Drawing.Size(152, 22);
            this.tsmiSMServer.Text = "媒体发布点(&M)";
            this.tsmiSMServer.Click += new System.EventHandler(this.tsmiMMSServer_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(83, 20);
            this.tsmiAbout.Text = "关于(&About)";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 6000;
            // 
            // player
            // 
            this.Player.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Player.Enabled = true;
            this.Player.Location = new System.Drawing.Point(0, 24);
            this.Player.Name = "player";
            this.Player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("player.OcxState")));
            this.Player.Size = new System.Drawing.Size(546, 374);
            this.Player.TabIndex = 6;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslCommon});
            this.statusStrip1.Location = new System.Drawing.Point(0, 398);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(546, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "Ready";
            // 
            // tsslCommon
            // 
            this.tsslCommon.Name = "tsslCommon";
            this.tsslCommon.Size = new System.Drawing.Size(35, 17);
            this.tsslCommon.Text = "Ready";
            // 
            // tsmiPlayList
            // 
            this.tsmiPlayList.Name = "tsmiPlayList";
            this.tsmiPlayList.Size = new System.Drawing.Size(154, 22);
            this.tsmiPlayList.Text = "服务器媒体列表";
            this.tsmiPlayList.Click += new System.EventHandler(this.tsmiPlayList_Click);
            // 
            // frmLiveTV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(546, 420);
            this.Controls.Add(this.Player);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmLiveTV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "E-Learning Push Player";
            this.Load += new System.EventHandler(this.frmLiveTV_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLiveTV_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Player)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmiSMServer;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiPlay;
        private System.Windows.Forms.ToolStripMenuItem tsmiPause_Continue;
        private System.Windows.Forms.ToolStripMenuItem tsmiStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFullScreen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiPlayMMSServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Timer timer1;
        private AxWMPLib.AxWindowsMediaPlayer Player;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslCommon;
        private System.Windows.Forms.ToolStripMenuItem tsmiPlayList;

    }
}

