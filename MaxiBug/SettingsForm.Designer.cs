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
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtIpaddress = new System.Windows.Forms.TextBox();
            this.txtDbName = new System.Windows.Forms.TextBox();
            this.labelPdfTitle = new System.Windows.Forms.Label();
            this.txtPdfTitle = new System.Windows.Forms.TextBox();
            this.labelIpaddress = new System.Windows.Forms.Label();
            this.groupBoxPostgres = new System.Windows.Forms.GroupBox();
            this.labelTestConn = new System.Windows.Forms.Label();
            this.buttonTestConnection = new System.Windows.Forms.Button();
            this.buttonEye = new System.Windows.Forms.Button();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageConnection = new System.Windows.Forms.TabPage();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.groupBoxProject = new System.Windows.Forms.GroupBox();
            this.labelDbName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageUi = new System.Windows.Forms.TabPage();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxPostgres.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageConnection.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.groupBoxProject.SuspendLayout();
            this.tabPageUi.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOk.Location = new System.Drawing.Point(368, 414);
            this.btOk.Margin = new System.Windows.Forms.Padding(4);
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
            this.btCancel.Location = new System.Drawing.Point(463, 413);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(88, 31);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lblGridlineColor
            // 
            this.lblGridlineColor.Location = new System.Drawing.Point(264, 32);
            this.lblGridlineColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGridlineColor.Name = "lblGridlineColor";
            this.lblGridlineColor.Size = new System.Drawing.Size(110, 17);
            this.lblGridlineColor.TabIndex = 1;
            this.lblGridlineColor.Text = "&Gridline Color:";
            // 
            // chkShowGridlines
            // 
            this.chkShowGridlines.Location = new System.Drawing.Point(27, 30);
            this.chkShowGridlines.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowGridlines.Name = "chkShowGridlines";
            this.chkShowGridlines.Size = new System.Drawing.Size(166, 21);
            this.chkShowGridlines.TabIndex = 0;
            this.chkShowGridlines.Text = "S&how Gridlines";
            this.chkShowGridlines.UseVisualStyleBackColor = true;
            this.chkShowGridlines.CheckedChanged += new System.EventHandler(this.chkShowGridlines_CheckedChanged);
            // 
            // chkAlternateRowColors
            // 
            this.chkAlternateRowColors.Location = new System.Drawing.Point(206, 35);
            this.chkAlternateRowColors.Margin = new System.Windows.Forms.Padding(4);
            this.chkAlternateRowColors.Name = "chkAlternateRowColors";
            this.chkAlternateRowColors.Size = new System.Drawing.Size(166, 23);
            this.chkAlternateRowColors.TabIndex = 2;
            this.chkAlternateRowColors.Text = "&Alternating Row Colors";
            this.chkAlternateRowColors.UseVisualStyleBackColor = true;
            this.chkAlternateRowColors.CheckedChanged += new System.EventHandler(this.chkAlternateRowColors_CheckedChanged);
            // 
            // GridlineColor
            // 
            this.GridlineColor.BackColor = System.Drawing.Color.White;
            this.GridlineColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridlineColor.Location = new System.Drawing.Point(382, 30);
            this.GridlineColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GridlineColor.Name = "GridlineColor";
            this.GridlineColor.Size = new System.Drawing.Size(40, 28);
            this.GridlineColor.TabIndex = 2;
            this.GridlineColor.Click += new System.EventHandler(this.GridlineColor_Click);
            // 
            // labelFont
            // 
            this.labelFont.AutoSize = true;
            this.labelFont.Location = new System.Drawing.Point(12, 31);
            this.labelFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFont.Name = "labelFont";
            this.labelFont.Size = new System.Drawing.Size(36, 17);
            this.labelFont.TabIndex = 0;
            this.labelFont.Text = "&Font:";
            // 
            // cboFont
            // 
            this.cboFont.FormattingEnabled = true;
            this.cboFont.Location = new System.Drawing.Point(59, 26);
            this.cboFont.Margin = new System.Windows.Forms.Padding(4);
            this.cboFont.Name = "cboFont";
            this.cboFont.Size = new System.Drawing.Size(339, 25);
            this.cboFont.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboFont, "The font to use in all MaxiBug forms");
            // 
            // labelFontsize
            // 
            this.labelFontsize.AutoSize = true;
            this.labelFontsize.Location = new System.Drawing.Point(420, 31);
            this.labelFontsize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFontsize.Name = "labelFontsize";
            this.labelFontsize.Size = new System.Drawing.Size(34, 17);
            this.labelFontsize.TabIndex = 2;
            this.labelFontsize.Text = "&Size:";
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(473, 26);
            this.txtFontSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(55, 25);
            this.txtFontSize.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtFontSize, "Recommended sizes 8 to 14 points");
            this.txtFontSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(23, 37);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 21);
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
            this.label7.Location = new System.Drawing.Point(264, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 21);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Text Color:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SelectionTextColor);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.SelectionBackgroundColor);
            this.groupBox1.Location = new System.Drawing.Point(18, 156);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(510, 73);
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
            this.groupBox2.Location = new System.Drawing.Point(18, 249);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(510, 73);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Row Colors";
            // 
            // AlternateRowColor
            // 
            this.AlternateRowColor.BackColor = System.Drawing.Color.White;
            this.AlternateRowColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AlternateRowColor.Location = new System.Drawing.Point(382, 30);
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
            this.RowColor.Location = new System.Drawing.Point(107, 30);
            this.RowColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RowColor.Name = "RowColor";
            this.RowColor.Size = new System.Drawing.Size(40, 28);
            this.RowColor.TabIndex = 1;
            this.RowColor.Click += new System.EventHandler(this.RowColor_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(23, 36);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 22);
            this.label9.TabIndex = 0;
            this.label9.Text = "&Row Color:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblGridlineColor);
            this.groupBox3.Controls.Add(this.GridlineColor);
            this.groupBox3.Controls.Add(this.chkShowGridlines);
            this.groupBox3.Location = new System.Drawing.Point(18, 72);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(510, 73);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gridlines";
            // 
            // btLoadDefaults
            // 
            this.btLoadDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btLoadDefaults.Location = new System.Drawing.Point(60, 413);
            this.btLoadDefaults.Margin = new System.Windows.Forms.Padding(4);
            this.btLoadDefaults.Name = "btLoadDefaults";
            this.btLoadDefaults.Size = new System.Drawing.Size(112, 30);
            this.btLoadDefaults.TabIndex = 7;
            this.btLoadDefaults.Text = "&Load defaults";
            this.btLoadDefaults.UseVisualStyleBackColor = true;
            this.btLoadDefaults.Click += new System.EventHandler(this.btLoadDefaults_Click);
            // 
            // chkScrollToLastRow
            // 
            this.chkScrollToLastRow.Checked = true;
            this.chkScrollToLastRow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScrollToLastRow.Location = new System.Drawing.Point(27, 39);
            this.chkScrollToLastRow.Margin = new System.Windows.Forms.Padding(4);
            this.chkScrollToLastRow.Name = "chkScrollToLastRow";
            this.chkScrollToLastRow.Size = new System.Drawing.Size(489, 21);
            this.chkScrollToLastRow.TabIndex = 3;
            this.chkScrollToLastRow.Text = "Scroll to last row after loading project";
            this.chkScrollToLastRow.UseVisualStyleBackColor = true;
            // 
            // chkCaseInsensitive
            // 
            this.chkCaseInsensitive.Location = new System.Drawing.Point(27, 81);
            this.chkCaseInsensitive.Margin = new System.Windows.Forms.Padding(4);
            this.chkCaseInsensitive.Name = "chkCaseInsensitive";
            this.chkCaseInsensitive.Size = new System.Drawing.Size(253, 21);
            this.chkCaseInsensitive.TabIndex = 10;
            this.chkCaseInsensitive.Text = "&Case insensitive search";
            this.chkCaseInsensitive.UseVisualStyleBackColor = true;
            // 
            // chkOpenPdf
            // 
            this.chkOpenPdf.Location = new System.Drawing.Point(27, 123);
            this.chkOpenPdf.Margin = new System.Windows.Forms.Padding(4);
            this.chkOpenPdf.Name = "chkOpenPdf";
            this.chkOpenPdf.Size = new System.Drawing.Size(209, 22);
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
            this.buttonPath.Location = new System.Drawing.Point(197, 412);
            this.buttonPath.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPath.Name = "buttonPath";
            this.buttonPath.Size = new System.Drawing.Size(38, 30);
            this.buttonPath.TabIndex = 12;
            this.buttonPath.Text = "1";
            this.toolTip1.SetToolTip(this.buttonPath, "Open user config directory");
            this.buttonPath.UseVisualStyleBackColor = false;
            this.buttonPath.Click += new System.EventHandler(this.buttonPath_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(125, 90);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(179, 25);
            this.txtPassword.TabIndex = 23;
            this.txtPassword.Text = "abc123";
            this.toolTip1.SetToolTip(this.txtPassword, "Use normal characters and underscores only");
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(126, 58);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(179, 25);
            this.txtUsername.TabIndex = 21;
            this.toolTip1.SetToolTip(this.txtUsername, "Use normal characters and underscores only");
            // 
            // txtIpaddress
            // 
            this.txtIpaddress.Location = new System.Drawing.Point(126, 27);
            this.txtIpaddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtIpaddress.Name = "txtIpaddress";
            this.txtIpaddress.Size = new System.Drawing.Size(179, 25);
            this.txtIpaddress.TabIndex = 17;
            this.toolTip1.SetToolTip(this.txtIpaddress, "The ip address or URI for the Postgres server");
            // 
            // txtDbName
            // 
            this.txtDbName.Location = new System.Drawing.Point(132, 76);
            this.txtDbName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDbName.MaxLength = 50;
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(334, 25);
            this.txtDbName.TabIndex = 17;
            this.toolTip1.SetToolTip(this.txtDbName, "Use normal characters and underscores only");
            // 
            // labelPdfTitle
            // 
            this.labelPdfTitle.Location = new System.Drawing.Point(251, 124);
            this.labelPdfTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPdfTitle.Name = "labelPdfTitle";
            this.labelPdfTitle.Size = new System.Drawing.Size(79, 21);
            this.labelPdfTitle.TabIndex = 13;
            this.labelPdfTitle.Text = "PDF title";
            // 
            // txtPdfTitle
            // 
            this.txtPdfTitle.Location = new System.Drawing.Point(338, 120);
            this.txtPdfTitle.Margin = new System.Windows.Forms.Padding(4);
            this.txtPdfTitle.Name = "txtPdfTitle";
            this.txtPdfTitle.Size = new System.Drawing.Size(179, 25);
            this.txtPdfTitle.TabIndex = 14;
            this.txtPdfTitle.Text = "MiniBug issue";
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
            // groupBoxPostgres
            // 
            this.groupBoxPostgres.Controls.Add(this.labelTestConn);
            this.groupBoxPostgres.Controls.Add(this.buttonTestConnection);
            this.groupBoxPostgres.Controls.Add(this.buttonEye);
            this.groupBoxPostgres.Controls.Add(this.txtPassword);
            this.groupBoxPostgres.Controls.Add(this.labelPassword);
            this.groupBoxPostgres.Controls.Add(this.txtUsername);
            this.groupBoxPostgres.Controls.Add(this.labelUser);
            this.groupBoxPostgres.Controls.Add(this.txtPort);
            this.groupBoxPostgres.Controls.Add(this.labelPort);
            this.groupBoxPostgres.Controls.Add(this.txtIpaddress);
            this.groupBoxPostgres.Controls.Add(this.labelIpaddress);
            this.groupBoxPostgres.Location = new System.Drawing.Point(19, 32);
            this.groupBoxPostgres.Name = "groupBoxPostgres";
            this.groupBoxPostgres.Size = new System.Drawing.Size(557, 203);
            this.groupBoxPostgres.TabIndex = 16;
            this.groupBoxPostgres.TabStop = false;
            this.groupBoxPostgres.Text = "PostgreSQL";
            // 
            // labelTestConn
            // 
            this.labelTestConn.Location = new System.Drawing.Point(272, 138);
            this.labelTestConn.Name = "labelTestConn";
            this.labelTestConn.Size = new System.Drawing.Size(273, 53);
            this.labelTestConn.TabIndex = 25;
            this.labelTestConn.Text = "Could not connect to Postgres server !";
            this.labelTestConn.Visible = false;
            // 
            // buttonTestConnection
            // 
            this.buttonTestConnection.Location = new System.Drawing.Point(126, 133);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new System.Drawing.Size(132, 31);
            this.buttonTestConnection.TabIndex = 17;
            this.buttonTestConnection.Text = "Test Connection";
            this.buttonTestConnection.UseVisualStyleBackColor = true;
            this.buttonTestConnection.Click += new System.EventHandler(this.buttonTestConnection_Click);
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
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(22, 93);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(96, 17);
            this.labelPassword.TabIndex = 22;
            this.labelPassword.Text = "Password";
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageConnection);
            this.tabControl1.Controls.Add(this.tabPageProject);
            this.tabControl1.Controls.Add(this.tabPageUi);
            this.tabControl1.Controls.Add(this.tabPageMisc);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(606, 391);
            this.tabControl1.TabIndex = 17;
            // 
            // tabPageConnection
            // 
            this.tabPageConnection.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageConnection.Controls.Add(this.groupBoxPostgres);
            this.tabPageConnection.Location = new System.Drawing.Point(4, 26);
            this.tabPageConnection.Name = "tabPageConnection";
            this.tabPageConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConnection.Size = new System.Drawing.Size(598, 361);
            this.tabPageConnection.TabIndex = 0;
            this.tabPageConnection.Text = "Connection";
            // 
            // tabPageProject
            // 
            this.tabPageProject.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageProject.Controls.Add(this.groupBoxProject);
            this.tabPageProject.Location = new System.Drawing.Point(4, 26);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProject.Size = new System.Drawing.Size(598, 361);
            this.tabPageProject.TabIndex = 3;
            this.tabPageProject.Text = "Project";
            // 
            // groupBoxProject
            // 
            this.groupBoxProject.Controls.Add(this.txtDbName);
            this.groupBoxProject.Controls.Add(this.labelDbName);
            this.groupBoxProject.Controls.Add(this.txtName);
            this.groupBoxProject.Controls.Add(this.label1);
            this.groupBoxProject.Location = new System.Drawing.Point(19, 21);
            this.groupBoxProject.Name = "groupBoxProject";
            this.groupBoxProject.Size = new System.Drawing.Size(564, 144);
            this.groupBoxProject.TabIndex = 0;
            this.groupBoxProject.TabStop = false;
            this.groupBoxProject.Text = "Project details";
            // 
            // labelDbName
            // 
            this.labelDbName.AutoSize = true;
            this.labelDbName.Location = new System.Drawing.Point(22, 83);
            this.labelDbName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDbName.Name = "labelDbName";
            this.labelDbName.Size = new System.Drawing.Size(102, 17);
            this.labelDbName.TabIndex = 16;
            this.labelDbName.Text = "Database name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(132, 37);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(334, 25);
            this.txtName.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Project name:";
            // 
            // tabPageUi
            // 
            this.tabPageUi.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUi.Controls.Add(this.labelFont);
            this.tabPageUi.Controls.Add(this.cboFont);
            this.tabPageUi.Controls.Add(this.labelFontsize);
            this.tabPageUi.Controls.Add(this.txtFontSize);
            this.tabPageUi.Controls.Add(this.groupBox1);
            this.tabPageUi.Controls.Add(this.groupBox2);
            this.tabPageUi.Controls.Add(this.groupBox3);
            this.tabPageUi.Location = new System.Drawing.Point(4, 26);
            this.tabPageUi.Name = "tabPageUi";
            this.tabPageUi.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUi.Size = new System.Drawing.Size(598, 361);
            this.tabPageUi.TabIndex = 1;
            this.tabPageUi.Text = "User Interface";
            // 
            // tabPageMisc
            // 
            this.tabPageMisc.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageMisc.Controls.Add(this.chkScrollToLastRow);
            this.tabPageMisc.Controls.Add(this.txtPdfTitle);
            this.tabPageMisc.Controls.Add(this.chkCaseInsensitive);
            this.tabPageMisc.Controls.Add(this.labelPdfTitle);
            this.tabPageMisc.Controls.Add(this.chkOpenPdf);
            this.tabPageMisc.Location = new System.Drawing.Point(4, 26);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMisc.Size = new System.Drawing.Size(598, 361);
            this.tabPageMisc.TabIndex = 2;
            this.tabPageMisc.Text = "Misc";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 456);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonPath);
            this.Controls.Add(this.btLoadDefaults);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBoxPostgres.ResumeLayout(false);
            this.groupBoxPostgres.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageConnection.ResumeLayout(false);
            this.tabPageProject.ResumeLayout(false);
            this.groupBoxProject.ResumeLayout(false);
            this.groupBoxProject.PerformLayout();
            this.tabPageUi.ResumeLayout(false);
            this.tabPageUi.PerformLayout();
            this.tabPageMisc.ResumeLayout(false);
            this.tabPageMisc.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Button buttonEye;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageConnection;
        private System.Windows.Forms.TabPage tabPageUi;
        private System.Windows.Forms.TabPage tabPageMisc;
        private System.Windows.Forms.Label labelTestConn;
        private System.Windows.Forms.Button buttonTestConnection;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.GroupBox groupBoxProject;
        private System.Windows.Forms.TextBox txtDbName;
        private System.Windows.Forms.Label labelDbName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
    }
}