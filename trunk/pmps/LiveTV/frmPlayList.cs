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
        private WebServiceInvoker wsi;

        public frmPlayList(MMSServerCFG cfg_, frmLiveTV ftv_)
            : this()
        {
            cfg = cfg_;
            ftv = ftv_;
            InitWs();
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
            if (wsi != null)
            {
                try
                {
                    string[] urlArr = wsi.InvokeMethodReturnNativeObjectArray<string>("PmpsService", "GetMedialUrl");
                    if (urlArr != null && urlArr.Length > 0)
                    {
                        Pmps.Common.MoMediaservindex[] rc = new Pmps.Common.MoMediaservindex[urlArr.Length];
                        for (int i = 0; i < urlArr.Length; i++)
                        {
                            rc[i].Url = urlArr[i];
                            rc[i].Description = "����治֧�ַ�������.";
                        }

                        if (rc != null && rc.Length > 0)
                        {
                            mediaList.Clear();
                            mediaList.AddRange(rc);
                            objectListView1.BuildList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("�ӷ�������ȡ�����б�ʧ�ܡ��������ķ�������ַ���ô���\r\nϵͳ��Ϣ��" + ex.Message);
                }
            }
        }

        private void InitWs()
        {
             string url = String.Format("http://{0}/Pmps.asmx", cfg.Base_Url);
            try
            {
                //�ӷ�������ȡ���µĲ����б�
               
                wsi = new WebServiceInvoker("http://localhost/zbws/Pmps.asmx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("������Ч��{0}\r\n{1}" ,url, ex.Message));
            }
        }
    }
}