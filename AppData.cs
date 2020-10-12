using System;

namespace TempMonitoring
{
    public class AppSettings
    {
        public string EmailAddress { get; set; }
        public string IPAddress { get; set; }
        public string Port { get; set; }
        public string MaxTemperature { get; set; }

        public AppData.Language Language { get; set; }

        public string AdminPassword { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmptPassword { get; set; }
    }

    public class ScanResult
    {
        public string CheckTime { get; set; }
        public string Temperature { get; set; }
        public string CheckPic { get; set; }
    }

    public static class AppData
    {
        public static AppSettings CurrentSettings = new AppSettings();

        public static string Message { get; set; }
        public static ScanResult NewScanResult;

        public static string Prefix = "http://*:4333/";
        public static string EmailSubject = "Test";

        public enum Language
        {
            Celcius,
            Fahrenheit
        }

        public static void UpdateDefaultSettings(AppSettings settings)
        {
            CurrentSettings = settings;
        }
    }
}
