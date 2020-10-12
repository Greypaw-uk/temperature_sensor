using System.Linq;
using System.Text.RegularExpressions;

namespace TempMonitoring
{
    public class Validation
    {
        public bool ValidateEmail(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public bool ValidateIP(string ip)
        {
            // Validate Wildcard
            if (ip.Equals("*"))
                return true;

            //Validate anything other than Wildcard
            string[] splitValues = ip.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public bool ValidatePort(string port)
        {
            Regex regex = new Regex("^[0-9]+$");
            return regex.IsMatch(port);
        }

        public bool ValidateTemperature(string temp)
        {
            Regex regex = new Regex(@"^[0-9]+.[0-9]+$");
            return regex.IsMatch(temp);
        }

        public bool ValidateSMTPServer(string server)
        {
            if (!string.IsNullOrWhiteSpace(server))
                return true;

            return false;
        }

        public bool ValidateSMTPPort(string port)
        {
            Regex regex = new Regex("^[0-9]+$");
            return regex.IsMatch(port);
        }

        public bool ValidateSMTPUsername(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
                return true;

            return false;
        }

        public bool ValidateSMTPPassword(string password)
        {
            if (!string.IsNullOrWhiteSpace(password))
                return true;

            return false;
        }
    }
}
