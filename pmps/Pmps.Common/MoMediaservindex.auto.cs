

/*----------------------------------------------------------------//
// 文件名：MoMediaservindex.cs
// 文件功能描述：实体 MoMediaservindex.
// 
// 创建标识：Template  ver 5.0.20090429 2010-02-04
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
#region MoMediaservindex
using System;
using System.Data;
using System.Xml.Serialization;
namespace Pmps.Common
{
	/// <summary>
	/// 业务实体MoMediaservindex
	/// </summary>
    [SoapType("SoapMoMediaservindex", "http://www.china.com")]
	public partial class MoMediaservindex:ICloneable
	{
		#region 表Mediaservindex的字段
		private string servername;
		private string url;
		private string description;
		private DateTime duration;
		#endregion
		
		#region 构造函数
		public MoMediaservindex()
		{
			   duration=new DateTime(1900,1,1);  	    
		   
		}
		#endregion
		
		#region 拷贝构造函数

		public MoMediaservindex(MoMediaservindex item)
		{
			item.CopyTo(this);
		}
		#endregion
		
		#region 带参数构造函数
	    public MoMediaservindex(string servername,string url,string description,DateTime duration)
		{
		this.servername=servername;
		this.url=url;
		this.description=description;
		this.duration=duration;
		}
		#endregion
		
		#region 表Mediaservindex的字段属性
		/// <summary>
		/// SERVERNAME
		/// </summary>
		public string Servername
		{
			get {return servername;}
			set {servername=value;}
		}
		/// <summary>
		/// URL
		/// </summary>
		public string Url
		{
			get {return url;}
			set {url=value;}
		}
		/// <summary>
		/// DESCRIPTION
		/// </summary>
		public string Description
		{
			get {return description;}
			set {description=value;}
		}
		/// <summary>
		/// DURATION
		/// </summary>
		public DateTime Duration
		{
			get {return duration;}
			set {duration=value;}
		}
		#endregion
		
		#region 辅助函数
		/// <summary>
		/// 拷贝函数
		/// </summary>
		public MoMediaservindex CopyTo(MoMediaservindex item)
        {
		    item.servername=this.servername;
		    item.url=this.url;
		    item.description=this.description;
		    item.duration=this.duration;
			return item;
        }
		/// <summary>
		/// 克隆函数
		/// </summary>		
		public object Clone()
        {
            MoMediaservindex mo=new MoMediaservindex();
		    mo.Servername=this.Servername;
		    mo.Url=this.Url;
		    mo.Description=this.Description;
		    mo.Duration=this.Duration;
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
                MoMediaservindex NewMediaservindex = obj as MoMediaservindex;
                if (NewMediaservindex == null)
                {
                    return false;
                }
                else
                {
				if (NewMediaservindex.Servername==this.Servername && NewMediaservindex.Url==this.Url && NewMediaservindex.Description==this.Description && NewMediaservindex.Duration==this.Duration)		
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
		   return String.Format(System.Globalization.CultureInfo.InvariantCulture,"Servername={0}/Url={1}/Description={2}/Duration={3}",servername,url,description,duration);
		 
		}
		#endregion
		
		
		#region DataTable Help Function
		///<summary>
		///DataRow转换成Model
		///</summary>
		public void Row2Model(DataRow row)
		{
		    if(row["ServerName"]!=null && row["ServerName"]!=DBNull.Value)
			{
			    this.Servername = (string)row["ServerName"];
			}
			else
			{
				this.Servername =string.Empty;
			}
		    if(row["URL"]!=null && row["URL"]!=DBNull.Value)
			{
			    this.Url = (string)row["URL"];
			}
			else
			{
				this.Url =string.Empty;
			}
		    if(row["Description"]!=null && row["Description"]!=DBNull.Value)
			{
			    this.Description = (string)row["Description"];
			}
			else
			{
				this.Description =string.Empty;
			}
		    if(row["Duration"]!=null && row["Duration"]!=DBNull.Value)
			{
			    this.Duration = (DateTime)row["Duration"];
			}
			else
			{
				this.Duration =DateTime.MinValue;
			}
		}
		
		///<summary>
		///Model转换成DataRow
		///</summary>
	　　public void Model2Row(DataRow row)
		{
			row["ServerName"] = this.Servername;
			row["URL"] = this.Url;
			row["Description"] = this.Description;
			row["Duration"] = this.Duration;
		}
		#endregion
	}
}
#endregion
