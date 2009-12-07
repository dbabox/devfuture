using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{
    /// <summary>
    /// 树表适配器
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
        /// 得到第一层节点，第一层节点都是根节点。此为从数据库重新获取。
        /// </summary>
        /// <returns></returns>
        IList<ITvaNode> GetRootNodes();



        /// <summary>
        /// 获取所有树节点
        /// </summary>
        /// <returns></returns>
        IList<ITvaNode> GetTreeNodes();

        /// <summary>
        /// 获取树节点的DataTable.
        /// </summary>
        /// <returns></returns>
        DataSet GetTreeNodeDataTable();

        int SyncTreeNodes2DataTable(Dictionary<string, ITvaNode> tvnDic, DataTable dt);

        int SyncDataTable2TreeNodes(DataTable dt, Dictionary<string, ITvaNode> tvnDic);

        /// <summary>
        /// 同步树节点集合到数据库
        /// force=true，则先清空旧数据库，然后再同步。这将应用删除逻辑。
        /// </summary>
        /// <param name="treeNodeModelList"></param>
        /// <returns></returns>
        int SyncToDb(IEnumerable<ITvaNode> treeNodeModelList,bool force);

        /// <summary>
        /// 同步节点DataSet到数据库
        /// </summary>
        /// <param name="tvaNodeTreeDs"></param>
        /// <returns></returns>
        int SyncToDb(DataSet tvaNodeTreeDs);
        /// <summary>
        /// 得到某个节点下直属的孩子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        IList<ITvaNode> GetNextChildTreeNodes(ITvaNode parent);
        /// <summary>
        /// 得到某个节点下的所有孩子结点。
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        IList<ITvaNode> GetCssChildTreeNodes(ITvaNode parent);

        /// <summary>
        /// 判断一个节点是否还有孩子
        /// </summary>
        /// <param name="nm"></param>
        /// <returns></returns>
        bool IsHasChild(ITvaNode nm);

       

    }
}
