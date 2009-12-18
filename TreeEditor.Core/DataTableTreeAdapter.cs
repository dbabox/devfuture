using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{
    public class DataTableTreeAdapter : ITreeTableAdapter
    {
        internal class DataRowTvaNode : ITvaNode
        {
            private DataRow row;
            private DataTableTreeAdapter dtta;
            internal DataRowTvaNode(DataRow row_, DataTableTreeAdapter dtta_)
            {
                row = row_;
                dtta = dtta_;
            }


            #region ITvaNode 成员

            public string TNA_ID
            {
                get
                {
                    return (string)row[dtta.IdFieldName];
                }
                set
                {
                    row[dtta.IdFieldName] = value;
                }
            }

            public string TNA_PID
            {
                get
                {
                    return  (string)row[dtta.ParentIdFieldName];
                }
                set
                {
                    row[dtta.ParentIdFieldName] = value;
                }
            }

            public string TNA_Text
            {
                get
                {
                    return  (string)row[dtta.TextFieldName];
                }
                set
                {
                    row[dtta.TextFieldName] = value;
                }
            }

            public string TNA_LogicId
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public IList<ITvaNode> Owner
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int TNA_Level
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int TNA_Index
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public System.Drawing.Image Icon
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public bool IsChecked
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            #endregion

            #region ICloneable 成员

            public object Clone()
            {
                //TODO:由dtta负责复制一个新行
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }

        #region ITreeTableAdapter 成员


        private string getRootsWhereClause;

        public string GetRootsWhereClause
        {
            get { return getRootsWhereClause; }
            set { getRootsWhereClause = value; }
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
                idFieldName = value;
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
                parentIdFieldName = value;
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
                textFieldName = value;
            }
        }

        public void AddTvaNode2DataTable(ITvaNode node, System.Data.DataTable dt)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITvaNode Row2TvaNode(System.Data.DataRow row)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetNodesTotalCount()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 从DataTable中解析出所有根节点
        /// </summary>
        /// <returns></returns>
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
