/*
 * ���ݷ��ʲ������ࡣ�����ṩ������ʹ�õĹ���������
 * 
 * V5 20090807
 * */

using System;
using System.Data;
using System.Globalization;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Common.Logging;
using System.Collections.Generic;

namespace TreeEditor.Core
{
    #region DalException
    /// <summary>
    /// DalException ��ʾ�����ݷ��ʲ㷢�ֵ��쳣������ͬ��DbException.
    /// ���е����ݿ�����쳣����DbException��DalException���Ծ�����δ���DbExcetpion����:�����쳣��������
    /// ��Ҫ�ٴ��׳�����DalException���װDbExcetpion,�����µ��쳣��ʽ�׳���
    /// DalExceptionӦ����Ӧ�ó���淶��ErrorCode��ϵ��
    /// </summary>
    [SerializableAttribute]
    public class DalException : Exception
    {
        private string exceptionMessage;

        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }

        public DalException()
        {
            exceptionMessage = "Data Access Exception.";
        }
        public DalException(string msg)
            : base(msg)
        {
            exceptionMessage = msg;
        }

        public DalException(string msg, Exception ex)
            : base(msg, ex)
        {
            exceptionMessage = msg;
        }


        protected DalException(SerializationInfo info, StreamingContext ctx)
            : base(info, ctx)
        {

        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue("exceptionMessage", exceptionMessage);
            base.GetObjectData(info, context);
        }


    }

    #endregion

    public abstract class DaBase<T> where T :class, ITvaNode,new()
    {

        protected static readonly ILog log = LogManager.GetCurrentClassLogger();
        protected Database db;

        public Database DB
        {
            get
            {
                return db;
            }
            set
            {
                db = value;
            }
        }

        #region ����������
        protected abstract T ConstructT();
        #endregion

        #region Addģ��

        #region Addģ�� Helper

        protected abstract DbCommand ConstructAddCommand(Database db, T entity);
        protected abstract DbCommand ConstructAddCommand(Database db);
        protected abstract DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, T entity);
        protected abstract DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, DataRow row);

        #endregion

        #region Addģ�� ͨ�÷���

        /// <summary>
        ///��ĳ����������һ����¼������û��commit
        /// </summary>
        public int Add(T entity, DbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException("entity", "Could not add null T.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            DbCommand dbCommand = ConstructAddCommand(db, entity);
            try
            {
                return db.ExecuteNonQuery(dbCommand, trans);
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return 0;

        }

        public int Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity Could not add null.");
            DbCommand dbCommand = ConstructAddCommand(db, entity);
            try
            {
                return db.ExecuteNonQuery(dbCommand);
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return 0;
        }

        public int Add(ReadOnlyCollection<T> entity, DbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException("entity", "ReadOnlyCollection<T> Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            int rc = 0;
            DbCommand dbCommand = ConstructAddCommand(db);
            int j = 0;
            try
            {
                for (j = 0; j < entity.Count; j++)
                {
                    PopulateAddCommandParameters(db, dbCommand, entity[j]);
                    rc += db.ExecuteNonQuery(dbCommand, trans);
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity[j]);
                rc = 0;
            }
            return rc;
        }


        public int Add(System.Collections.ObjectModel.ReadOnlyCollection<T> entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "ReadOnlyCollection<T> Could not be null.");

            int rc = 0;
            using (DbConnection cnn = db.CreateConnection())
            {
                try
                {
                    cnn.Open();
                    using (DbTransaction trans = cnn.BeginTransaction())
                    {
                        try
                        {
                            rc = Add(entity, trans);//This function throw customer exception.
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            if (trans != null) trans.Rollback();
                            HandleDbException(ex);
                        }
                    }
                    cnn.Close();
                }
                catch (DbException ex)
                {
                    HandleDbException(ex);
                    rc = 0;
                }
            }
            return rc;
        }

        public int Add(DataTable dt, DbTransaction trans)
        {
            if (dt == null) throw new ArgumentNullException("dt", "DataTable Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            int j = 0;
            int rc = 0;
            DbCommand dbCommand = ConstructAddCommand(db);
            try
            {
                for (j = 0; j < dt.Rows.Count; j++)
                {
                    PopulateAddCommandParameters(db, dbCommand, dt.Rows[j]);
                    rc += db.ExecuteNonQuery(dbCommand, trans);
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, dt.Rows[j]);
            }

            return rc;
        }


        public int Add(DataTable dt)
        {
            if (dt == null) throw new ArgumentNullException("dt", "DataTable Could not be null.");

            int rc = 0;

            using (DbConnection cnn = db.CreateConnection())
            {
                try
                {
                    cnn.Open();
                    using (DbTransaction trans = cnn.BeginTransaction())
                    {
                        try
                        {
                            rc = Add(dt, trans);//This function throw customer exception.
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            if (trans != null) trans.Rollback();
                            HandleDbException(ex);
                        }
                    }
                    cnn.Close();
                }
                catch (DbException ex)
                {
                    HandleDbException(ex);
                    rc = 0;
                }
            }
            return rc;
        }

        #endregion

        #endregion

        #region Updateģ��

        #region Updateģ�� Helper
        protected abstract DbCommand ConstructUpdateCommand(Database db);
        protected abstract DbCommand ConstructUpdateCommand(Database db, T entity);
        protected abstract DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, T entity);
        protected abstract DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, DataRow row);
        protected abstract void PrepareDataAdapterCommand(Database db, out DbCommand dbInsertCommand, out DbCommand dbUpdateCommand, out DbCommand dbDeleteCommand);
        #endregion

        #region Updateģ��  ͨ�÷���
        /// <summary>
        /// ������������ʵ�塣ע��:�����б���δ���¡�
        /// </summary>	
        public int Update(T entity, DbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            DbCommand dbCommand = ConstructUpdateCommand(db, entity);
            try
            {
                return db.ExecuteNonQuery(dbCommand, trans);
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return 0;
        }
        public int Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity) Could not be null.");
            DbCommand dbCommand = ConstructUpdateCommand(db, entity);
            try
            {
                return db.ExecuteNonQuery(dbCommand);
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return 0;
        }
        public int Update(ReadOnlyCollection<T> entity, DbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException("entity", "ReadOnlyCollection<T> Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            int rc = 0; int j = 0;
            DbCommand dbCommand = ConstructUpdateCommand(db);
            try
            {
                for (j = 0; j < entity.Count; j++)
                {
                    PopulateUpdateCommandParameters(db, dbCommand, entity[j]);
                    rc += db.ExecuteNonQuery(dbCommand, trans);
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity[j]);
                rc = 0;
            }
            return rc;
        }
        public int Update(ReadOnlyCollection<T> entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "ReadOnlyCollection<T> Could not be null.");

            int rc = 0;

            using (DbConnection cnn = db.CreateConnection())
            {
                try
                {
                    cnn.Open();
                    using (DbTransaction trans = cnn.BeginTransaction())
                    {
                        try
                        {
                            rc = Update(entity, trans);//This function throw customer exception.
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            if (trans != null) trans.Rollback();
                            HandleDbException(ex);
                        }
                    }
                    cnn.Close();
                }
                catch (DbException ex)
                {
                    HandleDbException(ex);
                    rc = 0;
                }
            }

            return rc;
        }
        public int Update(DataTable dt, DbTransaction trans)
        {
            if (dt == null) throw new ArgumentNullException("dt", "dt Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            int rc = 0; int j = 0;
            DbCommand dbCommand = ConstructUpdateCommand(db);
            try
            {
                for (j = 0; j < dt.Rows.Count; j++)
                {
                    PopulateUpdateCommandParameters(db, dbCommand, dt.Rows[j]);
                    rc += db.ExecuteNonQuery(dbCommand, trans);
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, dt.Rows[j]);
                rc = 0;
            }
            return rc;

        }
        public int Update(DataTable dt)
        {
            if (dt == null) throw new ArgumentNullException("dt", "dt Could not be null.");

            int rc = 0;

            using (DbConnection cnn = db.CreateConnection())
            {
                try
                {
                    cnn.Open();
                    using (DbTransaction trans = cnn.BeginTransaction())
                    {
                        try
                        {
                            rc = Update(dt, trans);//This function throw customer exception.
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            if (trans != null) trans.Rollback();
                            HandleDbException(ex);
                        }
                    }
                    cnn.Close();
                }
                catch (DbException ex)
                {
                    HandleDbException(ex);
                    rc = 0;
                }
            }
            return rc;
        }

        #endregion

        #region Updateģ�� By DataSet


        /// <summary>
        /// ͨ��DataSet�޸ı�����
        /// </summary>
        /// <param name="dst">DataSet ,contain the all data.</param>
        /// <param name="strTableName">The taget table .</param>
        /// <param name="trans">The DbTransaction,could not be null.</param>
        /// <returns>how many rows to be modifid.</returns>
        public int UpdateByDataSet(DataSet dst, string strTableName, DbTransaction trans)
        {
            if (dst == null) throw new ArgumentNullException("dst", "DataSet Could not be null.");
            if (String.IsNullOrEmpty(strTableName)) throw new ArgumentNullException("strTableName", "The taget table name should not be null or blank.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            DbCommand dbInsertCommand = null;
            DbCommand dbUpdateCommand = null;
            DbCommand dbDeleteCommand = null;
            PrepareDataAdapterCommand(db, out dbInsertCommand, out dbUpdateCommand, out dbDeleteCommand);
            try
            {
                return db.UpdateDataSet(dst, strTableName, dbInsertCommand, dbUpdateCommand, dbDeleteCommand, trans);
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return -1;//TODO:Should return Application Specific Error Code.
        }

        /// <summary>
        /// ͨ��DataSet�޸ı����ݣ���������
        /// </summary>
        public int UpdateByDataSet(DataSet dst, string strTableName)
        {
            if (dst == null) throw new ArgumentNullException("dst", "DataSet Could not be null.");
            if (String.IsNullOrEmpty(strTableName)) throw new ArgumentNullException("strTableName", "The taget table name should not be null or blank.");

            DbCommand dbInsertCommand = null;
            DbCommand dbUpdateCommand = null;
            DbCommand dbDeleteCommand = null;
            PrepareDataAdapterCommand(db, out dbInsertCommand, out dbUpdateCommand, out dbDeleteCommand);
            try
            {
                return db.UpdateDataSet(dst, strTableName, dbInsertCommand, dbUpdateCommand, dbDeleteCommand, UpdateBehavior.Transactional);
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return 0;//TODO:Should return Application Specific Error Code.			
        }

        #endregion

        #endregion

        #region Deleteģ��

        #region Deleteģ�� Helper
        protected abstract DbCommand ConstructDeleteCommand(Database db);
        protected abstract DbCommand ConstructDeleteCommand(Database db, T entity);
        protected abstract DbCommand ConstructDeleteCommandForWhere(Database db, string where);
        protected abstract DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, T entity);
        protected abstract DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, DataRow row);
        #endregion helper

        #region Deleteģ�� ͨ�÷���
        /// <summary>
        /// delete by pk(entity)
        /// </summary>	
        public int Delete(T entity, DbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            DbCommand dbCommand = ConstructDeleteCommand(db, entity);
            try
            {
                return db.ExecuteNonQuery(dbCommand, trans);
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return 0;
        }

        public int Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity Could not be null.");
            DbCommand dbCommand = ConstructDeleteCommand(db, entity);
            try
            {
                return db.ExecuteNonQuery(dbCommand);
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return 0;
        }
        /// <summary>
        /// mul-delete
        /// </summary>	
        public int Delete(ReadOnlyCollection<T> entities, DbTransaction trans)
        {
            if (entities == null) throw new ArgumentNullException("entities", "ReadOnlyCollection<T> Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            int rc = 0;
            int j = 0;
            DbCommand dbCommand = ConstructDeleteCommand(db);
            try
            {
                for (j = 0; j < entities.Count; j++)
                {
                    PopulateDeleteCommandParameters(db, dbCommand, entities[j]);
                    rc += db.ExecuteNonQuery(dbCommand, trans);
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entities[j]);
                rc = 0;
            }
            return rc;
        }

        public int Delete(ReadOnlyCollection<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities", "ReadOnlyCollection<T> Could not be null.");

            int rc = 0;

            using (DbConnection cnn = db.CreateConnection())
            {
                try
                {
                    cnn.Open();
                    using (DbTransaction trans = cnn.BeginTransaction())
                    {
                        try
                        {
                            rc = Delete(entities, trans);//This function throw customer exception
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            if (trans != null) trans.Rollback();
                            HandleDbException(ex);
                        }
                    }
                    cnn.Close();
                }
                catch (DbException ex)
                {
                    HandleDbException(ex);
                    rc = 0;
                }
            }

            return rc;
        }

        /// <summary>
        /// delete via DataTable
        /// </summary>	
        public int Delete(DataTable dt, DbTransaction trans)
        {
            if (dt == null) throw new ArgumentNullException("dt", "dt Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "trans can not be null.");

            int rc = 0;
            int j = 0;
            DbCommand dbCommand = ConstructDeleteCommand(db);
            try
            {
                for (j = 0; j < dt.Rows.Count; j++)
                {
                    PopulateDeleteCommandParameters(db, dbCommand, dt.Rows[j]);
                    rc += db.ExecuteNonQuery(dbCommand, trans);
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, dt.Rows[j]);
                rc = 0;
            }
            return rc;

        }
        public int Delete(DataTable dt)
        {
            if (dt == null) throw new ArgumentNullException("dt", "dt Could not be null.");
            int rc = 0;

            using (DbConnection cnn = db.CreateConnection())
            {
                try
                {
                    cnn.Open();
                    using (DbTransaction trans = cnn.BeginTransaction())
                    {
                        try
                        {
                            rc = Delete(dt, trans);//This function throw customer exception
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            if (trans != null) trans.Rollback();
                            HandleDbException(ex);
                        }
                    }
                    cnn.Close();
                }
                catch (DbException ex)
                {
                    HandleDbException(ex);
                    rc = 0;
                }
            }

            return rc;
        }

        public int DeleteByWhereClause(string where)
        {
            DbCommand dbCommand = ConstructDeleteCommandForWhere(db, where);
            try
            {
                return db.ExecuteNonQuery(dbCommand);
            }
            catch (DbException ex)
            {
                log.ErrorFormat("DeleteByWhereClause:Where ����={0}", where);
                HandleDbException(ex);
            }
            return -1;

        }

        public int DeleteByWhereClause(string where, DbTransaction trans)
        {
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            DbCommand dbCommand = ConstructDeleteCommandForWhere(db, where);

            try
            {
                return db.ExecuteNonQuery(dbCommand, trans);
            }
            catch (DbException ex)
            {
                log.ErrorFormat("DeleteByWhereClause:Where ����={0}", where);
                HandleDbException(ex);
            }
            return -1;

        }
        #endregion

        #endregion

        #region Queryģ��

        #region Queryģ�� Helper

        protected abstract DbCommand ConstructQueryCommand(string condition);

        #endregion

        #region Queryģ�� ͨ�÷���
        /// <summary>
        /// ��������WHERE�����ļ�¼�ļ���,��conditionΪ�գ����ѯ����
        /// </summary>		
        public DataSet Query(string condition)
        {
            DbCommand dbCommand = ConstructQueryCommand(condition);
            try
            {
                return db.ExecuteDataSet(dbCommand);
            }
            catch (DbException ex)
            {
                log.ErrorFormat("Query:condition ����={0}", condition);
                HandleDbException(ex);
            }
            return null;
        }


        public DataSet Query(string condition, DbTransaction trans)
        {
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            DbCommand dbCommand = ConstructQueryCommand(condition);
            try
            {
                return db.ExecuteDataSet(dbCommand, trans);
            }
            catch (DbException ex)
            {
                log.ErrorFormat("Query:condition ����={0}", condition);
                HandleDbException(ex);
            }
            return null;
        }

        #endregion

        #endregion

        #region GetEntity(s)ģ��

        #region GetEntity(s)ģ�� Helper
        protected abstract DbCommand ConstructSelectOneCommand(Database db, T entity);

        protected abstract void PopulateEntityByDataReader(IDataReader reader, ref T entity);

        protected abstract DbCommand ConstructSelectConditionCommand(Database db, string condition);
        #endregion

        #region GetEntity(s)ģ�� ͨ�÷���

        /// <summary>
        ///ʵ��������Ѿ���ֵ,����ԭ����ʹ��ref��������ơ�    
        /// </summary>        
        public bool GetEntityEx(ref T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity can not be null.");
            DbCommand dbCommand = ConstructSelectOneCommand(db, entity);
            try
            {
                using (IDataReader reader = db.ExecuteReader(dbCommand))
                {
                    if (reader.Read())
                    {
                        PopulateEntityByDataReader(reader, ref entity);
                        return true;
                    }
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return false;
        }

        public bool GetEntityEx(ref T entity, DbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity Could not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            DbCommand dbCommand = ConstructSelectOneCommand(db, entity);
            try
            {
                using (IDataReader reader = db.ExecuteReader(dbCommand, trans))
                {
                    if (reader.Read())
                    {
                        PopulateEntityByDataReader(reader, ref entity);
                        return true;
                    }
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex, entity);
            }
            return false;

        }

        public Collection<T> GetEntities(string condition, DbTransaction trans)
        {
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            Collection<T> list = new Collection<T>();
            DbCommand dbCommand = ConstructSelectConditionCommand(db, condition);
            try
            {
                using (IDataReader reader = db.ExecuteReader(dbCommand, trans))
                {
                    while (reader.Read())
                    {
                        T entity = ConstructT();
                        PopulateEntityByDataReader(reader, ref entity);
                        list.Add(entity);
                    }
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return list;

        }


        public Collection<T> GetEntities(string condition)
        {
            Collection<T> list = new Collection<T>();
            DbCommand dbCommand = ConstructSelectConditionCommand(db, condition);
            try
            {
                using (IDataReader reader = db.ExecuteReader(dbCommand))
                {
                    while (reader.Read())
                    {
                        T entity = ConstructT();
                        PopulateEntityByDataReader(reader, ref entity);
                        list.Add(entity);
                    }
                }
            }
            catch (DbException ex)
            {
                log.ErrorFormat("GetEntities:condition ����={0}", condition);
                HandleDbException(ex);
            }
            return list;
        }

        public ICollection<T> GetEntities(string condition, DbTransaction trans, ICollection<T> list)
        {
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            DbCommand dbCommand = ConstructSelectConditionCommand(db, condition);
            try
            {
                using (IDataReader reader = db.ExecuteReader(dbCommand, trans))
                {
                    while (reader.Read())
                    {
                        T entity = ConstructT();
                        PopulateEntityByDataReader(reader, ref entity);
                        list.Add(entity);
                    }
                }
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return list;

        }

        public ICollection<T> GetEntitiesEx(string condition, ICollection<T> list)
        {
            list.Clear();
            DbCommand dbCommand = ConstructSelectConditionCommand(db, condition);
            try
            {
                using (IDataReader reader = db.ExecuteReader(dbCommand))
                {
                    while (reader.Read())
                    {
                        T entity = ConstructT();
                        PopulateEntityByDataReader(reader, ref entity);
                        list.Add(entity);
                    }
                }
            }
            catch (DbException ex)
            {
                log.ErrorFormat("GetEntities:condition ����={0}", condition);
                HandleDbException(ex);
            }
            return list;
        }


        #endregion

        #endregion

        #region Countģ��

        #region Countģ�� Hepler
        protected abstract DbCommand ConstructCountCommand(Database db);

        protected abstract DbCommand ConstructCountConditionCommand(Database db, string condition);
        #endregion

        #region Countģ�� ͨ�÷���
        /// <summary>
        /// ���ر�ļ�¼����
        /// </summary>
        public int GetCount()
        {
            DbCommand dbCommand = ConstructCountCommand(db);
            try
            {
                return Convert.ToInt32(db.ExecuteScalar(dbCommand), CultureInfo.InvariantCulture);
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return -1;
        }

        public int GetCount(DbTransaction trans)
        {
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            DbCommand dbCommand = ConstructCountCommand(db);
            try
            {
                return Convert.ToInt32(db.ExecuteScalar(dbCommand, trans), CultureInfo.InvariantCulture);
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return -1;
        }

        /// <summary>
        /// ��������WHERE������¼��
        /// </summary>
        public int GetCount(string where)
        {
            DbCommand dbCommand = ConstructCountConditionCommand(db, where);
            try
            {
                return Convert.ToInt32(db.ExecuteScalar(dbCommand), CultureInfo.InvariantCulture);
            }
            catch (DbException ex)
            {
                log.ErrorFormat("GetCount:where ����={0}", where);
                HandleDbException(ex);
            }
            return -1;
        }

        public int GetCount(string where, DbTransaction trans)
        {
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");
            DbCommand dbCommand = ConstructCountConditionCommand(db, where);
            try
            {
                return Convert.ToInt32(db.ExecuteScalar(dbCommand, trans), CultureInfo.InvariantCulture);
            }
            catch (DbException ex)
            {
                log.ErrorFormat("GetCount:where ����={0}", where);
                HandleDbException(ex);
            }
            return -1;
        }
        #endregion

        #endregion

        #region IsExistģ��

        #region IsExistģ�� Helper

        protected abstract DbCommand ConstructIsExistCommand(Database db, T entity);
        #endregion

        #region IsExistģ�� ͨ�÷���


        public bool IsExist(T entity, DbTransaction trans)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity can not be null.");
            if (trans == null) throw new ArgumentNullException("trans", "DbTransaction can not be null.");

            DbCommand dbCommand = ConstructIsExistCommand(db, entity);
            try
            {
                return Convert.ToInt32(db.ExecuteScalar(dbCommand, trans), CultureInfo.InvariantCulture) > 0;
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return false;
        }

        public bool IsExist(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "entity can not be null.");
            DbCommand dbCommand = ConstructIsExistCommand(db, entity);
            try
            {
                return Convert.ToInt32(db.ExecuteScalar(dbCommand), CultureInfo.InvariantCulture) > 0;
            }
            catch (DbException ex)
            {
                HandleDbException(ex);
            }
            return false;
        }

        #endregion

        #endregion

        #region Handle Exception
        internal static void HandleDbException(Exception ex)
        {
            //@if MS SQL Server
            log.Error(ex);
            throw new DalException("Data Access Exception.", ex);
        }
        internal static void HandleDbException(Exception ex, object entity)
        {
            //NOTE:If you use SQL Server/Oracle/Db2... ,Please add the [internal static class ErrorCodes]
            //Use the ErrorCodes like below:
            //if (ex.Number == ErrorCodes.SqlUserRaisedError)
            //{
            //    switch (ex.State)
            //    {
            //    case ErrorCodes.ValidationError:
            //        string[] messageParts = ex.Errors[0].Message.Split(':');
            //        throw new RepositoryValidationException(
            //            mapper.MapDbParameterToBusinessEntityProperty(messageParts[0]),
            //            messageParts[1], ex);

            //    case ErrorCodes.ConcurrencyViolationError:
            //        throw new ConcurrencyViolationException(ex.Message, ex);

            //    }
            //}

            log.Error(entity, ex);
            throw new DalException("Data Access Exception.The Entity content is:" + entity.ToString(), ex);
        }

        #endregion

    
        /// <summary>
        /// �����������ĸ���.��������ӣ�Ҳ�����Ǹ��¡�
        /// </summary>
        /// <param name="list"></param>
        /// <param name="force">���force=true,��ǿ�Ʊ��棬�����Ŀ���Ȼ��ȫ��ִ����ӡ��˶���ɾ�������оɵ����ݣ������أ�</param>
        /// <returns></returns>
        public int Save(IList<T> list, bool force)
        {
            int rc = 0;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    if (force)
                    {
                        DeleteByWhereClause(null, trans);
                        if (log.IsWarnEnabled) log.WarnFormat("ɾ��������{0}ʵ������.", typeof(T));
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        T mo = list[i];
                        if ((!force) && IsExist(mo, trans))
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
