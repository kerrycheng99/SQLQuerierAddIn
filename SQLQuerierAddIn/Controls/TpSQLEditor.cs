using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Office.Interop.Excel;
using SQLQuerierAddIn.Common;
using SQLQuerierAddIn.Controls;
using SQLQuerierAddIn.Forms;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace SQLQuerierAddIn.Controls
{
    public partial class TpSQLEditor : UserControl
    {
        public AutocompleteMenu popupMenu;
        string[] keywords = { "select", "distinct", "from", "left", "right", "full", "outer", "join", "on", "where", "and", "in", "like", "order", "by", "group", "null", "insert", "into", "delete", "declare", "set" };
        //string[] functions = { "isnull", "count", "sum", "min", "max" };
        string[] functions = { "isnull(^)", "count(^)", "sum(^)", "min(^)", "max(^)" };
        string[] snippets = { 
               "select * from ",
               "select count(*) from "
               };

        private string _ucTitle = "SQL Editor";
        /// <summary>SQL编辑器显示名称，供多语言显示使用</summary>
        public string ucTitle { get { return _ucTitle; } }

        public TpSQLEditor()
        {
            InitializeComponent();

            //初始化其他补足
            lblRtnCnt.Text = "0 " + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_ROW", "Row(s)");
            _ucTitle = ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_FORM_TITLE", "SQL Editor");
            ImageList iconsList = new ImageList();
            iconsList.ImageSize = new Size(13, 13);
            iconsList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.script_edit);
            iconsList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.list_information);
            tabCtrlSQLEdit.ImageList = iconsList;
            tabPageSQLEdit.ImageIndex = 0;
            tabPageMsg.ImageIndex = 1;

            //create autocomplete popup menu
            popupMenu = new AutocompleteMenu(fctbSQLEdit);
            ImageList tipImgList = new ImageList();
            tipImgList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.db_column);
            tipImgList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.db_table);
            tipImgList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.db_view);
            tipImgList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.db_synonym);
            tipImgList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.db_function);
            tipImgList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.db_procedure);
            tipImgList.Images.Add(global::SQLQuerierAddIn.Properties.Resources.shortcut);
            popupMenu.Items.ImageList = tipImgList;
            popupMenu.SearchPattern = @"[\w\.:=!<>]";
            popupMenu.AllowTabKey = true;
            //
            BuildAutocompleteMenu();
        }

        /// <summary>
        /// [获取数据]按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            PopulateFromSql();
        }

        #region PopulateFromSql ： 从SQL Server获取数据并填充Excel工作表
        /// <summary>
        /// 从SQL Server获取数据并填充Excel工作表
        /// </summary>
        public void PopulateFromSql()
        {
            try
            {
                tabPageSQLEdit.Show();
                tabCtrlSQLEdit.SelectedIndex = 0;//tabPageSQLEdit

                if (string.IsNullOrEmpty(fctbSQLEdit.Text)) return;

                //数据库连接设定
                SqlComDB db = Globals.ThisAddIn.comDB;
                if (db == null)
                {
                    using (FmDBConnection f = new FmDBConnection())
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            Globals.Ribbons.SqlRibbon.btnDisconnect.Enabled = true;
                            Globals.Ribbons.SqlRibbon.btnConnection.Enabled = false;
                            db = Globals.ThisAddIn.comDB;
                            //SQL编辑器数据源状态显示
                            Globals.ThisAddIn.SqlEditor.lblExeMsg.Image = global::SQLQuerierAddIn.Properties.Resources.netstatus_on;
                            Globals.ThisAddIn.SqlEditor.lblSvrDbVerFilter.Visible = true;
                            Globals.ThisAddIn.SqlEditor.lblSvrDbVer.Text = f.SERVER_NAME;
                            Globals.ThisAddIn.SqlEditor.lblUserPidFilter.Visible = true;
                            Globals.ThisAddIn.SqlEditor.lblUserPid.Text = f.SYS_USER + "(" + f.CURRENT_PID + ")";
                            Globals.ThisAddIn.SqlEditor.lblDbNameFilter.Visible = true;
                            Globals.ThisAddIn.SqlEditor.lblDbName.Text = f.CURRENT_DB_NAME;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                DateTime begdt = System.DateTime.Now;
                DateTime enddt = System.DateTime.Now;
                TimeSpan begts = new TimeSpan(begdt.Ticks);
                TimeSpan endts = new TimeSpan(enddt.Ticks);
                TimeSpan ts = begts.Subtract(endts).Duration();
                //lblExeMsg.Text = "Executing the query...";//"正在执行查询...";
                lblExeMsg.Text = ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_EXECUTING_QRY", "Executing the query...");

                var strSql = fctbSQLEdit.Text;
                var dt = db.DbDataTable(strSql, "TBL");
                if (dt == null)
                {
                    rtbMsg.Text = db.expmsg;
                    rtbMsg.ForeColor = Color.Red;
                    tabPageMsg.Show();
                    tabCtrlSQLEdit.SelectedIndex = 1;//tabPageMsg
                    lblExeMsg.Image = global::SQLQuerierAddIn.Properties.Resources.qry_warning;
                    //lblExeMsg.Text = "Inquiry has been completed, but with errors.";//"查询已完成，但有错误。";
                    lblExeMsg.Text = ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_QRY_COMPL_BUT_ERR", "Inquiry has been completed, but with errors.");
                    lblUseTime.Text = "00:00:00";
                    lblRtnCnt.Text = "0 Rows(s)";
                    progressBarGetData.Value = 0;
                    return;
                }
                else
                {
                    //rtbMsg.Text = dt.Rows.Count <= ushort.MaxValue ?
                    //    dt.Rows.Count.ToString() + " Row(s)" :
                    //    "Query: " + dt.Rows.Count.ToString() + " Row(s)" + "\r\n" + "Display: " + ushort.MaxValue.ToString() + " Row(s)";
                    rtbMsg.Text = dt.Rows.Count <= ushort.MaxValue ?
                        dt.Rows.Count.ToString() + " " + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_ROW","Row(s)") :
                        ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_QUERY","Query") + ": " + dt.Rows.Count.ToString() + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_ROW", "Row(s)") + "\r\n"
                        + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_DISPLAY","Display") + ": " + ushort.MaxValue.ToString() + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_ROW", "Row(s)");
                    
                    rtbMsg.ForeColor = Color.Black;
                    //lblRtnCnt.Text = dt.Rows.Count <= ushort.MaxValue ?
                    //    dt.Rows.Count.ToString() + " Row(s)" :
                    //    ushort.MaxValue.ToString() + " Row(s)";
                    lblRtnCnt.Text = dt.Rows.Count <= ushort.MaxValue ?
                        dt.Rows.Count.ToString() + " " + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_ROW", "Row(s)") :
                        ushort.MaxValue.ToString() + " " + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_ROW", "Row(s)");
                    lblExeMsg.Image = global::SQLQuerierAddIn.Properties.Resources.qry_ok;
                    //lblExeMsg.Text = "Query has executed successfully.";//"查询已成功执行。";
                    lblExeMsg.Text = ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_QRY_SUCCESS", "Query has executed successfully.");
                    enddt = System.DateTime.Now;
                    endts = new TimeSpan(enddt.Ticks);
                    ts = begts.Subtract(endts).Duration();
                    string strTimes = ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + ts.Milliseconds.ToString();
                    lblUseTime.Text = strTimes;
                    if (tabCtrlSQLEdit.SelectedIndex != 0) tabCtrlSQLEdit.SelectedIndex = 0;

                    ////
                    //ComRegistry.SetLastSQLQuery(fctbSQLEdit.Text);
                }

                // Define the active Workbook
                string qryName = this.tabPageSQLEdit.Text;
                var wb = Globals.ThisAddIn.Application.ActiveWorkbook as Workbook;
                if (wb == null)
                {
                    //如果还未建立[SQL编辑]工作簿则建立之
                    Globals.ThisAddIn.Application.Workbooks.Add();
                    wb = Globals.ThisAddIn.Application.ActiveWorkbook as Workbook;
                    wb.Application.Caption = "Microsoft Excel";
                    wb.Application.ActiveWindow.Caption = qryName;
                    //2015.12.03 del, use default worksheet name
                    //int seq = 1;
                    //foreach (Worksheet ws in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                    //{
                    //    ws.Name = qryName + seq.ToString();
                    //    seq++;
                    //}
                    //<---
                }
                //--->2015.12.03 del for Excel2013
                //else if (wb.Application.ActiveWindow.Caption != qryName)
                //{
                //    //如果当前工作簿不是[SQL编辑]工作簿,则循环所有工作簿查找[SQL编辑]工作簿并激活
                //    bool isCreated = false;
                //    foreach (Workbook item in Globals.ThisAddIn.Application.Workbooks)
                //    {
                //        ((Microsoft.Office.Interop.Excel._Workbook)item).Activate();
                //        if (item.Application.ActiveWindow.Caption == qryName)
                //        {
                //            isCreated = true;
                //            break;
                //        }
                //    }
                //    //如果所有工作簿中都不存在[SQL编辑]工作簿则建立之,工作表名称顺延
                //    if (!isCreated)
                //    {
                //        Globals.ThisAddIn.Application.Workbooks.Add();
                //        wb = Globals.ThisAddIn.Application.ActiveWorkbook as Workbook;
                //        wb.Application.Caption = "Microsoft Excel";
                //        wb.Application.ActiveWindow.Caption = qryName;
                //        int seq = 1;
                //        foreach (Worksheet ws in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                //        {
                //            ws.Name = qryName + seq.ToString();
                //            seq++;
                //        }
                //    }
                //}
                //<---

                //foreach (Worksheet ws in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                //{
                //    if (ws.Name == _qrySheetName)
                //    {
                //        ws.Activate();
                //        break;
                //    }
                //}

                // Define the active Worksheet
                var sht = Globals.ThisAddIn.Application.ActiveSheet as Worksheet;
                if (sht.UsedRange.Rows.Count > 1)
                {
                    //当前工作表有内容时,激活相邻的空工作表
                    bool isEmptySheet = false;
                    foreach (Worksheet ws in Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets)
                    {
                        if (ws.UsedRange.Rows.Count <= 1)
                        {
                            ((Microsoft.Office.Interop.Excel._Worksheet)ws).Activate();
                            isEmptySheet = true;
                            ws.Cells.EntireRow.Delete(Type.Missing);
                            sht = ws;
                            break;
                        }
                    }
                    //当前工作簿都无空工作表时,新追加工作表
                    if (!isEmptySheet)
                    {
                        int cnt = Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets.Count;
                        Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets.Add(After: Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets[cnt]);
                        sht = Globals.ThisAddIn.Application.ActiveSheet as Worksheet;
                        //--->2015.12.03 del, use default worksheet name
                        //int seq = cnt + 1;
                        //sht.Name = qryName + seq.ToString();
                        //<---
                    }
                }
                else
                {
                    //当前工作表无内容时
                    sht.Cells.EntireRow.Delete(Type.Missing);
                }
                sht.Cells.Font.Size = 10;
                //sht.Application.ScreenUpdating = false;

                //总栏位数
                int dtColCnt = dt.Columns.Count;
                //总记录条数
                int dtRowCnt = dt.Rows.Count;
                //每个SHEET的行数
                int SheetMaxRow = ushort.MaxValue;   //excel 65536 rows

                //第一行表头
                // Add the header the first time through 
                for (int col = 1; col <= dtColCnt && col <= byte.MaxValue; col++)
                {
                    if (sht != null)
                    {
                        sht.Cells[1, col] = dt.Columns[col - 1].ColumnName;

                        if (dt.Columns[col - 1].DataType == typeof(string))
                        {
                            ((Microsoft.Office.Interop.Excel.Range)sht.Columns[col, Type.Missing]).NumberFormatLocal = "@"; //文字
                        }
                    }
                }
                //第一行表头格式设定
                int begCol = 1;
                int endCol = (dtColCnt <= byte.MaxValue) ? dtColCnt : byte.MaxValue;
                if (sht != null)
                {
                    //标题栏背景渐变设置
                    Microsoft.Office.Interop.Excel.Range titleRange = sht.Range[sht.Cells[1, begCol], sht.Cells[1, endCol]];
                    titleRange.Interior.Pattern = XlPattern.xlPatternLinearGradient;
                    LinearGradient grd = titleRange.Interior.Gradient;
                    grd.Degree = 90;
                    grd.ColorStops.Clear();
                    ColorStop cs = grd.ColorStops.Add(0);
                    cs.ThemeColor = (int)XlThemeColor.xlThemeColorDark1;
                    cs.TintAndShade = 0;
                    cs = grd.ColorStops.Add(1);
                    cs.ThemeColor = (int)XlThemeColor.xlThemeColorDark1;
                    cs.TintAndShade = -5.09659108249153E-02;
                    titleRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    titleRange.Borders.ThemeColor = 1;
                    titleRange.Borders.TintAndShade = -0.14996795556505;
                    titleRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    //sht.Cells[1,1].Select();
                    //Globals.ThisAddIn.Application.ActiveWindow.SplitColumn = 0;
                    //Globals.ThisAddIn.Application.ActiveWindow.SplitRow = 1;
                    //Globals.ThisAddIn.Application.ActiveWindow.FreezePanes = true;
                }

                var rowNo = 1;
                progressBarGetData.Minimum = 0;
                progressBarGetData.Maximum = dtRowCnt > SheetMaxRow ? SheetMaxRow : dtRowCnt;

                // Loop thrue the Datatable and add it to Excel
                foreach (DataRow dr in dt.Rows)
                {
                    if (rowNo > SheetMaxRow) break;

                    progressBarGetData.Value = rowNo;

                    rowNo++;
                    for (var i = 1; i <= dtColCnt && i <= byte.MaxValue; i++)
                    {
                        if (sht != null) sht.Cells[rowNo, i] = dr[i - 1].ToString();
                    }
                    //progressBarGetData.Refresh();
                }

                sht.Cells.EntireColumn.AutoFit();

                //向Excel插入表格（ListObject）
                int endRowNo = dtRowCnt <= (SheetMaxRow - 1) ? (dtRowCnt + 1) : SheetMaxRow;
                Microsoft.Office.Interop.Excel.Range allRange = sht.Range[sht.Cells[1, begCol], sht.Cells[endRowNo, endCol]];
                sht.ListObjects.AddEx(XlListObjectSourceType.xlSrcRange, allRange, Type.Missing, XlYesNoGuess.xlYes, Type.Missing,Type.Missing).Name = "LSTOBJ";
                sht.ListObjects["LSTOBJ"].TableStyle = "";
                if ((endRowNo + 1) <= SheetMaxRow)
                    sht.Cells[endRowNo + 1, 1].Select();
                else
                    sht.Cells[endRowNo, 1].Select();
                
                //[SQL查询] Ribbon重获焦点
                Globals.Ribbons.SqlRibbon.RibbonUI.ActivateTab(Globals.Ribbons.SqlRibbon.TabSqlBrowser.Name);
                //让垂直滚动条回到顶端
                Globals.ThisAddIn.Application.ActiveWindow.ScrollRow = 1;

                //sht.Application.ScreenUpdating = true;

                enddt = System.DateTime.Now;
                endts = new TimeSpan(enddt.Ticks);
                ts = begts.Subtract(endts).Duration();
                string strTimes2 = ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') + "." + ts.Milliseconds.ToString();
                lblUseTime.ToolTipText = lblUseTime.Text;
                lblUseTime.Text = strTimes2;

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
                progressBarGetData.Value = 0;
                rtbMsg.Text = ex.ToString();
                rtbMsg.ForeColor = Color.Red;
                tabPageMsg.Show();
                tabCtrlSQLEdit.SelectedIndex = 1;//tabPageMsg
            }

        }
        #endregion

        /// <summary>
        /// [清空Excel]按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXlsDataClear_Click(object sender, EventArgs e)
        {
            string strExeMsg="";
            // Define the active Worksheet
            var sht = Globals.ThisAddIn.Application.ActiveSheet as Worksheet;
            if (sht != null)
            {
                sht.Cells.EntireRow.Delete(Type.Missing);
                strExeMsg = ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_XLS_DATA_CLEARED", "Current worksheet data has been cleared.");
                //((Microsoft.Office.Interop.Excel.Range)sht.Cells[1, 1]).Activate();
                ((Microsoft.Office.Interop.Excel.Range)sht.Cells[1, 1]).Select();
                sht.Cells.Interior.Pattern = XlPattern.xlPatternNone;           //取消单元格背景样式
                sht.Cells.Interior.TintAndShade = 0;
                sht.Cells.Interior.PatternTintAndShade = 0;
                //Globals.ThisAddIn.Application.ActiveWindow.SplitColumn = 0;
                //Globals.ThisAddIn.Application.ActiveWindow.SplitRow = 0;
                //Globals.ThisAddIn.Application.ActiveWindow.FreezePanes = false; //取消冻结窗格
            }
            else
            {
                strExeMsg = ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_NO_OPEN_WORKSHEET", "No opened worksheet.");
            }
            progressBarGetData.Value = 0;
            rtbMsg.Text = string.Empty;
            rtbMsg.ForeColor = Color.Black;
            lblExeMsg.Text = strExeMsg;
            lblUseTime.Text = "00:00:00";
            //lblRtnCnt.Text = "0 Rows(s)";
            lblRtnCnt.Text = "0 " + ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_ROW","Rows(s)");
        }

        /// <summary>
        /// [工作表切换]按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSheetSwitch_Click(object sender, EventArgs e)
        {
            string strTag = ((System.Windows.Forms.Button)sender).Tag.ToString();
            var wb = Globals.ThisAddIn.Application.ActiveWorkbook as Workbook;
            var sht = Globals.ThisAddIn.Application.ActiveSheet as Worksheet;
            int goto_idx = 0;

            if (wb == null)
            {
                string strExeMsg = ComFunction.GetResxString(typeof(TpSQLEditor), "TXT_NO_OPEN_WORKBOOK", "No opened workbook.");
                lblExeMsg.Text = strExeMsg;
                return;
            }

            //第一个工作表
            if (strTag.ToUpper().Trim() == "FIRST")
            {
                goto_idx = 1;
            }
            //上一个工作表
            if (strTag.ToUpper().Trim() == "PREVIOUS")
            {
                int pre_idx = sht.Index - 1;
                //if (pre_idx <= 0) return;
                goto_idx = pre_idx;
                if (pre_idx <= 0) goto_idx = wb.Worksheets.Count;
            }
            //后一个工作表
            if (strTag.ToUpper().Trim() == "NEXT")
            {
                int next_idx = sht.Index + 1;
                //if (next_idx > wb.Worksheets.Count) return;
                goto_idx = next_idx;
                if (next_idx > wb.Worksheets.Count) goto_idx = 1;
            }
            //最末工作表
            if (strTag.ToUpper().Trim() == "LAST")
            {
                goto_idx = wb.Worksheets.Count;

            }
            //工作表切换
            ((Microsoft.Office.Interop.Excel._Worksheet)wb.Worksheets[goto_idx]).Activate();
        }

        /// <summary>
        /// 定义快捷键，F5：查询、F9：清空Excel工作表内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fctbSQLEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnGetData.PerformClick();//[Get Data] shortcut
            }

            if (e.KeyCode == Keys.F9)
            {
                btnXlsDataClear.PerformClick();//[Excel Clear] shortcut
            }

            //2015.11.27 add
            if (e.KeyCode == Keys.F2)
            {
                FmSQLFavorites f = new FmSQLFavorites();
                f.ShowDialog();
            }

        }

        private void TpSQLEditor_KeyDown(object sender, KeyEventArgs e)
        {
            //
        }

        //2015.08.26 test
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
            popupMenu.Items.SetAutocompleteItems(items);
        }

        ///// <summary>
        ///// This item appears when any part of snippet text is typed
        ///// </summary>
        //class FunctionSnippet : SnippetAutocompleteItem
        //{
        //    public FunctionSnippet(string snippet)
        //        : base(snippet)
        //    {
        //    }

        //    public override CompareResult Compare(string fragmentText)
        //    {
        //        var pattern = Regex.Escape(fragmentText);
        //        if (Regex.IsMatch(Text, "\\b" + pattern, RegexOptions.IgnoreCase))
        //            return CompareResult.Visible;
        //        return CompareResult.Hidden;
        //    }
        //}

        ///// <summary>
        ///// Divides numbers and words: "123AND456" -> "123 AND 456"
        ///// Or "i=2" -> "i = 2"
        ///// </summary>
        //class InsertSpaceSnippet : AutocompleteItem
        //{
        //    string pattern;

        //    public InsertSpaceSnippet(string pattern)
        //        : base("")
        //    {
        //        this.pattern = pattern;
        //    }

        //    public InsertSpaceSnippet()
        //        : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
        //    {
        //    }

        //    public override CompareResult Compare(string fragmentText)
        //    {
        //        if (Regex.IsMatch(fragmentText, pattern))
        //        {
        //            Text = InsertSpaces(fragmentText);
        //            if (Text != fragmentText)
        //                return CompareResult.Visible;
        //        }
        //        return CompareResult.Hidden;
        //    }

        //    public string InsertSpaces(string fragment)
        //    {
        //        var m = Regex.Match(fragment, pattern);
        //        if (m == null)
        //            return fragment;
        //        if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
        //            return fragment;
        //        return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
        //    }

        //    public override string ToolTipTitle
        //    {
        //        get
        //        {
        //            return Text;
        //        }
        //    }
        //}



    }
}
