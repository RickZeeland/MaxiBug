﻿// Copyright(c) João Martiniano. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MiniBug
{
    /// <summary>
    /// Contains the main <see cref="SoftwareProject"/> with issues and tasks shared between forms.
    /// </summary>
    static class Program
    {
        public static string myName = "MiniBug v2 Issue Tracker";

        /// <summary>
        /// Postgres database name in lower case.
        /// </summary>
        public static string databaseName = "minibug1";

        /// <summary>
        /// A software project that this application will work with, contains issues and tasks.
        /// </summary>
        public static Project SoftwareProject = null;

        /// <summary>
        /// The mutex to ensure only one instance can be run.
        /// </summary>
        private static Mutex mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!IsSingleInstance())
            {
                // Only one instance allowed to run
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Only allow one instance to be run.
        /// </summary>
        /// <returns>True on success</returns>
        internal static bool IsSingleInstance()
        {
            try
            {
                // Try to open existing mutex
                Mutex.OpenExisting(myName);
            }
            catch
            {
                // Ok, only one instance
                mutex = new Mutex(true, myName);
                return true;
            }

            // More than one instance
            MessageBox.Show("Only one instance allowed to run!", myName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>
        /// Create project .lock file.
        /// </summary>
        /// <param name="fullFilename">The project file name</param>
        internal static void CreateLockFile(string fullFilename)
        {
            //try
            //{
            //    File.WriteAllText(fullFilename.Replace(".json", ".lock"), $"{Environment.UserName} {DateTime.Now}");
            //}
            //catch
            //{
            //}
        }

        /// <summary>
        /// Delete project .lock file.
        /// </summary>
        /// <param name="fullFilename">The full file name</param>
        internal static void DeleteLockFile(string fullFilename = "")
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(fullFilename))
            //    {
            //        if (SoftwareProject == null)
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            // Use current project file name
            //            fullFilename = Path.Combine(SoftwareProject.Location, SoftwareProject.Filename);
            //        }
            //    }

            //    if (File.Exists(fullFilename))
            //    {
            //        // Delete lock file
            //        File.Delete(fullFilename.Replace(".json", ".lock"));
            //    }
            //}
            //catch
            //{
            //}
        }

        /// <summary>
        /// Test if project file is locked.
        /// </summary>
        /// <param name="fullFilename">The full file name</param>
        /// <returns>True when locked</returns>
        internal static bool IsLocked(string fullFilename)
        {
            //try
            //{
            //    if (File.Exists(fullFilename.Replace(".json", ".lock")))
            //    {
            //        return true;
            //    }
            //}
            //catch
            //{
            //    return false;
            //}

            return false;
        }
    }
}
