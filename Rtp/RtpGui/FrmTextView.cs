using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rtp.Gui
{
    public partial class FrmTextView : Form
    {
        public FrmTextView()
        {
            InitializeComponent();
        }

        public FrmTextView(IList<string> content):this()
        {
            SetContent(content);
        }

        public void SetContent(IList<String> content)
        {
            textBox1.Clear();
           
            

            for (int i = 0; i < content.Count; i++)
            {
                textBox1.AppendText(i.ToString("D4"));
                textBox1.AppendText(":");
                textBox1.AppendText(content[i]);
                textBox1.AppendText(Environment.NewLine);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "文本文件(*.txt)|*.txt";
                sfd.DefaultExt = "txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {                   
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
                    {                       
                        sw.Write(textBox1.Text);
                    }
                }
            }
        }


        
    }
}