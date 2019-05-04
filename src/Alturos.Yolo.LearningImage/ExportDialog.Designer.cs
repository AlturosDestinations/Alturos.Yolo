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
            this.labelChooseObjects = new System.Windows.Forms.Label();
            this.buttonExport = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelChooseFiles = new System.Windows.Forms.Label();
            this.annotationImageListControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationImageListControl();
            this.objectClassListControl = new Alturos.Yolo.LearningImage.CustomControls.ObjectClassListControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelChooseObjects
            // 
            this.labelChooseObjects.AutoSize = true;
            this.labelChooseObjects.Location = new System.Drawing.Point(3, 0);
            this.labelChooseObjects.Name = "labelChooseObjects";
            this.labelChooseObjects.Size = new System.Drawing.Size(237, 13);
            this.labelChooseObjects.TabIndex = 4;
            this.labelChooseObjects.Text = "Choose objects to export (Names can be edited):";
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(12, 405);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(715, 41);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelChooseFiles);
            this.splitContainer1.Panel1.Controls.Add(this.annotationImageListControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.objectClassListControl);
            this.splitContainer1.Panel2.Controls.Add(this.labelChooseObjects);
            this.splitContainer1.Size = new System.Drawing.Size(715, 387);
            this.splitContainer1.SplitterDistance = 446;
            this.splitContainer1.TabIndex = 6;
            // 
            // labelChooseFiles
            // 
            this.labelChooseFiles.AutoSize = true;
            this.labelChooseFiles.Location = new System.Drawing.Point(3, 0);
            this.labelChooseFiles.Name = "labelChooseFiles";
            this.labelChooseFiles.Size = new System.Drawing.Size(111, 13);
            this.labelChooseFiles.TabIndex = 6;
            this.labelChooseFiles.Text = "Choose files to export:";
            // 
            // annotationImageListControl
            // 
            this.annotationImageListControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationImageListControl.ExtractionRequested = null;
            this.annotationImageListControl.ImageSelected = null;
            this.annotationImageListControl.Location = new System.Drawing.Point(3, 16);
            this.annotationImageListControl.Name = "annotationImageListControl";
            this.annotationImageListControl.Size = new System.Drawing.Size(440, 368);
            this.annotationImageListControl.TabIndex = 0;
            // 
            // objectClassListControl
            // 
            this.objectClassListControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectClassListControl.Location = new System.Drawing.Point(6, 16);
            this.objectClassListControl.Name = "objectClassListControl";
            this.objectClassListControl.Size = new System.Drawing.Size(256, 368);
            this.objectClassListControl.TabIndex = 5;
            // 
            // ExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 458);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonExport);
            this.Name = "ExportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelChooseObjects;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labelChooseFiles;
        private CustomControls.AnnotationImageListControl annotationImageListControl;
        private CustomControls.ObjectClassListControl objectClassListControl;
    }
}