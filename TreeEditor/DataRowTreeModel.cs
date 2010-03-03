using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls.Tree;
using TreeEditor.Core;

using System.Data;
using System.Diagnostics;

namespace TreeEditor
{
    public class DataRowTvaNode
    {
        private DataRow row;

        public DataRow DataRow
        {
            get { return row; }
            set { row = value; }
        }
        private TvaSchema rowSchema;
        public DataRowTvaNode(DataRow row_, TvaSchema rowSchema_)
        {
            row = row_;
            rowSchema = rowSchema_;
            SyncDataRow2TvnNode();
            original_tna_id = tna_id;//���ڹ��캯���и�ֵ
        }

        public void SyncDataRow2TvnNode()
        {
            tna_id = row[rowSchema.Tna_id_field_name].ToString();
            tna_pid = row[rowSchema.Tna_pid_field_name].ToString();
            tna_text = row[rowSchema.Tna_text_field_name].ToString();
        }


        #region ITvaNode ��Ա


        private readonly string original_tna_id;
        /// <summary>
        /// ԭʼ��ID
        /// </summary>
        public string Original_tna_id
        {
            get
            {
                return original_tna_id;
            }

        }

        /// <summary>
        /// tna_id��ֱ��ʹ�� return row[rowSchema.Tna_id_field_name].ToString()��ԭ�����ڵ�Row��ɾ��
        /// ʱ���˵��ý����ִ���2009-12-20
        /// </summary>
        private string tna_id;
        public string TNA_ID
        {
            get
            {
                return tna_id;
            }
            set
            {
                tna_id = value;
                row[rowSchema.Tna_id_field_name] = value;
            }
        }

        private string tna_pid;
        public string TNA_PID
        {
            get
            {
                return tna_pid;
            }
            set
            {
                tna_pid = value;
                row[rowSchema.Tna_pid_field_name] = value;
            }
        }

        private string tna_text;
        public string TNA_Text
        {
            get
            {

                return tna_text;
            }
            set
            {
                tna_text = value;
                row[rowSchema.Tna_text_field_name] = value;
            }
        }

        private string tna_LogicId;
        public string TNA_LogicId
        {
            get
            {
                return tna_LogicId;
            }
            set
            {
                tna_LogicId = value;
                if (!String.IsNullOrEmpty(rowSchema.Tna_logic_id_map_field))
                {
                    row[rowSchema.Tna_logic_id_map_field] = value;
                }
            }
        }

        private IList<DataRowTvaNode> owner;
        public IList<DataRowTvaNode> Owner
        {
            get
            {
                return owner;
            }
            set
            {
                if (object.ReferenceEquals(owner, value) == false)
                {
                    owner = value;
                }
            }
        }

        private int tna_Level;
        public int TNA_Level
        {
            get
            {
                return tna_Level;
            }
            set
            {
                tna_Level = value;
            }
        }

        public int TNA_Index
        {
            get
            {
                if (owner != null) return owner.IndexOf(this);
                return -1;
            }
        }

