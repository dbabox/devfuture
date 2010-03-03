#region DaFunction
/*----------------------------------------------------------------
// 文件名：DaFunction.cs
// 功能描述：表dbo.MT_FUNCTION的数据访问层
//
// 
// 创建时间：2009-12-18 DtataAccess template . Ver 5.0.20090413
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
    ///可以继承更上层接口,IFunction ,数据访问层基类DaBase，
    ///DaBase中包含了DbException异常处理静态方法
    public partial class DaFunction : DaBase<MoFunction>
    {
        #region SQL const
        internal const string TABLE_COLUMNS = " FUNC_CODE ,PARENT_FUNC_CODE ,FUNC_TITLE ,FUNC_TYPE ,ORDER_INDEX ,ORDER_TAG ,REM ,CREATE_TIME   ";

        internal const string SQL_INSERT = "INSERT INTO MT_FUNCTION (" + TABLE_COLUMNS + ") VALUES (@FuncCode,@ParentFuncCode,@FuncTitle,@FuncType,@OrderIndex,@OrderTag,@Rem,@CreateTime)";
        internal const string SQL_SELECT = "SELECT " + TABLE_COLUMNS + " FROM MT_FUNCTION  ";
        internal const string SQL_SELECT_ONE = SQL_SELECT + " WHERE FUNC_CODE=@FuncCode";

        internal const string SQL_EXIST = "SELECT COUNT(*) FROM MT_FUNCTION  WHERE FUNC_CODE=@FuncCode  ";
        internal const string SQL_UPDATE = "UPDATE MT_FUNCTION SET PARENT_FUNC_CODE=@ParentFuncCode, FUNC_TITLE=@FuncTitle, FUNC_TYPE=@FuncType, ORDER_INDEX=@OrderIndex, ORDER_TAG=@OrderTag, REM=@Rem, CREATE_TIME=@CreateTime WHERE FUNC_CODE=@FuncCode";

        internal const string SQL_DELETE_DEFAULT = "DELETE FROM MT_FUNCTION ";
        internal const string SQL_DELETE = "DELETE FROM MT_FUNCTION WHERE FUNC_CODE=@FuncCode";

        internal const string SQL_COUNT = "SELECT COUNT(*) FROM MT_FUNCTION ";
        #endregion

        #region Constructor

        /// <summary>
        /// 默认构造函数，使用配置文件中默认的数据库配置。
        /// </summary>
        public DaFunction()
        {
            this.db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 使用指定的databaseName数据库配置。
        /// </summary>
        /// <param name="databaseName">配置文件中数据库的配置名称。</param>
        public DaFunction(string databaseName)
        {
            this.db = DatabaseFactory.CreateDatabase(databaseName);
        }

        public DaFunction(Database db)
        {
            this.db = db;
        }

        #endregion

        //以下是CRUD方法，每个方法都有引入外部事务的版本

        #region 构造器模块

        protected override MoFunction ConstructT()
        {
            return new MoFunction();
        }
        #endregion



        #region Add模块 Helper

        ///build the command object.It never throw exception.
        protected override DbCommand ConstructAddCommand(Database db)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT);

            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString);
            db.AddInParameter(dbCommand, "ParentFuncCode", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncTitle", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncType", DbType.Int32);
            db.AddInParameter(dbCommand, "OrderIndex", DbType.Int32);
            db.AddInParameter(dbCommand, "OrderTag", DbType.AnsiString);
            db.AddInParameter(dbCommand, "Rem", DbType.AnsiString);
            db.AddInParameter(dbCommand, "CreateTime", DbType.DateTime);
            return dbCommand;
        }
        protected override DbCommand ConstructAddCommand(Database db, MoFunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT);
            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString, entity.FuncCode);
            db.AddInParameter(dbCommand, "ParentFuncCode", DbType.AnsiString, entity.ParentFuncCode);
            db.AddInParameter(dbCommand, "FuncTitle", DbType.AnsiString, entity.FuncTitle);
            db.AddInParameter(dbCommand, "FuncType", DbType.Int32, entity.FuncType);
            db.AddInParameter(dbCommand, "OrderIndex", DbType.Int32, entity.OrderIndex);
            db.AddInParameter(dbCommand, "OrderTag", DbType.AnsiString, entity.OrderTag);
            db.AddInParameter(dbCommand, "Rem", DbType.AnsiString, entity.Rem);
            db.AddInParameter(dbCommand, "CreateTime", DbType.DateTime, entity.CreateTime);
            return dbCommand;
        }

        protected override DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, MoFunction entity)
        {
            db.SetParameterValue(addCmd, "FuncCode", entity.FuncCode);
            db.SetParameterValue(addCmd, "ParentFuncCode", entity.ParentFuncCode);
            db.SetParameterValue(addCmd, "FuncTitle", entity.FuncTitle);
            db.SetParameterValue(addCmd, "FuncType", entity.FuncType);
            db.SetParameterValue(addCmd, "OrderIndex", entity.OrderIndex);
            db.SetParameterValue(addCmd, "OrderTag", entity.OrderTag);
            db.SetParameterValue(addCmd, "Rem", entity.Rem);
            db.SetParameterValue(addCmd, "CreateTime", entity.CreateTime);
            return addCmd;
        }

        protected override DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, DataRow row)
        {
            db.SetParameterValue(addCmd, "FuncCode", row["FuncCode"]);
            db.SetParameterValue(addCmd, "ParentFuncCode", row["ParentFuncCode"]);
            db.SetParameterValue(addCmd, "FuncTitle", row["FuncTitle"]);
            db.SetParameterValue(addCmd, "FuncType", row["FuncType"]);
            db.SetParameterValue(addCmd, "OrderIndex", row["OrderIndex"]);
            db.SetParameterValue(addCmd, "OrderTag", row["OrderTag"]);
            db.SetParameterValue(addCmd, "Rem", row["Rem"]);
            db.SetParameterValue(addCmd, "CreateTime", row["CreateTime"]);
            return addCmd;
        }

        #endregion

        #region Update模块 Helper

        protected override DbCommand ConstructUpdateCommand(Database db)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE);
            db.AddInParameter(dbCommand, "ParentFuncCode", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncTitle", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncType", DbType.Int32);
            db.AddInParameter(dbCommand, "OrderIndex", DbType.Int32);
            db.AddInParameter(dbCommand, "OrderTag", DbType.AnsiString);
            db.AddInParameter(dbCommand, "Rem", DbType.AnsiString);
            db.AddInParameter(dbCommand, "CreateTime", DbType.DateTime);
            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString);

            return dbCommand;
        }
        protected override DbCommand ConstructUpdateCommand(Database db, MoFunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE);
            db.AddInParameter(dbCommand, "ParentFuncCode", DbType.AnsiString, entity.ParentFuncCode);
            db.AddInParameter(dbCommand, "FuncTitle", DbType.AnsiString, entity.FuncTitle);
            db.AddInParameter(dbCommand, "FuncType", DbType.Int32, entity.FuncType);
            db.AddInParameter(dbCommand, "OrderIndex", DbType.Int32, entity.OrderIndex);
            db.AddInParameter(dbCommand, "OrderTag", DbType.AnsiString, entity.OrderTag);
            db.AddInParameter(dbCommand, "Rem", DbType.AnsiString, entity.Rem);
            db.AddInParameter(dbCommand, "CreateTime", DbType.DateTime, entity.CreateTime);
            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString, entity.FuncCode);

            return dbCommand;
        }

        protected override DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, MoFunction entity)
        {
            db.SetParameterValue(updateCmd, "ParentFuncCode", entity.ParentFuncCode);
            db.SetParameterValue(updateCmd, "FuncTitle", entity.FuncTitle);
            db.SetParameterValue(updateCmd, "FuncType", entity.FuncType);
            db.SetParameterValue(updateCmd, "OrderIndex", entity.OrderIndex);
            db.SetParameterValue(updateCmd, "OrderTag", entity.OrderTag);
            db.SetParameterValue(updateCmd, "Rem", entity.Rem);
            db.SetParameterValue(updateCmd, "CreateTime", entity.CreateTime);
            db.SetParameterValue(updateCmd, "FuncCode", entity.FuncCode);
            return updateCmd;
        }
        protected override DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, DataRow row)
        {
            db.SetParameterValue(updateCmd, "ParentFuncCode", row["ParentFuncCode"]);
            db.SetParameterValue(updateCmd, "FuncTitle", row["FuncTitle"]);
            db.SetParameterValue(updateCmd, "FuncType", row["FuncType"]);
            db.SetParameterValue(updateCmd, "OrderIndex", row["OrderIndex"]);
            db.SetParameterValue(updateCmd, "OrderTag", row["OrderTag"]);
            db.SetParameterValue(updateCmd, "Rem", row["Rem"]);
            db.SetParameterValue(updateCmd, "CreateTime", row["CreateTime"]);
            db.SetParameterValue(updateCmd, "FuncCode", row["FuncCode"]);
            return updateCmd;
        }

        protected override void PrepareDataAdapterCommand(Database db, out DbCommand dbInsertCommand,
                             out DbCommand dbUpdateCommand, out DbCommand dbDeleteCommand)
        {
            dbInsertCommand = db.GetSqlStringCommand(SQL_INSERT);
            #region set insert cmd parameters
            db.AddInParameter(dbInsertCommand, "FuncCode", DbType.AnsiString, "FUNC_CODE", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "ParentFuncCode", DbType.AnsiString, "PARENT_FUNC_CODE", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "FuncTitle", DbType.AnsiString, "FUNC_TITLE", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "FuncType", DbType.Int32, "FUNC_TYPE", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "OrderIndex", DbType.Int32, "ORDER_INDEX", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "OrderTag", DbType.AnsiString, "ORDER_TAG", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "Rem", DbType.AnsiString, "REM", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "CreateTime", DbType.DateTime, "CREATE_TIME", DataRowVersion.Current);
            #endregion
            dbUpdateCommand = db.GetSqlStringCommand(SQL_UPDATE);
            #region Set update cmd value parameters
            db.AddInParameter(dbUpdateCommand, "ParentFuncCode", DbType.AnsiString, "PARENT_FUNC_CODE", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "FuncTitle", DbType.AnsiString, "FUNC_TITLE", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "FuncType", DbType.Int32, "FUNC_TYPE", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "OrderIndex", DbType.Int32, "ORDER_INDEX", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "OrderTag", DbType.AnsiString, "ORDER_TAG", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "Rem", DbType.AnsiString, "REM", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "CreateTime", DbType.DateTime, "CREATE_TIME", DataRowVersion.Current);
            #endregion
            #region set update cmd pk where parameters
            db.AddInParameter(dbUpdateCommand, "FuncCode", DbType.AnsiString, "FUNC_CODE", DataRowVersion.Current);
            #endregion
            dbDeleteCommand = db.GetSqlStringCommand(SQL_DELETE);
            #region set delete cmd pk where parameters
            db.AddInParameter(dbDeleteCommand, "FuncCode", DbType.AnsiString, "FUNC_CODE", DataRowVersion.Current);
            #endregion
        }
        #endregion

        #region Delete模块 Helper

        protected override DbCommand ConstructDeleteCommand(Database db)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE);

            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString);

            return dbCommand;
        }
        protected override DbCommand ConstructDeleteCommand(Database db, MoFunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE);

            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString, entity.FuncCode);

            return dbCommand;
        }

        protected override DbCommand ConstructDeleteCommandForWhere(Database db, string where)
        {
            return db.GetSqlStringCommand(SQL_DELETE_DEFAULT + where);
        }

        protected override DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, MoFunction entity)
        {
            db.SetParameterValue(deleteCmd, "FuncCode", entity.FuncCode);
            return deleteCmd;
        }

        protected override DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, DataRow row)
        {
            db.SetParameterValue(deleteCmd, "FuncCode", row["FuncCode"]);
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

        protected override DbCommand ConstructSelectOneCommand(Database db, MoFunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ONE);

            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString, entity.FuncCode);
            return dbCommand;
        }

        protected override void PopulateEntityByDataReader(IDataReader reader, ref MoFunction entity)
        {
            if (!reader.IsDBNull(0)) entity.FuncCode = reader.GetString(0);
            if (!reader.IsDBNull(1)) entity.ParentFuncCode = reader.GetString(1);
            if (!reader.IsDBNull(2)) entity.FuncTitle = reader.GetString(2);
            if (!reader.IsDBNull(3)) entity.FuncType = reader.GetInt32(3);
            if (!reader.IsDBNull(4)) entity.OrderIndex = reader.GetInt32(4);
            if (!reader.IsDBNull(5)) entity.OrderTag = reader.GetString(5);
            if (!reader.IsDBNull(6)) entity.Rem = reader.GetString(6);
            if (!reader.IsDBNull(7)) entity.CreateTime = reader.GetDateTime(7);
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

        protected override DbCommand ConstructIsExistCommand(Database db, MoFunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_EXIST);
            db.AddInParameter(dbCommand, "FuncCode", DbType.AnsiString, entity.FuncCode);
            return dbCommand;
        }

        #endregion

     


    }
}
#endregion

