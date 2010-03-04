/*
 
 * 
 * 
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
 
using Aga.Controls.Tree.NodeControls; 
using Aga.Controls.Tree;
using DF.WinUI.CustomComponent;
using System.Diagnostics;
using DF.Core.Trace;

namespace DF.WinUI.TreeEditor
{
    public partial class FrmTreeEditor : Form
    {
         
        private readonly TvaDataTableTreeModel model;
        private readonly TextBoxTraceListener _textBoxListener;
        DbSchema schema;
     
        public FrmTreeEditor(DbSchema schema_)
        {
            InitializeComponent();
            
            dgv.RowPostPaint += delegate(object sender, DataGridViewRowPostPaintEventArgs e)
                  {
                      using (System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(dgv.RowHeadersDefaultCellStyle.ForeColor))
                      {
                          e.Graphics.DrawString(Convert.ToString((int)(e.RowIndex + 1),
                              (IFormatProvider)System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b,
                              (float)(e.RowBounds.Location.X + 20), (float)(e.RowBounds.Location.Y + 4));
                      }
                  };

            #region ׼�����ڵ�
            NodeTextBox tb = new NodeTextBox();
            tb.DataPropertyName = "Text";       
            tva.NodeControls.Add(tb);
            #endregion
            //����ȫ���ڵ�

            //schema = new DbSchema();
            //schema.ColumnName_Key = "ASS_CONT_ID";
            //schema.ColumnName_LogicKey = "ASS_CONT_LOGIC_ID";
            //schema.ColumnName_ParentKey = "PARENT_ASS_CONT_ID";
            //schema.ColumnName_Text = "CONTENT";
            //schema.ConnectionString = "Data Source=.;Initial Catalog=zb;User ID=sa;Password=admin;User Instance=False; Connect Timeout=6";
           
            //schema.ProviderName = "System.Data.SqlClient";
            //schema.Schema_Name = "ZB_ASSESS_CONTENT"; 
            //schema.DataTable_Root_Filter_Expression = "ASS_CONT_TYPE=1";
            schema = schema_;
            model = new TvaDataTableTreeModel(schema);
            tva.Model = model;
            dgv.DataSource = model.DtTreeNodeMo;

            
           
            Trace.Listeners.Clear();
            _textBoxListener = new TextBoxTraceListener(textBoxLog);
            _textBoxListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Timestamp | TraceOptions.ThreadId;
            Trace.Listeners.Add(_textBoxListener);
 
        }

        
        private void btn_Click(object sender, EventArgs e)
        {
              
            Button btn = (Button)sender;
            string opt = btn.Name;
            switch (opt)
            {
                case "btnSaveTree2Db":
                    {
                        if (MessageBox.Show("�������ṹ���ĸ��ģ���Ӱ���������ø������ݵ��κ��������ݡ�\r\n����ά��Ӧ��ֻ��ϵͳ����Ա���У���Ӧ��������ϵͳһ�£�\r\n�����ȷ�������������Ѿ�������ʵ����������ϣ���������ֹ�ϵ����ȡ�����߼����IDӳ�䵽������ \r\n\r\n��ȷ�����浱ǰ���ṹ��", "����",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }

                        //ִ�б���:�ȸ��������߼�ID���ٽ��߼�ID��ֵ������ID
                        //�������ݿ�
                        model.FullUpdateIDAndSave();
                        tva.ClearSelection();
                         
                       
                        MessageBox.Show("�������ṹ�����ݿ�ɹ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                       
                        break;
                    }          
            
                case "btnDel":
                    {

                        if (tva.SelectedNode != null)
                        {
                            if (MessageBox.Show("��ȷ��ɾ���˽ڵ���\r\nע�⣺����ýڵ�����ӽڵ㣬����ͬ�ӽڵ�һ��ɾ����", "����", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }

                            model.Remove(tva.GetPath(tva.SelectedNode.Parent), tva.SelectedNode.Tag as ITvaModelNode);
                        }
                        else
                        {
                            MessageBox.Show("��ѡ��һ���ڵ�.");
                        }
                        break;
                    }
            
                default:
                    {
                        MessageBox.Show("����ʶ��Ĳ���:" + opt, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
            }
        }

       
    
 

      
        private void tva_DragDrop(object sender, DragEventArgs e)
        {
            Trace.TraceInformation("tva_DragDrop:e.Data={0}", e.Data);
            //���ϷŵĽڵ㣬ʹ�������ж����λ��
            if (tva.DropPosition.Node != null)
            {
                //����ק�Ľڵ�
                TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
                TreeNodeAdv dropOnNode = tva.DropPosition.Node;

                ITvaModelNode dropOnNodeNM = tva.DropPosition.Node.Tag as ITvaModelNode;
                Trace.TraceInformation("���½ڵ㱻��ק���˽ڵ�{0}��:", dropOnNodeNM);
                for (int i = 0; i < nodes.Length; i++)
                {
                    Trace.TraceInformation("�ڵ�{0} ����ק��:{0}��", nodes[i].Tag);
                }


                tva.BeginUpdate();
               
                Trace.TraceInformation("��ק��Level={0},NM={1},λ��={2}", dropOnNode.Level, dropOnNode.Tag, tva.DropPosition.Position);//�ڵ�ǰ���߽ڵ���ڵ���inside
                //�������(������)
                if (tva.DropPosition.Position == NodePosition.Inside)
                {
                    Trace.TraceInformation("�����ڽڵ���:{0}", tva.DropPosition.Node.Tag);
                    foreach (TreeNodeAdv n in nodes) //��ק�ڵ�
                    {
                        ITvaModelNode nm = n.Tag as ITvaModelNode;                         
                        if (n!=dropOnNode && nm.ParentKey.ToString()!= (string)dropOnNodeNM.Key)
                        {
                            model.Move(tva.GetPath(n.Parent), nm, tva.GetPath(dropOnNode), dropOnNode.Children.Count);//����ĩβ
                        }
                    }
                    tva.DropPosition.Node.IsExpanded = true;
                }
                else //���ⲿ(��ǰ��)
                {                    
                    int toIndex = -1;
                    if (tva.DropPosition.Position == NodePosition.Before)
                    {
                        toIndex = dropOnNode.Index;
                    }
                    else
                    {
                        toIndex = dropOnNode.Index+1;
                    }
                    foreach (TreeNodeAdv n in nodes)
                    {
                        ITvaModelNode nm = n.Tag as ITvaModelNode;                        
                        if (n != dropOnNode && (string)nm.ParentKey != (string)dropOnNodeNM.Key)
                        {
                            model.Move(tva.GetPath(n.Parent), nm, tva.GetPath(dropOnNode.Parent), toIndex);                            
                        }
                    }                 

                }
                tva.EndUpdate();
            }
            
        }

        private void tva_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[]))) //��ʽ��Ч����
            {
                Trace.WriteLine("�϶���������:tva_DragEnter:��ʽ��Ч");
                e.Effect = e.AllowedEffect;
            }
            else
            {
                Trace.TraceWarning("�϶���������:tva_DragEnter:��ʽ��Ч");
                e.Effect = DragDropEffects.None;
            }
        }

        private void tva_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Trace.TraceInformation("��ʼ�϶�:_tree_ItemDrag:e.Item={0}", e.Item);
            tva.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        

         

        private void dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dgv[e.ColumnIndex, e.RowIndex].Value = DBNull.Value;
        }

        private void btnDt2Tree_Click(object sender, EventArgs e)
        {
            model.SyncDt2Tree();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            model.LoadFromSchema();
        }

        private void tva_SelectionChanged(object sender, EventArgs e)
        {
            if (tva.CurrentNode != null)
            {
                TvaDataRowNode node = tva.CurrentNode.Tag as TvaDataRowNode;
                Trace.TraceInformation(node.ToString());
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    if (Object.ReferenceEquals((r.DataBoundItem as DataRowView).Row,
                        node.Tag))
                    {
                        dgv.CurrentCell = r.Cells[schema.ColumnName_Text];
                        break;
                    }
                }
            }
        }

        private void checkBoxAutoMap_CheckedChanged(object sender, EventArgs e)
        {
            model.AutoMapLogicKey2Key = checkBoxAutoMap.Checked;
            if (checkBoxAutoMap.Checked)
            {
                MessageBox.Show("�Զ�ӳ������������\r\nԭ���Ͻ�������Ӧ���ܴ����κκ��塣���Զ�ӳ��������ڳ��ν������ṹʱʹ�á�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}