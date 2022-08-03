namespace MaxiBug
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lblGridlineColor = new System.Windows.Forms.Label();
            this.chkShowGridlines = new System.Windows.Forms.CheckBox();
            this.chkAlternateRowColors = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.GridlineColor = new System.Windows.Forms.Label();
            this.labelFont = new System.Windows.Forms.Label();
            this.cboFont = new System.Windows.Forms.ComboBox();
            this.labelFontsize = new System.Windows.Forms.Label();
            this.txtFontSize = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SelectionBackgroundColor = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SelectionTextColor = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AlternateRowColor = new System.Windows.Forms.Label();
            this.RowColor = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btLoadDefaults = new System.Windows.Forms.Button();
            this.chkScrollToLastRow = new System.Windows.Forms.CheckBox();
            this.chkCaseInsensitive = new System.Windows.Forms.CheckBox();
            this.chkOpenPdf = new System.Windows.Forms.CheckBox();
            this.buttonPath = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelPdfTitle = new System.Windows.Forms.Label();
            this.txtPdfTitle = new System.Windows.Forms.TextBox();
            this.labelIpaddress = new System.Windows.Forms.Label();
            this.groupBoxPostgres = new System.Windows.Forms.GroupBox();
            this.txtIpaddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxPostgres.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOk.Location = new System.Drawing.Point(359, 665);
            this.btOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(88, 30);
            this.btOk.TabIndex = 8;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.Location = new System.Drawing.Point(454, 664);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(88, 31);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lblGridlineColor
            // 
            this.lblGridlineColor.AutoSize = true;
            this.lblGridlineColor.Location = new System.Drawing.Point(264, 41);
            this.lblGridlineColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGridlineColor.Name = "lblGridlineColor";
            this.lblGridlineColor.Size = new System.Drawing.Size(92, 17);
            this.lblGridlineColor.TabIndex = 1;
            this.lblGridlineColor.Text = "&Gridline Color:";
            // 
            // chkShowGridlines
            // 
            this.chkShowGridlines.AutoSize = true;
            this.chkShowGridlines.Location = new System.Drawing.Point(27, 39);
            this.chkShowGridlines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkShowGridlines.Name = "chkShowGridlines";
            this.chkShowGridlines.Size = new System.Drawing.Size(113, 21);
            this.chkShowGridlines.TabIndex = 0;
            this.chkShowGridlines.Text = "S&how Gridlines";
            this.chkShowGridlines.UseVisualStyleBackColor = true;
            this.chkShowGridlines.CheckedChanged += new System.EventHandler(this.chkShowGridlines_CheckedChanged);
            // 
            // chkAlternateRowColors
            // 
            this.chkAlternateRowColors.AutoSize = true;
            this.chkAlternateRowColors.Location = new System.Drawing.Point(192, 42);
            this.chkAlternateRowColors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAlternateRowColors.Name = "chkAlternateRowColors";
            this.chkAlternateRowColors.Size = new System.Drawing.Size(161, 21);
            this.chkAlternateRowColors.TabIndex = 2;
            this.chkAlternateRowColors.Text = "&Alternating Row Colors";
            this.chkAlternateRowColors.UseVisualStyleBackColor = true;
            this.chkAlternateRowColors.CheckedChanged += new System.EventHandler(this.chkAlternateRowColors_CheckedChanged);
            // 
            // GridlineColor
            // 
            this.GridlineColor.BackColor = System.Drawing.Color.White;
            this.GridlineColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridlineColor.Location = new System.Drawing.Point(382, 39);
            this.GridlineColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GridlineColor.Name = "GridlineColor";
            this.GridlineColor.Size = new System.Drawing.Size(40, 28);
            this.GridlineColor.TabIndex = 2;
            this.GridlineColor.Click += new System.EventHandler(this.GridlineColor_Click);
            // 
            // labelFont
            // 
            this.labelFont.AutoSize = true;
            this.labelFont.Location = new System.Drawing.Point(15, 173);
            this.labelFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFont.Name = "labelFont";
            this.labelFont.Size = new System.Drawing.Size(36, 17);
            this.labelFont.TabIndex = 0;
            this.labelFont.Text = "&Font:";
            // 
            // cboFont
            // 
            this.cboFont.FormattingEnabled = true;
            this.cboFont.Location = new System.Drawing.Point(62, 168);
            this.cboFont.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboFont.Name = "cboFont";
            this.cboFont.Size = new System.Drawing.Size(359, 25);
            this.cboFont.TabIndex = 1;
            // 
            // labelFontsize
            // 
            this.labelFontsize.AutoSize = true;
            this.labelFontsize.Location = new System.Drawing.Point(437, 173);
            this.labelFontsize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFontsize.Name = "labelFontsize";
            this.labelFontsize.Size = new System.Drawing.Size(34, 17);
            this.labelFontsize.TabIndex = 2;
            this.labelFontsize.Text = "&Size:";
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(479, 167);
            this.txtFontSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(55, 25);
            this.txtFontSize.TabIndex = 3;
            this.txtFontSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 37);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "&Background Color:";
            // 
            // SelectionBackgroundColor
            // 
            this.SelectionBackgroundColor.BackColor = System.Drawing.Color.White;
            this.SelectionBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SelectionBackgroundColor.Location = new System.Drawing.Point(156, 30);
            this.SelectionBackgroundColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SelectionBackgroundColor.Name = "SelectionBackgroundColor";
            this.SelectionBackgroundColor.Size = new System.Drawing.Size(40, 28);
            this.SelectionBackgroundColor.TabIndex = 1;
            this.SelectionBackgroundColor.Click += new System.EventHandler(this.SelectionBackgroundColor_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(288, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Text Color:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SelectionTextColor);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.SelectionBackgroundColor);
            this.groupBox1.Location = new System.Drawing.Point(19, 312);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(517, 86);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selection";
            // 
            // SelectionTextColor
            // 
            this.SelectionTextColor.BackColor = System.Drawing.Color.White;
            this.SelectionTextColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SelectionTextColor.Location = new System.Drawing.Point(382, 30);
            this.SelectionTextColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SelectionTextColor.Name = "SelectionTextColor";
            this.SelectionTextColor.Size = new System.Drawing.Size(40, 28);
            this.SelectionTextColor.TabIndex = 3;
            this.SelectionTextColor.Click += new System.EventHandler(this.SelectionTextColor_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AlternateRowColor);
            this.groupBox2.Controls.Add(this.RowColor);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.chkAlternateRowColors);
            this.groupBox2.Location = new System.Drawing.Point(19, 406);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(517, 89);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Row Colors";
            // 
            // AlternateRowColor
            // 
            this.AlternateRowColor.BackColor = System.Drawing.Color.White;
            this.AlternateRowColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AlternateRowColor.Location = new System.Drawing.Point(382, 37);
            this.AlternateRowColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AlternateRowColor.Name = "AlternateRowColor";
            this.AlternateRowColor.Size = new System.Drawing.Size(40, 28);
            this.AlternateRowColor.TabIndex = 4;
            this.AlternateRowColor.Click += new System.EventHandler(this.AlternateRowColor_Click);
            // 
            // RowColor
            // 
            this.RowColor.BackColor = System.Drawing.Color.White;
            this.RowColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RowColor.Location = new System.Drawing.Point(107, 37);
            this.RowColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RowColor.Name = "RowColor";
            this.RowColor.Size = new System.Drawing.Size(40, 28);
            this.RowColor.TabIndex = 1;
            this.RowColor.Click += new System.EventHandler(this.RowColor_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 43);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "&Row Color:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblGridlineColor);
            this.groupBox3.Controls.Add(this.GridlineColor);
            this.groupBox3.Controls.Add(this.chkShowGridlines);
            this.groupBox3.Location = new System.Drawing.Point(19, 215);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(517, 89);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gridlines";
            // 
            // btLoadDefaults
            // 
            this.btLoadDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btLoadDefaults.Location = new System.Drawing.Point(51, 664);
            this.btLoadDefaults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btLoadDefaults.Name = "btLoadDefaults";
            this.btLoadDefaults.Size = new System.Drawing.Size(112, 30);
            this.btLoadDefaults.TabIndex = 7;
            this.btLoadDefaults.Text = "&Load defaults";
            this.btLoadDefaults.UseVisualStyleBackColor = true;
            this.btLoadDefaults.Click += new System.EventHandler(this.btLoadDefaults_Click);
            // 
            // chkScrollToLastRow
            // 
            this.chkScrollToLastRow.AutoSize = true;
            this.chkScrollToLastRow.Checked = true;
            this.chkScrollToLastRow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScrollToLastRow.Location = new System.Drawing.Point(45, 520);
            this.chkScrollToLastRow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkScrollToLastRow.Name = "chkScrollToLastRow";
            this.chkScrollToLastRow.Size = new System.Drawing.Size(249, 21);
            this.chkScrollToLastRow.TabIndex = 3;
            this.chkScrollToLastRow.Text = "Scroll to last row after loading project";
            this.chkScrollToLastRow.UseVisualStyleBackColor = true;
            // 
            // chkCaseInsensitive
            // 
            this.chkCaseInsensitive.AutoSize = true;
            this.chkCaseInsensitive.Location = new System.Drawing.Point(45, 562);
            this.chkCaseInsensitive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCaseInsensitive.Name = "chkCaseInsensitive";
            this.chkCaseInsensitive.Size = new System.Drawing.Size(160, 21);
            this.chkCaseInsensitive.TabIndex = 10;
            this.chkCaseInsensitive.Text = "&Case insensitive search";
            this.chkCaseInsensitive.UseVisualStyleBackColor = true;
            // 
            // chkOpenPdf
            // 
            this.chkOpenPdf.AutoSize = true;
            this.chkOpenPdf.Location = new System.Drawing.Point(45, 604);
            this.chkOpenPdf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkOpenPdf.Name = "chkOpenPdf";
            this.chkOpenPdf.Size = new System.Drawing.Size(167, 21);
            this.chkOpenPdf.TabIndex = 11;
            this.chkOpenPdf.Text = "&Open PDF after creating";
            this.chkOpenPdf.UseVisualStyleBackColor = true;
            // 
            // buttonPath
            // 
            this.buttonPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPath.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPath.FlatAppearance.BorderSize = 0;
            this.buttonPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPath.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonPath.Location = new System.Drawing.Point(188, 663);
            this.buttonPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonPath.Name = "buttonPath";
            this.buttonPath.Size = new System.Drawing.Size(38, 30);
            this.buttonPath.TabIndex = 12;
            this.buttonPath.Text = "1";
            this.toolTip1.SetToolTip(this.buttonPath, "Open user config directory");
            this.buttonPath.UseVisualStyleBackColor = false;
            this.buttonPath.Click += new System.EventHandler(this.buttonPath_Click);
            // 
            // labelPdfTitle
            // 
            this.labelPdfTitle.AutoSize = true;
            this.labelPdfTitle.Location = new System.Drawing.Point(269, 605);
            this.labelPdfTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPdfTitle.Name = "labelPdfTitle";
            this.labelPdfTitle.Size = new System.Drawing.Size(55, 17);
            this.labelPdfTitle.TabIndex = 13;
            this.labelPdfTitle.Text = "PDF title";
            // 
            // txtPdfTitle
            // 
            this.txtPdfTitle.Location = new System.Drawing.Point(356, 601);
            this.txtPdfTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPdfTitle.Name = "txtPdfTitle";
            this.txtPdfTitle.Size = new System.Drawing.Size(179, 25);
            this.txtPdfTitle.TabIndex = 14;
            this.txtPdfTitle.Text = "MiniBug issue";
            // 
            // labelIpaddress
            // 
            this.labelIpaddress.AutoSize = true;
            this.labelIpaddress.Location = new System.Drawing.Point(23, 30);
            this.labelIpaddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIpaddress.Name = "labelIpaddress";
            this.labelIpaddress.Size = new System.Drawing.Size(70, 17);
            this.labelIpaddress.TabIndex = 15;
            this.labelIpaddress.Text = "Ip address";
            // 
            // groupBoxPostgres
            // 
            this.groupBoxPostgres.Controls.Add(this.txtPassword);
            this.groupBoxPostgres.Controls.Add(this.labelPassword);
            this.groupBoxPostgres.Controls.Add(this.txtUsername);
            this.groupBoxPostgres.Controls.Add(this.labelUser);
            this.groupBoxPostgres.Controls.Add(this.txtPort);
            this.groupBoxPostgres.Controls.Add(this.labelPort);
            this.groupBoxPostgres.Controls.Add(this.txtIpaddress);
            this.groupBoxPostgres.Controls.Add(this.labelIpaddress);
            this.groupBoxPostgres.Location = new System.Drawing.Point(18, 22);
            this.groupBoxPostgres.Name = "groupBoxPostgres";
            this.groupBoxPostgres.Size = new System.Drawing.Size(516, 130);
            this.groupBoxPostgres.TabIndex = 16;
            this.groupBoxPostgres.TabStop = false;
            this.groupBoxPostgres.Text = "PostgreSQL";
            // 
            // txtIpaddress
            // 
            this.txtIpaddress.Location = new System.Drawing.Point(126, 27);
            this.txtIpaddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtIpaddress.Name = "txtIpaddress";
            this.txtIpaddress.Size = new System.Drawing.Size(179, 25);
            this.txtIpaddress.TabIndex = 17;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(370, 27);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(73, 25);
            this.txtPort.TabIndex = 19;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(326, 30);
            this.labelPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(32, 17);
            this.labelPort.TabIndex = 18;
            this.labelPort.Text = "Port";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(126, 58);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(179, 25);
            this.txtUsername.TabIndex = 21;
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(23, 61);
            this.labelUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(71, 17);
            this.labelUser.TabIndex = 20;
            this.labelUser.Text = "User name";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(125, 90);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(179, 25);
            this.txtPassword.TabIndex = 23;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(22, 93);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(64, 17);
            this.labelPassword.TabIndex = 22;
            this.labelPassword.Text = "Password";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 710);
            this.Controls.Add(this.groupBoxPostgres);
            this.Controls.Add(this.txtPdfTitle);
            this.Controls.Add(this.labelPdfTitle);
            this.Controls.Add(this.buttonPath);
            this.Controls.Add(this.chkOpenPdf);
            this.Controls.Add(this.chkCaseInsensitive);
            this.Controls.Add(this.chkScrollToLastRow);
            this.Controls.Add(this.btLoadDefaults);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtFontSize);
            this.Controls.Add(this.labelFontsize);
            this.Controls.Add(this.cboFont);
            this.Controls.Add(this.labelFont);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxPostgres.ResumeLayout(false);
            this.groupBoxPostgres.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lblGridlineColor;
        private System.Windows.Forms.CheckBox chkShowGridlines;
        private System.Windows.Forms.CheckBox chkAlternateRowColors;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label GridlineColor;
        private System.Windows.Forms.Label labelFont;
        private System.Windows.Forms.ComboBox cboFont;
        private System.Windows.Forms.Label labelFontsize;
        private System.Windows.Forms.TextBox txtFontSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label SelectionBackgroundColor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label SelectionTextColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label RowColor;
        private System.Windows.Forms.Label AlternateRowColor;
        private System.Windows.Forms.Button btLoadDefaults;
        private System.Windows.Forms.CheckBox chkScrollToLastRow;
        private System.Windows.Forms.CheckBox chkCaseInsensitive;
        private System.Windows.Forms.CheckBox chkOpenPdf;
        private System.Windows.Forms.Button buttonPath;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelPdfTitle;
        private System.Windows.Forms.TextBox txtPdfTitle;
        private System.Windows.Forms.Label labelIpaddress;
        private System.Windows.Forms.GroupBox groupBoxPostgres;
        private System.Windows.Forms.TextBox txtIpaddress;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label labelPort;
    }
}