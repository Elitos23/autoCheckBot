using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PuppeteerSharp;
using Discord.Webhook;
using Discord;
using System.Windows;
using System.Threading;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Jering.Javascript.NodeJS;

namespace STDAPP
{
    public class AutoregTaskClass
    {

        private string[] wordsP = new string[5];
        private string[] words = new string[5];
       
        public static Browser[] browserArAutoreg = new Browser[150];
      
        string apiDiscord = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook");
        string API_KEY = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/api.api");
        public async Task ShowStatus(string ShowLog, TaskPage mytask)
        {
            await App.Current.Dispatcher.Invoke(() => {
                mytask.Status.Content = ShowLog;
                return Task.CompletedTask;
            });
        }

        private int RandomWords(int a, int b, Random d)
        {
            return d.Next(a, b);
        }


        public async void sour(int i, AutoregPage.DataGive DataGive, TaskPage mytask)
        {
            try
            {



                string[] names = { "Василий", "Иван", "Александр", "Кирилл", "Виталий", "Михаил", "Алексей", "Дмитрий", "Савелий", "Генадий", "Евгений" };
                /////Фамилии
                string[] surnames = { "Лакомкин", "Журавлев", "Мартышкин", "Иванов", "Порошенко", "Мишкин", "Зайцев", "Лисичкин", "Куропаткин", "Заселов", "Птушкин" };



                await ShowStatus("Reg", mytask);
                if (DataGive.Random != true)
                {
                    words = DataGive.Account.Split(':', '\n', '\t', '\r');
                }

                while (true)
                {

                    Random rnd = new Random();
                    if (DataGive.Random)
                    {
                        string email = "";
                        string mask = "abcdefghijklmnopqrstuvwxyz";
                        string mask2 = "0123456789";
                        for (int j = 1; j <= 12; j++)
                        {
                            email += mask.Substring(RandomWords(0, 25, rnd), 1);
                        }
                        email += "@yandex.ru";

                        string password = "";
                        for (int j = 1; j <= 8; j++)
                        {
                            if (j % 4 == 0) password += mask2.Substring(RandomWords(0, 9, rnd), 1);
                            else if (j % 3 == 0) password += mask.Substring(RandomWords(0, 25, rnd), 1).ToUpper();
                            else password += mask.Substring(RandomWords(0, 25, rnd), 1);

                        }
                        words[0] = email;
                        words[1] = password;
                    }
                    else
                    {

                    }

                    Random random = new Random();

                    string name = names[random.Next(0, 10)];
                    string surname = surnames[random.Next(0, 10)];
                    DateTime from = new DateTime(1980, 1, 1);
                    DateTime to = new DateTime(2000, 1, 1);
                    int daysDiff = (to - from).Days;
                    string date = from.AddDays(random.Next(daysDiff)).ToString("d");



                    var services = new ServiceCollection();
                    services.AddNodeJS();

                    NodeJSProcessOptions kaka = new NodeJSProcessOptions();
                    services.Configure<NodeJSProcessOptions>(options => options.ExecutablePath = DefinePaths.NodePath + "node.exe");
                    services.Configure<OutOfProcessNodeJSServiceOptions>(options => options.TimeoutMS = -1);
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

                    string result = await nodeJSService.InvokeFromFileAsync<string>(DefinePaths.scriptPathAutoreg, null, args: new[] { words[0], words[1], DataGive.Proxy, name, surname, date, API_KEY }, cancellationToken);
                    Console.WriteLine(result);


                    nodeJSService.Dispose();

                    if (result == "Succsess")
                    {
                        await ShowStatus("Successful", mytask);


                        var user = new DiscordWebhookClient(apiDiscord);
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithColor(0, 255, 0);
                        eb.WithTitle("ACCOUNT REGISTERED");

                        eb.AddField("Account", "||" + words[0] + ":"+ words[1] + "||");
                        eb.WithFooter("Stardom");
                        eb.WithCurrentTimestamp();
                        await user.SendMessageAsync(embeds: new List<Embed>() { eb.Build() });

                        break;
                    }
                    await ShowStatus("Retry", mytask);

                }

            }
            catch
            {
                await ShowStatus("Closed", mytask);
            }

        }

    }
}