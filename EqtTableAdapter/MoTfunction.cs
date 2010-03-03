
/*----------------------------------------------------------------//
// 文件名：MoTfunction.cs
// 文件功能描述：实体 MoTfunction.
// 
// 创建标识：Template  ver 5.0.20090429 2009-11-20
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
#region MoTfunction
using System;
using System.Data;

using TreeEditor.Core;
using System.Collections.Generic;
namespace EQT.Model
{
    /// <summary>
    /// 业务实体MoTfunction
    /// </summary>
    public partial class MoTfunction : ITvaNode
    {
        #region 表Tfunction的字段
        private string funcId;
        private string funcName;
        private string funcClass;
        private string funcSign;
        private string rem;
        private string orderNum;
        private string parentFunc;
        #endregion

        #region 构造函数
        public MoTfunction()
        {


        }
        #endregion

        #region 拷贝构造函数

        public MoTfunction(MoTfunction item)
        {
            item.CopyTo(this);
        }
        #endregion

        #region 带参数构造函数
        public MoTfunction(string funcId, string funcName, string funcClass, string funcSign, string rem, string orderNum, string parentFunc)
        {
            this.funcId = funcId;
            this.funcName = funcName;
            this.funcClass = funcClass;
            this.funcSign = funcSign;
            this.rem = rem;
            this.orderNum = orderNum;
            this.parentFunc = parentFunc;
        }
        #endregion

        #region 表Tfunction的字段属性
        /// <summary>
        ///对WINDFORM系统，功能项ID可以ASCII字符串，也可以是数字的字符串格式。目前考虑使用特定编码规则来实现。对WEB系统，功能项应是特定的URL请求或WEBFORM的相对URL编码；
        /// </summary>
        public string FuncId
        {
            get { return funcId; }
            set { funcId = value; }
        }
        /// <summary>
        /// FUNC_NAME
        /// </summary>
        public string FuncName
        {
            get { return funcName; }
            set { funcName = value; }
        }
        /// <summary>
        ///内别是系统内置的。不可由程序来维护。
        /// </summary>
        public string FuncClass
        {
            get { return funcClass; }
            set { funcClass = value; }
        }
        /// <summary>
        /// FUNC_SIGN
        /// </summary>
        public string FuncSign
        {
            get { return funcSign; }
            set { funcSign = value; }
        }
        /// <summary>
        ///'序号ORDER_NUM用罗
        /// </summary>
        public string Rem
        {
            get { return rem; }
            set { rem = value; }
        }
        /// <summary>
        /// ORDER_NUM
        /// </summary>
        public string OrderNum
        {
            get { return orderNum; }
            set { orderNum = value; }
        }
        /// <summary>
        /// PARENT_FUNC
        /// </summary>
        public string ParentFunc
        {
            get { return parentFunc; }
            set { parentFunc = value; }
        }
        #endregion

        #region 辅助函数
        /// <summary>
        /// 拷贝函数
        /// </summary>
        public MoTfunction CopyTo(MoTfunction item)
        {
            item.funcId = this.funcId;
            item.funcName = this.funcName;
            item.funcClass = this.funcClass;
            item.funcSign = this.funcSign;
            item.rem = this.rem;
            item.orderNum = this.orderNum;
            item.parentFunc = this.parentFunc;
            return item;
        }
        /// <summary>
        /// 克隆函数
        /// </summary>		
        public object Clone()
        {
            MoTfunction mo = new MoTfunction();
            mo.FuncId = this.FuncId;
            mo.FuncName = this.FuncName;
            mo.FuncClass = this.FuncClass;
            mo.FuncSign = this.FuncSign;
            mo.Rem = this.Rem;
            mo.OrderNum = this.OrderNum;
            mo.ParentFunc = this.ParentFunc;
            return mo;
        }
        ///<summary>
        ///判断对象是否相等
        ///<summary>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else
            {
                MoTfunction NewTfunction = obj as MoTfunction;
                if (NewTfunction == null)
                {
                    return false;
                }
                else
                {
                    if (NewTfunction.FuncId == this.FuncId && NewTfunction.FuncName == this.FuncName && NewTfunction.FuncClass == this.FuncClass && NewTfunction.FuncSign == this.FuncSign && NewTfunction.Rem == this.Rem && NewTfunction.OrderNum == this.OrderNum && NewTfunction.ParentFunc == this.ParentFunc)
                    {
                        return true;
                    }
                }

            }
            return false;

        }
        ///TODO:you should modify GetHashCode by yourself.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(System.Globalization.CultureInfo.InvariantCulture, "FuncId={0}/FuncName={1}/FuncClass={2}/FuncSign={3}/Rem={4}/OrderNum={5}/ParentFunc={6}", funcId, funcName, funcClass, funcSign, rem, orderNum, parentFunc);

        }
        #endregion


        #region DataTable Help Function
        ///<summary>
        ///DataRow转换成Model
        ///</summary>
        public void Row2Model(DataRow row)
        {
            if (row["Func_ID"] != null && row["Func_ID"] != DBNull.Value)
            {
                this.FuncId = (string)row["Func_ID"];
            }
            else
            {
                this.FuncId = string.Empty;
            }
            if (row["Func_Name"] != null && row["Func_Name"] != DBNull.Value)
            {
                this.FuncName = (string)row["Func_Name"];
            }
            else
            {
                this.FuncName = string.Empty;
            }
            if (row["Func_Class"] != null && row["Func_Class"] != DBNull.Value)
            {
                this.FuncClass = (string)row["Func_Class"];
            }
            else
            {
                this.FuncClass = string.Empty;
            }
            if (row["Func_Sign"] != null && row["Func_Sign"] != DBNull.Value)
            {
                this.FuncSign = (string)row["Func_Sign"];
            }
            else
            {
                this.FuncSign = string.Empty;
            }
            if (row["Rem"] != null && row["Rem"] != DBNull.Value)
            {
                this.Rem = (string)row["Rem"];
            }
            else
            {
                this.Rem = string.Empty;
            }
            if (row["Order_Num"] != null && row["Order_Num"] != DBNull.Value)
            {
                this.OrderNum = (string)row["Order_Num"];
            }
            else
            {
                this.OrderNum = string.Empty;
            }
            if (row["Parent_Func"] != null && row["Parent_Func"] != DBNull.Value)
            {
                this.ParentFunc = (string)row["Parent_Func"];
            }
            else
            {
                this.ParentFunc = string.Empty;
            }
        }

        ///<summary>
        ///Model转换成DataRow
        ///</summary>
        public void Model2Row(DataRow row)
        {
            row["Func_ID"] = this.FuncId;
            row["Func_Name"] = this.FuncName;
            row["Func_Class"] = this.FuncClass;
            row["Func_Sign"] = this.FuncSign;
            row["Rem"] = this.Rem;
            row["Order_Num"] = this.OrderNum;
            row["Parent_Func"] = this.ParentFunc;
        }
        #endregion

        #region ITreeNodeModel 成员

        private IList<ITvaNode> owner;

        public IList<ITvaNode> Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        //private string logicId;
        public string TNA_LogicId
        {
            get
            {
                return orderNum;
            }
            set
            {
                orderNum = value;
            }
        }


        public int level = -1;
        public int TNA_Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }
               

        public int TNA_Index
        {
            get
            {
                if (owner != null) 
                    return owner.IndexOf(this);                
                return -1;
            }
         
        }


        public string TNA_Text
        {
            get
            {
                return funcName;
            }
            set
            {
                funcName=value;
            }
        }

        public string TNA_ID
        {
            get
            {
                return funcId;
            }
            set
            {
                funcId=value;
            }
        }

        public string TNA_PID
        {
            get
            {
                return parentFunc;
            }
            set
            {
                parentFunc = value;
            }
        }
        protected System.Drawing.Image icon_;
        public System.Drawing.Image Icon
        {
            get
            {
                return icon_;
            }
            set
            {
                icon_ = value;
            }
        }

      

        protected bool isChecked;
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
 
    }
}
#endregion
