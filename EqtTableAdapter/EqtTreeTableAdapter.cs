using System;
using System.Collections.Generic;
using System.Text;

using TreeEditor.Core;

using EQT.Model;
using EQT.Dal;
using System.Data;
using Common.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace TreeEditor.TableAdapter
{
    public class EqtTreeTableAdapter:ITreeTableAdapter
    {
        protected static readonly ILog log=LogManager.GetCurrentClassLogger();
        private DaTfunction datf;

        
       
        public EqtTreeTableAdapter():this(new DaTfunction())
        {
          
        }

        public EqtTreeTableAdapter(DaTfunction datf_)
        {
            datf = datf_;
        }

        #region 私有工具函数
        private static IList<ITvaNode> ConvertModelList2NodeList(IList<MoTfunction> listMo)
        {
            log.DebugFormat("转换Model实体到接口类型:Count={0}",listMo.Count);
            IList<ITvaNode> listNode = new List<ITvaNode>(listMo.Count);
            for (int i = 0; i < listMo.Count; i++)
            {
                listNode.Add(listMo[i]);
            }
            return listNode;
        }

        private static IList<MoTfunction> ConvertNodeList2ModelList(IEnumerable<ITvaNode> listNode)
        {
             
            IList<MoTfunction> list = new List<MoTfunction>();
            foreach(ITvaNode n in listNode)
            {
                list.Add((MoTfunction)n);
            }
            return list;
        }
        #endregion

        #region ITreeTableAdapter 成员

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="dt"></param>
        public int SyncTreeNodes2DataTable(Dictionary<string, ITvaNode> tvnDic, DataTable dt)
        {
            int rcError = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                string id = row[IdFieldName].ToString();
                if (tvnDic.ContainsKey(id))
                {
                    MoTfunction mo = (MoTfunction)tvnDic[id];
                    mo.Model2Row(row);
                }
                else
                {
                    ++rcError;
                    log.ErrorFormat("没找到id={0}对应的表记录", id);
                }
            }
            return rcError;
           
        }
        /// <summary>
        /// 将DataTable同步到树.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tvnDic"></param>
        public int SyncDataTable2TreeNodes(DataTable dt, Dictionary<string, ITvaNode> tvnDic)
        {
            int rcError = 0;
            string Id = null;
            dt.AcceptChanges();            
            foreach (DataRow row in dt.Rows)
            {
                Id = (string)row[IdFieldName];
                if (tvnDic.ContainsKey(Id))
                {
                    (tvnDic[Id] as MoTfunction).Row2Model(row);
                }
                else
                {
                    log.ErrorFormat("{0}没有对应的树节点.", Id);
                    ++rcError;
                }
            }
            return rcError;
        }

        public void AddTvaNode2DataTable(ITvaNode node, DataTable dt)
        {
            MoTfunction mo = node as MoTfunction;
            if (mo != null)//节点不空
            {
                DataRow row= dt.NewRow();
                //Note:做了同步
                mo.OrderNum = mo.TNA_LogicId;
                mo.Model2Row(row);
                dt.Rows.Add(row);
               
            }
        }

        public ITvaNode Row2TvaNode(DataRow row)
        {
            MoTfunction mo = new MoTfunction();
            mo.Row2Model(row);
            return mo;
        }

        private string treeTableName = "TFUNCTION";
        public string TreeTableName
        {
            get
            {
                return treeTableName;
            }
            set
            {
                treeTableName = value;
            }
        }

        private string getRootsWhereClause = " WHERE PARENT_FUNC is null Or PARENT_FUNC=''";
        public string GetRootsWhereClause
        {
            get
            {
                return getRootsWhereClause;
            }
            set
            {
                getRootsWhereClause = value;
            }
        }

        private string idFieldName = "FUNC_ID";
        public string IdFieldName
        {
            get
            {
                return idFieldName;
            }
            set
            {
                idFieldName = value;
            }
        }

        private string parentIdFieldName = "PARENT_FUNC";
        public string ParentIdFieldName
        {
            get
            {
                return parentIdFieldName;
            }
            set
            {
                parentIdFieldName = value;
            }
        }

        public int GetNodesTotalCount()
        {
            return datf.GetCount();
        }

        public IList<ITvaNode> GetRootNodes()
        {
            log.Debug("获取所有根节点");

            return ConvertModelList2NodeList(datf.GetEntities(GetRootsWhereClause));   
        }

        public IList<ITvaNode> GetTreeNodes()
        {
            log.Debug("获取所有节点 IList<ITreeNodeModel>");
            return ConvertModelList2NodeList(datf.GetEntities(null));           
        }

        /// <summary>
        /// 获取树表的全部数据。
        /// </summary>
        /// <returns></returns>
        public DataSet GetTreeNodeDataTable()
        {
            log.Debug("获取所有根节点 DataTable");
            return datf.Query(null);
        }

        public int SyncToDb(IEnumerable<ITvaNode> treeNodeModelList,bool force)
        {
            return datf.Save( ConvertNodeList2ModelList(treeNodeModelList) ,force);    
        }

        public int SyncToDb(DataSet tvaNodeTreeds)
        {
            //用DS更新可能会出错，因为树可以增删改 

            throw new Exception("The method or operation is not implemented.");
            //return datf.UpdateByDataSet(tvaNodeTreeds, tvaNodeTreeds.Tables[0].TableName);
        }

        public IList<ITvaNode> GetNextChildTreeNodes(ITvaNode nm)
        {
            log.Debug("获取紧接的孩子节点 IList<ITreeNodeModel> ");
            return ConvertModelList2NodeList(datf.GetEntities(String.Format(" WHERE {0}='{1}'",
                ParentIdFieldName,nm.TNA_ID)));   
        }

        public IList<ITvaNode> GetCssChildTreeNodes(ITvaNode parent)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsHasChild(ITvaNode nm)
        {
            log.DebugFormat("判定一个节点是否是叶子节点:{0} ",nm);
            return datf.GetCount(String.Format(" WHERE {0}='{1}'",ParentIdFieldName, nm.TNA_ID)) > 0;
        }

        #endregion
    }
}
