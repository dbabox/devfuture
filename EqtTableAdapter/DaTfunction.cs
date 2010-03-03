#region DaTfunction
/*----------------------------------------------------------------
// �ļ�����DaTfunction.cs
// ������������dbo.TFunction�����ݷ��ʲ�
//
// 
// ����ʱ�䣺2009-11-20 DtataAccess template . Ver 5.0.20090413
//
// �޸ı�ʶ�� 
// �޸������� 
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

using EQT.Model;
using TreeEditor.Core;


namespace EQT.Dal
{
    ///���Լ̳и��ϲ�ӿ�,ITfunction ,���ݷ��ʲ����DaBase��
    ///DaBase�а�����DbException�쳣����̬����
    public partial class DaTfunction : DaBase<MoTfunction>
    {
        #region SQL const
        internal const string TABLE_COLUMNS = " TFUNCTION.FUNC_ID ,TFUNCTION.FUNC_NAME ,FUNC_CLASS ,FUNC_SIGN ,TFUNCTION.REM ,ORDER_NUM ,PARENT_FUNC   ";

        internal const string SQL_INSERT = "INSERT INTO TFUNCTION (" + TABLE_COLUMNS + ") VALUES (@FuncId,@FuncName,@FuncClass,@FuncSign,@Rem,@OrderNum,@ParentFunc)";
        internal const string SQL_SELECT = "SELECT " + TABLE_COLUMNS + " FROM TFUNCTION  ";
        internal const string SQL_SELECT_ONE = SQL_SELECT + " WHERE FUNC_ID=@FuncId";

        internal const string SQL_EXIST = "SELECT COUNT(*) FROM TFUNCTION  WHERE FUNC_ID=@FuncId  ";
        internal const string SQL_UPDATE = "UPDATE TFUNCTION SET FUNC_NAME=@FuncName, FUNC_CLASS=@FuncClass, FUNC_SIGN=@FuncSign, REM=@Rem, ORDER_NUM=@OrderNum, PARENT_FUNC=@ParentFunc WHERE FUNC_ID=@FuncId";

        internal const string SQL_DELETE_DEFAULT = "DELETE FROM TFUNCTION ";
        internal const string SQL_DELETE = "DELETE FROM TFUNCTION WHERE FUNC_ID=@FuncId";

        internal const string SQL_COUNT = "SELECT COUNT(*) FROM TFunction ";
        #endregion

        #region Constructor

        /// <summary>
        /// Ĭ�Ϲ��캯����ʹ�������ļ���Ĭ�ϵ����ݿ����á�
        /// </summary>
        public DaTfunction()
        {
            this.db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// ʹ��ָ����databaseName���ݿ����á�
        /// </summary>
        /// <param name="databaseName">�����ļ������ݿ���������ơ�</param>
        public DaTfunction(string databaseName)
        {
            this.db = DatabaseFactory.CreateDatabase(databaseName);
        }

        public DaTfunction(Database db)
        {
            this.db = db;
        }

        #endregion

        //������CRUD������ÿ���������������ⲿ����İ汾

        #region ������ģ��

        protected override MoTfunction ConstructT()
        {
            return new MoTfunction();
        }
        #endregion



        #region Addģ�� Helper

        ///build the command object.It never throw exception.
        protected override DbCommand ConstructAddCommand(Database db)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT);

            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncName", DbType.String);
            db.AddInParameter(dbCommand, "FuncClass", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncSign", DbType.AnsiString);
            db.AddInParameter(dbCommand, "Rem", DbType.String);
            db.AddInParameter(dbCommand, "OrderNum", DbType.AnsiString);
            db.AddInParameter(dbCommand, "ParentFunc", DbType.AnsiString);
            return dbCommand;
        }
        protected override DbCommand ConstructAddCommand(Database db, MoTfunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_INSERT);
            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString, entity.FuncId);
            db.AddInParameter(dbCommand, "FuncName", DbType.String, entity.FuncName);
            db.AddInParameter(dbCommand, "FuncClass", DbType.AnsiString, entity.FuncClass);
            db.AddInParameter(dbCommand, "FuncSign", DbType.AnsiString, entity.FuncSign);
            db.AddInParameter(dbCommand, "Rem", DbType.String, entity.Rem);
            db.AddInParameter(dbCommand, "OrderNum", DbType.AnsiString, entity.OrderNum);
            db.AddInParameter(dbCommand, "ParentFunc", DbType.AnsiString, entity.ParentFunc);
            return dbCommand;
        }

        protected override DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, MoTfunction entity)
        {
            db.SetParameterValue(addCmd, "FuncId", entity.FuncId);
            db.SetParameterValue(addCmd, "FuncName", entity.FuncName);
            db.SetParameterValue(addCmd, "FuncClass", entity.FuncClass);
            db.SetParameterValue(addCmd, "FuncSign", entity.FuncSign);
            db.SetParameterValue(addCmd, "Rem", entity.Rem);
            db.SetParameterValue(addCmd, "OrderNum", entity.OrderNum);
            db.SetParameterValue(addCmd, "ParentFunc", entity.ParentFunc);
            return addCmd;
        }

        protected override DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, DataRow row)
        {
            db.SetParameterValue(addCmd, "FuncId", row["FuncId"]);
            db.SetParameterValue(addCmd, "FuncName", row["FuncName"]);
            db.SetParameterValue(addCmd, "FuncClass", row["FuncClass"]);
            db.SetParameterValue(addCmd, "FuncSign", row["FuncSign"]);
            db.SetParameterValue(addCmd, "Rem", row["Rem"]);
            db.SetParameterValue(addCmd, "OrderNum", row["OrderNum"]);
            db.SetParameterValue(addCmd, "ParentFunc", row["ParentFunc"]);
            return addCmd;
        }

        #endregion

        #region Updateģ�� Helper

        protected override DbCommand ConstructUpdateCommand(Database db)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE);
            db.AddInParameter(dbCommand, "FuncName", DbType.String);
            db.AddInParameter(dbCommand, "FuncClass", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncSign", DbType.AnsiString);
            db.AddInParameter(dbCommand, "Rem", DbType.String);
            db.AddInParameter(dbCommand, "OrderNum", DbType.AnsiString);
            db.AddInParameter(dbCommand, "ParentFunc", DbType.AnsiString);
            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString);

            return dbCommand;
        }
        protected override DbCommand ConstructUpdateCommand(Database db, MoTfunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_UPDATE);
            db.AddInParameter(dbCommand, "FuncName", DbType.String, entity.FuncName);
            db.AddInParameter(dbCommand, "FuncClass", DbType.AnsiString, entity.FuncClass);
            db.AddInParameter(dbCommand, "FuncSign", DbType.AnsiString, entity.FuncSign);
            db.AddInParameter(dbCommand, "Rem", DbType.String, entity.Rem);
            db.AddInParameter(dbCommand, "OrderNum", DbType.AnsiString, entity.OrderNum);
            db.AddInParameter(dbCommand, "ParentFunc", DbType.AnsiString, entity.ParentFunc);
            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString, entity.FuncId);

            return dbCommand;
        }

        protected override DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, MoTfunction entity)
        {
            db.SetParameterValue(updateCmd, "FuncName", entity.FuncName);
            db.SetParameterValue(updateCmd, "FuncClass", entity.FuncClass);
            db.SetParameterValue(updateCmd, "FuncSign", entity.FuncSign);
            db.SetParameterValue(updateCmd, "Rem", entity.Rem);
            db.SetParameterValue(updateCmd, "OrderNum", entity.OrderNum);
            db.SetParameterValue(updateCmd, "ParentFunc", entity.ParentFunc);
            db.SetParameterValue(updateCmd, "FuncId", entity.FuncId);
            return updateCmd;
        }
        protected override DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, DataRow row)
        {
            db.SetParameterValue(updateCmd, "FuncName", row["FuncName"]);
            db.SetParameterValue(updateCmd, "FuncClass", row["FuncClass"]);
            db.SetParameterValue(updateCmd, "FuncSign", row["FuncSign"]);
            db.SetParameterValue(updateCmd, "Rem", row["Rem"]);
            db.SetParameterValue(updateCmd, "OrderNum", row["OrderNum"]);
            db.SetParameterValue(updateCmd, "ParentFunc", row["ParentFunc"]);
            db.SetParameterValue(updateCmd, "FuncId", row["FuncId"]);
            return updateCmd;
        }

        protected override void PrepareDataAdapterCommand(Database db, out DbCommand dbInsertCommand,
                             out DbCommand dbUpdateCommand, out DbCommand dbDeleteCommand)
        {
            dbInsertCommand = db.GetSqlStringCommand(SQL_INSERT);
            #region set insert cmd parameters
            db.AddInParameter(dbInsertCommand, "FuncId", DbType.AnsiString, "FUNC_ID", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "FuncName", DbType.String, "FUNC_NAME", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "FuncClass", DbType.AnsiString, "FUNC_CLASS", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "FuncSign", DbType.AnsiString, "FUNC_SIGN", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "Rem", DbType.String, "REM", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "OrderNum", DbType.AnsiString, "ORDER_NUM", DataRowVersion.Current);
            db.AddInParameter(dbInsertCommand, "ParentFunc", DbType.AnsiString, "PARENT_FUNC", DataRowVersion.Current);
            #endregion
            dbUpdateCommand = db.GetSqlStringCommand(SQL_UPDATE);
            #region Set update cmd value parameters
            db.AddInParameter(dbUpdateCommand, "FuncName", DbType.String, "FUNC_NAME", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "FuncClass", DbType.AnsiString, "FUNC_CLASS", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "FuncSign", DbType.AnsiString, "FUNC_SIGN", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "Rem", DbType.String, "REM", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "OrderNum", DbType.AnsiString, "ORDER_NUM", DataRowVersion.Current);
            db.AddInParameter(dbUpdateCommand, "ParentFunc", DbType.AnsiString, "PARENT_FUNC", DataRowVersion.Current);
            #endregion
            #region set update cmd pk where parameters
            db.AddInParameter(dbUpdateCommand, "FuncId", DbType.AnsiString, "FUNC_ID", DataRowVersion.Current);
            #endregion
            dbDeleteCommand = db.GetSqlStringCommand(SQL_DELETE);
            #region set delete cmd pk where parameters
            db.AddInParameter(dbDeleteCommand, "FuncId", DbType.AnsiString, "FUNC_ID", DataRowVersion.Current);
            #endregion
        }
        #endregion

        #region Deleteģ�� Helper

        protected override DbCommand ConstructDeleteCommand(Database db)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE);

            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString);

            return dbCommand;
        }
        protected override DbCommand ConstructDeleteCommand(Database db, MoTfunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_DELETE);

            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString, entity.FuncId);

            return dbCommand;
        }

        protected override DbCommand ConstructDeleteCommandForWhere(Database db, string where)
        {
            return db.GetSqlStringCommand(SQL_DELETE_DEFAULT + where);
        }

        protected override DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, MoTfunction entity)
        {
            db.SetParameterValue(deleteCmd, "FuncId", entity.FuncId);
            return deleteCmd;
        }

        protected override DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, DataRow row)
        {
            db.SetParameterValue(deleteCmd, "FuncId", row["FuncId"]);
            return deleteCmd;
        }

        #endregion helper

        #region Queryģ�� Helper

        protected override DbCommand ConstructQueryCommand(string condition)
        {
            return db.GetSqlStringCommand(SQL_SELECT + condition);
        }

        #endregion

        #region GetEntity(s)ģ�� Helper

        protected override DbCommand ConstructSelectOneCommand(Database db, MoTfunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_SELECT_ONE);

            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString, entity.FuncId);
            return dbCommand;
        }

        protected override void PopulateEntityByDataReader(IDataReader reader, ref MoTfunction entity)
        {
            if (!reader.IsDBNull(0)) entity.FuncId = reader.GetString(0);
            if (!reader.IsDBNull(1)) entity.FuncName = reader.GetString(1);
            if (!reader.IsDBNull(2)) entity.FuncClass = reader.GetString(2);
            if (!reader.IsDBNull(3)) entity.FuncSign = reader.GetString(3);
            if (!reader.IsDBNull(4)) entity.Rem = reader.GetString(4);
            if (!reader.IsDBNull(5)) entity.OrderNum = reader.GetString(5);
            if (!reader.IsDBNull(6)) entity.ParentFunc = reader.GetString(6);
        }



        protected override DbCommand ConstructSelectConditionCommand(Database db, string condition)
        {
            return db.GetSqlStringCommand(SQL_SELECT + condition);
        }

        #endregion

        #region Countģ�� Helper

        protected override DbCommand ConstructCountCommand(Database db)
        {
            return db.GetSqlStringCommand(SQL_COUNT);
        }

        protected override DbCommand ConstructCountConditionCommand(Database db, string condition)
        {
            return db.GetSqlStringCommand(SQL_COUNT + condition);
        }

        #endregion

        #region IsExistģ�� Helper

        protected override DbCommand ConstructIsExistCommand(Database db, MoTfunction entity)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(SQL_EXIST);
            db.AddInParameter(dbCommand, "FuncId", DbType.AnsiString, entity.FuncId);
            return dbCommand;
        }

        #endregion

        const string sql_updatepid = "update TFUNCTION set PARENT_FUNC='{0}' where FUNC_ID='{1}'";

        public int UpdatePID(string id, string pid)
        {
            string sql = String.Format(sql_updatepid, pid, id);
            return db.ExecuteNonQuery(CommandType.Text, sql);
        }

        

    }
}
#endregion

