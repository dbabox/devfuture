using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevFuture.Common;
using BrightIdeasSoftware;

namespace LiveTV
{
    public partial class frmPlayList : Form
    {
        protected frmPlayList()
        {
            InitializeComponent();
        }
        MMSServerCFG cfg;
        private readonly List<Pmps.Common.MoMediaservindex> mediaList = new List<Pmps.Common.MoMediaservindex>();
        private readonly frmLiveTV ftv;
        public frmPlayList(MMSServerCFG cfg_, frmLiveTV ftv_)
            : this()
        {
            cfg = cfg_;
            ftv = ftv_;
            objectListView1.ShowGroups = false;
            TypedObjectListView<Pmps.Common.MoMediaservindex> medialLV =
                new TypedObjectListView<Pmps.Common.MoMediaservindex>(this.objectListView1);
            medialLV.GenerateAspectGetters();
            objectListView1.SetObjects(mediaList);
            objectListView1.MouseDoubleClick += new MouseEventHandler(objectListView1_MouseDoubleClick);

            btnRefreshList_Click(null, null);
        }

        void objectListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            object obj = objectListView1.GetSelectedObject();
            if (obj != null)
            {
                Pmps.Common.MoMediaservindex m = (Pmps.Common.MoMediaservindex)obj;
                //让主窗体播放
                ftv.PlayUrl(m.Url);


            }
        }

        /// <summary>
        /// 从服务器获取最新的播放列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            try
            {
                //从服务器获取最新的播放列表
                string url = String.Format("http://{0}/Pmps.asmx", cfg.Base_Url);
                WebServiceInvoker wsi = new WebServiceInvoker(url);

                Pmps.Common.MoMediaservindex[] rc = wsi.InvokeMethodReturnCustomObjectArray<Pmps.Common.MoMediaservindex>("PmpsService", "GetMedialList");
                if (rc != null && rc.Length > 0)
                {
                    mediaList.Clear();
                    mediaList.AddRange(rc);
                    objectListView1.BuildList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("从服务器获取播放列表失败。可能您的服务器地址配置错误。\r\n系统信息："+ex.Message);
            }
        }
    }
}