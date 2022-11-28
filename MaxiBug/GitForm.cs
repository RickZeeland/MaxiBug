using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            // Local tabpage
            this.txtGitFolder.Text = ApplicationSettings.GitFolder;
            this.txtGitCommand.Text = ApplicationSettings.GitCommand.Replace("{issue_id}", ID.ToString());
            this.labelGitInfoAxo.Visible = this.txtGitCommand.Text.Contains("axo.:");                       // Show or hide default axosoft examples

            //// GitHub tabpage
            //this.txtGitHubUserName.Text = string.Empty;
            //this.txtGitHubToken.Text = string.Empty;
            //this.txtGitHubRepo.Text = string.Empty;
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
        /// Run Git log or GitHub API command and save to a temporary text file.
        /// </summary>
        private async void btOk_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string gitTempFile = Path.Combine(Application.StartupPath, "git_temp.txt");         // Used by IssueForm

                if (File.Exists(gitTempFile))
                {
                    File.Delete(gitTempFile);
                }

                if (this.tabControlGit.SelectedIndex == 0)          // Local
                {
                    // Working directory search, replace the {path} and {issue_id} placeholders
                    gitTempFile = "\"" + gitTempFile + "\"";
                    string gitCommand = this.txtGitCommand.Text.Replace("{path}", this.txtGitFolder.Text);
                    gitCommand = gitCommand.Replace("{issue_id}", ID.ToString());
                    this.RunGitCommand(gitCommand, gitTempFile);
                }
                else if (!string.IsNullOrEmpty(this.txtGitHubUserName.Text) && !string.IsNullOrEmpty(this.txtGitHubRepo.Text))
                {
                    // GitHub API with token authentication
                    string gitCommand = this.txtGitHubRepo.Text.Replace("{issue_id}", ID.ToString());
                    await Task.Run(() => DownloadFromGitHub(gitCommand, gitTempFile, this.txtGitHubUserName.Text, this.txtGitHubToken.Text));
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
        /// <param name="userName">GitHub user name (not used at the moment)</param>
        /// <param name="auth_token">GitHub authentication token</param>
        private async Task DownloadFromGitHub(string api_url, string fullFileName, string userName, string auth_token)
        {
            if (!api_url.StartsWith(@"https://"))
            {
                api_url = @"https://" + api_url;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(api_url);

            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("MaxiBug", Application.ProductVersion));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", auth_token);

            var response = await client.GetAsync(api_url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON string and save to fullFileName
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(content)!;
            var items = dynamicObject.items;
            var sb = new StringBuilder();

            foreach (var item in items)
            {
                var commit = item.commit;
                string message = commit.message;
                string committerName = commit.committer.name;
                string committerDate = commit.committer.date;
                Debug.Print($"{committerDate} {committerName}");
                Debug.Print(message);
                Debug.Print("");

                sb.AppendLine($"{committerDate} {committerName}");
                sb.AppendLine(message);
                sb.AppendLine();
            }

            File.WriteAllText(fullFileName, sb.ToString());
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
                Debug.Print("GitForm linkLabel1_LinkClicked() error");
            }
        }

        /// <summary>
        /// Show GitHub personal access token help in browser.
        /// </summary>
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.TopMost = false;
                Process.Start(@"https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token#creating-a-personal-access-token-classic");
            }
            catch
            {
                Debug.Print("GitForm linkLabel2_LinkClicked() error");
            }
        }

        /// <summary>
        /// Toggle password char.
        /// </summary>
        private void buttonEye_Click(object sender, EventArgs e)
        {
            if (this.txtGitHubToken.PasswordChar == '●')
            {
                this.txtGitHubToken.PasswordChar = '\0';
            }
            else
            {
                this.txtGitHubToken.PasswordChar = '●';
            }
        }
    }
}
