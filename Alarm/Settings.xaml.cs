using System;
using System.Text;
using System.Windows;
using System.Xml;

namespace Alarm
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Settings : Window
    {

        XmlDocument xmlDocument = new XmlDocument();
        static public string pathToXml = "settings.xml";

        public Settings()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            bool find_file = false;
            while (!find_file)
            {
                try
                {
                    xmlDocument.Load(pathToXml);
                    XmlNodeList elementList = xmlDocument.GetElementsByTagName("head");
                    if(elementList[0].InnerXml == "")
                    {
                        XmlNode element = xmlDocument.CreateElement("data");
                        XmlAttribute attribute = xmlDocument.CreateAttribute("id");
                        attribute.Value = "time";
                        element.InnerText = Convert.ToString(MainWindow.time);
                        element.Attributes.Append(attribute);
                        xmlDocument.DocumentElement.AppendChild(element);
                        element = xmlDocument.CreateElement("data");
                        attribute = xmlDocument.CreateAttribute("id");
                        attribute.Value = "music";
                        element.Attributes.Append(attribute);
                        element.InnerText = Convert.ToString(MainWindow.file_path);
                        xmlDocument.DocumentElement.AppendChild(element);
                        xmlDocument.Save(pathToXml);
                    } else
                    {
                        XmlNode node = xmlDocument.SelectSingleNode("//*[@id = 'time']");
                        MainWindow.time = Convert.ToDouble(node.InnerText);
                        node = xmlDocument.SelectSingleNode("//*[@id = 'music']");
                        MainWindow.file_path = Convert.ToString(node.InnerText);
                        tbMaxTime.Text = Convert.ToString(MainWindow.time);
                        strFilePath.Text = Convert.ToString(MainWindow.file_path);
                    }
                    find_file = true;
                }
                catch (System.IO.FileNotFoundException e)
                {
                    XmlTextWriter textWritter = new XmlTextWriter(pathToXml, Encoding.UTF8);
                    textWritter.WriteStartDocument();
                    textWritter.WriteStartElement("head");
                    textWritter.WriteEndElement();
                    textWritter.Close();
                    find_file = true;
                }
            }
        }

        private void buttonGetFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mp3";
            dlg.Filter = "MP3 files (.mp3)|*.mp3|Wav files (.wav)|*.wav";
            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                string filename = dlg.FileName;
                MainWindow.file_path = filename;
                strFilePath.Text = filename;
            }
        }

        private void buttonSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            double time = Double.Parse(tbMaxTime.Text);
            MainWindow.time = time;

            XmlNode node = xmlDocument.SelectSingleNode("//*[@id = 'time']");
            node.InnerText = Convert.ToString(MainWindow.time);
            node = xmlDocument.SelectSingleNode("//*[@id = 'music']");
            node.InnerText = Convert.ToString(MainWindow.file_path);
            xmlDocument.Save(pathToXml);

            Close();
        }
    }
}
