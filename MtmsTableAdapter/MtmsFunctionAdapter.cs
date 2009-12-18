using System;
using System.Collections.Generic;
using System.Text;
using TreeEditor.Core;
using Mtms.Dal;
using Mtms.Model;
using System.Data;
using Common.Logging;

namespace TreeEditor.TableAdapter
{
    public  class MtmsFunctionAdapter : ITreeTableAdapter
    {
        protected static readonly ILog log = LogManager.GetCurrentClassLogger();
        private DaFunction da;

        public MtmsFunctionAdapter():this(new DaFunction())
        {
        }

        public MtmsFunctionAdapter(DaFunction da_)
        {
            this.da = da_;
        }


        #region ITreeTableAdapter ��Ա

      
        public string GetRootsWhereClause
        {
            get
            {
                return String.Format(" WHERE {0} is NULL OR {0}='' ", ParentIdFieldName); ;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string IdFieldName
        {
            get
            {
                return "FUNC_CODE";
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string ParentIdFieldName
        {
            get
            {
                return "PARENT_FUNC_CODE";
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string TextFieldName
        {
            get
            {
                return "FUNC_TITLE";
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void AddTvaNode2DataTable(ITvaNode node, System.Data.DataTable dt)
        {
            MoFunction mo = node as MoFunction;
            if (mo != null)//�ڵ㲻��
            {
                DataRow row = dt.NewRow();
                //Note:����ͬ��
                mo.OrderTag = mo.TNA_LogicId; //�������

                mo.Model2Row(row);
                dt.Rows.Add(row);

            }
        }

        public ITvaNode Row2TvaNode(System.Data.DataRow row)
        {
            MoFunction mo = new MoFunction();
            mo.Row2Model(row);
            return mo;
        }

        public int GetNodesTotalCount()
        {
            return da.GetCount();
        }

        public IList<ITvaNode> GetRootNodes()
        {
            log.Debug("��ȡ���и��ڵ�");
            return ConvertModelList2NodeList(da.GetEntities(GetRootsWhereClause));   
        }

        public IList<ITvaNode> GetTreeNodes()
        {
            log.Debug("��ȡ���нڵ� IList<ITreeNodeModel>");
            return ConvertModelList2NodeList(da.GetEntities(null));  
        }

        public System.Data.DataSet GetTreeNodeDataTable()
        {
            log.Debug("��ȡ���нڵ� DataTable");
            return da.Query(null);
        }

        public int SyncTreeNodes2DataTable(Dictionary<string, ITvaNode> tvnDic, System.Data.DataTable dt)
        {
            int rcError = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                string id = row[IdFieldName].ToString();
                if (tvnDic.ContainsKey(id))
                {
                    MoFunction mo = (MoFunction)tvnDic[row[IdFieldName].ToString()];
                    mo.Model2Row(row);
                }
                else
                {
                    ++rcError;
                    log.ErrorFormat("û�ҵ�id={0}��Ӧ�ı��¼", id);
                }
            }
            return rcError;
        }

        public int SyncDataTable2TreeNodes(System.Data.DataTable dt, Dictionary<string, ITvaNode> tvnDic)
        {
            int rcError = 0;
            dt.AcceptChanges();
            string Id = null;
            foreach (DataRow row in dt.Rows)
            {
                Id = (string)row[IdFieldName];
                if (tvnDic.ContainsKey(Id))
                {
                    (tvnDic[Id] as MoFunction).Row2Model(row);
                }
                else
                {
                    log.ErrorFormat("{0}û�ж�Ӧ�����ڵ�.", Id);
                    ++rcError;
                }
            }
            return rcError;
        }

        public int SyncToDb(IEnumerable<ITvaNode> treeNodeModelList, bool force)
        {
            return da.Save(ConvertNodeList2ModelList(treeNodeModelList), force);    
        }

        public int SyncToDb(System.Data.DataSet tvaNodeTreeDs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<ITvaNode> GetNextChildTreeNodes(ITvaNode parent)
        {
            log.Debug("��ȡ���ӵĺ��ӽڵ� IList<ITreeNodeModel> ");
            return ConvertModelList2NodeList(da.GetEntities(String.Format(" WHERE {0}='{1}'",
                ParentIdFieldName, parent.TNA_ID)));   
        }

        public IList<ITvaNode> GetCssChildTreeNodes(ITvaNode parent)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsHasChild(ITvaNode nm)
        {
            log.DebugFormat("�ж�һ���ڵ��Ƿ���Ҷ�ӽڵ�:{0} ", nm);
            return da.GetCount(String.Format(" WHERE {0}='{1}'", ParentIdFieldName, nm.TNA_ID)) > 0;
        }

        #endregion

        #region ˽�й��ߺ���
        private static IList<ITvaNode> ConvertModelList2NodeList(IList<MoFunction> listMo)
        {
            log.DebugFormat("ת��Modelʵ�嵽�ӿ�����:Count={0}", listMo.Count);
            IList<ITvaNode> listNode = new List<ITvaNode>(listMo.Count);
            for (int i = 0; i < listMo.Count; i++)
            {
                listNode.Add(listMo[i]);
            }
            return listNode;
        }

        private static IList<MoFunction> ConvertNodeList2ModelList(IEnumerable<ITvaNode> listNode)
        {

            IList<MoFunction> list = new List<MoFunction>();
            foreach (ITvaNode n in listNode)
            {
                list.Add((MoFunction)n);
            }
            return list;
        }
        #endregion
    }
}
