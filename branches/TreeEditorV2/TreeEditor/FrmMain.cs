/*
 * 值得改进的地方： 2009-12-04
 * 1、DataGridView显示超过400行的数据，则会操作非常慢.
 * 2、规约逻辑ID很慢，应考虑多线程异步.
 * 3、优化内存使用，在内存分配上，预先预测好内存使用量。
 * 4、精简接口。
 * 5、适配器模板化基类。
 * 6、集成Spring.
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
using TreeEditor.Core;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using System.Diagnostics;
using System.Data.Common;
using Logging;

namespace TreeEditor
{
    public partial class FrmMain : Form
    {

        private readonly TextBoxTraceListener _textBoxListener;
        
        const string TEMP_UNKNOW_NODE_PID = "#UNKNOW_PID#";
        const string NODE_INFO_FMT = "ID={0}\r\nText={1}\r\nLogicID={2}\r\nIndex={3}\r\nPID={4}\r\nIsChecked={5}\r\n实体信息:{6}";

        //ITreeTableAdapter tta;
        //TNMTreeModel tnmModel;
        DataRowTreeModel model;
        IList<DataRowTvaNode> checkedNodes;
        private TvaSchema s;

        //private Dictionary<ITreeTableAdapter, string> treeAdapterDic = new Dictionary<ITreeTableAdapter, string>();

        public FrmMain(TvaSchema s)
        {
            this.s = s;
            InitializeComponent();
            SetupTree(tva);

            model = new DataRowTreeModel(this.s);         
            
            tva.SelectionChanged += new EventHandler(_tree_SelectionChanged);
            
            dgvTarget.RowPostPaint += delegate(object sender, DataGridViewRowPostPaintEventArgs e)
                    {
                        using (System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(dgvTarget.RowHeadersDefaultCellStyle.ForeColor))
                        {
                            e.Graphics.DrawString(Convert.ToString((int)(e.RowIndex + 1),
                                (IFormatProvider)System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b,
                                (float)(e.RowBounds.Location.X + 20), (float)(e.RowBounds.Location.Y + 4));
                        }
                    };

            checkedNodes = new List<DataRowTvaNode>();

            _textBoxListener = new TextBoxTraceListener(textBoxLog);
            Trace.Listeners.Add(_textBoxListener);
            tva.Model = model;
            dgvTarget.DataSource = model.DtTreeNodeMo;
        }

        void _tree_SelectionChanged(object sender, EventArgs e)
        {
            if (tva.CurrentNode != null)
            {
                DataRowTvaNode tn= tva.CurrentNode.Tag as DataRowTvaNode;
                Trace.WriteLineIf(tn != null, tn.ToString());
            }
        }

  

        #region 树设置
        /// <summary>
        /// 对树做通用初始化设定
        /// </summary>
        /// <param name="tva"></param>
        private void SetupTree(Aga.Controls.Tree.TreeViewAdv tva)
        {
            tva.LoadOnDemand = false;       
            //tva.SelectionChanged += new EventHandler(tva_SelectionChanged);
            //TODO:请在这里根据情况设置
            NodeCheckBox cb = new NodeCheckBox("IsChecked");
            cb.EditEnabled = true;
            cb.ThreeState = false;
            cb.CheckStateChanged += new EventHandler<TreePathEventArgs>(cb_CheckStateChanged);
            tva.NodeControls.Add(cb);

            NodeStateIcon ni = new NodeStateIcon();
            ni.DataPropertyName = "Icon";
            tva.NodeControls.Add(ni);

            NodeTextBox tb = new NodeTextBox();
            tb.DataPropertyName = "TNA_Text";
            
            tb.EditEnabled = true;
            tb.EditOnClick = false;
            //tb.ChangesApplied += new EventHandler(tb_ChangesApplied);
            tva.NodeControls.Add(tb);
            tva.SelectionChanged += new EventHandler(tva_SelectionChanged);

            
        }

        void tva_SelectionChanged(object sender, EventArgs e)
        {
            if (tva.CurrentNode != null)
            {
                DataRowTvaNode node=tva.CurrentNode.Tag as DataRowTvaNode;
                foreach (DataGridViewRow r in dgvTarget.Rows)
                {
                    if (r.Cells[s.Tna_id_field_name.ToUpper()].Value == node.DataRow[s.Tna_id_field_name])
                    {
                        dgvTarget.CurrentCell = r.Cells[s.Tna_id_field_name.ToUpper()];
                        break;
                    }
                }
            }
        }
 
        void cb_CheckStateChanged(object sender, TreePathEventArgs e)
        {
            //选中或非选中
            DataRowTvaNode node = e.Path.LastNode as DataRowTvaNode;
            if (node.IsChecked)
            {
                checkedNodes.Add(node);
            }
            else
            {
                checkedNodes.Remove(node);
            }
        }
        #endregion


        #region 拖放操作
        /// <summary>
        /// 拖动完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_DragDrop(object sender, DragEventArgs e)
        {
            
            Trace.TraceInformation("_tree_DragDrop:e.Data={0}",e.Data);
           
            //往上放的节点，使用它来判断相对位置
            if (tva.DropPosition.Node != null)
            {
                //被拖拽的节点
                TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
                TreeNodeAdv dropOnNode = tva.DropPosition.Node;
                DataRowTvaNode dropOnNodeNM = tva.DropPosition.Node.Tag as DataRowTvaNode;


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
                        DataRowTvaNode nm = n.Tag as DataRowTvaNode;

                        if (n != dropOnNode && nm.TNA_ID != dropOnNodeNM.TNA_ID)
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
                        toIndex = dropOnNode.Index + 1;
                    }
                    foreach (TreeNodeAdv n in nodes)
                    {
                        DataRowTvaNode nm = n.Tag as DataRowTvaNode;
                        if (n != dropOnNode && nm.TNA_ID != dropOnNodeNM.TNA_ID)
                        {
                            model.Move(tva.GetPath(n.Parent), nm, tva.GetPath(dropOnNode.Parent), toIndex);
                        }
                    }

                }
                tva.EndUpdate();
            }
            
        }



        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Trace.TraceInformation("开始拖动:_tree_ItemDrag:e.Item={0}", e.Item);            
            tva.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        #endregion

        /// <summary>
        /// 区域内拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_DragEnter(object sender, DragEventArgs e)
        {            
           
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[]))) //格式有效数据
            {
                Trace.WriteLine("拖动进入区域:_tree_DragEnter:格式有效");
                e.Effect = e.AllowedEffect;
            }
            else
            {
                Trace.WriteLine("拖动进入区域:_tree_DragEnter:格式无效");
                e.Effect = DragDropEffects.None;
            }
                     
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tva.CurrentNode != null)
            {
                if (MessageBox.Show("删除节点将破坏树结构，造成的影响无法撤销！\r\n您确认删除选定的数节点吗？", "警告",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                if (tva.SelectedNode != null)
                {
                    
                    model.Remove(tva.GetPath(tva.SelectedNode.Parent), tva.SelectedNode.Tag as DataRowTvaNode);
                }

 

               

                
            }
        }

       
        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delCheckedNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkedNodes.Count > 0)
            {
                if (MessageBox.Show("删除节点将破坏树结构，造成的影响无法撤销！\r\n您确认删除选定的数节点吗？", "警告",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }               

                //先获取已经选定的节点           
                foreach (DataRowTvaNode n in checkedNodes)
                {
                    TreeNodeAdv tn= tva.FindNodeByTag(n);

                    model.Remove(tva.GetPath(tn.Parent), n);
                   
                }
                checkedNodes.Clear();
            }

        }

        private void unCheckAllNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataRowTvaNode n in checkedNodes)
            {
                n.IsChecked = false;                
            }
            checkedNodes.Clear();
            tva.Refresh();
        }

        private void btnSync2Tree_Click(object sender, EventArgs e)
        {          
            try
            {
                model.RefreshFromAdapter();
                if (!object.ReferenceEquals(tva.Model, model))
                {
                    tva.Model = model;
                }
                dgvTarget.DataSource = model.DtTreeNodeMo;
                MessageBox.Show("从数据库加载树完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
            catch (DbException ex)
            {
                MessageBox.Show("数据库异常" + ex.InnerException.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSyncToDb_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请确保已经备份了原始的数据库！\r\n您确认继续吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                return;
            }

            model.FullUpdateIDAndSave();
            MessageBox.Show(String.Format("写树节点信息到数据库完成!\r\n共影响记录:{0}",100),"提示",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }

      



        private void BtnDt2Tree_Click(object sender, EventArgs e)
        {
            tva.BeginUpdate();
            int rc = model.SyncDataTable2TreeNodes();           
            tva.EndUpdate();
            MessageBox.Show(String.Format("表格中的数据同步到树完成!{0}",
                rc == model.DtTreeNodeMo.Rows.Count ? String.Empty :
                String.Format("\r\n表格中有{0}条记录，树中有{1}个节点，说明表格中存在[悬空]节点.\r\n具体信息见错误日志.",dgvTarget.RowCount,
                rc)), "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        #region 移动节点的按钮事件
        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (tva.CurrentNode == null)
            {
                MessageBox.Show("请选择要上移的节点!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            DataRowTvaNode nm = tva.CurrentNode.Tag as DataRowTvaNode;
            //TODO:上移
         
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            //下移1格
            if (tva.CurrentNode == null)
            {
                MessageBox.Show("请选择要下移的节点!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (tva.CurrentNode.NextNode == null)
            {
                MessageBox.Show("末节点无法在本层内再下移！如果要转移层次，请拖拽！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            DataRowTvaNode nm = tva.CurrentNode.Tag as DataRowTvaNode;          
            //TODO:下移


        }

        #endregion

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TODO:非常不友好，暂时这么干
            model.Tta.Schema.Dispose();
        }
    }
}