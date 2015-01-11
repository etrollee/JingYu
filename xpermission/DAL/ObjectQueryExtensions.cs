using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public static class ObjectQueryExtensions
    {
        /// <summary>
        /// 取得实体框架的连接字符串
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static string GetEntityConnectStringAsSqlConn(ObjectContext Context)
        {
            return (Context.Connection as EntityConnection).StoreConnection.ConnectionString;
        }
        /// <summary>
        /// 执行存储过程,返回DataSet
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="scmd"></param>
        /// <returns></returns>
        public static DataSet ExceuteEntityProcReturnDataset(this ObjectContext Context, SqlCommand scmd)
        {
            DataSet ReDataSet = new DataSet();
            using (SqlConnection sconn = new SqlConnection(GetEntityConnectStringAsSqlConn(Context)))
            {
                bool flag = false;


                scmd.Connection = sconn;
                if ((scmd.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    sconn.Open();
                    flag = true;
                }
                SqlDataAdapter myAdapter = new SqlDataAdapter(scmd);
                if (flag)
                    myAdapter.Fill(ReDataSet);
                else
                    return null;

                sconn.Close();
                return ReDataSet;
            }
        }
        /// <summary>
        /// 执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this ObjectContext context, SqlCommand cmd)
        {
            using (SqlConnection sconn = new SqlConnection(GetEntityConnectStringAsSqlConn(context)))
            {
                cmd.Connection = sconn;
                if ((cmd.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    sconn.Open();
                }
                return cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 执行SQL语句,返回第一行第一列的单元
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static object ExecuteEntityScalar(this ObjectContext context, SqlCommand cmd)
        {
            using (SqlConnection sconn = new SqlConnection(GetEntityConnectStringAsSqlConn(context)))
            {
                cmd.Connection = sconn;
                if ((cmd.Connection.State & ConnectionState.Open) != ConnectionState.Open)
                {
                    sconn.Open();
                }
                return cmd.ExecuteScalar();
            }
        }
        public static int ExecuteNonQuery(this ObjectContext context, string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            SqlConnection conn = (context.Connection as EntityConnection).StoreConnection as SqlConnection;
            if (conn == null) return -1;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            bool needClose = false;
            switch (cmd.Connection.State)
            {
                case ConnectionState.Closed:
                    {
                        cmd.Connection.Open();
                        break;
                    }
                case ConnectionState.Open:
                    {
                        break;
                    }
                case ConnectionState.Broken:
                    {
                        cmd.Connection.Close();
                        cmd.Connection.Open();
                        needClose = true;
                        break;
                    }
                //为未来版本保留的值
                case ConnectionState.Connecting:
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                    {
                        break;
                    }
            }
            int ret = cmd.ExecuteNonQuery();
            if (needClose) cmd.Connection.Close();
            return ret;
        }
    }
}
