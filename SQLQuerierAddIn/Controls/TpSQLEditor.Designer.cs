namespace SQLQuerierAddIn.Controls
{
    partial class TpSQLEditor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TpSQLEditor));
            this.tabCtrlSQLEdit = new System.Windows.Forms.TabControl();
            this.tabPageSQLEdit = new System.Windows.Forms.TabPage();
            this.fctbSQLEdit = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tabPageMsg = new System.Windows.Forms.TabPage();
            this.rtbMsg = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblExeMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSvrDbVerFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSvrDbVer = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUserPidFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUserPid = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDbNameFilter = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDbName = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFilter4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUseTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFilter5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRtnCnt = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFilter6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBarGetData = new System.Windows.Forms.ToolStripProgressBar();
            this.btnXlsDataClear = new System.Windows.Forms.Button();
            this.btnGetData = new System.Windows.Forms.Button();
            this.btnFirstSheet = new System.Windows.Forms.Button();
            this.btnPreSheet = new System.Windows.Forms.Button();
            this.btnNextSheet = new System.Windows.Forms.Button();
            this.btnLastSheet = new System.Windows.Forms.Button();
            this.tabCtrlSQLEdit.SuspendLayout();
            this.tabPageSQLEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbSQLEdit)).BeginInit();
            this.tabPageMsg.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCtrlSQLEdit
            // 
            resources.ApplyResources(this.tabCtrlSQLEdit, "tabCtrlSQLEdit");
            this.tabCtrlSQLEdit.Controls.Add(this.tabPageSQLEdit);
            this.tabCtrlSQLEdit.Controls.Add(this.tabPageMsg);
            this.tabCtrlSQLEdit.Name = "tabCtrlSQLEdit";
            this.tabCtrlSQLEdit.SelectedIndex = 0;
            // 
            // tabPageSQLEdit
            // 
            this.tabPageSQLEdit.Controls.Add(this.fctbSQLEdit);
            resources.ApplyResources(this.tabPageSQLEdit, "tabPageSQLEdit");
            this.tabPageSQLEdit.Name = "tabPageSQLEdit";
            this.tabPageSQLEdit.UseVisualStyleBackColor = true;
            // 
            // fctbSQLEdit
            // 
            this.fctbSQLEdit.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctbSQLEdit.AutoIndentCharsPatterns = "";
            resources.ApplyResources(this.fctbSQLEdit, "fctbSQLEdit");
            this.fctbSQLEdit.BackBrush = null;
            this.fctbSQLEdit.CharHeight = 15;
            this.fctbSQLEdit.CharWidth = 7;
            this.fctbSQLEdit.CommentPrefix = "--";
            this.fctbSQLEdit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctbSQLEdit.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctbSQLEdit.IsReplaceMode = false;
            this.fctbSQLEdit.Language = FastColoredTextBoxNS.Language.SQL;
            this.fctbSQLEdit.LeftBracket = '(';
            this.fctbSQLEdit.Name = "fctbSQLEdit";
            this.fctbSQLEdit.Paddings = new System.Windows.Forms.Padding(0);
            this.fctbSQLEdit.RightBracket = ')';
            this.fctbSQLEdit.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctbSQLEdit.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbSQLEdit.ServiceColors")));
            this.fctbSQLEdit.Zoom = 100;
            this.fctbSQLEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fctbSQLEdit_KeyDown);
            // 
            // tabPageMsg
            // 
            this.tabPageMsg.Controls.Add(this.rtbMsg);
            resources.ApplyResources(this.tabPageMsg, "tabPageMsg");
            this.tabPageMsg.Name = "tabPageMsg";
            this.tabPageMsg.UseVisualStyleBackColor = true;
            // 
            // rtbMsg
            // 
            this.rtbMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.rtbMsg, "rtbMsg");
            this.rtbMsg.Name = "rtbMsg";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.btnXlsDataClear);
            this.panel1.Controls.Add(this.btnGetData);
            this.panel1.Controls.Add(this.btnFirstSheet);
            this.panel1.Controls.Add(this.btnPreSheet);
            this.panel1.Controls.Add(this.btnNextSheet);
            this.panel1.Controls.Add(this.btnLastSheet);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblExeMsg,
            this.lblSpring,
            this.lblSvrDbVerFilter,
            this.lblSvrDbVer,
            this.lblUserPidFilter,
            this.lblUserPid,
            this.lblDbNameFilter,
            this.lblDbName,
            this.lblFilter4,
            this.lblUseTime,
            this.lblFilter5,
            this.lblRtnCnt,
            this.lblFilter6,
            this.progressBarGetData});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // lblExeMsg
            // 
            this.lblExeMsg.Image = global::SQLQuerierAddIn.Properties.Resources.netstatus_off;
            this.lblExeMsg.Name = "lblExeMsg";
            resources.ApplyResources(this.lblExeMsg, "lblExeMsg");
            // 
            // lblSpring
            // 
            this.lblSpring.Name = "lblSpring";
            resources.ApplyResources(this.lblSpring, "lblSpring");
            this.lblSpring.Spring = true;
            // 
            // lblSvrDbVerFilter
            // 
            this.lblSvrDbVerFilter.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblSvrDbVerFilter.Name = "lblSvrDbVerFilter";
            resources.ApplyResources(this.lblSvrDbVerFilter, "lblSvrDbVerFilter");
            // 
            // lblSvrDbVer
            // 
            this.lblSvrDbVer.Name = "lblSvrDbVer";
            resources.ApplyResources(this.lblSvrDbVer, "lblSvrDbVer");
            // 
            // lblUserPidFilter
            // 
            this.lblUserPidFilter.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblUserPidFilter.Name = "lblUserPidFilter";
            resources.ApplyResources(this.lblUserPidFilter, "lblUserPidFilter");
            // 
            // lblUserPid
            // 
            this.lblUserPid.Name = "lblUserPid";
            resources.ApplyResources(this.lblUserPid, "lblUserPid");
            // 
            // lblDbNameFilter
            // 
            this.lblDbNameFilter.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblDbNameFilter.Name = "lblDbNameFilter";
            resources.ApplyResources(this.lblDbNameFilter, "lblDbNameFilter");
            // 
            // lblDbName
            // 
            this.lblDbName.Name = "lblDbName";
            resources.ApplyResources(this.lblDbName, "lblDbName");
            // 
            // lblFilter4
            // 
            this.lblFilter4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblFilter4.Name = "lblFilter4";
            resources.ApplyResources(this.lblFilter4, "lblFilter4");
            // 
            // lblUseTime
            // 
            this.lblUseTime.Name = "lblUseTime";
            resources.ApplyResources(this.lblUseTime, "lblUseTime");
            // 
            // lblFilter5
            // 
            this.lblFilter5.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblFilter5.Name = "lblFilter5";
            resources.ApplyResources(this.lblFilter5, "lblFilter5");
            // 
            // lblRtnCnt
            // 
            this.lblRtnCnt.Name = "lblRtnCnt";
            resources.ApplyResources(this.lblRtnCnt, "lblRtnCnt");
            // 
            // lblFilter6
            // 
            this.lblFilter6.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblFilter6.Name = "lblFilter6";
            resources.ApplyResources(this.lblFilter6, "lblFilter6");
            // 
            // progressBarGetData
            // 
            this.progressBarGetData.Name = "progressBarGetData";
            resources.ApplyResources(this.progressBarGetData, "progressBarGetData");
            // 
            // btnXlsDataClear
            // 
            resources.ApplyResources(this.btnXlsDataClear, "btnXlsDataClear");
            this.btnXlsDataClear.Image = global::SQLQuerierAddIn.Properties.Resources.button_xls_clear;
            this.btnXlsDataClear.Name = "btnXlsDataClear";
            this.btnXlsDataClear.UseVisualStyleBackColor = true;
            this.btnXlsDataClear.Click += new System.EventHandler(this.btnXlsDataClear_Click);
            // 
            // btnGetData
            // 
            resources.ApplyResources(this.btnGetData, "btnGetData");
            this.btnGetData.Image = global::SQLQuerierAddIn.Properties.Resources.button_ok;
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // btnFirstSheet
            // 
            resources.ApplyResources(this.btnFirstSheet, "btnFirstSheet");
            this.btnFirstSheet.Image = global::SQLQuerierAddIn.Properties.Resources.GotoFirst;
            this.btnFirstSheet.Name = "btnFirstSheet";
            this.btnFirstSheet.Tag = "FIRST";
            this.btnFirstSheet.UseVisualStyleBackColor = true;
            this.btnFirstSheet.Click += new System.EventHandler(this.btnSheetSwitch_Click);
            // 
            // btnPreSheet
            // 
            resources.ApplyResources(this.btnPreSheet, "btnPreSheet");
            this.btnPreSheet.Image = global::SQLQuerierAddIn.Properties.Resources.GotoPre;
            this.btnPreSheet.Name = "btnPreSheet";
            this.btnPreSheet.Tag = "PREVIOUS";
            this.btnPreSheet.UseVisualStyleBackColor = true;
            this.btnPreSheet.Click += new System.EventHandler(this.btnSheetSwitch_Click);
            // 
            // btnNextSheet
            // 
            resources.ApplyResources(this.btnNextSheet, "btnNextSheet");
            this.btnNextSheet.Image = global::SQLQuerierAddIn.Properties.Resources.GotoNext;
            this.btnNextSheet.Name = "btnNextSheet";
            this.btnNextSheet.Tag = "NEXT";
            this.btnNextSheet.UseVisualStyleBackColor = true;
            this.btnNextSheet.Click += new System.EventHandler(this.btnSheetSwitch_Click);
            // 
            // btnLastSheet
            // 
            resources.ApplyResources(this.btnLastSheet, "btnLastSheet");
            this.btnLastSheet.Image = global::SQLQuerierAddIn.Properties.Resources.GotoLast;
            this.btnLastSheet.Name = "btnLastSheet";
            this.btnLastSheet.Tag = "LAST";
            this.btnLastSheet.UseVisualStyleBackColor = true;
            this.btnLastSheet.Click += new System.EventHandler(this.btnSheetSwitch_Click);
            // 
            // TpSQLEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabCtrlSQLEdit);
            this.Name = "TpSQLEditor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TpSQLEditor_KeyDown);
            this.tabCtrlSQLEdit.ResumeLayout(false);
            this.tabPageSQLEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fctbSQLEdit)).EndInit();
            this.tabPageMsg.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCtrlSQLEdit;
        private System.Windows.Forms.TabPage tabPageSQLEdit;
        private System.Windows.Forms.TabPage tabPageMsg;
        private System.Windows.Forms.RichTextBox rtbMsg;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnXlsDataClear;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblFilter4;
        private System.Windows.Forms.ToolStripStatusLabel lblUseTime;
        private System.Windows.Forms.ToolStripStatusLabel lblFilter5;
        private System.Windows.Forms.ToolStripStatusLabel lblRtnCnt;
        private System.Windows.Forms.ToolStripStatusLabel lblFilter6;
        private System.Windows.Forms.ToolStripProgressBar progressBarGetData;
        private System.Windows.Forms.ToolStripStatusLabel lblSpring;
        public System.Windows.Forms.ToolStripStatusLabel lblExeMsg;
        public System.Windows.Forms.ToolStripStatusLabel lblSvrDbVerFilter;
        public System.Windows.Forms.ToolStripStatusLabel lblSvrDbVer;
        public System.Windows.Forms.ToolStripStatusLabel lblUserPid;
        public System.Windows.Forms.ToolStripStatusLabel lblUserPidFilter;
        public System.Windows.Forms.ToolStripStatusLabel lblDbNameFilter;
        public System.Windows.Forms.ToolStripStatusLabel lblDbName;
        private System.Windows.Forms.Button btnFirstSheet;
        private System.Windows.Forms.Button btnPreSheet;
        private System.Windows.Forms.Button btnNextSheet;
        private System.Windows.Forms.Button btnLastSheet;
        public FastColoredTextBoxNS.FastColoredTextBox fctbSQLEdit;
    }
}
