using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TreeEditor.Core;

namespace TreeEditor
{
    public partial class FrmSchema : Form
    {
        public FrmSchema()
        {
            InitializeComponent();

            BindingSource bs = new BindingSource(TvaSchema.ProviderDic, null);
            comboBox1.DataSource = bs;
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TvaSchema s = new TvaSchema();
            s.ConnectionString = textBoxConnStr.Text;
            s.ProviderName = comboBox1.SelectedValue.ToString();
            s.Tna_table_name = textBoxTreeTableName.Text;
            s.Tna_id_field_name = textBoxIdFieldName.Text;
            s.Tna_pid_field_name = textBoxPID.Text;
            s.Tna_logic_id_map_field = textBoxLogicIDMap.Text;
            s.Tna_text_field_name = textBoxNodeName.Text;

            using (FrmMain frm = new FrmMain(s))
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}