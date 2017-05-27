namespace SQLQuerierAddIn
{
    partial class SqlRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public SqlRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlRibbon));
            this.TabSqlBrowser = this.Factory.CreateRibbonTab();
            this.grpConnection = this.Factory.CreateRibbonGroup();
            this.btnConnection = this.Factory.CreateRibbonButton();
            this.btnDisconnect = this.Factory.CreateRibbonButton();
            this.grpSQLEditPane = this.Factory.CreateRibbonGroup();
            this.btnSQLEditor = this.Factory.CreateRibbonToggleButton();
            this.mnuFavorites = this.Factory.CreateRibbonMenu();
            this.mnuOption = this.Factory.CreateRibbonMenu();
            this.btnRefresh = this.Factory.CreateRibbonButton();
            this.btnSetting = this.Factory.CreateRibbonButton();
            this.grpAboutPane = this.Factory.CreateRibbonGroup();
            this.btnAbout = this.Factory.CreateRibbonButton();
            this.TabSqlBrowser.SuspendLayout();
            this.grpConnection.SuspendLayout();
            this.grpSQLEditPane.SuspendLayout();
            this.grpAboutPane.SuspendLayout();
            // 
            // TabSqlBrowser
            // 
            this.TabSqlBrowser.Groups.Add(this.grpConnection);
            this.TabSqlBrowser.Groups.Add(this.grpSQLEditPane);
            this.TabSqlBrowser.Groups.Add(this.grpAboutPane);
            resources.ApplyResources(this.TabSqlBrowser, "TabSqlBrowser");
            this.TabSqlBrowser.Name = "TabSqlBrowser";
            // 
            // grpConnection
            // 
            this.grpConnection.Items.Add(this.btnConnection);
            this.grpConnection.Items.Add(this.btnDisconnect);
            resources.ApplyResources(this.grpConnection, "grpConnection");
            this.grpConnection.Name = "grpConnection";
            // 
            // btnConnection
            // 
            resources.ApplyResources(this.btnConnection, "btnConnection");
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.OfficeImageId = "DatabaseSqlServer";
            this.btnConnection.ShowImage = true;
            this.btnConnection.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnConnection_Click);
            // 
            // btnDisconnect
            // 
            resources.ApplyResources(this.btnDisconnect, "btnDisconnect");
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.OfficeImageId = "TableUnlinkExternalData";
            this.btnDisconnect.ShowImage = true;
            this.btnDisconnect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDisconnect_Click);
            // 
            // grpSQLEditPane
            // 
            this.grpSQLEditPane.Items.Add(this.btnSQLEditor);
            this.grpSQLEditPane.Items.Add(this.mnuFavorites);
            resources.ApplyResources(this.grpSQLEditPane, "grpSQLEditPane");
            this.grpSQLEditPane.Name = "grpSQLEditPane";
            // 
            // btnSQLEditor
            // 
            this.btnSQLEditor.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            resources.ApplyResources(this.btnSQLEditor, "btnSQLEditor");
            this.btnSQLEditor.Name = "btnSQLEditor";
            this.btnSQLEditor.OfficeImageId = "AdpStoredProcedureEditSql";
            this.btnSQLEditor.ShowImage = true;
            this.btnSQLEditor.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSQLEditor_Click);
            // 
            // mnuFavorites
            // 
            this.mnuFavorites.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.mnuFavorites.Dynamic = true;
            this.mnuFavorites.Items.Add(this.mnuOption);
            resources.ApplyResources(this.mnuFavorites, "mnuFavorites");
            this.mnuFavorites.Name = "mnuFavorites";
            this.mnuFavorites.OfficeImageId = "FileOpen";
            this.mnuFavorites.ShowImage = true;
            // 
            // mnuOption
            // 
            this.mnuOption.Items.Add(this.btnRefresh);
            this.mnuOption.Items.Add(this.btnSetting);
            resources.ApplyResources(this.mnuOption, "mnuOption");
            this.mnuOption.Name = "mnuOption";
            this.mnuOption.OfficeImageId = "ControlToolboxOutlook";
            this.mnuOption.ShowImage = true;
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.OfficeImageId = "RecurrenceEdit";
            this.btnRefresh.ShowImage = true;
            this.btnRefresh.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnRefresh_Click);
            // 
            // btnSetting
            // 
            resources.ApplyResources(this.btnSetting, "btnSetting");
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.OfficeImageId = "FileCompactAndRepairDatabase";
            this.btnSetting.ShowImage = true;
            this.btnSetting.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSetting_Click);
            // 
            // grpAboutPane
            // 
            this.grpAboutPane.Items.Add(this.btnAbout);
            resources.ApplyResources(this.grpAboutPane, "grpAboutPane");
            this.grpAboutPane.Name = "grpAboutPane";
            // 
            // btnAbout
            // 
            this.btnAbout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.OfficeImageId = "DocumentPanelTemplate";
            this.btnAbout.ShowImage = true;
            this.btnAbout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAbout_Click);
            // 
            // SqlRibbon
            // 
            this.Name = "SqlRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.TabSqlBrowser);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.SqlRibbon_Load);
            this.TabSqlBrowser.ResumeLayout(false);
            this.TabSqlBrowser.PerformLayout();
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.grpSQLEditPane.ResumeLayout(false);
            this.grpSQLEditPane.PerformLayout();
            this.grpAboutPane.ResumeLayout(false);
            this.grpAboutPane.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab TabSqlBrowser;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpSQLEditPane;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton btnSQLEditor;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpConnection;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnConnection;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDisconnect;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpAboutPane;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu mnuOption;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnRefresh;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSetting;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu mnuFavorites;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal SqlRibbon SqlRibbon
        {
            get { return this.GetRibbon<SqlRibbon>(); }
        }
    }
}
