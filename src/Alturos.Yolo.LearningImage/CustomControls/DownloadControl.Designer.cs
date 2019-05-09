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
            this.groupBoxDownload.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDownload
            // 
            this.buttonDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownload.Location = new System.Drawing.Point(6, 32);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(262, 31);
            this.buttonDownload.TabIndex = 3;
            this.buttonDownload.Text = "&Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // labelNotification
            // 
            this.labelNotification.AutoSize = true;
            this.labelNotification.Location = new System.Drawing.Point(6, 16);
            this.labelNotification.Name = "labelNotification";
            this.labelNotification.Size = new System.Drawing.Size(251, 13);
            this.labelNotification.TabIndex = 2;
            this.labelNotification.Text = "Download this package to view the contents within.";
            this.labelNotification.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarDownload.Location = new System.Drawing.Point(6, 32);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(262, 31);
            this.progressBarDownload.TabIndex = 4;
            this.progressBarDownload.Visible = false;
            // 
            // groupBoxDownload
            // 
            this.groupBoxDownload.Controls.Add(this.progressBarDownload);
            this.groupBoxDownload.Controls.Add(this.labelNotification);
            this.groupBoxDownload.Controls.Add(this.buttonDownload);
            this.groupBoxDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDownload.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDownload.Name = "groupBoxDownload";
            this.groupBoxDownload.Size = new System.Drawing.Size(274, 69);
            this.groupBoxDownload.TabIndex = 5;
            this.groupBoxDownload.TabStop = false;
            this.groupBoxDownload.Text = "Download";
            // 
            // DownloadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxDownload);
            this.Name = "DownloadControl";
            this.Size = new System.Drawing.Size(274, 69);
            this.groupBoxDownload.ResumeLayout(false);
            this.groupBoxDownload.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label labelNotification;
        private System.Windows.Forms.ProgressBar progressBarDownload;
        private System.Windows.Forms.GroupBox groupBoxDownload;
    }
}
