/*
 * 数据访问层抽象基类。这里提供所有类使用的公共方法。
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
    /// DalException 表示在数据访问层发现的异常，它不同于DbException.
    /// 所有的数据库相关异常，由DbException表达。DalException可以决定如何处理DbExcetpion，如:忽略异常，继续，
    /// 若要再次抛出，则DalException会包装DbExcetpion,并以新的异常形式抛出。
    /// DalException应具有应用程序规范的ErrorCode体系。
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

        #region 构造器方法
        protected abstract T ConstructT();
        #endregion

        #region Add模块

        #region Add模块 Helper

        protected abstract DbCommand ConstructAddCommand(Database db, T entity);
        protected abstract DbCommand ConstructAddCommand(Database db);
        protected abstract DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, T entity);
        protected abstract DbCommand PopulateAddCommandParameters(Database db, DbCommand addCmd, DataRow row);

        #endregion

        #region Add模块 通用方法

        /// <summary>
        ///在某事务下增加一条记录，事务没有commit
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

        #region Update模块

        #region Update模块 Helper
        protected abstract DbCommand ConstructUpdateCommand(Database db);
        protected abstract DbCommand ConstructUpdateCommand(Database db, T entity);
        protected abstract DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, T entity);
        protected abstract DbCommand PopulateUpdateCommandParameters(Database db, DbCommand updateCmd, DataRow row);
        protected abstract void PrepareDataAdapterCommand(Database db, out DbCommand dbInsertCommand, out DbCommand dbUpdateCommand, out DbCommand dbDeleteCommand);
        #endregion

        #region Update模块  通用方法
        /// <summary>
        /// 根据主键更新实体。注意:主键列本身并未更新。
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

        #region Update模块 By DataSet


        /// <summary>
        /// 通过DataSet修改表数据
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
        /// 通过DataSet修改表数据（不带事务）
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

        #region Delete模块

        #region Delete模块 Helper
        protected abstract DbCommand ConstructDeleteCommand(Database db);
        protected abstract DbCommand ConstructDeleteCommand(Database db, T entity);
        protected abstract DbCommand ConstructDeleteCommandForWhere(Database db, string where);
        protected abstract DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, T entity);
        protected abstract DbCommand PopulateDeleteCommandParameters(Database db, DbCommand deleteCmd, DataRow row);
        #endregion helper

        #region Delete模块 通用方法
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
                log.ErrorFormat("DeleteByWhereClause:Where 参数={0}", where);
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
                log.ErrorFormat("DeleteByWhereClause:Where 参数={0}", where);
                HandleDbException(ex);
            }
            return -1;

        }
        #endregion

        #endregion

        #region Query模块

        #region Query模块 Helper

        protected abstract DbCommand ConstructQueryCommand(string condition);

        #endregion

        #region Query模块 通用方法
        /// <summary>
        /// 返回满足WHERE条件的记录的集合,若condition为空，则查询所有
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
                log.ErrorFormat("Query:condition 参数={0}", condition);
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
                log.ErrorFormat("Query:condition 参数={0}", condition);
                HandleDbException(ex);
            }
            return null;
        }

        #endregion

        #endregion

        #region GetEntity(s)模块

        #region GetEntity(s)模块 Helper
        protected abstract DbCommand ConstructSelectOneCommand(Database db, T entity);

        protected abstract void PopulateEntityByDataReader(IDataReader reader, ref T entity);

        protected abstract DbCommand ConstructSelectConditionCommand(Database db, string condition);
        #endregion

        #region GetEntity(s)模块 通用方法

        /// <summary>
        ///实体的主键已经赋值,返回原对象，使用ref避免对象复制。    
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
                log.ErrorFormat("GetEntities:condition 参数={0}", condition);
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
                log.ErrorFormat("GetEntities:condition 参数={0}", condition);
                HandleDbException(ex);
            }
            return list;
        }


        #endregion

        #endregion

        #region Count模块

        #region Count模块 Hepler
        protected abstract DbCommand ConstructCountCommand(Database db);

        protected abstract DbCommand ConstructCountConditionCommand(Database db, string condition);
        #endregion

        #region Count模块 通用方法
        /// <summary>
        /// 返回表的记录总数
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
        /// 返回满足WHERE条件记录数
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
                log.ErrorFormat("GetCount:where 参数={0}", where);
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
                log.ErrorFormat("GetCount:where 参数={0}", where);
                HandleDbException(ex);
            }
            return -1;
        }
        #endregion

        #endregion

        #region IsExist模块

        #region IsExist模块 Helper

        protected abstract DbCommand ConstructIsExistCommand(Database db, T entity);
        #endregion

        #region IsExist模块 通用方法


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
        /// 保存给定对象的更改.可能是添加，也可能是更新。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="force">如果force=true,则强制保存，先清空目标表，然后全部执行添加。此动作删除了所有旧的数据，请慎重！</param>
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
                        if (log.IsWarnEnabled) log.WarnFormat("删除了所有{0}实体数据.", typeof(T));
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
