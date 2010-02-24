/*
 * ���������ڲ����ļ�����.
 * 
 * 
 * */
 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

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
                MessageBox.Show("��ȡ����ʧ�ܣ�"+ ex.Message);
            }
        }

        private void BtnDownloadViaStream_Click(object sender, EventArgs e)
        {
            if (wsi != null)
            {
                //�ļ���ȡ���ڴ��У���δ����
                Pmps.Common.StubGetFileResponseStreaming rs = 
                    wsi.InvokeMethodReturnCustomObject<Pmps.Common.StubGetFileResponseStreaming>("FileTransfer", 
                    "GetFileStreaming", "������MV.wmv");
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

                bool rc = wsi.InvokeMethodReturnNativeObject<bool>("FileTransfer", "PutFileStreaming", 
                    wsi.TranslateStub(rq, "GetFileRequestStreaming"));
                MessageBox.Show(rc.ToString());
            }
        }

        private void BtnSendByChunk_Click(object sender, EventArgs e)
        {
            if (wsi != null)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(textBoxFileName.Text);
                bool rc = wsi.InvokeMethodReturnNativeObject<bool>("FileTransfer", "UploadFile",
                    fi.Name,
                    System.IO.File.ReadAllBytes(fi.FullName), 
                    0);

                MessageBox.Show(rc.ToString());

            }
        }

        private void BtnSend2Chunk_Click(object sender, EventArgs e)
        {
            if (wsi != null)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(textBoxFileName.Text);
                bool rc = false;
                string rfilename = String.Empty;

                ParameterModifier[] mods = new ParameterModifier[3];
                for (int i = 0; i < mods.Length; i++)
                {
                    mods[i] = new ParameterModifier(1);
                    if (i == 2) mods[i][0] = true;
                    else mods[i][0] = false;
                }             

                //��������Ƕ������е�Ԫ�ص����õ��޸�
                //�мǣ�String������ֵ���ͣ���Ϊ�����Ԫ��ʱ������ֵ���ƣ��ṹ����װ��Ĺ��̣�
                object[] args = new object[3] { fi.Extension, fi.Length, rfilename };

                rc = wsi.InvokeMethodReturnNativeObject<bool>("FileTransfer", "PrepareForUploadFileByServer",
                    ref args, new bool[] { false,false,true } );

                MessageBox.Show(args[2].ToString());

                
                
              

            }
        }

        private void BtnVerify_Click(object sender, EventArgs e)
        {
            string hash = null;
            using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                using (FileStream fs = File.OpenRead(textBoxFileName.Text))
                {
                    hash = Convert.ToBase64String(md5.ComputeHash(fs));                    
                }
            }
            bool rc= wsi.InvokeMethodReturnNativeObject<bool>("FileTransfer", "VerifyFileMD5", textBox1.Text, hash);
            MessageBox.Show(rc.ToString());
        }

        private void BtnClearWsCache_Click(object sender, EventArgs e)
        {
            //�������������Ϊ����UnloadӦ�ó�����
        }

       
    }
}
 