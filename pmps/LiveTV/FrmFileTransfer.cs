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
                //文件读取到内存中，并未保存
                Pmps.Common.StubGetFileResponseStreaming rs = wsi.InvokeMethodReturnCustomObject<Pmps.Common.StubGetFileResponseStreaming>("FileTransfer", "GetFileStreaming", "布兰妮MV.wmv");
                MessageBox.Show(rs.fileData.Length.ToString());

            }
        }

        private void BtnPutStream_Click(object sender, EventArgs e)
        {
            if (wsi != null)
            {
                //超大文件时，客户端的SOCKET将报告缓冲区不足
                //因为Web服务并不能真正流处理客户端请求
                Pmps.Common.StubGetFileRequestStreaming rq = new Pmps.Common.StubGetFileRequestStreaming();
                rq.fileName =  System.IO.Path.GetFileName(textBoxFileName.Text);
                rq.fileData = System.IO.File.ReadAllBytes(textBoxFileName.Text);

                bool rc = wsi.InvokeMethodReturnNativeObject<bool>("FileTransfer", "PutFileStreaming", wsi.TranslateStub(rq, "GetFileRequestStreaming"));
                MessageBox.Show(rc.ToString());
            }
        }
    }
}