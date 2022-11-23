﻿// Copyright(c) João Martiniano. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Npgsql;

namespace MaxiBug
{
    public partial class SettingsForm : Form
    {
        private bool projectExists;

        private string oldProjectName = string.Empty;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Suspend the layout logic for the form, while the application is initializing
            this.SuspendLayout();

            txtIpaddress.Text = Properties.Settings.Default.PostgresIpaddress;
            txtPort.Text = Properties.Settings.Default.PostgresPort.ToString();
            txtUsername.Text = Properties.Settings.Default.PostgresUser;
            txtPassword.Text = Properties.Settings.Default.PostgresPassword;

            projectExists = Program.SoftwareProject != null;
            this.groupBoxProject.Enabled = projectExists;

            if (projectExists)
            {
                oldProjectName = Program.SoftwareProject.Name;
                this.txtName.Text = Program.SoftwareProject.Name;
                this.txtDbName.Text = Program.databaseName;
                this.txtDbName.Enabled = false;
            }

            this.Font = ApplicationSettings.AppFont;

            //this.Icon = MiniBug.Properties.Resources.
            this.AcceptButton = btOk;
            this.CancelButton = btCancel;

            // Populate the font combo box, with the fonts installed in the system
            InstalledFontCollection fonts = new InstalledFontCollection();

            FontFamily[] fontFamilies = fonts.Families;

            foreach (FontFamily font in fontFamilies)
            {
                cboFont.Items.Add(font.Name);
            }

            cboFont.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboFont.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboFont.DropDownStyle = ComboBoxStyle.DropDownList;

            txtFontSize.MaxLength = 2;

            InitializeControls();

            // Initialization of the color dialog
            colorDialog1.AllowFullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.FullOpen = true;

            // Resume the layout logic
            this.ResumeLayout();
            this.CenterToScreen();

            SetAccessibilityInformation();
        }

        /// <summary>
        /// Add accessibility data to form controls.
        /// </summary>
        private void SetAccessibilityInformation()
        {
            cboFont.AccessibleDescription = "Text font used to display information in the Issues and Tasks grids";
            txtFontSize.AccessibleName = "Font size";
            txtFontSize.AccessibleDescription = "Font size for the Issues and Tasks grids";

            chkShowGridlines.AccessibleDescription = "Turn on or off gridlines in the Issues and Tasks grids";
            GridlineColor.AccessibleDescription = "Gridline color in the Issues and Tasks grids";
            SelectionBackgroundColor.AccessibleDescription = "Background color for the selected rows in the Issues and Tasks grids";
            SelectionTextColor.AccessibleDescription = "Foreground color for the selected rows in the Issues and Tasks grids";

            RowColor.AccessibleDescription = "Background color of rows in the Issues and Tasks grids";
            chkAlternateRowColors.AccessibleDescription = "Display rows of alternating colors in the Issues and Tasks grids";
            AlternateRowColor.AccessibleDescription = "Alternate row background color in the Issues and Tasks grids";

            btLoadDefaults.AccessibleDescription = "Load application default values for the settings";
        }

