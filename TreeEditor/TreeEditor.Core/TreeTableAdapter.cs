/*
 * ����ʹ��ģ���࣬����ֱ��ʹ��DataTable��DataRow��ʵ�֡�
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{


    public class TreeTableAdapter<T,U> : ITreeTableAdapter
        where T :class, ITvaNode,new()
        where U:DaBase<T>
       
    {

     


        #region ITreeTableAdapter ��Ա

        private string getRootsWhereClause;
        public string GetRootsWhereClause
        {
            get
            {
                return getRootsWhereClause;
            }
            set
            {
                getRootsWhereClause=value;
            }
        }

        private string idFieldName;
        public string IdFieldName
        {
            get
            {
                return idFieldName;
            }
            set
            {
                idFieldName=value;
            }
        }

        private string parentIdFieldName;
        public string ParentIdFieldName
        {
            get
            {
                return parentIdFieldName;
            }
            set
            {
                parentIdFieldName=value;
            }
        }

        private string textFieldName;
        public string TextFieldName
        {
            get
            {
                return textFieldName;
            }
            set
            {
                textFieldName=value;
            }
        }

        /// <summary>
        /// ������д�˷�������Modelʵ���ֵ���������������罫LogicId��ֵ��Model��ĳ���ֶΡ�
        /// ����Ҫ��Nodeǿ��cast��ĳ��ʵ�������в�����ע��������base.AddTvaNode2DataTable.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dt"></param>
        public virtual void AddTvaNode2DataTable(ITvaNode node, System.Data.DataTable dt)
        {
            DataRow row = dt.NewRow();
            node.Model2Row(row);
            dt.Rows.Add(row);
        }

        public ITvaNode Row2TvaNode(System.Data.DataRow row)
        {
            T mo = new T();
            mo.Row2Model(row);
            return mo;
        }

        public int GetNodesTotalCount()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<ITvaNode> GetRootNodes()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<ITvaNode> GetTreeNodes()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Data.DataSet GetTreeNodeDataTable()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int SyncTreeNodes2DataTable(Dictionary<string, ITvaNode> tvnDic, System.Data.DataTable dt)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int SyncDataTable2TreeNodes(System.Data.DataTable dt, Dictionary<string, ITvaNode> tvnDic)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int SyncToDb(IEnumerable<ITvaNode> treeNodeModelList, bool force)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int SyncToDb(System.Data.DataSet tvaNodeTreeDs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<ITvaNode> GetNextChildTreeNodes(ITvaNode parent)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<ITvaNode> GetCssChildTreeNodes(ITvaNode parent)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsHasChild(ITvaNode nm)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
