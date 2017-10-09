namespace cloudscribeTemplate
{
    partial class ProjectOptionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectOptionsDialog));
            this.cbDataStorage = new System.Windows.Forms.ComboBox();
            this.gbSimpleContentConfig = new System.Windows.Forms.GroupBox();
            this.txtNonRootPagesTitle = new System.Windows.Forms.TextBox();
            this.txtNonRootPagesSegment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.optionZ = new System.Windows.Forms.RadioButton();
            this.optionD = new System.Windows.Forms.RadioButton();
            this.optionC = new System.Windows.Forms.RadioButton();
            this.optionB = new System.Windows.Forms.RadioButton();
            this.optionA = new System.Windows.Forms.RadioButton();
            this.chkLogging = new System.Windows.Forms.CheckBox();
            this.chkContactForm = new System.Windows.Forms.CheckBox();
            this.chkKvpProfile = new System.Windows.Forms.CheckBox();
            this.chkIdentityServer = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.cbMultiTenancy = new System.Windows.Forms.ComboBox();
            this.gbSimpleContentConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbDataStorage
            // 
            this.cbDataStorage.FormattingEnabled = true;
            this.cbDataStorage.Location = new System.Drawing.Point(71, 12);
            this.cbDataStorage.Name = "cbDataStorage";
            this.cbDataStorage.Size = new System.Drawing.Size(973, 33);
            this.cbDataStorage.TabIndex = 0;
            // 
            // gbSimpleContentConfig
            // 
            this.gbSimpleContentConfig.Controls.Add(this.txtNonRootPagesTitle);
            this.gbSimpleContentConfig.Controls.Add(this.txtNonRootPagesSegment);
            this.gbSimpleContentConfig.Controls.Add(this.label2);
            this.gbSimpleContentConfig.Controls.Add(this.label1);
            this.gbSimpleContentConfig.Controls.Add(this.optionZ);
            this.gbSimpleContentConfig.Controls.Add(this.optionD);
            this.gbSimpleContentConfig.Controls.Add(this.optionC);
            this.gbSimpleContentConfig.Controls.Add(this.optionB);
            this.gbSimpleContentConfig.Controls.Add(this.optionA);
            this.gbSimpleContentConfig.Location = new System.Drawing.Point(71, 132);
            this.gbSimpleContentConfig.Name = "gbSimpleContentConfig";
            this.gbSimpleContentConfig.Size = new System.Drawing.Size(973, 437);
            this.gbSimpleContentConfig.TabIndex = 1;
            this.gbSimpleContentConfig.TabStop = false;
            this.gbSimpleContentConfig.Text = "SimpleContent Configuration";
            // 
            // txtNonRootPagesTitle
            // 
            this.txtNonRootPagesTitle.Enabled = false;
            this.txtNonRootPagesTitle.Location = new System.Drawing.Point(304, 188);
            this.txtNonRootPagesTitle.Name = "txtNonRootPagesTitle";
            this.txtNonRootPagesTitle.Size = new System.Drawing.Size(385, 31);
            this.txtNonRootPagesTitle.TabIndex = 12;
            this.txtNonRootPagesTitle.Text = "Articles";
            // 
            // txtNonRootPagesSegment
            // 
            this.txtNonRootPagesSegment.Enabled = false;
            this.txtNonRootPagesSegment.Location = new System.Drawing.Point(304, 150);
            this.txtNonRootPagesSegment.Name = "txtNonRootPagesSegment";
            this.txtNonRootPagesSegment.Size = new System.Drawing.Size(385, 31);
            this.txtNonRootPagesSegment.TabIndex = 11;
            this.txtNonRootPagesSegment.Text = "p";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "Page Section Menu Title";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Page Route Segment";
            // 
            // optionZ
            // 
            this.optionZ.AutoSize = true;
            this.optionZ.Location = new System.Drawing.Point(23, 354);
            this.optionZ.Name = "optionZ";
            this.optionZ.Size = new System.Drawing.Size(503, 29);
            this.optionZ.TabIndex = 4;
            this.optionZ.Text = "Not installed, SimpleContent will not be included";
            this.optionZ.UseVisualStyleBackColor = true;
            // 
            // optionD
            // 
            this.optionD.AutoSize = true;
            this.optionD.Location = new System.Drawing.Point(23, 304);
            this.optionD.Name = "optionD";
            this.optionD.Size = new System.Drawing.Size(510, 29);
            this.optionD.TabIndex = 3;
            this.optionD.Text = "Blog ONLY with Home Controller as default route";
            this.optionD.UseVisualStyleBackColor = true;
            // 
            // optionC
            // 
            this.optionC.AutoSize = true;
            this.optionC.Location = new System.Drawing.Point(23, 252);
            this.optionC.Name = "optionC";
            this.optionC.Size = new System.Drawing.Size(398, 29);
            this.optionC.TabIndex = 2;
            this.optionC.Text = "Blog ONLY with Blog as default route";
            this.optionC.UseVisualStyleBackColor = true;
            // 
            // optionB
            // 
            this.optionB.AutoSize = true;
            this.optionB.Location = new System.Drawing.Point(22, 100);
            this.optionB.Name = "optionB";
            this.optionB.Size = new System.Drawing.Size(555, 29);
            this.optionB.TabIndex = 1;
            this.optionB.Text = "Pages and Blog with Home Controller as default route";
            this.optionB.UseVisualStyleBackColor = true;
            this.optionB.CheckedChanged += new System.EventHandler(this.optionB_CheckedChanged);
            // 
            // optionA
            // 
            this.optionA.AutoSize = true;
            this.optionA.Checked = true;
            this.optionA.Location = new System.Drawing.Point(22, 54);
            this.optionA.Name = "optionA";
            this.optionA.Size = new System.Drawing.Size(461, 29);
            this.optionA.TabIndex = 0;
            this.optionA.TabStop = true;
            this.optionA.Text = "Pages and Blog with Pages as default route";
            this.optionA.UseVisualStyleBackColor = true;
            // 
            // chkLogging
            // 
            this.chkLogging.AutoSize = true;
            this.chkLogging.Location = new System.Drawing.Point(93, 594);
            this.chkLogging.Name = "chkLogging";
            this.chkLogging.Size = new System.Drawing.Size(571, 29);
            this.chkLogging.TabIndex = 2;
            this.chkLogging.Text = "Include Logging to data storage with UI for viewing logs";
            this.chkLogging.UseVisualStyleBackColor = true;
            // 
            // chkContactForm
            // 
            this.chkContactForm.AutoSize = true;
            this.chkContactForm.Location = new System.Drawing.Point(93, 650);
            this.chkContactForm.Name = "chkContactForm";
            this.chkContactForm.Size = new System.Drawing.Size(319, 29);
            this.chkContactForm.TabIndex = 3;
            this.chkContactForm.Text = "Include Simple Contact Form";
            this.chkContactForm.UseVisualStyleBackColor = true;
            // 
            // chkKvpProfile
            // 
            this.chkKvpProfile.AutoSize = true;
            this.chkKvpProfile.Location = new System.Drawing.Point(94, 707);
            this.chkKvpProfile.Name = "chkKvpProfile";
            this.chkKvpProfile.Size = new System.Drawing.Size(425, 29);
            this.chkKvpProfile.TabIndex = 4;
            this.chkKvpProfile.Text = "Include (key/value) Custom Registration";
            this.chkKvpProfile.UseVisualStyleBackColor = true;
            // 
            // chkIdentityServer
            // 
            this.chkIdentityServer.AutoSize = true;
            this.chkIdentityServer.Location = new System.Drawing.Point(93, 760);
            this.chkIdentityServer.Name = "chkIdentityServer";
            this.chkIdentityServer.Size = new System.Drawing.Size(370, 29);
            this.chkIdentityServer.TabIndex = 5;
            this.chkIdentityServer.Text = "Include IdentityServer4 Integration";
            this.chkIdentityServer.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(468, 833);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(219, 51);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // cbMultiTenancy
            // 
            this.cbMultiTenancy.FormattingEnabled = true;
            this.cbMultiTenancy.Location = new System.Drawing.Point(71, 66);
            this.cbMultiTenancy.Name = "cbMultiTenancy";
            this.cbMultiTenancy.Size = new System.Drawing.Size(973, 33);
            this.cbMultiTenancy.TabIndex = 7;
            // 
            // ProjectOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 924);
            this.Controls.Add(this.cbMultiTenancy);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkIdentityServer);
            this.Controls.Add(this.chkKvpProfile);
            this.Controls.Add(this.chkContactForm);
            this.Controls.Add(this.chkLogging);
            this.Controls.Add(this.gbSimpleContentConfig);
            this.Controls.Add(this.cbDataStorage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectOptionsDialog";
            this.Text = "cloudscribe Options";
            this.gbSimpleContentConfig.ResumeLayout(false);
            this.gbSimpleContentConfig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbDataStorage;
        private System.Windows.Forms.GroupBox gbSimpleContentConfig;
        private System.Windows.Forms.RadioButton optionZ;
        private System.Windows.Forms.RadioButton optionD;
        private System.Windows.Forms.RadioButton optionC;
        private System.Windows.Forms.RadioButton optionB;
        private System.Windows.Forms.RadioButton optionA;
        private System.Windows.Forms.CheckBox chkLogging;
        private System.Windows.Forms.CheckBox chkContactForm;
        private System.Windows.Forms.CheckBox chkKvpProfile;
        private System.Windows.Forms.CheckBox chkIdentityServer;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cbMultiTenancy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNonRootPagesTitle;
        private System.Windows.Forms.TextBox txtNonRootPagesSegment;
    }
}