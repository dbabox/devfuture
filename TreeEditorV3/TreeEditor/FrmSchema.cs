using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DF.WinUI.CustomComponent;

 

namespace DF.WinUI.TreeEditor
{
    public partial class FrmSchema : Form
    {
        DbSchema s = new DbSchema();
        public FrmSchema()
        {
            InitializeComponent();

            Dictionary<String, String> providerDic = new Dictionary<string, string>();
            providerDic.Add("SQL Server", "System.Data.SqlClient");
            providerDic.Add("Oracle", "Oracle.DataAccess.Client");


            BindingSource bs = new BindingSource(providerDic, null);
            comboBox1.DataSource = bs;
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";



        }


        private void UI2Model()
        {
            s.ConnectionString = textBoxConnStr.Text;
            s.ProviderName = comboBox1.SelectedValue.ToString();
            s.Schema_Name = textBoxTreeTableName.Text;
            s.ColumnName_Key = textBoxIdFieldName.Text;
            s.ColumnName_ParentKey = textBoxPID.Text;
            s.ColumnName_LogicKey = textBoxLogicIDMap.Text;
            s.ColumnName_Text = textBoxNodeName.Text;
            s.DataTable_Root_Filter_Expression = textBoxRootFilter.Text;
            s.SQL_Record_Filter_Clause = textBoxNodeWhere.Text;
        }

        private void Model2UI()
        {
            textBoxConnStr.Text=s.ConnectionString   ;
            comboBox1.SelectedValue=s.ProviderName ;
            textBoxTreeTableName.Text = s.Schema_Name;
            textBoxIdFieldName.Text = s.ColumnName_Key;
            textBoxPID.Text = s.ColumnName_ParentKey;
            textBoxLogicIDMap.Text = s.ColumnName_LogicKey;
            textBoxNodeName.Text = s.ColumnName_Text;
            textBoxRootFilter.Text = s.DataTable_Root_Filter_Expression;
            textBoxNodeWhere.Text = s.SQL_Record_Filter_Clause;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            UI2Model();

            using (FrmTreeEditor frm = new FrmTreeEditor(s))
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
                    s= DevFuture.Common.ObjectXMLSerializer<DbSchema>.Load(file);
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
                    DevFuture.Common.ObjectXMLSerializer<DbSchema>.Save(s, file);
                    MessageBox.Show("保存完成!" + file, "保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        
    }
}