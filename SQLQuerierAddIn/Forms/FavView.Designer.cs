namespace SQLQuerierAddIn.Forms
{
    partial class FavView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavView));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSplitLine1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRefrush = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSrch = new System.Windows.Forms.TextBox();
            this.tvFav = new System.Windows.Forms.TreeView();
            this.fbdRootFloder = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_RootSet = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_NewFolder = new System.Windows.Forms.ToolStripButton();
            this.btn_NewFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btn_Refrush = new System.Windows.Forms.ToolStripButton();
            this.btn_Delete = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "fav.gif");
            this.imageList1.Images.SetKeyName(1, "Folderclose.gif");
            this.imageList1.Images.SetKeyName(2, "Folderopen.gif");
            this.imageList1.Images.SetKeyName(3, "Document.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpen,
            this.tsmiNew,
            this.tsmiSplitLine1,
            this.tsmiRefrush,
            this.tsmiDelete,
            this.tsmiRename});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.Name = "tsmiOpen";
            resources.ApplyResources(this.tsmiOpen, "tsmiOpen");
            this.tsmiOpen.Click += new System.EventHandler(this.tsmiOpen_Click);
            // 
            // tsmiNew
            // 
            this.tsmiNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewFolder,
            this.tsmiNewFile});
            this.tsmiNew.Name = "tsmiNew";
            resources.ApplyResources(this.tsmiNew, "tsmiNew");
            // 
            // tsmiNewFolder
            // 
            this.tsmiNewFolder.Image = global::SQLQuerierAddIn.Properties.Resources.btn_NewFolder;
            this.tsmiNewFolder.Name = "tsmiNewFolder";
            resources.ApplyResources(this.tsmiNewFolder, "tsmiNewFolder");
            this.tsmiNewFolder.Click += new System.EventHandler(this.tsmiNewFolder_Click);
            // 
            // tsmiNewFile
            // 
            this.tsmiNewFile.Image = global::SQLQuerierAddIn.Properties.Resources.Document;
            this.tsmiNewFile.Name = "tsmiNewFile";
            resources.ApplyResources(this.tsmiNewFile, "tsmiNewFile");
            this.tsmiNewFile.Click += new System.EventHandler(this.tsmiNewFile_Click);
            // 
            // tsmiSplitLine1
            // 
            this.tsmiSplitLine1.Name = "tsmiSplitLine1";
            resources.ApplyResources(this.tsmiSplitLine1, "tsmiSplitLine1");
            // 
            // tsmiRefrush
            // 
            this.tsmiRefrush.Image = global::SQLQuerierAddIn.Properties.Resources.refrush;
            this.tsmiRefrush.Name = "tsmiRefrush";
            resources.ApplyResources(this.tsmiRefrush, "tsmiRefrush");
            this.tsmiRefrush.Click += new System.EventHandler(this.tsmiRefrush_Click);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Image = global::SQLQuerierAddIn.Properties.Resources.delete;
            this.tsmiDelete.Name = "tsmiDelete";
            resources.ApplyResources(this.tsmiDelete, "tsmiDelete");
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // tsmiRename
            // 
            this.tsmiRename.Name = "tsmiRename";
            resources.ApplyResources(this.tsmiRename, "tsmiRename");
            this.tsmiRename.Click += new System.EventHandler(this.tmsiRename_Click);
            // 
            // txtSrch
            // 
            resources.ApplyResources(this.txtSrch, "txtSrch");
            this.txtSrch.ForeColor = System.Drawing.Color.Gray;
            this.txtSrch.Name = "txtSrch";
            this.txtSrch.Enter += new System.EventHandler(this.txtSrch_Enter);
            this.txtSrch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSrch_KeyDown);
            // 
            // tvFav
            // 
            this.tvFav.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.tvFav, "tvFav");
            this.tvFav.ImageList = this.imageList1;
            this.tvFav.Name = "tvFav";
            this.tvFav.ShowRootLines = false;
            this.tvFav.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvFav_AfterLabelEdit);
            this.tvFav.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFav_BeforeSelect);
            this.tvFav.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFav_AfterSelect);
            this.tvFav.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvFav_NodeMouseClick);
            this.tvFav.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvFav_KeyDown);
            this.tvFav.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvFav_MouseUp);
            // 
            // fbdRootFloder
            // 
            resources.ApplyResources(this.fbdRootFloder, "fbdRootFloder");
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackgroundImage = global::SQLQuerierAddIn.Properties.Resources.toolbar;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_RootSet,
            this.toolStripSeparator1,
            this.btn_NewFolder,
            this.btn_NewFile,
            this.toolStripSeparator2,
            this.btn_Refrush,
            this.btn_Delete});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btn_RootSet
            // 
            this.btn_RootSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_RootSet.Image = global::SQLQuerierAddIn.Properties.Resources.fav;
            resources.ApplyResources(this.btn_RootSet, "btn_RootSet");
            this.btn_RootSet.Name = "btn_RootSet";
            this.btn_RootSet.Click += new System.EventHandler(this.btn_RootSet_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btn_NewFolder
            // 
            this.btn_NewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_NewFolder.Image = global::SQLQuerierAddIn.Properties.Resources.btn_NewFolder;
            resources.ApplyResources(this.btn_NewFolder, "btn_NewFolder");
            this.btn_NewFolder.Name = "btn_NewFolder";
            this.btn_NewFolder.Click += new System.EventHandler(this.btn_NewFolder_Click);
            // 
            // btn_NewFile
            // 
            this.btn_NewFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_NewFile.Image = global::SQLQuerierAddIn.Properties.Resources.EditDocument;
            resources.ApplyResources(this.btn_NewFile, "btn_NewFile");
            this.btn_NewFile.Name = "btn_NewFile";
            this.btn_NewFile.Click += new System.EventHandler(this.btn_NewFile_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btn_Refrush
            // 
            this.btn_Refrush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Refrush.Image = global::SQLQuerierAddIn.Properties.Resources.refrush;
            resources.ApplyResources(this.btn_Refrush, "btn_Refrush");
            this.btn_Refrush.Name = "btn_Refrush";
            this.btn_Refrush.Click += new System.EventHandler(this.btn_Refrush_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Delete.Image = global::SQLQuerierAddIn.Properties.Resources.delete;
            resources.ApplyResources(this.btn_Delete, "btn_Delete");
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // FavView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvFav);
            this.Controls.Add(this.txtSrch);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FavView";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_NewFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btn_NewFile;
        private System.Windows.Forms.ToolStripButton btn_Refrush;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripSeparator tsmiSplitLine1;
        private System.Windows.Forms.ToolStripMenuItem tsmiNew;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefrush;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiRename;
        private System.Windows.Forms.TextBox txtSrch;
        private System.Windows.Forms.TreeView tvFav;
        private System.Windows.Forms.ToolStripButton btn_RootSet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.FolderBrowserDialog fbdRootFloder;
        private System.Windows.Forms.ToolStripButton btn_Delete;

    }
}