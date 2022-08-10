﻿// Copyright(c) João Martiniano. All rights reserved.
// Forked by RickZeeland.
// Create single exe with Fody.Costura https://github.com/Fody/Costura
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MaxiBug
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// If true, signals that the issues grid is initializing.
        /// </summary>
        private bool initializingGridIssues = false;

        /// <summary>
        /// If true, signals that the tasks grid is initializing.
        /// </summary>
        private bool initializingGridTasks = false;

        /// <summary>
        /// Do not show closed and resolved issues when false.
        /// </summary>
        public bool ShowClosedIssues { get; set; }

        /// <summary>
        /// Database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Start by retrieving the application settings
                ApplicationSettings.Load();

                this.Font = ApplicationSettings.AppFont;
                Point startPosition = Properties.Settings.Default.FormStartPosition;
                Size formSize = Properties.Settings.Default.FormSize;

                // Suspend the layout logic for the form, while the application is initializing
                this.SuspendLayout();

                this.Icon = Properties.Resources.Minibug;
                this.Text = Program.myName;
                this.MinimumSize = new Size(478, 303);
                //this.modernPieChart1.Size = new Size(400, 300);                     // Do not scale Pie chart with font size
                this.panelPie.Location = new Point(this.Width - 600, this.Height - 400);

                Program.postgresIpaddress = Properties.Settings.Default.PostgresIpaddress;
                Program.postgresPort = Properties.Settings.Default.PostgresPort;
                Program.postgresUser = Properties.Settings.Default.PostgresUser;
                Program.postgresPassword = Properties.Settings.Default.PostgresPassword;

                // Initialization of the Issues and Tasks grids
                InitializeGridIssues();
                InitializeGridTasks();

                // Apply the settings to the Issues and Tasks grids
                ApplySettingsToGrids();
                ShowClosedIssues = true;
                SetControlsState();

                // Initialize the recent projects submenu
                InitializeRecentProjects();
                LoadRecentProject();

                if (Font.Size > 12)
                {
                    // Make sure entire grid is visible
                    this.TabControl.Height -= ((int)Font.Size - 12) * 25;
                }

                // Resume the layout logic
                this.ResumeLayout();

                if (startPosition.X > 0)
                {
                    this.Location = startPosition;
                    this.Size = formSize;
                }
                else
                {
                    this.CenterToScreen();
                }

                // Set the sort glyph for the issues and tasks DataGridViews
                SetGridSortGlyph(GridType.All);

                SetAccessibilityInformation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Load project into grid (when there is only one project)
        /// </summary>
        private void LoadRecentProject()
        {
            bool result = true;

            try
            {
                this.panelPie.Visible = false;
                int projectsCount = Properties.Settings.Default.RecentProjectsNames.Count;

                if (projectsCount == 1)
                {
                    // Load the only project immediately
                    string projectName = Properties.Settings.Default.RecentProjectsNames[0];
                    string dbName = Properties.Settings.Default.RecentProjectsPaths[0];
                    result = OpenProject(projectName, dbName);

                    if (result)
                    {
                        this.tabPage1.Invalidate();

                        if (this.GridIssues.RowCount > 0)
                        {
                            ShowPieChart();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                result = false;
            }

            if (!result)
            {
                MessageBox.Show("Could not load recent project", Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearRecentProjects();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Add accessibility data to form controls.
        /// </summary>
        private void SetAccessibilityInformation()
        {
            GridIssues.AccessibleName = "Issues Grid";
            GridIssues.AccessibleDescription = "Grid with the project issues";
            GridTasks.AccessibleName = "Tasks Grid";
            GridTasks.AccessibleDescription = "Grid with the project tasks";
        }

        /// <summary>
        /// Initialize the recent projects submenu.
        /// </summary>
        private void InitializeRecentProjects()
        {
            bool flagNoRecentProjects = true;

            if ((Properties.Settings.Default.RecentProjectsNames == null) || (Properties.Settings.Default.RecentProjectsPaths == null))
            {
                Properties.Settings.Default.RecentProjectsNames = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.RecentProjectsPaths = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.Save();
            }
            else if ((Properties.Settings.Default.RecentProjectsNames.Count > 0) && (Properties.Settings.Default.RecentProjectsPaths.Count > 0))
            {
                // Load the recent projects from the application settings and insert them in the recent projects submenu
                for (int i = 0, n = Properties.Settings.Default.RecentProjectsNames.Count - 1; i <= n; ++i)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(Properties.Settings.Default.RecentProjectsNames[i])
                    {
                        Tag = Properties.Settings.Default.RecentProjectsPaths[i]        // Database name
                    };
                    recentProjectsToolStripMenuItem.DropDownItems.Add(item);

                    // Add an event handler for the new menu item
                    item.Click += new System.EventHandler(this.FileMenuRecentProjectItem_Click);
                }

                flagNoRecentProjects = false;
            }

            // If there are no recent projects, disable menu items
            if (flagNoRecentProjects)
            {
                // Disable the recent projects menu item
                recentProjectsToolStripMenuItem.Enabled = false;

                // Disable the clear recent projects list menu item
                clearRecentProjectsListToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Enable/disable controls when the user changes tabs.
        /// </summary>
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlsState();
        }

        /// <summary>
        /// Sets the state (enabled/disabled) of some controls: menu items, toolbar icons, Pie chart, etc.
        /// </summary>
        private void SetControlsState()
        {
            // The user can edit the project settings only if the current project != null
            bool projectExists = Program.SoftwareProject != null;

            editProjectToolStripMenuItem.Enabled = projectExists;

            // The user can export the project only if the current project != null AND there are issues OR tasks
            exportToolStripMenuItem.Enabled = false;

            if (projectExists)
            {
                if (((Program.SoftwareProject.Issues != null) && (Program.SoftwareProject.Issues.Count > 0)) ||
                    ((Program.SoftwareProject.Tasks != null) && (Program.SoftwareProject.Tasks.Count > 0)))
                {
                    exportToolStripMenuItem.Enabled = true;
                }
            }
            
            if (TabControl.SelectedIndex == 0)          // The tab 'Issues' is selected
            {
                // The user can create a new issue only if the current project != null
                newIssueToolStripMenuItem.Enabled = IconNewIssue.Enabled = projectExists;

                if (projectExists && (Program.SoftwareProject.Issues != null) && (GridIssues.Rows.Count > 0))
                {
                    // ENABLE these controls if there are issues
                    editIssueToolStripMenuItem.Enabled = true;
                    deleteIssueToolStripMenuItem.Enabled = true;
                    cloneIssueToolStripMenuItem.Enabled = true;
                    IconEditIssue.Enabled = true;
                    IconDeleteIssue.Enabled = true;
                    IconCloneIssue.Enabled = true;
                    IconPieChart.Enabled = true;
                    IconShowClosed.Enabled = true;
                }
                else
                {
                    // DISABLE these controls if there are no issues
                    editIssueToolStripMenuItem.Enabled = false;
                    deleteIssueToolStripMenuItem.Enabled = false;
                    cloneIssueToolStripMenuItem.Enabled = false;
                    IconEditIssue.Enabled = false;
                    IconDeleteIssue.Enabled = false;
                    IconCloneIssue.Enabled = false;
                    IconPieChart.Enabled = false;
                    IconShowClosed.Enabled = false;
                }

                // Disable tasks menu items
                newTaskToolStripMenuItem.Enabled = false;
                editTaskToolStripMenuItem.Enabled = false;
                deleteTaskToolStripMenuItem.Enabled = false;
                cloneTaskToolStripMenuItem.Enabled = false;

                // Disable tasks toolbar icons
                IconNewTask.Enabled = false;
                IconEditTask.Enabled = false;
                IconDeleteTask.Enabled = false;
                IconCloneTask.Enabled = false;
            }
            else if (TabControl.SelectedIndex == 1)         // The tab 'Tasks' is selected
            {
                // The user can create a new task only if the current project != null
                newTaskToolStripMenuItem.Enabled = IconNewTask.Enabled = projectExists;

                if (projectExists && (Program.SoftwareProject.Tasks != null) && (GridTasks.Rows.Count > 0))
                {
                    // ENABLE these controls if there are tasks
                    editTaskToolStripMenuItem.Enabled = true;
                    deleteTaskToolStripMenuItem.Enabled = true;
                    cloneTaskToolStripMenuItem.Enabled = true;
                    IconEditTask.Enabled = true;
                    IconDeleteTask.Enabled = true;
                    IconCloneTask.Enabled = true;
                }
                else
                {
                    // DISABLE these controls if there are no issues
                    editTaskToolStripMenuItem.Enabled = false;
                    deleteTaskToolStripMenuItem.Enabled = false;
                    cloneTaskToolStripMenuItem.Enabled = false;
                    IconEditTask.Enabled = false;
                    IconDeleteTask.Enabled = false;
                    IconCloneTask.Enabled = false;
                }

                // Pie chart is only for issues at the moment
                IconPieChart.Enabled = false;
                panelPie.Visible = false;

                // Disable issues menu items
                newIssueToolStripMenuItem.Enabled = false;
                editIssueToolStripMenuItem.Enabled = false;
                deleteIssueToolStripMenuItem.Enabled = false;
                cloneIssueToolStripMenuItem.Enabled = false;

                // Disable issues toolbar icons
                IconNewIssue.Enabled = false;
                IconEditIssue.Enabled = false;
                IconDeleteIssue.Enabled = false;
                IconCloneIssue.Enabled = false;
                IconShowClosed.Enabled = false;
            }
        }

        /// <summary>
        /// Handle closing of the main form.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseApplication();
            e.Cancel = false;
        }

        /// <summary>
        /// Perform cleanup or final operations when the application is about to close.
        /// </summary>
        private void CloseApplication()
        {
            // Remember the last form position
            Properties.Settings.Default.FormStartPosition = this.Location;
            Properties.Settings.Default.FormSize = this.Size;

            // Save the order of the columns in the issues and tasks DataGridViews
            ApplicationSettings.Save(ApplicationSettings.SaveSettings.ColumnOrderSort);
        }

        #region "RecentProject"
        /// <summary>
        /// Add a project to the recent projects submenu and to the application settings
        /// </summary>
        /// <param name="projectName">The project name</param>
        /// <param name="dbName">The database name in lower case</param>
        private void AddRecentProject(string projectName, string dbName)
        {
            recentProjectsToolStripMenuItem.Enabled = true;
            clearRecentProjectsListToolStripMenuItem.Enabled = true;

            // Determine if the project already exists in the Recent Projects submenu
            for (int i = 0, n = recentProjectsToolStripMenuItem.DropDownItems.Count; i < n; ++i)
            {
                if (recentProjectsToolStripMenuItem.DropDownItems[i].Text == projectName)
                {
                    // If the project is already at the top of the list, do nothing
                    if (i == 0)
                    {
                        return;
                    }

                    // Remove the item from its current position
                    recentProjectsToolStripMenuItem.DropDownItems.RemoveAt(i);
                    Properties.Settings.Default.RecentProjectsNames.RemoveAt(i);
                    Properties.Settings.Default.RecentProjectsPaths.RemoveAt(i);

                    // Insert at the top of the list
                    AddRecentProjectItem(projectName, dbName);

                    return;
                }
            }

            // Remove the last item in the menu when the maximum number of items is reached
            if (recentProjectsToolStripMenuItem.DropDownItems.Count == ApplicationSettings.MaxRecentProjects)
            {
                recentProjectsToolStripMenuItem.DropDownItems.RemoveAt(recentProjectsToolStripMenuItem.DropDownItems.Count - 1);

                Properties.Settings.Default.RecentProjectsNames.RemoveAt(Properties.Settings.Default.RecentProjectsNames.Count - 1);
                Properties.Settings.Default.RecentProjectsPaths.RemoveAt(Properties.Settings.Default.RecentProjectsPaths.Count - 1);
            }

            // Add the new menu item to the top of the submenu
            AddRecentProjectItem(projectName, dbName);
        }

        /// <summary>
        /// Add the recent project item details to the top of the submenu and to the application settings.
        /// </summary>
        /// <param name="projectName">The project name</param>
        /// <param name="dbName">The database name in lower case</param>
        private void AddRecentProjectItem(string projectName, string dbName)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(projectName)
            {
                Tag = dbName
            };

            // Add the new menu item to the top of the submenu
            recentProjectsToolStripMenuItem.DropDownItems.Insert(0, item);

            // Save the project name and path and filename in the application settings
            Properties.Settings.Default.RecentProjectsNames.Insert(0, projectName);
            Properties.Settings.Default.RecentProjectsPaths.Insert(0, dbName);
            Properties.Settings.Default.Save();

            // Add an event handler for the new menu item
            item.Click += new System.EventHandler(this.FileMenuRecentProjectItem_Click);
        }

        /// <summary>
        /// Clear the recent projects list.
        /// </summary>
        private void ClearRecentProjects()
        {
            // Clear the recent projects submenu
            recentProjectsToolStripMenuItem.DropDownItems.Clear();

            // Clear the recent projects (file names and paths) in the application settings
            Properties.Settings.Default.RecentProjectsNames = new System.Collections.Specialized.StringCollection();
            Properties.Settings.Default.RecentProjectsPaths = new System.Collections.Specialized.StringCollection();
            Properties.Settings.Default.Save();

            // Disable the recent projects menu item
            recentProjectsToolStripMenuItem.Enabled = false;

            // Disable the clear recent projects list menu item
            clearRecentProjectsListToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Occurs when a recent projects menu item is clicked.
        /// </summary>
        private void FileMenuRecentProjectItem_Click(object sender, EventArgs e)
        {
            string projectName = ((ToolStripMenuItem)sender).Text.ToString();
            string dbName = ((ToolStripMenuItem)sender).Tag.ToString();
            OpenProject(projectName, dbName);
        }
        #endregion

        #region "Project"
        /// <summary>
        /// Create a new project.
        /// </summary>
        private void NewProject()
        {
            //var status = FileSystemOperationStatus.None;
            this.panelPie.Visible = false;
            var frmProject = new ProjectForm(OperationType.New);

            try
            {
                DialogResult result = frmProject.ShowDialog();
                this.Cursor = Cursors.WaitCursor;

                if (result == DialogResult.OK)
                {
                    Program.SoftwareProject = null;

                    // Clear the issues and tasks grids
                    GridIssues.Rows.Clear();
                    GridIssues.Refresh();
                    GridTasks.Rows.Clear();
                    GridTasks.Refresh();

                    Program.SoftwareProject = new Project(frmProject.ProjectName);
                    Program.SoftwareProject.Users.Add(Program.postgresUser);

                    ////status = ApplicationData.SaveProjectToFile(Program.SoftwareProject);
                    Program.SoftwareProject.DbName = Program.databaseName;
                    Database.CreateProject(Program.databaseName);
                }

                frmProject.Dispose();
                this.Cursor = Cursors.Default;

                if (result == DialogResult.Cancel) return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Set the main form title bar text
            this.Text = $"{frmProject.ProjectName} - {Program.myName}";

            // Add this project to the recent projects submenu and application settings
            AddRecentProject(Program.SoftwareProject.Name, Program.SoftwareProject.DbName);

            SetControlsState();
            MessageBox.Show($"Created {Program.SoftwareProject.Name}", Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Open an existing project.
        /// </summary>
        /// <param name="projectName">The full project name</param>
        /// <param name="dbName">(optional) The database to open, if present, the project is opened directly</param>
        /// <returns>True on success</returns>
        private bool OpenProject(string projectName = "", string dbName = "")
        {
            if (!string.IsNullOrEmpty(dbName))
            {
                Program.databaseName = dbName.ToLower();
            }

            var dbNames = Database.GetDbNames();            // Find existing database names
            bool result = false;

            if (dbNames.Length < 1)
            {
                MessageBox.Show("No database found, please use 'New Project'", Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(dbName))
            {
                result = SelectDatabase("Select a database", dbNames);
            }
            else if (!dbNames.Contains(Program.databaseName))
            {
                result = SelectDatabase($"Database {dbName} not found, select another database or close to abort", dbNames);
            }
            else
            {
                result = true;
            }

            if (!result)
            {
                return false;
            }

            Project newProject = new Project(Program.databaseName);

            if (!string.IsNullOrEmpty(projectName))
            {
                newProject.Name = projectName;
            }

            ConnectionString = Database.GetConnectionString(Program.databaseName);

            if (ConnectionString.StartsWith("Error"))
            {
                MessageBox.Show(ConnectionString, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ConnectionString = string.Empty;
                return false;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Cursor = Cursors.WaitCursor;
            string message = string.Empty;
            bool importJson = false;

            if (!importJson)
            {
                message = Database.LoadProject(ConnectionString, ref newProject);
            }
            else
            {
                // Load Old JSON file for testing purposes
                var status = ApplicationData.LoadProject("MiniBug Sample Project XL.json", out newProject);

                //// If there was an error loading the project file, show feedback
                //if (status != FileSystemOperationStatus.OK)
                //    ShowProjectErrorFeedback(status);
            }

            if (!string.IsNullOrEmpty(message))
            {
                // Error, abort the project
                this.Cursor = Cursors.Default;
                MessageBox.Show(message, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                newProject = null;
                return false;
            }
            else
            {
                Program.SoftwareProject = null;
                Program.SoftwareProject = newProject;

                if (!Program.SoftwareProject.Users.Contains(Program.postgresUser))          // Save current postgres user if not in database
                {
                    Program.SoftwareProject.Users.Add(Program.postgresUser);
                    Database.SaveUser(Program.postgresUser);
                }

                // Set the main form title bar text
                this.Text = $"{Program.SoftwareProject.Name} - {Program.myName}";

                // Clear the issues and tasks grids
                GridIssues.Rows.Clear();
                GridIssues.Refresh();
                GridTasks.Rows.Clear();
                GridTasks.Refresh();

                // Suspend the layout logic for the form, while the application is initializing
                this.SuspendLayout();

                PopulateGridIssues(importJson);
                PopulateGridTasks();

                // Add this project to the recent projects submenu and application settings
                AddRecentProject(Program.SoftwareProject.Name, Program.databaseName);

                // Jump to last row
                if (Properties.Settings.Default.ScrollToLastRow && this.GridIssues.RowCount > 10)
                {
                    this.GridIssues.FirstDisplayedScrollingRowIndex = this.GridIssues.RowCount - 1;
                }

                // Resume the layout logic
                this.ResumeLayout();

                if (this.GridIssues.RowCount > 0)
                {
                    ShowPieChart();
                }
                else
                {
                    this.panelPie.Visible = false;
                }
            }

            stopwatch.Stop();
            Debug.Print($"Loaded project in {stopwatch.Elapsed.TotalSeconds} seconds");
            File.AppendAllText("MaxiBug.log", $"Loaded project in {stopwatch.Elapsed.TotalSeconds} seconds\n");

            this.Cursor = Cursors.Default;
            SetControlsState();
            return true;
        }

        /// <summary>
        /// Shows a database selection form.
        /// </summary>
        /// <param name="formTitle">The form title</param>
        /// <param name="dbNames">Array with database names</param>
        /// <returns>True when selected, false on close form</returns>
        private bool SelectDatabase(string formTitle, string[] dbNames)
        {
            var selForm = new SelectionForm(dbNames, formTitle);

            if (selForm.ShowDialog() == DialogResult.OK)
            {
                Program.databaseName = selForm.SelectedItem;
                Debug.Print("" + Program.databaseName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Edit the current project settings.
        /// </summary>
        private void EditProjectSettings()
        {
            try
            {
                ProjectForm frmProject = new ProjectForm(OperationType.Edit, Program.SoftwareProject.Name);

                if (frmProject.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.Text = $"{frmProject.ProjectName} - {Program.myName}";     // Set the main form title
                    Program.SoftwareProject.Name = frmProject.ProjectName;
                    Database.UpdateProject(frmProject.ProjectName);
                    ClearRecentProjects();
                    AddRecentProject(frmProject.ProjectName, Program.databaseName);
                }

                frmProject.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Export the current project.
        /// </summary>
        private void ExportProject()
        {
            ExportForm frmExport = new ExportForm();        // Show the export to CSV form
            var frmExportResult = frmExport.ShowDialog();
            frmExport.Dispose();

            if (frmExportResult == DialogResult.Cancel)
            {
                return;
            }

            Program.SoftwareProject.Export(frmExport.IssuesFilename, frmExport.TasksFilename);

            // Show feedback about the project export operation
            ImportExportFeedbackForm frmFeedback = new ImportExportFeedbackForm(ImportExportOperation.Export);
            frmFeedback.ShowDialog();
            frmFeedback.Dispose();

            // Clear the export result information
            Program.SoftwareProject.ExportResult = null;
        }

        /// <summary>
        /// Deprecated: Saves the current project to a .json file.
        /// </summary>
        private void SaveProjectFile()
        {
            //this.Cursor = Cursors.WaitCursor;
            //var status = ApplicationData.SaveProjectToFile(Program.SoftwareProject);
            //this.Cursor = Cursors.Default;

            //// If there was an error saving the project file, show feedback
            //if (status != FileSystemOperationStatus.OK)
            //{
            //    ShowProjectErrorFeedback(status);
            //}
        }
        #endregion

        #region "Menu"
        /// <summary>
        /// Create a new project.
        /// </summary>
        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject();
        }

        /// <summary>
        /// Open an existing project.
        /// </summary>
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        /// <summary>
        /// Edit the current project settings.
        /// </summary>
        private void editProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditProjectSettings();
        }

        /// <summary>
        /// Export the current project issues and tasks to a file.
        /// </summary>
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportProject();
        }

        /// <summary>
        /// Configure column settings in the issues and tasks DataGridViews.
        /// </summary>
        private void configureColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigureColumns();
        }

        /// <summary>
        /// Change the application settings.
        /// </summary>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm frmSettings = new SettingsForm();

            if (frmSettings.ShowDialog() == DialogResult.OK)
            {
                this.Font = ApplicationSettings.AppFont;
                ApplySettingsToGrids();
            }

            frmSettings.Dispose();
        }

        /// <summary>
        /// Clear the recent projects list.
        /// </summary>
        private void clearRecentProjectsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearRecentProjects();
        }

        /// <summary>
        /// Close the application.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Create a new issue.
        /// </summary>
        private void newIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewIssue();
        }

        /// <summary>
        /// Edit the selected issue.
        /// </summary>
        private void editIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditIssue();
        }

        /// <summary>
        /// Delete the selected issues.
        /// </summary>
        private void deleteIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteIssue();
        }

        /// <summary>
        /// Clone the selected issue.
        /// </summary>
        private void cloneIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloneIssue();
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        private void newTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTask();
        }

        /// <summary>
        /// Edit the selected task.
        /// </summary>
        private void editTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditTask();
        }

        /// <summary>
        /// Delete the selected tasks.
        /// </summary>
        private void deleteTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteTask();
        }

        /// <summary>
        /// Clone the selected task.
        /// </summary>
        private void cloneTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloneTask();
        }

        /// <summary>
        /// Show information about this application.
        /// </summary>
        private void aboutMiniBugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm frmAbout = new AboutForm();

            frmAbout.ShowDialog();
            frmAbout.Dispose();
        }
        #endregion

        #region "Toolbar"
        /// <summary>
        /// Create a new issue.
        /// </summary>
        private void IconNewIssue_Click(object sender, EventArgs e)
        {
            NewIssue();
        }

        /// <summary>
        /// Edit the selected issue.
        /// </summary>
        private void IconEditIssue_Click(object sender, EventArgs e)
        {
            EditIssue();
        }

        /// <summary>
        /// Delete the selected issues.
        /// </summary>
        private void IconDeleteIssue_Click(object sender, EventArgs e)
        {
            DeleteIssue();
        }

        /// <summary>
        /// Clone the selected issue.
        /// </summary>
        private void IconCloneIssue_Click(object sender, EventArgs e)
        {
            CloneIssue();
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        private void IconNewTask_Click(object sender, EventArgs e)
        {
            NewTask();
        }

        /// <summary>
        /// Edit the selected task.
        /// </summary>
        private void IconEditTask_Click(object sender, EventArgs e)
        {
            EditTask();
        }

        /// <summary>
        /// Delete the selected tasks.
        /// </summary>
        private void IconDeleteTask_Click(object sender, EventArgs e)
        {
            DeleteTask();
        }

        /// <summary>
        /// Clone the selected task.
        /// </summary>
        private void IconCloneTask_Click(object sender, EventArgs e)
        {
            CloneTask();
        }

        /// <summary>
        /// Configure column settings in the issues and tasks DataGridViews.
        /// </summary>
        private void IconConfigureColumns_Click(object sender, EventArgs e)
        {
            ConfigureColumns();
        }
        #endregion

        #region "Issues"
        /// <summary>
        /// Initialize the issues DataGridView.
        /// </summary>
        private void InitializeGridIssues()
        {
            initializingGridIssues = true;

            GridIssues.BackgroundColor = TabControl.DefaultBackColor;
            GridIssues.BorderStyle = BorderStyle.None;
            //GridIssues.Dock = DockStyle.Fill;

            GridIssues.AllowUserToAddRows = false;
            GridIssues.AllowUserToDeleteRows = false;
            GridIssues.AllowUserToOrderColumns = true;
            GridIssues.AllowUserToResizeColumns = true;
            GridIssues.AllowUserToResizeRows = false;
            GridIssues.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            GridIssues.ColumnHeadersVisible = true;
            GridIssues.RowHeadersVisible = false;
            GridIssues.ReadOnly = true;
            GridIssues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridIssues.MultiSelect = true;
            GridIssues.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            GridIssues.ShowCellToolTips = true;

            GridIssues.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridIssues.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GridIssues.AutoGenerateColumns = false;

            // Add columns to the issues grid
            DataGridViewTextBoxColumn column = null;
            GridColumn col;

            // ID
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.ID];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            GridIssues.Columns.Add(column);

            // Priority
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Priority];
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                MinimumWidth = 32
            };
            GridIssues.Columns.Add(imageColumn);

            // Status
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Status];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            column.DefaultCellStyle.Padding = new Padding(15, 0, 6, 0);
            GridIssues.Columns.Add(column);

            // Version
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Version];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            GridIssues.Columns.Add(column);

            // Target version
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.TargetVersion];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            GridIssues.Columns.Add(column);

            // Summary
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Summary];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible
            };
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            GridIssues.Columns.Add(column);

            // Date created
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.DateCreated];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            GridIssues.Columns.Add(column);

            // Date modified
            col = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.DateModified];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            GridIssues.Columns.Add(column);

            GridIssues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set the display order for each column and also programmatic sort mode
            foreach (DataGridViewColumn c in GridIssues.Columns)
            {
                GridColumn issueCol = ApplicationSettings.GridIssuesColumns.Where(z => z.Value.Name == c.Name).FirstOrDefault().Value;
                c.DisplayIndex = issueCol.DisplayIndex;

                c.SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            initializingGridIssues = false;
        }

        /// <summary>
        /// Populate the issues grid.
        /// </summary>
        private void PopulateGridIssues(bool createTestDb = false)
        {
            IssuesClosed = 0;
            IssuesInProgress = 0;
            IssuesResolved = 0;
            IssuesUnconfirmed = 0;
            IssuesConfirmed = 0;
            IssuesFilteredTotal = 0;

            if ((Program.SoftwareProject != null) && (Program.SoftwareProject.Issues != null))
            {
                foreach (KeyValuePair<int, Issue> issue in Program.SoftwareProject.Issues)
                {
                    if (createTestDb)
                    {
                        //Program.SoftwareProject.AddIssue(issue.Value);               // Create test DB
                        Database.SaveIssue(issue.Value);
                    }

                    var issueStatus = Program.SoftwareProject.Issues[issue.Key].Status;

                    if (!ShowClosedIssues && (issueStatus == IssueStatus.Closed || issueStatus == IssueStatus.Resolved))
                    {
                        continue;
                    }

                    AddIssueToGrid(issue.Value);
                    PiechartCountersAdd(issueStatus);
                }

                if (this.panelPie.Visible)
                {
                    ShowPieChart();
                }
            }

            // Sort the contents according to the sort criteria
            GridIssues.Sort(new IssuesDataGridViewRowComparer(SortOrder.Ascending));
        }

        /// <summary>
        /// Updates the visibility of the issues grid columns, according to the current settings.
        /// </summary>
        private void UpdateColumnsVisibilityGridIssues()
        {
            GridIssues.SuspendLayout();

            foreach (KeyValuePair<IssueFieldsUI, GridColumn> item in ApplicationSettings.GridIssuesColumns)
            {
                GridIssues.Columns[item.Value.Name].Visible = item.Value.Visible;
            }

            GridIssues.ResumeLayout();
        }

        /// <summary>
        /// Advanced formatting of cells.
        /// </summary>
        private void GridIssues_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((Program.SoftwareProject == null) || (Program.SoftwareProject.Issues.Count == 0))
            {
                return;
            }

            int key = Convert.ToInt32(GridIssues[ApplicationSettings.GridIssuesColumns[IssueFieldsUI.ID].Name, e.RowIndex].Value.ToString());

            // Text color of closed issues
            if (Program.SoftwareProject.Issues.ContainsKey(key))
            {
                if (Program.SoftwareProject.Issues[key].Status == IssueStatus.Closed)
                {
                    e.CellStyle.ForeColor = ApplicationSettings.GridClosedItem;
                    e.CellStyle.SelectionForeColor = ApplicationSettings.GridClosedItem;
                }
            }

            // Configure/format the "priority" column's cells
            if (GridIssues.Columns[e.ColumnIndex].Name == ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Priority].Name)
            {
                // Test if the cell has no image assigned
                if (e.Value == null)
                {
                    // Assign a 1x1 bitmap so that a "missing image" icon is not displayed
                    e.Value = new Bitmap(1, 1);

                    // Assign an empty tooltip
                    GridIssues[e.ColumnIndex, e.RowIndex].ToolTipText = string.Empty;
                }
                else
                {
                    // Set the tooltip for this cell, but only if the project has issues
                    if ((Program.SoftwareProject.Issues == null) || (Program.SoftwareProject.Issues.Count == 0))
                    {
                        return;
                    }

                    string s = string.Empty;

                    switch (Program.SoftwareProject.Issues[key].Priority)
                    {
                        case IssuePriority.Low:
                            s = "Low priority";
                            break;

                        case IssuePriority.Normal:
                            s = "Normal priority";
                            break;

                        case IssuePriority.High:
                            s = "High priority";
                            break;

                        case IssuePriority.Urgent:
                            s = "Urgent priority";
                            break;

                        case IssuePriority.Immediate:
                            s = "Immediate priority";
                            break;
                    }

                    GridIssues[e.ColumnIndex, e.RowIndex].ToolTipText = s;
                }
            }
        }

        /// <summary>
        /// Advanced formatting of cells.
        /// </summary>
        private void GridIssues_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            e.PaintBackground(e.ClipBounds, true);
            e.PaintContent(e.ClipBounds);

            // Draw a filled rectangle in the "Status" column
            if (GridIssues.Columns[e.ColumnIndex].Name == ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Status].Name)
            {
                int key = Convert.ToInt32(GridIssues[ApplicationSettings.GridIssuesColumns[IssueFieldsUI.ID].Name, e.RowIndex].Value.ToString());
                Brush fillColor;

                // Set the color for the rectangle, according to the Issue status
                fillColor = ApplicationSettings.IssueStatusColors[Program.SoftwareProject.Issues[key].Status];

                if (fillColor != Brushes.Transparent)
                {
                    int size = ApplicationSettings.GridStatusRectangleSize;
                    Rectangle rect = new Rectangle(e.CellBounds.Location.X + 4, e.CellBounds.Location.Y + (e.CellBounds.Height / 2) - (size / 2), size, size);
                    e.Graphics.FillRectangle(fillColor, rect);
                    e.PaintContent(rect);
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// Returns a bitmap image corresponding to a given issue priority.
        /// </summary>
        /// <param name="priority">An issue priority</param>
        /// <returns>A bitmap image.</returns>
        private object GetIssuePriorityImage(IssuePriority priority)
        {
            switch (priority)
            {
                case IssuePriority.High:
                    return MaxiBug.Properties.Resources.Priority_High;

                case IssuePriority.Urgent:
                    return MaxiBug.Properties.Resources.Priority_Urgent;

                case IssuePriority.Immediate:
                    return MaxiBug.Properties.Resources.Priority_Immediate;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Add a new issue to the grid.
        /// </summary>
        /// <param name="newIssue">The issue to add to the grid.</param>
        private void AddIssueToGrid(Issue newIssue)
        {
            GridIssues.Rows.Add(new object[] {
                newIssue.ID.ToString(),
                GetIssuePriorityImage(newIssue.Priority),
                newIssue.Status.ToDescription(),
                newIssue.Version,
                newIssue.TargetVersion,
                newIssue.Summary,
                newIssue.DateCreated.ToString("g"),
                newIssue.DateModified.ToString("g")
            });
        }

        /// <summary>
        /// Refresh an issue's data in the issues grid.
        /// </summary>
        /// <param name="rowIndex">The row index in the issue grid, that holds the data for the issue.</param>
        /// <param name="issueID">The id of the issue (the issue key in the collection of issues).</param>
        private void RefreshIssueInGrid(int rowIndex, int issueID)
        {
            string key = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Priority].Name;
            this.GridIssues.Rows[rowIndex].Cells[key].Value = GetIssuePriorityImage(Program.SoftwareProject.Issues[issueID].Priority);

            key = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Status].Name;
            this.GridIssues.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Issues[issueID].Status.ToDescription();

            key = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Version].Name;
            this.GridIssues.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Issues[issueID].Version;

            key = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.TargetVersion].Name;
            this.GridIssues.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Issues[issueID].TargetVersion;

            key = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.Summary].Name;
            this.GridIssues.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Issues[issueID].Summary;

            key = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.DateCreated].Name;
            this.GridIssues.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Issues[issueID].DateCreated.ToString("g");

            key = ApplicationSettings.GridIssuesColumns[IssueFieldsUI.DateModified].Name;
            this.GridIssues.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Issues[issueID].DateModified.ToString("g");
        }

        /// <summary>
        /// Create a new issue.
        /// </summary>
        private void NewIssue()
        {
            try
            {
                // Lock the issue by user name
                Database.UpdateUserLocks(Program.postgresUser, Program.SoftwareProject.IssueIdCounter, 0);

                IssueForm frmIssue = new IssueForm(OperationType.New);

                if (frmIssue.ShowDialog() == DialogResult.OK)
                {
                    Program.SoftwareProject.AddIssue(frmIssue.CurrentIssue);        // Add and save in database
                    PiechartCountersAdd(frmIssue.CurrentIssue.Status, 1, this.panelPie.Visible);
                    AddIssueToGrid(frmIssue.CurrentIssue);                          // Add the new issue to the grid
                    SaveProjectFile();
                    SetControlsState();                                             // Refresh the UI controls
                    this.GridIssues.Sort(new IssuesDataGridViewRowComparer(SortOrder.Ascending));            // Sort the contents according to the sort criteria
                    this.GridIssues.ClearSelection();

                    if (ApplicationSettings.ScrollToLastRow)
                    {
                        this.GridIssues.Rows[GridIssues.Rows.Count - 1].Selected = true;                     // Select the last row
                        this.GridIssues.FirstDisplayedScrollingRowIndex = GridIssues.Rows.Count - 1;    // Jump to last row
                    }
                }

                frmIssue.Dispose();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            // Unlock
            Database.UpdateUserLocks(Program.postgresUser, 0, 0);
        }

        /// <summary>
        /// Edit the selected issue, the id can change if user jumps to another issue in the IssueForm.
        /// </summary>
        private void EditIssue()
        {
            if (this.GridIssues.SelectedRows.Count != 1)
            {
                return;
            }

            // Get the key of the issue in the selected row 
            int id = int.Parse(this.GridIssues.SelectedRows[0].Cells["id"].Value.ToString());
            int idOld = id;     // The id can change

            string userlock = Database.IssueLockedBy(id);

            if (!string.IsNullOrEmpty(userlock))
            {
                MessageBox.Show($"Issue is locked by {userlock} \nplease try again later ...", Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                // Lock the issue by user name
                Database.UpdateUserLocks(Program.postgresUser, id, 0);
            }

            IssueForm frmIssue = new IssueForm(OperationType.Edit, Program.SoftwareProject.Issues[id]);

            if (frmIssue.ShowDialog() == DialogResult.OK)
            {
                id = frmIssue.CurrentIssue.ID;
                Program.SoftwareProject.Issues[id] = frmIssue.CurrentIssue;

                if (frmIssue.PreviousStatus != frmIssue.CurrentIssue.Status)
                {
                    // Update the counters for the Pie chart
                    PiechartCountersAdd(frmIssue.PreviousStatus, -1);
                    PiechartCountersAdd(frmIssue.CurrentIssue.Status, 1, this.panelPie.Visible);
                }

                if (id == idOld)
                {
                    // Refresh the issue information in the grid
                    RefreshIssueInGrid(this.GridIssues.SelectedRows[0].Index, id);
                }
                else
                {
                    // Scroll to issue and select
                    foreach (DataGridViewRow row in this.GridIssues.Rows)
                    {
                        if (int.Parse(row.Cells["id"].Value.ToString()) == id)
                        {
                            this.GridIssues.ClearSelection();
                            this.GridIssues.FirstDisplayedScrollingRowIndex = row.Index;
                            row.Selected = true;
                            RefreshIssueInGrid(row.Index, id);
                            break;
                        }
                    }
                }

                // Save the project file
                SaveProjectFile();
                Database.UpdateIssue(frmIssue.CurrentIssue);            // Update in database

                // Refresh the UI controls
                SetControlsState();

                // Sort the contents according to the sort criteria
                this.GridIssues.Sort(new IssuesDataGridViewRowComparer(SortOrder.Ascending));
            }

            // Remove lock
            Database.UpdateUserLocks(Program.postgresUser, 0, 0);
            frmIssue.Dispose();
        }

        /// <summary>
        /// Delete the selected issues.
        /// </summary>
        private void DeleteIssue()
        {
            if (GridIssues.SelectedRows.Count > 0)
            {
                string msgIssues = (GridIssues.SelectedRows.Count == 1) ? "issue" : "issues";

                // Confirm this operation
                if (MessageBox.Show($"Are you sure you want to delete the selected {msgIssues}?", $"Delete {msgIssues}", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    // Iterate all the selected rows in the grid
                    foreach (DataGridViewRow row in GridIssues.SelectedRows)
                    {
                        int key = int.Parse(row.Cells["id"].Value.ToString());      // Get the key of the issue
                        var status = Program.SoftwareProject.Issues[key].Status;
                        GridIssues.Rows.RemoveAt(row.Index);                        // Remove the row from the grid
                        Program.SoftwareProject.Issues.Remove(key);                 // Remove the issue from the collection
                        Database.DeleteIssue(key);                                  // and from the database
                        PiechartCountersAdd(status, -1, this.panelPie.Visible);
                    }

                    // Save the project file
                    SaveProjectFile();

                    // Refresh the UI controls
                    SetControlsState();

                    // Sort the contents according to the sort criteria
                    GridIssues.Sort(new IssuesDataGridViewRowComparer(SortOrder.Ascending));
                }
            }
        }

        /// <summary>
        /// Clone one selected issue.
        /// </summary>
        private void CloneIssue()
        {
            if (GridIssues.SelectedRows.Count != 1)
            {
                return;

                //// For testing only, clone multiple issues
                //foreach (DataGridViewRow row in GridIssues.SelectedRows)
                //{
                //    // Get the key of the issue in the selected row 
                //    int id = int.Parse(row.Cells["id"].Value.ToString());

                //    // Update the counters for the Pie chart
                //    var status = Program.SoftwareProject.Issues[id].Status;
                //    PiechartCountersAdd(status);

                //    Issue newIssue = new Issue();
                //    Program.SoftwareProject.Issues[id].Clone(ref newIssue);
                //    Program.SoftwareProject.AddIssue(newIssue);
                //    AddIssueToGrid(newIssue);
                //}
            }
            else
            {
                // Get the key of the issue in the selected row 
                int id = int.Parse(this.GridIssues.SelectedRows[0].Cells["id"].Value.ToString());
                var status = Program.SoftwareProject.Issues[id].Status;
                Issue newIssue = new Issue();
                Program.SoftwareProject.Issues[id].Clone(ref newIssue);
                Program.SoftwareProject.AddIssue(newIssue);                 // Add and save in database
                PiechartCountersAdd(status, 1, this.panelPie.Visible);
                AddIssueToGrid(newIssue);                                   // Add the new issue to the grid
            }

            // Save the project file
            SaveProjectFile();
            SetControlsState();                                             // Refresh the UI controls
            this.GridIssues.Sort(new IssuesDataGridViewRowComparer(SortOrder.Ascending));

            if (ApplicationSettings.ScrollToLastRow)
            {
                this.GridIssues.ClearSelection();
                this.GridIssues.Rows[GridIssues.Rows.Count - 1].Selected = true;                     // Select the last row
                this.GridIssues.FirstDisplayedScrollingRowIndex = GridIssues.Rows.Count - 1;    // Jump to last row
            }
        }

        /// <summary>
        /// Handle a double-click on the issues grid: edit an issue.
        /// </summary>
        private void GridIssues_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditIssue();
        }

        /// <summary>
        /// When the selection changes in the issues grid, enable/disable certain icons.
        /// </summary>
        private void GridIssues_SelectionChanged(object sender, EventArgs e)
        {
            if (GridIssues.SelectedRows.Count == 1)
            {
                IconEditIssue.Enabled = true;
                IconCloneIssue.Enabled = true;
            }
            else
            {
                IconEditIssue.Enabled = false;
                IconCloneIssue.Enabled = false;
            }
        }

        /// <summary>
        /// Delete the selected issues when the user clicks the Delete key.
        /// </summary>
        private void GridIssues_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                DeleteIssue();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                EditIssue();
            }
            else
            {
                e.Handled = false;
                Debug.Print("Key = " + e.KeyCode);
            }
        }

        /// <summary>
        /// Handle changing the column order in the issues grid.
        /// </summary>
        private void GridIssues_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (!this.initializingGridIssues)
            {
                // Get the Issue instance with the specified column name
                GridColumn col = ApplicationSettings.GridIssuesColumns.Where(z => z.Value.Name == e.Column.Name).FirstOrDefault().Value;

                // Update the column order
                if (col != null)
                {
                    col.DisplayIndex = e.Column.DisplayIndex;
                }
            }
        }

        /// <summary>
        /// Handle clicks on the header cells in the issues DataGridView: sort by the clicked column.
        /// </summary>
        private void GridIssues_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Is the clicked column header the first sort column?
            if (GridIssues.Columns[e.ColumnIndex].Name == ApplicationSettings.GridIssuesColumns[ApplicationSettings.GridIssuesSort.FirstColumn].Name)
            {
                // Reverse the sort order
                if (ApplicationSettings.GridIssuesSort.FirstColumnSortOrder == SortOrder.Ascending)
                {
                    ApplicationSettings.GridIssuesSort.FirstColumnSortOrder = SortOrder.Descending;
                }
                else
                {
                    ApplicationSettings.GridIssuesSort.FirstColumnSortOrder = SortOrder.Ascending;
                }

                // Remove the sort glyph from the second sort column (if it is not null)
                if (ApplicationSettings.GridIssuesSort.SecondColumn != null)
                {
                    GridIssues.Columns[ApplicationSettings.GridIssuesColumns[ApplicationSettings.GridIssuesSort.SecondColumn.Value].Name].HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                // Set the second sort column to null
                ApplicationSettings.GridIssuesSort.SecondColumn = null;
                ApplicationSettings.GridIssuesSort.SecondColumnSortOrder = null;
            }
            else if ((ApplicationSettings.GridIssuesSort.SecondColumn != null) && (GridIssues.Columns[e.ColumnIndex].Name == ApplicationSettings.GridIssuesColumns[ApplicationSettings.GridIssuesSort.SecondColumn.Value].Name))
            {
                // Is the clicked column header the second sort column?

                // Remove the sort glyph from the first sort column
                GridIssues.Columns[ApplicationSettings.GridIssuesColumns[ApplicationSettings.GridIssuesSort.FirstColumn].Name].HeaderCell.SortGlyphDirection = SortOrder.None;

                // This is now the first sort column
                ApplicationSettings.GridIssuesSort.FirstColumn = ApplicationSettings.GridIssuesSort.SecondColumn.Value;
                // Reverse the sort order
                ApplicationSettings.GridIssuesSort.FirstColumnSortOrder = (ApplicationSettings.GridIssuesSort.SecondColumnSortOrder.Value == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;

                // Set the second sort column to null
                ApplicationSettings.GridIssuesSort.SecondColumn = null;
                ApplicationSettings.GridIssuesSort.SecondColumnSortOrder = null;
            }
            else
            {
                // Remove the sort glyph from the old sort columns
                RemoveGridSortGlyph(GridType.Issues, ApplicationSettings.GridIssuesSort, null);

                // Get the issue field with the specified column name
                IssueFieldsUI column = ApplicationSettings.GridIssuesColumns.Where(z => z.Value.Name == GridIssues.Columns[e.ColumnIndex].Name).FirstOrDefault().Key;

                // Set the new sort settings
                ApplicationSettings.GridIssuesSort.FirstColumn = column;
                ApplicationSettings.GridIssuesSort.FirstColumnSortOrder = SortOrder.Ascending;
                ApplicationSettings.GridIssuesSort.SecondColumn = null;
                ApplicationSettings.GridIssuesSort.SecondColumnSortOrder = null;
            }

            GridIssues.Sort(new IssuesDataGridViewRowComparer(SortOrder.Ascending));

            // Set the sort glyph
            SetGridSortGlyph(GridType.Issues);
        }
        #endregion

        #region "Tasks"
        /// <summary>
        /// Initialize the tasks DataGridView.
        /// </summary>
        private void InitializeGridTasks()
        {
            initializingGridTasks = true;

            GridTasks.BackgroundColor = TabControl.DefaultBackColor;
            GridTasks.BorderStyle = BorderStyle.None;
            GridTasks.Dock = DockStyle.Fill;

            GridTasks.AllowUserToAddRows = false;
            GridTasks.AllowUserToDeleteRows = false;
            GridTasks.AllowUserToOrderColumns = true;
            GridTasks.AllowUserToResizeColumns = true;
            GridTasks.AllowUserToResizeRows = false;
            GridTasks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            GridTasks.ColumnHeadersVisible = true;
            GridTasks.RowHeadersVisible = false;
            GridTasks.ReadOnly = true;
            GridTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridTasks.MultiSelect = true;
            GridTasks.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            GridTasks.ShowCellToolTips = true;

            GridTasks.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            GridTasks.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GridTasks.AutoGenerateColumns = false;

            // Add columns to the tasks grid
            DataGridViewTextBoxColumn column = null;
            GridColumn col;

            // ID
            col = ApplicationSettings.GridTasksColumns[TaskFieldsUI.ID];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            GridTasks.Columns.Add(column);

            // Priority
            col = ApplicationSettings.GridTasksColumns[TaskFieldsUI.Priority];
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                MinimumWidth = 32
            };
            GridTasks.Columns.Add(imageColumn);

            // Status
            col = ApplicationSettings.GridTasksColumns[TaskFieldsUI.Status];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            column.DefaultCellStyle.Padding = new Padding(15, 0, 6, 0);
            GridTasks.Columns.Add(column);

            // Target version
            col = ApplicationSettings.GridTasksColumns[TaskFieldsUI.TargetVersion];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            GridTasks.Columns.Add(column);

            // Summary
            col = ApplicationSettings.GridTasksColumns[TaskFieldsUI.Summary];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible
            };
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            GridTasks.Columns.Add(column);

            // Date created
            col = ApplicationSettings.GridTasksColumns[TaskFieldsUI.DateCreated];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            GridTasks.Columns.Add(column);

            // Date modified
            col = ApplicationSettings.GridTasksColumns[TaskFieldsUI.DateModified];
            column = new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.HeaderText,
                Visible = col.Visible,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            GridTasks.Columns.Add(column);

            GridTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set the display order for each column and also programmatic sort mode
            foreach (DataGridViewColumn c in GridTasks.Columns)
            {
                GridColumn taskCol = ApplicationSettings.GridTasksColumns.Where(z => z.Value.Name == c.Name).FirstOrDefault().Value;
                c.DisplayIndex = taskCol.DisplayIndex;

                c.SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            initializingGridTasks = false;
        }

        /// <summary>
        /// Populate the tasks grid.
        /// </summary>
        private void PopulateGridTasks()
        {
            if ((Program.SoftwareProject != null) && (Program.SoftwareProject.Tasks != null))
            {
                foreach (KeyValuePair<int, Task> item in Program.SoftwareProject.Tasks)
                {
                    AddTaskToGrid(item.Value);
                }
            }

            // Sort the contents according to the sort criteria
            GridTasks.Sort(new TasksDataGridViewRowComparer(SortOrder.Ascending));
        }

        /// <summary>
        /// Updates the visibility of the tasks grid columns, according to the current settings.
        /// </summary>
        private void UpdateColumnsVisibilityGridTasks()
        {
            GridTasks.SuspendLayout();

            foreach (KeyValuePair<TaskFieldsUI, GridColumn> item in ApplicationSettings.GridTasksColumns)
            {
                GridTasks.Columns[item.Value.Name].Visible = item.Value.Visible;
            }

            GridTasks.ResumeLayout();
        }

        /// <summary>
        /// Advanced formatting of cells.
        /// </summary>
        private void GridTasks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((Program.SoftwareProject == null) || (Program.SoftwareProject.Tasks.Count == 0))
            {
                return;
            }

            int key = Convert.ToInt32(GridTasks[ApplicationSettings.GridTasksColumns[TaskFieldsUI.ID].Name, e.RowIndex].Value.ToString());

            // Text color of finished tasks
            if (Program.SoftwareProject.Tasks.ContainsKey(key))
            {
                if (Program.SoftwareProject.Tasks[key].Status == TaskStatus.Finished)
                {
                    e.CellStyle.ForeColor = ApplicationSettings.GridClosedItem;
                    e.CellStyle.SelectionForeColor = ApplicationSettings.GridClosedItem;
                }
            }

            // Configure/format the "priority" column's cells
            if (GridTasks.Columns[e.ColumnIndex].Name == ApplicationSettings.GridTasksColumns[TaskFieldsUI.Priority].Name)
            {
                // Test if the cell has no image assigned
                if (e.Value == null)
                {
                    // Assign a 1x1 bitmap so that a "missing image" icon is not displayed
                    e.Value = new Bitmap(1, 1);
                }
                else
                {
                    // Set the tooltip for this cell, but only if the project has tasks
                    if ((Program.SoftwareProject.Tasks == null) || (Program.SoftwareProject.Tasks.Count == 0))
                    {
                        return;
                    }

                    string s = string.Empty;

                    switch (Program.SoftwareProject.Tasks[key].Priority)
                    {
                        case TaskPriority.Low:
                            s = "Low priority";
                            break;

                        case TaskPriority.Normal:
                            s = "Normal priority";
                            break;

                        case TaskPriority.High:
                            s = "High priority";
                            break;

                        case TaskPriority.Urgent:
                            s = "Urgent priority";
                            break;

                        case TaskPriority.Immediate:
                            s = "Immediate priority";
                            break;
                    }

                    GridTasks[e.ColumnIndex, e.RowIndex].ToolTipText = s;
                }
            }
        }

        /// <summary>
        /// Advanced formatting of cells.
        /// </summary>
        private void GridTasks_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            e.PaintBackground(e.ClipBounds, true);
            e.PaintContent(e.ClipBounds);

            // Draw a filled rectangle in the "Status" column
            if (GridTasks.Columns[e.ColumnIndex].Name == ApplicationSettings.GridTasksColumns[TaskFieldsUI.Status].Name)
            {
                int key = Convert.ToInt32(GridTasks[ApplicationSettings.GridTasksColumns[TaskFieldsUI.ID].Name, e.RowIndex].Value.ToString());
                Brush fillColor;

                // Set the color for the rectangle, according to the Task status
                fillColor = ApplicationSettings.TaskStatusColors[Program.SoftwareProject.Tasks[key].Status];

                if (fillColor != Brushes.Transparent)
                {
                    int size = ApplicationSettings.GridStatusRectangleSize;
                    Rectangle rect = new Rectangle(e.CellBounds.Location.X + 4, e.CellBounds.Location.Y + (e.CellBounds.Height / 2) - (size / 2), size, size);
                    e.Graphics.FillRectangle(fillColor, rect);
                    e.PaintContent(rect);
                }
            }

            e.Handled = true;
        }

        /// <summary>
        /// Returns a bitmap image corresponding to a given task priority.
        /// </summary>
        /// <param name="priority">A task priority</param>
        /// <returns>A bitmap image.</returns>
        private object GetTaskPriorityImage(TaskPriority priority)
        {
            switch (priority)
            {
                case TaskPriority.High:
                    return MaxiBug.Properties.Resources.Priority_High;

                case TaskPriority.Urgent:
                    return MaxiBug.Properties.Resources.Priority_Urgent;

                case TaskPriority.Immediate:
                    return MaxiBug.Properties.Resources.Priority_Immediate;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Add a new task to the grid.
        /// </summary>
        /// <param name="newTask">The task to add to the grid.</param>
        private void AddTaskToGrid(Task newTask)
        {
            GridTasks.Rows.Add(new object[] {
                newTask.ID.ToString(),
                GetTaskPriorityImage(newTask.Priority),
                newTask.Status.ToDescription(),
                newTask.TargetVersion,
                newTask.Summary,
                newTask.DateCreated.ToString("g"),
                newTask.DateModified.ToString("g")
            });
        }

        /// <summary>
        /// Refresh task's data in the tasks grid.
        /// </summary>
        /// <param name="rowIndex">The row index in the task grid, that holds the data for the task.</param>
        /// <param name="taskID">The id of the task (the task key in the collection of tasks).</param>
        private void RefreshTaskInGrid(int rowIndex, int taskID)
        {
            string key = string.Empty;

            key = ApplicationSettings.GridTasksColumns[TaskFieldsUI.Priority].Name;
            GridTasks.Rows[rowIndex].Cells[key].Value = GetTaskPriorityImage(Program.SoftwareProject.Tasks[taskID].Priority);

            key = ApplicationSettings.GridTasksColumns[TaskFieldsUI.Status].Name;
            GridTasks.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Tasks[taskID].Status.ToDescription();

            key = ApplicationSettings.GridTasksColumns[TaskFieldsUI.TargetVersion].Name;
            GridTasks.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Tasks[taskID].TargetVersion;

            key = ApplicationSettings.GridTasksColumns[TaskFieldsUI.Summary].Name;
            GridTasks.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Tasks[taskID].Summary;

            key = ApplicationSettings.GridTasksColumns[TaskFieldsUI.DateCreated].Name;
            GridTasks.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Tasks[taskID].DateCreated.ToString("g");

            key = ApplicationSettings.GridTasksColumns[TaskFieldsUI.DateModified].Name;
            GridTasks.Rows[rowIndex].Cells[key].Value = Program.SoftwareProject.Tasks[taskID].DateModified.ToString("g");
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        private void NewTask()
        {
            TaskForm frmTask = new TaskForm(OperationType.New);

            if (frmTask.ShowDialog() == DialogResult.OK)
            {
                Program.SoftwareProject.AddTask(frmTask.CurrentTask);

                // Add the new task to the grid
                AddTaskToGrid(frmTask.CurrentTask);

                // Unselect all the previously selected rows
                foreach (DataGridViewRow row in GridTasks.SelectedRows)
                {
                    row.Selected = false;
                }

                // Select the last row (the one which was added)
                GridTasks.Rows[GridTasks.Rows.Count - 1].Selected = true;

                // Save the project file
                SaveProjectFile();

                // Refresh the UI controls
                SetControlsState();

                // Sort the contents according to the sort criteria
                GridTasks.Sort(new TasksDataGridViewRowComparer(SortOrder.Ascending));
            }

            frmTask.Dispose();
        }

        /// <summary>
        /// Edit the selected task.
        /// </summary>
        private void EditTask()
        {
            if (GridTasks.SelectedRows.Count == 1)
            {
                // Get the key of the task in the selected row 
                int id = int.Parse(GridTasks.SelectedRows[0].Cells["id"].Value.ToString());

                TaskForm frmTask = new TaskForm(OperationType.Edit, Program.SoftwareProject.Tasks[id]);

                if (frmTask.ShowDialog() == DialogResult.OK)
                {
                    Program.SoftwareProject.Tasks[id] = frmTask.CurrentTask;

                    // Refresh the task information in the grid
                    RefreshTaskInGrid(GridTasks.SelectedRows[0].Index, id);

                    // Save the project file
                    SaveProjectFile();
                    Database.UpdateTask(frmTask.CurrentTask);            // Update in database

                    // Refresh the UI controls
                    SetControlsState();

                    // Sort the contents according to the sort criteria
                    GridTasks.Sort(new TasksDataGridViewRowComparer(SortOrder.Ascending));
                }

                frmTask.Dispose();
            }
        }

        /// <summary>
        /// Delete the selected task(s).
        /// </summary>
        private void DeleteTask()
        {
            if (GridTasks.SelectedRows.Count > 0)
            {
                string msgTasks = (GridTasks.SelectedRows.Count == 1) ? "task" : "tasks";

                if (MessageBox.Show($"Are you sure you want to delete the selected {msgTasks}?", $"Delete {msgTasks}", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    // Iterate all the selected rows in the grid
                    foreach (DataGridViewRow row in GridTasks.SelectedRows)
                    {
                        int key = int.Parse(row.Cells["id"].Value.ToString());      // Get the key of the task
                        Program.SoftwareProject.Tasks.Remove(key);                  // Remove the task from the collection
                        Database.DeleteTask(key);                                   // Delete from database
                        GridTasks.Rows.RemoveAt(row.Index);                         // Remove the row from the grid
                    }

                    // Save the project file
                    SaveProjectFile();

                    // Refresh the UI controls
                    SetControlsState();

                    // Sort the contents according to the sort criteria
                    GridTasks.Sort(new TasksDataGridViewRowComparer(SortOrder.Ascending));
                }
            }
        }

        /// <summary>
        /// Clone the selected task.
        /// </summary>
        private void CloneTask()
        {
            if (GridTasks.SelectedRows.Count == 1)
            {
                // Get the key of the task in the selected row 
                int id = int.Parse(GridTasks.SelectedRows[0].Cells["id"].Value.ToString());

                Task newTask = new Task();
                Program.SoftwareProject.Tasks[id].Clone(ref newTask);
                Program.SoftwareProject.AddTask(newTask);           // Add to project and save in database
                AddTaskToGrid(newTask);                             // Add the new task to the grid

                // Save the project file
                SaveProjectFile();

                // Refresh the UI controls
                SetControlsState();

                // Sort the contents according to the sort criteria
                GridTasks.Sort(new TasksDataGridViewRowComparer(SortOrder.Ascending));
            }
        }

        /// <summary>
        /// Handle a double-click on the tasks grid: edit a task.
        /// </summary>
        private void GridTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditTask();
        }

        /// <summary>
        /// When the selection changes in the tasks grid, enable/disable icons.
        /// </summary>
        private void GridTasks_SelectionChanged(object sender, EventArgs e)
        {
            if (GridTasks.SelectedRows.Count == 1)
            {
                IconEditTask.Enabled = true;
                IconCloneTask.Enabled = true;
            }
            else
            {
                IconEditTask.Enabled = false;
                IconCloneTask.Enabled = false;
            }
        }

        /// <summary>
        /// Delete the selected tasks when the user clicks the Delete key.
        /// </summary>
        private void GridTasks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                DeleteTask();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                EditTask();
            }
        }

        /// <summary>
        /// Handle changing the column order in the tasks grid.
        /// </summary>
        private void GridTasks_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (!this.initializingGridTasks)
            {
                // Get the Task instance with the specified column name
                GridColumn Col = ApplicationSettings.GridTasksColumns.Where(z => z.Value.Name == e.Column.Name).FirstOrDefault().Value;

                // Update the column order
                if (Col != null)
                {
                    Col.DisplayIndex = e.Column.DisplayIndex;
                }
            }
        }

        /// <summary>
        /// Handle clicks on the header cells in the tasks DataGridView: sort by the clicked column.
        /// </summary>
        private void GridTasks_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Is the clicked column header the first sort column?
            if (GridTasks.Columns[e.ColumnIndex].Name == ApplicationSettings.GridTasksColumns[ApplicationSettings.GridTasksSort.FirstColumn].Name)
            {
                // Reverse the sort order
                if (ApplicationSettings.GridTasksSort.FirstColumnSortOrder == SortOrder.Ascending)
                {
                    ApplicationSettings.GridTasksSort.FirstColumnSortOrder = SortOrder.Descending;
                }
                else
                {
                    ApplicationSettings.GridTasksSort.FirstColumnSortOrder = SortOrder.Ascending;
                }

                // Remove the sort glyph from the second sort column (if it is not null)
                if (ApplicationSettings.GridTasksSort.SecondColumn != null)
                {
                    GridTasks.Columns[ApplicationSettings.GridTasksColumns[ApplicationSettings.GridTasksSort.SecondColumn.Value].Name].HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                // Set the second sort column to null
                ApplicationSettings.GridTasksSort.SecondColumn = null;
                ApplicationSettings.GridTasksSort.SecondColumnSortOrder = null;
            }
            else if ((ApplicationSettings.GridTasksSort.SecondColumn != null) && (GridTasks.Columns[e.ColumnIndex].Name == ApplicationSettings.GridTasksColumns[ApplicationSettings.GridTasksSort.SecondColumn.Value].Name))
            {
                // Is the clicked column header the second sort column?

                // Remove the sort glyph from the first sort column
                GridTasks.Columns[ApplicationSettings.GridTasksColumns[ApplicationSettings.GridTasksSort.FirstColumn].Name].HeaderCell.SortGlyphDirection = SortOrder.None;

                // This is now the first sort column
                ApplicationSettings.GridTasksSort.FirstColumn = ApplicationSettings.GridTasksSort.SecondColumn.Value;
                // Reverse the sort order
                ApplicationSettings.GridTasksSort.FirstColumnSortOrder = (ApplicationSettings.GridTasksSort.SecondColumnSortOrder.Value == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;

                // Set the second sort column to null
                ApplicationSettings.GridTasksSort.SecondColumn = null;
                ApplicationSettings.GridTasksSort.SecondColumnSortOrder = null;
            }
            else
            {
                // Remove the sort glyph from the old sort columns
                RemoveGridSortGlyph(GridType.Tasks, null, ApplicationSettings.GridTasksSort);

                // Get the task field with the specified column name
                TaskFieldsUI column = ApplicationSettings.GridTasksColumns.Where(z => z.Value.Name == GridTasks.Columns[e.ColumnIndex].Name).FirstOrDefault().Key;

                // Set the new sort settings
                ApplicationSettings.GridTasksSort.FirstColumn = column;
                ApplicationSettings.GridTasksSort.FirstColumnSortOrder = SortOrder.Ascending;
                ApplicationSettings.GridTasksSort.SecondColumn = null;
                ApplicationSettings.GridTasksSort.SecondColumnSortOrder = null;
            }

            GridTasks.Sort(new TasksDataGridViewRowComparer(SortOrder.Ascending));

            // Set the sort glyph
            SetGridSortGlyph(GridType.Tasks);
        }
        #endregion

        #region "Grids"
        /// <summary>
        ///  Apply the settings to the Issues and Tasks grids.
        /// </summary>
        private void ApplySettingsToGrids()
        {
            // Apply settings to the Issues grid
            //GridIssues.Font = ApplicationSettings.AppFont;
            GridIssues.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            // Grid cell borders
            if (ApplicationSettings.GridShowBorders)
            {
                GridIssues.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                GridIssues.GridColor = ApplicationSettings.GridBorderColor;
            }
            else
            {
                GridIssues.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }

            // Selection color
            GridIssues.DefaultCellStyle.SelectionBackColor = ApplicationSettings.GridSelectionBackColor;
            GridIssues.DefaultCellStyle.SelectionForeColor = ApplicationSettings.GridSelectionForeColor;

            // Alternating row colors
            GridIssues.RowsDefaultCellStyle.BackColor = ApplicationSettings.GridRowBackColor;
            if (ApplicationSettings.GridAlternatingRowColor)
            {
                GridIssues.AlternatingRowsDefaultCellStyle.BackColor = ApplicationSettings.GridAlternateRowBackColor;
            }
            else
            {
                GridIssues.AlternatingRowsDefaultCellStyle.BackColor = ApplicationSettings.GridRowBackColor;
            }

            // Apply settings to the Tasks grid
            //GridTasks.Font = ApplicationSettings.AppFont;
            GridTasks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            // Grid cell borders
            if (ApplicationSettings.GridShowBorders)
            {
                GridTasks.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                GridTasks.GridColor = ApplicationSettings.GridBorderColor;
            }
            else
            {
                GridTasks.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }

            // Selection color
            GridTasks.DefaultCellStyle.SelectionBackColor = ApplicationSettings.GridSelectionBackColor;
            GridTasks.DefaultCellStyle.SelectionForeColor = ApplicationSettings.GridSelectionForeColor;

            // Alternating row colors
            GridTasks.RowsDefaultCellStyle.BackColor = ApplicationSettings.GridRowBackColor;
            if (ApplicationSettings.GridAlternatingRowColor)
            {
                GridTasks.AlternatingRowsDefaultCellStyle.BackColor = ApplicationSettings.GridAlternateRowBackColor;
            }
            else
            {
                GridTasks.AlternatingRowsDefaultCellStyle.BackColor = ApplicationSettings.GridRowBackColor;
            }
        }

        /// <summary>
        /// Display the sort glyph, in the sort columns of the issues or tasks DataGridViews.
        /// </summary>
        /// <param name="grid">The DataGridView to apply the sort glyph.</param>
        private void SetGridSortGlyph(GridType grid)
        {
            if ((grid == GridType.All) || (grid == GridType.Issues))
            {
                GridIssues.Columns[ApplicationSettings.GridIssuesColumns[ApplicationSettings.GridIssuesSort.FirstColumn].Name].HeaderCell.SortGlyphDirection = ApplicationSettings.GridIssuesSort.FirstColumnSortOrder;

                if (ApplicationSettings.GridIssuesSort.SecondColumn != null)
                {
                    GridIssues.Columns[ApplicationSettings.GridIssuesColumns[ApplicationSettings.GridIssuesSort.SecondColumn.Value].Name].HeaderCell.SortGlyphDirection = ApplicationSettings.GridIssuesSort.SecondColumnSortOrder.Value;
                }
            }

            if ((grid == GridType.All) || (grid == GridType.Tasks))
            {
                GridTasks.Columns[ApplicationSettings.GridTasksColumns[ApplicationSettings.GridTasksSort.FirstColumn].Name].HeaderCell.SortGlyphDirection = ApplicationSettings.GridTasksSort.FirstColumnSortOrder;

                if (ApplicationSettings.GridTasksSort.SecondColumn != null)
                {
                    GridTasks.Columns[ApplicationSettings.GridTasksColumns[ApplicationSettings.GridTasksSort.SecondColumn.Value].Name].HeaderCell.SortGlyphDirection = ApplicationSettings.GridTasksSort.SecondColumnSortOrder.Value;
                }
            }
        }

        /// <summary>
        /// Remove the sort glyph, in the specificied columns, of the issues and/or tasks DataGridView.
        /// </summary>
        /// <param name="grid">The DataGridView(s) to remove the sort glyph.</param>
        /// <param name="issuesSortSettings">Contains the columns to remove the glyph in the issues DataGridView.</param>
        /// <param name="tasksSortSettings">Contains the columns to remove the glyph in the tasks DataGridView.</param>
        private void RemoveGridSortGlyph(GridType grid, GridIssuesSortSettings issuesSortSettings, GridTasksSortSettings tasksSortSettings)
        {
            if ((issuesSortSettings != null) && ((grid == GridType.All) || (grid == GridType.Issues)))
            {
                GridIssues.Columns[ApplicationSettings.GridIssuesColumns[issuesSortSettings.FirstColumn].Name].HeaderCell.SortGlyphDirection = SortOrder.None;

                if (issuesSortSettings.SecondColumn != null)
                {
                    GridIssues.Columns[ApplicationSettings.GridIssuesColumns[issuesSortSettings.SecondColumn.Value].Name].HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }

            if ((tasksSortSettings != null) && ((grid == GridType.All) || (grid == GridType.Tasks)))
            {
                GridTasks.Columns[ApplicationSettings.GridTasksColumns[tasksSortSettings.FirstColumn].Name].HeaderCell.SortGlyphDirection = SortOrder.None;

                if (tasksSortSettings.SecondColumn != null)
                {
                    GridTasks.Columns[ApplicationSettings.GridTasksColumns[tasksSortSettings.SecondColumn.Value].Name].HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
        }

        /// <summary>
        /// Configure column settings in the issues and tasks DataGridViews.
        /// </summary>
        private void ConfigureColumns()
        {
            // Store the current sort settings
            GridIssuesSortSettings gridIssuesSortOld = new GridIssuesSortSettings(ApplicationSettings.GridIssuesSort.FirstColumn, ApplicationSettings.GridIssuesSort.FirstColumnSortOrder, ApplicationSettings.GridIssuesSort.SecondColumn, ApplicationSettings.GridIssuesSort.SecondColumnSortOrder);
            GridTasksSortSettings gridTasksSortOld = new GridTasksSortSettings(ApplicationSettings.GridTasksSort.FirstColumn, ApplicationSettings.GridTasksSort.FirstColumnSortOrder, ApplicationSettings.GridTasksSort.SecondColumn, ApplicationSettings.GridTasksSort.SecondColumnSortOrder);

            ConfigureViewForm frmConfigureView = new ConfigureViewForm();

            if (frmConfigureView.ShowDialog() == DialogResult.OK)
            {
                // Apply the new visibility settings
                UpdateColumnsVisibilityGridIssues();
                UpdateColumnsVisibilityGridTasks();

                ApplicationSettings.Save(ApplicationSettings.SaveSettings.ColumnOrderSort);

                // Apply the new sort settings, if there were changes
                if (!gridIssuesSortOld.Equals(ApplicationSettings.GridIssuesSort))
                {
                    // Remove the sort glyph from the old sort columns
                    RemoveGridSortGlyph(GridType.Issues, gridIssuesSortOld, null);

                    GridIssues.Sort(new IssuesDataGridViewRowComparer(SortOrder.Ascending));

                    // Set the sort glyph
                    SetGridSortGlyph(GridType.Issues);
                }

                // Apply the new sort settings, if there were changes
                if (!gridTasksSortOld.Equals(ApplicationSettings.GridTasksSort))
                {
                    // Remove the sort glyph from the old sort columns
                    RemoveGridSortGlyph(GridType.Tasks, null, gridTasksSortOld);

                    GridTasks.Sort(new TasksDataGridViewRowComparer(SortOrder.Ascending));

                    // Set the sort glyph
                    SetGridSortGlyph(GridType.Tasks);
                }
            }

            frmConfigureView.Dispose();
        }

        #endregion

        /// <summary>
        /// Clear the text search box on first click.
        /// </summary>
        private void txtSearch_Click(object sender, EventArgs e)
        {
            if (this.txtSearch.Text.Equals("search"))
            {
                this.txtSearch.Clear();
            }
        }

        /// <summary>
        /// Search after pressing Enter key, select found rows.
        /// </summary>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchstring = txtSearch.Text;
                var stringComparison = StringComparison.Ordinal;
                int row = 0;
                bool found = false;
                this.GridIssues.ClearSelection();

                if (ApplicationSettings.SearchCaseInsensitive)
                {
                    stringComparison = StringComparison.OrdinalIgnoreCase;          // Case insensitive search
                }

                // Search in version and summary first, select all found rows
                foreach (DataGridViewRow issue in this.GridIssues.Rows)
                {
                    string version = issue.Cells["Version"].Value.ToString();
                    string summary = issue.Cells["Summary"].Value.ToString();

                    bool contains = version.IndexOf(searchstring, stringComparison) >= 0;
                    contains |= summary.IndexOf(searchstring, stringComparison) >= 0;

                    if (contains)
                    {
                        if (!found)
                        {
                            // Scroll to first found row
                            this.GridIssues.FirstDisplayedScrollingRowIndex = row;
                        }

                        this.GridIssues.Rows[row].Selected = true;
                        found = true;
                    }

                    row++;
                }

                if (!found)
                {
                    // Search Description, select first found row and stop searching
                    row = 0;

                    foreach (DataGridViewRow issue in this.GridIssues.Rows)
                    {
                        int id = int.Parse(issue.Cells["id"].Value.ToString());
                        var description = Program.SoftwareProject.Issues[id].Description;

                        if (description.IndexOf(searchstring, stringComparison) >= 0)
                        {
                            if (!found)
                            {
                                // Scroll to first found row
                                this.GridIssues.FirstDisplayedScrollingRowIndex = row;
                            }

                            this.GridIssues.Rows[row].Selected = true;
                            found = true;
                            break;
                        }

                        row++;
                    }
                }
            }
        }

        /// <summary>
        /// Show or hide the closed and resolved issues.
        /// </summary>
        private void IconShowClosed_Click(object sender, EventArgs e)
        {
            // Toggle show closed issues flag
            ShowClosedIssues = !ShowClosedIssues;

            if (ShowClosedIssues)
            {
                IconShowClosed.Image = Properties.Resources.Filter_32x32;
            }
            else
            {
                IconShowClosed.Image = Properties.Resources.Filter_clear_32x32;
            }

            int id = 0;

            if (GridIssues.SelectedRows.Count > 0)
            {
                // Get the key of the issue in the selected row 
                id = int.Parse(GridIssues.SelectedRows[0].Cells["id"].Value.ToString());
            }

            GridIssues.Rows.Clear();
            GridIssues.Refresh();
            PopulateGridIssues();

            // Scroll to selected row if set
            if (id > 0)
            {
                // Scroll to issue and select
                foreach (DataGridViewRow row in GridIssues.Rows)
                {
                    if (int.Parse(row.Cells["id"].Value.ToString()) == id)
                    {
                        this.GridIssues.FirstDisplayedScrollingRowIndex = row.Index;
                        row.Selected = true;
                        break;
                    }
                }
            }

            SetControlsState();
        }

        private void TabControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Do not allow switching tabs with e.g. Ctrl-Home or Ctrl-End
            this.GridIssues.Focus();
        }

        private void TabControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Do not allow switching tabs with e.g. Ctrl-Home or Ctrl-End
            this.GridIssues.Focus();
        }
    }
}
