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
       

        public frmLiveTV()
        {
            InitializeComponent();
            tsmiClose.Enabled = false;
            #region//TV设置
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
            //player.ErrorEvent += new EventHandler(player_ErrorEvent);//这里可截获错误

            Player.Ctlenabled = false;
            Player.StatusChange += new EventHandler(player_StatusChange);
            //播放时发生警告
            Player.Warning +=new AxWMPLib._WMPOCXEvents_WarningEventHandler(player_Warning);


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

        

        #region 控制键响应
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                Player.Ctlcontrols.stop();                
            }
            catch (COMException comExc)
            {
                int hr = comExc.ErrorCode;
                String Message = String.Format("系统错误。\nHRESULT = {1}\n{2}", hr.ToString(), comExc.Message);
                MessageBox.Show(Message, "COM Exception");
            }   
        }

       

        /// <summary>
        /// 打开URL（本地文件或URL）
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
                        //若正在播放，则关闭当前视频
                        Player.URL = frm.SelectedMediaSource;                        
                        Player.Ctlcontrols.play();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("指定的地址不正确，无法播放！\r\n"+ex.Message, "错误");
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
        /// 退出应用程序
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
        /// 暂停
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
                String Message = String.Format("系统错误。\nHRESULT = {1}\n{2}", hr.ToString(), comExc.Message);
                MessageBox.Show(Message, "COM Exception");
            }  
          
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiStop_Click(object sender, EventArgs e)
        {
            try
            {
                Player.Ctlcontrols.stop();
                tsmiPlayMMSServer.Enabled = true;
               
            }
            catch (COMException comExc)
            {
                int hr = comExc.ErrorCode;
                String Message = String.Format("系统错误.\nHRESULT = {1}\n{2}", hr.ToString(), comExc.Message);
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
        /// 显示服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiMMSServer_Click(object sender, EventArgs e)
        {
            using (frmMMSServer frm = new frmMMSServer())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    //播放服务器时自动重新加载了，这里无需加载
                    //cfg.LoadServerURLFromCfg();
                }
            }
        }
        #endregion

        private void frmLiveTV_Load(object sender, EventArgs e)
        {
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
        /// 播放服务器
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
                MessageBox.Show("加载服务器配置失败，重新安装应用程序可以解决问题！\r\n" + ex.Message,
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Player.URL=cfg.Broadcast_Url;
                

                if (Player.Error.errorCount > 0)
                {
                    //发生了错误
                    MessageBox.Show("广播地址" + cfg.Broadcast_Url + "有错误，无法播放。\r\n可能网络不通，请检查网络！", "错误");
                }
                else
                {
                    tsmiPlayMMSServer.Enabled = false;
                }
#if ENABLE_CHECK_TIME
                timer1.Start();
#endif

            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器配置错误，或者网络不可用！\r\n请检查服务器设置和网络情况。"+ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            

        }
        /// <summary>
        /// 关闭时释放控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLiveTV_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void tsmiPlayList_Click(object sender, EventArgs e)
        {
            try
            {
                cfg.LoadServerURLFromCfg();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载服务器配置失败，重新安装应用程序可以解决问题！\r\n" + ex.Message,
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            frmPlayList frm = new frmPlayList(cfg,this);
            frm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            
            frm.Show();
        }

        public void PlayUrl(string url)
        {
            try
            {
                Player.Ctlcontrols.stop();
                //若正在播放，则关闭当前视频
                Player.URL = url;
                Player.Ctlcontrols.play();
                tsmiClose.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("指定的地址不正确，无法播放！\r\n" + ex.Message, "错误");
            }
            
        }
        
    }
}