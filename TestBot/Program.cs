using System;
using System.Windows.Forms;

namespace MaxiBug
{
    internal static class Program
    {
        public static string myName = "MaxiBug TestBot";

        /// <summary>
        /// Postgres database name in lower case.
        /// </summary>
        public static string databaseName = "maxibug1";

        public static string postgresIpaddress;

        public static int postgresPort;

        public static string postgresUser;

        public static string postgresPassword;

        /// <summary>
        /// A software project that this application will work with, contains issues and tasks.
        /// </summary>
        public static Project SoftwareProject = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
