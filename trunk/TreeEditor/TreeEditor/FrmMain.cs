/*
 * ֵ�øĽ��ĵط��� 2009-12-04
 * 1��DataGridView��ʾ����400�е����ݣ��������ǳ���.
 * 2����Լ�߼�ID������Ӧ���Ƕ��߳��첽.
 * 3���Ż��ڴ�ʹ�ã����ڴ�����ϣ�Ԥ��Ԥ����ڴ�ʹ������
 * 4������ӿڡ�
 * 5��������ģ�廯���ࡣ
 * 6������Spring.
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
        const string NODE_INFO_FMT = "ID={0}\r\nText={1}\r\nLogicID={2}\r\nIndex={3}\r\nPID={4}\r\nIsChecked={5}\r\nʵ����Ϣ:{6}";

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
            //ͬ����DT            
            //foreach (DataRowTvaNode node in e.Children)
            //{
            //    node.TNA_PID = TEMP_UNKNOW_NODE_PID;
            //    tnmModel.TreeTable.Rows.Remove(node.DataRow);
            //}
        }
 

        #region ������
        /// <summary>
        /// ������ͨ�ó�ʼ���趨
        /// </summary>
        /// <param name="tva"></param>
        private void SetupTree(Aga.Controls.Tree.TreeViewAdv tva)
        {
            tva.LoadOnDemand = false;       
            //tva.SelectionChanged += new EventHandler(tva_SelectionChanged);
            //TODO:������������������
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
            //ѡ�л��ѡ��
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


        #region �ϷŲ���
        /// <summary>
        /// �϶����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_DragDrop(object sender, DragEventArgs e)
        {
            log.DebugFormat("_tree_DragDrop:e.Data={0}",e.Data);  
            //���ϷŵĽڵ㣬ʹ�������ж����λ��
            if (_tree.DropPosition.Node != null)
            {
                
                //����ק�Ľڵ�
                TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
                TreeNodeAdv dropOnNode = _tree.DropPosition.Node;
                DataRowTvaNode dropOnNodeNM = _tree.DropPosition.Node.Tag as DataRowTvaNode;
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("���½ڵ㱻��ק���˽ڵ�{0}��:", dropOnNodeNM);
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        log.DebugFormat("�ڵ�{0} ����ק��:{0}��", nodes[i].Tag);
                    }
                }


                _tree.BeginUpdate();      
                //�������(������)
                if (_tree.DropPosition.Position == NodePosition.Inside)
                {
                    log.DebugFormat("�����ڽڵ���:{0}", _tree.DropPosition.Node.Tag);                    
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
                else //���ⲿ(��ǰ��)
                {
                    TreeNodeAdv parent = dropOnNode.Parent;
                    log.DebugFormat("��ק��Level={0},NM={1},λ��={2}", parent.Level, parent.Tag, _tree.DropPosition.Position);

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
        /// ��ʼ�϶�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            log.DebugFormat("��ʼ�϶�:_tree_ItemDrag:e.Item={0}", e.Item);            
            _tree.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        #endregion

        /// <summary>
        /// �������϶�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_DragEnter(object sender, DragEventArgs e)
        {            
           
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[]))) //��ʽ��Ч����
            {
                log.Debug("�϶���������:_tree_DragEnter:��ʽ��Ч");
                e.Effect = e.AllowedEffect;
            }
            else
            {
                log.Debug("�϶���������:_tree_DragEnter:��ʽ��Ч");
                e.Effect = DragDropEffects.None;
            }
                     
        }

        /// <summary>
        /// ɾ��һ���ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_tree.CurrentNode != null)
            {
                if (MessageBox.Show("ɾ���ڵ㽫�ƻ����ṹ����ɵ�Ӱ���޷�������\r\n��ȷ��ɾ��ѡ�������ڵ���", "����",
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
        /// ɾ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delCheckedNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkedNodes.Count > 0)
            {
                if (MessageBox.Show("ɾ���ڵ㽫�ƻ����ṹ����ɵ�Ӱ���޷�������\r\n��ȷ��ɾ��ѡ�������ڵ���", "����",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }               

                //�Ȼ�ȡ�Ѿ�ѡ���Ľڵ�           
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
                MessageBox.Show("�����ݿ���������!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
            catch (DbException ex)
            {
                MessageBox.Show("���ݿ��쳣" + ex.InnerException.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSyncToDb_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("��ȷ���Ѿ�������ԭʼ�����ݿ⣡\r\n��ȷ�ϼ�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                return;
            }

            int rc = tnmModel.SyncToDb(checkBoxForce.Checked);
            MessageBox.Show(String.Format("д���ڵ���Ϣ�����ݿ����!\r\n��Ӱ���¼:{0}",rc),"��ʾ",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }

      

        string msg_force_sync = @"ǿ��ͬ������������ݿ�Դ��Ȼ�󽫵�ǰ���ڵ�ͬ�������ݿ⡣�⽫����Ӱ�죺
1�����ݿ���Դ��ȫ������գ��������ָ���ֻ�ܴ��Զ�����XML�лָ���
2������ǰ��ɾ���ڵ��������Ч��
3�����洢�����ݿ���¼�͵�ǰ�����ṹ���ݽ���ȫһ�¡�
4�����ݿ��еġ����ա��ڵ㽫�������

���ؾ��棺������ȷ��ǿ��ͬ��������Ҫ�ĲŽ���ǿ��ͬ����
";
        private void checkBoxForce_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxForce.Checked)
            {

                MessageBox.Show(msg_force_sync, "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void BtnDt2Tree_Click(object sender, EventArgs e)
        {
            _tree.BeginUpdate();
            int rc = tnmModel.SyncDataTable2TreeNodes();           
            _tree.EndUpdate();
            MessageBox.Show(String.Format("����е�����ͬ���������!{0}",
                rc==dgvTarget.RowCount?String.Empty:
                String.Format("\r\n�������{0}����¼��������{1}���ڵ㣬˵������д���[����]�ڵ�.\r\n������Ϣ��������־.",dgvTarget.RowCount,
                rc)), "��ʾ",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        #region �ƶ��ڵ�İ�ť�¼�
        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (_tree.CurrentNode == null)
            {
                MessageBox.Show("��ѡ��Ҫ���ƵĽڵ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            DataRowTvaNode nm = _tree.CurrentNode.Tag as DataRowTvaNode;
            if (nm.TNA_Index == 0)
            {
                MessageBox.Show("�������׽ڵ��޷���ͬ����������ƣ����Ҫת�Ʋ�Σ�����ק��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            int toIndex = nm.TNA_Index - 1;
            if (_tree.CurrentNode.Level == 1)
            {
                tnmModel.MoveToRoot(nm, toIndex);
            }
            else
            {
                //����1��
                tnmModel.MoveNode(nm, _tree.CurrentNode.Parent.Tag as DataRowTvaNode, toIndex);

            }
         
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            //����1��
            if (_tree.CurrentNode == null)
            {
                MessageBox.Show("��ѡ��Ҫ���ƵĽڵ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (_tree.CurrentNode.NextNode == null)
            {
                MessageBox.Show("ĩ�ڵ��޷��ڱ����������ƣ����Ҫת�Ʋ�Σ�����ק��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
            //TODO:�ǳ����Ѻã���ʱ��ô��
            tnmModel.Tta.Schema.Dispose();
        }
    }
}