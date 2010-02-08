using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LiveTV
{
    public partial class FrmFileTransfer : Form
    {
        string url = "http://localhost:4155/FileTransfer.asmx";
        DevFuture.Common.WebServiceInvoker wsi;
        public FrmFileTransfer()
        {
            InitializeComponent();
            try
            {
                wsi = new DevFuture.Common.WebServiceInvoker(url);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDownloadViaStream_Click(object sender, EventArgs e)
        {
            if (wsi != null)
            {
                //�ļ���ȡ���ڴ��У���δ����
                Pmps.Common.StubGetFileResponseStreaming rs = wsi.InvokeMethodReturnCustomObject<Pmps.Common.StubGetFileResponseStreaming>("FileTransfer", "GetFileStreaming", "������MV.wmv");
                MessageBox.Show(rs.fileData.Length.ToString());

            }
        }

        private void BtnPutStream_Click(object sender, EventArgs e)
        {
            if (wsi != null)
            {
                //�����ļ�ʱ���ͻ��˵�SOCKET�����滺��������
                //��ΪWeb���񲢲�������������ͻ�������
                Pmps.Common.StubGetFileRequestStreaming rq = new Pmps.Common.StubGetFileRequestStreaming();
                rq.fileName =  System.IO.Path.GetFileName(textBoxFileName.Text);
                rq.fileData = System.IO.File.ReadAllBytes(textBoxFileName.Text);

                bool rc = wsi.InvokeMethodReturnNativeObject<bool>("FileTransfer", "PutFileStreaming", wsi.TranslateStub(rq, "GetFileRequestStreaming"));
                MessageBox.Show(rc.ToString());
            }
        }
    }
}