namespace Alturos.Yolo.LearningImage
{
    partial class StartupForm
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
            this.comboBoxAnnotationPackageProvider = new System.Windows.Forms.ComboBox();
            this.labelAnnotationPackageProvider = new System.Windows.Forms.Label();
            this.groupBoxAnnotationPackageProvider = new System.Windows.Forms.GroupBox();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.groupBoxObjectClasses = new System.Windows.Forms.GroupBox();
            this.checkBoxTemplate = new System.Windows.Forms.CheckBox();
            this.labelSelectObjectClasses = new System.Windows.Forms.Label();
            this.groupBoxAnnotationPackageProvider.SuspendLayout();
            this.groupBoxObjectClasses.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxAnnotationPackageProvider
            // 
            this.comboBoxAnnotationPackageProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnnotationPackageProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnnotationPackageProvider.FormattingEnabled = true;
            this.comboBoxAnnotationPackageProvider.Location = new System.Drawing.Point(6, 32);
            this.comboBoxAnnotationPackageProvider.Name = "comboBoxAnnotationPackageProvider";
            this.comboBoxAnnotationPackageProvider.Size = new System.Drawing.Size(390, 21);
            this.comboBoxAnnotationPackageProvider.TabIndex = 0;
            this.comboBoxAnnotationPackageProvider.SelectedIndexChanged += new System.EventHandler(this.comboBoxAnnotationPackageProvider_SelectedIndexChanged);
            // 
            // labelAnnotationPackageProvider
            // 
            this.labelAnnotationPackageProvider.AutoSize = true;
            this.labelAnnotationPackageProvider.Location = new System.Drawing.Point(6, 16);
            this.labelAnnotationPackageProvider.Name = "labelAnnotationPackageProvider";
            this.labelAnnotationPackageProvider.Size = new System.Drawing.Size(377, 13);
            this.labelAnnotationPackageProvider.TabIndex = 1;
            this.labelAnnotationPackageProvider.Text = "Which package provider do you want to use to load and sync your packages?";
            // 
            // groupBoxAnnotationPackageProvider
            // 
            this.groupBoxAnnotationPackageProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAnnotationPackageProvider.Controls.Add(this.labelAnnotationPackageProvider);
            this.groupBoxAnnotationPackageProvider.Controls.Add(this.comboBoxAnnotationPackageProvider);
            this.groupBoxAnnotationPackageProvider.Location = new System.Drawing.Point(12, 12);
            this.groupBoxAnnotationPackageProvider.Name = "groupBoxAnnotationPackageProvider";
            this.groupBoxAnnotationPackageProvider.Size = new System.Drawing.Size(411, 63);
            this.groupBoxAnnotationPackageProvider.TabIndex = 2;
            this.groupBoxAnnotationPackageProvider.TabStop = false;
            this.groupBoxAnnotationPackageProvider.Text = "Annotation Package Provider";
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfirm.Location = new System.Drawing.Point(12, 185);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(411, 36);
            this.buttonConfirm.TabIndex = 4;
            this.buttonConfirm.Text = "Start Annotating";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // groupBoxObjectClasses
            // 
            this.groupBoxObjectClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxObjectClasses.Controls.Add(this.checkBoxTemplate);
            this.groupBoxObjectClasses.Controls.Add(this.labelSelectObjectClasses);
            this.groupBoxObjectClasses.Location = new System.Drawing.Point(12, 81);
            this.groupBoxObjectClasses.Name = "groupBoxObjectClasses";
            this.groupBoxObjectClasses.Size = new System.Drawing.Size(411, 98);
            this.groupBoxObjectClasses.TabIndex = 5;
            this.groupBoxObjectClasses.TabStop = false;
            this.groupBoxObjectClasses.Text = "Object Classes";
            // 
            // checkBoxTemplate
            // 
            this.checkBoxTemplate.Location = new System.Drawing.Point(9, 45);
            this.checkBoxTemplate.Name = "checkBoxTemplate";
            this.checkBoxTemplate.Size = new System.Drawing.Size(75, 17);
            this.checkBoxTemplate.TabIndex = 4;
            this.checkBoxTemplate.Text = "Template";
            this.checkBoxTemplate.UseVisualStyleBackColor = true;
            this.checkBoxTemplate.CheckedChanged += new System.EventHandler(this.checkBoxObject_CheckedChanged);
            // 
            // labelSelectObjectClasses
            // 
            this.labelSelectObjectClasses.AutoSize = true;
            this.labelSelectObjectClasses.Location = new System.Drawing.Point(6, 16);
            this.labelSelectObjectClasses.Name = "labelSelectObjectClasses";
            this.labelSelectObjectClasses.Size = new System.Drawing.Size(232, 13);
            this.labelSelectObjectClasses.TabIndex = 3;
            this.labelSelectObjectClasses.Text = "Select the Object Classes you wish to annotate:";
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 233);
            this.Controls.Add(this.groupBoxObjectClasses);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.groupBoxAnnotationPackageProvider);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StartupForm";
            this.Text = "Alturos.Yolo.LearningImage";
            this.groupBoxAnnotationPackageProvider.ResumeLayout(false);
            this.groupBoxAnnotationPackageProvider.PerformLayout();
            this.groupBoxObjectClasses.ResumeLayout(false);
            this.groupBoxObjectClasses.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxAnnotationPackageProvider;
        private System.Windows.Forms.Label labelAnnotationPackageProvider;
        private System.Windows.Forms.GroupBox groupBoxAnnotationPackageProvider;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.GroupBox groupBoxObjectClasses;
        private System.Windows.Forms.CheckBox checkBoxTemplate;
        private System.Windows.Forms.Label labelSelectObjectClasses;
    }
}