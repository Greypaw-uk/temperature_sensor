using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Xml;

namespace Temperature_Monitor
{
    class SaveXml
    {
        //TODO Change to File Dialog
        //private String xmlpath = @"E:\OsborneTechDemo.xml";
        private string xmlpath;

        
        public void XMLSave()
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;

            dialog.ShowDialog();

            try
            {
                xmlpath = dialog.FileName;

                if (File.Exists(xmlpath))
                {
                    File.Delete(xmlpath);
                }

                using (XmlWriter writer = XmlWriter.Create(xmlpath))
                {
                    writer.WriteStartElement("root");

                    foreach (var thing in MainWindowDataContext.PersonList)
                    {
                        writer.WriteStartElement("Person");

                        writer.WriteStartElement("Name");
                        writer.WriteString(thing.Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Age");
                        writer.WriteString(thing.Age.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("Temperature");
                        writer.WriteString(thing.Temperature.ToString());
                        writer.WriteEndElement();

                        writer.WriteEndElement();

                        writer.Flush();
                    }

                    foreach (var thing in MainWindowDataContext.PersonListWarning)
                    {
                        writer.WriteStartElement("Person");

                        writer.WriteStartElement("Name");
                        writer.WriteString(thing.Name);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Age");
                        writer.WriteString(thing.Age.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("Temperature");
                        writer.WriteString(thing.Temperature.ToString());
                        writer.WriteEndElement();

                        writer.WriteEndElement();

                        writer.Flush();
                    }

                    writer.WriteEndElement();
                    writer.Close();
                }
            }
            
            catch
            {
                MessageBox.Show("You must specify a save location");
            }
        }
    }
}
