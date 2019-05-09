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
            this.syncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelImageList = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.annotationImageListControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationImageListControl();
            this.tagListControl = new Alturos.Yolo.LearningImage.CustomControls.TagListControl();
            this.annotationPackageListControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationPackageListControl();
            this.annotationImageControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationImageControl();
            this.menuStripMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelImageList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.syncToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1334, 24);
            this.menuStripMain.TabIndex = 2;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.loadToolStripMenuItem.Text = "&Refresh";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // syncToolStripMenuItem
            // 
            this.syncToolStripMenuItem.Enabled = false;
            this.syncToolStripMenuItem.Name = "syncToolStripMenuItem";
            this.syncToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.syncToolStripMenuItem.Text = "Sync";
            this.syncToolStripMenuItem.Click += new System.EventHandler(this.syncToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allImagesToolStripMenuItem,
            this.selectedImagesToolStripMenuItem});
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "&Export";
            // 
            // allImagesToolStripMenuItem
            // 
            this.allImagesToolStripMenuItem.Name = "allImagesToolStripMenuItem";
            this.allImagesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.allImagesToolStripMenuItem.Text = "&All Images";
            this.allImagesToolStripMenuItem.Click += new System.EventHandler(this.allImagesToolStripMenuItem_Click);
            // 
            // selectedImagesToolStripMenuItem
            // 
            this.selectedImagesToolStripMenuItem.Name = "selectedImagesToolStripMenuItem";
            this.selectedImagesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.selectedImagesToolStripMenuItem.Text = "&Selected Images";
            this.selectedImagesToolStripMenuItem.Click += new System.EventHandler(this.selectedImagesToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelImageList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotationPackageListControl, 0, 0);
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
            this.panelImageList.Controls.Add(this.splitContainer1);
            this.panelImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageList.Location = new System.Drawing.Point(353, 3);
            this.panelImageList.Name = "panelImageList";
            this.panelImageList.Size = new System.Drawing.Size(344, 524);
            this.panelImageList.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.annotationImageListControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tagListControl);
            this.splitContainer1.Size = new System.Drawing.Size(344, 524);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 0;
            // 
            // annotationImageListControl
            // 
            this.annotationImageListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationImageListControl.ImageSelected = null;
            this.annotationImageListControl.Location = new System.Drawing.Point(0, 0);
            this.annotationImageListControl.Name = "annotationImageListControl";
            this.annotationImageListControl.Size = new System.Drawing.Size(344, 300);
            this.annotationImageListControl.TabIndex = 0;
            this.annotationImageListControl.Load += new System.EventHandler(this.annotationImageList_Load);
            // 
            // tagListControl
            // 
            this.tagListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagListControl.Location = new System.Drawing.Point(0, 0);
            this.tagListControl.Name = "tagListControl";
            this.tagListControl.Size = new System.Drawing.Size(344, 220);
            this.tagListControl.TabIndex = 0;
            // 
            // annotationPackageListControl
            // 
            this.annotationPackageListControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationPackageListControl.FolderSelected = null;
            this.annotationPackageListControl.Location = new System.Drawing.Point(3, 3);
            this.annotationPackageListControl.Name = "annotationPackageListControl";
            this.annotationPackageListControl.Size = new System.Drawing.Size(344, 524);
            this.annotationPackageListControl.TabIndex = 1;
            this.annotationPackageListControl.Load += new System.EventHandler(this.annotationPackageList_Load);
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
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedImagesToolStripMenuItem;
        private CustomControls.AnnotationImageListControl annotationImageListControl;
        private CustomControls.AnnotationPackageListControl annotationPackageListControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CustomControls.AnnotationImageControl annotationImageControl;
        private System.Windows.Forms.Panel panelImageList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private CustomControls.TagListControl tagListControl;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem;
    }
}

