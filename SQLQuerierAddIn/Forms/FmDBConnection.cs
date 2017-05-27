using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Threading.Tasks;
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

        private Task _task;
        //private Task<string> _task;
        //private Thread _thread;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private bool _isCancel = false;
 
        private System.Timers.Timer _timer;
        private SqlConnectionParams.SqlAuthentication _SqlAuthMethod;
        private string _msg_title = "Error";
        private string _msg_val = "";
        private Bitmap _image = new Bitmap(global::SQLQuerierAddIn.Properties.Resources.log_progressbar);
        private int _xDisplacement;//图片水平位移变量

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

        #region SetDefDbSourceFromReg() : 从注册表获取指定数据源基本信息
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
                    Dictionary<String, String> map = ComRegistry.splitStringToMap(DbSourceInfos);
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
        #endregion

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
            #region : 判断控件值的状态
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
            #endregion

            //Cursor.Current = Cursors.WaitCursor;
            _isCancel = false;
            timeStart();

            //SqlConnectionParams sqlcon = new SqlConnectionParams();
            //sqlcon.Server = cmbServerName.Text.Trim();      //定义服务器名称
            //sqlcon.AuthenticationMethod = _SqlAuthMethod;   //身份验证模式
            //sqlcon.User = txtUserName.Text.Trim();          //定义用户名
            //sqlcon.Password = txtPwd.Text.Trim();           //定义密码
            //sqlcon.Catalog = cmbDataName.Text.Trim();       //定义数据库名称
            //sqlcon.ConnectionString = "";
            //SqlComDB db = new SqlComDB(sqlcon.ConnectionString);

            ////数据库连接
            //try
            //{
            //    btnConnect.Enabled = false;
            //    int rtn = db.DbConnection();
            //    if (rtn == ComConst.FAILED)
            //    {
            //        MessageBox.Show(db.expmsg, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    Globals.ThisAddIn.comDB = db;
            //    SettingsDBInfo(db);
            //    RegWrites();
            //    //
            //    BuildAutocompleteMenu(sqlcon.ConnectionString);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //finally
            //{
            //    Cursor.Current = Cursors.Default;
            //    timerStop();
            //    picConn.Image = global::SQLQuerierAddIn.Properties.Resources.log_progressbar;
            //    btnConnect.Enabled = true;
            //}

            ////OK
            //this.DialogResult = DialogResult.OK;

            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            _task = Task.Factory.StartNew(() => ConnectionTask(_token), _token);
            //_task = new Task<string>(() => ConnectionTask(_token), _token);
            //_task.Start();
            //_task.ContinueWith(TaskEndedByCatch,TaskContinuationOptions.None);

            //try
            //{
            //    _task.Start();
            //    //Thread.Sleep(10000);
            //    //Task.WaitAll(_task);
            //    //MessageBox.Show(_task.IsCanceled.ToString() + " " + _task.IsCompleted.ToString());
            //    FrmCtrlsEnabled(true);
            //    //timerStop();
            //}
            //catch (AggregateException exs)
            //{
            //    foreach (var ex in exs.InnerExceptions)
            //    {
            //        Console.WriteLine("\nhi,我是OperationCanceledException：{0}\n", ex.Message);
            //    }
            //    //exs.Handle((err) => err is OperationCanceledException);
            //}
            //catch (OperationCanceledException)
            //{
            //    MessageBox.Show("cnacel");
            //}

            //ThreadPool.QueueUserWorkItem(state => ConnectionTask(_token),  _token);
            ////ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectionTask), _token);

            //_thread = new Thread(new ThreadStart(ConnectionTask));
            //_thread = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        if (_token.IsCancellationRequested)
            //        {
            //            //Console.WriteLine("线程被终止！");
            //            break;
            //        }
            //        //Console.WriteLine(DateTime.Now.ToString());
            //        //Thread.Sleep(1000);
            //        ConnectionTask(); 
            //    }
            //});
            //_thread.Start();
            //_cts.Cancel();
            //_cts.Dispose();
        }

        private void TaskEndedByCatch(Task<string> task)
        {
            try
            {
                if (!_isCancel && !string.IsNullOrEmpty(task.Result))
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        MessageBox.Show(task.Result, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
                FrmCtrlsEnabled(true);
            }
            catch (AggregateException e)
            {
                //e.Handle((err) => err is OperationCanceledException);
                //MessageBox.Show(task.IsCanceled.ToString() + "; " + task.IsCompleted.ToString());
            }
            catch (OperationCanceledException)
            {
                //MessageBox.Show("cnacel");
            }
        }

        //Task test
        private void ConnectionTask(CancellationToken ct)
        //private string ConnectionTask(CancellationToken ct)
        //private void ConnectionTask()
        {
            string rtnMsg = "";

            if (IsDisposed || !this.IsHandleCreated) return;

            ////判断是否取消任务
            while (!ct.IsCancellationRequested)
            {
                //ct.ThrowIfCancellationRequested();

                if (IsDisposed || !this.IsHandleCreated || this.DialogResult==DialogResult.OK) break;

                SqlConnectionParams sqlcon = new SqlConnectionParams();
                this.Invoke((EventHandler)(delegate
                {
                    sqlcon.Server = cmbServerName.Text.Trim();      //定义服务器名称
                    sqlcon.AuthenticationMethod = _SqlAuthMethod;   //身份验证模式
                    sqlcon.User = txtUserName.Text.Trim();          //定义用户名
                    sqlcon.Password = txtPwd.Text.Trim();           //定义密码
                    sqlcon.Catalog = cmbDataName.Text.Trim();       //定义数据库名称
                    sqlcon.ConnectionString = "";
                }));
                SqlComDB db = new SqlComDB(sqlcon.ConnectionString);

                try
                {
                    FrmCtrlsEnabled(false);
                    int rtn = db.DbConnection();
                    if (rtn == ComConst.FAILED)
                    {
                        timerStop();
                        rtnMsg = db.expmsg;
                        return;
                    }
                    else
                    {
                        //生成ComDB
                        Globals.ThisAddIn.comDB = db;
                        //状态栏显示用数据源基本信息
                        SettingsDBInfo(db);
                        //将当前登录成功的数据源信息写入注册表
                        RegWrites();
                        //SQL脚本编辑自动提示生成
                        BuildAutocompleteMenu(sqlcon.ConnectionString);
                        //OK
                        this.DialogResult = DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    timerStop();
                    rtnMsg = ex.ToString();
                    return;
                }
                finally
                {
                    timerStop();
                    FrmCtrlsEnabled(true);
                    _cts = null;
                    if (!_isCancel && !string.IsNullOrEmpty(rtnMsg))
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            MessageBox.Show(rtnMsg, _msg_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }));
                    }
                }

            }

            //return rtnMsg;

        }

        #region FrmCtrlsEnabled() : 画面控件控制处理
        /// <summary>
        /// 画面控件控制处理
        /// </summary>
        /// <param name="enable"></param>
        private void FrmCtrlsEnabled(bool enable)
        {
            SqlConnectionParams.SqlAuthentication SqlAuth;
            if (btnConnect.InvokeRequired)
            {
                this.Invoke((EventHandler)(delegate
                {
                    SqlAuth = (SqlConnectionParams.SqlAuthentication)cmbAuth.SelectedIndex;
                    lblServerName.Enabled = enable;
                    cmbServerName.Enabled = enable;
                    chkSaveConInfo.Enabled = enable;
                    lblAuth.Enabled = enable;
                    cmbAuth.Enabled = enable;
                    lblUserName.Enabled = enable;
                    txtUserName.Enabled = enable;
                    lblPwd.Enabled = enable;
                    txtPwd.Enabled = enable;
                    chkSavePwd.Enabled = enable;
                    if (SqlAuth == SqlConnectionParams.SqlAuthentication.Windows && txtUserName.Enabled)
                    {
                        lblUserName.Enabled = false;
                        lblPwd.Enabled = false;
                        txtUserName.Enabled = false;
                        txtPwd.Enabled = false;
                        chkSavePwd.Enabled = false;
                    }
                    lblDataName.Enabled = enable;
                    cmbDataName.Enabled = enable;
                    btnConnect.Enabled = enable;
                }));
            }
            else
            {
                SqlAuth = (SqlConnectionParams.SqlAuthentication)cmbAuth.SelectedIndex;
                lblServerName.Enabled = enable;
                cmbServerName.Enabled = enable;
                chkSaveConInfo.Enabled = enable;
                lblAuth.Enabled = enable;
                cmbAuth.Enabled = enable;
                lblUserName.Enabled = enable;
                txtUserName.Enabled = enable;
                lblPwd.Enabled = enable;
                txtPwd.Enabled = enable;
                chkSavePwd.Enabled = enable;
                if (SqlAuth == SqlConnectionParams.SqlAuthentication.Windows && txtUserName.Enabled)
                {
                    lblUserName.Enabled = false;
                    lblPwd.Enabled = false;
                    txtUserName.Enabled = false;
                    txtPwd.Enabled = false;
                    chkSavePwd.Enabled = false;
                }
                lblDataName.Enabled = enable;
                cmbDataName.Enabled = enable;
                btnConnect.Enabled = enable;
            }
        }
        #endregion

        #region timer相关事件
        /// <summary>
        /// timer启动
        /// </summary>
        private void timeStart()
        {
            _xDisplacement = 0;
            _timer = new System.Timers.Timer(100);
            _timer.Enabled = true;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.AutoReset = true;
            _timer.Start();
        }
        /// <summary>
        /// timer停止
        /// </summary>
        private void timerStop()
        {
            _timer.Stop();
            _timer.Enabled = false;
        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (btnConnect.InvokeRequired)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        //给图片框创建画布
                        Graphics graphics = picConn.CreateGraphics();
                        //移动的量
                        _xDisplacement += 10;
                        if (_xDisplacement >= picConn.Width) _xDisplacement = 0;
                        //画图
                        graphics.DrawImage(_image, _xDisplacement, 0);
                        //使图片首尾相连
                        int xx = picConn.Width - _xDisplacement;
                        if (xx > 0) xx = -xx;
                        //画图
                        graphics.DrawImage(_image, xx, 0);
                    }));
                }
                else
                {
                    //给图片框创建画布
                    Graphics graphics = picConn.CreateGraphics();
                    //移动的量
                    _xDisplacement += 10;
                    if (_xDisplacement >= picConn.Width) _xDisplacement = 0;
                    //画图
                    graphics.DrawImage(_image, _xDisplacement, 0);
                    //使图片首尾相连
                    int xx = picConn.Width - _xDisplacement;
                    if (xx > 0) xx = -xx;
                    //画图
                    graphics.DrawImage(_image, xx, 0);
                }
            }
            catch (Exception)
            {
            }

        }
        #endregion

        #region SettingsDBInfo() : 数据源基本信息取得
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
        #endregion

        #region RegWrites() : 数据源相关信息写入注册表
        /// <summary>
        /// 数据源相关信息写入注册表
        /// </summary>
        private void RegWrites()
        {
            if (btnConnect.InvokeRequired)
            {
                this.Invoke((EventHandler)(delegate
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
                }));
            }
            else
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
        }
        #endregion

        #region : 生成SQL脚本输入自动提示相关
        string[] keywords = { "select", "distinct", "from", "left", "right", "full", "outer", "join", "on", "where", "and", "in", "like", "order", "by", "group", "null", "insert", "into", "delete", "declare", "set" };
        string[] functions = { "isnull(^)", "count(^)", "sum(^)", "min(^)", "max(^)" };
        string[] snippets = { 
               "select * from ",
               "select count(*) from "
               };
        /// <summary>
        /// 生成SQL脚本输入自动提示
        /// </summary>
        /// <param name="conection"></param>
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
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ////キャンセル
            //this.DialogResult = DialogResult.Cancel;

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
                _isCancel = true;
                timerStop();
                FrmCtrlsEnabled(true);
            }
            else
            {
                //キャンセル
                this.DialogResult = DialogResult.Cancel;
            }

            
        }

        private void FmDBConnection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cts != null) e.Cancel = true; ;
        }

        #region : [Enter] 键移动到下一个控件
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

        #endregion


    }
}
