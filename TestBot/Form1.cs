using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxiBug
{
    /// <summary>
    /// This TestBot inserts new issues in a MaxiBug database.
    /// It also simulates the issue being worked on by locking the issue.
    /// Before using this TestBot, matching user names and passwords must be added in Postgres.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Indicates that tasks are running.
        /// Declared volatile as it can be used by multiple threads.
        /// </summary>
        public static volatile bool Running;

        /// <summary>
        /// Database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The simulated time in millisecondes that testbot works on an issue.
        /// </summary>
        private int IntervalMs { get; set; } = 10000;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Run()
        {
            Running = true;
            Program.postgresIpaddress = txtIpaddress.Text;
            Program.postgresPort = int.Parse(txtPort.Text);
            Program.postgresUser = txtUsername.Text;
            Program.postgresPassword = txtPassword.Text;
            Debug.Print("Postgres user = " + Program.postgresUser);
            int recordCount = 1;

            if (this.OpenProject())
            {
                this.Cursor = Cursors.WaitCursor;
                this.panel1.Enabled = false;
                this.progressBar1.Visible = true;
                this.progressBar1.Maximum = (int)this.numericUpDownRecords.Value;
                this.progressBar1.Value = 0;
                Task newTask = Task.Factory.StartNew(NewTestIssue);

                // Enter the loop, when task has finished, start a new one
                while (Running)
                {
                    if (newTask.IsCompleted)
                    {
                        Debug.Print($"Task {recordCount} completed");
                        this.progressBar1.Value = recordCount;
                        recordCount++;
                        newTask.Dispose();

                        if (recordCount > this.numericUpDownRecords.Value)
                        {
                            break;
                        }

                        newTask = Task.Factory.StartNew(NewTestIssue);        // Spin up a new Task
                    }

                    Application.DoEvents();         // Keep UI responsive
                }
            }

            // Make sure locks are released
            Database.UpdateUserLocks(Program.postgresUser, 0, 0);
            this.Cursor = Cursors.Default;
            this.panel1.Enabled = true;

            if (Running)
            {
                MessageBox.Show("Testbot completed adding records!", Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.progressBar1.Visible = false;
        }

        /// <summary>
        /// Create a new test issue and save in database.
        /// </summary>
        private void NewTestIssue()
        {
            try
            {
                // Lock the issue by user name
                Database.UpdateUserLocks(Program.postgresUser, Program.SoftwareProject.IssueIdCounter, 0);
                Issue testIssue = new Issue(Program.SoftwareProject.IssueIdCounter);
                testIssue.CreatedBy = Program.postgresUser;
                testIssue.Status = IssueStatus.Unconfirmed;
                testIssue.Summary = $"Test issue created by {Program.postgresUser}";
                testIssue.Version = Program.SoftwareProject.IssueIdCounter.ToString();
                Program.SoftwareProject.AddIssue(testIssue);        // Add and save in database

                // Simulate user working, issue remains locked during IntervalMs
                Thread.Sleep(IntervalMs);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            // Unlock
            Database.UpdateUserLocks(Program.postgresUser, 0, 0);
        }

        /// <summary>
        /// Open a project.
        /// </summary>
        /// <param name="projectName">The full project name</param>
        /// <param name="dbName">(optional) The database to open, if present, the project is opened directly</param>
        /// <returns>True on success</returns>
        private bool OpenProject(string projectName = "", string dbName = "")
        {
            this.Cursor = Cursors.WaitCursor;

            if (!string.IsNullOrEmpty(dbName))
            {
                Program.databaseName = dbName.ToLower();
            }

            var dbNames = Database.GetDbNames();            // Find existing database names
            bool result = false;

            if (dbNames.Length < 1)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("No database found, please create a 'New Project' in MaxiBug", Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                this.Cursor = Cursors.Default;
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
            string message = Database.LoadProject(ConnectionString, ref newProject);

            // Set user status to active
            Database.UpdateUserActive(Program.postgresUser, true);

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
            }

            stopwatch.Stop();
            Debug.Print($"Loaded project in {stopwatch.Elapsed.TotalSeconds} seconds");

            this.Cursor = Cursors.Default;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Abort running tasks
            Running = false;
            Database.UpdateUserActive(Program.postgresUser, false);
        }
    }
}
