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
       
        }

    


        #region ITvaNode ��Ա


       
        
        /// <summary>
        /// ��ֵʱ��ͬrowһ��ֵ�����ˡ�
        /// ����ID����ԭ��
        /// </summary>
        public string TNA_ID
        {
            get
            {
                return row[rowSchema.Tna_id_field_name].ToString();
            }
            set
            {
                
                row[rowSchema.Tna_id_field_name] = value;
            }
        }

        
        /// <summary>
        /// ��ID
        /// </summary>
        public string TNA_PID
        {
            get
            {
                return row[rowSchema.Tna_pid_field_name].ToString();
            }
            set
            {                
                row[rowSchema.Tna_pid_field_name] = value;
            }
        }

     
        public string TNA_Text
        {
            get
            {
                return row[rowSchema.Tna_text_field_name].ToString();
            }
            set
            {
               
                row[rowSchema.Tna_text_field_name] = value;
            }
        }

        private string tna_LogicId;
        /// <summary>
        /// �༭��Ҫʹ�õ��߼�ID
        /// </summary>
        public string TNA_LogicId
        {
            get
            {
                return tna_LogicId;
            }
            set
            {
                tna_LogicId = value;
                //���������߼�ID�ֶΣ����Զ�ӳ����ȥ
                if (!String.IsNullOrEmpty(rowSchema.Tna_logic_id_map_field))
                {
                    row[rowSchema.Tna_logic_id_map_field] = value;
                }
            }
        }

      

        protected System.Drawing.Image icon;
        /// <summary>
        /// ͼ��
        /// </summary>
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
        /// <summary>
        /// ��ѡʱ�Ƿ�ѡ��
        /// </summary>
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

        const string Default_TVA_ROOT_KEY = "ROOT";

        bool is_auto_backup = true;

        string backup_path = System.IO.Path.Combine(Environment.CurrentDirectory, "backup");

        #endregion



        private int nodesTotalCount;



 

        private Dictionary<string, DataRowTvaNode> allNodesMap;
        /// <summary>
        /// ���нڵ㡣���ҽ���������ExpandAll֮�����������������нڵ㣻
        /// </summary>
        internal Dictionary<string, DataRowTvaNode> AllNodesMap
        {
            get { return allNodesMap; }

        }
             
        //����ʵ�ֽڵ㱾��CRUD       
        /// <summary>
        /// ���ڵ�ID���ڽӺ����б����︸�ڵ�ID������Ϊ�ա�
        /// </summary>
        private  Dictionary<string, DataRowTvaNode[]> treeNodesMap = new Dictionary<string, DataRowTvaNode[]>();

        public Dictionary<string, DataRowTvaNode[]> TreeNodesMap
        {
            get { return treeNodesMap; }
        }



        //���������ʱ��ʹ�ô˱����������нڵ�ı�����
        private DataTable dtTreeNodeMo;

        public DataTable DtTreeNodeMo
        {
            get { return dtTreeNodeMo; }
        }

        public DataRowTreeModel(TvaSchema schema_)
        {
            tta = new DataTableTreeAdapter(schema_);
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

            dtTreeNodeMo = tta.GetTreeNodeDataTable().Tables[0];
            InitTreeModelViaDataTable();
        }

        public DataRowTreeModel():this(new TvaSchema())
        {

        }

        
        /// <summary>
        /// ��ָ���ĸ������� , ����ͨ��ʵ�֣�����Ҫ������Դ����TreeEditor�ı�׼������
        /// �����߼����ID�ֶΣ����磺001001002... ��ʱ���߼����ID�ֶΣ����������ֶΡ�
        /// �����Ƽ���ô������Ϊ����һ����Ϊ��������ɸ��ġ�
        /// </summary>
        /// <param name="rootFilter"></param>
        private void InitTreeModelViaDataTable()
        {
            DataRow[] rootRows = dtTreeNodeMo.Select( tta.Schema.RowFilter_GetRootNodes );//Ĭ�ϸ�Ϊ������1�����

            treeNodesMap = new Dictionary<string, DataRowTvaNode[]>();
            allNodesMap = new Dictionary<string, DataRowTvaNode>(dtTreeNodeMo.Rows.Count);

            DataRowTvaNode[] roots = new DataRowTvaNode[rootRows.Length];
            for (int i = 0; i < rootRows.Length; i++)
            {
                roots[i] = new DataRowTvaNode(rootRows[i],tta.Schema);
                allNodesMap.Add(roots[i].TNA_ID, roots[i]);
                Trace.TraceInformation("��ʼ�����ڵ�{0}.", roots[i]);
            }
            lock (treeNodesMap)
            {
                treeNodesMap.Add(Default_TVA_ROOT_KEY, roots);
            }
                   
           
        }


        #region ��DB������
        /// <summary>
        /// ʹ��������ˢ��Model.
        /// </summary>
        public void RefreshFromAdapter()
        {
            Trace.WriteLine("��DB������.");
            nodesTotalCount = tta.GetNodesTotalCount();
            if (allNodesMap == null)
            {
                allNodesMap = new Dictionary<string, DataRowTvaNode>(nodesTotalCount);
            }
            else
            {
                allNodesMap.Clear();
            }

            treeNodesMap.Clear();
            DataSet treeDs = tta.GetTreeNodeDataTable();

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
            dtTreeNodeMo = treeDs.Tables[0];
            nodesTotalCount = dtTreeNodeMo.Rows.Count;            
            base.Refresh();
        }

        #endregion

        #region ������д�ĺ���
        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            lock (treeNodesMap)
            {
                if (treePath.IsEmpty())
                {
                    return treeNodesMap[Default_TVA_ROOT_KEY];
                }
                else
                {
                    DataRowTvaNode n = treePath.LastNode as DataRowTvaNode;//��ȡ���ĺ���
                    if (treeNodesMap.ContainsKey(n.TNA_ID))
                    {
                        return treeNodesMap[n.TNA_ID];
                    }
                    else
                    {
                        //�����е�ֱ������
                        DataRow[] rows = dtTreeNodeMo.Select(String.Format("{0}='{1}'",
                            tta.Schema.Tna_pid_field_name , n.TNA_ID));
                        if (rows != null && rows.Length > 0)
                        {
                            DataRowTvaNode[] nodeArray = new DataRowTvaNode[rows.Length];
                            for (int r = 0; r < rows.Length; r++)
                            {
                                nodeArray[r] = new DataRowTvaNode(rows[r], tta.Schema);
                                allNodesMap.Add(nodeArray[r].TNA_ID, nodeArray[r]);
                            }
                            lock (treeNodesMap)
                            {
                                treeNodesMap.Add(n.TNA_ID, nodeArray);
                            }
                            return nodeArray;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
            }
        }

        public override bool IsLeaf(TreePath treePath)
        {
            DataRowTvaNode n = treePath.LastNode as DataRowTvaNode;
            if (n == null) return true;
            return Convert.ToInt32(dtTreeNodeMo.Compute(String.Format("COUNT({0})", tta.Schema.Tna_id_field_name),
                String.Format("{0}='{1}'",tta.Schema.Tna_pid_field_name, n.TNA_ID))) == 0;
        }
        #endregion

        #region �������ͬ����������ʱ����ʵ����ӡ�
        /// <summary>
        /// �������ͬ����������ʱ����ʵ����ӡ�
        /// </summary>
        /// <returns></returns>
        public int SyncDataTable2TreeNodes()
        {
            dtTreeNodeMo.AcceptChanges();         
            this.nodesTotalCount = dtTreeNodeMo.Rows.Count;
            treeNodesMap.Clear();
          
            allNodesMap.Clear();
           
            //���¹�����
            Refresh();//���Ǹ���ʱ��Ĺ��̡�
            return allNodesMap.Count;
        }
        #endregion

        #region ����TVA���ɹ��õķ���



        #region ����½ڵ㵽��

        /// <summary>
        /// ����½ڵ㵽ĳ��ָ�����׵�ĳ��ָ��λ�á�
        /// </summary>
        /// <param name="node">�������Key���ı���</param>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataRowTvaNode Add(DataRowTvaNode node, TreePath parent, int index)
        {
            //��һ��Ҫ�ı�
            node.TNA_PID = (parent == TreePath.Empty) ? null : ((DataRowTvaNode)parent.LastNode).TNA_ID;
            //ȷ�����׼�
            string parentId = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((DataRowTvaNode)parent.LastNode).TNA_ID;

            DataRowTvaNode[] newArr = null;
            if (treeNodesMap.ContainsKey(parentId))
            {
                //ԭ���鳤��
                DataRowTvaNode[] oldArr = treeNodesMap[parentId];
                int oldLenth = oldArr.Length;
                newArr = new DataRowTvaNode[oldLenth + 1];
                Array.Copy(oldArr, 0, newArr, 0, index);
                newArr[index] = node;
                Trace.TraceInformation("Ϊ�ڵ㸳����������ParentKey����ʱΨһKey:{0}", node);
                if (newArr.Length > (index + 1)) //����ûcopy��
                {
                    Array.Copy(oldArr, index, newArr, index + 1, oldArr.Length - index);
                }

            }
            else
            {
                newArr = new DataRowTvaNode[] { node };
            }
            treeNodesMap[parentId] = newArr; //�������µ�����
            allNodesMap.Add(node.TNA_ID, node);//��ʱKey��GUID���������ظ�
            dtTreeNodeMo.Rows.Add(node.DataRow);
            OnStructureChanged(new TreeModelEventArgs(parent, new int[] { index }, new object[] { node }));

            return node;


        }
        #endregion

        #region ɾ���ڵ�
        /// <summary>
        /// ɾ��ĳ���ڵ��µ�ĳ���ӽڵ�
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        internal DataRowTvaNode RemoveNode(TreePath parent, DataRowTvaNode node)
        {
            //ȷ�����׼�
            string partentKey = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((DataRowTvaNode)parent.LastNode).TNA_ID;
            DataRowTvaNode[] oldArr = treeNodesMap[partentKey];
            if (oldArr.Length == 1) //ֻ��1�����ӣ���ô�϶���node
            {
                System.Diagnostics.Trace.Assert(node.TNA_ID == oldArr[0].TNA_ID);
                Trace.TraceInformation("��{0}��ֻ��1�����ӣ���ɾ����:{1}", partentKey, node);
                //ִ��ɾ��
                treeNodesMap.Remove(partentKey);
            }
            else
            {
                DataRowTvaNode[] newArr = new DataRowTvaNode[oldArr.Length - 1];
                int removedNodeIndex = -1;
                for (int i = 0; i < oldArr.Length; i++)
                {
                    if (oldArr[i] == node || oldArr[i].TNA_ID == node.TNA_ID)
                    {
                        removedNodeIndex = i;
                        break;
                    }

                }
                Array.Copy(oldArr, 0, newArr, 0, removedNodeIndex);
                if (removedNodeIndex < (newArr.Length - 1)) //��ĩ�ڵ�
                {
                    Array.Copy(oldArr, removedNodeIndex + 1,
                        newArr, removedNodeIndex, oldArr.Length - removedNodeIndex); //copyʣ�µ�
                }
                //���Ѿ�ִ��ɾ��������Ż�ӳ���
                treeNodesMap[partentKey] = newArr;

            }
            //�������
            allNodesMap.Remove((string)node.TNA_ID);
            DataRow[] rows = dtTreeNodeMo.Select(String.Format("{1}='{0}'", node.TNA_ID, tta.Schema.Tna_id_field_name));
            if (rows.Length == 1) dtTreeNodeMo.Rows.Remove(rows[0]);
            OnNodesRemoved(new TreeModelEventArgs(parent, new object[] { node }));
            return node;
        }
        /// <summary>
        /// ����ɾ��ʱʹ�ã���һ���ڵ㳹��ɾ��
        /// </summary>
        /// <param name="node"></param>
        internal void RemoveNodeCssWithRow(DataRowTvaNode node)
        {
            if (treeNodesMap.ContainsKey((string)node.TNA_ID))
            {
                treeNodesMap.Remove((string)node.TNA_ID);
            }
            allNodesMap.Remove((string)node.TNA_ID);
            DataRow[] rows = dtTreeNodeMo.Select(String.Format("{1}='{0}'", node.TNA_ID, tta.Schema.Tna_id_field_name));
            if (rows.Length == 1) dtTreeNodeMo.Rows.Remove(rows[0]);
        }

        /// <summary>
        /// ɾ���ִ��һ���ڵ㡣ɾ��ʵ���ǴӺ��Ӽ������Ƴ���
        /// </summary>
        /// <param name="parent">���ڵ�·��.</param>
        /// <param name="node">��ɾ���Ľڵ㣬ֻ����Ҷ�ӡ�</param>
        public void Remove(TreePath parent, DataRowTvaNode node)
        {
            Stack<DataRowTvaNode> removed = new Stack<DataRowTvaNode>();
            GetChildCss(node, removed);
            while (removed.Count > 1)
            {
                DataRowTvaNode n = removed.Pop();
                RemoveNodeCssWithRow(n);
            }
            RemoveNode(parent, node); //ɾ������ڵ㣬����Ҳһ��ɾ����

        }





        internal void GetChildCss(DataRowTvaNode node, Stack<DataRowTvaNode> stack)
        {
            stack.Push(node);
            if (treeNodesMap.ContainsKey((string)node.TNA_ID))
            {
                foreach (DataRowTvaNode n in treeNodesMap[(string)node.TNA_ID])
                {
                    GetChildCss(n, stack);
                }
            }

        }


        #endregion

        #region �ƶ��ڵ�
        /// <summary>
        /// �ƶ���֪��node�ڵ㵽��һ���ڵ�֮�¡�
        /// </summary>
        /// <param name="fromPath">�������ƶ�</param>
        /// <param name="node">���ƶ��Ľڵ�</param>
        /// <param name="toPath">�ƶ���Ŀ�ĵط�</param>
        /// <param name="index">λ��Ŀ�ĵط���ͬ�㺢�ӵڼ�����</param>
        /// <returns>�������ձ��ƶ�����λ�õĽڵ�</returns>
        public DataRowTvaNode Move(TreePath fromPath, DataRowTvaNode node, TreePath toPath, int index)
        {
            //��ɾ��            
            return Add(RemoveNode(fromPath, node), toPath, index);

        }
        #endregion

        #region ��ȫ����ID��������
        /// <summary>
        /// �ݹ���º��ӵ��߼�ID
        /// </summary>
        /// <param name="node"></param>
        internal void UpdateChildLogicId(DataRowTvaNode node)
        {
            if (treeNodesMap.ContainsKey(node.TNA_ID))//���к���
            {
                DataRowTvaNode[] child = treeNodesMap[node.TNA_ID];
                for (int i = 0; i < child.Length; i++)
                {
                    child[i].TNA_LogicId = String.Format("{0}{1,3:D3}", node.TNA_LogicId, i + 1);
                    dtTreeNodeMo.Select(String.Format("{1}='{0}'", child[i].TNA_ID, tta.Schema.Tna_id_field_name))[0][tta.Schema.Tna_logic_id_map_field] = child[i].TNA_LogicId;
                    UpdateChildLogicId(child[i]);
                }
            }
        }


        /// <summary>
        /// ��������ID���߼�ID��������������DB�С�
        /// ��������ɵ���������ӵ�ǰ����
        /// </summary>
        public void FullUpdateIDAndSave()
        {
            if (allNodesMap.Count > 0)
            {
                allNodesMap.Clear();
                DataRowTvaNode[] root = treeNodesMap[Default_TVA_ROOT_KEY];
                //������ݿ��оõ�Key
                StringBuilder sbdel = new StringBuilder();
                sbdel.Append(" WHERE ");
                for (int i = 0; i < root.Length; i++)
                {
                    sbdel.AppendFormat(" {1} LIKE '{0}%' ", root[i].TNA_ID, tta.Schema.Tna_id_field_name);
                    if (i < root.Length - 1) sbdel.Append(" OR ");
                }
                //����ɾ���ɵļ�¼
                Trace.TraceInformation("ɾ�����������ϼ����ӽڵ�:{0}", sbdel.ToString());
                for (int i = 0; i < root.Length; i++)
                {
                    root[i].TNA_LogicId = String.Format("{0,3:D3}", i + 1);
                    DataRow row = dtTreeNodeMo.Select(String.Format("{1}='{0}'", root[i].TNA_ID, tta.Schema.Tna_id_field_name))[0];
                    row[tta.Schema.Tna_logic_id_map_field] = root[i].TNA_LogicId;

                    allNodesMap.Add(root[i].TNA_LogicId, root[i]);
                    UpdateChildLogicId(root[i]);//���º��ӽ����߼�Key
                    root[i].TNA_ID = root[i].TNA_LogicId;//���߼�Keyͬ������Key
                    row[tta.Schema.Tna_id_field_name] = root[i].TNA_ID;
                }
                foreach (DataRow row in dtTreeNodeMo.Rows)
                {
                    row[tta.Schema.Tna_id_field_name] = row[tta.Schema.Tna_logic_id_map_field];//�߼�ID��ֵ����������ID��
                    string lkey = row[tta.Schema.Tna_logic_id_map_field].ToString();
                    if (lkey.Length > 3)
                    {
                        row[tta.Schema.Tna_pid_field_name] = lkey.Substring(0, lkey.Length - 3);
                    }
                    else
                    {
                        row[tta.Schema.Tna_pid_field_name] = null;
                    }
                }
                dtTreeNodeMo.AcceptChanges();//���е��߼�ID������ϣ���dtҲȫ��ͬ����map��ֻ��root�ڵ�    
                //��dt�洢������
                //da.SaveTree(sbdel.ToString(), dtTreeNodeMo);
                //TODO:�������浽DB

                treeNodesMap.Clear();
                treeNodesMap.Add(Default_TVA_ROOT_KEY, root);
                OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            }

        }
        #endregion
        #endregion
         
    }
}
