using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LiveTV
{
    public partial class frmOpenURL : Form
    {
        private const string clipFileFilters = "����ý���ļ�(avi asf wmv mpg mpeg mp3 wma)|*.avi;*.asf;*.wmv;*.mpg;*.mpeg;*.wma;*.mp3|" +
        "Video Files (avi asf wmv qt mov mpg mpeg m1v)|*.avi;*.asf;*.wmv;*.qt;*.mov;*.mpg;*.mpeg;*.m1v|" +
        "Audio files (wav wma mpa mp2 mp3 au aif aiff snd)|*.wav;*.wma;*.mpa;*.mp2;*.mp3;*.au;*.aif;*.aiff;*.snd|" +
        "MIDI Files (mid midi rmi)|*.mid;*.midi;*.rmi|" +
        "All Files (*.*)|*.*";

        private string selectedMediaSource;

        public string SelectedMediaSource
        {
            get { return selectedMediaSource; }            
        }

        public frmOpenURL()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
            
        }

        private void btnBrownser_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "����Ƶ�ļ�...";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                ofd.Filter = clipFileFilters;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //���Ŷ�Ӧ���ļ�
                    textBoxFile.Text = ofd.FileName;
                    selectedMediaSource = ofd.FileName;
                    
                }
            }


            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(selectedMediaSource) && !String.IsNullOrEmpty(textBoxFile.Text))
            {
                selectedMediaSource = textBoxFile.Text;
            }
            if (String.IsNullOrEmpty(selectedMediaSource))
            {
                MessageBox.Show("��������ȷ���ļ�������URL.\r\n��ȷ�ĵ�ַ���磺\r\nC:\\�ҵ�����\\example.wmv\r\nmms://server/liveshow/ch1.asf", 
                    "�ļ�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFile.Focus();                
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectedMediaSource = null;
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}