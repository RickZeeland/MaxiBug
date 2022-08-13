namespace MaxiBug
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownRecords = new System.Windows.Forms.NumericUpDown();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBoxPostgres = new System.Windows.Forms.GroupBox();
            this.buttonEye = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.txtIpaddress = new System.Windows.Forms.TextBox();
            this.labelIpaddress = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecords)).BeginInit();
            this.groupBoxPostgres.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 206);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of records to add";
            // 
            // numericUpDownRecords
            // 
            this.numericUpDownRecords.Location = new System.Drawing.Point(244, 204);
            this.numericUpDownRecords.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownRecords.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRecords.Name = "numericUpDownRecords";
            this.numericUpDownRecords.Size = new System.Drawing.Size(65, 25);
            this.numericUpDownRecords.TabIndex = 1;
            this.numericUpDownRecords.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(361, 234);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(92, 36);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBoxPostgres
            // 
            this.groupBoxPostgres.Controls.Add(this.buttonEye);
            this.groupBoxPostgres.Controls.Add(this.txtPassword);
            this.groupBoxPostgres.Controls.Add(this.labelPassword);
            this.groupBoxPostgres.Controls.Add(this.txtUsername);
            this.groupBoxPostgres.Controls.Add(this.labelUser);
            this.groupBoxPostgres.Controls.Add(this.txtPort);
            this.groupBoxPostgres.Controls.Add(this.labelPort);
            this.groupBoxPostgres.Controls.Add(this.txtIpaddress);
            this.groupBoxPostgres.Controls.Add(this.labelIpaddress);
            this.groupBoxPostgres.Location = new System.Drawing.Point(4, 58);
            this.groupBoxPostgres.Name = "groupBoxPostgres";
            this.groupBoxPostgres.Size = new System.Drawing.Size(472, 130);
            this.groupBoxPostgres.TabIndex = 17;
            this.groupBoxPostgres.TabStop = false;
            this.groupBoxPostgres.Text = "PostgreSQL";
            // 
            // buttonEye
            // 
            this.buttonEye.BackColor = System.Drawing.Color.Transparent;
            this.buttonEye.FlatAppearance.BorderSize = 0;
            this.buttonEye.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEye.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonEye.Location = new System.Drawing.Point(308, 89);
            this.buttonEye.Name = "buttonEye";
            this.buttonEye.Size = new System.Drawing.Size(32, 27);
            this.buttonEye.TabIndex = 24;
            this.buttonEye.Text = "N";
            this.buttonEye.UseVisualStyleBackColor = false;
            this.buttonEye.Click += new System.EventHandler(this.buttonEye_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(125, 90);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(179, 25);
            this.txtPassword.TabIndex = 23;
            this.txtPassword.Text = "testbot1";
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(22, 93);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(96, 17);
            this.labelPassword.TabIndex = 22;
            this.labelPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(126, 58);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(179, 25);
            this.txtUsername.TabIndex = 21;
            this.txtUsername.Text = "testbot1";
            // 
            // labelUser
            // 
            this.labelUser.Location = new System.Drawing.Point(23, 61);
            this.labelUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(95, 17);
            this.labelUser.TabIndex = 20;
            this.labelUser.Text = "User name";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(376, 27);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(73, 25);
            this.txtPort.TabIndex = 19;
            this.txtPort.Text = "5432";
            // 
            // labelPort
            // 
            this.labelPort.Location = new System.Drawing.Point(326, 30);
            this.labelPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(44, 17);
            this.labelPort.TabIndex = 18;
            this.labelPort.Text = "Port";
            // 
            // txtIpaddress
            // 
            this.txtIpaddress.Location = new System.Drawing.Point(126, 27);
            this.txtIpaddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtIpaddress.Name = "txtIpaddress";
            this.txtIpaddress.Size = new System.Drawing.Size(179, 25);
            this.txtIpaddress.TabIndex = 17;
            this.txtIpaddress.Text = "127.0.0.1";
            // 
            // labelIpaddress
            // 
            this.labelIpaddress.Location = new System.Drawing.Point(23, 30);
            this.labelIpaddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIpaddress.Name = "labelIpaddress";
            this.labelIpaddress.Size = new System.Drawing.Size(95, 17);
            this.labelIpaddress.TabIndex = 15;
            this.labelIpaddress.Text = "Ip address";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(14, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(422, 46);
            this.labelInfo.TabIndex = 18;
            this.labelInfo.Text = "Note: before using this TestBot, matching user names and passwords must be added " +
    "in Postgres.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelInfo);
            this.panel1.Controls.Add(this.groupBoxPostgres);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonOk);
            this.panel1.Controls.Add(this.numericUpDownRecords);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(486, 279);
            this.panel1.TabIndex = 19;
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.LimeGreen;
            this.progressBar1.Location = new System.Drawing.Point(17, 286);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(450, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 20;
            this.progressBar1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 318);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MaxiBug TestBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecords)).EndInit();
            this.groupBoxPostgres.ResumeLayout(false);
            this.groupBoxPostgres.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownRecords;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBoxPostgres;
        private System.Windows.Forms.Button buttonEye;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox txtIpaddress;
        private System.Windows.Forms.Label labelIpaddress;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

