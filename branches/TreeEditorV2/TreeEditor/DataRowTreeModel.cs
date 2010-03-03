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
            original_tna_id = tna_id;//仅在构造函数中赋值
        }

        public void SyncDataRow2TvnNode()
        {
            tna_id = row[rowSchema.Tna_id_field_name].ToString();
            tna_pid = row[rowSchema.Tna_pid_field_name].ToString();
            tna_text = row[rowSchema.Tna_text_field_name].ToString();
        }


        #region ITvaNode 成员


        private readonly string original_tna_id;
        /// <summary>
        /// 原始的ID
        /// </summary>
        public string Original_tna_id
        {
            get
            {
                return original_tna_id;
            }

        }

        /// <summary>
        /// tna_id不直接使用 return row[rowSchema.Tna_id_field_name].ToString()的原因在于当Row被删除
        /// 时，此调用将出现错误。2009-12-20
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

        #region ICloneable 成员

        public object Clone()
        {
            DataRow newrow = row.Table.NewRow();
            row.ItemArray.CopyTo(newrow.ItemArray, 0);
            return new DataRowTvaNode(newrow, this.rowSchema);

        }

        #endregion

        /// <summary>
        /// 将逻辑ID赋给行的某个值
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

        

        #region 从配置文件获取
        /// <summary>
        /// 根的逻辑ID格式 [前缀][###]
        /// </summary>
        string LOGIC_ID_FMT_1 = "{1}{0,3:D3}";
        /// <summary>
        /// 支及叶子的逻辑ID格式
        /// </summary>
        string LOGIC_ID_FMT_2 = "{0,3:D3}{1,3:D3}";
        /// <summary>
        /// 根逻辑ID前缀
        /// </summary>
        string ROOT_LOGIC_ID_PREFIX = String.Empty;
        /// <summary>
        /// 仅使用指定的单根ID
        /// </summary>
        bool USE_SINGLE_SPECIAL_ROOT_ID = false;
        /// <summary>
        /// 给定特定的单根ID
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
        /// 根节点列表，在构造函数中自动初始化.
        /// </summary>
        private IList<DataRowTvaNode> rootList;

        /// <summary>
        /// 节点集合
        /// </summary>
        private Dictionary<string, DataRowTvaNode> tnmDic = null;
        //必须实现节点本地CRUD       
        /// <summary>
        /// 父节点ID，邻接孩子列表，这里父节点ID，不能为空。
        /// </summary>
        private Dictionary<string, IList<DataRowTvaNode>> p2cDic = new Dictionary<string, IList<DataRowTvaNode>>();


        private bool isLoadNodesComplete = false;

        public DataRowTreeModel(TvaSchema schema_)
        {
            TvaSchema schema = schema_;
            tta = new DataTableTreeAdapter(schema);
            #region 配置信息
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
        /// 使用适配器刷新Model.
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

            #region //这里自动备份
            if (is_auto_backup)
            {
                if (!System.IO.Directory.Exists(backup_path))
                {
                    System.IO.Directory.CreateDirectory(backup_path);
                }
                string backupFileName = System.IO.Path.Combine(backup_path, Guid.NewGuid().ToString("D") + ".xml");
                treeDs.WriteXml(backupFileName, XmlWriteMode.WriteSchema);
                Trace.TraceInformation("备份{0}到{1}", tta.GetType(), backupFileName);
            }
            #endregion
            treeTable = treeDs.Tables[0];
            nodesTotalCount = treeTable.Rows.Count;
           
            Refresh();
        }

      

        /// <summary>
        /// 添加节点到tnmDic中。
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
                Trace.TraceInformation("查询根节点:{0}", treePath.FirstNode);
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
                Trace.TraceInformation("查询{0}孩子节点", nm);
                if (p2cDic.ContainsKey(nm.TNA_ID))
                {
                    return p2cDic[nm.TNA_ID];
                }
                else //从DataTable中找，一次找出该节点的所有孩子
                {                  
                    DataRow[] rows = treeTable.Select(String.Format("{0}='{1}'", tta.Schema.Tna_pid_field_name, nm.TNA_ID));
                    IList<DataRowTvaNode> list = new List<DataRowTvaNode>(rows.Length);

                    for (int i = 0; i < rows.Length; i++)
                    {
                        DataRowTvaNode tn = new DataRowTvaNode(rows[i], tta.Schema);
                        tn.TNA_LogicId = String.Format(LOGIC_ID_FMT_2, nm.TNA_LogicId, i + 1);
                        //对所有行
                        list.Add(tn);
                    }
                    //list中返回孩子节点列表
                    AddTvaNode2tnmDic(list);
                    //增加了孩子列表
                    p2cDic.Add(nm.TNA_ID, list);
                    return list;
                }


            }
            
        }

        public override bool IsLeaf(TreePath treePath)
        {
            DataRowTvaNode nm = treePath.LastNode as DataRowTvaNode;
            Trace.TraceInformation("判定{0}是否叶子.", nm);

            if (isLoadNodesComplete)
            {
                return (p2cDic.ContainsKey(nm.TNA_ID) == false);
            }

          
            return treeTable.Select(String.Format("{0}='{1}'",
                        tta.Schema.Tna_pid_field_name, nm.TNA_ID)).Length == 0;
        }


        #region 节点移动

        /// <summary>
        /// 获取一个节点的路径
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreePath GetPath(DataRowTvaNode node)
        {
            if (rootList.Contains(node))//是根节点，则直接返回
            {
                return new TreePath(node);
            }
            else //非根
            {
                Stack<object> stack = new Stack<object>();
                stack.Push(node);
                while (p2cDic.ContainsKey(node.TNA_PID)) //在非根节点中找
                {
                    node = tnmDic[node.TNA_PID];//切换到其父亲
                    stack.Push(node);
                    if (String.IsNullOrEmpty(node.TNA_PID)) break;

                }
                return new TreePath(stack.ToArray());

            }

        }

        /// <summary>
        /// 删除叶子节点，不影响节点索引.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveLeafNode(DataRowTvaNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            Trace.TraceInformation("RemoveLeafNode {0} ", node);

            if (!String.IsNullOrEmpty(node.TNA_PID))
            {
                TreePath fromOwnerPath = GetPath(tnmDic[node.TNA_PID]);
                tnmDic.Remove(node.TNA_ID);//删除此节点
                treeTable.Rows.Remove(node.DataRow);
                p2cDic[node.TNA_PID].Remove(node);//从列表中也删除
                OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new DataRowTvaNode[] { node }));

                if (p2cDic[node.TNA_PID].Count == 0)
                {
                    Trace.TraceInformation("节点{0}下已经无子节点,移除空列表", tnmDic[node.TNA_PID]);
                    p2cDic.Remove(node.TNA_PID);
                    OnStructureChanged(new TreeModelEventArgs(fromOwnerPath, new object[] { tnmDic[node.TNA_PID] }));
                }
            }
            else
            {
                rootList.Remove(node);//如果它是根，则也删除
                tnmDic.Remove(node.TNA_ID);//删除此节点
                treeTable.Rows.Remove(node.DataRow);
                OnNodesRemoved(new TreeModelEventArgs(TreePath.Empty, new DataRowTvaNode[] { node }));
            }

        }

        /// <summary>
        /// 删除任意一个节点.此函数将重建树。
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
                Trace.TraceInformation("试图删除不存在的节点:{0}", node);
            }
        }

        /// <summary>
        /// 找出一个节点的所有孩子结点，含自己.
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
        /// 转移节点，影响节点索引。
        /// 转移到ownerNode下的第几个节点位置。
        /// </summary>
        /// <param name="node"></param>
        public void MoveNode(DataRowTvaNode node, DataRowTvaNode ownerNode, int index)
        {

            if (node == null) throw new ArgumentNullException("node");
            if (ownerNode == null) throw new ArgumentNullException("ownerNode");


            Trace.TraceInformation("MoveNode {0} 到 {1} 下", node, ownerNode);

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


                if (node.TNA_PID != ownerNode.TNA_ID) //不同层
                {
                    #region 跨层移动
                    if (!String.IsNullOrEmpty(node.TNA_PID)) //此节点非根
                    {
                        //从原父亲列表中移除
                        from = p2cDic[node.TNA_PID].IndexOf(node);
                        p2cDic[node.TNA_PID].RemoveAt(from);
                        OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));//从源路径移除
                        if (p2cDic[node.TNA_PID].Count == 0)
                        {
                            Trace.TraceInformation("节点{0}下已经无子节点,移除空列表，结构发生变化", fromOwner);
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
                        rootList.RemoveAt(from);//从根集合中移除     
                        //此时fromOwnerPath=Empty
                        OnNodesRemoved(new TreeModelEventArgs(TreePath.Empty, new int[] { from }, new object[] { node }));
                        RefreshRootLogicId();
                    }
                    //重新放到集合中              
                    node.TNA_PID = ownerNode.TNA_ID;//改变父亲

                    if (p2cDic.ContainsKey(ownerNode.TNA_ID))//目标节点原来就是非叶子节点，有孩子
                    {
                        p2cDic[ownerNode.TNA_ID].Insert(index, node);
                        node.Owner = p2cDic[ownerNode.TNA_ID];
                        node.TNA_LogicId = GetLogicId(node);
                        //如何在非根节点插入呢？
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        RefreshLogicId(ownerNode.TNA_ID);
                    }
                    else//目标节点没孩子
                    {
                        p2cDic.Add(ownerNode.TNA_ID, new List<DataRowTvaNode>(new DataRowTvaNode[] { node }));
                        node.Owner = p2cDic[ownerNode.TNA_ID];
                        node.TNA_LogicId = String.Format(LOGIC_ID_FMT_2, ownerNode.TNA_LogicId, index + 1); 
                        Trace.TraceInformation("节点从叶子变成支节点，结构发生变化。节点信息：{0}", ownerNode);
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        OnStructureChanged(new TreeModelEventArgs(toPath, new object[] { ownerNode }));
                    }


                    #endregion
                }
                else
                {
                    #region 同层调整
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    Trace.TraceInformation("要转移的目标位置和当前位置层相同:转移节点={0},目标位置={1}，只改变序号from={2} to={3}", node, ownerNode, from, index);
                    if (from != index)
                    {
                        p2cDic[node.TNA_PID].Remove(node);
                        OnNodesRemoved(new TreeModelEventArgs(toPath, new int[] { from }, new object[] { node }));

                        p2cDic[node.TNA_PID].Insert(index, node); //添加了此节点到列表中
                        node.TNA_LogicId = GetLogicId(node, index);
                        //实际上同层的其他逻辑ID都应该改变
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        RefreshLogicId(ownerNode.TNA_ID);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                
                Trace.TraceError("Move出现错误:{0}", ex);
            }



        }

        /// <summary>
        /// 移动到根
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

                Trace.TraceInformation("MoveNode {0} 到根下.", node);
                if (String.IsNullOrEmpty(node.TNA_PID)) //同根层调整
                {
                    from = rootList.IndexOf(node);
                    Trace.TraceInformation("要转移的目标位置和当前位置相同:转移节点={0}就是根节点，只改变序号.From={1},To={2}", node, from, index);
                    rootList.RemoveAt(from);
                    OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));

                }
                else //从其他层转移到根
                {
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    p2cDic[fromOwner.TNA_ID].Remove(node);

                    if (p2cDic[fromOwner.TNA_ID].Count == 0)
                    {
                        Trace.TraceInformation("节点{0}下已经无子节点.", fromOwner);
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
                node.TNA_PID = null;//改变父亲 
                rootList.Insert(index, node);//移动到根
                node.Owner = rootList;
                OnNodesInserted(new TreeModelEventArgs(ownerPath, new int[] { index }, new object[] { node }));
                //刷新该层的逻辑ID
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
            string parentLogicId = tnmDic[parentId].TNA_LogicId; //父节点的逻辑ID
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
            if (rootList.Contains(node))//如果根中包含
            {
                if (rootList.Count == 1 && USE_SINGLE_SPECIAL_ROOT_ID)
                {
                    return SPECIALED_SIGNLE_ROOT_ID;
                }
                return String.Format(LOGIC_ID_FMT_1, node.TNA_Index + 1, ROOT_LOGIC_ID_PREFIX);
            }
            else //非根
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
        /// 把更改通过适配器写回
        /// </summary>
        public int SyncToDb(bool force)
        {
            return tta.SyncToDb(tnmDic.Values, force);
        }

        /// <summary>
        /// 表格数据同步到树，这时可以实现添加。
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
            //重新构建树
            Refresh();//这是个长时间的过程。
            return tnmDic.Count;
        }
    }
}
