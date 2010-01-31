using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LiveTV
{
    public partial class frmPlayList : Form
    {
        public frmPlayList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 从服务器获取最新的播放列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            //TODO:从服务器获取最新的播放列表
        }
    }
}