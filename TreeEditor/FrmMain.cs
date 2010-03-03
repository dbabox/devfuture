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
using System.Diagnostics;
using System.Data.Common;
using Logging;

namespace TreeEditor
{
    public partial class FrmMain : Form
    {

        private readonly TextBoxTraceListener _textBoxListener;
        
        const string TEMP_UNKNOW_NODE_PID = "#UNKNOW_PID#";
        const string NODE_INFO_FMT = "ID={0}\r\nText={1}\r\nLogicID={2}\r\nIndex={3}\r\nPID={4}\r\nIsChecked={5}\r\nʵ����Ϣ:{6}";

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
            
            Trace.TraceInformation("_tree_DragDrop:e.Data={0}",e.Data);
           
            //���ϷŵĽڵ㣬ʹ�������ж����λ��
            if (tva.DropPosition.Node != null)
            {
                //����ק�Ľڵ�
                TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
                TreeNodeAdv dropOnNode = tva.DropPosition.Node;
                DataRowTvaNode dropOnNodeNM = tva.DropPosition.Node.Tag as DataRowTvaNode;


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
                        DataRowTvaNode nm = n.Tag as DataRowTvaNode;

                        if (n != dropOnNode && nm.TNA_ID != dropOnNodeNM.TNA_ID)
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
        /// ��ʼ�϶�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Trace.TraceInformation("��ʼ�϶�:_tree_ItemDrag:e.Item={0}", e.Item);            
            tva.DoDragDropSelectedNodes(DragDropEffects.Move);
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
                Trace.WriteLine("�϶���������:_tree_DragEnter:��ʽ��Ч");
                e.Effect = e.AllowedEffect;
            }
            else
            {
                Trace.WriteLine("�϶���������:_tree_DragEnter:��ʽ��Ч");
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
            if (tva.CurrentNode != null)
            {
                if (MessageBox.Show("ɾ���ڵ㽫�ƻ����ṹ����ɵ�Ӱ���޷�������\r\n��ȷ��ɾ��ѡ�������ڵ���", "����",
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

            model.FullUpdateIDAndSave();
            MessageBox.Show(String.Format("д���ڵ���Ϣ�����ݿ����!\r\n��Ӱ���¼:{0}",100),"��ʾ",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
        }

      



        private void BtnDt2Tree_Click(object sender, EventArgs e)
        {
            tva.BeginUpdate();
            int rc = model.SyncDataTable2TreeNodes();           
            tva.EndUpdate();
            MessageBox.Show(String.Format("����е�����ͬ���������!{0}",
                rc == model.DtTreeNodeMo.Rows.Count ? String.Empty :
                String.Format("\r\n�������{0}����¼��������{1}���ڵ㣬˵������д���[����]�ڵ�.\r\n������Ϣ��������־.",dgvTarget.RowCount,
                rc)), "��ʾ",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        #region �ƶ��ڵ�İ�ť�¼�
        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (tva.CurrentNode == null)
            {
                MessageBox.Show("��ѡ��Ҫ���ƵĽڵ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            DataRowTvaNode nm = tva.CurrentNode.Tag as DataRowTvaNode;
            //TODO:����
         
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            //����1��
            if (tva.CurrentNode == null)
            {
                MessageBox.Show("��ѡ��Ҫ���ƵĽڵ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (tva.CurrentNode.NextNode == null)
            {
                MessageBox.Show("ĩ�ڵ��޷��ڱ����������ƣ����Ҫת�Ʋ�Σ�����ק��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            DataRowTvaNode nm = tva.CurrentNode.Tag as DataRowTvaNode;          
            //TODO:����


        }

        #endregion

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TODO:�ǳ����Ѻã���ʱ��ô��
            model.Tta.Schema.Dispose();
        }
    }
}