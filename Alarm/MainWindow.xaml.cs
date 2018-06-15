using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace Alarm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static public string file_path;
        static public double time = 9;
        private DateTime endTime;
        private DispatcherTimer timer;
        private MediaPlayer mp;

        public MainWindow()
        {
            InitializeComponent();
            labelClock.MouseDown += (s, e) => DragMove();
            InitializeSettings();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings
            {
                ShowInTaskbar = false
            };
            settings.ShowDialog();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            timer.Tick += new EventHandler(CheckTime);
            mp = new MediaPlayer();
            DateTime date = DateTime.Now;
            endTime = date.AddHours(time);
            timer.Start();
            buttonStart.IsEnabled = false;
        }

        private void CheckTime(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            TimeSpan time_dif = new TimeSpan(endTime.Ticks - date.Ticks);
            if (time_dif.Ticks <= 0)
            {
                DoAlarm();
            }
            labelClock.Content = time_dif.ToString(@"hh\:mm\:ss");
        }

        private void DoAlarm()
        {
            timer.Stop();
            if (File.Exists(file_path))
            {
                mp.Open(new Uri(file_path));
                mp.Play();
            }
            buttonStart.IsEnabled = true;
        }

        public void StopTimer()
        {
            mp.Stop();
            timer.Stop();
        }

        private void InitializeSettings()
        {
            XmlDocument xmlDocument = new XmlDocument();
            bool find_file = false;
            while (!find_file)
            {
                try
                {
                    xmlDocument.Load(Settings.pathToXml);
                    XmlNodeList elementList = xmlDocument.GetElementsByTagName("head");
                    if (elementList[0].InnerXml != "")
                    {
                        XmlNode node = xmlDocument.SelectSingleNode("//*[@id = 'time']");
                        MainWindow.time = Convert.ToDouble(node.InnerText);
                        node = xmlDocument.SelectSingleNode("//*[@id = 'music']");
                        MainWindow.file_path = Convert.ToString(node.InnerText);
                    }
                    find_file = true;
                }
                catch (System.IO.FileNotFoundException e)
                {
                    MessageBox.Show("Перед использованием, настройте будильник");
                    XmlTextWriter textWritter = new XmlTextWriter(Settings.pathToXml, Encoding.UTF8);
                    textWritter.WriteStartDocument();
                    textWritter.WriteStartElement("head");
                    textWritter.WriteEndElement();
                    textWritter.Close();
                    find_file = true;
                }
            }
        }
    }
}