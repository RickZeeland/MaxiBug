// Copyright(c) João Martiniano. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows.Forms;

namespace MiniBug
{
    /// <summary>
    /// Create a new project, or edit existing project settings.
    /// </summary>
    public partial class ProjectForm : Form
    {
        /// <summary>
        /// Gets the current operation.
        /// </summary>
        public MiniBug.OperationType Operation { get; private set; } = OperationType.None;

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        public string ProjectName { get; private set; } = string.Empty;

        /// <summary>
        /// Create a new project, or edit existing project settings.
        /// </summary>
        public ProjectForm(OperationType operation, string projectName = "")
        {
            InitializeComponent();
            this.Font = ApplicationSettings.AppFont;
            Operation = operation;

            if (Operation == OperationType.Edit)
            {
                // Edit existing project settings
                ProjectName = projectName;
            }
        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            // Suspend the layout logic for the form, while the application is initializing
            this.SuspendLayout();

            // Make initializations based on the type of operation
            if (Operation == OperationType.New)
            {
                this.Text = "New Project";
                lblFormTitle.Text = "Create a new project";
                btOk.Enabled = false;
            }
            else if (Operation == OperationType.Edit)
            {
                this.Text = "Edit Project";
                lblFormTitle.Text = "Edit the current project";

                // Populate the controls
                txtName.Text = ProjectName;
            }

            // Resume the layout logic
            this.ResumeLayout();
        }

        /// <summary>
        /// Close the form.
        /// </summary>
        private void btOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                ProjectName = txtName.Text;

                //if (File.Exists(fullFilename))
                //{
                //    MessageBox.Show($"{ProjectFilename} already exists!\nPlease choose another name.", Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Cancel this operation and close the form.
        /// </summary>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Handle the TextChanged event for the project name textbox control.
        /// </summary>
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btOk.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
        }
    }
}
