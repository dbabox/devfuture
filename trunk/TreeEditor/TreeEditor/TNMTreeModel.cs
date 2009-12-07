/*TreeEditor\TreeEditor\TNMTreeModel.cs
 * 将各个适配器装配起来的树模型。MVC模式中的M。
 * 
 * 2009-12-04 SZJ
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls.Tree;
using TreeEditor.Core;
using Common.Logging;
using System.Data;

namespace TreeEditor
{
    public class TNMTreeModel : TreeModelBase
    {
        static readonly ILog log = LogManager.GetCurrentClassLogger();

        #region 从配置文件获取
        /// <summary>
        /// 根的逻辑ID格式
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
        bool USE_SINGLE_ROOT_ID = true;
        /// <summary>
        /// 给定特定的单根ID
        /// </summary>
        string SIGNLE_ROOT_ID = "2009";
        #endregion



        private int nodesTotalCount;
        /// <summary>
        /// 用于访问底层数据源
        /// </summary>
        private ITreeTableAdapter tta;

        private DataSet treeDs;
        private DataTable treeTable;

        public DataTable TreeTable
        {
            get { return treeTable; }
            set { treeTable = value; }
        }

        private TNMTreeModel()
        {
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
                SIGNLE_ROOT_ID = System.Configuration.ConfigurationManager.AppSettings["SIGNLE_ROOT_ID"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["USE_SINGLE_ROOT_ID"]))
            {
                string boolstr = System.Configuration.ConfigurationManager.AppSettings["USE_SINGLE_ROOT_ID"];
                if (bool.TryParse(boolstr, out USE_SINGLE_ROOT_ID) == false)
                {
                    int intvalue = 0;
                    if (Int32.TryParse(boolstr, out intvalue) == true)
                    {
                        USE_SINGLE_ROOT_ID = intvalue != 0;
                    }
                }
                else
                {
                    USE_SINGLE_ROOT_ID = false;
                }
            }
            #endregion

        }


        public TNMTreeModel(ITreeTableAdapter tta_):this()
        {
            tta = tta_;             
        }

        /// <summary>
        /// 使用适配器刷新Model.
        /// </summary>
        public void RefreshFromAdapter()
        {
            nodesTotalCount = tta.GetNodesTotalCount();
            if (tnmDic == null)
            {
                tnmDic = new Dictionary<string, ITvaNode>(nodesTotalCount);
            }
            else
            {
                tnmDic.Clear();               
            }
            
            p2cDic.Clear();            
            treeDs = tta.GetTreeNodeDataTable();
            treeTable = treeDs.Tables[0];
            rootList = tta.GetRootNodes();
            for (int i = 0; i < rootList.Count; i++)
            {
                rootList[i].TNA_LogicId = GetRoolLogicId(i);
            }
            
            nodesTotalCount = tta.GetNodesTotalCount();
            AddTvaNode2tnmDic(rootList);
            Refresh();
        }


        /// <summary>
        /// 把更改通过适配器写回
        /// </summary>
        public int SyncViaAdapter(bool force)
        {
            return tta.SyncToDb(tnmDic.Values,force);
            //tta.SyncToDb(treeDs);
            //treeTable.AcceptChanges();
        }

        /// <summary>
        /// 将树节点同步到DataTable
        /// </summary>
        public int SyncTreeNodes2DataTable()
        {            
            return tta.SyncTreeNodes2DataTable(tnmDic, treeTable);           
        }

        public int SyncDataTable2TreeNodesDic()
        {
            try
            {
                return tta.SyncDataTable2TreeNodes(treeTable, tnmDic);
            }
            finally
            {
                Refresh();
            }
        }


       
        /// <summary>
        /// 已经废弃
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rcContainer"></param>
        /// <param name="startLevel"></param>
        [Obsolete("已经废弃，规格化耗时太久")]
        private void NormalizeChildNodes(ITvaNode node, Stack<ITvaNode> rcContainer, int startLevel)
        {
            node.TNA_Level = startLevel;
            node.TNA_LogicId = GetLogicId(node);
            rcContainer.Push(node);
            if (p2cDic.ContainsKey(node.TNA_ID))
            {
                foreach (ITvaNode n in p2cDic[node.TNA_ID])
                {
                    NormalizeChildNodes(n, rcContainer,startLevel+1);
                }
            }
        }




        #region 获取逻辑ID
        private string GetLogicId(ITvaNode node)
        {
            if (rootList.Contains(node))//如果根中包含
            {
                if (rootList.Count == 1 && USE_SINGLE_ROOT_ID)
                {
                    return SIGNLE_ROOT_ID;
                }
                return String.Format(LOGIC_ID_FMT_1, node.TNA_Index + 1, ROOT_LOGIC_ID_PREFIX);
            }
            else //非根
            {
                return String.Format(LOGIC_ID_FMT_2, tnmDic[node.TNA_PID].TNA_LogicId, node.TNA_Index + 1);
            }
           
        }

        private string GetLogicId(ITvaNode node,int index)
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

        private string GetLogicId(string parentLogicId, int index)
        {
            return String.Format(LOGIC_ID_FMT_2, parentLogicId, index + 1);
        }

        private string GetRoolLogicId(int index)
        {
            if (rootList.Count == 1 && USE_SINGLE_ROOT_ID)
            {
                return SIGNLE_ROOT_ID;
            }
            return String.Format(LOGIC_ID_FMT_1, index + 1, ROOT_LOGIC_ID_PREFIX);
        }
        #endregion



        /// <summary>
        /// 根节点列表，在构造函数中自动初始化.
        /// </summary>
        private IList<ITvaNode> rootList;

        /// <summary>
        /// 节点集合
        /// </summary>
        private Dictionary<string, ITvaNode> tnmDic =null;
        //必须实现节点本地CRUD       
        /// <summary>
        /// 父节点ID，邻接孩子列表，这里父节点ID，不能为空。
        /// </summary>
        private Dictionary<string, IList<ITvaNode>> p2cDic = new Dictionary<string, IList<ITvaNode>>();


        private bool isLoadNodesComplete = false;
        /// <summary>
        /// 添加节点到tnmDic中。
        /// </summary>
        /// <param name="nodes"></param>
        private void AddTvaNode2tnmDic(IList<ITvaNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Owner = nodes;                 
                tnmDic.Add(nodes[i].TNA_ID, nodes[i]);
            }
          
        }

        #region 接口实现
        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                log.DebugFormat("查询根节点:{0}", treePath.FirstNode);
                return rootList;
            }
            else
            {

                ITvaNode nm = treePath.LastNode as ITvaNode;
                log.DebugFormat("查询{0}孩子节点", nm);
                if (p2cDic.ContainsKey(nm.TNA_ID))
                {
                    return p2cDic[nm.TNA_ID];
                }
                else //从DataTable中找，一次找出该节点的所有孩子
                {
                    //IList<ITvaNode> list = tta.GetNextChildTreeNodes(nm);                    
                    //从treeTable中找，以便减少数据库操作
                    DataRow[] rows = treeTable.Select(String.Format("{0}='{1}'",tta.ParentIdFieldName, nm.TNA_ID));
                    IList<ITvaNode> list = new List<ITvaNode>(rows.Length);

                    for (int i = 0; i < rows.Length;i++ )
                    {
                        ITvaNode tn = tta.Row2TvaNode(rows[i]);
                        tn.TNA_LogicId = GetLogicId(nm.TNA_LogicId, i);
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
            ITvaNode nm = treePath.LastNode as ITvaNode;
            log.DebugFormat("判定{0}是否叶子.", nm);

            if (isLoadNodesComplete)
            {
                return (p2cDic.ContainsKey(nm.TNA_ID) == false);
            }

            //return tta.IsHasChild(nm) == false;
            //从TreeTable中解决
            return treeTable.Select(String.Format("{0}='{1}'",
                        tta.ParentIdFieldName, nm.TNA_ID)).Length == 0;
        }
        #endregion

        #region 节点移动

        /// <summary>
        /// 获取一个节点的路径
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreePath GetPath(ITvaNode node)
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
        public void RemoveLeafNode(ITvaNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            log.DebugFormat("RemoveLeafNode {0} ", node);

            if (tnmDic.ContainsKey(node.TNA_ID))
            {
                if (p2cDic.ContainsKey(node.TNA_ID))
                {
                    log.WarnFormat("此函数非叶子节点不允许删除{0}.删除任意节点请使用RemoveNode。", node);
                    return;
                }
                
                //if (p2cDic.ContainsKey(node.ID))
                //{
                //    //这么删除，造成了大量悬浮节点（即父节点不存在了），所以目前只允许删除叶子
                //    child = new ITvaNode[p2cDic[node.ID].Count+1];
                //    child[0] = node;
                //    p2cDic[node.ID].CopyTo(child, 1);
                //}
                //else
                //{
                //    child = new ITvaNode[] { node };
                //}


                //p2cDic.Remove(node.ID);//TODO:同时删除它的孩子，孩子的孩子，将来在另外的函数中实现吧.091203

                if (!String.IsNullOrEmpty(node.TNA_PID))
                {
                    TreePath fromOwnerPath = GetPath(tnmDic[node.TNA_PID]);
                    tnmDic.Remove(node.TNA_ID);//删除此节点
                    

                    p2cDic[node.TNA_PID].Remove(node);//从列表中也删除
                    OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new ITvaNode[] { node }));

                    if (p2cDic[node.TNA_PID].Count == 0)
                    {                        
                        log.DebugFormat("节点{0}下已经无子节点,移除空列表", tnmDic[node.TNA_PID]);
                        p2cDic.Remove(node.TNA_PID);                        
                        OnStructureChanged(new TreeModelEventArgs(fromOwnerPath, new object[] { tnmDic[node.TNA_PID] }));
                    }
                }
                else
                {
                    rootList.Remove(node);//如果它是根，则也删除
                    OnNodesRemoved(new TreeModelEventArgs(TreePath.Empty, new ITvaNode[] { node }));
                }

                

            }
            else
            {
                log.WarnFormat("试图删除不存在的节点:{0}", node);
               
            }
        }

        /// <summary>
        /// 删除任意一个节点.此函数将重建树。
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(ITvaNode node)
        {
            if (tnmDic.ContainsKey(node.TNA_ID))
            {
                Stack<ITvaNode> rcContainer = new Stack<ITvaNode>(nodesTotalCount);
                FindChild(node, rcContainer);
                ITvaNode[] nodes = rcContainer.ToArray();
                for (int i = 0; i < nodes.Length; i++)
                {
                    RemoveLeafNode(nodes[i]);
                }
                Refresh();
            }
            else
            {
                log.WarnFormat("试图删除不存在的节点:{0}", node);
            }
        }

        /// <summary>
        /// 找出一个节点的所有孩子结点，含自己.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rcContainer"></param>
        private void FindChild(ITvaNode node, Stack<ITvaNode> rcContainer)
        {
            rcContainer.Push(node);
            if (p2cDic.ContainsKey(node.TNA_ID))
            {
                foreach (ITvaNode n in p2cDic[node.TNA_ID])
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
        public void MoveNode(ITvaNode node, ITvaNode ownerNode, int index)
        {

            if (node == null) throw new ArgumentNullException("node");
            if (ownerNode == null) throw new ArgumentNullException("ownerNode");


            log.DebugFormat("MoveNode {0} 到 {1} 下", node, ownerNode);

            try
            {
                int from = -1;
                TreePath fromPath = GetPath(node);
                TreePath toPath = GetPath(ownerNode);
                ITvaNode fromOwner = null;
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
                            log.DebugFormat("节点{0}下已经无子节点,移除空列表，结构发生变化", fromOwner);
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
                        p2cDic.Add(ownerNode.TNA_ID, new List<ITvaNode>(new ITvaNode[] { node }));
                        node.Owner = p2cDic[ownerNode.TNA_ID];
                        node.TNA_LogicId = GetLogicId(ownerNode.TNA_LogicId, index);
                        log.DebugFormat("节点从叶子变成支节点，结构发生变化。节点信息：{0}", ownerNode);
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        OnStructureChanged(new TreeModelEventArgs(toPath, new object[] { ownerNode }));
                    }


                    #endregion
                }
                else
                {
                    #region 同层调整
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    log.DebugFormat("要转移的目标位置和当前位置层相同:转移节点={0},目标位置={1}，只改变序号from={2} to={3}", node, ownerNode, from, index);
                    if (from != index)
                    {
                        p2cDic[node.TNA_PID].Remove(node);
                        OnNodesRemoved(new TreeModelEventArgs(toPath, new int[] { from }, new object[] { node }));
                      
                        p2cDic[node.TNA_PID].Insert(index, node); //添加了此节点到列表中
                        node.TNA_LogicId = GetLogicId(node,index);
                        //实际上同层的其他逻辑ID都应该改变
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        RefreshLogicId(ownerNode.TNA_ID);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error("Move出现错误", ex);    
            }



        }

        /// <summary>
        /// 移动到根
        /// </summary>
        /// <param name="node"></param>
        public void MoveToRoot(ITvaNode node, int index)
        {
            if (node == null) throw new ArgumentNullException("node");
            try
            {
                TreePath ownerPath = TreePath.Empty;
                TreePath fromPath = GetPath(node);

                ITvaNode fromOwner = null;
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

                log.DebugFormat("MoveNode {0} 到根下.", node);
                if (String.IsNullOrEmpty(node.TNA_PID)) //同根层调整
                {
                    from = rootList.IndexOf(node);
                    log.DebugFormat("要转移的目标位置和当前位置相同:转移节点={0}就是根节点，只改变序号.From={1},To={2}", node, from, index);
                    rootList.RemoveAt(from);
                    OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));

                }
                else //从其他层转移到根
                {
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    p2cDic[fromOwner.TNA_ID].Remove(node);

                    if (p2cDic[fromOwner.TNA_ID].Count == 0)
                    {
                        log.DebugFormat("节点{0}下已经无子节点.", fromOwner);
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
            string parentLogicId=tnmDic[parentId].TNA_LogicId;
            IList<ITvaNode> list = p2cDic[parentId];
            for (int i = 0; i < list.Count; i++)
            {
                list[i].TNA_LogicId = GetLogicId(parentId, i);
            }
        }

        private void RefreshRootLogicId()
        {
            for (int i = 0; i < rootList.Count; i++)
            {
                rootList[i].TNA_LogicId = GetRoolLogicId(i);
            }
        }


    }
}
