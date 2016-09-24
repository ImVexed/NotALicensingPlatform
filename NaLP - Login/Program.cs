using NotALicensingPlatform;
using System;
using System.Windows.Forms;

namespace NaLP___Login
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        public static void SetClient(Client c)
        {
            frmMain.client = c;
        }
    }
}