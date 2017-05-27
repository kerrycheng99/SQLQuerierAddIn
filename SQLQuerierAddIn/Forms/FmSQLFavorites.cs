using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SQLQuerierAddIn.Common;

namespace SQLQuerierAddIn.Forms
{
    public partial class FmSQLFavorites : Form
    {
        private string _MsgText = string.Empty;

        public FmSQLFavorites()
        {
            InitializeComponent();

        }

        private void FmSQLFavorites_Load(object sender, EventArgs e)
        {
            FavView dockView = new FavView();
            dockView.Show(this.dpFavorites, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            dockView.CloseButtonVisible = false;
            dockView.CloseButton = false;
            dockView.Name = "SQLFavorites";
            //dockView.Text = "SQL收藏夹";
            dockView.Text = ComFunction.GetResxString(typeof(FmSQLFavorites), "TXT_SQL_FAV", "SQL Favorites");
            dockView.ToolTipText = dockView.Text;

            FavSQLBase dockSQL = new FavSQLBase();
            dockSQL.Show(this.dpFavorites);
            dockSQL.CloseButtonVisible = false;
            dockSQL.CloseButton = false;
            dockSQL.AllowEndUserDocking = false;// 禁止拖移
            dockSQL.Name = "FavSQLBase";
            //dockSQL.Text = "SQL编辑器";
            dockSQL.Text = ComFunction.GetResxString(typeof(FmSQLFavorites), "TXT_SQL_EDITOR", "SQL Editor");
            dockSQL.ToolTipText = dockSQL.Text;
        }

        private void FmSQLFavorites_FormClosing(object sender, FormClosingEventArgs e)
        {
            FavSQLBase sqleditorfrm = (FavSQLBase)Application.OpenForms["FavSQLBase"];

            if (sqleditorfrm != null)
            {
                //文件修改后尚未保存
                if (sqleditorfrm.FileNotSave)
                {
                    //_MsgText = "文件 \"{0}\" 尚未保存，是否需要保存？";
                    _MsgText = ComFunction.GetResxString(typeof(FavSQLBase), "TXT_FILE_NOT_SAVE", "File \"{0}\" has not been saved, save it?");
                    _MsgText = string.Format(_MsgText, sqleditorfrm.FileName);

                    DialogResult diaRes = MessageBox.Show(_MsgText, this.Text, MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);
                    if (diaRes == DialogResult.Cancel) e.Cancel = true;
                    if (diaRes == DialogResult.Yes)
                    {
                        sqleditorfrm.tecSQLEdit.SaveFile(sqleditorfrm.FilePath);
                        sqleditorfrm.FileNotSave = false;
                    }
                }
            }
        }


    }
}
