namespace SQLQuerierAddIn.Forms
{
    partial class FavSQLBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavSQLBase));
            this.pLeft = new System.Windows.Forms.Panel();
            this.pLeftTop = new System.Windows.Forms.Panel();
            this.tecSQLEdit = new ICSharpCode.TextEditor.TextEditorControl();
            this.pLeftBottom = new System.Windows.Forms.Panel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pLeft.SuspendLayout();
            this.pLeftTop.SuspendLayout();
            this.pLeftBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pLeft
            // 
            this.pLeft.Controls.Add(this.pLeftTop);
            this.pLeft.Controls.Add(this.pLeftBottom);
            resources.ApplyResources(this.pLeft, "pLeft");
            this.pLeft.Name = "pLeft";
            // 
            // pLeftTop
            // 
            this.pLeftTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pLeftTop.Controls.Add(this.tecSQLEdit);
            resources.ApplyResources(this.pLeftTop, "pLeftTop");
            this.pLeftTop.Name = "pLeftTop";
            // 
            // tecSQLEdit
            // 
            resources.ApplyResources(this.tecSQLEdit, "tecSQLEdit");
            this.tecSQLEdit.IsReadOnly = false;
            this.tecSQLEdit.Name = "tecSQLEdit";
            this.tecSQLEdit.TextChanged += new System.EventHandler(this.tecSQLEdit_TextChanged);
            // 
            // pLeftBottom
            // 
            this.pLeftBottom.Controls.Add(this.btnRun);
            this.pLeftBottom.Controls.Add(this.btnSave);
            resources.ApplyResources(this.pLeftBottom, "pLeftBottom");
            this.pLeftBottom.Name = "pLeftBottom";
            // 
            // btnRun
            // 
            resources.ApplyResources(this.btnRun, "btnRun");
            this.btnRun.Image = global::SQLQuerierAddIn.Properties.Resources.button_ok;
            this.btnRun.Name = "btnRun";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::SQLQuerierAddIn.Properties.Resources.save;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FavSQLBase
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pLeft);
            this.Name = "FavSQLBase";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FavSQLBase_KeyDown);
            this.pLeft.ResumeLayout(false);
            this.pLeftTop.ResumeLayout(false);
            this.pLeftBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pLeft;
        private System.Windows.Forms.Panel pLeftBottom;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Panel pLeftTop;
        public ICSharpCode.TextEditor.TextEditorControl tecSQLEdit;
    }
}