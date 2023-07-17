using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace STDAPP
{
    /// <summary>
    /// New Creation 
    /// Логика взаимодействия для SourcePage.xaml
    /// </sumhttps://www.google.com/search?q=%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4%D1%87%D0%B8%D0%BA&oq=%D0%BF%D0%B5%D1%80%D0%B5&aqs=chrome.1.69i57j69i59j35i39j0i131i433j0i433j69i60l2j69i61.3345j0j7&sourceid=chrome&ie=UTF-8mary>
    public partial class SourcePage : Page
    {
        public SourcePage()
        {
            InitializeComponent();
            DpiFactorContainer.Resize(SourceGreed);
            SourceOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");

        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            //DetectOpenBrowser();
            if (TaskBorder.Visibility == Visibility.Visible)
            {
                TaskBorder.Visibility = Visibility.Hidden;
            }
            else
            {
                // если таск редактировали и нажали create task
                TaskBorder.Visibility = Visibility.Visible; // возвращаем обратно все надписи и убираем окно 
                TaskLabel.Visibility = Visibility.Visible;
                AddTask.Visibility = Visibility.Visible; // подмeна
                EditTask.Visibility = Visibility.Hidden;
                Tasks.Visibility = Visibility.Visible;
                TasksBord.Visibility = Visibility.Visible;
                if (Shop.SelectedIndex == 0)
                {
                    Delay.Visibility = Visibility.Visible;
                }
                else
                {
                    Delay.Visibility = Visibility.Hidden;
                }
                //
            }

        }


        private CreationTasksSource Creation = new CreationTasksSource();
        private void AddTask_Click(object sender, RoutedEventArgs e)  // ADD TASK FUNCTIOM   
        {
            Creation.CreateTaskWithData(this);
        }

        private void Accounts_DropDownOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OPENED");

            try
            {
                if (Directory.Exists(DefinePaths.AccountsPath))
                {

                    Console.WriteLine("ACCOUNTS: ");
                    string[] files = Directory.GetFiles(DefinePaths.AccountsPath);
                    foreach (string s in files)
                    {
                        Console.WriteLine(s);
                        string NameToAddInCb = Regex.Match(s, @"ACCS/(.*?).txt").Groups[1].Value;
                        Console.WriteLine("Adding - " + NameToAddInCb);
                        if (Accounts.Items.Contains(NameToAddInCb))
                        {
                            Console.WriteLine("Already exists");
                        }
                        else
                        {
                            Accounts.Items.Add(NameToAddInCb);
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

            }
        }

        private void Profile_DropDownOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OPENED");
            try
            {
                if (Directory.Exists(DefinePaths.ProfilePath))
                {

                    Console.WriteLine("ACCOUNTS: ");
                    string[] files = Directory.GetFiles(DefinePaths.ProfilePath);
                    foreach (string s in files)
                    {
                        Console.WriteLine(s);
                        string NameToAddInCb = Regex.Match(s, @"PROF/(.*?).txt").Groups[1].Value;
                        Console.WriteLine("Adding - " + NameToAddInCb);
                        if (Profile.Items.Contains(NameToAddInCb))
                        {
                            Console.WriteLine("Already exists");
                        }
                        else
                        {

                            Profile.Items.Add(NameToAddInCb);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

            }
        }

        private void AccountsChoose_DropDownOpened(object sender, EventArgs e)
        {
            if (Accounts.SelectedItem != null)
            {
                try
                {
                    StreamReader sr = new StreamReader(DefinePaths.AccountsPath + Accounts.SelectedItem.ToString() + ".txt");
                    //Read all text
                    string Content = sr.ReadToEnd();
                    Console.WriteLine(Content + "ALL CONTENT");
                    string[] AccountData = Content.Split('\n');
                    foreach (string s in AccountData)
                    {
                        string[] sFinal = new string[2];
                        sFinal = s.Split('\r');
                        Console.WriteLine("ADD to CB - " + s + " ?");
                        if (AccountsChoose.Items.Contains(sFinal[0]))
                        {
                            Console.WriteLine("Skip");
                        }
                        else
                        {
                            Console.WriteLine("ADD");

                            AccountsChoose.Items.Add(sFinal[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        private void Accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            AccountsChoose.Items.Clear();
            AccountsChoose.Items.Add("ALL");
            AccountsChoose.SelectedIndex = 0;

        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            Creation.EditTaskWhithData(this);
        }

        private void Proxy_DropDownOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OPENED");

            try
            {
                if (Directory.Exists(DefinePaths.ProxysPath))
                {

                    Console.WriteLine("PROXYS: ");
                    string[] files = Directory.GetFiles(DefinePaths.ProxysPath);
                    foreach (string s in files)
                    {
                        Console.WriteLine(s);
                        string NameToAddInCb = Regex.Match(s, @"PROX/(.*?).txt").Groups[1].Value;
                        Console.WriteLine("Adding - " + NameToAddInCb);
                        if (Proxy.Items.Contains(NameToAddInCb))
                        {
                            Console.WriteLine("Already exists");
                        }
                        else
                        {
                            Proxy.Items.Add(NameToAddInCb);
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

            }
        }

        private void Proxy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProxyChoose != null)
            {
                ProxyChoose.Items.Clear();
                ProxyChoose.Items.Add("ALL");
                ProxyChoose.SelectedIndex = 0;

                if (Proxy.SelectedIndex == 0)
                {
                    ProxyChoose.Visibility = Visibility.Hidden;
                }
                else
                {
                    ProxyChoose.Visibility = Visibility.Visible;
                }

            }


        }

        private void ProxyChoose_DropDownOpened(object sender, EventArgs e)
        {
            if (Proxy.SelectedItem != null | Proxy.SelectedIndex != 0)
            {
                try
                {
                    StreamReader sr = new StreamReader(DefinePaths.ProxysPath + Proxy.SelectedItem.ToString() + ".txt");
                    //Read all text
                    string Content = sr.ReadToEnd();
                    Console.WriteLine(Content + "ALL CONTENT");
                    string[] ProxyData = Content.Split('\n');
                    foreach (string s in ProxyData)
                    {
                        string[] sFinal = new string[2];
                        sFinal = s.Split('\r');
                        Console.WriteLine("ADD to CB - " + s + " ?");
                        if (ProxyChoose.Items.Contains(sFinal[0]))
                        {
                            Console.WriteLine("Skip");
                        }
                        else
                        {
                            Console.WriteLine("ADD");

                            ProxyChoose.Items.Add(sFinal[0]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        private void DropTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DropTimeHou.Text.Length == 2)
            {
                DropTimeMin.Focus();
            }


        }

        private void DropTimeMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DropTimeMin.Text.Length == 2)
            {
                DropTimeSec.Focus();
            }
            if (DropTimeMin.Text.Length == 0)
            {
                DropTimeHou.Focus();
            }
        }

        private void DropTimeSec_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DropTimeSec.Text.Length == 2)
            {
                DropTimeMili.Focus();
            }
            if (DropTimeSec.Text.Length == 0)
            {
                DropTimeMin.Focus();
            }

        }

        private void DropTimeMili_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DropTimeMili.Text.Length == 0)
            {
                DropTimeSec.Focus();
            }
        }

        private async void StartAllTasksClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (TaskPage s in Creation.StartAllEx)
                {
                    if (s.Visibility == Visibility.Visible)
                    {
                        if (s.ButtonStart.Visibility == Visibility.Visible)
                        {
                            Console.WriteLine("Visable ---------------");
                            ButtonAutomationPeer peer = new ButtonAutomationPeer(s.ButtonStart);

                            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                            invokeProv.Invoke();

                            await Task.Delay(1000);
                        }
                        else
                        {
                            Console.WriteLine("Already started");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Deleted ----------------");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async void StopAllTasksClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (TaskPage s in Creation.StartAllEx)
                {
                    if (s.Visibility == Visibility.Visible)
                    {
                        if (s.ButtonPause.Visibility == Visibility.Visible)
                        {
                            Console.WriteLine("Visable ---------------");

                            ButtonAutomationPeer peer = new ButtonAutomationPeer(s.ButtonPause);
                            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                            invokeProv.Invoke();
                            await Task.Delay(500);
                        }
                        else
                        {
                            Console.WriteLine("Already Paused");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Deleted ----------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void MinimizeButtonSource_Click(object sender, RoutedEventArgs e)
        {
            TaskBorder.Visibility = Visibility.Hidden;

        }

        private void ProfileEditorOpenButton_Click(object sender, RoutedEventArgs e)
        {

            FrameChanger.SetEditorToFrame();
            MainWindowContnetRemember.remember = "editor";

        }

        private void ProfileEditorOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ProfileEditorOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void ProfileEditorOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ProfileEditorOpenButton.Background = Brushes.Black;
        }

        private void SourceOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SourceOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void SourceOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //SourceOpenButton.Background = Brushes.Black;
        }

        private void TimeMod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeMod.SelectedIndex != 0)
            {
                PreloadT.Visibility = Visibility.Visible;
                PreloadT.SelectedIndex = 0;
            }
            else
            {
                if (PreloadT != null)
                {
                    PreloadT.Visibility = Visibility.Hidden;
                }
            }
        }

        private void AutoregOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FrameChanger.SetAutoregToFrame();
            MainWindowContnetRemember.remember = "autoreg";
        }

        private void AutoregOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            AutoregOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void AutoregOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            AutoregOpenButton.Background = Brushes.Black;
        }

        private void ChekoutMod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChekoutMod != null & TimeBorder != null)
            {
                if (ChekoutMod.SelectedIndex == 2)
                {
                    TimeBorder.Visibility = Visibility.Hidden;
                    Delay.Visibility = Visibility.Hidden;
                    LabelDropTime.Visibility = Visibility.Hidden;
                    Console.WriteLine("DUDOS ACTIVATED");
                }
                else
                {
                    TimeBorder.Visibility = Visibility.Visible;
                    Delay.Visibility = Visibility.Visible;
                    LabelDropTime.Visibility = Visibility.Visible;
                    Console.WriteLine("NOOOOO");
                }
            }
        }

        private void ShopComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Shop != null)
            {
                if (Shop.SelectedIndex == 0)
                {  // snkrs

                    if (LabelMode != null)
                    {

                        if (LabelMode.Visibility != Visibility.Visible)
                        {
                            Default_Condition();
                            LabelMode.Visibility = Visibility.Visible;
                            LabelSize.Visibility = Visibility.Visible;
                            LabelProfile.Visibility = Visibility.Visible;
                            LabelTimeMod.Content = "Time Mode";
                            LabelTimeMod.Visibility = Visibility.Visible;
                            LabelDropTime.Visibility = Visibility.Visible;
                            LabelLink.Visibility = Visibility.Visible;
                            LabelAccounts.Visibility = Visibility.Visible;
                            LabelProxy.Visibility = Visibility.Visible;
                            LabelProfile.Visibility = Visibility.Visible;

                            ChekoutMod.Visibility = Visibility.Visible;
                            Size.Visibility = Visibility.Visible;
                            Profile.Visibility = Visibility.Visible;
                            TimeMod.Visibility = Visibility.Visible;
                            //   Delay.Visibility = Visibility.Visible;
                            TimeBorder.Visibility = Visibility.Visible;
                            BorderLink.Visibility = Visibility.Visible;
                            Proxy.Visibility = Visibility.Visible;
                            ProxyChoose.Visibility = Visibility.Visible;
                            Accounts.Visibility = Visibility.Visible;
                            AccountsChoose.Visibility = Visibility.Visible;
                            Profile.Visibility = Visibility.Visible;
                        }
                    }
                }
                else if (Shop.SelectedIndex == 1) // DNS
                {
                    Default_Condition();
                    LabelTimeMod.Content = "City";
                    LabelTimeMod.Visibility = Visibility.Visible;
                    LabelAccounts.Visibility = Visibility.Visible;
                    LabelProxy.Visibility = Visibility.Visible;
                    LabelShopId.Visibility = Visibility.Visible;
                    LabelDnsMod.Visibility = Visibility.Visible;
                    LabelLink.Visibility = Visibility.Visible;
                    LabelPreloadDns.Visibility = Visibility.Visible;

                    BorderLink.Visibility = Visibility.Visible;
                    DnsCityBorder.Visibility = Visibility.Visible;
                    BorderShopId.Visibility = Visibility.Visible;
                    Mode.Visibility = Visibility.Visible;
                    Proxy.Visibility = Visibility.Visible;
                    ProxyChoose.Visibility = Visibility.Visible;
                    Accounts.Visibility = Visibility.Visible;
                    AccountsChoose.Visibility = Visibility.Visible;
                    PreloadTimeDns.Visibility = Visibility.Visible;
                    Thickness margin = Accounts.Margin;
                    margin.Top -= 70;
                    Accounts.Margin = margin;

                    margin = LabelAccounts.Margin;
                    margin.Top -= 70;
                    LabelAccounts.Margin = margin;

                    margin = AccountsChoose.Margin;
                    margin.Top -= 70;
                    AccountsChoose.Margin = margin;

                    margin = LabelProxy.Margin;
                    margin.Top -= 50;
                    LabelProxy.Margin = margin;

                    margin = Proxy.Margin;
                    margin.Top -= 50;
                    Proxy.Margin = margin;

                    margin = ProxyChoose.Margin;
                    margin.Top -= 50;
                    ProxyChoose.Margin = margin;

                    margin = LabelTimeMod.Margin;
                    margin.Top += 52;
                    LabelTimeMod.Margin = margin;

                }
                else if (Shop.SelectedIndex == 2) // Lamoda
                {
                    if (LabelLamodaMod.Visibility != Visibility.Visible)
                    {
                        Default_Condition();

                        LabelSize.Visibility = Visibility.Visible;
                        LabelProfile.Visibility = Visibility.Visible;
                        LabelLink.Visibility = Visibility.Visible;
                        LabelAccounts.Visibility = Visibility.Visible;
                        LabelProxy.Visibility = Visibility.Visible;
                        LabelProfile.Visibility = Visibility.Visible;
                        LabelLamodaMod.Visibility = Visibility.Visible;

                        LamodaMode.Visibility = Visibility.Visible;
                        Size.Visibility = Visibility.Visible;
                        BorderLink.Visibility = Visibility.Visible;
                        Proxy.Visibility = Visibility.Visible;
                        ProxyChoose.Visibility = Visibility.Visible;
                        Accounts.Visibility = Visibility.Visible;
                        AccountsChoose.Visibility = Visibility.Visible;
                        Profile.Visibility = Visibility.Visible;

                        Thickness margin = Accounts.Margin;
                        margin.Top -= 70;
                        Accounts.Margin = margin;

                        margin = LabelAccounts.Margin;
                        margin.Top -= 70;
                        LabelAccounts.Margin = margin;

                        margin = AccountsChoose.Margin;
                        margin.Top -= 70;
                        AccountsChoose.Margin = margin;

                        margin = Profile.Margin;
                        margin.Top -= 70;
                        Profile.Margin = margin;

                        margin = LabelProfile.Margin;
                        margin.Top -= 70;
                        LabelProfile.Margin = margin;


                        margin = LabelProxy.Margin;
                        margin.Top -= 50;
                        LabelProxy.Margin = margin;

                        margin = Proxy.Margin;
                        margin.Top -= 50;
                        Proxy.Margin = margin;

                        margin = ProxyChoose.Margin;
                        margin.Top -= 50;
                        ProxyChoose.Margin = margin;

                        margin = LabelTimeMod.Margin;
                        margin.Top += 52;
                        LabelTimeMod.Margin = margin;
                    }
                }
                else if (Shop.SelectedIndex == 3)
                {

                    Default_Condition();
                    LabelLink.Visibility = Visibility.Visible;
                    LabelProxy.Visibility = Visibility.Visible;
                    LabelCookie.Visibility = Visibility.Visible;
                    LabelHeaders.Visibility = Visibility.Visible;
                    LabelDropTime.Visibility = Visibility.Visible;
                    LabelAmount.Visibility = Visibility.Visible;

                    BorderCookies.Visibility = Visibility.Visible;
                    BorderHeaders.Visibility = Visibility.Visible;
                    TimeBorder.Visibility = Visibility.Visible;
                    Proxy.Visibility = Visibility.Visible;
                    BorderLink.Visibility = Visibility.Visible;
                    AmounytBorder.Visibility = Visibility.Visible;
                }
                else if (Shop.SelectedIndex == 4)
                {
                    Default_Condition();
                    LabelAccounts.Visibility = Visibility.Visible;
                    LabelTimeMod.Visibility = Visibility.Visible;
                    LabelTimeMod.Content = "City";
                    LabelPsVersion.Visibility = Visibility.Visible;
                    LabelProxy.Visibility = Visibility.Visible;

                    Accounts.Visibility = Visibility.Visible;
                    AccountsChoose.Visibility = Visibility.Visible;
                    DnsCityBorder.Visibility = Visibility.Visible;
                    PSVersion.Visibility = Visibility.Visible;
                    Proxy.Visibility = Visibility.Visible;
                    Thickness margin = Accounts.Margin;
                    margin.Top -= 82;
                    Accounts.Margin = margin;

                    margin = LabelAccounts.Margin;
                    margin.Top -= 82;
                    LabelAccounts.Margin = margin;

                    margin = AccountsChoose.Margin;
                    margin.Top -= 82;
                    AccountsChoose.Margin = margin;

                    margin = LabelTimeMod.Margin;
                    margin.Top += 52;
                    LabelTimeMod.Margin = margin;

                    margin = LabelProxy.Margin;
                    margin.Top -= 35;
                    LabelProxy.Margin = margin;

                    margin = Proxy.Margin;
                    margin.Top -= 35;
                    Proxy.Margin = margin;

                    margin = ProxyChoose.Margin;
                    margin.Top -= 35;
                    ProxyChoose.Margin = margin;
                }

            }
        }

        private void Default_Condition() // Hide всех полей и выставление их в дефолтное положение 
        {
            LabelMode.Visibility = Visibility.Hidden;
            LabelSize.Visibility = Visibility.Hidden;
            LabelProfile.Visibility = Visibility.Hidden;
            LabelTimeMod.Content = "Time Mode";
            LabelDropTime.Visibility = Visibility.Hidden;
            LabelTimeMod.Visibility = Visibility.Hidden;
            LabelLink.Visibility = Visibility.Hidden;
            LabelAccounts.Visibility = Visibility.Hidden;
            LabelProxy.Visibility = Visibility.Hidden;
            LabelShopId.Visibility = Visibility.Hidden;
            LabelDnsMod.Visibility = Visibility.Hidden;
            LabelLamodaMod.Visibility = Visibility.Hidden;
            LabelDnsMod.Visibility = Visibility.Hidden;
            LabelPreloadDns.Visibility = Visibility.Hidden;
            LabelCookie.Visibility = Visibility.Hidden;
            LabelHeaders.Visibility = Visibility.Hidden;
            LabelAmount.Visibility = Visibility.Hidden;
            LabelPsVersion.Visibility = Visibility.Hidden;

            BorderCookies.Visibility = Visibility.Hidden;
            BorderHeaders.Visibility = Visibility.Hidden;
            LamodaMode.Visibility = Visibility.Hidden;
            BorderLink.Visibility = Visibility.Hidden;
            Accounts.Visibility = Visibility.Hidden;
            AccountsChoose.Visibility = Visibility.Hidden;
            Proxy.Visibility = Visibility.Hidden;
            ProxyChoose.Visibility = Visibility.Hidden;
            ChekoutMod.Visibility = Visibility.Hidden;
            Size.Visibility = Visibility.Hidden;
            Profile.Visibility = Visibility.Hidden;
            TimeMod.Visibility = Visibility.Hidden;
            Delay.Visibility = Visibility.Hidden;
            TimeBorder.Visibility = Visibility.Hidden;
            DnsCityBorder.Visibility = Visibility.Hidden;
            BorderShopId.Visibility = Visibility.Hidden;
            Mode.Visibility = Visibility.Hidden;
            PreloadTimeDns.Visibility = Visibility.Hidden;
            AmounytBorder.Visibility = Visibility.Hidden;
            PSVersion.Visibility = Visibility.Hidden;

            Thickness margin = new Thickness(765, 374, 0, 0);

            Accounts.Margin = margin;

            margin = new Thickness(638, 374, 0, 0);

            LabelAccounts.Margin = margin;

            margin = new Thickness(765, 436, 0, 0);

            AccountsChoose.Margin = margin;

            margin = new Thickness(97, 395, 0, 0);

            LabelProxy.Margin = margin;

            margin = new Thickness(231, 399, 0, 0);

            Proxy.Margin = margin;

            margin = new Thickness(231, 468, 0, 0);

            ProxyChoose.Margin = margin;

            margin = new Thickness(97, 237, 0, 0);

            LabelTimeMod.Margin = margin;

            margin = new Thickness(765, 311, 0, 0);

            Profile.Margin = margin;

            margin = new Thickness(638, 304, 0, 0);

            LabelProfile.Margin = margin;

            LabelLink.Content = "Product Link";
        }

        private void StopCapthaClick(object sender, RoutedEventArgs e) // farm dns capcha 
        {
            CaptchaStorage.Continue = false;
            StartCaptcha.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFC2FFBB");
            Messages.ShowAlertMessage(CaptchaStorage.CaptchaBankReady.Count + " captcha available", AlertStack);

        }

        private void StartCapthaClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists(DefinePaths.pathCapMonster))
            {
                Messages.ShowAlertMessage("Start farm captcha", AlertStack);
            }
            else
            {
                Messages.ShowAlertMessage("No Capmonster API key", AlertStack);
                return;
            }

            CaptchaStorage.Continue = true;
            StartCaptcha.BorderBrush = Brushes.Red;

            var outer = Task.Factory.StartNew(() =>      // Другой поток для фарма капчи 
            {
                CaptchaStorage.DoCapthaAndAdd();
            });

        }

        private void DnsModeSelectionIndexChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EldoradoOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            EldoradoOpenButton.Background = Brushes.Black;
        }

        private void EldoradoOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            EldoradoOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void EldoradoOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FrameChanger.SetEldoradoToFrame();
            MainWindowContnetRemember.remember = "eldorado";
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {

            if (Creation.TaskDataForSerialization == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(Creation.TaskDataForSerialization.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, Creation.TaskDataForSerialization);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(DefinePaths.TasksPath);
                }
                Messages.ShowAlertMessage("Tasks exported", AlertStack);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Messages.ShowAlertMessage("Error in export tasks", AlertStack);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DefinePaths.TasksPath)) { return; }

            SerializeTask objectOut = default(SerializeTask);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(DefinePaths.TasksPath);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(SerializeTask);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (SerializeTask)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.ShowAlertMessage("Import tasks error", AlertStack);
            }

            foreach (TaskData d in objectOut.taskDatas)
            {
                if (d != null)
                {
                    if (d.IsDeleted != true)
                    {
                        try
                        {
                            Creation.CreateSavedTask(this, d);
                        }
                        catch
                        {
                            Messages.ShowAlertMessage("Error in creating imported tasks", AlertStack);
                        }

                    }
                }

            }

            //   Console.WriteLine(objectOut.someProperty);
            // Console.WriteLine(objectOut.someProperty1);
            // objectOut.dosmt(AlertStack);
        }

    }
}