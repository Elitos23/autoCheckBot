using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;



namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {

        public SourcePage Source;
        public AccountsPage Accounts;
        public ProfilePage Profile;
        public ProfileEditorPage ProfileEditor;
        public Page SettingsPg;



        public MainWindow()
        {

            InitializeComponent();


            Source = new SourcePage();
            Accounts = new AccountsPage();
            Profile = new ProfilePage();
            FrameChanger.MainFrame = FrameOfMainContent;
            FrameChanger.SourcePage = Source;
            //ScaleTo125Percents(DpiFactorContainer.DPI);
            SettingsPg = new SettingsPage();
            CheckThemeCon();
        }

        public void CheckThemeCon()
        {
            string ThemePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/theme.state";
            if (System.IO.File.Exists(ThemePath))
            {

                string state = System.IO.File.ReadAllText(ThemePath);
                ThemeCondition.ThemeCon = Convert.ToBoolean(state);
                Console.WriteLine("TXT checked");
            }
            else
            {

                ThemeCondition.ThemeCon = false;
            }


            if (ThemeCondition.ThemeCon == true)
            {
                playSound();
            }
        }

        private void playSound()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/Stardom/yui_ohayo.wav";

                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = path;
                player.Load();
                player.Play();
            }
            catch
            {
                Console.WriteLine("Error with sound file");
            }
        }
        private void ScaleTo125Percents(double dpiFactor)
        {
            // Change scale of window content

            Console.WriteLine(dpiFactor);
            double ScaleCount;
            if (dpiFactor == 1.25)
            {
                ScaleCount = 0.90;
            }
            else
            {
                ScaleCount = 1;
            }
            FrameBorder.LayoutTransform = new ScaleTransform(ScaleCount, ScaleCount, 0, 0);
            Width *= ScaleCount;
            Height *= ScaleCount;

            // Bring window center screen
            var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Top = (screenHeight - Height) / 2;
            Left = (screenWidth - Width) / 2;
        }
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            FrameBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF");
            WindowBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF");
            StartPageLogo.Visibility = Visibility.Hidden;
            FrameOfMainContent.Content = SettingsPg;
        }
        private void Menu_Button_Source_Click(object sender, RoutedEventArgs e)
        {

            //ScaleTo125Percents();
            if (ThemeCondition.ThemeCon)
            {
                Source.pic.Visibility = Visibility.Visible;
            }
            else
            {
                Source.pic.Visibility = Visibility.Hidden;
            }
            FrameBorder.BorderBrush = System.Windows.Media.Brushes.Pink;
            WindowBorder.BorderBrush = System.Windows.Media.Brushes.Pink;
            /*    SourcePage AddClickSource = new SourcePage();
                AddClickSource.ProfileEditorOpenButton.Click += (x, g) =>
                {

                    ProfileEditorPage AddClickEditor = new ProfileEditorPage();
                    AddClickEditor.SourceOpenButton.Click += (x1, g1) =>
                    {
                        Source = new SourcePage();
                        FrameOfMainContent.Content = Source;
                    };
                    ProfileEditor = new ProfileEditorPage();
                    FrameOfMainContent.Content = ProfileEditor;
                };

                Source = AddClickSource;*/

            StartPageLogo.Visibility = Visibility.Hidden;
            SourceLine.Visibility = Visibility.Visible;
            AccountsLine.Visibility = Visibility.Hidden;
            ProfileLine.Visibility = Visibility.Hidden;
            if (MainWindowContnetRemember.remember == "source")
            {

                FrameOfMainContent.Content = Source;


            }
            else if (MainWindowContnetRemember.remember == "editor")
            {

                FrameChanger.SetEditorToFrame();


            }
            else if (MainWindowContnetRemember.remember == "autoreg")
            {

                FrameChanger.SetAutoregToFrame();

            }
            else if (MainWindowContnetRemember.remember == "eldorado")
            {

                FrameChanger.SetEldoradoToFrame();

            }



        }

        private void MenuItem_Profile_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeCondition.ThemeCon)
            {
                Profile.pic.Visibility = Visibility.Visible;
            }
            else
            {
                Profile.pic.Visibility = Visibility.Hidden;
            }
            FrameBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE1C1EA");
            WindowBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE1C1EA");
            FrameOfMainContent.Content = Profile;
            StartPageLogo.Visibility = Visibility.Hidden;
            SourceLine.Visibility = Visibility.Hidden;
            AccountsLine.Visibility = Visibility.Hidden;
            ProfileLine.Visibility = Visibility.Visible;
        }

        private void MenuItem_Accounts_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeCondition.ThemeCon)
            {
                Accounts.pic.Visibility = Visibility.Visible;
            }
            else
            {
                Accounts.pic.Visibility = Visibility.Hidden;
            }
            FrameBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF6594C8");
            WindowBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF6594C8");
            FrameOfMainContent.Content = Accounts;
            StartPageLogo.Visibility = Visibility.Hidden;
            SourceLine.Visibility = Visibility.Hidden;
            AccountsLine.Visibility = Visibility.Visible;
            ProfileLine.Visibility = Visibility.Hidden;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void FullSize_Click(object sender, RoutedEventArgs e)
        {

            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
            WindowStyle = WindowStyle.None;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MenuItem_ProfileEdtor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Source_Mouse_Enter(object sender, MouseEventArgs e)
        {
            SourceLine.Visibility = Visibility.Visible;
            AccountsLine.Visibility = Visibility.Hidden;
            ProfileLine.Visibility = Visibility.Hidden;
        }

        private void Source_Mouse_Leave(object sender, MouseEventArgs e)
        {
            if (FrameOfMainContent.Content == Source)
            {
                SourceLine.Visibility = Visibility.Visible;
                AccountsLine.Visibility = Visibility.Hidden;
                ProfileLine.Visibility = Visibility.Hidden;
            }
            else
            {
                SourceLine.Visibility = Visibility.Hidden;
                if (FrameOfMainContent.Content == Profile)
                {
                    ProfileLine.Visibility = Visibility.Visible;
                    AccountsLine.Visibility = Visibility.Hidden;
                }
                if (FrameOfMainContent.Content == Accounts)
                {
                    AccountsLine.Visibility = Visibility.Visible;
                    ProfileLine.Visibility = Visibility.Hidden;
                }


            }
        }

        private void Account_Mouse_Enter(object sender, MouseEventArgs e)
        {
            SourceLine.Visibility = Visibility.Hidden;
            AccountsLine.Visibility = Visibility.Visible;
            ProfileLine.Visibility = Visibility.Hidden;
        }

        private void Account_Mouse_Leave(object sender, MouseEventArgs e)
        {
            if (FrameOfMainContent.Content == Accounts)
            {
                SourceLine.Visibility = Visibility.Hidden;
                AccountsLine.Visibility = Visibility.Visible;
                ProfileLine.Visibility = Visibility.Hidden;
            }
            else
            {
                AccountsLine.Visibility = Visibility.Hidden;
                if (FrameOfMainContent.Content == Profile)
                {
                    ProfileLine.Visibility = Visibility.Visible;
                    SourceLine.Visibility = Visibility.Hidden;
                }
                if (FrameOfMainContent.Content == Source)
                {
                    SourceLine.Visibility = Visibility.Visible;
                    ProfileLine.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Profile_Mouse_Enter(object sender, MouseEventArgs e)
        {
            SourceLine.Visibility = Visibility.Hidden;
            AccountsLine.Visibility = Visibility.Hidden;
            ProfileLine.Visibility = Visibility.Visible;
        }

        private void Profile_Mouse_Leave(object sender, MouseEventArgs e)
        {
            if (FrameOfMainContent.Content == Profile)
            {
                SourceLine.Visibility = Visibility.Hidden;
                AccountsLine.Visibility = Visibility.Hidden;
                ProfileLine.Visibility = Visibility.Visible;
            }
            else
            {
                ProfileLine.Visibility = Visibility.Hidden;
                if (FrameOfMainContent.Content == Accounts)
                {
                    AccountsLine.Visibility = Visibility.Visible;
                    SourceLine.Visibility = Visibility.Hidden;
                }
                if (FrameOfMainContent.Content == Source)
                {
                    SourceLine.Visibility = Visibility.Visible;
                    AccountsLine.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseGrid.Visibility = Visibility.Visible;
        }

        private void Close_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseGrid.Visibility = Visibility.Hidden;
        }

        private void WebLogo_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://stardomaio.ru/");
        }

        private void FullScreen_MouseEnter(object sender, MouseEventArgs e)
        {
            MaximizeGrid.Visibility = Visibility.Visible;
        }

        private void FullScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            MaximizeGrid.Visibility = Visibility.Hidden;
        }

        private void HideWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            MinimizeGrid.Visibility = Visibility.Visible;
            
        }

        private void HideWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            MinimizeGrid.Visibility = Visibility.Hidden;
        }
    

      
       
    }
}
