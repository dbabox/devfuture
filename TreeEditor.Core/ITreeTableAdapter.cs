using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{
    /// <summary>
    /// ����������
    /// </summary>
    public interface ITreeTableAdapter
    { 
        string GetRootsWhereClause
        {
            get;
            set;
        }

        string IdFieldName
        {
            get;
            set;
        }

        string ParentIdFieldName
        {
            get;
            set;
        }

        //string TextFieldName
        //{
        //    get;
        //    set;
        //}

   

        void AddTvaNode2DataTable(ITvaNode node, DataTable dt);

        ITvaNode Row2TvaNode(DataRow row);


        int GetNodesTotalCount();

        /// <summary>
        /// �õ���һ��ڵ㣬��һ��ڵ㶼�Ǹ��ڵ㡣��Ϊ�����ݿ����»�ȡ��
        /// </summary>
        /// <returns></returns>
        IList<ITvaNode> GetRootNodes();



        /// <summary>
        /// ��ȡ�������ڵ�
        /// </summary>
        /// <returns></returns>
        IList<ITvaNode> GetTreeNodes();

        /// <summary>
        /// ��ȡ���ڵ��DataTable.
        /// </summary>
        /// <returns></returns>
        DataSet GetTreeNodeDataTable();

        int SyncTreeNodes2DataTable(Dictionary<string, ITvaNode> tvnDic, DataTable dt);

        int SyncDataTable2TreeNodes(DataTable dt, Dictionary<string, ITvaNode> tvnDic);

        /// <summary>
        /// ͬ�����ڵ㼯�ϵ����ݿ�
        /// force=true��������վ����ݿ⣬Ȼ����ͬ�����⽫Ӧ��ɾ���߼���
        /// </summary>
        /// <param name="treeNodeModelList"></param>
        /// <returns></returns>
        int SyncToDb(IEnumerable<ITvaNode> treeNodeModelList,bool force);

        /// <summary>
        /// ͬ���ڵ�DataSet�����ݿ�
        /// </summary>
        /// <param name="tvaNodeTreeDs"></param>
        /// <returns></returns>
        int SyncToDb(DataSet tvaNodeTreeDs);
        /// <summary>
        /// �õ�ĳ���ڵ���ֱ���ĺ��ӽڵ�
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        IList<ITvaNode> GetNextChildTreeNodes(ITvaNode parent);
        /// <summary>
        /// �õ�ĳ���ڵ��µ����к��ӽ�㡣
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        IList<ITvaNode> GetCssChildTreeNodes(ITvaNode parent);

        /// <summary>
        /// �ж�һ���ڵ��Ƿ��к���
        /// </summary>
        /// <param name="nm"></param>
        /// <returns></returns>
        bool IsHasChild(ITvaNode nm);

       

    }
}
