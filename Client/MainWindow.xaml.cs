using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        readonly RemoteProvider c = new RemoteProvider("localhost", 1337);

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            lblpreauth.Content = await c.ProtectedFunction();

            if (await c.Login("Killpot", Encoding.UTF8.GetBytes("pass1word")))
                lblpostauth.Content = await c.ProtectedFunction();
        }
    }
}
