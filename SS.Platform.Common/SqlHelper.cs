using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SS.Platform.Common
{
    /// <summary>
    /// 用来操作数据库的帮助类
    /// </summary>
    public static class SqlHelper
    {
        //连接字符串
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        //封装常用方法

        #region ExcuteNonquery 返回结果为影响行数
        /// <summary>
        ///执行insert/delete/update的方法
        ///如果不需要返回查询结果则使用ExecuteNonQuery方法，一般用来执行Update、Delete、Insert语句。
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <param name="cmdType">查询语句类型</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //设置当前执行的是存储过程还是带参数的Sql语句
                    cmd.CommandType = cmdType;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region ExcuteScalar 返回第一行第一列的数据，其他行忽略，如统计数据条数等（count）
        /// <summary>
        /// 执行返回单个值的方法
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <param name="cmdType">查询语句类型</param>
        /// <param name="parameters">查询参数列表</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //设置当前执行的是存储过程还是带参数的Sql语句
                    cmd.CommandType = cmdType;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region ExecuteReader 返回一个SqlDataReader对象
        /// <summary>
        /// 3.返回SqlDataReader的方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>包含查询结果的SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connStr);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                try
                {
                    conn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
        }
        #endregion

        #region ExecuteDataTable 返回一个DataTable对象查询结果
        //4.返回DataTable
        /// <summary>
        /// 返回DataTable的查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                adapter.SelectCommand.CommandType = cmdType;
                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region 插入(Insert),返回值为影响行数
        /// <summary>
        /// 完成给数据库插入操作
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static int Insert(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(sql, cmdType, parameters);
        }
        #endregion

        #region 修改(Update),返回值为影响行数
        /// <summary>
        /// 完成给数据库更新操作
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static int Update(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(sql, cmdType, parameters);
        }
        #endregion

        #region 删除(Delete),返回值为影响行数
        /// <summary>
        /// 完成给数据库删除操作
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static int Delete(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(sql, cmdType, parameters);
        }
        #endregion

        #region 查询(select),返回值为DataTable
        /// <summary>
        /// 返回DataTable的查询操作
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static DataTable SelectTable(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            return ExecuteDataTable(sql, cmdType, parameters);
        }
        #endregion

        #region 查询(select),返回值为DataReader
        /// <summary>
        /// 返回DataReader的查询操作
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="cmdType">查询类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public static SqlDataReader SelectReader(string sql, CommandType cmdType, params SqlParameter[] parameters)
        {
            return ExecuteReader(sql, cmdType, parameters);
        }
        #endregion

        #region 读取数据库null值
        /// <summary>
        /// 把数据库DBnull转换为C#NULL
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object FromDbValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else
            {
                return value;
            }
        }
        #endregion

        #region 写入数据库null值
        /// <summary>
        /// 把数据库C#NULL转换为DBnull
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }
        #endregion
    }
}
