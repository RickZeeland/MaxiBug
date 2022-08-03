﻿// Copyright(c) João Martiniano. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows.Forms;

namespace MaxiBug
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.Text = "About MaxiBug";
            this.lblApplicationName.Text = Program.myName;
            this.AcceptButton = btOK;

            pictureBox1.Left = (lblApplicationName.Left / 2) - (pictureBox1.Width / 2);

            lblVersion.Text = $"Version {Application.ProductVersion}";
        }

        /// <summary>
        /// Open a web browser when the user clicks on the link label.
        /// </summary>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        /// <summary>
        /// Close this form.
        /// </summary>
        private void btOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
