using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace STDAPP
{
    public class CreationTasksSource
    {
        private DataSourceStructures.DataEdit SaveMenuData(SourcePage pg)  // функция для сохранения всех полей при создании таска для отображения при редактировании
        {
            DataSourceStructures.DataEdit DataNow = new DataSourceStructures.DataEdit();

            DataNow.ProductLink = pg.link.Text;
            DataNow.Size = pg.Size.txtContent.Text;
            DataNow.DropTime = (pg.DropTimeHou.Text + ":" + pg.DropTimeMin.Text + ":" + pg.DropTimeSec.Text + ":" + pg.DropTimeMili.Text).Split(':');
            DataNow.TimeMode = pg.TimeMod.SelectedIndex;
            DataNow.Preload = pg.PreloadT.SelectedIndex;
            DataNow.Account = pg.Accounts.SelectedIndex;
            DataNow.AccountChoose = pg.AccountsChoose.SelectedIndex;
            DataNow.Proxy = pg.Proxy.SelectedIndex;
            DataNow.ProxyChoose = pg.ProxyChoose.SelectedIndex;
            DataNow.Prof = pg.Profile.SelectedIndex;
            DataNow.ChekoutMod = pg.ChekoutMod.SelectedIndex;
            DataNow.Shop = pg.Shop.SelectedIndex;
            DataNow.ShopId = pg.ShopId.Text;
            DataNow.City = pg.DnsCity.Text;
            DataNow.DnsMode = pg.Mode.SelectedIndex;
            DataNow.LamodaPayment = pg.LamodaMode.SelectedIndex;
            DataNow.DnsPreload = pg.PreloadTimeDns.SelectedIndex;
            DataNow.Cookies = pg.Cookies.Text;
            DataNow.Headers = pg.Headers.Text;
            DataNow.Amount = pg.Amount.Text;
            DataNow.PsVersion = pg.PSVersion.SelectedIndex;
            return DataNow;
        }

        private TaskData SaveDataForSerialization(int CreatedTasksForButton, DataSourceStructures.DataGive DataToTask, TaskPage mytask, DataSourceStructures.DataEdit dataEdit, SourcePage pg) // сохранение данных в класс сериализации
        {
            // Добавление данных в экземпляр класса для последующей сериализации и сохранения 
            TaskData dt = new TaskData();
            dt.DataGive = DataToTask;
            dt.DataEdit = dataEdit;
            dt.accout = mytask.Account.Content.ToString();
            dt.profile = mytask.Profile.Content.ToString();
            dt.proxy = mytask.Proxy.Content.ToString();
            dt.time = mytask.Time.Content.ToString();
            if (mytask.Size.Content != null)
            {
                dt.size = mytask.Size.Content.ToString();
            }
            dt.Shop = pg.Shop.SelectedIndex;
            TaskDataForSerialization.taskDatas.Add(dt);
            return dt;
            // 
        }

        private void StartTaskEvents(Task SourceTask, int CreatedTasksForButton, DataSourceStructures.DataGive DataToTask, MultiTask Task, TaskPage mytask, CancellationToken token, int shop)
        {
            Console.WriteLine(DataToTask.Account);
            switch (shop)
            {
                case 0:
                    {
                        Task.SNKRS.stop = false;
                        SourceTask = new Task(() => Task.SNKRS.Src(CreatedTasksForButton, DataToTask, mytask));
                        break;
                    }
                case 1:
                    {
                        SourceTask = new Task(() => Task.DNS.Src(CreatedTasksForButton, mytask, DataToTask, Task.logW, token));
                        break;
                    }
                case 2:
                    {
                        SourceTask = new Task(() => Task.Lamoda.Src(CreatedTasksForButton, mytask, DataToTask, token, Task.logW));
                        break;
                    }
                case 3:
                    {
                        SourceTask = new Task(() => Task.Binance.Src(DataToTask, mytask, token, Task.logW));
                        break;
                    }
                case 4:
                    {
                        SourceTask = new Task(() => Task.Citilink.Src(DataToTask, mytask, token, Task.logW));
                        break;
                    }
            }
            SourceTask.Start();
            mytask.ButtonPause.Visibility = Visibility.Visible; // подмена кнопок
            mytask.ButtonStart.Visibility = Visibility.Hidden;// подмена кнопок

        }

        private void StopTaskEvents(Task SourceTask, int CreatedTasksForButton, TaskPage mytask, CancellationToken token, CancellationTokenSource cancelTokenSource, int shop)
        {
            mytask.ButtonStart.Visibility = Visibility.Visible; // подмена кнопок

            switch (shop)
            {
                case 0:
                    {
                        if (TaskClass.browsers[CreatedTasksForButton] != null)
                        {
                            TaskClass.browsers[CreatedTasksForButton].CloseAsync();
                        }
                        break;
                    }
                case 1:
                    {
                        cancelTokenSource.Cancel();
                        if (DnsTaskClass.browsers[CreatedTasksForButton] != null)
                        {
                            DnsTaskClass.browsers[CreatedTasksForButton].CloseAsync();
                        }

                        break;
                    }
                case 2:
                    {
                        cancelTokenSource.Cancel();
                        break;
                    }
                case 3:
                    {
                        cancelTokenSource.Cancel();
                        break;
                    }
                case 4:
                    {
                        cancelTokenSource.Cancel();
                        break;
                    }

            }
            Console.WriteLine(CreatedTasksForButton + " Task Paused");
            mytask.ButtonPause.Visibility = Visibility.Hidden;// подмена кнопок
            if (shop == 0)
            {
                SourceTask.Wait();
            }

        }

        private void CompleteMenuData(DataSourceStructures.DataEdit dataEdit, SourcePage pg)// Заполнение полей соответствующими им данными при редактировании (save menu data )
        {
            pg.link.Text = dataEdit.ProductLink;
            pg.Size.txtContent.Text = dataEdit.Size;
            pg.TimeMod.SelectedIndex = dataEdit.TimeMode;
            if (pg.TimeMod.SelectedIndex != 0)
            {
                pg.PreloadT.SelectedIndex = dataEdit.Preload;
            }
            pg.Proxy.SelectedIndex = dataEdit.Proxy;
            pg.ProxyChoose.SelectedIndex = dataEdit.ProxyChoose;
            pg.DropTimeHou.Text = dataEdit.DropTime[0];
            pg.DropTimeMin.Text = dataEdit.DropTime[1];
            pg.DropTimeSec.Text = dataEdit.DropTime[2];
            pg.DropTimeMili.Text = dataEdit.DropTime[3];
            pg.Profile.SelectedIndex = dataEdit.Prof;
            pg.Accounts.SelectedIndex = dataEdit.Account;
            pg.AccountsChoose.SelectedIndex = dataEdit.AccountChoose;
            pg.ChekoutMod.SelectedIndex = dataEdit.ChekoutMod;
            pg.Mode.SelectedIndex = dataEdit.DnsMode;
            pg.Shop.SelectedIndex = dataEdit.Shop;
            pg.ShopId.Text = dataEdit.ShopId;
            pg.DnsCity.Text = dataEdit.City;
            pg.LamodaMode.SelectedIndex = dataEdit.LamodaPayment;
            pg.PreloadTimeDns.SelectedItem = dataEdit.DnsPreload;
            pg.Cookies.Text = dataEdit.Cookies;
            pg.Headers.Text = dataEdit.Headers;
            pg.Amount.Text = dataEdit.Amount;
            pg.PSVersion.SelectedIndex = dataEdit.PsVersion;
        }

        private int EditTag; // запоминаем какой таск мы редактируем    
        private System.Windows.Controls.Frame[] FrameForTask = new System.Windows.Controls.Frame[100]; // фреймы для тасков
        private TaskPage PageToEdit; // запоминаем страницу с таском в которую при редактировании заносим изменения   
        public List<TaskPage> StartAllEx = new List<TaskPage>(); // экземляры класс будут добаляться сюда для кнопки startall
        private List<string> AccountBlackList = new List<string>(); // список заюзаных аков которые надо будет пропускать     
        public SerializeTask TaskDataForSerialization = new SerializeTask();
        public void CreateTaskWithData(SourcePage DataSource)
        {
            int CreatedTasks = DataSource.TaskStack.Children.Count; //Сколько тасков уже созданно
            Console.WriteLine(DataSource.TaskStack.Children.Count + "  Before Creating");

            if (DataSource.Tasks.Text == "")
            {
                Messages.ShowAlertMessage(Messages.NullNumberOfTasks, DataSource.AlertStack);
            }
            else
            {
                int TasksCounter = int.Parse(DataSource.Tasks.Text); // сколько добавить тасков

                for (int i = 0; i < TasksCounter; i++)  // цикл по созданию таков от введенного числа 
                {

                    TaskPage mytask = new TaskPage(); // Берем страницу
                    StartAllEx.Add(mytask);
                    DataSourceStructures.DataGive DataToTask = new DataSourceStructures.DataGive(); // объявление                                                                                                    // 
                    MultiTask Task = new MultiTask();
                    int CreatedTasksForButton = CreatedTasks;
                    try
                    {
                        CollectingData.SetDataByMode(DataToTask, DataSource, mytask, "Create");
                    }
                    catch
                    {
                        return;
                    }


                    DataSourceStructures.DataEdit dataEdit = SaveMenuData(DataSource);  // Выполняем сохранение в переменную                   
                    Task SourceTask = new Task(() => Console.WriteLine("inisializing task"));
                    TaskData AddedItemToSerialization = SaveDataForSerialization(CreatedTasksForButton, DataToTask, mytask, dataEdit, DataSource);

                    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                    CancellationToken token = cancelTokenSource.Token;
                    mytask.ButtonStart.Tag = CreatedTasksForButton;

                    Task.logW.LogTitle.Content = "Task - " + CreatedTasksForButton;
                    int shp = DataSource.Shop.SelectedIndex;
                    mytask.ButtonEdit.Tag = AddedItemToSerialization;

                    mytask.ButtonStart.Click += (x, g) => // добавление действия кнопки старт
                    {
                        StartTaskEvents(SourceTask, CreatedTasksForButton, DataToTask, Task, mytask, token, shp);
                    };

                    mytask.ButtonPause.Click += (x, g) =>
                    {
                        StopTaskEvents(SourceTask, CreatedTasksForButton, mytask, token, cancelTokenSource, shp);
                        cancelTokenSource = new CancellationTokenSource(); // refreshing  token for next pause 
                        token = cancelTokenSource.Token;
                    };

                    mytask.ButtonEdit.Click += (x, g) =>
                    {
                        if (DataSource.EditTask.Visibility != Visibility.Visible)
                        {
                            Console.WriteLine(CreatedTasksForButton + " Task Editing");
                            if (DataSource.TaskBorder.Visibility == Visibility.Visible)
                            {
                                DataSource.TaskLabel.Visibility = Visibility.Hidden;
                                DataSource.AddTask.Visibility = Visibility.Hidden; // подмeна
                                DataSource.EditTask.Visibility = Visibility.Visible;
                                DataSource.Tasks.Visibility = Visibility.Hidden;
                                DataSource.TasksBord.Visibility = Visibility.Hidden;
                                DataSource.Delay.SelectedIndex = 0;
                                DataSource.Delay.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                DataSource.TaskBorder.Visibility = Visibility.Visible;
                                DataSource.TaskLabel.Visibility = Visibility.Hidden;
                                DataSource.AddTask.Visibility = Visibility.Hidden; // подмeна
                                DataSource.EditTask.Visibility = Visibility.Visible;
                                DataSource.Tasks.Visibility = Visibility.Hidden;
                                DataSource.TasksBord.Visibility = Visibility.Hidden;
                                DataSource.Delay.SelectedIndex = 0;
                                DataSource.Delay.Visibility = Visibility.Hidden;
                            }

                            CompleteMenuData(dataEdit, DataSource);
                            EditTag = CreatedTasksForButton;
                            PageToEdit = mytask;
                            StartAllEx.Remove(mytask);

                        }
                        else
                        {
                            DataSource.TaskBorder.Visibility = Visibility.Hidden; // возвращаем обратно все надписи и убираем окно 
                            DataSource.TaskLabel.Visibility = Visibility.Visible;
                            DataSource.AddTask.Visibility = Visibility.Visible; // подмeна
                            DataSource.EditTask.Visibility = Visibility.Hidden;
                            DataSource.Tasks.Visibility = Visibility.Visible;
                            DataSource.TasksBord.Visibility = Visibility.Visible;
                            DataSource.Delay.Visibility = Visibility.Visible;
                            StartAllEx.Add(mytask);

                        }

                    };

                    mytask.ButtonDelete.Click += (x, g) =>
                    {
                        DataSource.TaskStack.Children.Remove(FrameForTask[CreatedTasksForButton]);
                        TaskDataForSerialization.taskDatas.Remove(AddedItemToSerialization);
                        Console.WriteLine("Deleting Task - " + CreatedTasksForButton);
                        try
                        {
                            SourceTask.Dispose();
                        }
                        catch
                        {
                            Console.WriteLine("Already disposed or not activated flow");
                        }

                        DataSource.TotalTasks.Content = "Total Tasks - " + (DataSource.TaskStack.Children.Count).ToString(); // обновление списка при удалении 

                    };


                    mytask.ButtonLog.Click += (x, g) =>
                    {
                        Console.WriteLine("Opening Log Window");
                        Task.logW.WindowState = WindowState.Normal; // возврат если свернут 
                        Task.logW.Show(); // показ окна 
                        Task.logW.Activate(); // передний план

                    };


                    Console.WriteLine(CreatedTasks);
                    FrameForTask[CreatedTasks] = new Frame(); // Новый элемент frame
                    FrameForTask[CreatedTasks].Content = mytask; // Новый элемент добавляем в frame.content
                    DataSource.TaskStack.Children.Add(FrameForTask[CreatedTasks]); // Добавляем все в Stack                   

                    CreatedTasks++; // увеличение созданных
                }
                Console.WriteLine(DataSource.TaskStack.Children.Count + "  After Creating");
                DataSource.TotalTasks.Content = "Total Tasks - " + (DataSource.TaskStack.Children.Count).ToString(); //Вывод общего количества при добавлении нового (с учетом удаленных )

            }
        }

        public void EditTaskWhithData(SourcePage DataSource)
        {
            Console.WriteLine(EditTag + "Edited");
            // вносим новые данные 
            TaskData OldSerializationTaskData = (TaskData)PageToEdit.ButtonEdit.Tag;  // удаление таска до редактирования из списка сериализации 
            TaskDataForSerialization.taskDatas.Remove(OldSerializationTaskData);

            TaskPage mytask = PageToEdit;// Страница редактирования откуда беруться данные 
            MultiTask Task = new MultiTask();// Экземпляр со всеми возможными типами данных 
            DataSourceStructures.DataGive DataToTask = new DataSourceStructures.DataGive(); // объявление экземпляра данных для таска 
            int CreatedTasksForButton = EditTag;

            StartAllEx.Add(mytask);

            //очистка действий кнопок
            DeleteButtonAction(mytask.ButtonStart);
            DeleteButtonAction(mytask.ButtonLog);
            DeleteButtonAction(mytask.ButtonDelete);

            try
            {
                CollectingData.SetDataByMode(DataToTask, DataSource, mytask, "Edit");
            }
            catch
            {
                return;
            }

            if (DataSource.AccountsChoose.SelectedIndex == 0)
            {
                DataToTask.Account = PageToEdit.Account.Content.ToString();
                mytask.Account.Content = PageToEdit.Account.Content;
            }
            else
            {
                DataToTask.Account = DataSource.AccountsChoose.SelectedItem.ToString();
            }

            DataSourceStructures.DataEdit dataEdit = SaveMenuData(DataSource);
            TaskData AddedItemToSerialization = SaveDataForSerialization(CreatedTasksForButton, DataToTask, mytask, dataEdit, DataSource);

            Task SourceTask = new Task(() => Console.WriteLine("inisializing task"));
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            Task.logW.LogTitle.Content = "Task - " + CreatedTasksForButton;

            mytask.ButtonStart.Tag = CreatedTasksForButton;
            mytask.ButtonEdit.Tag = AddedItemToSerialization;
            int shp = DataSource.Shop.SelectedIndex;
            mytask.ButtonStart.Click += (x, g) =>
            {
                StartTaskEvents(SourceTask, CreatedTasksForButton, DataToTask, Task, mytask, token, shp);
            };

            mytask.ButtonPause.Click += (x, g) =>
            {
                StopTaskEvents(SourceTask, CreatedTasksForButton, mytask, token, cancelTokenSource, shp);
                cancelTokenSource = new CancellationTokenSource(); // refreshing  token for next pause 
                token = cancelTokenSource.Token;
            };

            mytask.ButtonEdit.Click += (x, g) =>
            {
                CompleteMenuData(dataEdit, DataSource);
            };

            mytask.ButtonDelete.Click += (x, g) =>
            {
                DataSource.TaskStack.Children.Remove(FrameForTask[CreatedTasksForButton]);
                TaskDataForSerialization.taskDatas.Remove(AddedItemToSerialization);
                Console.WriteLine("Deleting Task - " + CreatedTasksForButton);
                try
                {
                    SourceTask.Dispose();
                }
                catch
                {
                    Console.WriteLine("Already disposed or not activated flow");
                }

                DataSource.TotalTasks.Content = "Total Tasks - " + (DataSource.TaskStack.Children.Count).ToString(); // обновление счетчика при удалении 

            };

            mytask.ButtonLog.Click += (x, g) =>
            {
                Console.WriteLine("Opening Log Window");
                Task.logW.WindowState = WindowState.Normal; // возврат если свернут 
                Task.logW.Show(); // показ окна 
                Task.logW.Activate(); // передний план
            };

            DataSource.TaskBorder.Visibility = Visibility.Hidden; // возвращаем обратно все надписи и убираем окно 
            DataSource.TaskLabel.Visibility = Visibility.Visible;
            DataSource.AddTask.Visibility = Visibility.Visible; // подмeна
            DataSource.EditTask.Visibility = Visibility.Hidden;
            DataSource.Tasks.Visibility = Visibility.Visible;
            DataSource.TasksBord.Visibility = Visibility.Visible;

        }

        public void CreateSavedTask(SourcePage DataSource, TaskData d)
        {
            Console.WriteLine(d.DataGive.Account);

            int CreatedTasks = DataSource.TaskStack.Children.Count;
            Console.WriteLine(DataSource.TaskStack.Children.Count + "  Before Creating");

            DataSourceStructures.DataEdit dataEdit = d.DataEdit;
            DataSourceStructures.DataGive DataToTask = d.DataGive;
            MultiTask Task = new MultiTask();
            TaskPage mytask = new TaskPage();
            int CreatedTasksForButton = CreatedTasks;

            mytask.Account.Content = d.accout;
            mytask.Profile.Content = d.profile;
            mytask.Proxy.Content = d.proxy;
            mytask.Time.Content = d.time;
            mytask.Size.Content = d.size;
            DataSource.Shop.SelectedIndex = d.Shop;

            StartAllEx.Add(mytask);

            TaskData AddedItemToSerialization = SaveDataForSerialization(CreatedTasksForButton, DataToTask, mytask, dataEdit, DataSource);


            Task SourceTask = new Task(() => Console.WriteLine("inisializing task"));
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            mytask.ButtonStart.Tag = CreatedTasksForButton;
            Task.logW.LogTitle.Content = "Task - " + CreatedTasksForButton;
            int shp = d.Shop;

            mytask.ButtonStart.Click += (x, g) => // добавление действия кнопки старт
            {
                StartTaskEvents(SourceTask, CreatedTasksForButton, DataToTask, Task, mytask, token, shp);
            };

            mytask.ButtonPause.Click += (x, g) =>
            {
                StopTaskEvents(SourceTask, CreatedTasksForButton, mytask, token, cancelTokenSource, shp);
                cancelTokenSource = new CancellationTokenSource(); // refreshing  token for next pause 
                token = cancelTokenSource.Token;
            };

            mytask.ButtonEdit.Click += (x, g) =>
            {
                if (DataSource.EditTask.Visibility != Visibility.Visible)
                {
                    Console.WriteLine(CreatedTasksForButton + " Task Editing");
                    if (DataSource.TaskBorder.Visibility == Visibility.Visible)
                    {
                        DataSource.TaskLabel.Visibility = Visibility.Hidden;
                        DataSource.AddTask.Visibility = Visibility.Hidden; // подмeна
                        DataSource.EditTask.Visibility = Visibility.Visible;
                        DataSource.Tasks.Visibility = Visibility.Hidden;
                        DataSource.TasksBord.Visibility = Visibility.Hidden;
                        DataSource.Delay.SelectedIndex = 0;
                        DataSource.Delay.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        DataSource.TaskBorder.Visibility = Visibility.Visible;
                        DataSource.TaskLabel.Visibility = Visibility.Hidden;
                        DataSource.AddTask.Visibility = Visibility.Hidden; // подмeна
                        DataSource.EditTask.Visibility = Visibility.Visible;
                        DataSource.Tasks.Visibility = Visibility.Hidden;
                        DataSource.TasksBord.Visibility = Visibility.Hidden;
                        DataSource.Delay.SelectedIndex = 0;
                        DataSource.Delay.Visibility = Visibility.Hidden;
                    }
                    CompleteMenuData(dataEdit, DataSource);
                    EditTag = CreatedTasksForButton;
                    PageToEdit = mytask;
                    StartAllEx.Remove(mytask);

                }
                else
                {
                    DataSource.TaskBorder.Visibility = Visibility.Hidden; // возвращаем обратно все надписи и убираем окно 
                    DataSource.TaskLabel.Visibility = Visibility.Visible;
                    DataSource.AddTask.Visibility = Visibility.Visible; // подмeна
                    DataSource.EditTask.Visibility = Visibility.Hidden;
                    DataSource.Tasks.Visibility = Visibility.Visible;
                    DataSource.TasksBord.Visibility = Visibility.Visible;
                    DataSource.Delay.Visibility = Visibility.Visible;
                    StartAllEx.Add(mytask);
                }

            };

            mytask.ButtonDelete.Click += (x, g) =>
            {
                DataSource.TaskStack.Children.Remove(FrameForTask[CreatedTasksForButton]);
                TaskDataForSerialization.taskDatas.Remove(AddedItemToSerialization);
                Console.WriteLine("Deleting Task - " + CreatedTasksForButton);
                try
                {
                    SourceTask.Dispose();
                }
                catch
                {
                    Console.WriteLine("Already disposed or not activated flow");
                }

                DataSource.TotalTasks.Content = "Total Tasks - " + (DataSource.TaskStack.Children.Count).ToString(); // обновление счетчика при удалении 

            };


            mytask.ButtonLog.Click += (x, g) =>
            {
                Console.WriteLine("Opening Log Window");
                Task.logW.WindowState = WindowState.Normal; // возврат если свернут 
                Task.logW.Show(); // показ окна 
                Task.logW.Activate(); // передний план

            };

            Console.WriteLine(CreatedTasks);
            FrameForTask[CreatedTasks] = new Frame(); // Новый элемент frame
            FrameForTask[CreatedTasks].Content = mytask; // Новый элемент добавляем в frame.content
            DataSource.TaskStack.Children.Add(FrameForTask[CreatedTasks]); // Добавляем все в Stack

            Console.WriteLine(DataSource.TaskStack.Children.Count + "  After Creating");
            DataSource.TotalTasks.Content = "Total Tasks - " + (DataSource.TaskStack.Children.Count).ToString(); //Вывод общего количества при добавлении нового (с учетом удаленных )
        }


        private void DeleteButtonAction(Button button)
        {
            // удалить действие предыдущей кнопки 
            // button start click delete action
            // Get the EventHandlersStore instance which holds event handlers for the specified element.
            // The EventHandlersStore class is declared as internal.
            var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            object eventHandlersStore = eventHandlersStoreProperty.GetValue(button, null);
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
                button.RemoveHandler(System.Windows.Controls.Button.ClickEvent, routedEventHandler.Handler);
        }
    }
}
