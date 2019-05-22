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
            this.addPackageStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoplaceAnnotationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelImageList = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.downloadControl = new Alturos.Yolo.LearningImage.CustomControls.DownloadControl();
            this.annotationImageListControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationImageListControl();
            this.tagListControl = new Alturos.Yolo.LearningImage.CustomControls.TagListControl();
            this.annotationPackageListControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationPackageListControl();
            this.annotationDrawControl = new Alturos.Yolo.LearningImage.CustomControls.AnnotationDrawControl();
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
            this.exportToolStripMenuItem,
            this.addPackageStripMenuItem,
            this.configurationToolStripMenuItem});
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
            this.syncToolStripMenuItem.Name = "syncToolStripMenuItem";
            this.syncToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.syncToolStripMenuItem.Text = "&Sync";
            this.syncToolStripMenuItem.Click += new System.EventHandler(this.syncToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "&Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // addPackageStripMenuItem
            // 
            this.addPackageStripMenuItem.Name = "addPackageStripMenuItem";
            this.addPackageStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.addPackageStripMenuItem.Text = "&Add Package";
            this.addPackageStripMenuItem.Click += new System.EventHandler(this.addPackageStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoplaceAnnotationsToolStripMenuItem,
            this.showLabelsToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configurationToolStripMenuItem.Text = "&Configuration";
            // 
            // autoplaceAnnotationsToolStripMenuItem
            // 
            this.autoplaceAnnotationsToolStripMenuItem.Name = "autoplaceAnnotationsToolStripMenuItem";
            this.autoplaceAnnotationsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.autoplaceAnnotationsToolStripMenuItem.Text = "Autoplace Annotations";
            this.autoplaceAnnotationsToolStripMenuItem.Click += new System.EventHandler(this.AutoplaceAnnotationsToolStripMenuItem_Click);
            // 
            // showLabelsToolStripMenuItem
            // 
            this.showLabelsToolStripMenuItem.Name = "showLabelsToolStripMenuItem";
            this.showLabelsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.showLabelsToolStripMenuItem.Text = "Show Labels";
            this.showLabelsToolStripMenuItem.Click += new System.EventHandler(this.ShowLabelsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelImageList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotationPackageListControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotationDrawControl, 2, 0);
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
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.downloadControl);
            this.splitContainer1.Panel1.Controls.Add(this.annotationImageListControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tagListControl);
            this.splitContainer1.Size = new System.Drawing.Size(344, 524);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 0;
            // 
            // downloadControl
            // 
            this.downloadControl.BackColor = System.Drawing.SystemColors.Control;
            this.downloadControl.Enabled = false;
            this.downloadControl.ExtractionRequested = null;
            this.downloadControl.Location = new System.Drawing.Point(0, 0);
            this.downloadControl.Name = "downloadControl";
            this.downloadControl.Size = new System.Drawing.Size(344, 69);
            this.downloadControl.TabIndex = 1;
            // 
            // annotationImageListControl
            // 
            this.annotationImageListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationImageListControl.Location = new System.Drawing.Point(0, 0);
            this.annotationImageListControl.Name = "annotationImageListControl";
            this.annotationImageListControl.Size = new System.Drawing.Size(344, 300);
            this.annotationImageListControl.TabIndex = 0;
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
            this.annotationPackageListControl.Location = new System.Drawing.Point(3, 3);
            this.annotationPackageListControl.Name = "annotationPackageListControl";
            this.annotationPackageListControl.Size = new System.Drawing.Size(344, 524);
            this.annotationPackageListControl.TabIndex = 1;
            // 
            // annotationDrawControl
            // 
            this.annotationDrawControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationDrawControl.AutoplaceAnnotations = false;
            this.annotationDrawControl.Location = new System.Drawing.Point(703, 3);
            this.annotationDrawControl.Name = "annotationDrawControl";
            this.annotationDrawControl.ShowLabels = false;
            this.annotationDrawControl.Size = new System.Drawing.Size(628, 524);
            this.annotationDrawControl.TabIndex = 2;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
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
        private CustomControls.AnnotationImageListControl annotationImageListControl;
        private CustomControls.AnnotationPackageListControl annotationPackageListControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CustomControls.AnnotationDrawControl annotationDrawControl;
        private System.Windows.Forms.Panel panelImageList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private CustomControls.TagListControl tagListControl;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPackageStripMenuItem;
        private CustomControls.DownloadControl downloadControl;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoplaceAnnotationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLabelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

