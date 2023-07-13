using QuickLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> Notes = new List<string>();
        string NoteFolderPath = @"C:\Users\" + Environment.UserName + @"\iCloudDrive\Storage\Note\";

        // Default text editor can be setup with config file
        // Example: default_text_editor,C:\Program Files\Sublime Text\sublime_text.exe
        string DefaultTextEditor = "";

        public MainWindow()
        {
            InitializeComponent();

            // Centre window on screen
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);

            // Load config
            SetEnableNote(SimpleConfigUtils.IsTrue("enable_note"));
            DefaultTextEditor = SimpleConfigUtils.GetConfig("default_text_editor");

            // Load action name
            RefreshBtnContent();

            // Load note list
            var noteFilePaths = Directory.GetFiles(NoteFolderPath, "*.txt");
            foreach (var noteFilePath in noteFilePaths)
            {
                string noteName = noteFilePath.Replace(".txt", "").Replace(NoteFolderPath, "");
                
                // Ignore README
                if (noteName.Contains("README")) continue;

                Notes.Add(noteName);
                CmbNoteName.Items.Add(noteName);
            }
        }

        private void SetEnableNote(bool? isEnable)
        {
            if (isEnable == null || (bool)!isEnable)
            {
                LblCreateNote.Visibility = Visibility.Collapsed;
                CmbNoteName.Visibility = Visibility.Collapsed;
                BtnExecute.Visibility = Visibility.Collapsed;
                WindowMain.Height = WindowMain.Height - 40;
            }
        }

        private void ExecuteAction(int actionNo)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                ActionSetting actionSetting = new ActionSetting(actionNo);
                actionSetting.ShowDialog();
                RefreshBtnContent();
                return;
            }
            string executeContent = GetActionExecuteContent(actionNo);
            if (!executeContent.Equals(string.Empty)) Cmd.ExecuteCommand(executeContent);
            Close();
        }

        private void RefreshBtnContent()
        {
            BtnAction1Text.Text = "Action 1\r\n" + GetActionName(1);
            BtnAction2Text.Text = "Action 2\r\n" + GetActionName(2);
            BtnAction3Text.Text = "Action 3\r\n" + GetActionName(3);
            BtnAction4Text.Text = "Action 4\r\n" + GetActionName(4);
            BtnAction5Text.Text = "Action 5\r\n" + GetActionName(5);
            BtnAction6Text.Text = "Action 6\r\n" + GetActionName(6);
            BtnAction7Text.Text = "Action 7\r\n" + GetActionName(7);
            BtnAction8Text.Text = "Action 8\r\n" + GetActionName(8);
            BtnAction9Text.Text = "Action 9\r\n" + GetActionName(9);
        }

        private string GetActionName(int actionNo)
        {
            string configFilePath = "Action" + actionNo + "Config.txt";
            string actionName = string.Empty;
            if (!File.Exists(configFilePath)) return actionName;

            using (StreamReader file = new StreamReader(configFilePath))
            {
                List<string> config = new List<string>();
                while (!file.EndOfStream) { config.Add(file.ReadLine()); }
                actionName = config[0];
            }
            return actionName;
        }

        private string GetActionExecuteContent(int actionNo)
        {
            string configFilePath = "Action" + actionNo + "Config.txt";
            string actionContent = string.Empty;
            if (!File.Exists(configFilePath)) return actionContent;

            using (StreamReader file = new StreamReader(configFilePath))
            {
                List<string> config = new List<string>();
                while (!file.EndOfStream) { config.Add(file.ReadLine()); }
                actionContent = config[1];
                for (int i = 2; i < config.Count; i++) { actionContent += " " + config[i]; }
            }
            return actionContent;
        }

        #region Status bar

        private void LblStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (File.Exists("Config.txt")) Process.Start(@"Notepad.exe", "Config.txt");
        }

        private void FadeStatusBarText()
        {
            Timer timFadeStatusBar = new Timer(1500);
            timFadeStatusBar.AutoReset = false;
            timFadeStatusBar.Elapsed += TimFadeStatusBar_Elapsed;
            timFadeStatusBar.Start();
        }

        private void FadeStatusBarText(int fadeTime)
        {
            Timer timFadeStatusBar = new Timer(fadeTime);
            timFadeStatusBar.AutoReset = false;
            timFadeStatusBar.Elapsed += TimFadeStatusBar_Elapsed;
            timFadeStatusBar.Start();
        }

        private void TimFadeStatusBar_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                LblStatus.Content = "OK, GO!";
            });
        }

        #endregion

        #region Create Note

        private void BtnExecute_Click(object sender, RoutedEventArgs e)
        {
            OpenOrExecute();
        }

        private void CmbNoteName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OpenOrExecute();
            }
            else if (e.Key == Key.Down)
            {
                CmbNoteName.IsDropDownOpen = true;
            }
            else
            {
                if (CmbNoteName.Text.StartsWith("\\"))
                {
                    // Command mode
                    CmbNoteName.IsDropDownOpen = false;
                }
                else
                {
                    // Other input show dropdown always
                    CmbNoteName.IsDropDownOpen = true;
                }
            }
        }

        private void TxtNoteName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) OpenOrExecute();
        }

        private void OpenOrExecute()
        {
            // Trim note name
            string note = CmbNoteName.Text.Trim();
            if (note.EndsWith(" Note"))
            {
                note = note.Replace(" Note", "").Trim();
            }

            if (note.Equals(string.Empty)) 
            {
                MessageBox.Show("Please input note name.", "Message"); 
                return;
            }

            if (note.StartsWith("\\"))
            {
                // Command mode
                // 1. Create
                if (note.StartsWith("\\create") || note.StartsWith("\\add"))
                {
                    note = note.Replace("\\add", "").Replace("\\create", "").Trim();
                    string filePath = NoteFolderPath + note + " Note.txt";

                    if (File.Exists(filePath))
                    {
                        // Already exist
                        LblStatus.Content = note + " Note.txt already exists.";
                    }
                    else
                    {
                        // Create note
                        CreateNote(note, filePath);
                        LblStatus.Content = note + " Note.txt created.";
                    }

                    FadeStatusBarText();
                    OpenNote(filePath);
                }

                // 2. Delete
                if (note.StartsWith("\\del") || note.StartsWith("\\delete"))
                {
                    note = note.Replace("\\delete", "").Replace("\\del", "").Trim();
                    string filePath = NoteFolderPath + note + " Note.txt";

                    // Create note
                    DeleteNote(filePath);
                }
            }
            else
            {
                // Note name only mode
                string filePath = NoteFolderPath + note + " Note.txt";
                if (File.Exists(filePath))
                {
                    LblStatus.Content = "Openning " + note + " Note.txt...";
                    FadeStatusBarText();
                    OpenNote(filePath);
                }
                else
                {
                    LblStatus.Content = "Not found.";
                }
            }

            // Exit application
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenNote(string notePath)
        {
            if (string.IsNullOrEmpty(DefaultTextEditor))
            { 
                Process.Start(notePath); 
            }
            else
            {
                Process.Start(DefaultTextEditor, "\"" + notePath + "\"");
            }
        }

        private void CreateNote(string note, string filePath)
        {
            
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine("");
                file.WriteLine(note + " Note"); string equalStr = string.Empty; for (int i = 0; i < note.Length; i++) { equalStr += "="; }
                file.WriteLine(equalStr + "=====");
                file.WriteLine("");
                file.WriteLine("");
                file.WriteLine(note);
                string dashString = string.Empty;
                for (int i = 0; i < note.Length; i++) { dashString += "-"; }
                file.WriteLine(dashString);
                file.WriteLine("");
            }
        }

        private void DeleteNote(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void CmbNoteName_GotFocus(object sender, RoutedEventArgs e)
        {
            CmbNoteName.IsDropDownOpen = true;
        }

        #endregion

        #region Events

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if (e.Key == Key.NumPad1) Action1();
            if (e.Key == Key.NumPad2) Action2();
            if (e.Key == Key.NumPad3) Action3();
            if (e.Key == Key.NumPad4) Action4();
            if (e.Key == Key.NumPad5) Action5();
            if (e.Key == Key.NumPad6) Action6();
            if (e.Key == Key.NumPad7) Action7();
            if (e.Key == Key.NumPad8) Action8();
            if (e.Key == Key.NumPad9) Action9();
        }

        private void BtnAction1_Click(object sender, RoutedEventArgs e)
        {
            Action1();
        }

        private void BtnAction2_Click(object sender, RoutedEventArgs e)
        {
            Action2();
        }

        private void BtnAction3_Click(object sender, RoutedEventArgs e)
        {
            Action3();
        }

        private void BtnAction4_Click(object sender, RoutedEventArgs e)
        {
            Action4();
        }

        private void BtnAction5_Click(object sender, RoutedEventArgs e)
        {
            Action5();
        }

        private void BtnAction6_Click(object sender, RoutedEventArgs e)
        {
            Action6();
        }

        private void BtnAction7_Click(object sender, RoutedEventArgs e)
        {
            Action7();
        }

        private void BtnAction8_Click(object sender, RoutedEventArgs e)
        {
            Action8();
        }

        private void BtnAction9_Click(object sender, RoutedEventArgs e)
        {
            Action9();
        }

        #endregion

        #region Actions

        private void Action1()
        {
            BtnAction1.Focus();
            ExecuteAction(1);
        }

        private void Action2()
        {
            BtnAction2.Focus();
            ExecuteAction(2);
        }

        private void Action3()
        {
            BtnAction3.Focus();
            ExecuteAction(3);
        }

        private void Action4()
        {
            BtnAction4.Focus();
            ExecuteAction(4);
        }

        private void Action5()
        {
            BtnAction5.Focus();
            ExecuteAction(5);
        }

        private void Action6()
        {
            BtnAction6.Focus();
            ExecuteAction(6);
        }

        private void Action7()
        {
            BtnAction7.Focus();
            ExecuteAction(7);
        }
        private void Action8()
        {
            BtnAction8.Focus();
            ExecuteAction(8);
        }

        private void Action9()
        {
            BtnAction9.Focus();
            ExecuteAction(9);
        }

        #endregion

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            CmbNoteName.Focus();
        }
    }
}
