using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuickLauncher
{
    /// <summary>
    /// Interaction logic for ActionSetting.xaml
    /// </summary>
    public partial class ActionSetting : Window
    {
        int _ActionNo;
        string _ConfigFilePath;

        public ActionSetting(int actionNo)
        {
            InitializeComponent();
            _ActionNo = actionNo;
            _ConfigFilePath = "Action" + _ActionNo + "Config.txt";
            Title += " - Action " + actionNo;

            // Centre window on screen
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);

            // Load config
            if (File.Exists(_ConfigFilePath)) LoadConfig();
        }

        private void LoadConfig()
        {
            using (StreamReader file = new StreamReader(_ConfigFilePath))
            {
                List<string> config = new List<string>();
                while (!file.EndOfStream) { config.Add(file.ReadLine()); }
                TxtActionName.Text = config[0];
                TxtExecute.Text = config[1];
                for (int i = 2; i < config.Count; i++) { TxtExecute.Text += "\r\n" + config[i]; }
            }
        }

        private void SaveConfig()
        {
            if (TxtExecute.Text.Equals(string.Empty))
            {
                MessageBox.Show("Please input execute text.", "Message");
                return;
            }

            using (StreamWriter file = new StreamWriter(_ConfigFilePath))
            {
                file.WriteLine(TxtActionName.Text);
                file.Write(TxtExecute.Text);
            }
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) SaveConfig();
            if (e.Key == Key.LeftCtrl && Keyboard.IsKeyDown(Key.S)) SaveConfig();
            if (e.Key == Key.Escape) Close();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(_ConfigFilePath)) File.Delete(_ConfigFilePath);
            Close();
        }
    }
}
