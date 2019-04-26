namespace Alturos.Yolo.LearningImage
{
    partial class Main
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromAmazonS3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelImageList = new System.Windows.Forms.Panel();
            this.annotationImageList = new Alturos.Yolo.LearningImage.CustomControls.AnnotationImageList();
            this.annotationPackageList = new Alturos.Yolo.LearningImage.CustomControls.AnnotationPackageList();
            this.annotationImageControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationImageControl();
            this.menuStripMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelImageList.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1334, 24);
            this.menuStripMain.TabIndex = 2;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromPCToolStripMenuItem,
            this.fromAmazonS3ToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // fromPCToolStripMenuItem
            // 
            this.fromPCToolStripMenuItem.Name = "fromPCToolStripMenuItem";
            this.fromPCToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.fromPCToolStripMenuItem.Text = "From PC";
            this.fromPCToolStripMenuItem.Click += new System.EventHandler(this.fromPCToolStripMenuItem_Click);
            // 
            // fromAmazonS3ToolStripMenuItem
            // 
            this.fromAmazonS3ToolStripMenuItem.Name = "fromAmazonS3ToolStripMenuItem";
            this.fromAmazonS3ToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.fromAmazonS3ToolStripMenuItem.Text = "From Amazon S3";
            this.fromAmazonS3ToolStripMenuItem.Click += new System.EventHandler(this.fromAmazonS3ToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allImagesToolStripMenuItem,
            this.selectedImagesToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // allImagesToolStripMenuItem
            // 
            this.allImagesToolStripMenuItem.Name = "allImagesToolStripMenuItem";
            this.allImagesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.allImagesToolStripMenuItem.Text = "All Images";
            this.allImagesToolStripMenuItem.Click += new System.EventHandler(this.allImagesToolStripMenuItem_Click);
            // 
            // selectedImagesToolStripMenuItem
            // 
            this.selectedImagesToolStripMenuItem.Name = "selectedImagesToolStripMenuItem";
            this.selectedImagesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.selectedImagesToolStripMenuItem.Text = "Selected Images";
            this.selectedImagesToolStripMenuItem.Click += new System.EventHandler(this.selectedImagesToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelImageList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotationPackageList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotationImageControl, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1334, 530);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panelImageList
            // 
            this.panelImageList.Controls.Add(this.annotationImageList);
            this.panelImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageList.Location = new System.Drawing.Point(353, 3);
            this.panelImageList.Name = "panelImageList";
            this.panelImageList.Size = new System.Drawing.Size(344, 524);
            this.panelImageList.TabIndex = 4;
            // 
            // annotationImageList
            // 
            this.annotationImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationImageList.ImageSelected = null;
            this.annotationImageList.Location = new System.Drawing.Point(0, 0);
            this.annotationImageList.Name = "annotationImageList";
            this.annotationImageList.Size = new System.Drawing.Size(344, 524);
            this.annotationImageList.TabIndex = 0;
            this.annotationImageList.Load += new System.EventHandler(this.annotationImageList_Load);
            // 
            // annotationPackageList
            // 
            this.annotationPackageList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationPackageList.FolderSelected = null;
            this.annotationPackageList.Location = new System.Drawing.Point(3, 3);
            this.annotationPackageList.Name = "annotationPackageList";
            this.annotationPackageList.Size = new System.Drawing.Size(344, 524);
            this.annotationPackageList.TabIndex = 1;
            this.annotationPackageList.Load += new System.EventHandler(this.annotationPackageList_Load);
            // 
            // annotationImageControl
            // 
            this.annotationImageControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationImageControl.Location = new System.Drawing.Point(703, 3);
            this.annotationImageControl.Name = "annotationImageControl";
            this.annotationImageControl.Size = new System.Drawing.Size(628, 524);
            this.annotationImageControl.TabIndex = 2;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 554);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "Main";
            this.Text = "Alturos.Yolo.LearningImage";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelImageList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedImagesToolStripMenuItem;
        private CustomControls.AnnotationImageList annotationImageList;
        private CustomControls.AnnotationPackageList annotationPackageList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CustomControls.AnnotationImageControl annotationImageControl;
        private System.Windows.Forms.ToolStripMenuItem fromPCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromAmazonS3ToolStripMenuItem;
        private System.Windows.Forms.Panel panelImageList;
    }
}

