using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rtp.Gui
{
    public partial class FrmOpenReader : Form
    {
        Dictionary<string, string> readersMap = new Dictionary<string, string>();
        public string SelectedReader
        {
            get
            {
                return comboBoxReaders.SelectedValue as string;
            }
        }

        public bool EnableDebug
        {
            get
            {
                return checkBoxEnableDebug.Checked;
            }
        }
        public FrmOpenReader()
        {
            InitializeComponent();

            readersMap.Add("D8U", "D8读卡器USB接口版（D8U）");
            readersMap.Add("T10N", "T10N读卡器USB接口（T10N）");
            BindingSource bs = new BindingSource();
            bs.DataSource = readersMap;
            comboBoxReaders.DisplayMember = "Value";
            comboBoxReaders.ValueMember = "Key";
            comboBoxReaders.DataSource = bs;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        

    }
}
