namespace Alturos.Yolo.LearningImage
{
    partial class ExportDialog
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
            this.dataGridViewObjects = new System.Windows.Forms.DataGridView();
            this.labelChoose = new System.Windows.Forms.Label();
            this.buttonExport = new System.Windows.Forms.Button();
            this.ColumnSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjects)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewObjects
            // 
            this.dataGridViewObjects.AllowUserToAddRows = false;
            this.dataGridViewObjects.AllowUserToDeleteRows = false;
            this.dataGridViewObjects.AllowUserToResizeColumns = false;
            this.dataGridViewObjects.AllowUserToResizeRows = false;
            this.dataGridViewObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewObjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSelected,
            this.ColumnId,
            this.ColumnName});
            this.dataGridViewObjects.Location = new System.Drawing.Point(12, 25);
            this.dataGridViewObjects.Name = "dataGridViewObjects";
            this.dataGridViewObjects.RowHeadersVisible = false;
            this.dataGridViewObjects.Size = new System.Drawing.Size(351, 134);
            this.dataGridViewObjects.TabIndex = 5;
            // 
            // labelChoose
            // 
            this.labelChoose.AutoSize = true;
            this.labelChoose.Location = new System.Drawing.Point(12, 9);
            this.labelChoose.Name = "labelChoose";
            this.labelChoose.Size = new System.Drawing.Size(127, 13);
            this.labelChoose.TabIndex = 4;
            this.labelChoose.Text = "Choose objects to export:";
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(12, 165);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(351, 41);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // ColumnSelected
            // 
            this.ColumnSelected.DataPropertyName = "Selected";
            this.ColumnSelected.FillWeight = 46.45733F;
            this.ColumnSelected.HeaderText = "";
            this.ColumnSelected.Name = "ColumnSelected";
            this.ColumnSelected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ColumnId
            // 
            this.ColumnId.DataPropertyName = "Id";
            this.ColumnId.FillWeight = 45.68528F;
            this.ColumnId.HeaderText = "Id";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            this.ColumnId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            this.ColumnName.FillWeight = 207.8574F;
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 218);
            this.Controls.Add(this.dataGridViewObjects);
            this.Controls.Add(this.labelChoose);
            this.Controls.Add(this.buttonExport);
            this.Name = "ExportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjects)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewObjects;
        private System.Windows.Forms.Label labelChoose;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
    }
}