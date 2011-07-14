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
using System.Diagnostics;
using Rtp.Driver;
using System.IO;

namespace Rtp.Gui
{
    public partial class FrmMain : Form
    {
        private IList<string> cmdHistory = new List<string>();

        private TextBoxTraceListener textBoxTraceListener;        
        private RfidBase rf =null;
        private CommandContext ctx = null;
        private RtpCore rtp = null;
        
        private const string USER_REG_KEY_LAST_SCRIPT_FILE = "LastScriptFile";
        private string scriptFile;

        private string readerType;

        public FrmMain(string readerType_,bool enableDebug_)
        {
            InitializeComponent();
            readerType = readerType_;
            this.Text = String.Format("RFID �ű����� V{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            if (enableDebug_)
            {
                textBoxTraceListener = new TextBoxTraceListener(textBoxLog);
                System.Diagnostics.Trace.Listeners.Add(textBoxTraceListener);
            }

            #region CopyRight
            textBoxLog.AppendText(String.Format("RFID Test Platform:Release {0} Production on {1}{2}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
                DateTime.Now.ToLocalTime(), Environment.NewLine));
            textBoxLog.AppendText("Copyright (c) 1999, 2011, SIASUN.  All rights reserved.");
            textBoxLog.AppendText(Environment.NewLine);
            textBoxLog.AppendText(String.Format("-------------------------------------------------------{0}", Environment.NewLine));
            #endregion                                    

            scriptFile = Application.UserAppDataRegistry.GetValue(USER_REG_KEY_LAST_SCRIPT_FILE) as string;
            if (string.IsNullOrEmpty(scriptFile)) scriptFile = "NewScript.his";
            #region RF ������ʼ��
            if (readerType == "D8U")
            {
                rf = new RfidD8U();
            }
            else if (readerType == "T10N")
            {
                RfidT10N rfT10N = new RfidT10N();                
                rfT10N.SamBaudrate = 4;
                rfT10N.Volt = 3;
                rf=rfT10N;
            }
            else if (readerType == "D6U")
            {
                rf = new RfidT6U();
            }
            else
            {
                throw new ArgumentException("��Ч�Ķ���������");
            }          
            
            rf.CpuRequest += new EventHandler<Rtp.Driver.CardIO.CpuRequestEventArgs>(rf_CpuRequest);
            rf.CpuResponse += new EventHandler<Rtp.Driver.CardIO.CpuResponseEventArgs>(rf_CpuResponse);
            rf.SamRequest += new EventHandler<Rtp.Driver.CardIO.SamRequestEventArgs>(rf_SamRequest);
            rf.SamResponse += new EventHandler<Rtp.Driver.CardIO.SamResponseEventArgs>(rf_SamResponse);
            #endregion
            ctx = new CommandContext(rf);
            ctx.RecvCtxMsg = new ReceiveContxtMessage(DisplayContextMessage);
            rtp = new RtpCore(ctx);
            

        }

        void DisplayContextMessage(string message)
        {
            textBoxLog.AppendText(message);
            textBoxLog.AppendText(Environment.NewLine);
        }

        void rf_SamResponse(object sender, Rtp.Driver.CardIO.SamResponseEventArgs e)
        {
            textBoxLog.AppendText(String.Format("{2}>>{0}|{1}{3}", e.ResponseString, rtp.CosIO.GetDescription(e.Sw), ctx.CmdTarget,Environment.NewLine));
            tsslResponse.Text = e.ResponseString;
            tsslAsciiResponse.Text = Utility.ByteArrayToAsciiString(ctx.rlen, ctx.rbuff);
            cmdHistory.Add(String.Format("{2}>> {0}\t[{1}]", e.ResponseString,rtp.CosIO.GetDescription(e.Sw) , ctx.CmdTarget));
            if (e.Sw != 0x9000)
            {
                tsslResponse.ForeColor = Color.Red;
            }
            else
            {
                tsslResponse.ForeColor = Color.Black;
            }
        }

        void rf_SamRequest(object sender, Rtp.Driver.CardIO.SamRequestEventArgs e)
        {
            textBoxLog.AppendText(String.Format("{2}<<{0}|{1}{3}", e.Cmdstr, rtp.CosIO.GetDescription(e.Cmd), ctx.CmdTarget,Environment.NewLine));
            tsslRequest.Text = e.Cmdstr;
            cmdHistory.Add(String.Format("{2}<< {0}\t[{1}]", e.Cmdstr, rtp.CosIO.GetDescription(e.Cmd), ctx.CmdTarget));         
        }

        void rf_CpuResponse(object sender, Rtp.Driver.CardIO.CpuResponseEventArgs e)
        {
            textBoxLog.AppendText(String.Format("{2}>>{0}|{1}{3}", e.ResponseString, rtp.CosIO.GetDescription(e.Sw), ctx.CmdTarget,Environment.NewLine));
            tsslResponse.Text = e.ResponseString;
            tsslAsciiResponse.Text = Utility.ByteArrayToAsciiString(ctx.rlen, ctx.rbuff);
            cmdHistory.Add(String.Format("{2}>> {0}\t[{1}]", e.ResponseString, rtp.CosIO.GetDescription(e.Sw), ctx.CmdTarget));
            if (e.Sw != 0x9000)
            {
                tsslResponse.ForeColor = Color.Red;
            }
            else
            {
                tsslResponse.ForeColor = Color.Black;
            }
        }

        void rf_CpuRequest(object sender, Rtp.Driver.CardIO.CpuRequestEventArgs e)
        {
            textBoxLog.AppendText(String.Format("{2}<<{0}|{1}{3}", e.Cmdstr, rtp.CosIO.GetDescription(e.Cmd), ctx.CmdTarget,Environment.NewLine));
            tsslRequest.Text = e.Cmdstr;
            cmdHistory.Add(String.Format("{2}<< {0}\t[{1}]", e.Cmdstr, rtp.CosIO.GetDescription(e.Cmd), ctx.CmdTarget));           
        }

        private void btnExcute_Click(object sender, EventArgs e)
        {
            #region �˵��Ͱ�ť��Ӧ
            if (sender.Equals(btnExcute)||sender.Equals(tsmiRun))
            {
                DateTime dtstart=DateTime.Now;
                textBoxLog.AppendText(String.Format("��ʼִ�нű�:{0}{1}.", dtstart,Environment.NewLine));
                foreach (string l in textBoxCmd.Lines)
                {
                    if (l.Length > 0)
                    {
                        if (!rtp.CommandExcuter(l.Trim().ToUpper()))
                            break;
                    }
                }
                TimeSpan ts=DateTime.Now-dtstart;
                textBoxLog.AppendText(String.Format("�ű�ִ�����:{0},����ʱ:{1}����.{2}", dtstart, ts.TotalMilliseconds, Environment.NewLine));
                return;
            }
            if (sender.Equals(btnClearLog) || sender.Equals(tsmiClearLog))
            {
                textBoxLog.Clear();
                return;
            }
            if (sender.Equals(btnSaveCmd) || sender.Equals(tsmiSave))
            {
                FileInfo fi = new FileInfo(scriptFile);
                if (fi.Exists && fi.Name==scriptFile)
                {
                    if (MessageBox.Show( String.Format("{0}�ļ��Ѵ��ڣ���ϣ�����Ǵ��ļ���", fi.FullName),
                        "��ʾ",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }
                using (System.IO.StreamWriter sw = new StreamWriter(new System.IO.FileStream(scriptFile, FileMode.Create, FileAccess.Write)))
                {
                    sw.Write(textBoxCmd.Text);
                    sw.Close();
                    tsslRequest.Text = DateTime.Now.ToString();
                    scriptFile = fi.FullName;
                    tsslResponse.Text = "�ű��ѱ��浽"+scriptFile;
                }
                return;
            }
            if (sender.Equals(btnViewCmdLog) || sender.Equals(tsmiViewCmdHis))
            {
                using (FrmTextView frm = new FrmTextView(cmdHistory))
                {
                    frm.ShowDialog(this);
                }
                return;
            }
            if (sender.Equals(btnOpenFile) || sender.Equals(tsmiOpen))
            {
                using (OpenFileDialog sfd = new OpenFileDialog())
                {                   
                    sfd.Filter = "txt;isu;rfid;log;his;rfs|*.txt;*.isu;*.rfid;*.his;*.rfs|All files (*.*)|*.*";
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                     
                        textBoxCmd.Clear();
                        scriptFile = sfd.FileName;
                        using (System.IO.StreamReader sr = new StreamReader(scriptFile, Encoding.UTF8))
                        {
                            textBoxCmd.AppendText(sr.ReadToEnd());
                            sr.Close();
                            tsslResponse.Text = String.Format("���ؽű�{0}���.", scriptFile);
                        }
                    }
                }
                return;
            }
            if (sender.Equals(btnSaveAs) || sender.Equals(tsmiSaveAs))
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "RFID�ű��ļ�(*.rfs)|*.rfs|�ű��ļ�(txt;rfs;rfid;log;his)|*.txt;*.rfs;*.rfid;*.log;*.his|All files (*.*)|*.*";
          
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {                 
                        
                        scriptFile = sfd.FileName;
                        using (System.IO.StreamWriter sw = new StreamWriter(new System.IO.FileStream(scriptFile, FileMode.Create, FileAccess.Write)))
                        {
                            sw.Write(textBoxCmd.Text);
                            sw.Close();
                            tsslRequest.Text = DateTime.Now.ToString();
                            tsslResponse.Text = "�ű��ѱ��浽" + scriptFile;
                        }
                    }
                }
                return;
            }
            #endregion

            #region �˵���Ӧ
            if (sender.Equals(tsmiSaveLog))
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.DefaultExt = ".log";
                    sfd.Filter = "��־�ļ�|*.log";
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        string path = sfd.FileName;
                        using (System.IO.StreamWriter sw = new StreamWriter(path))
                        {
                            sw.Write(textBoxLog.Text);
                        }
                    }
                }
                return;
            }
            if (sender.Equals(tsmiScriptHelp))
            {
                if (System.IO.File.Exists("help.rtf"))
                {
                    //��ʾ�ű��﷨����
                    System.Diagnostics.Process.Start("help.rtf");
                }
                else
                {
                    MessageBox.Show("�����ļ�ȱʧ����ο���http://code.google.com/p/devfuture ��վ�ĵ�.", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            if (sender.Equals(tsmiAbout))
            {
                MessageBox.Show(String.Format("RFID �ű��������� V{0}.\r\n\r\n ��Ȩ����(c) SIASUN 2000-2011",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version), 
                    "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (sender.Equals(tsmiNew))
            {
                scriptFile = "NewScript.his";
                textBoxCmd.Clear();
                tsslResponse.Text = String.Format("�½��˽ű�{0}",scriptFile);
                return;
            }



            #endregion

        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnExcute_Click(btnExcute, e);
                e.Handled = true;
                return;
            }            
            if (e.KeyCode == Keys.F4)
            {
                btnExcute_Click(btnSaveCmd, e);
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.F8)
            {
                if (textBoxCmd.SelectedText.Length > 0)
                {
                    DateTime dtstart = DateTime.Now;
                    if (textBoxCmd.SelectedText.Contains(Environment.NewLine))
                    {
                        string[] lines = textBoxCmd.SelectedText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in lines)
                        {
                            if (!rtp.CommandExcuter(s))
                            {
                                Trace.TraceError("Command {0} excute failed!", s);
                                break;
                            }
                        }
                    }
                    else
                    {
                        rtp.CommandExcuter(textBoxCmd.SelectedText.Trim().ToUpper());
                    }
                    TimeSpan ts = DateTime.Now - dtstart;
                    Trace.TraceInformation("�ű�ִ�����:{0},����ʱ:{1}����.", dtstart, ts.TotalMilliseconds);
                }
            }

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {      

            if(System.IO.File.Exists(scriptFile))
            {
                using (System.IO.StreamReader sr = new StreamReader(scriptFile,Encoding.UTF8))
                {
                    textBoxCmd.AppendText(sr.ReadToEnd());
                    sr.Close();
                    tsslResponse.Text = String.Format("�Զ����ؽű��ļ�{0}", scriptFile);
                }
            }

            if (rf != null)
            {
                int rc = rf.Open();
                if (rc > 0)
                {
                    tsslRequest.Text = String.Format("������������{0}", rf.DeviceVersion());
                }
                else
                {
                    tsslRequest.Text = "�򿪶�����ʧ�ܣ�";
                }
            }
            rtp.CommandExcuter("HELP");//��ʾ������Ϣ
            
        }

        

         

    }
}