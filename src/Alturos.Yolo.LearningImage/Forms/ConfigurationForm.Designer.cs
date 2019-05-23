namespace Alturos.Yolo.LearningImage.Forms
{
    partial class ConfigurationForm
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerSettings = new System.Windows.Forms.SplitContainer();
            this.groupBoxObjectClasses = new System.Windows.Forms.GroupBox();
            this.textBoxObjectClass = new System.Windows.Forms.TextBox();
            this.buttonAddObjectClass = new System.Windows.Forms.Button();
            this.dataGridViewObjectClasses = new System.Windows.Forms.DataGridView();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxTags = new System.Windows.Forms.GroupBox();
            this.textBoxTag = new System.Windows.Forms.TextBox();
            this.buttonAddTag = new System.Windows.Forms.Button();
            this.dataGridViewTags = new System.Windows.Forms.DataGridView();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripTag = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSettings)).BeginInit();
            this.splitContainerSettings.Panel1.SuspendLayout();
            this.splitContainerSettings.Panel2.SuspendLayout();
            this.splitContainerSettings.SuspendLayout();
            this.groupBoxObjectClasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjectClasses)).BeginInit();
            this.groupBoxTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTags)).BeginInit();
            this.contextMenuStripTag.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerSettings);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.buttonSave);
            this.splitContainerMain.Size = new System.Drawing.Size(572, 289);
            this.splitContainerMain.SplitterDistance = 254;
            this.splitContainerMain.TabIndex = 0;
            // 
            // splitContainerSettings
            // 
            this.splitContainerSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSettings.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSettings.Name = "splitContainerSettings";
            // 
            // splitContainerSettings.Panel1
            // 
            this.splitContainerSettings.Panel1.Controls.Add(this.groupBoxObjectClasses);
            // 
            // splitContainerSettings.Panel2
            // 
            this.splitContainerSettings.Panel2.Controls.Add(this.groupBoxTags);
            this.splitContainerSettings.Size = new System.Drawing.Size(572, 254);
            this.splitContainerSettings.SplitterDistance = 292;
            this.splitContainerSettings.TabIndex = 0;
            // 
            // groupBoxObjectClasses
            // 
            this.groupBoxObjectClasses.Controls.Add(this.textBoxObjectClass);
            this.groupBoxObjectClasses.Controls.Add(this.buttonAddObjectClass);
            this.groupBoxObjectClasses.Controls.Add(this.dataGridViewObjectClasses);
            this.groupBoxObjectClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxObjectClasses.Location = new System.Drawing.Point(0, 0);
            this.groupBoxObjectClasses.Name = "groupBoxObjectClasses";
            this.groupBoxObjectClasses.Size = new System.Drawing.Size(292, 254);
            this.groupBoxObjectClasses.TabIndex = 0;
            this.groupBoxObjectClasses.TabStop = false;
            this.groupBoxObjectClasses.Text = "Object Classes";
            // 
            // textBoxObjectClass
            // 
            this.textBoxObjectClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxObjectClass.Location = new System.Drawing.Point(7, 227);
            this.textBoxObjectClass.Name = "textBoxObjectClass";
            this.textBoxObjectClass.Size = new System.Drawing.Size(183, 20);
            this.textBoxObjectClass.TabIndex = 2;
            // 
            // buttonAddObjectClass
            // 
            this.buttonAddObjectClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddObjectClass.Location = new System.Drawing.Point(196, 226);
            this.buttonAddObjectClass.Name = "buttonAddObjectClass";
            this.buttonAddObjectClass.Size = new System.Drawing.Size(90, 21);
            this.buttonAddObjectClass.TabIndex = 1;
            this.buttonAddObjectClass.Text = "Add";
            this.buttonAddObjectClass.UseVisualStyleBackColor = true;
            this.buttonAddObjectClass.Click += new System.EventHandler(this.ButtonAddObjectClass_Click);
            // 
            // dataGridViewObjectClasses
            // 
            this.dataGridViewObjectClasses.AllowUserToAddRows = false;
            this.dataGridViewObjectClasses.AllowUserToDeleteRows = false;
            this.dataGridViewObjectClasses.AllowUserToResizeColumns = false;
            this.dataGridViewObjectClasses.AllowUserToResizeRows = false;
            this.dataGridViewObjectClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewObjectClasses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewObjectClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewObjectClasses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnId,
            this.ColumnName});
            this.dataGridViewObjectClasses.Location = new System.Drawing.Point(7, 20);
            this.dataGridViewObjectClasses.Name = "dataGridViewObjectClasses";
            this.dataGridViewObjectClasses.ReadOnly = true;
            this.dataGridViewObjectClasses.RowHeadersVisible = false;
            this.dataGridViewObjectClasses.Size = new System.Drawing.Size(279, 200);
            this.dataGridViewObjectClasses.TabIndex = 0;
            // 
            // ColumnId
            // 
            this.ColumnId.DataPropertyName = "Id";
            this.ColumnId.FillWeight = 50.76142F;
            this.ColumnId.HeaderText = "Id";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            this.ColumnName.FillWeight = 149.2386F;
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // groupBoxTags
            // 
            this.groupBoxTags.Controls.Add(this.textBoxTag);
            this.groupBoxTags.Controls.Add(this.buttonAddTag);
            this.groupBoxTags.Controls.Add(this.dataGridViewTags);
            this.groupBoxTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTags.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTags.Name = "groupBoxTags";
            this.groupBoxTags.Size = new System.Drawing.Size(276, 254);
            this.groupBoxTags.TabIndex = 0;
            this.groupBoxTags.TabStop = false;
            this.groupBoxTags.Text = "Tags";
            // 
            // textBoxTag
            // 
            this.textBoxTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTag.Location = new System.Drawing.Point(6, 226);
            this.textBoxTag.Name = "textBoxTag";
            this.textBoxTag.Size = new System.Drawing.Size(168, 20);
            this.textBoxTag.TabIndex = 3;
            // 
            // buttonAddTag
            // 
            this.buttonAddTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddTag.Location = new System.Drawing.Point(180, 226);
            this.buttonAddTag.Name = "buttonAddTag";
            this.buttonAddTag.Size = new System.Drawing.Size(90, 21);
            this.buttonAddTag.TabIndex = 2;
            this.buttonAddTag.Text = "Add";
            this.buttonAddTag.UseVisualStyleBackColor = true;
            this.buttonAddTag.Click += new System.EventHandler(this.ButtonAddTag_Click);
            // 
            // dataGridViewTags
            // 
            this.dataGridViewTags.AllowUserToAddRows = false;
            this.dataGridViewTags.AllowUserToDeleteRows = false;
            this.dataGridViewTags.AllowUserToResizeColumns = false;
            this.dataGridViewTags.AllowUserToResizeRows = false;
            this.dataGridViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTags.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTags.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnValue});
            this.dataGridViewTags.ContextMenuStrip = this.contextMenuStripTag;
            this.dataGridViewTags.Location = new System.Drawing.Point(6, 20);
            this.dataGridViewTags.Name = "dataGridViewTags";
            this.dataGridViewTags.ReadOnly = true;
            this.dataGridViewTags.RowHeadersVisible = false;
            this.dataGridViewTags.Size = new System.Drawing.Size(264, 200);
            this.dataGridViewTags.TabIndex = 1;
            // 
            // ColumnValue
            // 
            this.ColumnValue.DataPropertyName = "Value";
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.ReadOnly = true;
            // 
            // contextMenuStripTag
            // 
            this.contextMenuStripTag.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.contextMenuStripTag.Name = "contextMenuStripTag";
            this.contextMenuStripTag.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "&Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItem_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(476, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(90, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 289);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "ConfigurationForm";
            this.Text = "Settings";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerSettings.Panel1.ResumeLayout(false);
            this.splitContainerSettings.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSettings)).EndInit();
            this.splitContainerSettings.ResumeLayout(false);
            this.groupBoxObjectClasses.ResumeLayout(false);
            this.groupBoxObjectClasses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjectClasses)).EndInit();
            this.groupBoxTags.ResumeLayout(false);
            this.groupBoxTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTags)).EndInit();
            this.contextMenuStripTag.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerSettings;
        private System.Windows.Forms.GroupBox groupBoxObjectClasses;
        private System.Windows.Forms.GroupBox groupBoxTags;
        private System.Windows.Forms.Button buttonAddObjectClass;
        private System.Windows.Forms.DataGridView dataGridViewObjectClasses;
        private System.Windows.Forms.Button buttonAddTag;
        private System.Windows.Forms.DataGridView dataGridViewTags;
        private System.Windows.Forms.TextBox textBoxObjectClass;
        private System.Windows.Forms.TextBox textBoxTag;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTag;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
    }
}