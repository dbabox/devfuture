using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace LiveTV
{
    public partial class frmMMSServer : Form
    {
        #region  网络测试
        System.Net.NetworkInformation.Ping ping;
      
        int pingCount = 1;
        int maxpingCount = 4; //ping多少次
        List<long> rpt = new List<long>();
        //32 bytes
        byte[] buffer = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        // Wait 5 seconds for a reply.
        int timeout = 5000;
        // Set options for transmission:
        // The data can go through 64 gateways or routers
        // before it is destroyed, and the data packet
        // cannot be fragmented.
        PingOptions options = new PingOptions(64, true);
        #endregion

        private MMSServerCFG cfg;

        public frmMMSServer()
        {
            InitializeComponent();
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
            cfg = new MMSServerCFG();
            ping = new System.Net.NetworkInformation.Ping();
            ping.PingCompleted += new System.Net.NetworkInformation.PingCompletedEventHandler(ping_PingCompleted);
            
           
        }
        void ping_PingCompleted(object sender, System.Net.NetworkInformation.PingCompletedEventArgs e)
        {
            if (e.Cancelled && ping!=null)
            {
                textBoxPingLog.AppendText("Ping Cancled.");
                textBoxPingLog.AppendText(Environment.NewLine);
            }
            else if (!e.Cancelled && ping!=null && e.Error != null)
            {
                textBoxPingLog.AppendText("Error:");
                textBoxPingLog.AppendText(e.Error.ToString());
                textBoxPingLog.AppendText(Environment.NewLine);

            }           
            else if(!e.Cancelled && ping!=null)
            {
                rpt.Add(e.Reply.RoundtripTime);

                textBoxPingLog.AppendText(e.UserState.ToString());
                textBoxPingLog.AppendText(":");
                DisplayReply(e.Reply);
                if (pingCount <= maxpingCount)
                {
                    ping.SendAsync(textBoxBase_Url.Text, timeout, buffer, options, (object)pingCount++);
                }
                else
                {

                    textBoxPingLog.AppendText("\r\n\r\nPing测试完成" + Environment.NewLine);
                    rpt.Sort();
                    textBoxPingLog.AppendText("统计信息：" + Environment.NewLine);
                    textBoxPingLog.AppendText(String.Format("min RoundtripTime:{0}ms,  max RoundtripTime:{1}ms\r\n", rpt[0], rpt[pingCount - 2]));
                }
            }

        }

        public void DisplayReply(System.Net.NetworkInformation.PingReply reply)
        {
            if (reply == null)
                return;

            textBoxPingLog.AppendText(String.Format("ping status: {0}\r\n", reply.Status));

            if (reply.Status == IPStatus.Success)
            {
                textBoxPingLog.AppendText(String.Format("Reply from {0}:", reply.Address.ToString()));
                textBoxPingLog.AppendText(String.Format("bytes= {0}  ", reply.Buffer.Length));
                textBoxPingLog.AppendText(String.Format("time={0}ms  ", reply.RoundtripTime));
                textBoxPingLog.AppendText(String.Format("TTL={0}  ", reply.Options.Ttl));
                textBoxPingLog.AppendText(String.Format("Don't fragment= {0}\r\n", reply.Options.DontFragment));

            }

        }

        private void btnTestServer_Click(object sender, EventArgs e)
        {
            textBoxPingLog.Clear();
            //检查是否URL
            UriHostNameType uhnt= System.Uri.CheckHostName(textBoxBase_Url.Text);
            if (uhnt != UriHostNameType.Unknown)
            {
                pingCount = 1;
                rpt.Clear();
                ping.SendAsync(textBoxBase_Url.Text, timeout, buffer, options, (object)pingCount++);
            }
            else
            {
                MessageBox.Show("IP地址错误！");
                textBoxBase_Url.Focus();
            }
          
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            cfg.Base_Url = textBoxBase_Url.Text;
            cfg.Video_Proto = comboBoxProto.SelectedItem.ToString();
            cfg.Video_Url = textBoxVieo_Url.Text;


            cfg.SaveServerURLCfg();
            MessageBox.Show(String.Format("服务器地址被设置为:\r\n{0}", cfg.Media_Url), 
                "服务器", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void frmMMSServer_Load(object sender, EventArgs e)
        {
            cfg.LoadServerURLFromCfg();
            textBoxPingLog.Clear();
            textBoxBase_Url.Text = cfg.Base_Url;
            textBoxVieo_Url.Text = cfg.Video_Url;
            if (!String.IsNullOrEmpty(cfg.Video_Proto))
            {
                comboBoxProto.SelectedItem = cfg.Video_Proto;
            }
        }

        /// <summary>
        /// 正常释放ping资源
        /// </summary>
        private void DisposePing()
        {
            if (ping != null)
            {
                ping.SendAsyncCancel();
                ping.Dispose();
                ping = null;
            }
        }

        private void frmMMSServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposePing();
        }
    }
}