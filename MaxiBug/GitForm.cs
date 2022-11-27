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
        /// Gets or Sets the current issue id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Initialize the GitForm.
        /// </summary>
        /// <param name="id">The issue id number to retrieve from Git tags</param>
        public GitForm(int id)
        {
            InitializeComponent();
            this.ID = id;
            this.InitForm();
        }

        /// <summary>
        /// Get the default settings and replace the {issue_id} placeholder.
        /// </summary>
        private void InitForm()
        {
            this.txtGitFolder.Text = ApplicationSettings.GitFolder;
            this.txtGitCommand.Text = ApplicationSettings.GitCommand.Replace("{issue_id}", ID.ToString());
            this.labelGitInfoAxo.Visible = this.txtGitCommand.Text.Contains("axo.:");                       // Show or hide default axosoft examples
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
        /// </summary>
        private void btOk_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string gitCommand = this.txtGitCommand.Text.Replace("{path}", this.txtGitFolder.Text);
                gitCommand = gitCommand.Replace("{issue_id}", ID.ToString());
                string gitTempFile = "\"" + Path.Combine(Application.StartupPath, "git_temp.txt") + "\"";

                if (!string.IsNullOrEmpty(gitTempFile) && File.Exists(gitTempFile))
                {
                    File.Delete(gitTempFile);
                }

                if (gitCommand.Contains("api.github.com"))
                {
                    // TODO: GitHub authentication
                    this.DownloadFromGitHub(gitCommand, gitTempFile, 0, "");
                }
                else
                {
                    // Working directory search
                    this.RunGitCommand(gitCommand, gitTempFile);
                }

                Debug.Print("Created: " + gitTempFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

            this.Cursor= Cursors.Default;
            this.Close();
        }

        /// <summary>
        /// Run Git command, e.g. git log and redirect the output to a file.
        /// </summary>
        /// <param name="gitCommand">Git command</param>
        /// <param name="fullFileName">Output file name</param>
        private void RunGitCommand(string gitCommand, string fullFileName)
        {
            gitCommand += " > " + fullFileName;
            Debug.Print(gitCommand);

            // Note that the DOS command must start with /C for cmd.exe to work
            if (!gitCommand.StartsWith(@"/C"))
            {
                gitCommand = @"/C " + gitCommand;
            }

            using (Process process = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = gitCommand,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    FileName = "cmd.exe"
                };

                process.StartInfo = startInfo;
                process.Start();
                string stderr = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(stderr))
                {
                    MessageBox.Show(stderr, Program.myName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }

                process.WaitForExit(30000);     // Time out after 30 seconds
            }
        }

        /// <summary>
        /// Use the GitHub API to download a file.
        /// See: https://docs.github.com/en/rest/commits
        /// </summary>
        /// <param name="api_url">e.g. "api.github.com/search/commits?q=repo:RickZeeland/MaxiBug+Bumped"</param>
        /// <param name="fullFileName">Full path to git_temp.txt</param>
        /// <param name="userId">GitHub user id</param>
        /// <param name="auth_token">GitHub authentication token</param>
        private void DownloadFromGitHub(string api_url, string fullFileName, int userId, string auth_token)
        {
            if (!api_url.StartsWith(@"https://"))
            {
                api_url = @"https://" + api_url;
            }

            this.TopMost = false;
            Process.Start(api_url);                // Test in browser (which takes care of authentication)
            return;

            //var uri = new Uri(api_url);
            //var client = new WebClient();

            ////using var client = new HttpClient();
            ////var result = await client.GetAsync(gitCommand);

            //// Add a user agent header as the requested URI contains a query.
            //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            //if (!string.IsNullOrEmpty(auth_token))
            //{
            //    client.Headers.Add("auth_token", auth_token);
            //    client.QueryString.Add("userId", userId.ToString());
            //}

            //client.DownloadFile(uri, fullFileName);

            ////using Stream data = client.OpenRead(api_url);
            ////using StreamReader reader = new StreamReader(data);
            ////string s = reader.ReadToEnd();
            ////Debug.Print(s);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            string gitTempFile = Path.Combine(Application.StartupPath, "git_temp.txt");

            if (File.Exists(gitTempFile))
            {
                File.Delete(gitTempFile);
            }

            this.Close();
        }

        /// <summary>
        /// Show git log help in browser.
        /// </summary>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.TopMost = false;
                Process.Start(@"https://git-scm.com/docs/git-log");
            }
            catch
            {
                Debug.Print("linkLabel1_LinkClicked() error");
            }
        }
    }
}
