using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Siasun.Gui.Components;
using Rtp.Driver.RfidReader;
using Rtp.Driver.Command;
using Rtp.Driver;

namespace RtpTool
{
    public partial class FrmTools : Form
    {
        //private TextBoxTraceListener textBoxTraceListener;
        private RfidBase rf = null;
        private CommandContext ctx = null;
        private RtpCore rtp = null;
        private const string USER_REG_KEY_LAST_SCRIPT_FILE = "LastScriptFile";
        private const string USER_REG_KEY_LAST_ENV_FILE = "EnvFile";
        private string cycCommand;
        private int exed = 0;

        public FrmTools()
        {
            InitializeComponent();

            this.Text = String.Format("RFID 脚本执行工具 V{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            //textBoxTraceListener = new TextBoxTraceListener(textBoxLog);

            //System.Diagnostics.Trace.Listeners.Add(textBoxTraceListener);

            #region CopyRight
            textBoxLog.AppendText(String.Format("RFID Test Platform:Release {0} Production on {1}{2}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
                DateTime.Now.ToLocalTime(), Environment.NewLine));
            textBoxLog.AppendText("Copyright (c) 1999, 2011, SIASUN.  All rights reserved.");
            textBoxLog.AppendText(String.Format("{0}-------------------------------------------------------{0}", Environment.NewLine));
            #endregion


            rf = new RfidD8U();
            
            ctx = new CommandContext(rf);
            ctx.RecvCtxMsg = new ReceiveContxtMessage(DisplayContextMessage);

            rtp = new RtpCore(ctx);
            rtp.CommandExcuter("HELP");

            textBoxScriptCyc.Text = Application.UserAppDataRegistry.GetValue(USER_REG_KEY_LAST_SCRIPT_FILE) as string;
            textBoxScriptOnece.Text = Application.UserAppDataRegistry.GetValue(USER_REG_KEY_LAST_ENV_FILE) as string;

            btnStart.Enabled = true;
            btnPause.Enabled = false;
            btnStop.Enabled = false;
            labelExe.Text = String.Empty;
        }

        void DisplayContextMessage(string message)
        {
            textBoxLog.AppendText(message);
            textBoxLog.AppendText(Environment.NewLine);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(textBoxScriptOnece.Text) && System.IO.File.Exists(textBoxScriptCyc.Text))
            {
                //开始执行
                //执行一次脚本文件
                rtp.CommandExcuter(String.Format("!{0}", textBoxScriptOnece.Text));
                cycCommand = String.Format("!{0}", textBoxScriptCyc.Text);
                exed = 0;
                timer1.Interval = (int)nudInterval.Value;
                timer1.Start();
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;
                
            }
            else
            {
                MessageBox.Show("指定的文件不存在，请检查！");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            rtp.CommandExcuter(cycCommand);
            labelExe.Text = String.Format("/已执行{0}次", ++exed);
            if (nudCycTimes.Value > -1 && (exed) >= nudCycTimes.Value)
            {
                btnStop_Click(sender, e);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnStart.Enabled = true;
            btnPause.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnStart.Enabled = true;
            btnPause.Enabled = false;
            btnStop.Enabled = false;
            labelExe.Text = String.Empty;
            exed = 0;
        }

        private void btnFileScriptOnece_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog sfd = new OpenFileDialog())
            {
                sfd.Filter = "txt;isu;rfid;log;his;rfs|*.txt;*.isu;*.rfid;*.his;*.rfs|All files (*.*)|*.*";
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxScriptOnece.Text = sfd.FileName;                   
                }
            }
        }

        private void btnFileScriptCyc_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog sfd = new OpenFileDialog())
            {
                sfd.Filter = "txt;isu;rfid;log;his|*.txt;*.isu;*.rfid;*.his|All files (*.*)|*.*";
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxScriptCyc.Text = sfd.FileName;
                }
            }
        }

        private void FrmTools_Load(object sender, EventArgs e)
        {
            rf.Open();
        }
    }
}