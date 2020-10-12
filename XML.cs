using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TempMonitoring
{
    public static class XML
    {
        static readonly string xmlPath = Path.Combine(Directory.GetCurrentDirectory(), ".\\config.xml");

        public static bool CheckForXML()
        {
            return File.Exists(xmlPath);
        }

        public static void CreateXML()
        {
            using (var xmlWriter = XmlWriter.Create(xmlPath))
            {
                new XmlSerializer(typeof(AppSettings)).Serialize(xmlWriter, AppData.CurrentSettings);
            }
        }

        public static void LoadFromXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            FileStream fs = new FileStream(xmlPath, FileMode.Open);
            AppSettings settings = (AppSettings)serializer.Deserialize(fs);

            AppData.UpdateDefaultSettings(settings);
        }

        public static void SaveXML()
        {
            if(File.Exists(xmlPath))
                File.Delete(xmlPath);

            using (var xmlWriter = XmlWriter.Create(xmlPath))
            {
                new XmlSerializer(typeof(AppSettings)).Serialize(xmlWriter, AppData.CurrentSettings);
            }
        }
    }
}