        protected System.Drawing.Image icon;
        public virtual System.Drawing.Image Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
            }
        }




        #endregion

        #region ICloneable ��Ա

        public object Clone()
        {
            DataRow newrow = row.Table.NewRow();
            row.ItemArray.CopyTo(newrow.ItemArray, 0);
            return new DataRowTvaNode(newrow, this.rowSchema);

        }

        #endregion

        /// <summary>
        /// ���߼�ID�����е�ĳ��ֵ
        /// </summary>
        private void SyncLogicId()
        {
            if (!String.IsNullOrEmpty(rowSchema.Tna_logic_id_map_field))
            {
                row[rowSchema.Tna_logic_id_map_field] = TNA_LogicId;
            }
        }

        public static DataRowTvaNode[] CreateNodes(TvaSchema rowSchema, params DataRow[] rows)
        {
            DataRowTvaNode[] nodes = new DataRowTvaNode[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                nodes[i] = new DataRowTvaNode(rows[i], rowSchema);
            }
            return nodes;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (i < row.Table.Columns.Count - 1)
                {
                    sb.AppendFormat("{0}={1}/", row.Table.Columns[i].ColumnName, row[i]);
                }
                else
                {
                    sb.AppendFormat("{0}={1}", row.Table.Columns[i].ColumnName, row[i]);
                }
            }
            return sb.ToString();
        }

    }

    public class DataRowTreeModel : TreeModelBase
    {
        private DataTableTreeAdapter tta;

        public DataTableTreeAdapter Tta
        {
            get { return tta; }
            set { tta = value; }
        }

        

        #region �������ļ���ȡ
        /// <summary>
        /// �����߼�ID��ʽ [ǰ׺][###]
        /// </summary>
        string LOGIC_ID_FMT_1 = "{1}{0,3:D3}";
        /// <summary>
        /// ֧��Ҷ�ӵ��߼�ID��ʽ
        /// </summary>
        string LOGIC_ID_FMT_2 = "{0,3:D3}{1,3:D3}";
        /// <summary>
        /// ���߼�IDǰ׺
        /// </summary>
        string ROOT_LOGIC_ID_PREFIX = String.Empty;
        /// <summary>
        /// ��ʹ��ָ���ĵ���ID
        /// </summary>
        bool USE_SINGLE_SPECIAL_ROOT_ID = false;
        /// <summary>
        /// �����ض��ĵ���ID
        /// </summary>
        string SPECIALED_SIGNLE_ROOT_ID = String.Empty;

        bool is_auto_backup = true;

        string backup_path = System.IO.Path.Combine(Environment.CurrentDirectory, "backup");

        #endregion



        private int nodesTotalCount;
       

        private DataSet treeDs;
        private DataTable treeTable;

        public DataTable TreeTable
        {
            get { return treeTable; }
            set { treeTable = value; }
        }

        /// <summary>
        /// ���ڵ��б��ڹ��캯�����Զ���ʼ��.
        /// </summary>
        private IList<DataRowTvaNode> rootList;

        /// <summary>
        /// �ڵ㼯��
        /// </summary>
        private Dictionary<string, DataRowTvaNode> tnmDic = null;
        //����ʵ�ֽڵ㱾��CRUD       
        /// <summary>
        /// ���ڵ�ID���ڽӺ����б����︸�ڵ�ID������Ϊ�ա�
        /// </summary>
        private Dictionary<string, IList<DataRowTvaNode>> p2cDic = new Dictionary<string, IList<DataRowTvaNode>>();


        private bool isLoadNodesComplete = false;

        public DataRowTreeModel(TvaSchema schema_)
        {
            TvaSchema schema = schema_;
            tta = new DataTableTreeAdapter(schema);
            #region ������Ϣ
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_1"]))
            {
                LOGIC_ID_FMT_1 = System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_1"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_2"]))
            {
                LOGIC_ID_FMT_2 = System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_2"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ROOT_LOGIC_ID_PREFIX"]))
            {
                ROOT_LOGIC_ID_PREFIX = System.Configuration.ConfigurationManager.AppSettings["ROOT_LOGIC_ID_PREFIX"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["SIGNLE_ROOT_ID"]))
            {
                SPECIALED_SIGNLE_ROOT_ID = System.Configuration.ConfigurationManager.AppSettings["SIGNLE_ROOT_ID"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["USE_SINGLE_ROOT_ID"]))
            {
                string boolstr = System.Configuration.ConfigurationManager.AppSettings["USE_SINGLE_ROOT_ID"];
                if (bool.TryParse(boolstr, out USE_SINGLE_SPECIAL_ROOT_ID) == false)
                {
                    int intvalue = 0;
                    if (Int32.TryParse(boolstr, out intvalue) == true)
                    {
                        USE_SINGLE_SPECIAL_ROOT_ID = intvalue != 0;
                    }
                }
                else
                {
                    USE_SINGLE_SPECIAL_ROOT_ID = false;
                }
            }

            if (System.Configuration.ConfigurationManager.AppSettings["AutoBackup"] != null)
            {
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["AutoBackup"], out is_auto_backup);
            }
            #endregion
        }

        public DataRowTreeModel():this(new TvaSchema())
        {
             
        }

       

        /// <summary>
        /// ʹ��������ˢ��Model.
        /// </summary>
        public void RefreshFromAdapter()
        {
            nodesTotalCount = tta.GetNodesTotalCount();
            if (tnmDic == null)
            {
                tnmDic = new Dictionary<string, DataRowTvaNode>(nodesTotalCount);
            }
            else
            {
                tnmDic.Clear();
            }

            p2cDic.Clear();
            treeDs = tta.GetTreeNodeDataTable();

            #region //�����Զ�����
            if (is_auto_backup)
            {
                if (!System.IO.Directory.Exists(backup_path))
                {
                    System.IO.Directory.CreateDirectory(backup_path);
                }
                string backupFileName = System.IO.Path.Combine(backup_path, Guid.NewGuid().ToString("D") + ".xml");
                treeDs.WriteXml(backupFileName, XmlWriteMode.WriteSchema);
                Trace.TraceInformation("����{0}��{1}", tta.GetType(), backupFileName);
            }
            #endregion
            treeTable = treeDs.Tables[0];
            nodesTotalCount = treeTable.Rows.Count;
           
            Refresh();
        }

      

        /// <summary>
        /// ��ӽڵ㵽tnmDic�С�
        /// </summary>
        /// <param name="nodes"></param>
        private void AddTvaNode2tnmDic(IList<DataRowTvaNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Owner = nodes;
                tnmDic.Add(nodes[i].TNA_ID, nodes[i]);
            }

        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                Trace.TraceInformation("��ѯ���ڵ�:{0}", treePath.FirstNode);
                if (rootList == null || rootList.Count == 0)
                {
                    DataRow[] rootRows = treeTable.Select(tta.Schema.RowFilter_GetRootNodes);
                    if(rootList==null)
                       rootList = new List<DataRowTvaNode>(rootRows.Length);
                    for (int i = 0; i < rootRows.Length; i++)
                    {
                        DataRowTvaNode node = new DataRowTvaNode(rootRows[i], tta.Schema);
                        node.TNA_LogicId = String.Format(LOGIC_ID_FMT_1, i + 1, ROOT_LOGIC_ID_PREFIX);
                        rootList.Add(node);
                    }
                    AddTvaNode2tnmDic(rootList);
                }
                return rootList;
            }
            else
            {

                DataRowTvaNode nm = treePath.LastNode as DataRowTvaNode;
                Trace.TraceInformation("��ѯ{0}���ӽڵ�", nm);
                if (p2cDic.ContainsKey(nm.TNA_ID))
                {
                    return p2cDic[nm.TNA_ID];
                }
                else //��DataTable���ң�һ���ҳ��ýڵ�����к���
                {                  
                    DataRow[] rows = treeTable.Select(String.Format("{0}='{1}'", tta.Schema.Tna_pid_field_name, nm.TNA_ID));
                    IList<DataRowTvaNode> list = new List<DataRowTvaNode>(rows.Length);

                    for (int i = 0; i < rows.Length; i++)
                    {
                        DataRowTvaNode tn = new DataRowTvaNode(rows[i], tta.Schema);
                        tn.TNA_LogicId = String.Format(LOGIC_ID_FMT_2, nm.TNA_LogicId, i + 1);
                        //��������
                        list.Add(tn);
                    }
                    //list�з��غ��ӽڵ��б�
                    AddTvaNode2tnmDic(list);
                    //�����˺����б�
                    p2cDic.Add(nm.TNA_ID, list);
                    return list;
                }


            }
            
        }

        public override bool IsLeaf(TreePath treePath)
        {
            DataRowTvaNode nm = treePath.LastNode as DataRowTvaNode;
            Trace.TraceInformation("�ж�{0}�Ƿ�Ҷ��.", nm);

            if (isLoadNodesComplete)
            {
                return (p2cDic.ContainsKey(nm.TNA_ID) == false);
            }

          
            return treeTable.Select(String.Format("{0}='{1}'",
                        tta.Schema.Tna_pid_field_name, nm.TNA_ID)).Length == 0;
        }


        #region �ڵ��ƶ�

        /// <summary>
        /// ��ȡһ���ڵ��·��
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreePath GetPath(DataRowTvaNode node)
        {
            if (rootList.Contains(node))//�Ǹ��ڵ㣬��ֱ�ӷ���
            {
                return new TreePath(node);
            }
            else //�Ǹ�
            {
                Stack<object> stack = new Stack<object>();
                stack.Push(node);
                while (p2cDic.ContainsKey(node.TNA_PID)) //�ڷǸ��ڵ�����
                {
                    node = tnmDic[node.TNA_PID];//�л����丸��
                    stack.Push(node);
                    if (String.IsNullOrEmpty(node.TNA_PID)) break;

                }
                return new TreePath(stack.ToArray());

            }

        }

        /// <summary>
        /// ɾ��Ҷ�ӽڵ㣬��Ӱ��ڵ�����.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveLeafNode(DataRowTvaNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            Trace.TraceInformation("RemoveLeafNode {0} ", node);

            if (!String.IsNullOrEmpty(node.TNA_PID))
            {
                TreePath fromOwnerPath = GetPath(tnmDic[node.TNA_PID]);
                tnmDic.Remove(node.TNA_ID);//ɾ���˽ڵ�
                treeTable.Rows.Remove(node.DataRow);
                p2cDic[node.TNA_PID].Remove(node);//���б���Ҳɾ��
                OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new DataRowTvaNode[] { node }));

                if (p2cDic[node.TNA_PID].Count == 0)
                {
                    Trace.TraceInformation("�ڵ�{0}���Ѿ����ӽڵ�,�Ƴ����б�", tnmDic[node.TNA_PID]);
                    p2cDic.Remove(node.TNA_PID);
                    OnStructureChanged(new TreeModelEventArgs(fromOwnerPath, new object[] { tnmDic[node.TNA_PID] }));
                }
            }
            else
            {
                rootList.Remove(node);//������Ǹ�����Ҳɾ��
                tnmDic.Remove(node.TNA_ID);//ɾ���˽ڵ�
                treeTable.Rows.Remove(node.DataRow);
                OnNodesRemoved(new TreeModelEventArgs(TreePath.Empty, new DataRowTvaNode[] { node }));
            }

        }

        /// <summary>
        /// ɾ������һ���ڵ�.�˺������ؽ�����
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(DataRowTvaNode node)
        {
            if (tnmDic.ContainsKey(node.TNA_ID))
            {
                Stack<DataRowTvaNode> rcContainer = new Stack<DataRowTvaNode>(nodesTotalCount);
                FindChild(node, rcContainer);
                DataRowTvaNode[] nodes = rcContainer.ToArray();
                for (int i = 0; i < nodes.Length; i++)
                {
                    RemoveLeafNode(nodes[i]);
                }
                Refresh();
            }
            else
            {
                Trace.TraceInformation("��ͼɾ�������ڵĽڵ�:{0}", node);
            }
        }

        /// <summary>
        /// �ҳ�һ���ڵ�����к��ӽ�㣬���Լ�.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rcContainer"></param>
        private void FindChild(DataRowTvaNode node, Stack<DataRowTvaNode> rcContainer)
        {
            rcContainer.Push(node);
            if (p2cDic.ContainsKey(node.TNA_ID))
            {
                foreach (DataRowTvaNode n in p2cDic[node.TNA_ID])
                {
                    FindChild(n, rcContainer);
                }
            }

        }





        /// <summary>
        /// ת�ƽڵ㣬Ӱ��ڵ�������
        /// ת�Ƶ�ownerNode�µĵڼ����ڵ�λ�á�
        /// </summary>
        /// <param name="node"></param>
        public void MoveNode(DataRowTvaNode node, DataRowTvaNode ownerNode, int index)
        {

            if (node == null) throw new ArgumentNullException("node");
            if (ownerNode == null) throw new ArgumentNullException("ownerNode");


            Trace.TraceInformation("MoveNode {0} �� {1} ��", node, ownerNode);

            try
            {
                int from = -1;
                TreePath fromPath = GetPath(node);
                TreePath toPath = GetPath(ownerNode);
                DataRowTvaNode fromOwner = null;
                TreePath fromOwnerPath = null;

                if (!String.IsNullOrEmpty(node.TNA_PID))
                {
                    fromOwner = tnmDic[node.TNA_PID];
                    fromOwnerPath = GetPath(fromOwner);
                }
                else
                {
                    fromOwnerPath = TreePath.Empty;
                }


                if (node.TNA_PID != ownerNode.TNA_ID) //��ͬ��
                {
                    #region ����ƶ�
                    if (!String.IsNullOrEmpty(node.TNA_PID)) //�˽ڵ�Ǹ�
                    {
                        //��ԭ�����б����Ƴ�
                        from = p2cDic[node.TNA_PID].IndexOf(node);
                        p2cDic[node.TNA_PID].RemoveAt(from);
                        OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));//��Դ·���Ƴ�
                        if (p2cDic[node.TNA_PID].Count == 0)
                        {
                            Trace.TraceInformation("�ڵ�{0}���Ѿ����ӽڵ�,�Ƴ����б��ṹ�����仯", fromOwner);
                            p2cDic.Remove(fromOwner.TNA_ID);
                            OnStructureChanged(new TreeModelEventArgs(fromOwnerPath, new object[] { fromOwner }));
                        }
                        else
                        {
                            RefreshLogicId(fromOwner.TNA_ID);
                        }

                    }
                    else
                    {
                        from = rootList.IndexOf(node);
                        rootList.RemoveAt(from);//�Ӹ��������Ƴ�     
                        //��ʱfromOwnerPath=Empty
                        OnNodesRemoved(new TreeModelEventArgs(TreePath.Empty, new int[] { from }, new object[] { node }));
                        RefreshRootLogicId();
                    }
                    //���·ŵ�������              
                    node.TNA_PID = ownerNode.TNA_ID;//�ı丸��

                    if (p2cDic.ContainsKey(ownerNode.TNA_ID))//Ŀ��ڵ�ԭ�����Ƿ�Ҷ�ӽڵ㣬�к���
                    {
                        p2cDic[ownerNode.TNA_ID].Insert(index, node);
                        node.Owner = p2cDic[ownerNode.TNA_ID];
                        node.TNA_LogicId = GetLogicId(node);
                        //����ڷǸ��ڵ�����أ�
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        RefreshLogicId(ownerNode.TNA_ID);
                    }
                    else//Ŀ��ڵ�û����
                    {
                        p2cDic.Add(ownerNode.TNA_ID, new List<DataRowTvaNode>(new DataRowTvaNode[] { node }));
                        node.Owner = p2cDic[ownerNode.TNA_ID];
                        node.TNA_LogicId = String.Format(LOGIC_ID_FMT_2, ownerNode.TNA_LogicId, index + 1); 
                        Trace.TraceInformation("�ڵ��Ҷ�ӱ��֧�ڵ㣬�ṹ�����仯���ڵ���Ϣ��{0}", ownerNode);
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        OnStructureChanged(new TreeModelEventArgs(toPath, new object[] { ownerNode }));
                    }


                    #endregion
                }
                else
                {
                    #region ͬ�����
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    Trace.TraceInformation("Ҫת�Ƶ�Ŀ��λ�ú͵�ǰλ�ò���ͬ:ת�ƽڵ�={0},Ŀ��λ��={1}��ֻ�ı����from={2} to={3}", node, ownerNode, from, index);
                    if (from != index)
                    {
                        p2cDic[node.TNA_PID].Remove(node);
                        OnNodesRemoved(new TreeModelEventArgs(toPath, new int[] { from }, new object[] { node }));

                        p2cDic[node.TNA_PID].Insert(index, node); //����˴˽ڵ㵽�б���
                        node.TNA_LogicId = GetLogicId(node, index);
                        //ʵ����ͬ��������߼�ID��Ӧ�øı�
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        RefreshLogicId(ownerNode.TNA_ID);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                
                Trace.TraceError("Move���ִ���:{0}", ex);
            }



        }

        /// <summary>
        /// �ƶ�����
        /// </summary>
        /// <param name="node"></param>
        public void MoveToRoot(DataRowTvaNode node, int index)
        {
            if (node == null) throw new ArgumentNullException("node");
            try
            {
                TreePath ownerPath = TreePath.Empty;
                TreePath fromPath = GetPath(node);

                DataRowTvaNode fromOwner = null;
                TreePath fromOwnerPath = null;

                if (!String.IsNullOrEmpty(node.TNA_PID))
                {
                    fromOwner = tnmDic[node.TNA_PID];
                    fromOwnerPath = GetPath(fromOwner);
                }
                else
                {
                    fromOwnerPath = TreePath.Empty;
                }

                int from = -1;

                Trace.TraceInformation("MoveNode {0} ������.", node);
                if (String.IsNullOrEmpty(node.TNA_PID)) //ͬ�������
                {
                    from = rootList.IndexOf(node);
                    Trace.TraceInformation("Ҫת�Ƶ�Ŀ��λ�ú͵�ǰλ����ͬ:ת�ƽڵ�={0}���Ǹ��ڵ㣬ֻ�ı����.From={1},To={2}", node, from, index);
                    rootList.RemoveAt(from);
                    OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));

                }
                else //��������ת�Ƶ���
                {
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    p2cDic[fromOwner.TNA_ID].Remove(node);

                    if (p2cDic[fromOwner.TNA_ID].Count == 0)
                    {
                        Trace.TraceInformation("�ڵ�{0}���Ѿ����ӽڵ�.", fromOwner);
                        p2cDic.Remove(fromOwner.TNA_ID);
                        OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));
                        OnStructureChanged(new TreeModelEventArgs(fromOwnerPath, new object[] { fromOwner }));
                    }
                    else
                    {
                        OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));
                        RefreshLogicId(fromOwner.TNA_ID);
                    }

                }
                node.TNA_PID = null;//�ı丸�� 
                rootList.Insert(index, node);//�ƶ�����
                node.Owner = rootList;
                OnNodesInserted(new TreeModelEventArgs(ownerPath, new int[] { index }, new object[] { node }));
                //ˢ�¸ò���߼�ID
                RefreshRootLogicId();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Assert(ex != null, ex.Message);
            }

        }


        #endregion

        private void RefreshLogicId(string parentId)
        {
            string parentLogicId = tnmDic[parentId].TNA_LogicId; //���ڵ���߼�ID
            if (p2cDic.ContainsKey(parentId) && p2cDic[parentId].Count > 0)
            {
                IList<DataRowTvaNode> list = p2cDic[parentId];
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TNA_LogicId = String.Format(LOGIC_ID_FMT_2, parentLogicId, i + 1);
                    RefreshLogicId(list[i].TNA_ID);
                }
            }
        }

        private void RefreshRootLogicId()
        {
            for (int i = 0; i < rootList.Count; i++)
            {
                rootList[i].TNA_LogicId = GetRoolLogicId(i);
            }
            foreach (string key in tnmDic.Keys)
            {
                tnmDic[key].TNA_LogicId = GetLogicId(tnmDic[key]);
            }
        }

        private string GetLogicId(DataRowTvaNode node)
        {
            if (rootList.Contains(node))//������а���
            {
                if (rootList.Count == 1 && USE_SINGLE_SPECIAL_ROOT_ID)
                {
                    return SPECIALED_SIGNLE_ROOT_ID;
                }
                return String.Format(LOGIC_ID_FMT_1, node.TNA_Index + 1, ROOT_LOGIC_ID_PREFIX);
            }
            else //�Ǹ�
            {
                return String.Format(LOGIC_ID_FMT_2, tnmDic[node.TNA_PID].TNA_LogicId, node.TNA_Index + 1);
            }

        }
        private string GetLogicId(DataRowTvaNode node, int index)
        {
            if (rootList.Contains(node))
            {
                return GetRoolLogicId(index);
            }
            else
            {
                return String.Format(LOGIC_ID_FMT_2, tnmDic[node.TNA_PID].TNA_LogicId, index + 1);
            }
        }
        private string GetRoolLogicId(int index)
        {
            if (rootList.Count == 1 && USE_SINGLE_SPECIAL_ROOT_ID)
            {
                return SPECIALED_SIGNLE_ROOT_ID;
            }
            return String.Format(LOGIC_ID_FMT_1, index + 1, ROOT_LOGIC_ID_PREFIX);
        }

        /// <summary>
        /// �Ѹ���ͨ��������д��
        /// </summary>
        public int SyncToDb(bool force)
        {
            return tta.SyncToDb(tnmDic.Values, force);
        }

        /// <summary>
        /// �������ͬ����������ʱ����ʵ����ӡ�
        /// </summary>
        /// <returns></returns>
        public int SyncDataTable2TreeNodes()
        {
            treeTable.AcceptChanges();         
            this.nodesTotalCount = treeTable.Rows.Count;
            p2cDic.Clear();
            rootList.Clear();
            tnmDic.Clear();
            isLoadNodesComplete = false;
            //���¹�����
            Refresh();//���Ǹ���ʱ��Ĺ��̡�
            return tnmDic.Count;
        }
    }
}
