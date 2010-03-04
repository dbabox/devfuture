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

            #region 准备树节点
            NodeTextBox tb = new NodeTextBox();
            tb.DataPropertyName = "Text";       
            tva.NodeControls.Add(tb);
            #endregion
            //加载全部节点

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
                        if (MessageBox.Show("您对树结构做的更改，将影响所有引用该树内容的任何其他数据。\r\n树的维护应该只由系统管理员进行，且应保持所有系统一致！\r\n如果您确定树的主键被已经被其他实体引用且您希望保持这种关系，请取消【逻辑层次ID映射到主键】 \r\n\r\n请确定保存当前树结构吗？", "警告",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }

                        //执行保存:先更新所有逻辑ID，再将逻辑ID赋值给物理ID
                        //保存数据库
                        model.FullUpdateIDAndSave();
                        tva.ClearSelection();
                         
                       
                        MessageBox.Show("保存树结构到数据库成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                       
                        break;
                    }          
            
                case "btnDel":
                    {

                        if (tva.SelectedNode != null)
                        {
                            if (MessageBox.Show("您确定删除此节点吗？\r\n注意：如果该节点具有子节点，会连同子节点一起删除。", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }

                            model.Remove(tva.GetPath(tva.SelectedNode.Parent), tva.SelectedNode.Tag as ITvaModelNode);
                        }
                        else
                        {
                            MessageBox.Show("请选择一个节点.");
                        }
                        break;
                    }
            
                default:
                    {
                        MessageBox.Show("不可识别的操作:" + opt, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
            }
        }

       
    
 

      
        private void tva_DragDrop(object sender, DragEventArgs e)
        {
            Trace.TraceInformation("tva_DragDrop:e.Data={0}", e.Data);
            //往上放的节点，使用它来判断相对位置
            if (tva.DropPosition.Node != null)
            {
                //被拖拽的节点
                TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
                TreeNodeAdv dropOnNode = tva.DropPosition.Node;

                ITvaModelNode dropOnNodeNM = tva.DropPosition.Node.Tag as ITvaModelNode;
                Trace.TraceInformation("以下节点被拖拽到此节点{0}上:", dropOnNodeNM);
                for (int i = 0; i < nodes.Length; i++)
                {
                    Trace.TraceInformation("节点{0} 被拖拽到:{0}上", nodes[i].Tag);
                }


                tva.BeginUpdate();
               
                Trace.TraceInformation("拖拽到Level={0},NM={1},位置={2}", dropOnNode.Level, dropOnNode.Tag, tva.DropPosition.Position);//节点前或者节点后或节点上inside
                //如果在内(且下面)
                if (tva.DropPosition.Position == NodePosition.Inside)
                {
                    Trace.TraceInformation("放置在节点内:{0}", tva.DropPosition.Node.Tag);
                    foreach (TreeNodeAdv n in nodes) //被拽节点
                    {
                        ITvaModelNode nm = n.Tag as ITvaModelNode;                         
                        if (n!=dropOnNode && nm.ParentKey.ToString()!= (string)dropOnNodeNM.Key)
                        {
                            model.Move(tva.GetPath(n.Parent), nm, tva.GetPath(dropOnNode), dropOnNode.Children.Count);//附到末尾
                        }
                    }
                    tva.DropPosition.Node.IsExpanded = true;
                }
                else //在外部(分前后)
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

            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[]))) //格式有效数据
            {
                Trace.WriteLine("拖动进入区域:tva_DragEnter:格式有效");
                e.Effect = e.AllowedEffect;
            }
            else
            {
                Trace.TraceWarning("拖动进入区域:tva_DragEnter:格式无效");
                e.Effect = DragDropEffects.None;
            }
        }

        private void tva_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Trace.TraceInformation("开始拖动:_tree_ItemDrag:e.Item={0}", e.Item);
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
                MessageBox.Show("自动映射会更改主键。\r\n原则上讲：主键应不能代表任何含义。故自动映射仅建议在初次建立树结构时使用。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}