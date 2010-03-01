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
        private Uri wsUri;

        public frmMMSServer()
        {
            InitializeComponent();
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
            cfg = new MMSServerCFG();
            ping = new System.Net.NetworkInformation.Ping();
            ping.PingCompleted += new System.Net.NetworkInformation.PingCompletedEventHandler(ping_PingCompleted);
            labelSampleLink.Text = String.Empty;
           
        }
        void ping_PingCompleted(object sender, System.Net.NetworkInformation.PingCompletedEventArgs e)
        {
            if (e.Cancelled && ping!=null)
            {
                textBoxPingLog.AppendText("Ping Cancled.");
                textBoxPingLog.AppendText(Environment.NewLine);
                btnTestServer.Enabled = true;
            }
            else if (!e.Cancelled && ping!=null && e.Error != null)
            {
                textBoxPingLog.AppendText("Error:");
                textBoxPingLog.AppendText(e.Error.ToString());
                textBoxPingLog.AppendText(Environment.NewLine);
                btnTestServer.Enabled = true;

            }           
            else if(!e.Cancelled && ping!=null)
            {
                rpt.Add(e.Reply.RoundtripTime);

                textBoxPingLog.AppendText(e.UserState.ToString());
                textBoxPingLog.AppendText(":");
                DisplayReply(e.Reply);
                if (pingCount <= maxpingCount)
                {

                    ping.SendAsync(wsUri.DnsSafeHost, timeout, buffer, options, (object)pingCount++);
                }
                else
                {

                    textBoxPingLog.AppendText("\r\n\r\nPing测试完成" + Environment.NewLine);
                    rpt.Sort();
                    textBoxPingLog.AppendText("统计信息：" + Environment.NewLine);
                    textBoxPingLog.AppendText(String.Format("min RoundtripTime:{0}ms,  max RoundtripTime:{1}ms\r\n", rpt[0], rpt[pingCount - 2]));
                    btnTestServer.Enabled = true;
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
            wsUri= new Uri(String.Format("http://{0}", textBoxBase_Url.Text));
           

            textBoxPingLog.Clear();
            if (!String.IsNullOrEmpty(wsUri.DnsSafeHost))
            {
                pingCount = 1;
                rpt.Clear();
                btnTestServer.Enabled = false;
                ping.SendAsync(wsUri.DnsSafeHost, timeout, buffer, options, (object)pingCount++);
            }
            else
            {
                MessageBox.Show(String.Format( "服务地址有错误:\r\n{0}",textBoxBase_Url.Text),
                    "错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                textBoxBase_Url.Focus();
            }
            
          
          
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            cfg.Pmps_Base_Url = textBoxBase_Url.Text;
            cfg.Video_Proto = comboBoxProto.SelectedItem.ToString();
            cfg.Broadcast_Url = labelSampleLink.Text;


            cfg.SaveServerURLCfg();
            MessageBox.Show(String.Format("媒体广播地址被设置为:\r\n{0}", cfg.Broadcast_Url), 
                "服务器", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void frmMMSServer_Load(object sender, EventArgs e)
        {
            cfg.LoadServerURLFromCfg();
            textBoxPingLog.Clear();
            textBoxBase_Url.Text = cfg.Pmps_Base_Url;
            if (!String.IsNullOrEmpty(cfg.Video_Proto))
            {
                comboBoxProto.SelectedItem = cfg.Video_Proto;
            }
            if (cfg.Video_Proto == "FILE")
            {
                textBoxVieo_Url.Text = cfg.Broadcast_Url.Substring(8);
            }
            else
            {
                textBoxVieo_Url.Text = cfg.Broadcast_Url.Substring( cfg.Video_Proto.Length+3);
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

        private void comboBoxProto_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBroadcastUrl();
        }

        private void textBoxVieo_Url_Leave(object sender, EventArgs e)
        {
            UpdateBroadcastUrl();
        }

        private void UpdateBroadcastUrl()
        {
            string prefix = null;
            if (!String.IsNullOrEmpty(comboBoxProto.Text))
            {
                if (comboBoxProto.Text == "FILE")
                {
                    prefix = "///";
                }
                else
                {
                    prefix = "//";
                }

                labelSampleLink.Text = String.Format("{0}:{2}{1}", 
                    comboBoxProto.Text, textBoxVieo_Url.Text, prefix);
            }
            else
            {
                labelSampleLink.Text = String.Empty;
            }
        }
    }
}