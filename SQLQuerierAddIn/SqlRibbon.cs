using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using SQLQuerierAddIn.Common;
using SQLQuerierAddIn.Controls;
using SQLQuerierAddIn.Forms;
using FastColoredTextBoxNS;
using System.IO;

namespace SQLQuerierAddIn
{
    public partial class SqlRibbon
    {
        static System.Timers.Timer timer;

        private void SqlRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            timer = new System.Timers.Timer(500);
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = true;
            timer.Start();

            //初始化SQL收藏夹
            LoadFavMenu();
        }

        /// <summary>
        /// 用户点击SQL编辑器操作窗格右上角[x]关闭操作窗格后，
        /// [SQL编辑器]Ribbon按钮（RibbonToggleButton）选中状态不会自动改变，
        /// 故暂时用Timer事件来轮循改变选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                bool isShow = Globals.ThisAddIn.tpSqlEditorCTP.Visible;
                bool isChecked = Globals.Ribbons.SqlRibbon.btnSQLEditor.Checked;
                if (isChecked != isShow)
                {
                    Globals.Ribbons.SqlRibbon.btnSQLEditor.Checked = isShow;
                }
            }
            catch (Exception)
            {
            }

        }

        private void btnSQLEditor_Click(object sender, RibbonControlEventArgs e)
        {
            bool isShow = ((RibbonToggleButton)sender).Checked;
            Globals.ThisAddIn.tpSqlEditorCTP.Visible = isShow;
        }

        private void btnConnection_Click(object sender, RibbonControlEventArgs e)
        {
            using (FmDBConnection f = new FmDBConnection())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Globals.Ribbons.SqlRibbon.btnDisconnect.Enabled = true;
                    Globals.Ribbons.SqlRibbon.btnConnection.Enabled = false;
                    bool isShow = Globals.ThisAddIn.tpSqlEditorCTP.Visible;
                    if (!isShow)
                    {
                        Globals.ThisAddIn.tpSqlEditorCTP.Visible = true;
                    }
                    //SQL编辑器数据源状态显示
                    Globals.ThisAddIn.SqlEditor.lblExeMsg.Image = global::SQLQuerierAddIn.Properties.Resources.netstatus_on;
                    Globals.ThisAddIn.SqlEditor.lblSvrDbVerFilter.Visible = true;
                    Globals.ThisAddIn.SqlEditor.lblSvrDbVer.Text = f.SERVER_NAME;
                    Globals.ThisAddIn.SqlEditor.lblUserPidFilter.Visible = true;
                    Globals.ThisAddIn.SqlEditor.lblUserPid.Text = f.SYS_USER + "(" + f.CURRENT_PID + ")";
                    Globals.ThisAddIn.SqlEditor.lblDbNameFilter.Visible = true;
                    Globals.ThisAddIn.SqlEditor.lblDbName.Text = f.CURRENT_DB_NAME;
                }
            }

        }

        private void btnDisconnect_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.comDB.DbClose();
            Globals.ThisAddIn.comDB = null;

            Globals.Ribbons.SqlRibbon.btnConnection.Enabled = true;
            Globals.Ribbons.SqlRibbon.btnDisconnect.Enabled = false;
            
            //SQL编辑器数据源状态清空
            Globals.ThisAddIn.SqlEditor.lblExeMsg.Image = global::SQLQuerierAddIn.Properties.Resources.netstatus_off;
            Globals.ThisAddIn.SqlEditor.lblSvrDbVerFilter.Visible = false;
            Globals.ThisAddIn.SqlEditor.lblSvrDbVer.Text = "";
            Globals.ThisAddIn.SqlEditor.lblUserPidFilter.Visible = false;
            Globals.ThisAddIn.SqlEditor.lblUserPid.Text = "";
            Globals.ThisAddIn.SqlEditor.lblDbNameFilter.Visible = false;
            Globals.ThisAddIn.SqlEditor.lblDbName.Text = "";
            //
            BuildAutocompleteMenu();
        }

        private void btnFavorites_Click(object sender, RibbonControlEventArgs e)
        {
            FmSQLFavorites f = new FmSQLFavorites();
            f.ShowDialog();
        }

        private void btnAbout_Click(object sender, RibbonControlEventArgs e)
        {
            FmAbout f = new FmAbout();
            f.ShowDialog();
        }

        string[] keywords = { "select", "distinct", "from", "left", "right", "full", "outer", "join", "on", "where", "and", "in", "like", "order", "by", "group", "null", "insert", "into", "delete", "declare", "set" };
        string[] functions = { "isnull(^)", "count(^)", "sum(^)", "min(^)", "max(^)" };
        string[] snippets = { 
               "select * from ",
               "select count(*) from "
               };

        private void BuildAutocompleteMenu()
        {
            List<AutocompleteItem> items = new List<AutocompleteItem>();

            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item));
            foreach (var item in functions)
                items.Add(new FunctionSnippet(item));
            foreach (var item in snippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 6 });

            items.Add(new InsertSpaceSnippet());
            items.Add(new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$"));

            //set as autocomplete source
            Globals.ThisAddIn.SqlEditor.popupMenu.Items.SetAutocompleteItems(items);
        }


        #region : 动态生成SQL收藏夹菜单相关
        private string _FileFilter = "*.sql";       //文件类型
        private string _LblText = string.Empty;

        #region GetFavRootPath : 从注册表获取SQL收藏夹根目录路径
        /// <summary>
        /// 从注册表获取SQL收藏夹根目录路径；
        /// 注册表未设定收藏夹根目录路径 或 注册表设定的收藏夹根目录路径不存在时，
        /// 取用户配置文件夹
        /// </summary>
        /// <returns></returns>
        private string GetFavRootPath()
        {
            string rootpath = ComRegistry.GetFavRootFloder();
            //注册表未设定收藏夹根目录路径 或 注册表设定的收藏夹根目录路径不存在时
            //取用户配置文件夹
            if (string.IsNullOrEmpty(rootpath) || !Directory.Exists(rootpath))
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                rootpath = Path.Combine(path, "SQLFavorites");
            }

            return rootpath;
        }
        #endregion

        #region LoadFavMenu : 初始化SQL收藏夹菜单
        /// <summary>
        /// 初始化SQL收藏夹菜单
        /// </summary>
        private void LoadFavMenu()
        {
            mnuFavorites.Dynamic = true;
            mnuFavorites.Items.Clear();
            mnuFavorites.Items.Add(mnuOption);

            string rootpath = GetFavRootPath();
            ExpendFavMenu(rootpath, mnuFavorites);
        }

        #endregion

        #region ExpendFavMenu ： 递归获取指定文件夹下的所有子目录及文件并生成收藏夹菜单
        /// <summary>
        /// 递归获取指定文件夹下的所有子目录及文件，
        /// 并生成收藏夹菜单
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        private void ExpendFavMenu(string path, Microsoft.Office.Tools.Ribbon.RibbonMenu mnu)
        {
            if (!Directory.Exists(path)) return;
            try
            {
                //目录下的文件、文件夹集合
                string[] dirArr = System.IO.Directory.GetDirectories(path);
                string[] rootfileArr = System.IO.Directory.GetFiles(path, _FileFilter);

                //1.显示文件夹统计信息
                if (dirArr.Length > 0)
                {
                    _LblText = ComFunction.GetResxString(typeof(FavView), "TXT_INC_DIR", "Included {0} Directories");
                    Microsoft.Office.Tools.Ribbon.RibbonSeparator sepfo = this.Factory.CreateRibbonSeparator();
                    sepfo.Title = string.Format(_LblText, dirArr.Length);
                    mnu.Items.Add(sepfo);
                }

                //2.文件夹递归生成文件夹菜单
                for (int i = 0; i < dirArr.Length; i++)
                {
                    //增加父节点
                    string[] foArr = System.IO.Directory.GetDirectories(dirArr[i]);
                    string[] flArr = System.IO.Directory.GetFiles(dirArr[i], _FileFilter);
                    string dirName = Path.GetFileName(dirArr[i]);

                    if (foArr.Length == 0 && flArr.Length == 0)
                    {
                        //空目录时
                        Microsoft.Office.Tools.Ribbon.RibbonButton subbt = this.Factory.CreateRibbonButton();
                        subbt.Label = dirName;
                        subbt.Tag = dirArr[i];
                        subbt.OfficeImageId = "ReviewNewComment";
                        mnu.Items.Add(subbt);
                    }
                    else
                    {
                        //非空目录时
                        Microsoft.Office.Tools.Ribbon.RibbonMenu subfo = this.Factory.CreateRibbonMenu();
                        subfo.Dynamic = true;
                        subfo.Label = dirName;
                        subfo.Tag = dirArr[i];
                        subfo.OfficeImageId = "FileOpen";  
                        subfo.ShowImage = true;
                        mnu.Items.Add(subfo);
                        //递归
                        ExpendFavMenu(dirArr[i], subfo);
                    }
                }

                //3.显示文件统计信息
                if (rootfileArr.Length > 0)
                {
                    _LblText = ComFunction.GetResxString(typeof(FavView), "TXT_INC_FILE", "Included {0} Files");
                    Microsoft.Office.Tools.Ribbon.RibbonSeparator sepfl = this.Factory.CreateRibbonSeparator();
                    sepfl.Title = string.Format(_LblText, rootfileArr.Length);
                    mnu.Items.Add(sepfl);
                }

                //4.文件直接生成收藏夹菜单
                foreach (string var in rootfileArr)
                {
                    string filename = Path.GetFileName(var);
                    Microsoft.Office.Tools.Ribbon.RibbonButton subfl = this.Factory.CreateRibbonButton();
                    subfl.Label = filename;
                    subfl.Tag = var;
                    //subfl.OfficeImageId = "MailMergeGreetingLineInsert";
                    subfl.Image = global::SQLQuerierAddIn.Properties.Resources.Document;
                    subfl.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(subflOpen_Click);
                    mnu.Items.Add(subfl);
                }
            }
            catch { }
        }
        /// <summary>
        /// 收藏夹菜单Click事件 ： 打开SQL脚本文件并执行查询。
        /// 收藏夹对应为具体文件时有效。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subflOpen_Click(object sender, RibbonControlEventArgs e)
        {
            //打开SQL文档
            string filepath = ((RibbonButton)sender).Tag.ToString();
            if (File.Exists(filepath))
            {
                StreamReader srFile = new StreamReader(filepath, Encoding.Default);
                string strContents = srFile.ReadToEnd();
                srFile.Close();
                if (!string.IsNullOrEmpty(strContents))
                {
                    Globals.ThisAddIn.tpSqlEditorCTP.Visible = true;
                    Globals.ThisAddIn.SqlEditor.fctbSQLEdit.Text = strContents;
                    Globals.ThisAddIn.SqlEditor.PopulateFromSql();
                }
            }
            
        }
        #endregion
        /// <summary>
        /// 弹出设置SQL收藏夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, RibbonControlEventArgs e)
        {
            FmSQLFavorites f = new FmSQLFavorites();
            f.ShowDialog();
        }
        /// <summary>
        /// 刷新SQL收藏夹菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RibbonControlEventArgs e)
        {
            //刷新SQL收藏夹
            LoadFavMenu();
        }
        #endregion

    }
}
