using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LiveTV
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            textBoxLicence.Text = String.Format("{0},{1}\r\n本机客户端已授权。",
                Environment.MachineName, Environment.OSVersion);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}