using System;
using System.Drawing;
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

        /// <summary>
        /// Show an empty form with only a message.
        /// </summary>
        /// <param name="message">The message to show</param>
        public SelectionForm(string message = "Searching databases")
        {
            InitializeComponent();
            this.listBox1.Enabled = false;
            this.listBox1.BackColor = Color.FromKnownColor(KnownColor.Control);
            this.Text = message;
            this.labelInfo.Text = message;
            this.Height -= (this.listBox1.Height + 20);
        }

        /// <summary>
        /// Show a form with data in a ListBox, sets the SelectedItem property.
        /// </summary>
        /// <param name="data">The ListBox data</param>
        /// <param name="title">The form title</param>
        public SelectionForm(string[] data, string title = "Select database")
        {
            InitializeComponent();
            this.Text = title;
            this.labelInfo.Text = title;
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
