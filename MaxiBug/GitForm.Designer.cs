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
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabControlGit = new System.Windows.Forms.TabControl();
            this.tabPageGitLocal = new System.Windows.Forms.TabPage();
            this.tabPageGitHub = new System.Windows.Forms.TabPage();
            this.labelGitHubRepo = new System.Windows.Forms.Label();
            this.txtGitHubRepo = new System.Windows.Forms.TextBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.txtGitHubToken = new System.Windows.Forms.TextBox();
            this.txtGitHubUserName = new System.Windows.Forms.TextBox();
            this.labelGitHubToken = new System.Windows.Forms.Label();
            this.labelGitHubUserName = new System.Windows.Forms.Label();
            this.tabControlGit.SuspendLayout();
            this.tabPageGitLocal.SuspendLayout();
            this.tabPageGitHub.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelGitHeader
            // 
            this.labelGitHeader.AutoSize = true;
            this.labelGitHeader.Location = new System.Drawing.Point(15, 13);
            this.labelGitHeader.Name = "labelGitHeader";
            this.labelGitHeader.Size = new System.Drawing.Size(484, 34);
            this.labelGitHeader.TabIndex = 23;
            this.labelGitHeader.Text = "Git command to retrieve Git history for an issue with a regular expression (RegEx" +
    ")\r\nWith the -E option, POSIX ERE syntax is enforced";
            // 
            // labelGitInfo
            // 
            this.labelGitInfo.AutoSize = true;
            this.labelGitInfo.Location = new System.Drawing.Point(63, 144);
            this.labelGitInfo.Name = "labelGitInfo";
            this.labelGitInfo.Size = new System.Drawing.Size(287, 34);
            this.labelGitInfo.TabIndex = 22;
            this.labelGitInfo.Text = "Note: defaults can be set in settings.\r\n{path} will be substituted with the chose" +
    "n folder.";
            // 
            // txtGitCommand
            // 
            this.txtGitCommand.Location = new System.Drawing.Point(18, 64);
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
            this.buttonGitFolder.Location = new System.Drawing.Point(526, 104);
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
            this.txtGitFolder.Location = new System.Drawing.Point(66, 108);
            this.txtGitFolder.Name = "txtGitFolder";
            this.txtGitFolder.Size = new System.Drawing.Size(460, 25);
            this.txtGitFolder.TabIndex = 19;
            // 
            // labelDefault
            // 
            this.labelDefault.AutoSize = true;
            this.labelDefault.Location = new System.Drawing.Point(15, 111);
            this.labelDefault.Name = "labelDefault";
            this.labelDefault.Size = new System.Drawing.Size(45, 17);
            this.labelDefault.TabIndex = 18;
            this.labelDefault.Text = "Folder";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.Location = new System.Drawing.Point(417, 346);
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
            this.btOk.Location = new System.Drawing.Point(312, 347);
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
            this.labelGitInfoAxo.Location = new System.Drawing.Point(63, 188);
            this.labelGitInfoAxo.Name = "labelGitInfoAxo";
            this.labelGitInfoAxo.Size = new System.Drawing.Size(250, 85);
            this.labelGitInfoAxo.TabIndex = 26;
            this.labelGitInfoAxo.Text = "The example will find commits containing:\r\n[axod: 123]\r\n[axof: 123]\r\n[AXOT: 123]\r" +
    "\netc.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(423, 256);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(76, 17);
            this.linkLabel1.TabIndex = 27;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Git log help";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tabControlGit
            // 
            this.tabControlGit.Controls.Add(this.tabPageGitLocal);
            this.tabControlGit.Controls.Add(this.tabPageGitHub);
            this.tabControlGit.Location = new System.Drawing.Point(3, 12);
            this.tabControlGit.Name = "tabControlGit";
            this.tabControlGit.SelectedIndex = 0;
            this.tabControlGit.Size = new System.Drawing.Size(573, 328);
            this.tabControlGit.TabIndex = 28;
            // 
            // tabPageGitLocal
            // 
            this.tabPageGitLocal.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageGitLocal.Controls.Add(this.labelGitHeader);
            this.tabPageGitLocal.Controls.Add(this.linkLabel1);
            this.tabPageGitLocal.Controls.Add(this.labelDefault);
            this.tabPageGitLocal.Controls.Add(this.labelGitInfoAxo);
            this.tabPageGitLocal.Controls.Add(this.txtGitFolder);
            this.tabPageGitLocal.Controls.Add(this.buttonGitFolder);
            this.tabPageGitLocal.Controls.Add(this.txtGitCommand);
            this.tabPageGitLocal.Controls.Add(this.labelGitInfo);
            this.tabPageGitLocal.Location = new System.Drawing.Point(4, 26);
            this.tabPageGitLocal.Name = "tabPageGitLocal";
            this.tabPageGitLocal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGitLocal.Size = new System.Drawing.Size(565, 298);
            this.tabPageGitLocal.TabIndex = 0;
            this.tabPageGitLocal.Text = "Local";
            // 
            // tabPageGitHub
            // 
            this.tabPageGitHub.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageGitHub.Controls.Add(this.labelGitHubRepo);
            this.tabPageGitHub.Controls.Add(this.txtGitHubRepo);
            this.tabPageGitHub.Controls.Add(this.linkLabel2);
            this.tabPageGitHub.Controls.Add(this.txtGitHubToken);
            this.tabPageGitHub.Controls.Add(this.txtGitHubUserName);
            this.tabPageGitHub.Controls.Add(this.labelGitHubToken);
            this.tabPageGitHub.Controls.Add(this.labelGitHubUserName);
            this.tabPageGitHub.Location = new System.Drawing.Point(4, 26);
            this.tabPageGitHub.Name = "tabPageGitHub";
            this.tabPageGitHub.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGitHub.Size = new System.Drawing.Size(565, 298);
            this.tabPageGitHub.TabIndex = 1;
            this.tabPageGitHub.Text = "GitHub";
            // 
            // labelGitHubRepo
            // 
            this.labelGitHubRepo.AutoSize = true;
            this.labelGitHubRepo.Location = new System.Drawing.Point(22, 98);
            this.labelGitHubRepo.Name = "labelGitHubRepo";
            this.labelGitHubRepo.Size = new System.Drawing.Size(74, 17);
            this.labelGitHubRepo.TabIndex = 6;
            this.labelGitHubRepo.Text = "Repository:";
            // 
            // txtGitHubRepo
            // 
            this.txtGitHubRepo.Location = new System.Drawing.Point(25, 122);
            this.txtGitHubRepo.Name = "txtGitHubRepo";
            this.txtGitHubRepo.Size = new System.Drawing.Size(518, 25);
            this.txtGitHubRepo.TabIndex = 5;
            this.txtGitHubRepo.Text = "api.github.com/search/commits?q=repo:MyName/MyRepo+Bumped";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(22, 231);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(165, 17);
            this.linkLabel2.TabIndex = 4;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Personal access token help";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // txtGitHubToken
            // 
            this.txtGitHubToken.Location = new System.Drawing.Point(144, 59);
            this.txtGitHubToken.Name = "txtGitHubToken";
            this.txtGitHubToken.Size = new System.Drawing.Size(399, 25);
            this.txtGitHubToken.TabIndex = 3;
            this.txtGitHubToken.UseSystemPasswordChar = true;
            // 
            // txtGitHubUserName
            // 
            this.txtGitHubUserName.Location = new System.Drawing.Point(144, 24);
            this.txtGitHubUserName.Name = "txtGitHubUserName";
            this.txtGitHubUserName.Size = new System.Drawing.Size(399, 25);
            this.txtGitHubUserName.TabIndex = 2;
            this.txtGitHubUserName.Text = "myname@gmail.com";
            // 
            // labelGitHubToken
            // 
            this.labelGitHubToken.AutoSize = true;
            this.labelGitHubToken.Location = new System.Drawing.Point(22, 62);
            this.labelGitHubToken.Name = "labelGitHubToken";
            this.labelGitHubToken.Size = new System.Drawing.Size(94, 17);
            this.labelGitHubToken.TabIndex = 1;
            this.labelGitHubToken.Text = "Personal token";
            // 
            // labelGitHubUserName
            // 
            this.labelGitHubUserName.AutoSize = true;
            this.labelGitHubUserName.Location = new System.Drawing.Point(22, 27);
            this.labelGitHubUserName.Name = "labelGitHubUserName";
            this.labelGitHubUserName.Size = new System.Drawing.Size(71, 17);
            this.labelGitHubUserName.TabIndex = 0;
            this.labelGitHubUserName.Text = "User name";
            // 
            // GitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 390);
            this.Controls.Add(this.tabControlGit);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GitForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Retrieve Git history";
            this.TopMost = true;
            this.tabControlGit.ResumeLayout(false);
            this.tabPageGitLocal.ResumeLayout(false);
            this.tabPageGitLocal.PerformLayout();
            this.tabPageGitHub.ResumeLayout(false);
            this.tabPageGitHub.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TabControl tabControlGit;
        private System.Windows.Forms.TabPage tabPageGitLocal;
        private System.Windows.Forms.TabPage tabPageGitHub;
        private System.Windows.Forms.TextBox txtGitHubToken;
        private System.Windows.Forms.TextBox txtGitHubUserName;
        private System.Windows.Forms.Label labelGitHubToken;
        private System.Windows.Forms.Label labelGitHubUserName;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label labelGitHubRepo;
        private System.Windows.Forms.TextBox txtGitHubRepo;
    }
}