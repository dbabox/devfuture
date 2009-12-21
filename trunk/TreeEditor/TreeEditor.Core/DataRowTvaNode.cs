using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
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
        public DataRowTvaNode(DataRow row_,TvaSchema rowSchema_)
        {
            row = row_;
            rowSchema = rowSchema_;
            SyncDataRow2TvnNode();
        }

        public void SyncDataRow2TvnNode()
        {
            tna_id = row[rowSchema.Tna_id_field_name].ToString();
            tna_pid = row[rowSchema.Tna_pid_field_name].ToString();
            tna_text = row[rowSchema.Tna_text_field_name].ToString();
        }


        #region ITvaNode 成员

        /// <summary>
        /// tna_id不直接使用 return row[rowSchema.Tna_id_field_name].ToString()的原因在于当Row被删除
        /// 时，此调用将出现错误。
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
                if(!String.IsNullOrEmpty(rowSchema.Tna_logic_id_map_field))
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

        public static DataRowTvaNode[] CreateNodes(TvaSchema rowSchema,params DataRow[] rows)
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
}
