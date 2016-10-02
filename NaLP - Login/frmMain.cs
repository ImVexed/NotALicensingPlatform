using NotALicensingPlatform;
using System;
using System.Windows.Forms;

namespace NaLP___Login
{
    public partial class frmMain : Form
    {
        public static Client client = null;

        private int DoLogin(string usr, string pwd) =>
            client.RemoteCall<int>("Login", usr, pwd, Security.FingerPrint.Value());

        private int DoRegister(string usr, string pwd, string key) =>
            client.RemoteCall<int>("Register", usr, pwd, Security.FingerPrint.Value(), key);

        private int DoActivate(string key) =>
            client.RemoteCall<int>("ActivateKey", key);

        // This is out secure function that only logged in users can call (Well, thats for the server to decide)
        private string DoSecure() =>
            client.RemoteCall<string>("1337");

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                switch (DoLogin(txtUsername.Text, txtPassword.Text))
                {
                    case clsResults.SUCCESS:
                        MessageBox.Show("You've been logged in successfully!");
                        MessageBox.Show(DoSecure());
                        break;

                    case clsResults.SUCCESS_EXPIRED:
                        MessageBox.Show("You've been logged in successfully, however your account is expired and your access is limited!");
                        MessageBox.Show(DoSecure());
                        break;

                    case clsResults.INVALID_USERNAME:
                        MessageBox.Show("Invalid username!");
                        break;

                    case clsResults.INVALID_PASSWORD:
                        MessageBox.Show("Invalid password!");
                        break;

                    case clsResults.HWID_MISMATCH:
                        MessageBox.Show("This account was not created on this system! Booooo no Multi-ing >:(");
                        break;

                    case clsResults.BANNED:
                        MessageBox.Show("You are banned!");
                        break;

                    case clsResults.UNKNOWN_ERROR:
                        MessageBox.Show("An unknown error occurred on the server! Oops!");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                switch (DoRegister(txtRUsername.Text, txtRPassword.Text, txtRKey.Text))
                {
                    case clsResults.SUCCESS:
                        MessageBox.Show("You have been successfully registered & logged in!");
                        MessageBox.Show(DoSecure());
                        break;

                    case clsResults.ERR_USER_EXISTS:
                        MessageBox.Show("A user with that name already exists! Pick another!");
                        break;

                    case clsResults.INVALID_KEY:
                        MessageBox.Show("Invalid key, next time use a legit one!");
                        break;

                    case clsResults.BANNED:
                        MessageBox.Show("You are banned!");
                        break;

                    case clsResults.UNKNOWN_ERROR:
                        MessageBox.Show("An unknown error occurred on the server! Oops!");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            try
            {
                switch (DoActivate(txtActivateKey.Text))
                {
                    case clsResults.SUCCESS:
                        MessageBox.Show("Key applied to account!");
                        break;

                    case clsResults.INVALID_KEY:
                        MessageBox.Show("Key is invalid!");
                        break;

                    case clsResults.UNKNOWN_ERROR:
                        MessageBox.Show("Unknown error, you shouldn't be here! Are you even logged in?");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public static class clsResults
    {
        public const int SUCCESS = 0x00;
        public const int UNKNOWN_ERROR = 0x01;
        public const int ERR_USER_EXISTS = 0x02;
        public const int INVALID_KEY = 0x03;
        public const int HWID_MISMATCH = 0x04;
        public const int INVALID_USERNAME = 0x05;
        public const int INVALID_PASSWORD = 0x06;
        public const int SUCCESS_EXPIRED = 0x07;
        public const int BANNED = 0x08;
    }
}