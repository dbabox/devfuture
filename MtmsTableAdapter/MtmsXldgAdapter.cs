using System;
using System.Collections.Generic;
using System.Text;
using TreeEditor.Core;
using Mtms.Dal;
using Mtms.Model;
using Common.Logging;
using System.Data;

namespace TreeEditor.TableAdapter
{
    public class MtmsXldgAdapter : ITreeTableAdapter
    {
        protected static readonly ILog log = LogManager.GetCurrentClassLogger();
        private DaXldg da;

        public MtmsXldgAdapter():this(new DaXldg())
        {
            
        }

        public MtmsXldgAdapter(DaXldg da_)
        {
            da = da_;
        }

        #region ˽�й��ߺ���
        private static IList<ITvaNode> ConvertModelList2NodeList(IList<MoXldg> listMo)
        {
            log.DebugFormat("ת��Modelʵ�嵽�ӿ�����:Count={0}", listMo.Count);
            IList<ITvaNode> listNode = new List<ITvaNode>(listMo.Count);
            for (int i = 0; i < listMo.Count; i++)
            {              
                listNode.Add(listMo[i]);
            }
            return listNode;
        }

        private static IList<MoXldg> ConvertNodeList2ModelList(IEnumerable<ITvaNode> listNode)
        {

            IList<MoXldg> list = new List<MoXldg>();
            foreach (ITvaNode n in listNode)
            {
                list.Add((MoXldg)n);
            }
            return list;
        }
        #endregion

        #region ITreeTableAdapter ��Ա
 

        public void AddTvaNode2DataTable(ITvaNode node, System.Data.DataTable dt)
        {
            MoXldg mo = node as MoXldg;
            if (mo != null)//�ڵ㲻��
            {
                DataRow row = dt.NewRow();
                //Note:����ͬ��
                mo.DgClass2 = mo.TNA_LogicId; //�������

                mo.Model2Row(row);
                dt.Rows.Add(row);

            }
        }

        public ITvaNode Row2TvaNode(System.Data.DataRow row)
        {
            MoXldg mo = new MoXldg();
            mo.Row2Model(row);
            return mo;
        }

        private string getRootsWhereClause = " WHERE DG_ID ='2009'";
        public string GetRootsWhereClause
        {
            get { return getRootsWhereClause; }
            set { getRootsWhereClause = value; }
        }
        private string idFieldName = "DG_ID";
        public string IdFieldName
        {
            get { return idFieldName; }
            set { idFieldName = value; }
        }
        private string parentIdFieldName = "DG_PARENT";
        public string ParentIdFieldName
        {
            get { return parentIdFieldName; }
            set { parentIdFieldName = value; }
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
            return ConvertModelList2NodeList(da.GetEntities(" WHERE DG_ID LIKE '2009%'"));      
        }

        public System.Data.DataSet GetTreeNodeDataTable()
        {
            log.Debug("��ȡ���нڵ� DataTable");
            return da.Query(" WHERE DG_ID LIKE '2009%'");
        }

        public int SyncTreeNodes2DataTable(Dictionary<string,ITvaNode> tnDic, System.Data.DataTable dt)
        {
            int rcError = 0;
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                string id = row[IdFieldName].ToString();
                if (tnDic.ContainsKey(id))
                {
                    MoXldg mo = (MoXldg)tnDic[row[IdFieldName].ToString()];
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
                Id=(string)row[IdFieldName];
                if (tvnDic.ContainsKey(Id))
                {
                    (tvnDic[Id] as MoXldg).Row2Model(row);
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
    }
}
