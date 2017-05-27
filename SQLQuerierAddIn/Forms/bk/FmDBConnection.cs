using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SQLQuerierAddIn.Common;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace SQLQuerierAddIn.Forms
{
    public partial class FmDBConnection : Form
    {
        public FmDBConnection()
        {
            InitializeComponent();
        }

        private SqlConnectionParams.SqlAuthentication _SqlAuthMethod;
        private string _msg_title = "Error";
        private string _msg_val = "";

        private string _SERVER_NAME = "";
        private string _DB_VER = "";
        private string _SYS_USER = "";
        private string _CURRENT_PID = "";
        private string _CURRENT_DB_NAME = "";

        public string SERVER_NAME { get { return _SERVER_NAME; } }
        public string DB_VER { get { return _DB_VER; } }
        public string SYS_USER { get { return _SYS_USER; } }
        public string CURRENT_PID { get { return _CURRENT_PID; } }
        public string CURRENT_DB_NAME { get { return _CURRENT_DB_NAME; } }

        private void FmDBConnection_Load(object sender, EventArgs e)
        {
            cmbServerName.DataSource = null;
            cmbDataName.DataSource = null;
            List<string> svrList = new List<string>();
            //svrList = SqlBaseInfo.GetSqlServerNames();//比较慢
            svrList = ComRegistry.GetDbServerList();
            cmbServerName.DataSource = svrList;

            cmbAuth.SelectedIndex = (int)SqlConnectionParams.SqlAuthentication.SqlServer;
            _SqlAuthMethod = (SqlConnectionParams.SqlAuthentication)cmbAuth.SelectedIndex;

            string lastDbSource = ComRegistry.GetLastAccessDbSource();
            if (!string.IsNullOrEmpty(lastDbSource))
            {
                SetDefDbSourceFromReg(lastDbSource);
            }
            cmbServerName.Focus();
        }

        /// <summary>
        /// 从注册表获取指定数据源基本信息
        /// </summary>
        private void SetDefDbSourceFromReg(string DbSource)
        {
            
            string DbSourceInfos = string.Empty;
            string DbPwd = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(DbSource))
                {
                    DbSourceInfos = ComRegistry.GetDbSourceInfos(DbSource);
                    cmbServerName.Text = DbSource;
                    Dictionary<String, String> map = splitStringToMap(DbSourceInfos);
                    //map["HostName"]
                    //map["Database"]
                    //map["Authentication"]
                    //map["UserName"]
                    //map["Password"]
                    //map["SavePassword"]
                    //map["ParamsKey"]
                    cmbAuth.SelectedIndex = Int32.Parse(map["Authentication"]);
                    if (map["Authentication"] == "1")
                    {
                        //SQL Server 身份验证
                        txtUserName.Text = map["UserName"];
                        ComDES des = new ComDES();
                        string desStr = des.DecryptString(ComRegistry.GetDbSourcePwd(map["ParamsKey"]), des.DESKEY);
                        txtPwd.Text = desStr;
                        chkSavePwd.Checked = (map["SavePassword"].ToUpper() == "TRUE") ? true : false;
                        
                    }
                    else
                    {
                        //Windows 身份验证
                        txtUserName.Text = "";
                        txtPwd.Text = "";
                        chkSavePwd.Checked = false;
                    }

                    cmbDataName.Text = map["Database"];
                }
                
            }
            catch (Exception)
            {

            }
        }

        public static Dictionary<String, String> splitStringToMap(string s)
        {
            Dictionary<String, String> map = new Dictionary<String, String>();
            String[] temp = s.Split(';'); //通过分号进行分割  
            for (int i = 0; i < temp.Length; i++)
            {
                String[] arr = temp[i].Split('='); //通过等号继续分割   
                map.Add(arr[0], arr[1]);
            }
            return map;
        }

        private void cmbServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefDbSourceFromReg(cmbServerName.Text);
        }

        private void cmbAuth_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SqlAuthMethod = (SqlConnectionParams.SqlAuthentication)cmbAuth.SelectedIndex;
            if (_SqlAuthMethod == SqlConnectionParams.SqlAuthentication.Windows)
            {
                lblUserName.Enabled = false;
                lblPwd.Enabled = false;
                txtUserName.Enabled = false;
                txtPwd.Enabled = false;
                chkSavePwd.Enabled = false;
            }
            else
            {
                lblUserName.Enabled = true;
                lblPwd.Enabled = true;
                txtUserName.Enabled = true;
                txtPwd.Enabled = true;
                chkSavePwd.Enabled = true;
            }
        }

        private void cmbDataName_DropDown(object sender, EventArgs e)
        {
            string oldDataName = cmbDataName.Text;
            SqlConnectionParams sqlcon = new SqlConnectionParams();
            sqlcon.Server = cmbServerName.Text.Trim();      //定义服务器名称
            sqlcon.AuthenticationMethod = _SqlAuthMethod;   //身份验证模式
            sqlcon.User = txtUserName.Text.Trim();          //定义用户名
            sqlcon.Password = txtPwd.Text.Trim();           //定义密码
            sqlcon.Catalog = "master";                      //定义数据库名称
            sqlcon.ConnectionString = "";

            List<string> dbList = new List<string>();
            dbList = SqlBaseInfo.GetDatabaseList(sqlcon.ConnectionString);
            cmbDataName.DataSource = null;
            cmbDataName.DataSource = dbList;

            if (!string.IsNullOrEmpty(oldDataName) && dbList.Contains(oldDataName, StringComparer.OrdinalIgnoreCase))
                cmbDataName.Text = oldDataName;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _msg_title = ComFunction.GetResxString(typeof(FmDBConnection), "TXT_MGSBOX_TITLE", "Error");
            //判断控件值的状态
            if (string.IsNullOrEmpty(cmbServerName.Text.Trim()))
            {
                _msg_val = ComFunction.GetResxString(typeof(FmDBConnection), "TXT_SERVER_NAME_EMPTY_ERR", "SQL server name can not be empty, please enter(select) the SQL server name.");
                MessageBox.Show(_msg_val, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbServerName.Focus();
                return;
            }
            if (_SqlAuthMethod == SqlConnectionParams.SqlAuthentication.SqlServer && string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                _msg_val = ComFunction.GetResxString(typeof(FmDBConnection), "TXT_USER_NAME_EMPTY_ERR", "User name can not be empty, please enter your user name.");
                MessageBox.Show(_msg_val, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }
            if (_SqlAuthMethod == SqlConnectionParams.SqlAuthentication.SqlServer && string.IsNullOrEmpty(txtPwd.Text.Trim()))
            {
                _msg_val = ComFunction.GetResxString(typeof(FmDBConnection), "TXT_USER_PWD_EMPTY_ERR", "The password can not be empty, please enter your password.");
                MessageBox.Show(_msg_val, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPwd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(cmbDataName.Text.Trim()))
            {
                _msg_val = ComFunction.GetResxString(typeof(FmDBConnection), "TXT_DB_NAME_EMPTY_ERR", "SQL database name can not be blank, please enter the database name.");
                MessageBox.Show(_msg_val, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDataName.Focus();
                return;
            }

            SqlConnectionParams sqlcon = new SqlConnectionParams();
            sqlcon.Server = cmbServerName.Text.Trim();      //定义服务器名称
            sqlcon.AuthenticationMethod = _SqlAuthMethod;   //身份验证模式
            sqlcon.User = txtUserName.Text.Trim();          //定义用户名
            sqlcon.Password = txtPwd.Text.Trim();           //定义密码
            sqlcon.Catalog = cmbDataName.Text.Trim();       //定义数据库名称
            sqlcon.ConnectionString = "";

            //数据库连接
            SqlComDB db = new SqlComDB(sqlcon.ConnectionString);
            try
            {
                int rtn = db.DbConnection();
                if (rtn == ComConst.FAILED)
                {
                    MessageBox.Show(db.expmsg, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Globals.ThisAddIn.comDB = db;
                SettingsDBInfo(db);
                RegWrites();
                //
                BuildAutocompleteMenu(sqlcon.ConnectionString);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //OK
            this.DialogResult = DialogResult.OK;

        }

        string[] keywords = { "select", "distinct", "from", "left", "right", "full", "outer", "join", "on", "where", "and", "in", "like", "order", "by", "group", "null", "insert", "into", "delete", "declare", "set" };
        string[] functions = { "isnull(^)", "count(^)", "sum(^)", "min(^)", "max(^)" };
        string[] snippets = { 
               "select * from ",
               "select count(*) from "
               };
        private void BuildAutocompleteMenu(string conection)
        {
            //获取SQLServer中所有数据表的字段名列表（含表、视图、同义词）
            List<string> SelectTipsList = new List<string>();
            SelectTipsList = SqlBaseInfo.GetAllColumnsList(conection);
            //获取SQLServer中的数据表名（含表、视图、同义词、表值函数）
            DataTable FromTipsList = new DataTable();
            FromTipsList = SqlBaseInfo.GetAllTables(conection);


            List<AutocompleteItem> items = new List<AutocompleteItem>();

            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item));
            foreach (var item in functions)
                items.Add(new FunctionSnippet(item));
            foreach (var item in snippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 6 });
            if (SelectTipsList != null)
            {
                foreach (var item in SelectTipsList)
                    items.Add(new AutocompleteItem(item) { ImageIndex = 0 });//Column
            }
            if (FromTipsList != null)
            {
                foreach (var item in FromTipsList.Select("xtype='U'"))
                    items.Add(new AutocompleteItem(item["name"].ToString()) { ImageIndex = 1 });//Table
                foreach (var item in FromTipsList.Select("xtype='V'"))
                    items.Add(new AutocompleteItem(item["name"].ToString()) { ImageIndex = 2 });//View
                foreach (var item in FromTipsList.Select("xtype='SN'"))
                    items.Add(new AutocompleteItem(item["name"].ToString()) { ImageIndex = 3 });//Synonym
                foreach (var item in FromTipsList.Select("xtype='IF'"))
                    items.Add(new FunctionSnippet(item["name"].ToString() + "(^)") { ImageIndex = 4 });//Table-valued function
            }

            items.Add(new InsertSpaceSnippet());
            items.Add(new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$"));

            //set as autocomplete source
            Globals.ThisAddIn.SqlEditor.popupMenu.Items.SetAutocompleteItems(items);
        }

        /// <summary>
        /// This item appears when any part of snippet text is typed
        /// </summary>
        class FunctionSnippet : SnippetAutocompleteItem
        {
            public FunctionSnippet(string snippet)
                : base(snippet)
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                var pattern = Regex.Escape(fragmentText);
                if (Regex.IsMatch(Text, "\\b" + pattern, RegexOptions.IgnoreCase))
                    return CompareResult.Visible;
                return CompareResult.Hidden;
            }
        }

        /// <summary>
        /// Divides numbers and words: "123AND456" -> "123 AND 456"
        /// Or "i=2" -> "i = 2"
        /// </summary>
        class InsertSpaceSnippet : AutocompleteItem
        {
            string pattern;

            public InsertSpaceSnippet(string pattern)
                : base("")
            {
                this.pattern = pattern;
            }

            public InsertSpaceSnippet()
                : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                if (Regex.IsMatch(fragmentText, pattern))
                {
                    Text = InsertSpaces(fragmentText);
                    if (Text != fragmentText)
                        return CompareResult.Visible;
                }
                return CompareResult.Hidden;
            }

            public string InsertSpaces(string fragment)
            {
                var m = Regex.Match(fragment, pattern);
                if (m == null)
                    return fragment;
                if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
                    return fragment;
                return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
            }

            public override string ToolTipTitle
            {
                get
                {
                    return Text;
                }
            }
        }

        /// <summary>
        /// 数据源基本信息取得
        /// </summary>
        /// <param name="db"></param>
        private void SettingsDBInfo(SqlComDB db)
        {
            DataTable dt = SqlBaseInfo.GetConnectedDBInfo(db);//GetDBInfo(db);
            if (dt != null && dt.Rows.Count > 0)
            {
                _SERVER_NAME = dt.Rows[0]["SERVER_NAME"].ToString();
                _DB_VER = dt.Rows[0]["DB_VER"].ToString();
                _SYS_USER = dt.Rows[0]["SYS_USER"].ToString();
                _CURRENT_PID = dt.Rows[0]["CURRENT_PID"].ToString();
                _CURRENT_DB_NAME = dt.Rows[0]["CURRENT_DB_NAME"].ToString();
            }
        }

        //private DataTable GetDBInfo(SqlComDB db)
        //{
        //    DataTable dt = new DataTable();
        //    StringBuilder strSQL = new StringBuilder();
        //    strSQL.Append("SELECT ");
        //    strSQL.Append("SERVERPROPERTY('SERVERNAME') AS SERVER_NAME ");
        //    strSQL.Append(" ,CAST(SERVERPROPERTY('productversion') AS VARCHAR)+' '+ CAST(SERVERPROPERTY ('productlevel') AS VARCHAR) +' '+ CAST(SERVERPROPERTY ('edition') AS VARCHAR) AS DB_VER ");
        //    strSQL.Append(" ,SYSTEM_USER AS SYS_USER ");
        //    strSQL.Append(" ,CAST(@@SPID AS VARCHAR) CURRENT_PID ");
        //    strSQL.Append(" ,DB_NAME() AS  CURRENT_DB_NAME ");
        //    strSQL.Append(" ,DB_ID() AS CURRENT_DB_ID ");

        //    try
        //    {
        //        dt = db.DbDataTable(strSQL.ToString(), "Table");
        //        return dt;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
            
        //}

        /// <summary>
        /// 数据源相关信息写入注册表
        /// </summary>
        private void RegWrites()
        {
            ComRegistry.SetLastAccessDbSource(cmbServerName.Text.Trim());   //最后访问服务器名称保存
            ComRegistry.SetLastAccessDbName(cmbDataName.Text.Trim());       //最后访问数据库名称保存
            //数据源基本信息保存
            if (chkSaveConInfo.Checked)
            {
                string regNam = cmbServerName.Text.Trim();
                string regVal = "HostName=" + cmbServerName.Text.Trim() + ";" +
                    "Database=" + cmbDataName.Text.Trim() + ";" +
                    "Authentication=" + ((int)_SqlAuthMethod).ToString() + ";" +
                    "UserName=" + txtUserName.Text.Trim() + ";" +
                    "Password=********" + ";" +
                    "SavePassword=" + chkSavePwd.Checked.ToString() + ";" +
                    "ParamsKey=" + ComRegistry.ReplaceKeyBackslash(cmbServerName.Text.Trim()) + ".params";
                ComRegistry.SetDbSource(regNam, regVal);
            }
            //数据源密码保存（DES加密）
            if (chkSavePwd.Checked && _SqlAuthMethod == SqlConnectionParams.SqlAuthentication.SqlServer)
            {
                ComDES des = new ComDES();
                string desStr = des.EncryptString(txtPwd.Text.Trim(), des.DESKEY);
                
                string regKey = ComRegistry.ReplaceKeyBackslash(cmbServerName.Text.Trim());
                string regVal = desStr;
                ComRegistry.SetDbSourcePwd(regKey, regVal);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //キャンセル
            this.DialogResult = DialogResult.Cancel;
        }

        private void FmDBConnection_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                bool blnCHK = this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        private void cmbServerName_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                chkSaveConInfo.Focus();
            }
        }

        private void chkSaveConInfo_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                cmbAuth.Focus();
            }
        }

        private void cmbAuth_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (txtUserName.Enabled)
                    txtUserName.Focus();
                else
                    cmbDataName.Focus();
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txtPwd.Focus();
            }
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                chkSavePwd.Focus();
            }
        }

        private void chkSavePwd_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                cmbDataName.Focus();
            }
        }

        private void cmbDataName_KeyDown(object sender, KeyEventArgs e)
        {
            //[Enter] 键移动到下一个控件
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                this.btnConnect.Focus();
                this.btnConnect.PerformClick();
            }
        }




    }
}
