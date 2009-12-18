
/*----------------------------------------------------------------//
// 文件名：MoFunction.cs
// 文件功能描述：实体 MoFunction.
// 
// 创建标识：Template  ver 5.0.20090429 2009-12-18
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
#region MoFunction
using System;
using System.Data;
using TreeEditor.Core;
using System.Collections.Generic;
namespace Mtms.Model
{
	/// <summary>
	/// 业务实体MoFunction
	/// </summary>
    public partial class MoFunction : ITvaNode
	{
		#region 表Function的字段
		private string funcCode;
		private string parentFuncCode;
		private string funcTitle;
		private int funcType;
		private int orderIndex;
		private string orderTag;
		private string rem;
		private DateTime createTime;
		#endregion
		
		#region 构造函数
		public MoFunction()
		{
			       createTime=new DateTime(1900,1,1);  	    
		   
		}
		#endregion
		
		#region 拷贝构造函数

		public MoFunction(MoFunction item)
		{
			item.CopyTo(this);
		}
		#endregion
		
		#region 带参数构造函数
	    public MoFunction(string funcCode,string parentFuncCode,string funcTitle,int funcType,int orderIndex,string orderTag,string rem,DateTime createTime)
		{
		this.funcCode=funcCode;
		this.parentFuncCode=parentFuncCode;
		this.funcTitle=funcTitle;
		this.funcType=funcType;
		this.orderIndex=orderIndex;
		this.orderTag=orderTag;
		this.rem=rem;
		this.createTime=createTime;
		}
		#endregion
		
		#region 表Function的字段属性
		/// <summary>
		/// FUNC_CODE
		/// </summary>
		public string FuncCode
		{
			get {return funcCode;}
			set {funcCode=value;}
		}
		/// <summary>
		/// PARENT_FUNC_CODE
		/// </summary>
		public string ParentFuncCode
		{
			get {return parentFuncCode;}
			set {parentFuncCode=value;}
		}
		/// <summary>
		///这里可能也称其为功能名称（FUNC_NAME），但称为 功能标题 更确切。
		/// </summary>
		public string FuncTitle
		{
			get {return funcTitle;}
			set {funcTitle=value;}
		}
		/// <summary>
		///功能类型定义为：1——导航节点，   2——页面内节点（含页面内容器和页面内功能）
		/// </summary>
		public int FuncType
		{
			get {return funcType;}
			set {funcType=value;}
		}
		/// <summary>
		/// ORDER_INDEX
		/// </summary>
		public int OrderIndex
		{
			get {return orderIndex;}
			set {orderIndex=value;}
		}
		/// <summary>
		/// ORDER_TAG
		/// </summary>
		public string OrderTag
		{
			get {return orderTag;}
			set {orderTag=value;}
		}
		/// <summary>
		/// REM
		/// </summary>
		public string Rem
		{
			get {return rem;}
			set {rem=value;}
		}
		/// <summary>
		/// CREATE_TIME
		/// </summary>
		public DateTime CreateTime
		{
			get {return createTime;}
			set {createTime=value;}
		}
		#endregion
		
		#region 辅助函数
		/// <summary>
		/// 拷贝函数
		/// </summary>
		public MoFunction CopyTo(MoFunction item)
        {
		    item.funcCode=this.funcCode;
		    item.parentFuncCode=this.parentFuncCode;
		    item.funcTitle=this.funcTitle;
		    item.funcType=this.funcType;
		    item.orderIndex=this.orderIndex;
		    item.orderTag=this.orderTag;
		    item.rem=this.rem;
		    item.createTime=this.createTime;
			return item;
        }
		/// <summary>
		/// 克隆函数
		/// </summary>		
		public object Clone()
        {
            MoFunction mo=new MoFunction();
		    mo.FuncCode=this.FuncCode;
		    mo.ParentFuncCode=this.ParentFuncCode;
		    mo.FuncTitle=this.FuncTitle;
		    mo.FuncType=this.FuncType;
		    mo.OrderIndex=this.OrderIndex;
		    mo.OrderTag=this.OrderTag;
		    mo.Rem=this.Rem;
		    mo.CreateTime=this.CreateTime;
			return mo;
		}
				///<summary>
		///判断对象是否相等
		///<summary>
		public override bool Equals(object obj)
        {		 
            if (object.ReferenceEquals(this,obj))
            {
                return true;
            }
            else
            {
                MoFunction NewFunction = obj as MoFunction;
                if (NewFunction == null)
                {
                    return false;
                }
                else
                {
				if (NewFunction.FuncCode==this.FuncCode && NewFunction.ParentFuncCode==this.ParentFuncCode && NewFunction.FuncTitle==this.FuncTitle && NewFunction.FuncType==this.FuncType && NewFunction.OrderIndex==this.OrderIndex && NewFunction.OrderTag==this.OrderTag && NewFunction.Rem==this.Rem && NewFunction.CreateTime==this.CreateTime)		
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
		   return String.Format(System.Globalization.CultureInfo.InvariantCulture,"FuncCode={0}/ParentFuncCode={1}/FuncTitle={2}/FuncType={3}/OrderIndex={4}/OrderTag={5}/Rem={6}/CreateTime={7}",funcCode,parentFuncCode,funcTitle,funcType,orderIndex,orderTag,rem,createTime);
		 
		}
		#endregion
		
		
		#region DataTable Help Function
		///<summary>
		///DataRow转换成Model
		///</summary>
		public void Row2Model(DataRow row)
		{
		    if(row["FUNC_CODE"]!=null && row["FUNC_CODE"]!=DBNull.Value)
			{
			    this.FuncCode = (string)row["FUNC_CODE"];
			}
			else
			{
				this.FuncCode =string.Empty;
			}
		    if(row["PARENT_FUNC_CODE"]!=null && row["PARENT_FUNC_CODE"]!=DBNull.Value)
			{
			    this.ParentFuncCode = (string)row["PARENT_FUNC_CODE"];
			}
			else
			{
				this.ParentFuncCode =string.Empty;
			}
		    if(row["FUNC_TITLE"]!=null && row["FUNC_TITLE"]!=DBNull.Value)
			{
			    this.FuncTitle = (string)row["FUNC_TITLE"];
			}
			else
			{
				this.FuncTitle =string.Empty;
			}
		    if(row["FUNC_TYPE"]!=null && row["FUNC_TYPE"]!=DBNull.Value)
			{
			    this.FuncType = (int)row["FUNC_TYPE"];
			}
			else
			{
				this.FuncType =0;
			}
		    if(row["ORDER_INDEX"]!=null && row["ORDER_INDEX"]!=DBNull.Value)
			{
			    this.OrderIndex = (int)row["ORDER_INDEX"];
			}
			else
			{
				this.OrderIndex =0;
			}
		    if(row["ORDER_TAG"]!=null && row["ORDER_TAG"]!=DBNull.Value)
			{
			    this.OrderTag = (string)row["ORDER_TAG"];
			}
			else
			{
				this.OrderTag =string.Empty;
			}
		    if(row["REM"]!=null && row["REM"]!=DBNull.Value)
			{
			    this.Rem = (string)row["REM"];
			}
			else
			{
				this.Rem =string.Empty;
			}
		    if(row["CREATE_TIME"]!=null && row["CREATE_TIME"]!=DBNull.Value)
			{
			    this.CreateTime = (DateTime)row["CREATE_TIME"];
			}
			else
			{
				this.CreateTime =DateTime.MinValue;
			}
		}
		
		///<summary>
		///Model转换成DataRow
		///</summary>
	　　public void Model2Row(DataRow row)
		{
			row["FUNC_CODE"] = this.FuncCode;
			row["PARENT_FUNC_CODE"] = this.ParentFuncCode;
			row["FUNC_TITLE"] = this.FuncTitle;
			row["FUNC_TYPE"] = this.FuncType;
			row["ORDER_INDEX"] = this.OrderIndex;
			row["ORDER_TAG"] = this.OrderTag;
			row["REM"] = this.Rem;
			row["CREATE_TIME"] = this.CreateTime;
		}
		#endregion

        #region ITvaNode 成员

        public string TNA_ID
        {
            get
            {
                return funcCode;
            }
            set
            {
                funcCode=value;
            }
        }

        public string TNA_PID
        {
            get
            {
                return parentFuncCode;
            }
            set
            {
                parentFuncCode=value;
            }
        }

        public string TNA_Text
        {
            get
            {
                return funcTitle;
            }
            set
            {
                funcTitle = value;
            }
        }

        public string TNA_LogicId
        {
            get
            {
                return orderTag;
            }
            set
            {
                orderTag=value;
            }
        }

        private IList<ITvaNode> owner;

        public IList<ITvaNode> Owner
        {
            get { return owner; }
            set { owner = value; }
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
