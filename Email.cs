using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TempMonitoring
{
    public class Email
    {
        public void CreateMailItem()
        {
            MailAddress to = new MailAddress(AppData.CurrentSettings.EmailAddress);
            MailAddress from = new MailAddress(AppData.CurrentSettings.EmailAddress);

            MailMessage message = new MailMessage
            {
                From = from,
                Subject = AppData.EmailSubject,
                IsBodyHtml = true,
                Body = CreateBody(AppData.NewScanResult)
            };

            message.To.Add(to);

            SmtpClient client = new SmtpClient(AppData.CurrentSettings.SmtpServer, int.Parse(AppData.CurrentSettings.SmtpPort))
            {
                Credentials = new NetworkCredential(AppData.CurrentSettings.SmtpUsername, Crypto.DecryptString(AppData.CurrentSettings.SmptPassword)),
                EnableSsl = true
            };

            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string CreateBody(ScanResult r)
        {
            //Parse the date time 
            long unixDate = long.Parse(r.CheckTime);
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(
                "<!DOCTYPE html PUBLIC “-//W3C//DTD XHTML 1.0 Transitional//EN” “https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd”>");
            sb.Append("<html xmlns=“https://www.w3.org/1999/xhtml”>");
            sb.Append("<P> <H1> A user has displayed an elevated Temperature. </H1> </P>");

            //Add date/time to email
            sb.Append("<P>" + string.Format("Time: {0}", date.ToString()) + "</P>");

            //Add temperature to email after conversion
            var temp = ConvertTemp(r.Temperature);
            sb.Append("<P>" + string.Format("Temperature: {0}", temp) + "</P>");

            //Add image to email
            sb.Append("<P> <img src=\"data:image;base64," + r.CheckPic + "\" /> </P>");
            sb.Append("<html>");

            return sb.ToString();
        }

        private string ConvertTemp(string temp)
        {
            if (AppData.CurrentSettings.Language == AppData.Language.Fahrenheit)
            {
                return string.Format("{0:F1}", ((float.Parse(temp)) * 9) / 5 + 32);
            }

            return temp;
        }
    }
}
