/*TreeEditor\TreeEditor\TNMTreeModel.cs
 * ������������װ����������ģ�͡�MVCģʽ�е�M��
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

        #region �������ļ���ȡ
        /// <summary>
        /// �����߼�ID��ʽ
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
        bool USE_SINGLE_ROOT_ID = true;
        /// <summary>
        /// �����ض��ĵ���ID
        /// </summary>
        string SIGNLE_ROOT_ID = "2009";
        #endregion



        private int nodesTotalCount;
        /// <summary>
        /// ���ڷ��ʵײ�����Դ
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
        /// ʹ��������ˢ��Model.
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
        /// �Ѹ���ͨ��������д��
        /// </summary>
        public int SyncViaAdapter(bool force)
        {
            return tta.SyncToDb(tnmDic.Values,force);
            //tta.SyncToDb(treeDs);
            //treeTable.AcceptChanges();
        }

        /// <summary>
        /// �����ڵ�ͬ����DataTable
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
        /// �Ѿ�����
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rcContainer"></param>
        /// <param name="startLevel"></param>
        [Obsolete("�Ѿ���������񻯺�ʱ̫��")]
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




        #region ��ȡ�߼�ID
        private string GetLogicId(ITvaNode node)
        {
            if (rootList.Contains(node))//������а���
            {
                if (rootList.Count == 1 && USE_SINGLE_ROOT_ID)
                {
                    return SIGNLE_ROOT_ID;
                }
                return String.Format(LOGIC_ID_FMT_1, node.TNA_Index + 1, ROOT_LOGIC_ID_PREFIX);
            }
            else //�Ǹ�
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
        /// ���ڵ��б��ڹ��캯�����Զ���ʼ��.
        /// </summary>
        private IList<ITvaNode> rootList;

        /// <summary>
        /// �ڵ㼯��
        /// </summary>
        private Dictionary<string, ITvaNode> tnmDic =null;
        //����ʵ�ֽڵ㱾��CRUD       
        /// <summary>
        /// ���ڵ�ID���ڽӺ����б����︸�ڵ�ID������Ϊ�ա�
        /// </summary>
        private Dictionary<string, IList<ITvaNode>> p2cDic = new Dictionary<string, IList<ITvaNode>>();


        private bool isLoadNodesComplete = false;
        /// <summary>
        /// ��ӽڵ㵽tnmDic�С�
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

        #region �ӿ�ʵ��
        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (treePath.IsEmpty())
            {
                log.DebugFormat("��ѯ���ڵ�:{0}", treePath.FirstNode);
                return rootList;
            }
            else
            {

                ITvaNode nm = treePath.LastNode as ITvaNode;
                log.DebugFormat("��ѯ{0}���ӽڵ�", nm);
                if (p2cDic.ContainsKey(nm.TNA_ID))
                {
                    return p2cDic[nm.TNA_ID];
                }
                else //��DataTable���ң�һ���ҳ��ýڵ�����к���
                {
                    //IList<ITvaNode> list = tta.GetNextChildTreeNodes(nm);                    
                    //��treeTable���ң��Ա�������ݿ����
                    DataRow[] rows = treeTable.Select(String.Format("{0}='{1}'",tta.ParentIdFieldName, nm.TNA_ID));
                    IList<ITvaNode> list = new List<ITvaNode>(rows.Length);

                    for (int i = 0; i < rows.Length;i++ )
                    {
                        ITvaNode tn = tta.Row2TvaNode(rows[i]);
                        tn.TNA_LogicId = GetLogicId(nm.TNA_LogicId, i);
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
            ITvaNode nm = treePath.LastNode as ITvaNode;
            log.DebugFormat("�ж�{0}�Ƿ�Ҷ��.", nm);

            if (isLoadNodesComplete)
            {
                return (p2cDic.ContainsKey(nm.TNA_ID) == false);
            }

            //return tta.IsHasChild(nm) == false;
            //��TreeTable�н��
            return treeTable.Select(String.Format("{0}='{1}'",
                        tta.ParentIdFieldName, nm.TNA_ID)).Length == 0;
        }
        #endregion

        #region �ڵ��ƶ�

        /// <summary>
        /// ��ȡһ���ڵ��·��
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreePath GetPath(ITvaNode node)
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
        public void RemoveLeafNode(ITvaNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            log.DebugFormat("RemoveLeafNode {0} ", node);

            if (tnmDic.ContainsKey(node.TNA_ID))
            {
                if (p2cDic.ContainsKey(node.TNA_ID))
                {
                    log.WarnFormat("�˺�����Ҷ�ӽڵ㲻����ɾ��{0}.ɾ������ڵ���ʹ��RemoveNode��", node);
                    return;
                }
                
                //if (p2cDic.ContainsKey(node.ID))
                //{
                //    //��ôɾ��������˴��������ڵ㣨�����ڵ㲻�����ˣ�������Ŀǰֻ����ɾ��Ҷ��
                //    child = new ITvaNode[p2cDic[node.ID].Count+1];
                //    child[0] = node;
                //    p2cDic[node.ID].CopyTo(child, 1);
                //}
                //else
                //{
                //    child = new ITvaNode[] { node };
                //}


                //p2cDic.Remove(node.ID);//TODO:ͬʱɾ�����ĺ��ӣ����ӵĺ��ӣ�����������ĺ�����ʵ�ְ�.091203

                if (!String.IsNullOrEmpty(node.TNA_PID))
                {
                    TreePath fromOwnerPath = GetPath(tnmDic[node.TNA_PID]);
                    tnmDic.Remove(node.TNA_ID);//ɾ���˽ڵ�
                    

                    p2cDic[node.TNA_PID].Remove(node);//���б���Ҳɾ��
                    OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new ITvaNode[] { node }));

                    if (p2cDic[node.TNA_PID].Count == 0)
                    {                        
                        log.DebugFormat("�ڵ�{0}���Ѿ����ӽڵ�,�Ƴ����б�", tnmDic[node.TNA_PID]);
                        p2cDic.Remove(node.TNA_PID);                        
                        OnStructureChanged(new TreeModelEventArgs(fromOwnerPath, new object[] { tnmDic[node.TNA_PID] }));
                    }
                }
                else
                {
                    rootList.Remove(node);//������Ǹ�����Ҳɾ��
                    OnNodesRemoved(new TreeModelEventArgs(TreePath.Empty, new ITvaNode[] { node }));
                }

                

            }
            else
            {
                log.WarnFormat("��ͼɾ�������ڵĽڵ�:{0}", node);
               
            }
        }

        /// <summary>
        /// ɾ������һ���ڵ�.�˺������ؽ�����
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
                log.WarnFormat("��ͼɾ�������ڵĽڵ�:{0}", node);
            }
        }

        /// <summary>
        /// �ҳ�һ���ڵ�����к��ӽ�㣬���Լ�.
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
        /// ת�ƽڵ㣬Ӱ��ڵ�������
        /// ת�Ƶ�ownerNode�µĵڼ����ڵ�λ�á�
        /// </summary>
        /// <param name="node"></param>
        public void MoveNode(ITvaNode node, ITvaNode ownerNode, int index)
        {

            if (node == null) throw new ArgumentNullException("node");
            if (ownerNode == null) throw new ArgumentNullException("ownerNode");


            log.DebugFormat("MoveNode {0} �� {1} ��", node, ownerNode);

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
                            log.DebugFormat("�ڵ�{0}���Ѿ����ӽڵ�,�Ƴ����б��ṹ�����仯", fromOwner);
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
                        p2cDic.Add(ownerNode.TNA_ID, new List<ITvaNode>(new ITvaNode[] { node }));
                        node.Owner = p2cDic[ownerNode.TNA_ID];
                        node.TNA_LogicId = GetLogicId(ownerNode.TNA_LogicId, index);
                        log.DebugFormat("�ڵ��Ҷ�ӱ��֧�ڵ㣬�ṹ�����仯���ڵ���Ϣ��{0}", ownerNode);
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        OnStructureChanged(new TreeModelEventArgs(toPath, new object[] { ownerNode }));
                    }


                    #endregion
                }
                else
                {
                    #region ͬ�����
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    log.DebugFormat("Ҫת�Ƶ�Ŀ��λ�ú͵�ǰλ�ò���ͬ:ת�ƽڵ�={0},Ŀ��λ��={1}��ֻ�ı����from={2} to={3}", node, ownerNode, from, index);
                    if (from != index)
                    {
                        p2cDic[node.TNA_PID].Remove(node);
                        OnNodesRemoved(new TreeModelEventArgs(toPath, new int[] { from }, new object[] { node }));
                      
                        p2cDic[node.TNA_PID].Insert(index, node); //����˴˽ڵ㵽�б���
                        node.TNA_LogicId = GetLogicId(node,index);
                        //ʵ����ͬ��������߼�ID��Ӧ�øı�
                        OnNodesInserted(new TreeModelEventArgs(toPath, new int[] { index }, new object[] { node }));
                        RefreshLogicId(ownerNode.TNA_ID);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.Error("Move���ִ���", ex);    
            }



        }

        /// <summary>
        /// �ƶ�����
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

                log.DebugFormat("MoveNode {0} ������.", node);
                if (String.IsNullOrEmpty(node.TNA_PID)) //ͬ�������
                {
                    from = rootList.IndexOf(node);
                    log.DebugFormat("Ҫת�Ƶ�Ŀ��λ�ú͵�ǰλ����ͬ:ת�ƽڵ�={0}���Ǹ��ڵ㣬ֻ�ı����.From={1},To={2}", node, from, index);
                    rootList.RemoveAt(from);
                    OnNodesRemoved(new TreeModelEventArgs(fromOwnerPath, new int[] { from }, new object[] { node }));

                }
                else //��������ת�Ƶ���
                {
                    from = p2cDic[node.TNA_PID].IndexOf(node);
                    p2cDic[fromOwner.TNA_ID].Remove(node);

                    if (p2cDic[fromOwner.TNA_ID].Count == 0)
                    {
                        log.DebugFormat("�ڵ�{0}���Ѿ����ӽڵ�.", fromOwner);
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
