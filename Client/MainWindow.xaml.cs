using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly RemoteProvider c = new RemoteProvider("localhost", 1337);

        private async void ButtonLogin_ClickAsync(object sender, RoutedEventArgs e)
        {
            var username = TextBoxLoginUsername.Text;
            var password = Encoding.UTF8.GetBytes(TextBoxLoginPassword.Text);
            if (await c.Login(username, password))
            {
                LabelPostAuth.Content = $"Welcome back, {TextBoxLoginUsername.Text}";
                LabelLoggedIn.Content = $"Logged in as, {TextBoxLoginUsername.Text}";
                ButtonLogout.Visibility = Visibility.Visible;
            }
            else
            {
                LabelPostAuth.Content = "There was an error logging into your account!";
            }
        }

        private async void ButtonRegister_ClickAsync(object sender, RoutedEventArgs e)
        {
            var username = TextBoxRegisterUsername.Text;
            var password = Encoding.UTF8.GetBytes(TextBoxRegisterPassword.Text);
            var key = TextBoxKey.Text;

            if (await c.Register(username, password, key))
            {
                LabelPostRegister.Content = $"Thank you for registering {TextBoxRegisterUsername.Text}";
            }
            else
            {
                LabelPostRegister.Content = "There was an error registering your account!";
            }
        }

        private async void ButtonGetSecretData_ClickAsync(object sender, RoutedEventArgs e)
        {
            LabelSecretData.Content = await c.ProtectedFunction();
        }

        private async void ButtonLogout_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (await c.Logout())
            {
                LabelLoggedIn.Content = string.Empty;
                ButtonLogout.Visibility = Visibility.Hidden;
            }
        }
    }
}