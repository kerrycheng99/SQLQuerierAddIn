using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SQLQuerierAddIn.Controls;
using SQLQuerierAddIn.Common;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Actions;

namespace SQLQuerierAddIn.Forms
{
    public partial class FavSQLBase : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private string _MsgText = string.Empty;

        private string filepath = "";
        private string filename = "";
        private bool filenotsave = false;
        private FavNodeType nodetype = FavNodeType.RootNode;
        private bool isopenfile = false;

        public string FilePath
        {
            set { filepath = value; }
            get { return filepath; }
        }
        public string FileName
        {
            set { filename = value; }
            get { return filename; }
        }
        public bool FileNotSave
        {
            set { filenotsave = value; }
            get { return filenotsave; }
        }
        public FavNodeType NodeType
        {
            set { nodetype = value; }
            get { return nodetype; }
        }

        public FavSQLBase()
        {
            InitializeComponent();

            tecSQLEdit.Encoding = System.Text.Encoding.Default;
            tecSQLEdit.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TSQL");
            tecSQLEdit.Document.TextEditorProperties.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tecSQLEdit.VRulerRow = 0;
            tecSQLEdit.LineViewerStyle = LineViewerStyle.FullRow;
            tecSQLEdit.ShowLineNumbers = true;
            tecSQLEdit.ShowHRuler = true;
            tecSQLEdit.ShowSpaces = false;
            tecSQLEdit.ShowTabs = false;

            btnSave.Enabled = false;
            btnRun.Enabled = false;
        }

        #region 打开指定SQL文件
        public void DisplaySQLFile(string fileName)
        {
            //StreamReader srFile = new StreamReader(fileName, Encoding.Default);
            //string strContents = srFile.ReadToEnd();
            //srFile.Close();
            //tecSQLEdit.Text = string.Empty;
            //tecSQLEdit.Refresh();
            //tecSQLEdit.Text = strContents;
            isopenfile = true;
            tecSQLEdit.LoadFile(fileName);
            isopenfile = false;

            btnSave.Enabled = false;

        }
        #endregion

        public void EmptyContents()
        {
            isopenfile = true;
            tecSQLEdit.Text = string.Empty;
            tecSQLEdit.Refresh();
            isopenfile = false;
            btnSave.Enabled = false;
            btnRun.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (File.Exists(filepath))
            //{
            //    FileStream fs = new FileStream(filepath, FileMode.Create);
            //    StreamWriter sw = new StreamWriter(fs);
            //    try
            //    {
            //        sw.Write(tecSQLEdit.Text);
            //        sw.Flush();
            //        this.Text = filename;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message.ToString());
            //    }
            //    finally
            //    {
            //        sw.Close();
            //        fs.Close();
            //    }
            //}
            tecSQLEdit.SaveFile(filepath);
            this.Text = filename;
            btnSave.Enabled = false;
            filenotsave = false;
            isopenfile = false;
        }

        private void tecSQLEdit_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(filepath) && nodetype == FavNodeType.FileNode)
            {
                if (!isopenfile)
                {
                    this.Text = filename + "*";
                    filenotsave = true;
                    btnSave.Enabled = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(tecSQLEdit.Text.Trim()))
                btnRun.Enabled = true;
            else
                btnRun.Enabled = false;
            
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (filenotsave)
            {
                //string msg = "文件 \"{0}\" 尚未保存，是否需要保存？";
                _MsgText = ComFunction.GetResxString(typeof(FavSQLBase), "TXT_FILE_NOT_SAVE", "File \"{0}\" has not been saved, save it?");
                _MsgText = string.Format(_MsgText, filename);
                DialogResult diaRes = MessageBox.Show(_MsgText, this.filename, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (diaRes == DialogResult.Cancel) return;
                if (diaRes == DialogResult.Yes)
                {
                    tecSQLEdit.SaveFile(filepath);
                    this.Text = filename;
                }
            }

            //other
            FmSQLFavorites fm = (FmSQLFavorites)Application.OpenForms["FmSQLFavorites"];
            if (fm != null)
            {
                fm.Dispose();
                fm.Close();
            }
            Globals.ThisAddIn.tpSqlEditorCTP.Visible = true;
            Globals.ThisAddIn.SqlEditor.fctbSQLEdit.Text = tecSQLEdit.Text;
            Globals.ThisAddIn.SqlEditor.PopulateFromSql();
        }

        private void FavSQLBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnRun.PerformClick();//[Run] shortcut
            }

            if (e.KeyCode == Keys.F6)
            {
                btnSave.PerformClick();//[Save] shortcut
            }
        }

    }
}
