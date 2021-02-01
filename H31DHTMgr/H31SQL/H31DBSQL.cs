using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
//using MySql.Data.MySqlClient;
//using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace H31SQLLibrary
{
    /// <summary>
    /// ���ݿ�
    /// </summary>
    public class H31DBSQL
    {
        /// <summary>
        /// ö�٣����ݿ�����
        /// </summary>
        public enum DatabaseTypes
        {
            Sql, MySql, Oracle, Access
        }

        private DatabaseTypes databaseType;
        private string connectionString;

        public H31DBSQL()
        { }

        public H31DBSQL(DatabaseTypes type, string connectionString)
        {
            this.databaseType = type;
            this.connectionString = connectionString;
        }

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DatabaseTypes DatabaseType
        {
            get { return databaseType; }
            set { databaseType = value; }
        }

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private IH31DBSQL h31DBSql
        {
            get
            {
                switch (databaseType)
                {
                    case DatabaseTypes.Access:
                        return new H31Access();
                    //case DatabaseTypes.MySql:
                    //    return new MySqlHelper();
                    //case DatabaseTypes.Oracle:
                    //    return new MySqlHelper();
                    case DatabaseTypes.Sql:
                    default:
                        return new H31SQL2000();
                }
            }
        }

        public DbConnection CreateConnection()
        {
            switch (databaseType)
            {
                //case DatabaseTypes.MySql:
                //    return new MySqlConnection(connectionString);
                //case DatabaseTypes.Oracle:
                //    return new OracleConnection(connectionString);
                case DatabaseTypes.Access:
                    return new OleDbConnection(connectionString);
                case DatabaseTypes.Sql:
                default:
                    return new SqlConnection(connectionString);
            }
        }

        #region === ����DbParameter��ʵ�� ===

        /// <summary>
        /// ��������DbParameter��ʵ��
        /// </summary>
        public DbParameter CreateInDbParameter(string paraName, DbType dbType, int size, object value)
        {
            return CreateDbParameter(paraName, dbType, size, value, ParameterDirection.Input);
        }

        /// <summary>
        /// ��������DbParameter��ʵ��
        /// </summary>
        public DbParameter CreateInDbParameter(string paraName, DbType dbType, object value)
        {
            return CreateDbParameter(paraName, dbType, 0, value, ParameterDirection.Input);
        }

        /// <summary>
        /// �������DbParameter��ʵ��
        /// </summary>        
        public DbParameter CreateOutDbParameter(string paraName, DbType dbType, int size, object value)
        {
            return CreateDbParameter(paraName, dbType, size, value, ParameterDirection.Output);
        }

        /// <summary>
        /// �������DbParameter��ʵ��
        /// </summary>        
        public DbParameter CreateOutDbParameter(string paraName, DbType dbType, object value)
        {
            return CreateDbParameter(paraName, dbType, 0, value, ParameterDirection.Output);
        }

        /// <summary>
        /// ����DbParameter��ʵ��
        /// </summary>
        public DbParameter CreateDbParameter(string paraName, DbType dbType, int size, object value, ParameterDirection direction)
        {
            DbParameter para;
            switch (databaseType)
            {
                //case DatabaseTypes.MySql:
                //    para = new MySqlParameter();
                //    break;
                //case DatabaseTypes.Oracle:
                //    para = new OracleParameter();
                //    break;
                case DatabaseTypes.Access:
                    para = new OleDbParameter();
                    break;
                case DatabaseTypes.Sql:
                default:
                    para = new SqlParameter();
                    break;
            }
            para.ParameterName = paraName;
            if (size != 0)
                para.Size = size;
            para.DbType = dbType;
            para.Value = value;
            para.Direction = direction;

            return para;
        }

        #endregion

        #region === ���ݿ�ִ�з��� ===

        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteNonQuery(connectionString, cmdType, cmdText, cmdParms);
        }

        public int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteNonQuery(trans, cmdType, cmdText, cmdParms);
        }

        public DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteQuery(trans, cmdType, cmdText, cmdParms);
        }

        public DataSet ExecuteQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteQuery(connectionString, cmdType, cmdText, cmdParms);
        }

        public DataSet ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteReader(trans, cmdType, cmdText, cmdParms);
        }

        public DataSet ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteReader(connectionString, cmdType, cmdText, cmdParms);
        }

        public object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteScalar(trans, cmdType, cmdText, cmdParms);
        }

        public object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            return h31DBSql.ExecuteScalar(connectionString, cmdType, cmdText, cmdParms);
        }

        public DataSet PageList(string tblName, int pageSize, int pageIndex, string fldSort, bool sort, string condition)
        {
            return h31DBSql.PageList(connectionString, tblName, pageSize, pageIndex, fldSort, sort, condition);
        }

        #endregion

        #region === ��Objectȡֵ ===

        /// <summary>
        /// ȡ��Intֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetInt(object obj)
        {
            if (obj.ToString() != "")
                return int.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// ȡ��byteֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte Getbyte(object obj)
        {
            if (obj.ToString() != "")
                return byte.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// ���Longֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long GetLong(object obj)
        {
            if (obj.ToString() != "")
                return long.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// ȡ��Decimalֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public decimal GetDecimal(object obj)
        {
            if (obj.ToString() != "")
                return decimal.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// ȡ��Guidֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Guid GetGuid(object obj)
        {
            if (obj.ToString() != "")
                return new Guid(obj.ToString());
            else
                return Guid.Empty;
        }

        /// <summary>
        /// ȡ��DateTimeֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DateTime GetDateTime(object obj)
        {
            if (obj.ToString() != "" && obj.ToString() != "0000-0-0 0:00:00")
                return DateTime.Parse(obj.ToString());
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// ȡ��boolֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool GetBool(object obj)
        {
            if (obj.ToString() == "1" || obj.ToString().ToLower() == "true")
                return true;
            else
                return false;
        }

        /// <summary>
        /// ȡ��byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Byte[] GetByte(object obj)
        {
            if (obj.ToString() != "")
            {
                return (Byte[])obj;
            }
            else
                return null;
        }

        /// <summary>
        /// ȡ��stringֵ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetString(object obj)
        {
            return obj.ToString();
        }

        #endregion
    }
}
