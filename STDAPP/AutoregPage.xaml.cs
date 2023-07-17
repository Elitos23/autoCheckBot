using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для AutoregPage.xaml
    /// </summary>
    public partial class AutoregPage : Page
    {
        public AutoregPage()
        {
            InitializeComponent();
            AutoregOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
            DpiFactorContainer.Resize(AutoregGreed);
        }
       

         

        public int EditTag; // запоминаем какой таск мы редактируем    

        public TaskPage PageToEdit; // запоминаем страницу с таском в которую при редактировании заносим изменения   
        public int NumberOfDeleted = 0; // количество удаленных для того что бы отнимать от Total Tasks    
        public List<TaskPage> StartAllEx = new List<TaskPage>(); // экземляры класс будут добаляться сюда для кнопки startall
        public List<string> AccountBlackList = new List<string>(); // список заюзаных аков которые надо будет пропускать    
        public List<Task> TaskList = new List<Task>(); // лист с тасками     

        System.Windows.Controls.Frame[] FrameForTask = new System.Windows.Controls.Frame[100]; // фреймы для тасков



        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
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
                //
            }
        }

        public async void ShowAlertMessage(string message)
        {
            AlertPage alert = new AlertPage();
            alert.LabelAlertMessage.Content = message;
            Frame FrameForAlert = new Frame();
            FrameForAlert.Content = alert;
            AlertStack.Children.Add(FrameForAlert);
            await Task.Delay(3000);
            AlertStack.Children.Remove(FrameForAlert);

        }
        public struct DataGive // структура для передачи данных в таск класс
        {
         
            public string Account;
            public string Proxy;
            public bool Random;
        }


        public struct DataEdit // структура для привязки к edit
        {

            public int Account;
            public int AccountChoose;
            public int Proxy;
            public int ProxyChoose;
            public bool Random;

        }

        public DataEdit SaveMenuData()  // функция для сохранения всех полей при создании таска
        {
            DataEdit DataNow;
            DataNow.Account = Accounts.SelectedIndex;
            DataNow.AccountChoose = AccountsChoose.SelectedIndex;
            DataNow.Proxy = Proxy.SelectedIndex;
            DataNow.ProxyChoose = ProxyChoose.SelectedIndex;
            if (SwitchRandom.IsChecked == true)
            {
                DataNow.Random = true;
            }
            else
            {
                DataNow.Random = false;
            }
           
            return DataNow;
        }

        private void Accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AccountsChoose != null)
            {

                AccountsChoose.Items.Clear();
                AccountsChoose.Items.Add("ALL");
                AccountsChoose.SelectedIndex = 0;


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

        public void AddDataToVisualTask(DataGive data, TaskPage mytask) // что бы дохуя раз не писать сделал функцию которая к таску будет подставлять   
        {                                                               // данные в визуальный таск


            if (Proxy.SelectedIndex == 0)
            {
                mytask.Proxy.Content = "Localhost";
            }
            else
            {
                mytask.Proxy.Content = Proxy.SelectedItem.ToString();
            }


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

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            int CreatedTasks = TaskStack.Children.Count; //Сколько тасков уже созданно
            Console.WriteLine(TaskStack.Children.Count + "  Before Creating");

            if (Tasks.Text == "")
            {
                ShowAlertMessage(Messages.NullNumberOfTasks);
            }
            else
            {
                int TasksCounter = int.Parse(Tasks.Text); // сколько добавить тасков

                for (int i = 0; i < TasksCounter; i++)  // цикл по созданию таков от введенного числа 
                {
                    TaskPage mytask = new TaskPage(); // Берем страницу
                    StartAllEx.Add(mytask);
                    DataGive DataToTask = new DataGive(); // объявление                                      
                    AutoregTaskClass TSK = new AutoregTaskClass(); // создаем экземпляр основного таска
                    //

                    if (Accounts.SelectedItem != null || SwitchRandom.IsChecked == true)  // Добавление аккаунта
                    {
                        if (AccountsChoose.SelectedIndex == 0)
                        {
                            if (SwitchRandom.IsChecked == false)
                            {
                                int AccIndex = AccountData.GiveAccountIndexAutoreg(Accounts.SelectedItem.ToString());
                                DataToTask.Account = AccountData.GiveMeAccount(AccIndex, Accounts.SelectedItem.ToString());
                                while (AccountBlackList.Contains(DataToTask.Account))
                                {
                                    AccIndex = AccountData.GiveAccountIndexAutoreg(Accounts.SelectedItem.ToString());
                                    DataToTask.Account = AccountData.GiveMeAccount(AccIndex, Accounts.SelectedItem.ToString());
                                }
                                mytask.Account.Content = AccountData.GiveMeAccount(AccIndex, Accounts.SelectedItem.ToString());
                                Console.WriteLine(AccountData.GiveMeAccount(AccIndex, Accounts.SelectedItem.ToString()));
                                if (AccountData.GiveMeAccount(AccIndex, Accounts.SelectedItem.ToString()) == "ERROR")
                                {
                                    ShowAlertMessage(Messages.NeedMoreAccounts);
                                    return;
                                };
                            }
                            else
                            {
                                DataToTask.Account = "";
                            }

                        }
                        else
                        {
                            DataToTask.Account = AccountsChoose.SelectedItem.ToString();
                            AccountBlackList.Add(AccountsChoose.SelectedItem.ToString());
                            mytask.Account.Content = AccountsChoose.SelectedItem;
                        } // ======

                        if (Proxy.SelectedIndex != 0)
                        {
                            if (ProxyChoose.SelectedItem == null)
                            {
                                ShowAlertMessage(Messages.NullProxyChoose);
                                return;
                            }
                            else
                            {
                                DataToTask.Proxy = ProxyChoose.Text;
                            }
                        }

                        if(SwitchRandom.IsChecked == true)
                        {
                            DataToTask.Random = true;
                        }
                        else
                        {
                            DataToTask.Random = false;
                        }
                        mytask.Time.Content = "VAKSMS";
                        //
                        AddDataToVisualTask(DataToTask, mytask); // Добавляем в визуальный таск контент 
                        int CreatedTasksForButton = CreatedTasks;
                        DataEdit dataEdit = SaveMenuData();  // Выполняем сохранение в переменную 

                        TaskList.Add(new Task(() => Console.WriteLine("inisializing task")));

                        mytask.ButtonStart.Click += (x, g) => // добавление действия кнопки старт
                        {
                            Console.WriteLine(CreatedTasksForButton + " Task Started");
                            Console.WriteLine(DataToTask.Account);
                            try
                            {
                                TaskList[CreatedTasksForButton - NumberOfDeleted] = new Task(() => TSK.sour(CreatedTasksForButton, DataToTask, mytask));

                                TaskList[CreatedTasksForButton - NumberOfDeleted].Start();
                            }
                            catch
                            {
                                TaskList[CreatedTasksForButton] = new Task(() => TSK.sour(CreatedTasksForButton, DataToTask, mytask));

                                TaskList[CreatedTasksForButton].Start();

                            }

                            mytask.ButtonPause.Visibility = Visibility.Visible; // подмена кнопок

                            mytask.ButtonStart.Visibility = Visibility.Hidden;// подмена кнопок

                        };

                        mytask.ButtonPause.Click += (x, g) =>
                        {
                            mytask.ButtonStart.Visibility = Visibility.Visible; // подмена кнопок
                            if (AutoregTaskClass.browserArAutoreg[CreatedTasksForButton] != null)
                            {

                                AutoregTaskClass.browserArAutoreg[CreatedTasksForButton].CloseAsync();

                            }

                            Console.WriteLine(CreatedTasksForButton + " Task Paused");

                            mytask.ButtonPause.Visibility = Visibility.Hidden;// подмена кнопок
                            try
                            {
                                TaskList[CreatedTasksForButton - NumberOfDeleted].Wait();
                            }
                            catch
                            {
                                TaskList[CreatedTasksForButton].Wait();
                            }

                        };

                        mytask.ButtonEdit.Click += (x, g) =>
                        {

                            if (EditTask.Visibility != Visibility.Visible)
                            {

                                Console.WriteLine(CreatedTasksForButton + " Task Editing");
                                if (TaskBorder.Visibility == Visibility.Visible)
                                {
                                    TaskLabel.Visibility = Visibility.Hidden;
                                    AddTask.Visibility = Visibility.Hidden; // подмeна
                                    EditTask.Visibility = Visibility.Visible;
                                    Tasks.Visibility = Visibility.Hidden;
                                    TasksBord.Visibility = Visibility.Hidden;


                                }
                                else
                                {
                                    TaskBorder.Visibility = Visibility.Visible;
                                    TaskLabel.Visibility = Visibility.Hidden;
                                    AddTask.Visibility = Visibility.Hidden; // подмeна
                                    EditTask.Visibility = Visibility.Visible;
                                    Tasks.Visibility = Visibility.Hidden;
                                    TasksBord.Visibility = Visibility.Hidden;

                                }

                                Proxy.SelectedIndex = dataEdit.Proxy;
                                if (Proxy.SelectedIndex != 0)
                                {
                                    ProxyChoose.SelectedIndex = dataEdit.ProxyChoose;
                                }

                                Accounts.SelectedIndex = dataEdit.Account;
                                AccountsChoose.SelectedIndex = dataEdit.AccountChoose;
                                if (dataEdit.Random)
                                {
                                    SwitchRandom.IsChecked = true;
                                }
                                else
                                {
                                    SwitchRandom.IsChecked = false;
                                }
                               

                                EditTag = CreatedTasksForButton;
                                PageToEdit = mytask;
                                StartAllEx.Remove(mytask);


                            }
                            else
                            {
                                TaskBorder.Visibility = Visibility.Hidden; // возвращаем обратно все надписи и убираем окно 
                                TaskLabel.Visibility = Visibility.Visible;
                                AddTask.Visibility = Visibility.Visible; // подмeна
                                EditTask.Visibility = Visibility.Hidden;
                                Tasks.Visibility = Visibility.Visible;
                                TasksBord.Visibility = Visibility.Visible;
                                StartAllEx.Add(mytask);

                            }



                        };

                        mytask.ButtonDelete.Click += (x, g) =>  // Удаление методом скрытого анального проникновения 
                        {   // ячейка стека НЕ удаляется она просто прячеться, таким образон нумерация от стека не нарушается 
                            mytask.Visibility = Visibility.Hidden; // ставим visability hidden для startall и deleteall
                            FrameForTask[CreatedTasksForButton].Visibility = Visibility.Hidden; // прячим фрейм и уменьшеаем его размер в 0 
                            FrameForTask[CreatedTasksForButton].Height = 0;
                            try
                            {
                                TaskList.Remove(TaskList[CreatedTasksForButton - NumberOfDeleted]);
                            }
                            catch
                            {
                                TaskList.Remove(TaskList[CreatedTasksForButton]);
                            }

                            NumberOfDeleted++; // увеличиваем счетчик удаленных
                            TotalTasks.Content = "Total Tasks - " + (TaskStack.Children.Count - NumberOfDeleted).ToString(); // обновление списка при удалении 

                        };

                    }
                    else
                    {
                        ShowAlertMessage(Messages.NullProfileOrAccount);
                        return;
                    }

                    Console.WriteLine(CreatedTasks);
                    FrameForTask[CreatedTasks] = new Frame(); // Новый элемент frame
                    FrameForTask[CreatedTasks].Content = mytask; // Новый элемент добавляем в frame.content
                    TaskStack.Children.Add(FrameForTask[CreatedTasks]); // Добавляем все в Stack


                    CreatedTasks++; // увеличение созданных
                }


                Console.WriteLine(TaskStack.Children.Count + "  After Creating");
                TotalTasks.Content = "Total Tasks - " + (TaskStack.Children.Count - NumberOfDeleted).ToString(); //Вывод об
            }
        }

        private async void StopAllTasksClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (TaskPage s in StartAllEx)
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

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(EditTag + "Edited");
            // вносим новые данные 

            TaskPage RealPageEdit = PageToEdit;

            StartAllEx.Add(RealPageEdit);
            if (AccountsChoose.SelectedIndex == 0)
            {

            }
            else
            {

                RealPageEdit.Account.Content = AccountsChoose.SelectedItem;

            }

            // удалить действие предыдущей кнопки 
            // button start click delete action
            // Get the EventHandlersStore instance which holds event handlers for the specified element.
            // The EventHandlersStore class is declared as internal.
            var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            object eventHandlersStore = eventHandlersStoreProperty.GetValue(RealPageEdit.ButtonStart, null);
            // If no event handlers are subscribed, eventHandlersStore will be null.
            // Credit: /questions/24971/how-would-it-be-possible-to-remove-all-event-handlers-of-the-click-event-of-a-button/181999#181999
            if (eventHandlersStore == null)
                return;
            // Invoke the GetRoutedEventHandlers method on the EventHandlersStore instance 
            // for getting an array of the subscribed event handlers.
            var getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
                "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
                eventHandlersStore, new object[] { System.Windows.Controls.Button.ClickEvent });

            // Iteratively remove all routed event handlers from the element.
            foreach (var routedEventHandler in routedEventHandlers)
                RealPageEdit.ButtonStart.RemoveHandler(System.Windows.Controls.Button.ClickEvent, routedEventHandler.Handler);

            // button pause click delete action
            eventHandlersStoreProperty = typeof(UIElement).GetProperty(
               "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);


            eventHandlersStore = eventHandlersStoreProperty.GetValue(RealPageEdit.ButtonPause, null);


            if (eventHandlersStore == null)
                return;

            getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
                "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
                eventHandlersStore, new object[] { System.Windows.Controls.Button.ClickEvent });
            foreach (var routedEventHandler in routedEventHandlers)
                RealPageEdit.ButtonPause.RemoveHandler(System.Windows.Controls.Button.ClickEvent, routedEventHandler.Handler);

            //


            //Проверка пустых полей 

            /* if(Profile.SelectedItem == null)
             {
                 MessageBox.Show(Messages.NullProfileOrAccount);
                 return ;
             }

             if(DropTimeHou.Text == ""  || DropTimeMin.Text == "" || DropTimeSec .Text =="")
             {
                 MessageBox.Show(Messages.NullDropTime);
                 return;
             }

             if(link.Text == "")
             {
                 MessageBox.Show(Messages.NullLink);
                 return;
             }

             if (Size.SelectedItem == null)
             {
                 MessageBox.Show(Messages.NullSize);
                 return;
             }
            */
            // 

            // добавить таск по tasktag
            DataGive DataToTask = new DataGive(); // объявление    

            DataToTask.Account = RealPageEdit.Account.Content.ToString();
            if (SwitchRandom.IsChecked == true)
            {
                DataToTask.Random = true;
            }
            else
            {
                DataToTask.Random = false;
            }

            if (Proxy.SelectedIndex != 0)
            {
                DataToTask.Proxy = ProxyChoose.Text;
            }
            int CreatedTasksForButton = EditTag;

            TaskPage mytask = RealPageEdit; // Берем страницу
            AutoregTaskClass TSK = new AutoregTaskClass(); // создаем экземпляр основного таска
            DataEdit dataEdit = SaveMenuData();
            AddDataToVisualTask(DataToTask, mytask);                 //
            TaskList.Add(new Task(() => Console.WriteLine("inisializing task")));
            mytask.ButtonStart.Click += (x, g) => // добавление действия кнопки старт
            {
                Console.WriteLine(CreatedTasksForButton + " Task Started");
                Console.WriteLine(DataToTask.Account);
                try
                {
                    TaskList[CreatedTasksForButton - NumberOfDeleted] = new Task(() => TSK.sour(CreatedTasksForButton, DataToTask, mytask));

                    TaskList[CreatedTasksForButton - NumberOfDeleted].Start();
                }
                catch
                {
                    TaskList[CreatedTasksForButton] = new Task(() => TSK.sour(CreatedTasksForButton, DataToTask, mytask));

                    TaskList[CreatedTasksForButton].Start();
                }


                mytask.ButtonPause.Visibility = Visibility.Visible; // подмена кнопок            

                mytask.ButtonStart.Visibility = Visibility.Hidden;// подмена кнопок


            };

            mytask.ButtonPause.Click += (x, g) =>
            {
                mytask.ButtonStart.Visibility = Visibility.Visible; // подмена кнопок
                if (AutoregTaskClass.browserArAutoreg[CreatedTasksForButton] != null)
                {

                    AutoregTaskClass.browserArAutoreg[CreatedTasksForButton].CloseAsync();

                }
                Console.WriteLine(CreatedTasksForButton + " Task Paused");

                mytask.ButtonPause.Visibility = Visibility.Hidden;// подмена кнопок
                try
                {
                    TaskList[CreatedTasksForButton - NumberOfDeleted].Wait();
                }
                catch
                {
                    TaskList[CreatedTasksForButton].Wait();
                }

            };

            mytask.ButtonEdit.Click += (x, g) =>
            {

                Proxy.SelectedIndex = dataEdit.Proxy;
                if (Proxy.SelectedIndex != 0)
                {
                    ProxyChoose.SelectedIndex = dataEdit.ProxyChoose;
                }

                Accounts.SelectedIndex = dataEdit.Account;
                AccountsChoose.SelectedIndex = dataEdit.AccountChoose;

                if (dataEdit.Random)
                {
                    SwitchRandom.IsChecked = true;
                }
                else
                {
                    SwitchRandom.IsChecked = false;
                }

            };

            //
            TaskBorder.Visibility = Visibility.Hidden; // возвращаем обратно все надписи и убираем окно 
            TaskLabel.Visibility = Visibility.Visible;
            AddTask.Visibility = Visibility.Visible; // подмeна
            EditTask.Visibility = Visibility.Hidden;
            Tasks.Visibility = Visibility.Visible;
            TasksBord.Visibility = Visibility.Visible;
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

        private void Proxy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProxyChoose != null)
            {
                ProxyChoose.Items.Clear();
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

        private async void StartAllTasksClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (TaskPage s in StartAllEx)
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

        private void SourceOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FrameChanger.SetSourceToFrame();
            MainWindowContnetRemember.remember = "source";
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            TaskBorder.Visibility = Visibility.Hidden;
        }

        private void SourceOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SourceOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void SourceOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SourceOpenButton.Background = Brushes.Black;
        }

        private void ProfileEditorOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ProfileEditorOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void ProfileEditorOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ProfileEditorOpenButton.Background = Brushes.Black;
        }

        private void ProfileEditorOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FrameChanger.SetEditorToFrame();
            MainWindowContnetRemember.remember = "editor";
        }

        private void AutoregOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            AutoregOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void AutoregOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //AutoregOpenButton.Background = Brushes.Black;
        }

        private void EldoradoOpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            EldoradoOpenButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF241F25");
        }

        private void EldoradoOpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            EldoradoOpenButton.Background = Brushes.Black;
        }

        private void EldoradoOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FrameChanger.SetEldoradoToFrame();
            MainWindowContnetRemember.remember = "eldorado";
        }
    }
}
