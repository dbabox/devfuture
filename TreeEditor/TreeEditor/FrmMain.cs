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
using Common.Logging;
using System.Data.Common;

namespace TreeEditor
{
    public partial class FrmMain : Form
    {
         
        static readonly ILog log = LogManager.GetCurrentClassLogger();
        const string TEMP_UNKNOW_NODE_PID = "#UNKNOW_PID#";
        const string NODE_INFO_FMT = "ID={0}\r\nText={1}\r\nLogicID={2}\r\nIndex={3}\r\nPID={4}\r\nIsChecked={5}\r\n实体信息:{6}";

        //ITreeTableAdapter tta;
        //TNMTreeModel tnmModel;
        DataRowTreeModel tnmModel;
        IList<DataRowTvaNode> checkedNodes;

        //private Dictionary<ITreeTableAdapter, string> treeAdapterDic = new Dictionary<ITreeTableAdapter, string>();

        public FrmMain(TvaSchema s)
        {
            InitializeComponent();
            SetupTree(_tree);

            tnmModel = new DataRowTreeModel(s);         
            tnmModel.NodesRemoved += new EventHandler<TreeModelEventArgs>(tnmModel_NodesRemoved);
            tnmModel.NodesInserted += new EventHandler<TreeModelEventArgs>(tnmModel_NodesInserted);
            _tree.SelectionChanged += new EventHandler(_tree_SelectionChanged);
            
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
        }

        void _tree_SelectionChanged(object sender, EventArgs e)
        {
            if (_tree.CurrentNode != null)
            {
                DataRowTvaNode tn= _tree.CurrentNode.Tag as DataRowTvaNode;
                if (tn != null)
                {
                    textBoxLog.Text = String.Format(NODE_INFO_FMT,
                        tn.TNA_ID, tn.TNA_Text, tn.TNA_LogicId, tn.TNA_Index, tn.TNA_PID, tn.IsChecked, tn);
                }
            }
        }

        void tnmModel_NodesInserted(object sender, TreeModelEventArgs e)
        {
            //foreach (DataRowTvaNode node in e.Children)
            //{
            //    node.TNA_PID = TEMP_UNKNOW_NODE_PID;
            //    //tnmModel.TreeTable.Rows.Add(node.DataRow);
            //}
        }

        void tnmModel_NodesRemoved(object sender, TreeModelEventArgs e)
        {
            //同步到DT            
            //foreach (DataRowTvaNode node in e.Children)
            //{
            //    node.TNA_PID = TEMP_UNKNOW_NODE_PID;
            //    tnmModel.TreeTable.Rows.Remove(node.DataRow);
            //}
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
            log.DebugFormat("_tree_DragDrop:e.Data={0}",e.Data);  
            //往上放的节点，使用它来判断相对位置
            if (_tree.DropPosition.Node != null)
            {
                
                //被拖拽的节点
                TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
                TreeNodeAdv dropOnNode = _tree.DropPosition.Node;
                DataRowTvaNode dropOnNodeNM = _tree.DropPosition.Node.Tag as DataRowTvaNode;
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("以下节点被拖拽到此节点{0}上:", dropOnNodeNM);
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        log.DebugFormat("节点{0} 被拖拽到:{0}上", nodes[i].Tag);
                    }
                }