        /// <summary>
        /// Initialize the form controls with the current settings.
        /// </summary>
        private void InitializeControls()
        {
            Properties.Settings.Default.PostgresIpaddress = Program.postgresIpaddress;
            Properties.Settings.Default.PostgresPort = Program.postgresPort;
            Properties.Settings.Default.PostgresUser = Program.postgresUser;
            Properties.Settings.Default.PostgresPassword = Program.postgresPassword;

            // Set the default font and size
            cboFont.SelectedItem = ApplicationSettings.AppFont.FontFamily.Name;
            txtFontSize.Text = (Convert.ToInt32(ApplicationSettings.AppFont.Size)).ToString();

            // Gridlines
            chkShowGridlines.Checked = ApplicationSettings.GridShowBorders;
            lblGridlineColor.Enabled = ApplicationSettings.GridShowBorders;
            GridlineColor.BackColor = ApplicationSettings.GridBorderColor;

            // Selection
            SelectionBackgroundColor.BackColor = ApplicationSettings.GridSelectionBackColor;
            SelectionTextColor.BackColor = ApplicationSettings.GridSelectionForeColor;

            // Row colors (normal and alternating row colors)
            RowColor.BackColor = ApplicationSettings.GridRowBackColor;
            chkAlternateRowColors.Checked = ApplicationSettings.GridAlternatingRowColor;
            AlternateRowColor.Enabled = ApplicationSettings.GridAlternatingRowColor;
            AlternateRowColor.BackColor = ApplicationSettings.GridAlternateRowBackColor;

            chkScrollToLastRow.Checked = ApplicationSettings.ScrollToLastRow;
            chkCaseInsensitive.Checked = ApplicationSettings.SearchCaseInsensitive;
            chkOpenPdf.Checked = ApplicationSettings.OpenPdf;
            txtPdfTitle.Text = ApplicationSettings.PdfTitle;
            txtGitFolder.Text = ApplicationSettings.GitFolder;
            txtGitCommand.Text = ApplicationSettings.GitCommand;
        }

        #region ControlEvents
        /// <summary>
        /// Handle the KeyPress event for the font size textbox: filter any non numeric characters.
        /// </summary>
        private void txtFontSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits or the backspace character
            e.Handled = (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b') ? false : true;
        }

        /// <summary>
        /// Enable/disable gridlines.
        /// </summary>
        private void chkShowGridlines_CheckedChanged(object sender, EventArgs e)
        {
            lblGridlineColor.Enabled = chkShowGridlines.Checked;
            GridlineColor.Enabled = chkShowGridlines.Checked;
        }

