using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RescueApp.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {
        public Login()
        {
            InitializeComponent();
            txtMessage.Visibility = Visibility.Hidden;

            pwdPassword.KeyUp += PwdPassword_KeyUp;
            txtUsername.KeyUp += PwdPassword_KeyUp;
        }

        private void PwdPassword_KeyUp(object sender, KeyEventArgs e)
        {
            txtMessage.Visibility = Visibility.Hidden;
            e.Handled = false;
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "admin" && pwdPassword.Password == "pass")
            {
                txtMessage.Text = "Please wait while logging in...";
                txtMessage.Visibility = Visibility.Visible;
                MainWindow _mainWindow = null;
                await Task.Run(() =>
                 {
                     DispatcherHelper.CheckBeginInvokeOnUI(() =>
                     {
                         _mainWindow = new MainWindow();
                         _mainWindow.Show();
                     });

                 });

                Close();
            }
            else
            {
                txtMessage.Visibility = Visibility.Visible;
            }
        }
    }
}
