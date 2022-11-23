using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MaxiBug
{
    /// <summary>
    /// Retrieve Git history for an issue with a user defined git log command.
    /// </summary>
    public partial class GitForm : Form
    {
        /// <summary>
        /// Initialize the GitForm.
        /// </summary>
        /// <param name="id">The issue id number to retrieve from Git tags</param>
        public GitForm(int id)
        {
            InitializeComponent();
            this.InitForm(id);
        }

        /// <summary>
        /// Get the default settings and replace the {issue_id} placeholder.
        /// </summary>
        /// <param name="id">The issue id number</param>
        private void InitForm(int id)
        {
            this.txtGitFolder.Text = ApplicationSettings.GitFolder;
            this.txtGitCommand.Text = ApplicationSettings.GitCommand.Replace("{issue_id}", id.ToString());
        }

        /// <summary>
        /// Select the Git working copy folder.
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

        /// <summary>
        /// Run Git log command and save to a temporary text file.
        /// Note that the DOS command must start with /C for cmd.exe to work.
        /// </summary>
        private void btOk_Click(object sender, EventArgs e)
        {
            //string gitCommand = "/C dir";
            string gitCommand = this.txtGitCommand.Text.Replace("{path}", this.txtGitFolder.Text);
            string gitTempFile = Path.Combine(Application.StartupPath, "git_temp.txt");
            gitCommand += " >> " + gitTempFile;
            Debug.Print(gitCommand);

            if (!gitCommand.StartsWith(@"/C"))
            {
                gitCommand = @"/C " + gitCommand;
            }

            using (Process process = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = gitCommand,
                    //WindowStyle = ProcessWindowStyle.Hidden,
                    //CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = "cmd.exe"
                };

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit(10000);     // Time out after one minute
            }

            Debug.Print("Created: " + gitTempFile);
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
