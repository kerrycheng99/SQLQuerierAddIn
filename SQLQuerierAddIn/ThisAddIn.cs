using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

using SQLQuerierAddIn.Controls;
using SQLQuerierAddIn.Common;

namespace SQLQuerierAddIn
{
    public partial class ThisAddIn
    {
        private TpSQLEditor _tpSqlEditor;
        private Microsoft.Office.Tools.CustomTaskPane _tpSqlEditorCTP;
        private SqlComDB _comDB;

        /// <summary>SQL查询编辑器用户控件</summary>
        public TpSQLEditor SqlEditor { get { return _tpSqlEditor; } }
        /// <summary>SQL查询编辑器Office CustomTaskPane</summary>
        public Microsoft.Office.Tools.CustomTaskPane tpSqlEditorCTP { get { return _tpSqlEditorCTP; } }
        /// <summary>SQL数据库连接类</summary>
        public SqlComDB comDB { get { return _comDB; } set { _comDB = value; } }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //追加SQL编辑器任务窗格[Custom TaskPane]
            AddTpSqlEdit();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            try
            {
                _comDB.DbClose();
                _comDB = null;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 创建SQL查询编辑器自定义任务窗格且靠底部显示
        /// <para>Create the Custom TaskPane and Dock Bottom</para>
        /// <para></para>
        /// </summary>
        private void AddTpSqlEdit()
        {
            _tpSqlEditor = new TpSQLEditor();
            //_tpSqlEditorCTP = CustomTaskPanes.Add(_tpSqlEditor, "SQL Editor");
            string strTitle = _tpSqlEditor.ucTitle + " " + ComFunction.GetAppEdition();
            _tpSqlEditorCTP = CustomTaskPanes.Add(_tpSqlEditor, strTitle);
            _tpSqlEditorCTP.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionBottom;
            ////Show TaskPane
            //TpSqlEditCustomTaskPane.Visible = true;
            Globals.Ribbons.SqlRibbon.btnDisconnect.Enabled = false;
        }

        #region VSTO generated code

        //protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        //{
        //    return new Ribbon();
        //}

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }

}
