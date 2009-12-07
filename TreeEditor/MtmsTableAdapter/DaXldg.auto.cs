#region DaXldg
/*----------------------------------------------------------------
// 文件名：DaXldg.cs
// 功能描述：表dbo.MT_XLDG的数据访问层
//
// 
// 创建时间：2009-12-04 DtataAccess template . Ver 5.0.20090413
//
// 修改标识： 
// 修改描述： 
//----------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using Mtms.Model;
using TreeEditor.Core; 


namespace Mtms.Dal
{
	///可以继承更上层接口,IXldg ,数据访问层基类DaBase，
	///DaBase中包含了DbException异常处理静态方法
    public	partial  class DaXldg:DaBase<MoXldg>
    {	 
		#region SQL const
		internal const string TABLE_COLUMNS=" DG_ID ,DG_NAME ,DG_SIGN1 ,DG_SIGN2 ,DG_PARENT ,DG_CLASS1 ,DG_CLASS2 ,DG_ISDEL   ";
		
		internal const string SQL_INSERT="INSERT INTO MT_XLDG ("+TABLE_COLUMNS+") VALUES (@DgId,@DgName,@DgSign1,@DgSign2,@DgParent,@DgClass1,@DgClass2,@DgIsdel)";
		internal const string SQL_SELECT="SELECT "+TABLE_COLUMNS+" FROM MT_XLDG  ";
		internal const string SQL_SELECT_ONE=SQL_SELECT+" WHERE DG_ID=@DgId";                              
		
		internal const string SQL_EXIST="SELECT COUNT(*) FROM MT_XLDG  WHERE DG_ID=@DgId  ";
		internal const string SQL_UPDATE="UPDATE MT_XLDG SET DG_NAME=@DgName, DG_SIGN1=@DgSign1, DG_SIGN2=@DgSign2, DG_PARENT=@DgParent, DG_CLASS1=@DgClass1, DG_CLASS2=@DgClass2, DG_ISDEL=@DgIsdel WHERE DG_ID=@DgId";
		
		internal const string SQL_DELETE_DEFAULT = "DELETE FROM MT_XLDG ";
		internal const string SQL_DELETE="DELETE FROM MT_XLDG WHERE DG_ID=@DgId";
		
		internal const string SQL_COUNT="SELECT COUNT(*) FROM MT_XLDG ";
		#endregion
		       
		#region Constructor
	 
		/// <summary>
        /// 默认构造函数，使用配置文件中默认的数据库配置。
        /// </summary>
		public DaXldg ()
		{ 
			this.db=DatabaseFactory.CreateDatabase();
		}
		/// <summary>
        /// 使用指定的databaseName数据库配置。
        /// </summary>
        /// <param name="databaseName">配置文件中数据库的配置名称。</param>
		public DaXldg (string databaseName)
		{ 
            this.db = DatabaseFactory.CreateDatabase(databaseName);
		} 
		
		public DaXldg (Database db)
		{ 
            this.db = db;
		} 
		
		#endregion
		
		//以下是CRUD方法，每个方法都有引入外部事务的版本

		#region 构造器模块
		
        protected override MoXldg ConstructT()
        {
            return new MoXldg();
        }
        #endregion
		
	    
		
		#region Add模块 Helper
		
		///build the command object.It never throw exception.
		protected override  DbCommand ConstructAddCommand(Database db)
		{
			DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT);
			 
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgName",DbType.String);
			db.AddInParameter(dbCommand,"DgSign1",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgSign2",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgParent",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgClass1",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgClass2",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgIsdel",DbType.AnsiString);
			return dbCommand;
		}		
		protected override DbCommand ConstructAddCommand(Database db,MoXldg entity)
		{
			DbCommand dbCommand=db.GetSqlStringCommand(SQL_INSERT);			 
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString,entity.DgId);
			db.AddInParameter(dbCommand,"DgName",DbType.String,entity.DgName);
			db.AddInParameter(dbCommand,"DgSign1",DbType.AnsiString,entity.DgSign1);
			db.AddInParameter(dbCommand,"DgSign2",DbType.AnsiString,entity.DgSign2);
			db.AddInParameter(dbCommand,"DgParent",DbType.AnsiString,entity.DgParent);
			db.AddInParameter(dbCommand,"DgClass1",DbType.AnsiString,entity.DgClass1);
			db.AddInParameter(dbCommand,"DgClass2",DbType.AnsiString,entity.DgClass2);
			db.AddInParameter(dbCommand,"DgIsdel",DbType.AnsiString,entity.DgIsdel);
			return dbCommand;
		}
		
		protected override DbCommand PopulateAddCommandParameters(Database db,DbCommand addCmd,MoXldg entity)
		{
			db.SetParameterValue(addCmd,"DgId",entity.DgId);
			db.SetParameterValue(addCmd,"DgName",entity.DgName);
			db.SetParameterValue(addCmd,"DgSign1",entity.DgSign1);
			db.SetParameterValue(addCmd,"DgSign2",entity.DgSign2);
			db.SetParameterValue(addCmd,"DgParent",entity.DgParent);
			db.SetParameterValue(addCmd,"DgClass1",entity.DgClass1);
			db.SetParameterValue(addCmd,"DgClass2",entity.DgClass2);
			db.SetParameterValue(addCmd,"DgIsdel",entity.DgIsdel);
			return addCmd;
		}
		
		protected override DbCommand PopulateAddCommandParameters(Database db,DbCommand addCmd,DataRow row)
		{
			db.SetParameterValue(addCmd,"DgId",row["DgId"]);
			db.SetParameterValue(addCmd,"DgName",row["DgName"]);
			db.SetParameterValue(addCmd,"DgSign1",row["DgSign1"]);
			db.SetParameterValue(addCmd,"DgSign2",row["DgSign2"]);
			db.SetParameterValue(addCmd,"DgParent",row["DgParent"]);
			db.SetParameterValue(addCmd,"DgClass1",row["DgClass1"]);
			db.SetParameterValue(addCmd,"DgClass2",row["DgClass2"]);
			db.SetParameterValue(addCmd,"DgIsdel",row["DgIsdel"]);
			return addCmd;
		}
		
		#endregion
		    
		#region Update模块 Helper
		
		protected override DbCommand ConstructUpdateCommand(Database db)
		{
			DbCommand dbCommand=db.GetSqlStringCommand(SQL_UPDATE);		    
			db.AddInParameter(dbCommand,"DgName",DbType.String);
			db.AddInParameter(dbCommand,"DgSign1",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgSign2",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgParent",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgClass1",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgClass2",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgIsdel",DbType.AnsiString);
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString);
			 
			return dbCommand;
		}
		protected override DbCommand ConstructUpdateCommand(Database db,MoXldg entity)
		{
			DbCommand dbCommand=db.GetSqlStringCommand(SQL_UPDATE);		    
			db.AddInParameter(dbCommand,"DgName",DbType.String,entity.DgName);
			db.AddInParameter(dbCommand,"DgSign1",DbType.AnsiString,entity.DgSign1);
			db.AddInParameter(dbCommand,"DgSign2",DbType.AnsiString,entity.DgSign2);
			db.AddInParameter(dbCommand,"DgParent",DbType.AnsiString,entity.DgParent);
			db.AddInParameter(dbCommand,"DgClass1",DbType.AnsiString,entity.DgClass1);
			db.AddInParameter(dbCommand,"DgClass2",DbType.AnsiString,entity.DgClass2);
			db.AddInParameter(dbCommand,"DgIsdel",DbType.AnsiString,entity.DgIsdel);
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString,entity.DgId);
			 
			return dbCommand;
		}
		
		protected override DbCommand PopulateUpdateCommandParameters(Database db,DbCommand updateCmd,MoXldg entity)
		{
			db.SetParameterValue(updateCmd,"DgName",entity.DgName);
			db.SetParameterValue(updateCmd,"DgSign1",entity.DgSign1);
			db.SetParameterValue(updateCmd,"DgSign2",entity.DgSign2);
			db.SetParameterValue(updateCmd,"DgParent",entity.DgParent);
			db.SetParameterValue(updateCmd,"DgClass1",entity.DgClass1);
			db.SetParameterValue(updateCmd,"DgClass2",entity.DgClass2);
			db.SetParameterValue(updateCmd,"DgIsdel",entity.DgIsdel);
			db.SetParameterValue(updateCmd,"DgId",entity.DgId);
			return updateCmd;
		}
		protected override DbCommand PopulateUpdateCommandParameters(Database db,DbCommand updateCmd,DataRow row)
		{
			db.SetParameterValue(updateCmd,"DgName",row["DgName"]);
			db.SetParameterValue(updateCmd,"DgSign1",row["DgSign1"]);
			db.SetParameterValue(updateCmd,"DgSign2",row["DgSign2"]);
			db.SetParameterValue(updateCmd,"DgParent",row["DgParent"]);
			db.SetParameterValue(updateCmd,"DgClass1",row["DgClass1"]);
			db.SetParameterValue(updateCmd,"DgClass2",row["DgClass2"]);
			db.SetParameterValue(updateCmd,"DgIsdel",row["DgIsdel"]);
			db.SetParameterValue(updateCmd,"DgId",row["DgId"]);
			return updateCmd;
		}
		
		protected override void PrepareDataAdapterCommand(Database db,out DbCommand dbInsertCommand,
		                     out DbCommand dbUpdateCommand,out DbCommand dbDeleteCommand)
		{
			dbInsertCommand = db.GetSqlStringCommand(SQL_INSERT);
			#region set insert cmd parameters
			db.AddInParameter(dbInsertCommand, "DgId",DbType.AnsiString, "DG_ID", DataRowVersion.Current);
			db.AddInParameter(dbInsertCommand, "DgName",DbType.String, "DG_NAME", DataRowVersion.Current);
			db.AddInParameter(dbInsertCommand, "DgSign1",DbType.AnsiString, "DG_SIGN1", DataRowVersion.Current);
			db.AddInParameter(dbInsertCommand, "DgSign2",DbType.AnsiString, "DG_SIGN2", DataRowVersion.Current);
			db.AddInParameter(dbInsertCommand, "DgParent",DbType.AnsiString, "DG_PARENT", DataRowVersion.Current);
			db.AddInParameter(dbInsertCommand, "DgClass1",DbType.AnsiString, "DG_CLASS1", DataRowVersion.Current);
			db.AddInParameter(dbInsertCommand, "DgClass2",DbType.AnsiString, "DG_CLASS2", DataRowVersion.Current);
			db.AddInParameter(dbInsertCommand, "DgIsdel",DbType.AnsiString, "DG_ISDEL", DataRowVersion.Current);
			#endregion
			dbUpdateCommand = db.GetSqlStringCommand(SQL_UPDATE);
			#region Set update cmd value parameters
			db.AddInParameter(dbUpdateCommand, "DgName", DbType.String, "DG_NAME", DataRowVersion.Current);
			db.AddInParameter(dbUpdateCommand, "DgSign1", DbType.AnsiString, "DG_SIGN1", DataRowVersion.Current);
			db.AddInParameter(dbUpdateCommand, "DgSign2", DbType.AnsiString, "DG_SIGN2", DataRowVersion.Current);
			db.AddInParameter(dbUpdateCommand, "DgParent", DbType.AnsiString, "DG_PARENT", DataRowVersion.Current);
			db.AddInParameter(dbUpdateCommand, "DgClass1", DbType.AnsiString, "DG_CLASS1", DataRowVersion.Current);
			db.AddInParameter(dbUpdateCommand, "DgClass2", DbType.AnsiString, "DG_CLASS2", DataRowVersion.Current);
			db.AddInParameter(dbUpdateCommand, "DgIsdel", DbType.AnsiString, "DG_ISDEL", DataRowVersion.Current);
			#endregion
			#region set update cmd pk where parameters
			db.AddInParameter(dbUpdateCommand, "DgId", DbType.AnsiString, "DG_ID", DataRowVersion.Current);
			#endregion			
			dbDeleteCommand = db.GetSqlStringCommand(SQL_DELETE);
			#region set delete cmd pk where parameters
			db.AddInParameter(dbDeleteCommand, "DgId", DbType.AnsiString, "DG_ID", DataRowVersion.Current);
			#endregion
		}
		#endregion	
		
		#region Delete模块 Helper		
		
		protected override DbCommand ConstructDeleteCommand(Database db)
		{
			DbCommand dbCommand=db.GetSqlStringCommand(SQL_DELETE);
			 
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString);
			 
			return dbCommand;
		}		
		protected override DbCommand ConstructDeleteCommand(Database db,MoXldg entity)
		{
			DbCommand dbCommand=db.GetSqlStringCommand(SQL_DELETE);
			 
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString,entity.DgId);
		 
			return dbCommand;
		}
		
		protected override DbCommand ConstructDeleteCommandForWhere(Database db, string where)
        {
            return db.GetSqlStringCommand(SQL_DELETE_DEFAULT + where);            
        }

        protected override DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, MoXldg entity)
        {
			db.SetParameterValue(deleteCmd,"DgId",entity.DgId);
			return deleteCmd;
        }

        protected override DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, DataRow row)
        {
			db.SetParameterValue(deleteCmd,"DgId",row["DgId"]);
			return deleteCmd;
        }      
		
		#endregion helper
		
		#region Query模块 Helper

        protected override DbCommand ConstructQueryCommand(string condition)
        {
            return db.GetSqlStringCommand(SQL_SELECT + condition);
        }

        #endregion
		
		#region GetEntity(s)模块 Helper
        
		protected override DbCommand ConstructSelectOneCommand(Database db,MoXldg entity)
		{
			DbCommand dbCommand=db.GetSqlStringCommand(SQL_SELECT_ONE);
			 
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString,entity.DgId);
			return dbCommand;
		}
		
		protected override void PopulateEntityByDataReader(IDataReader reader,ref MoXldg entity)
		{
			if (!reader.IsDBNull(0)) entity.DgId = reader.GetString(0);
			if (!reader.IsDBNull(1)) entity.DgName = reader.GetString(1);
			if (!reader.IsDBNull(2)) entity.DgSign1 = reader.GetString(2);
			if (!reader.IsDBNull(3)) entity.DgSign2 = reader.GetString(3);
			if (!reader.IsDBNull(4)) entity.DgParent = reader.GetString(4);
			if (!reader.IsDBNull(5)) entity.DgClass1 = reader.GetString(5);
			if (!reader.IsDBNull(6)) entity.DgClass2 = reader.GetString(6);
			if (!reader.IsDBNull(7)) entity.DgIsdel = reader.GetString(7);
		}
		
		

        protected override DbCommand ConstructSelectConditionCommand(Database db, string condition)
        {
            return db.GetSqlStringCommand(SQL_SELECT + condition);
        }
		
		#endregion	
		
		#region Count模块 Helper

        protected override DbCommand ConstructCountCommand(Database db)
        {
            return db.GetSqlStringCommand(SQL_COUNT);
        }

        protected override DbCommand ConstructCountConditionCommand(Database db, string condition)
        {
            return db.GetSqlStringCommand(SQL_COUNT + condition);
        }

		#endregion
		
		#region IsExist模块 Helper

        protected override DbCommand ConstructIsExistCommand(Database db, MoXldg entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_EXIST);
			db.AddInParameter(dbCommand,"DgId",DbType.AnsiString,entity.DgId);
			return dbCommand;
        }

		#endregion

        /// <summary>
        /// 保存所有节点改变
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Save(IList<MoXldg> list, bool force)
        {
            int rc = 0;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    if (force) DeleteByWhereClause(null, trans);

                    for (int i = 0; i < list.Count; i++)
                    {
                        MoXldg mo = list[i];
                        if (IsExist(mo, trans))
                        {
                            rc += Update(mo, trans);
                        }
                        else
                        {
                            rc += Add(mo, trans);
                        }
                    }

                    trans.Commit();
                }
                conn.Close();
            }
            return rc;
        }

		
		
	}
}	
#endregion

