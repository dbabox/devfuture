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
        TvaSchema s = new TvaSchema();
        public FrmSchema()
        {
            InitializeComponent();

            BindingSource bs = new BindingSource(TvaSchema.ProviderDic, null);
            comboBox1.DataSource = bs;
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";

        }


        private void UI2Model()
        {
            s.ConnectionString = textBoxConnStr.Text;
            s.ProviderName = comboBox1.SelectedValue.ToString();
            s.Tna_table_name = textBoxTreeTableName.Text;
            s.Tna_id_field_name = textBoxIdFieldName.Text;
            s.Tna_pid_field_name = textBoxPID.Text;
            s.Tna_logic_id_map_field = textBoxLogicIDMap.Text;
            s.Tna_text_field_name = textBoxNodeName.Text;
        }

        private void Model2UI()
        {
            textBoxConnStr.Text=s.ConnectionString   ;
            comboBox1.SelectedValue=s.ProviderName ;
            textBoxTreeTableName.Text= s.Tna_table_name  ;
            textBoxIdFieldName.Text=s.Tna_id_field_name   ;
            textBoxPID.Text=s.Tna_pid_field_name   ;
            textBoxLogicIDMap.Text=s.Tna_logic_id_map_field   ;
             textBoxNodeName.Text=s.Tna_text_field_name ;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            UI2Model();

            using (FrmMain frm = new FrmMain(s))
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.ExecutablePath;
                ofd.Filter = "*.xml|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string file = ofd.FileName;
                    s= DevFuture.Common.ObjectXMLSerializer<TvaSchema>.Load(file);
                    Model2UI();
                        
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UI2Model();
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = Application.ExecutablePath;
                sfd.Filter = "*.xml|*.xml";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string file = sfd.FileName;
                    DevFuture.Common.ObjectXMLSerializer<TvaSchema>.Save(s, file);
                    MessageBox.Show("保存完成!" + file, "保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        
    }
}