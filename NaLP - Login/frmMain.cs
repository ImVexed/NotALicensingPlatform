using NotALicensingPlatform;
using System;
using System.Windows.Forms;

namespace NaLP___Login
{
    public partial class frmMain : Form
    {
        public static Client client = null;

        private bool DoLogin(string usr, string pwd) =>
            client.RemoteCall<bool>("Login", usr, pwd);

        // This is out secure function that only logged in users can call (Well, thats for the server to decide)
        private string DoSecure() =>
            client.RemoteCall<string>("1337");

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (DoLogin(txtUsername.Text, txtPassword.Text))
                MessageBox.Show(DoSecure());
            else
            {
                MessageBox.Show("Uhh ohh, incorrect login! We're gonna call the function anyway and see what it does!");
                MessageBox.Show(DoSecure());
            }
        }
    }
}