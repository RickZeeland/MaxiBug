using System;
using System.Windows.Forms;

namespace MaxiBug
{
    /// <summary>
    /// Displays a selection ListBox.
    /// </summary>
    public partial class SelectionForm : Form
    {
        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public string SelectedItem { get; private set; } = string.Empty;

        public SelectionForm(string[] data, string title = "Select")
        {
            InitializeComponent();
            this.Text = title;
            this.listBox1.DataSource = data;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedItem = listBox1.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
