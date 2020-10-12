using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TempMonitoring
{
    class Listener
    {
        public async void RunListener()
        {
            await Task.Run(() =>
            {
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(AppData.Prefix);
                try
                {
                    listener.Start();
                }
                catch
                {
                    return;
                }

                while (listener.IsListening)
                {
                    var context = listener.GetContext();
                    ProcessRequest(context);
                }

                listener.Close();
            });
        }

        public void ProcessRequest(HttpListenerContext context)
        {
            // Get the data from the HTTP stream
            AppData.Message = new StreamReader(context.Request.InputStream).ReadToEnd();
            AppData.NewScanResult = JsonConvert.DeserializeObject<ScanResult>(AppData.Message);

            float temp = float.Parse(AppData.NewScanResult.Temperature);
            float maxTemp = float.Parse(AppData.CurrentSettings.MaxTemperature);

            if (temp >= maxTemp)
            {
                Email email = new Email();
                email.CreateMailItem();
            }
        }
    }
}
