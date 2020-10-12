using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TempMonitoring
{
    public partial class SMTPWindow : Window
    {
        public SMTPWindow()
        {
            InitializeComponent();

            if (string.IsNullOrWhiteSpace(Crypto.DecryptString(AppData.CurrentSettings.AdminPassword)))
            {
                sp_setPassword.Visibility = Visibility.Visible;
                sp_password.Visibility = Visibility.Hidden;
            }
            else
            {
                sp_setPassword.Visibility = Visibility.Hidden;
                sp_password.Visibility = Visibility.Visible;
            }

            tb_sAddress.Text = AppData.CurrentSettings.SmtpServer;
            tb_sPort.Text = AppData.CurrentSettings.SmtpPort;
            tb_sUsername.Text = AppData.CurrentSettings.SmtpUsername;
            
            pw_password.Password = "";

            pw_sPassword.Password = Crypto.DecryptString(AppData.CurrentSettings.SmptPassword);
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (pw_password.Password.Equals(Crypto.DecryptString(AppData.CurrentSettings.AdminPassword)))
            {
                sp_password.Visibility = Visibility.Hidden;
                sp_settings.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Password incorrect");
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_Cancel2_Click(object sender, RoutedEventArgs e)
        {
            sp_password.Visibility = Visibility.Visible;
            sp_settings.Visibility = Visibility.Hidden;

            Close();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Validation val = new Validation();
            bool isServerValidated = val.ValidateSMTPServer(tb_sAddress.Text);
            bool isServerPortValidated = val.ValidateSMTPPort(tb_sPort.Text);
            bool isUsernameValidated = val.ValidateSMTPUsername(tb_sUsername.Text);
            bool isPasswordValidated = val.ValidateSMTPPassword(pw_sPassword.Password);

            if (isServerValidated)
            {
                AppData.CurrentSettings.SmtpServer = tb_sAddress.Text;
                img_address.Source = new BitmapImage(new Uri("../Resources/images/tick.png", UriKind.Relative));
            }
            else
            {
                img_address.Source = new BitmapImage(new Uri("../Resources/images/cross.png", UriKind.Relative));
            }

            if (isServerPortValidated)
            {
                AppData.CurrentSettings.SmtpPort = tb_sPort.Text;
                img_port.Source = new BitmapImage(new Uri("../Resources/images/tick.png", UriKind.Relative));
            }
            else
            {
                img_port.Source = new BitmapImage(new Uri("../Resources/images/cross.png", UriKind.Relative));
            }

            if (isUsernameValidated)
            {
                AppData.CurrentSettings.SmtpUsername = tb_sUsername.Text;
                img_username.Source = new BitmapImage(new Uri("../Resources/images/tick.png", UriKind.Relative));
            }
            else
            {
                img_username.Source = new BitmapImage(new Uri("../Resources/images/cross.png", UriKind.Relative));
            }

            if (isPasswordValidated)
            {
                AppData.CurrentSettings.SmptPassword = Crypto.EncryptString(pw_sPassword.Password);
                img_password.Source = new BitmapImage(new Uri("../Resources/images/tick.png", UriKind.Relative));
            }
            else
            {
                img_password.Source = new BitmapImage(new Uri("../Resources/images/cross.png", UriKind.Relative));
            }

            if (isServerValidated && isServerPortValidated && isUsernameValidated && isPasswordValidated)
                XML.SaveXML();
        }

        private void btn_setPassword_Click(object sender, RoutedEventArgs e)
        {
            if (pw_input.Password.Equals(pw_confirm.Password))
            {
                if (pw_input.Password.Length >= 8)
                {
                    AppData.CurrentSettings.AdminPassword = Crypto.EncryptString(pw_input.Password);
                    XML.SaveXML();

                    sp_setPassword.Visibility = Visibility.Hidden;
                    sp_password.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Password must be at least 8 characters long");
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match");
            }
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            sp_password.Visibility = Visibility.Hidden;
            sp_ResetPassword.Visibility = Visibility.Visible;
        }

        private void btn_ResetOkay_Click(object sender, RoutedEventArgs e)
        {
            if (!pw_ResetOld.Password.Equals(Crypto.DecryptString(AppData.CurrentSettings.AdminPassword)))
            {
                MessageBox.Show("Current password is incorrect");
            }
            else
            {
                var input = pw_ResetNew.Password;
                var confirm = pw_ResetConfirm.Password;

                if (input.Equals(confirm))
                {
                    AppData.CurrentSettings.AdminPassword = Crypto.EncryptString(input);
                    XML.SaveXML();

                    sp_ResetPassword.Visibility = Visibility.Hidden;
                    sp_password.Visibility = Visibility.Visible;

                    AppData.CurrentSettings.AdminPassword = Crypto.EncryptString(pw_ResetNew.Password);
                    XML.SaveXML();

                    pw_password.Password = "";
                    MessageBox.Show("Password has been changed");
                }
                else
                {
                    MessageBox.Show("Passwords did not match");
                }
            }

        }

        private void btn_ResetCancel_Click(object sender, RoutedEventArgs e)
        {
            sp_password.Visibility = Visibility.Visible;
            sp_ResetPassword.Visibility = Visibility.Hidden;
        }
    }
}