                _tree.BeginUpdate();      
                //如果在内(且下面)
                if (_tree.DropPosition.Position == NodePosition.Inside)
                {
                    log.DebugFormat("放置在节点内:{0}", _tree.DropPosition.Node.Tag);                    
                    foreach (TreeNodeAdv n in nodes)
                    {
                        DataRowTvaNode nm = n.Tag as DataRowTvaNode;
                        if (nm.TNA_PID != dropOnNodeNM.TNA_ID && nm.TNA_ID != dropOnNodeNM.TNA_ID)
                        {
                            tnmModel.MoveNode(nm, dropOnNodeNM, dropOnNode.Children.Count);
                        }
                    }
                    _tree.DropPosition.Node.IsExpanded = true;
                }
                else //在外部(分前后)
                {
                    TreeNodeAdv parent = dropOnNode.Parent;
                    log.DebugFormat("拖拽到Level={0},NM={1},位置={2}", parent.Level, parent.Tag, _tree.DropPosition.Position);

                    int toIndex = dropOnNode.Index;
                   
                    if (parent.Level == 0)
                    {
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            DataRowTvaNode node = nodes[i].Tag as DataRowTvaNode;
                            if (node!=dropOnNodeNM && node.TNA_ID != dropOnNodeNM.TNA_ID)
                            {
                                tnmModel.MoveToRoot(node, toIndex);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            DataRowTvaNode node = nodes[i].Tag as DataRowTvaNode;
                            if (node != dropOnNodeNM && node.TNA_ID != dropOnNodeNM.TNA_ID)
                            {
                                tnmModel.MoveNode(node, parent.Tag as DataRowTvaNode, toIndex);
                            }
                        }
                    }

                }
                _tree.EndUpdate();
            }
            
        }



        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            log.DebugFormat("开始拖动:_tree_ItemDrag:e.Item={0}", e.Item);            
            _tree.DoDragDropSelectedNodes(DragDropEffects.Move);
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
                log.Debug("拖动进入区域:_tree_DragEnter:格式有效");
                e.Effect = e.AllowedEffect;
            }
            else
            {
                log.Debug("拖动进入区域:_tree_DragEnter:格式无效");
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
            if (_tree.CurrentNode != null)
            {
                if (MessageBox.Show("删除节点将破坏树结构，造成的影响无法撤销！\r\n您确认删除选定的数节点吗？", "警告",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                _tree.BeginUpdate();
                if (_tree.CurrentNode.IsLeaf)
                {
                    tnmModel.RemoveLeafNode(_tree.CurrentNode.Tag as DataRowTvaNode);
                }
                else
                {
                    tnmModel.RemoveNode(_tree.CurrentNode.Tag as DataRowTvaNode);
                }

                _tree.EndUpdate();

               

                
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
                    tnmModel.RemoveNode(n);
                   
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
            _tree.Refresh();
        }

        private void btnSync2Tree_Click(object sender, EventArgs e)
        {          
            try
            {
                tnmModel.RefreshFromAdapter();
                if (!object.ReferenceEquals(_tree.Model, tnmModel))
                {
                    _tree.Model = tnmModel;
                }
                dgvTarget.DataSource = tnmModel.TreeTable;
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

            int rc = tnmModel.SyncToDb(checkBoxForce.Checked);
            MessageBox.Show(String.Format("写树节点信息到数据库完成!\r\n共影响记录:{0}",rc),"提示",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }

      

        string msg_force_sync = @"强制同步将先清空数据库源表，然后将当前树节点同步到数据库。这将产生影响：
1、数据库中源表将全部被清空，如果您想恢复，只能从自动备份XML中恢复。
2、您当前的删除节点操作将生效。
3、最后存储的数据库表记录和当前的树结构数据将完全一致。
4、数据库中的【悬空】节点将被清除。

严重警告：仅当您确认强制同步是您需要的才进行强制同步。
";
        private void checkBoxForce_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxForce.Checked)
            {

                MessageBox.Show(msg_force_sync, "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void BtnDt2Tree_Click(object sender, EventArgs e)
        {
            _tree.BeginUpdate();
            int rc = tnmModel.SyncDataTable2TreeNodes();           
            _tree.EndUpdate();
            MessageBox.Show(String.Format("表格中的数据同步到树完成!{0}",
                rc==dgvTarget.RowCount?String.Empty:
                String.Format("\r\n表格中有{0}条记录，树中有{1}个节点，说明表格中存在[悬空]节点.\r\n具体信息见错误日志.",dgvTarget.RowCount,
                rc)), "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        #region 移动节点的按钮事件
        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (_tree.CurrentNode == null)
            {
                MessageBox.Show("请选择要上移的节点!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            DataRowTvaNode nm = _tree.CurrentNode.Tag as DataRowTvaNode;
            if (nm.TNA_Index == 0)
            {
                MessageBox.Show("本层内首节点无法在同层次内再上移！如果要转移层次，请拖拽！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            int toIndex = nm.TNA_Index - 1;
            if (_tree.CurrentNode.Level == 1)
            {
                tnmModel.MoveToRoot(nm, toIndex);
            }
            else
            {
                //上移1格
                tnmModel.MoveNode(nm, _tree.CurrentNode.Parent.Tag as DataRowTvaNode, toIndex);

            }
         
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            //下移1格
            if (_tree.CurrentNode == null)
            {
                MessageBox.Show("请选择要下移的节点!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (_tree.CurrentNode.NextNode == null)
            {
                MessageBox.Show("末节点无法在本层内再下移！如果要转移层次，请拖拽！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            DataRowTvaNode nm = _tree.CurrentNode.Tag as DataRowTvaNode;          
            int toIndex = nm.TNA_Index + 1;
            if (_tree.CurrentNode.Level == 1)
            {
                tnmModel.MoveToRoot(nm, toIndex);
            }
            else
            {
                tnmModel.MoveNode(nm, _tree.CurrentNode.Parent.Tag as DataRowTvaNode, toIndex);
            }


        }

        #endregion

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TODO:非常不友好，暂时这么干
            tnmModel.Tta.Schema.Dispose();
        }
    }
}