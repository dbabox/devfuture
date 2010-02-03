using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WMPLib;
using System.Runtime.InteropServices;


namespace LiveTV
{
    public partial class frmLiveTV : Form
    {
       
        private MMSServerCFG cfg;
        /// <summary>
        /// ��ʱ��
        /// </summary>
        private int cnt;

        public frmLiveTV()
        {
            InitializeComponent();
            tsmiClose.Enabled = false;
            #region//TV����
            //tsmiOpen.Visible = false;
            //tsmiClose.Visible = false;
            //tsmiPause_Continue.Visible = false;
            //tsmiStop.Visible = false;
            //toolStripSeparator2.Visible = false;
            //toolStripSeparator1.Visible = false;
            #endregion           
            Player.uiMode = "none";
            Player.settings.autoStart = true;
            Player.enableContextMenu = true;
            //player.settings.enableErrorDialogs = true;
            //player.ErrorEvent += new EventHandler(player_ErrorEvent);//����ɽػ����

            Player.Ctlenabled = false;
            Player.StatusChange += new EventHandler(player_StatusChange);
            //����ʱ��������
            Player.Warning +=new AxWMPLib._WMPOCXEvents_WarningEventHandler(player_Warning);
            timer1.Tick += new EventHandler(timer1_Tick);//6��
            cnt = 0;

        }

        void player_Warning(object sender, AxWMPLib._WMPOCXEvents_WarningEvent e)
        {
            MessageBox.Show(e.description);
        }

     

        void player_ErrorEvent(object sender, EventArgs e)
        {
            MessageBox.Show(Player.Error.get_Item(Player.Error.errorCount - 1).errorDescription);
        }

       

        void player_StatusChange(object sender, EventArgs e)
        {
            tsslCommon.Text = Player.status;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (cnt++ > 100)
            {
                timer1.Stop();
                Player.Ctlcontrols.stop();
                MessageBox.Show("���ð����������10���ӣ�\r\n����ϲ�����������빺����ʽ�档");
                this.Close();
            }

        }


        #region ���Ƽ���Ӧ
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                Player.Ctlcontrols.stop();
            }
            catch (COMException comExc)
            {
                int hr = comExc.ErrorCode;
                String Message = String.Format("ϵͳ����\nHRESULT = {1}\n{2}", hr.ToString(), comExc.Message);
                MessageBox.Show(Message, "COM Exception");
            }   
        }

       

        /// <summary>
        /// ��URL�������ļ���URL��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiOpen_Click(object sender, EventArgs e)
        {          
           
            using (frmOpenURL frm = new frmOpenURL())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        //�����ڲ��ţ���رյ�ǰ��Ƶ
                        Player.URL = frm.SelectedMediaSource;                        
                        Player.Ctlcontrols.play();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ָ���ĵ�ַ����ȷ���޷����ţ�\r\n"+ex.Message, "����");
                    }
                    tsmiClose.Enabled = true;
                }
            }
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            tsmiClose.Enabled = false;
            Player.Ctlcontrols.stop();
            
        }

        /// <summary>
        /// �˳�Ӧ�ó���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            tsmiClose.Enabled = false;
            Player.Ctlcontrols.stop();
            Player.Dispose();
            this.Close();
        }

        /// <summary>
        /// ��ͣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPause_Continue_Click(object sender, EventArgs e)
        {
            try
            {

                if (Player.playState == WMPPlayState.wmppsPlaying)
                {
                    Player.Ctlcontrols.pause();
                }
                else
                {
                    Player.Ctlcontrols.play();
                }
            }
            catch (COMException comExc)
            {
                int hr = comExc.ErrorCode;
                String Message = String.Format("ϵͳ����\nHRESULT = {1}\n{2}", hr.ToString(), comExc.Message);
                MessageBox.Show(Message, "COM Exception");
            }  
          
        }

        /// <summary>
        /// ֹͣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiStop_Click(object sender, EventArgs e)
        {
            try
            {
                Player.Ctlcontrols.stop();
            }
            catch (COMException comExc)
            {
                int hr = comExc.ErrorCode;
                String Message = String.Format("ϵͳ����.\nHRESULT = {1}\n{2}", hr.ToString(), comExc.Message);
                MessageBox.Show(Message, "COM Exception");
            }   

        }

        private void tsmiFullScreen_Click(object sender, EventArgs e)
        {
            if (Player.playState == WMPPlayState.wmppsPlaying||
                Player.playState == WMPPlayState.wmppsPaused)
            {
                Player.fullScreen = true;
            }
        }

        /// <summary>
        /// ��ʾ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiMMSServer_Click(object sender, EventArgs e)
        {
            using (frmMMSServer frm = new frmMMSServer())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    //���ŷ�����ʱ�Զ����¼����ˣ������������
                    //cfg.LoadServerURLFromCfg();
                }
            }
        }
        #endregion

        private void frmLiveTV_Load(object sender, EventArgs e)
        {
            //����Web����
            string url = "http://localhost:4155/Pmps.asmx";
            DevFuture.Common.WebServiceInvoker wsi = new DevFuture.Common.WebServiceInvoker(new Uri(url));
            Pmps.Common.MyHeaderInternal h = new Pmps.Common.MyHeaderInternal();
            h.DF_Uid = "0001";
            //string result = wsi.InvokeMethodReturnNativeObject<string>("MyHeader", h, "PmpsService", "HelloWorld", null);


            Pmps.Common.MoUser result = wsi.InvokeMethodReturnCustomObject<Pmps.Common.MoUser>("MyHeader", h, "PmpsService", "GetUser", null);
            //object result = DevFuture.Common.WebServiceHelper.InvokeWebService(url, "PmpsService", "GetUser", null);
            //Pmps.Common.MoUser m = (Pmps.Common.MoUser)result;
            MessageBox.Show(result.UserName);
            //Pmps.Common.MoUser mo = wsi.InvokeMethod<Pmps.Common.MoUser>("Pmps.Common.PmpsService", "GetUser", null);
            //MessageBox.Show(mo.UserName);
            //MessageBox.Show(CLK.ClientLicence.SCA_GetLocalCACode());


            //int[] rc= wsi.InvokeMethodReturnNativeObjectArray<int>("PmpsService", "GetIntArray", 5);
            Pmps.Common.MoUser[] rc = wsi.InvokeMethodReturnCustomObjectArray<Pmps.Common.MoUser>("PmpsService", "GetUserArray", 5);

            MessageBox.Show(rc[4].UserName);


            cfg = new MMSServerCFG();             
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {            
            using (frmAbout frm = new frmAbout())
            {
                frm.ShowDialog(this);
            }
        }

        /// <summary>
        /// ���ŷ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPlayMMSServer_Click(object sender, EventArgs e)
        {            
            try
            {
                cfg.LoadServerURLFromCfg();
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ط���������ʧ�ܣ����°�װӦ�ó�����Խ�����⣡\r\n" + ex.Message,
                    "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Player.URL=cfg.Media_Url;
                

                if (Player.Error.errorCount > 0)
                {
                    //�����˴���
                    MessageBox.Show("��ַ" + cfg.Media_Url + "�д����޷����š�\r\n�������粻ͨ���������磡", "����");
                }
                else
                {
                    tsmiPlayMMSServer.Enabled = false;
                }
                
                timer1.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show("���������ô��󣬻������粻���ã�\r\n������������ú����������"+ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            

        }
        /// <summary>
        /// �ر�ʱ�ͷſؼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLiveTV_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void tsmiPlayList_Click(object sender, EventArgs e)
        {
            frmPlayList frm = new frmPlayList();
            frm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            
            frm.Show();
        }
        
    }
}