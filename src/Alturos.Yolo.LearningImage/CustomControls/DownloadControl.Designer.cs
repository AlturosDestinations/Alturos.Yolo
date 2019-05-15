namespace Alturos.Yolo.LearningImage.CustomControls
{
    partial class DownloadControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonDownload = new System.Windows.Forms.Button();
            this.labelNotification = new System.Windows.Forms.Label();
            this.progressBarDownload = new System.Windows.Forms.ProgressBar();
            this.groupBoxDownload = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelPercentage = new System.Windows.Forms.Label();
            this.labelDownload = new System.Windows.Forms.Label();
            this.groupBoxDownload.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDownload
            // 
            this.buttonDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.buttonDownload, 2);
            this.buttonDownload.Location = new System.Drawing.Point(3, 81);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(420, 51);
            this.buttonDownload.TabIndex = 3;
            this.buttonDownload.Text = "&Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // labelNotification
            // 
            this.labelNotification.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelNotification, 2);
            this.labelNotification.Location = new System.Drawing.Point(3, 0);
            this.labelNotification.Name = "labelNotification";
            this.labelNotification.Size = new System.Drawing.Size(294, 13);
            this.labelNotification.TabIndex = 2;
            this.labelNotification.Text = "Package isn\'t available locally, please start the download first";
            this.labelNotification.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarDownload.Location = new System.Drawing.Point(3, 33);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(360, 24);
            this.progressBarDownload.TabIndex = 4;
            this.progressBarDownload.Visible = false;
            // 
            // groupBoxDownload
            // 
            this.groupBoxDownload.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDownload.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDownload.Name = "groupBoxDownload";
            this.groupBoxDownload.Size = new System.Drawing.Size(432, 223);
            this.groupBoxDownload.TabIndex = 5;
            this.groupBoxDownload.TabStop = false;
            this.groupBoxDownload.Text = "Download";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.labelNotification, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBarDownload, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelPercentage, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonDownload, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelDownload, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 135);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // labelPercentage
            // 
            this.labelPercentage.AutoSize = true;
            this.labelPercentage.BackColor = System.Drawing.SystemColors.Control;
            this.labelPercentage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPercentage.Location = new System.Drawing.Point(369, 30);
            this.labelPercentage.Name = "labelPercentage";
            this.labelPercentage.Size = new System.Drawing.Size(54, 30);
            this.labelPercentage.TabIndex = 5;
            this.labelPercentage.Text = "0%";
            this.labelPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPercentage.Visible = false;
            // 
            // labelDownload
            // 
            this.labelDownload.AutoSize = true;
            this.labelDownload.Location = new System.Drawing.Point(3, 60);
            this.labelDownload.Name = "labelDownload";
            this.labelDownload.Size = new System.Drawing.Size(103, 13);
            this.labelDownload.TabIndex = 6;
            this.labelDownload.Text = "labelDownloadBytes";
            // 
            // DownloadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBoxDownload);
            this.Name = "DownloadControl";
            this.Size = new System.Drawing.Size(432, 223);
            this.groupBoxDownload.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label labelNotification;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.GroupBox groupBoxDownload;
        private System.Windows.Forms.Label labelPercentage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelDownload;
    }
}
