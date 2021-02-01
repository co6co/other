using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using System.Configuration;

namespace H31SQLLibrary
{
    /// <summary>
    /// ���ݿ��������(for Sql2000/2005)
    /// </summary>
    internal class H31SQL2000: H31DBPage, IH31DBSQL
    {
        /// <summary>
        /// ��ҳ��ȡ����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="tblName">����</param>
        /// <param name="fldName">�ֶ���</param>
        /// <param name="pageSize">ҳ��С</param>
        /// <param name="pageIndex">�ڼ�ҳ</param>
        /// <param name="fldSort">�����ֶ�</param>
        /// <param name="sort">����{False}/����(True)</param>
        /// <param name="strCondition">����(����Ҫwhere)</param>
        public DataSet PageList(string connectionString, string tblName, int pageSize,
            int pageIndex, string fldSort, bool sort, string condition)
        {
            string sql = GetPagerSQL(condition, pageSize, pageIndex, fldSort, tblName, sort);
            return ExecuteReader(connectionString, CommandType.Text, sql, null);
        }

        /// <summary>
        /// ִ�в�ѯ������DataSet
        /// </summary>
        public DataSet ExecuteQuery(string connectionString, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// ��������ִ�в�ѯ������DataSet
        /// </summary>
        public DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "ds");
            cmd.Parameters.Clear();
            return ds;
        }

        /// <summary>
        /// ִ�� Transact-SQL ��䲢������Ӱ���������
        /// </summary>
        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// ��������ִ�� Transact-SQL ��䲢������Ӱ���������
        /// </summary>
        public int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// ִ�в�ѯ������DataReader
        /// </summary>
        public DataSet ExecuteReader(string connectionString, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                //SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "ds");
                cmd.Parameters.Clear();
                return ds;

            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// ��������ִ�в�ѯ������DataReader
        /// </summary>
        public DataSet ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            //SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "ds");
            cmd.Parameters.Clear();
            return ds;
        }

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����������л��С�
        /// </summary>
        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// ��������ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����������л��С�
        /// </summary>
        public object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText,
            params DbParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// ����Ҫִ�е�����
        /// </summary>
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType,
            string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
