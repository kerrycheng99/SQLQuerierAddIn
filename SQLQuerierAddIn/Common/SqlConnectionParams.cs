using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLQuerierAddIn.Common
{
    /// <summary>
    /// 数据库连接字符串参数
    /// </summary>
    public class SqlConnectionParams
    {

        #region Public Properties

        public SqlAuthentication AuthenticationMethod { get; set; }
        public string Catalog { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get { return this._connectionString; }
            set
            {
                value = (AuthenticationMethod == SqlAuthentication.Windows) ?
                    "Data Source=" + Server + ";Initial Catalog=" + Catalog + ";Integrated Security=SSPI;" :
                    "Data Source=" + Server + ";Initial Catalog=" + Catalog + ";User Id=" + User + ";Password=" + Password + ";";
                _connectionString = value;
            }
        }

        #endregion

        #region SQL Server Authentication Enums
        /// <summary>
        /// SQL Server Authentication Enums
        /// <para>0.Windows</para>
        /// <para>1.SQL Server</para>
        /// </summary>
        public enum SqlAuthentication
        {
            Windows = 0,
            SqlServer = 1
        }

        #endregion
    }
}
