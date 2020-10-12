using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TempMonitoring
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Listener listener = new Listener();
            listener.RunListener();

            if (!XML.CheckForXML())
                XML.CreateXML();

            XML.LoadFromXML();
            
            tb_Email.Text = AppData.CurrentSettings.EmailAddress;
            tb_IP.Text = AppData.CurrentSettings.IPAddress;
            tb_Port.Text = AppData.CurrentSettings.Port;
            tb_Temperature.Text = AppData.CurrentSettings.MaxTemperature;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LanguagePicker.SelectedIndex == 0)
            {
                AppData.CurrentSettings.Language = AppData.Language.Celcius;
            }
            else if (LanguagePicker.SelectedIndex == 1)
            {
                AppData.CurrentSettings.Language = AppData.Language.Fahrenheit;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Validation val = new Validation();

            bool isEmailValidated = val.ValidateEmail(tb_Email.Text);
            bool isIPValidated = val.ValidateIP(tb_IP.Text);
            bool isPortValidated = val.ValidatePort(tb_Port.Text);
            bool isTempValidated = val.ValidateTemperature(tb_Temperature.Text);

            if (isEmailValidated)
            {
                img_Email.Source = new BitmapImage(new Uri("../Resources/images/tick.png", UriKind.Relative));
                AppData.CurrentSettings.EmailAddress = tb_Email.Text;
            }
            else
            {
                img_Email.Source = new BitmapImage(new Uri("../Resources/images/cross.png", UriKind.Relative));
            }

            if (isIPValidated && isPortValidated)
            {
                img_IP.Source = new BitmapImage(new Uri("../Resources/images/tick.png", UriKind.Relative));
                AppData.CurrentSettings.IPAddress = tb_IP.Text;
                AppData.CurrentSettings.Port = tb_Port.Text;
            }
            else
            {
                img_IP.Source = new BitmapImage(new Uri("../Resources/images/cross.png", UriKind.Relative));
            }

            if (isTempValidated)
            {
                img_temp.Source = new BitmapImage(new Uri("../Resources/images/tick.png", UriKind.Relative));
                AppData.CurrentSettings.MaxTemperature = tb_Temperature.Text;
            }
            else
            {
                img_temp.Source = new BitmapImage(new Uri("../Resources/images/cross.png", UriKind.Relative));
            }

            if (isEmailValidated && isIPValidated && isPortValidated && isTempValidated)
                XML.SaveXML();
        }

        private void btn_SMTP_Click(object sender, RoutedEventArgs e)
        {
            SMTPWindow s = new SMTPWindow();
            s.Show();
        }
    }
}
