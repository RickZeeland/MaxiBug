namespace MaxiBug
{
    partial class GitForm
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
            this.labelGitHeader = new System.Windows.Forms.Label();
            this.labelGitInfo = new System.Windows.Forms.Label();
            this.txtGitCommand = new System.Windows.Forms.TextBox();
            this.buttonGitFolder = new System.Windows.Forms.Button();
            this.txtGitFolder = new System.Windows.Forms.TextBox();
            this.labelDefault = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.labelGitInfoAxo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelGitHeader
            // 
            this.labelGitHeader.AutoSize = true;
            this.labelGitHeader.Location = new System.Drawing.Point(12, 20);
            this.labelGitHeader.Name = "labelGitHeader";
            this.labelGitHeader.Size = new System.Drawing.Size(484, 34);
            this.labelGitHeader.TabIndex = 23;
            this.labelGitHeader.Text = "Git command to retrieve Git history for an issue with a regular expression (RegEx" +
    ")\r\nWith the -E option, POSIX ERE syntax is enforced";
            // 
            // labelGitInfo
            // 
            this.labelGitInfo.AutoSize = true;
            this.labelGitInfo.Location = new System.Drawing.Point(12, 156);
            this.labelGitInfo.Name = "labelGitInfo";
            this.labelGitInfo.Size = new System.Drawing.Size(501, 34);
            this.labelGitInfo.TabIndex = 22;
            this.labelGitInfo.Text = "Note: defaults can be set in settings.\r\n{path} will be substituted with the chose" +
    "n folder and {issue_id} with the issue number.";
            // 
            // txtGitCommand
            // 
            this.txtGitCommand.Location = new System.Drawing.Point(15, 71);
            this.txtGitCommand.Name = "txtGitCommand";
            this.txtGitCommand.Size = new System.Drawing.Size(508, 25);
            this.txtGitCommand.TabIndex = 21;
            this.txtGitCommand.Text = "git -C \"{path}\" log -i -E --grep=\"\\[(axo.: {issue_id})\\]\"";
            // 
            // buttonGitFolder
            // 
            this.buttonGitFolder.BackColor = System.Drawing.Color.Transparent;
            this.buttonGitFolder.FlatAppearance.BorderSize = 0;
            this.buttonGitFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGitFolder.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonGitFolder.Location = new System.Drawing.Point(523, 111);
            this.buttonGitFolder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGitFolder.Name = "buttonGitFolder";
            this.buttonGitFolder.Size = new System.Drawing.Size(38, 30);
            this.buttonGitFolder.TabIndex = 20;
            this.buttonGitFolder.Text = "1";
            this.buttonGitFolder.UseVisualStyleBackColor = false;
            this.buttonGitFolder.Click += new System.EventHandler(this.buttonGitFolder_Click);
            // 
            // txtGitFolder
            // 
            this.txtGitFolder.Location = new System.Drawing.Point(106, 115);
            this.txtGitFolder.Name = "txtGitFolder";
            this.txtGitFolder.Size = new System.Drawing.Size(417, 25);
            this.txtGitFolder.TabIndex = 19;
            // 
            // labelDefault
            // 
            this.labelDefault.AutoSize = true;
            this.labelDefault.Location = new System.Drawing.Point(12, 118);
            this.labelDefault.Name = "labelDefault";
            this.labelDefault.Size = new System.Drawing.Size(88, 17);
            this.labelDefault.TabIndex = 18;
            this.labelDefault.Text = "Default folder";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.Location = new System.Drawing.Point(417, 296);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(88, 31);
            this.btCancel.TabIndex = 25;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOk.Location = new System.Drawing.Point(312, 297);
            this.btOk.Margin = new System.Windows.Forms.Padding(4);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(88, 30);
            this.btOk.TabIndex = 24;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // labelGitInfoAxo
            // 
            this.labelGitInfoAxo.AutoSize = true;
            this.labelGitInfoAxo.Location = new System.Drawing.Point(12, 200);
            this.labelGitInfoAxo.Name = "labelGitInfoAxo";
            this.labelGitInfoAxo.Size = new System.Drawing.Size(265, 85);
            this.labelGitInfoAxo.TabIndex = 26;
            this.labelGitInfoAxo.Text = "The example will find commits with tags like:\r\n[axod: 123]\r\n[axof: 123]\r\n[AXOT: 1" +
    "23]\r\netc.";
            // 
            // GitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 340);
            this.Controls.Add(this.labelGitInfoAxo);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.labelGitHeader);
            this.Controls.Add(this.labelGitInfo);
            this.Controls.Add(this.txtGitCommand);
            this.Controls.Add(this.buttonGitFolder);
            this.Controls.Add(this.txtGitFolder);
            this.Controls.Add(this.labelDefault);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Retrieve Git history";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelGitHeader;
        private System.Windows.Forms.Label labelGitInfo;
        private System.Windows.Forms.TextBox txtGitCommand;
        private System.Windows.Forms.Button buttonGitFolder;
        private System.Windows.Forms.TextBox txtGitFolder;
        private System.Windows.Forms.Label labelDefault;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label labelGitInfoAxo;
    }
}