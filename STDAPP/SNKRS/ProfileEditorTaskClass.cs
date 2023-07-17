using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace STDAPP
{
    class ProfileEditorTaskClass
    {

        class Data
        {
            public string asyncData { get; set; }
            public string componentName { get; set; }
        }

        public static Browser[] browserArEditor = new Browser[100];
        private string[] words = new string[120];
        private WaitForSelectorOptions waitForSelectorOptions = new WaitForSelectorOptions();
  


        public async Task ShowStatus(string ShowLog, TaskPage mytask)
        {
            await App.Current.Dispatcher.Invoke(() => {
                mytask.Status.Content = ShowLog;
                return Task.CompletedTask;
            });
        }

        public static void WriteLog(string message, int num)
        {
            string log = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/log.txt";
            using (StreamWriter sw = new StreamWriter(log, true))
            {
                sw.WriteLine("[" + DateTime.Now.ToString() + "] (" + num + ") " + message);
            }
        }
        private string[] wordsP = new string[5];


        public async void Src(int i, ProfileEditorPage.DataGive DataGive, TaskPage mytask)
        {
            try
            {

                await ShowStatus("Login", mytask);
                words = DataGive.Account.Split(':', '\n', '\t', '\r');


                while (true)
                {

                    Console.WriteLine("JS Stream starting");

                    var services = new ServiceCollection();
                    services.AddNodeJS();

                    NodeJSProcessOptions kaka = new NodeJSProcessOptions();
                    services.Configure<NodeJSProcessOptions>(options => options.ExecutablePath = DefinePaths.NodePath + "node.exe");
                    ServiceProvider serviceProvider = services.BuildServiceProvider();
                    INodeJSService nodeJSService = serviceProvider.GetRequiredService<INodeJSService>();


                    var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                           "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
                    object eventHandlersStore = eventHandlersStoreProperty.GetValue(mytask.ButtonPause, null);
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

                    // button pause click delete action
                    eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                       "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);


                    eventHandlersStore = eventHandlersStoreProperty.GetValue(mytask.ButtonPause, null);

                    if (eventHandlersStore == null)
                        return;

                    getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
                        "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
                        eventHandlersStore, new object[] { System.Windows.Controls.Button.ClickEvent });
                    foreach (var routedEventHandler in routedEventHandlers)
                        mytask.ButtonPause.RemoveHandler(System.Windows.Controls.Button.ClickEvent, routedEventHandler.Handler);
                    CancellationTokenSource cts = new CancellationTokenSource();
                    CancellationToken cancellationToken = cts.Token;
                    mytask.ButtonPause.Click += (x, g) =>
                    {
                        mytask.ButtonStart.Visibility = Visibility.Visible; // подмена кнопок
                        mytask.ButtonPause.Visibility = Visibility.Hidden;// подмена кнопок          
                        cts.Cancel();
                        nodeJSService.Dispose();
                        return;

                    };

                    string result = await nodeJSService.InvokeFromFileAsync<string>(DefinePaths.scriptPath, null, args: new[] { words[0], words[1], DataGive.Proxy,DefinePaths.pathProf, DefinePaths.path }, cancellationToken);
                    Console.WriteLine(result);


                    nodeJSService.Dispose();

                    if (result == "Succsess")
                    {
                        await ShowStatus("Successful", mytask);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("123");
                        await ShowStatus("Retry", mytask);
                        System.Threading.Thread.Sleep(3000);
                        if (Directory.Exists(DefinePaths.pathProf + words[0]))
                        {
                            Directory.Delete(DefinePaths.pathProf + words[0], true);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ShowStatus("Closed", mytask);
            }

        }
    }
}