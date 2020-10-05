using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;

namespace Temperature_Monitor
{
    public partial class MainWindow : Window
    {
        private static HttpClient httpClient = new HttpClient();

        MainWindowDataContext context = new MainWindowDataContext();
        ConfigFormDataContex config = new ConfigFormDataContex();

        public MainWindow()
        {
            config.BaseUrl = "https://dev.osbornetechnologies.co.uk/project/";
            config.WarningTemperature = 37.1m;

            InitializeComponent();
            DataContext = context;
        }

        private async void GetJson(object sender, RoutedEventArgs e)
        {
            // Clear the observable collection to avoid duplicates
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => MainWindowDataContext.PersonList.Clear()));
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => MainWindowDataContext.PersonListWarning.Clear()));

            //GET JSON and convert to JObject
            var response = await httpClient.GetAsync(config.BaseUrl).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var myObject = JObject.Parse(responseString);

            //Get JTokens from JObject and return as a list
            var people = myObject
                                        .Children()
                                        .SelectMany(jt => jt.Children())
                                        .Select(c => JsonConvert.DeserializeObject<Person>(c.ToString()))
                                        .ToList();

            //Create new Person from previous list
            foreach (var person in people)
            {
                if (person.Temperature <= RetainedValues.retainedWarningTemperature)
                {
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new Action(() => MainWindowDataContext.PersonList.Add(new Person
                            {
                                Name = person.Name,
                                Age = person.Age,
                                Temperature = person.Temperature})
                        ));
                }
                else
                {
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new Action(() => MainWindowDataContext.PersonListWarning.Add(new Person
                            {
                                Name = person.Name,
                                Age = person.Age,
                                Temperature = person.Temperature
                            })
                        ));
                }
            }

            //Present Average Age
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => CalculateAverageAge()));

        }

        private void SaveXml(object sender, RoutedEventArgs e)
        {
            SaveXml xml = new SaveXml();
            xml.XMLSave();
        }

        private void OpenConfig(object sender, RoutedEventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.Show();
        }

        private void CalculateAverageAge()
        {
            float number = 0f;
            float totalAge = 0f;

            foreach (var person in MainWindowDataContext.PersonList)
            {
                number++;
                totalAge += person.Age;
            }

            foreach (var person in MainWindowDataContext.PersonListWarning)
            {
                number++;
                totalAge += person.Age;
            }

            float averageAge = totalAge / number;
            tb_Age.Text = averageAge.ToString();
        }
    }
}
