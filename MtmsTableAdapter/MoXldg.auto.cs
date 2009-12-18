
/*----------------------------------------------------------------//
// 文件名：MoXldg.cs
// 文件功能描述：实体 MoXldg.
// 
// 创建标识：Template  ver 5.0.20090429 2009-12-04
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
#region MoXldg
using System;
using System.Data;
using TreeEditor.Core;
using System.Collections.Generic;
namespace Mtms.Model
{
	/// <summary>
	/// 业务实体MoXldg
	/// </summary>
    public partial class MoXldg : ITvaNode
	{
		#region 表Xldg的字段
		private string dgId;
		private string dgName;
		private string dgSign1;
		private string dgSign2;
		private string dgParent;
		private string dgClass1;
		private string dgClass2;
		private string dgIsdel;
		#endregion
		
		#region 构造函数
		public MoXldg()
		{
			         	    
		   
		}
		#endregion
		
		#region 拷贝构造函数

		public MoXldg(MoXldg item)
		{
			item.CopyTo(this);
		}
		#endregion
		
		#region 带参数构造函数
	    public MoXldg(string dgId,string dgName,string dgSign1,string dgSign2,string dgParent,string dgClass1,string dgClass2,string dgIsdel)
		{
		this.dgId=dgId;
		this.dgName=dgName;
		this.dgSign1=dgSign1;
		this.dgSign2=dgSign2;
		this.dgParent=dgParent;
		this.dgClass1=dgClass1;
		this.dgClass2=dgClass2;
		this.dgIsdel=dgIsdel;
		}
		#endregion
		
		#region 表Xldg的字段属性
		/// <summary>
		/// DG_ID
		/// </summary>
		public string DgId
		{
			get {return dgId;}
			set {dgId=value;}
		}
		/// <summary>
		/// DG_NAME
		/// </summary>
		public string DgName
		{
			get {return dgName;}
			set {dgName=value;}
		}
		/// <summary>
		/// DG_SIGN1
		/// </summary>
		public string DgSign1
		{
			get {return dgSign1;}
			set {dgSign1=value;}
		}
		/// <summary>
		/// DG_SIGN2
		/// </summary>
		public string DgSign2
		{
			get {return dgSign2;}
			set {dgSign2=value;}
		}
		/// <summary>
		/// DG_PARENT
		/// </summary>
		public string DgParent
		{
			get {return dgParent;}
			set {dgParent=value;}
		}
		/// <summary>
		/// DG_CLASS1
		/// </summary>
		public string DgClass1
		{
			get {return dgClass1;}
			set {dgClass1=value;}
		}
		/// <summary>
		/// DG_CLASS2
		/// </summary>
		public string DgClass2
		{
			get {return dgClass2;}
			set {dgClass2=value;}
		}
		/// <summary>
		/// DG_ISDEL
		/// </summary>
		public string DgIsdel
		{
			get {return dgIsdel;}
			set {dgIsdel=value;}
		}
		#endregion
		
		#region 辅助函数
		/// <summary>
		/// 拷贝函数
		/// </summary>
		public MoXldg CopyTo(MoXldg item)
        {
		    item.dgId=this.dgId;
		    item.dgName=this.dgName;
		    item.dgSign1=this.dgSign1;
		    item.dgSign2=this.dgSign2;
		    item.dgParent=this.dgParent;
		    item.dgClass1=this.dgClass1;
		    item.dgClass2=this.dgClass2;
		    item.dgIsdel=this.dgIsdel;
			return item;
        }
		/// <summary>
		/// 克隆函数
		/// </summary>		
		public object Clone()
        {
            MoXldg mo=new MoXldg();
		    mo.DgId=this.DgId;
		    mo.DgName=this.DgName;
		    mo.DgSign1=this.DgSign1;
		    mo.DgSign2=this.DgSign2;
		    mo.DgParent=this.DgParent;
		    mo.DgClass1=this.DgClass1;
		    mo.DgClass2=this.DgClass2;
		    mo.DgIsdel=this.DgIsdel;
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
                MoXldg NewXldg = obj as MoXldg;
                if (NewXldg == null)
                {
                    return false;
                }
                else
                {
				if (NewXldg.DgId==this.DgId && NewXldg.DgName==this.DgName && NewXldg.DgSign1==this.DgSign1 && NewXldg.DgSign2==this.DgSign2 && NewXldg.DgParent==this.DgParent && NewXldg.DgClass1==this.DgClass1 && NewXldg.DgClass2==this.DgClass2 && NewXldg.DgIsdel==this.DgIsdel)		
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
		   return String.Format(System.Globalization.CultureInfo.InvariantCulture,"DgId={0}/DgName={1}/DgSign1={2}/DgSign2={3}/DgParent={4}/DgClass1={5}/DgClass2={6}/DgIsdel={7}",dgId,dgName,dgSign1,dgSign2,dgParent,dgClass1,dgClass2,dgIsdel);
		 
		}
		#endregion
		
		
		#region DataTable Help Function
		///<summary>
		///DataRow转换成Model
		///</summary>
		public void Row2Model(DataRow row)
		{
		    if(row["DG_ID"]!=null && row["DG_ID"]!=DBNull.Value)
			{
			    this.DgId = (string)row["DG_ID"];
			}
			else
			{
				this.DgId =string.Empty;
			}
		    if(row["DG_NAME"]!=null && row["DG_NAME"]!=DBNull.Value)
			{
			    this.DgName = (string)row["DG_NAME"];
			}
			else
			{
				this.DgName =string.Empty;
			}
		    if(row["DG_SIGN1"]!=null && row["DG_SIGN1"]!=DBNull.Value)
			{
			    this.DgSign1 = (string)row["DG_SIGN1"];
			}
			else
			{
				this.DgSign1 =string.Empty;
			}
		    if(row["DG_SIGN2"]!=null && row["DG_SIGN2"]!=DBNull.Value)
			{
			    this.DgSign2 = (string)row["DG_SIGN2"];
			}
			else
			{
				this.DgSign2 =string.Empty;
			}
		    if(row["DG_PARENT"]!=null && row["DG_PARENT"]!=DBNull.Value)
			{
			    this.DgParent = (string)row["DG_PARENT"];
			}
			else
			{
				this.DgParent =string.Empty;
			}
		    if(row["DG_CLASS1"]!=null && row["DG_CLASS1"]!=DBNull.Value)
			{
			    this.DgClass1 = (string)row["DG_CLASS1"];
			}
			else
			{
				this.DgClass1 =string.Empty;
			}
		    if(row["DG_CLASS2"]!=null && row["DG_CLASS2"]!=DBNull.Value)
			{
			    this.DgClass2 = (string)row["DG_CLASS2"];
			}
			else
			{
				this.DgClass2 =string.Empty;
			}
		    if(row["DG_ISDEL"]!=null && row["DG_ISDEL"]!=DBNull.Value)
			{
			    this.DgIsdel = (string)row["DG_ISDEL"];
			}
			else
			{
				this.DgIsdel =string.Empty;
			}
		}
		
		///<summary>
		///Model转换成DataRow
		///</summary>
	　　public void Model2Row(DataRow row)
		{
			row["DG_ID"] = this.DgId;
			row["DG_NAME"] = this.DgName;
			row["DG_SIGN1"] = this.DgSign1;
			row["DG_SIGN2"] = this.DgSign2;
			row["DG_PARENT"] = this.DgParent;
			row["DG_CLASS1"] = this.DgClass1;
			row["DG_CLASS2"] = this.DgClass2;
			row["DG_ISDEL"] = this.DgIsdel;
		}
		#endregion

        #region ITvaNode 成员

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
                return dgClass2;
            }
            set
            {
                dgClass2 = value;
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
                return dgName;
            }
            set
            {
                dgName=value;
            }
        }

        public string TNA_ID
        {
            get
            {
                return dgId;
            }
            set
            {
                dgId=value;
            }
        }

        public string TNA_PID
        {
            get
            {
                return dgParent;
            }
            set
            {
                dgParent=value;
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
