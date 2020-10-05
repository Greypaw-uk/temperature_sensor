using System;
using System.Windows;

namespace Temperature_Monitor
{
    public partial class ConfigForm : Window
    {
        ConfigFormDataContex config = new ConfigFormDataContex();

        public ConfigForm()
        {
            InitializeComponent();

            config.BaseUrl = RetainedValues.retainedBaseUrl;
            config.WarningTemperature = RetainedValues.retainedWarningTemperature;

            tb_Endpoint.Text = config.BaseUrl;
            tb_Warn.Text = config.WarningTemperature.ToString();
        }

        private void Btn_Save_OnClick(object sender, RoutedEventArgs e)
        {
            RetainedValues.retainedBaseUrl = tb_Endpoint.Text;
            RetainedValues.retainedWarningTemperature = Convert.ToDecimal(tb_Warn.Text);

            config.BaseUrl = tb_Endpoint.Text;
            config.WarningTemperature = Convert.ToDecimal(tb_Warn.Text);


            Console.WriteLine("Base URL: " + config.BaseUrl);
            Console.WriteLine("Base warning: " + config.WarningTemperature);

            Close();
        }

        private void Btn_Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