        /// <summary>
        /// Select the color for the GridBorderColor setting.
        /// </summary>
        private void GridlineColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = GridlineColor.BackColor;
            colorDialog1.ShowDialog();
            GridlineColor.BackColor = colorDialog1.Color;
        }

        /// <summary>
        /// Select the color for the GridSelectionBackColor setting.
        /// </summary>
        private void SelectionBackgroundColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = SelectionBackgroundColor.BackColor;
            colorDialog1.ShowDialog();
            SelectionBackgroundColor.BackColor = colorDialog1.Color;
        }

        /// <summary>
        /// Select the color for the GridSelectionForeColor setting.
        /// </summary>
        private void SelectionTextColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = SelectionTextColor.BackColor;
            colorDialog1.ShowDialog();
            SelectionTextColor.BackColor = colorDialog1.Color;
        }

        /// <summary>
        /// Select the color for the GridRowBackColor setting.
        /// </summary>
        private void RowColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = RowColor.BackColor;
            colorDialog1.ShowDialog();
            RowColor.BackColor = colorDialog1.Color;
        }

        /// <summary>
        /// Enable/disable alternate row background colors.
        /// </summary>
        private void chkAlternateRowColors_CheckedChanged(object sender, EventArgs e)
        {
            AlternateRowColor.Enabled = chkAlternateRowColors.Checked;
        }

        /// <summary>
        /// Select the color for the GridAlternateRowBackColor setting.
        /// </summary>
        private void AlternateRowColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = AlternateRowColor.BackColor;
            colorDialog1.ShowDialog();
            AlternateRowColor.BackColor = colorDialog1.Color;
        }
        #endregion

        /// <summary>
        /// Load default settings.
        /// </summary>
        private void btLoadDefaults_Click(object sender, EventArgs e)
        {
            // Ask for confirmation before proceeding
            string message = "Are you sure you want to load the default settings?\nYour settings definitions will be discarded.";

            if (MessageBox.Show(message, "Load Default Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
            {
                ApplicationSettings.SetDefaultValues();

                // Update the user interface
                InitializeControls();
            }
        }

        /// <summary>
        /// Close the form and save the selected settings.
        /// </summary>
        private void btOk_Click(object sender, EventArgs e)
        {
            Program.postgresIpaddress = txtIpaddress.Text;
            Program.postgresPort = int.Parse(txtPort.Text);
            Program.postgresUser = txtUsername.Text;
            Program.postgresPassword = txtPassword.Text;
            Debug.Print("Postgres user = " + Program.postgresUser);

            if (oldProjectName != this.txtName.Text)
            {
                // Save changed project name
                Program.SoftwareProject.Name = this.txtName.Text;
            }

            // Font and font size
            float size = 0;

            // Validate the font size
            if ((!float.TryParse(txtFontSize.Text, out size)) || (txtFontSize.Text == string.Empty))
            {
                // If not valid, revert to the default font size
                size = ApplicationSettings.AppFont.Size;
            }

            // Limit font size to maximum 20
            if (size > 20)
            {
                size = ApplicationSettings.AppFont.Size;
            }

            ApplicationSettings.AppFont = new Font(cboFont.SelectedItem.ToString(), size);
            this.Font = ApplicationSettings.AppFont;

            // Gridlines
            ApplicationSettings.GridShowBorders = chkShowGridlines.Checked;
            ApplicationSettings.GridBorderColor = GridlineColor.BackColor;

            // Selection
            ApplicationSettings.GridSelectionBackColor = SelectionBackgroundColor.BackColor;
            ApplicationSettings.GridSelectionForeColor = SelectionTextColor.BackColor;

            // Row colors (normal and alternating row colors)
            ApplicationSettings.GridRowBackColor = RowColor.BackColor;
            ApplicationSettings.GridAlternatingRowColor = chkAlternateRowColors.Checked;
            ApplicationSettings.GridAlternateRowBackColor = AlternateRowColor.BackColor;

            ApplicationSettings.ScrollToLastRow = chkScrollToLastRow.Checked;
            ApplicationSettings.SearchCaseInsensitive = chkCaseInsensitive.Checked;
            ApplicationSettings.OpenPdf = chkOpenPdf.Checked;
            ApplicationSettings.PdfTitle = txtPdfTitle.Text;
            ApplicationSettings.GitFolder = txtGitFolder.Text;
            ApplicationSettings.GitCommand = txtGitCommand.Text;

            // Persist the new settings
            ApplicationSettings.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancel all changes to settings and close the form.
        /// </summary>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Open the directory with the user config, e.g. C:\Users\John.Doe\AppData\Local
        /// </summary>
        private void buttonPath_Click(object sender, EventArgs e)
        {
            var pathUserConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            Debug.Print(pathUserConfig);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = Path.GetDirectoryName(pathUserConfig),
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }

        /// <summary>
        /// Toggle password char.
        /// </summary>
        private void buttonEye_Click(object sender, EventArgs e)
        {
            if (this.txtPassword.PasswordChar == '●')
            {
                this.txtPassword.PasswordChar = '\0';
            }
            else
            {
                this.txtPassword.PasswordChar = '●';
            }
        }

        /// <summary>
        /// Test connection with Postgres server.
        /// </summary>
        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                Program.postgresIpaddress = txtIpaddress.Text;
                Program.postgresPort = int.Parse(txtPort.Text);
                Program.postgresUser = txtUsername.Text;
                Program.postgresPassword = txtPassword.Text;
                Debug.Print("Postgres user = " + Program.postgresUser);

                if (Database.TestConnection())
                {
                    this.labelTestConn.Text = "Successfully connected to Postgres server !";
                    this.labelTestConn.ForeColor = Color.Black;
                    this.labelTestConn.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            this.labelTestConn.Text = "Could not connect to Postgres server !";
            this.labelTestConn.ForeColor = Color.Red;
            this.labelTestConn.Visible = true;
        }

        /// <summary>
        /// Select the default Git working copy folder.
        /// </summary>
        private void buttonGitFolder_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowNewFolderButton = false;

            if (string.IsNullOrEmpty(txtGitFolder.Text))
            {
                txtGitFolder.Text = Environment.SpecialFolder.MyDocuments.ToString();
            }

            folderBrowserDialog1.SelectedPath = txtGitFolder.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtGitFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
