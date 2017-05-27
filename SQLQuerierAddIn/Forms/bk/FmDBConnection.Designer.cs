namespace SQLQuerierAddIn.Forms
{
    partial class FmDBConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmDBConnection));
            this.lblServerName = new System.Windows.Forms.Label();
            this.lblAuth = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.chkSavePwd = new System.Windows.Forms.CheckBox();
            this.lblDataName = new System.Windows.Forms.Label();
            this.cmbServerName = new System.Windows.Forms.ComboBox();
            this.chkSaveConInfo = new System.Windows.Forms.CheckBox();
            this.cmbAuth = new System.Windows.Forms.ComboBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.cmbDataName = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblServerName
            // 
            resources.ApplyResources(this.lblServerName, "lblServerName");
            this.lblServerName.Name = "lblServerName";
            // 
            // lblAuth
            // 
            resources.ApplyResources(this.lblAuth, "lblAuth");
            this.lblAuth.Name = "lblAuth";
            // 
            // lblUserName
            // 
            resources.ApplyResources(this.lblUserName, "lblUserName");
            this.lblUserName.Name = "lblUserName";
            // 
            // lblPwd
            // 
            resources.ApplyResources(this.lblPwd, "lblPwd");
            this.lblPwd.Name = "lblPwd";
            // 
            // chkSavePwd
            // 
            resources.ApplyResources(this.chkSavePwd, "chkSavePwd");
            this.chkSavePwd.Name = "chkSavePwd";
            this.chkSavePwd.UseVisualStyleBackColor = true;
            this.chkSavePwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkSavePwd_KeyDown);
            // 
            // lblDataName
            // 
            resources.ApplyResources(this.lblDataName, "lblDataName");
            this.lblDataName.Name = "lblDataName";
            // 
            // cmbServerName
            // 
            this.cmbServerName.FormattingEnabled = true;
            resources.ApplyResources(this.cmbServerName, "cmbServerName");
            this.cmbServerName.Name = "cmbServerName";
            this.cmbServerName.SelectedIndexChanged += new System.EventHandler(this.cmbServerName_SelectedIndexChanged);
            this.cmbServerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbServerName_KeyDown);
            // 
            // chkSaveConInfo
            // 
            resources.ApplyResources(this.chkSaveConInfo, "chkSaveConInfo");
            this.chkSaveConInfo.Checked = true;
            this.chkSaveConInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveConInfo.Name = "chkSaveConInfo";
            this.chkSaveConInfo.UseVisualStyleBackColor = true;
            this.chkSaveConInfo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkSaveConInfo_KeyDown);
            // 
            // cmbAuth
            // 
            this.cmbAuth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuth.FormattingEnabled = true;
            this.cmbAuth.Items.AddRange(new object[] {
            resources.GetString("cmbAuth.Items"),
            resources.GetString("cmbAuth.Items1")});
            resources.ApplyResources(this.cmbAuth, "cmbAuth");
            this.cmbAuth.Name = "cmbAuth";
            this.cmbAuth.SelectedIndexChanged += new System.EventHandler(this.cmbAuth_SelectedIndexChanged);
            this.cmbAuth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbAuth_KeyDown);
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserName_KeyDown);
            // 
            // txtPwd
            // 
            resources.ApplyResources(this.txtPwd, "txtPwd");
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.UseSystemPasswordChar = true;
            this.txtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwd_KeyDown);
            // 
            // cmbDataName
            // 
            this.cmbDataName.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDataName, "cmbDataName");
            this.cmbDataName.Name = "cmbDataName";
            this.cmbDataName.DropDown += new System.EventHandler(this.cmbDataName_DropDown);
            this.cmbDataName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbDataName_KeyDown);
            // 
            // btnConnect
            // 
            this.btnConnect.Image = global::SQLQuerierAddIn.Properties.Resources.button_ok;
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::SQLQuerierAddIn.Properties.Resources.button_undo;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FmDBConnection
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbDataName);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.cmbAuth);
            this.Controls.Add(this.chkSaveConInfo);
            this.Controls.Add(this.cmbServerName);
            this.Controls.Add(this.lblDataName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkSavePwd);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblAuth);
            this.Controls.Add(this.lblServerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FmDBConnection";
            this.Load += new System.EventHandler(this.FmDBConnection_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FmDBConnection_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Label lblAuth;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckBox chkSavePwd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDataName;
        private System.Windows.Forms.ComboBox cmbServerName;
        private System.Windows.Forms.CheckBox chkSaveConInfo;
        private System.Windows.Forms.ComboBox cmbAuth;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.ComboBox cmbDataName;
    }
}