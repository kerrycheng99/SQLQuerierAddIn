using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

namespace SQLQuerierAddIn.Common
{
    public class SqlBaseInfo
    {
        #region GetSqlServerNames : 获取局域网内的所有数据库服务器名称(NO USE)
        /// <summary>  
        /// 获取局域网内的所有数据库服务器名称  
        /// <para>No Use太慢未使用</para>
        /// </summary>  
        /// <returns>服务器名称数组</returns>  
        public static List<string> GetSqlServerNames()
        {
            DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();

            DataColumn colInstanceName = dataSources.Columns["InstanceName"];
            DataColumn colServerName = dataSources.Columns["ServerName"];

            DataRowCollection rows = dataSources.Rows;
            List<string> Serverlist = new List<string>();
            string array = string.Empty;
            for (int i = 0; i < rows.Count; i++)
            {
                string strServerName = rows[i][colServerName] as string;
                string strInstanceName = rows[i][colInstanceName] as string;
                if (((strInstanceName == null) || (strInstanceName.Length == 0)) || ("MSSQLSERVER" == strInstanceName))
                {
                    array = strServerName;
                }
                else
                {
                    array = strServerName + @"\" + strInstanceName;
                }

                Serverlist.Add(array);
            }

            Serverlist.Sort();

            return Serverlist;
        }
        #endregion

        #region GetDatabaseList ： 获取SQLServer中的非系统库列表
        /// <summary>  
        /// 获取SQLServer中的非系统库列表  
        /// </summary>  
        /// <param name="connection"></param>  
        /// <returns></returns>  
        public static List<string> GetDatabaseList(string connection)
        {
            List<string> getCataList = new List<string>();
            string cmdStirng = "select name from sys.databases where database_id > 4 order by name";
            SqlConnection connect = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand(cmdStirng, connect);
            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                    IDataReader dr = cmd.ExecuteReader();
                    getCataList.Clear();
                    while (dr.Read())
                    {
                        getCataList.Add(dr["name"].ToString());
                    }
                    dr.Close();
                }

            }
            catch (SqlException e)
            {
                //MessageBox.Show(e.Message);  
            }
            finally
            {
                if (connect != null && connect.State == ConnectionState.Open)
                {
                    connect.Dispose();
                }
            }
            return getCataList;
        }
        #endregion

        #region GetConnectedDBInfo ： 获取当前已连接数据库的基本信息
        /// <summary>
        /// 获取当前已连接数据库的基本信息
        /// </summary>
        /// <param name="db"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetConnectedDBInfo(SqlComDB db)
        {
            DataTable dt = new DataTable();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT ");
            strSQL.Append("SERVERPROPERTY('SERVERNAME') AS SERVER_NAME ");
            strSQL.Append(" ,CAST(SERVERPROPERTY('productversion') AS VARCHAR)+' '+ CAST(SERVERPROPERTY ('productlevel') AS VARCHAR) +' '+ CAST(SERVERPROPERTY ('edition') AS VARCHAR) AS DB_VER ");
            strSQL.Append(" ,SYSTEM_USER AS SYS_USER ");
            strSQL.Append(" ,CAST(@@SPID AS VARCHAR) CURRENT_PID ");
            strSQL.Append(" ,DB_NAME() AS  CURRENT_DB_NAME ");
            strSQL.Append(" ,DB_ID() AS CURRENT_DB_ID ");

            try
            {
                dt = db.DbDataTable(strSQL.ToString(), "Table");
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetTables : 获取SQLServer中的数据表名（NO USE）
        /// <summary>  
        /// 获取SQLServer中的数据表名（NO USE） 
        /// </summary>  
        /// <param name="connection"></param>  
        /// <returns></returns>  
        public static List<string> GetTables(string connection)
        {
            List<string> tablelist = new List<string>();
            SqlConnection objConnetion = new SqlConnection(connection);
            try
            {
                if (objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Open();
                    DataTable objTable = objConnetion.GetSchema("Tables");
                    foreach (DataRow row in objTable.Rows)
                    {
                        tablelist.Add(row[2].ToString());
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (objConnetion != null && objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Dispose();
                }

            }
            return tablelist;
        }
        #endregion

        #region GetAllTables : 获取SQLServer中的数据表名（含表、视图、同义词、表值函数）
        /// <summary>
        /// 获取SQLServer中的数据表名（含表、视图、同义词、表值函数） 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetAllTables(string connection)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT xtype, name ");
            strSQL.Append(" FROM sys.sysobjects ");
            strSQL.Append(" WHERE xtype IN ('U','V','SN','IF') ");
            strSQL.Append(" ORDER BY xtype, name ");

            DataTable tablelist = new DataTable();
            SqlConnection objConnetion = new SqlConnection(connection);
            try
            {
                if (objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Open();
                    SqlCommand cmd = new SqlCommand(strSQL.ToString(), objConnetion);
                    SqlDataReader objReader = cmd.ExecuteReader();

                    while (objReader.Read())
                    {
                        tablelist.Load(objReader);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (objConnetion != null && objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Dispose();
                }

            }
            return tablelist;
        }
        #endregion

        #region GetColumnField : 获取指定数据表名的字段列表（NO USE）
        /// <summary>  
        /// 获取指定数据表名的字段列表（NO USE）  
        /// </summary>  
        /// <param name="connection"></param>  
        /// <param name="TableName"></param>  
        /// <returns></returns>  
        public static List<string> GetColumnField(string connection, string TableName)
        {
            List<string> Columnlist = new List<string>();
            SqlConnection objConnetion = new SqlConnection(connection);
            try
            {
                if (objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Open();
                }

                SqlCommand cmd = new SqlCommand("Select Name FROM SysColumns Where id=Object_Id('" + TableName + "')", objConnetion);
                SqlDataReader objReader = cmd.ExecuteReader();

                while (objReader.Read())
                {
                    Columnlist.Add(objReader[0].ToString());

                }
            }
            catch
            {

            }
            objConnetion.Close();
            return Columnlist;
        }
        #endregion

        #region GetAllColumnField : 获取SQLServer中所有数据表的字段名列表（含表、视图、同义词）
        /// <summary>
        /// 获取SQLServer中所有数据表的字段名列表（含表、视图、同义词）
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>List</returns>
        public static List<string> GetAllColumnsList(string connection)
        {
            StringBuilder strSQL = new StringBuilder();
            //strSQL.Append(" SELECT o.name+'.'+c.name name ");
            strSQL.Append(" SELECT DISTINCT c.name ");
            strSQL.Append(" FROM sys.syscolumns c ");
            strSQL.Append(" INNER JOIN sys.sysobjects o ON o.id = c.id ");
            strSQL.Append(" WHERE o.xtype IN('U','V','SN') ");
            strSQL.Append(" ORDER BY c.name ");

            List<string> columnlist = new List<string>();
            SqlConnection objConnetion = new SqlConnection(connection);
            try
            {
                if (objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Open();
                    SqlCommand cmd = new SqlCommand(strSQL.ToString(), objConnetion);
                    SqlDataReader objReader = cmd.ExecuteReader();

                    while (objReader.Read())
                    {
                        columnlist.Add(objReader[0].ToString());

                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (objConnetion != null && objConnetion.State == ConnectionState.Closed)
                {
                    objConnetion.Dispose();
                }

            }
            return columnlist;
        }
        #endregion

    }
}
