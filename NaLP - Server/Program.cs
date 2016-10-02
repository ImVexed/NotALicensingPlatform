using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace NaLP___Server
{
    internal class Program
    {

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}