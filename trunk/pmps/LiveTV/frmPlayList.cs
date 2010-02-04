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
                //�������岥��
                ftv.PlayUrl(m.Url);


            }
        }

        /// <summary>
        /// �ӷ�������ȡ���µĲ����б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            try
            {
                //�ӷ�������ȡ���µĲ����б�
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
                MessageBox.Show("�ӷ�������ȡ�����б�ʧ�ܡ��������ķ�������ַ���ô���\r\nϵͳ��Ϣ��"+ex.Message);
            }
        }
    }
}