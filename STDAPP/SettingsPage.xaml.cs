using Discord;
using Discord.Webhook;
using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.IO.Compression;
using xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;
using System.Windows.Media.Imaging;

namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/api.api";
        string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";
        string pathCapMonster = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/CapMonster.api";
        public SettingsPage()
        {

            InitializeComponent();
            InitializeUserData.GetUsersData();
            if (File.Exists(Path))
            {

                API.Text = File.ReadAllText(Path);

            }
            if (File.Exists(pathApi))
            {
                DiscordHook.Text = File.ReadAllText(pathApi);

            }
            if (File.Exists(pathCapMonster))
            {
                CaptchaApi.Text = File.ReadAllText(pathCapMonster);
            }


            if (InitializeUserData.GetUsersData())
            {

                DiscordName.Content += InitializeUserData.DiscordName;
                ImageBrush myBrush = new ImageBrush();
                Image image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(
                    new Uri(InitializeUserData.DiscordAvatar));
                myBrush.ImageSource = image.Source;
                Grid grid = new Grid();
                PictureBorder.Background = myBrush;
                Key.Text += InitializeUserData.Key;
                RenewDate.Text += InitializeUserData.RenewDate;
            }
        }

        private void SaveApi_Click(object sender, RoutedEventArgs e)
        {

            string api = API.Text;
            System.IO.File.WriteAllText(Path, api);
            Messages.ShowAlertMessageSettings("API key saved", AlertStack);
        }

        private void SaveDiscord_Click(object sender, RoutedEventArgs e)
        {
            string Path2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";
            string dis = DiscordHook.Text;
            System.IO.File.WriteAllText(Path2, dis);
            Messages.ShowAlertMessageSettings("Discord webhook saved", AlertStack);
        }

        private void TestDiscord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InitializeUserData.GetUsersData();
                if (InitializeUserData.GetUsersData())
                {

                    DiscordName.Content += InitializeUserData.DiscordName;
                    ImageBrush myBrush = new ImageBrush();
                    Image image = new System.Windows.Controls.Image();
                    image.Source = new BitmapImage(
                        new Uri(InitializeUserData.DiscordAvatar));
                    myBrush.ImageSource = image.Source;
                    Grid grid = new Grid();
                    PictureBorder.Background = myBrush;
                    Key.Text += InitializeUserData.Key;
                    RenewDate.Text += InitializeUserData.RenewDate;
                }
                string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";
                string api = File.ReadAllText(pathApi).ToString();
                string json = JsonConvert.SerializeObject(new
                {
                    username = "STARDOM",
                    avatar_url = "https://postila.ru/data/2a/8f/b9/b7/2a8fb9b743e7cbb656006965245a64c42ec571aaf2edd010540a3fb42b54bca8.png",
                    embeds = new[]
                                {
                               new {
                                    title = "STARDOM TEST WEBHOOK",
                                    thumbnail = new
                                        {
                                            url = InitializeUserData.DiscordAvatar
                                        },
                                    fields = new[]
                                    {
                                        new
                                        {
                                            name = "Nickname",
                                            value = InitializeUserData.DiscordName
                                        },
                                        new
                                        {
                                            name = "Our socials",
                                            value = "[VK](https://vk.com/stardomru)"
                                        }
                                    },
                                    footer = new
                                    {
                                        text = "STARDOM",
                                        icon_url = "https://postila.ru/data/2a/8f/b9/b7/2a8fb9b743e7cbb656006965245a64c42ec571aaf2edd010540a3fb42b54bca8.png"
                                    }
                                }
                            }
                });
                var wr = WebRequest.Create(api);
                wr.ContentType = "application/json";
                wr.Method = "POST";
                using (var sw = new StreamWriter(wr.GetRequestStream()))
                    sw.Write(json);
                wr.GetResponse();

            }
            catch
            {
                Messages.ShowAlertMessageSettings("Save webhook first", AlertStack);
            }
        }

        private void Unbind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DB db = new DB();

                DataTable table = new DataTable();

                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand command = new MySqlCommand("SELECT * FROM `keysbot` WHERE `botkeys` = @uL ", db.getConnection());

                string dbkey = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/dbkey.txt";
                string key = System.IO.File.ReadAllText(dbkey);
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = key;

                Console.WriteLine(key);
                MySqlCommand command2 = new MySqlCommand("DELETE FROM `keysbot` WHERE `botkeys` = @uL ", db.getConnection());
                command2.Parameters.Add("@uL", MySqlDbType.VarChar).Value = key;
                db.openConnection();
                command2.ExecuteNonQuery();
                Console.WriteLine("Удалил");
                Environment.Exit(0);
            }
            catch
            {
                MessageBox.Show("Sorry, smth went wrong");
            }
        }

        private void SwitchAnime_Click(object sender, RoutedEventArgs e)
        {
            string ThemeCheckPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/themeCheck.state";
            string ThemePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/theme.state";
            if (SwitchAnime.IsChecked == true)
            {
                ThemeCondition.ThemeCon = false;
                System.IO.File.WriteAllText(ThemePath, "false");
                System.IO.File.WriteAllText(ThemeCheckPath, "b");
            }
            else
            {
                ThemeCondition.ThemeCon = true;
                System.IO.File.WriteAllText(ThemePath, "true");
                System.IO.File.WriteAllText(ThemeCheckPath, "a");
            }
        }

        private void UpdateButtom_Click(object sender, RoutedEventArgs e)
        {
            string pathUpdater1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/Updater/Microsoft.Deployment.Compression.Cab.dll";
            string pathUpdater2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/Updater/Microsoft.Deployment.Compression.dll";
            string pathUpdater3 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/Updater/Microsoft.Deployment.WindowsInstaller.dll";
            string pathUpdater4 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/Updater/Microsoft.Deployment.WindowsInstaller.Package.dll";
            string pathUpdater5 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/Updater/Updater.exe.config";
            string pathUpdater6 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/Updater/Updater.exe";

            string updaterPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM//Updater/upd/Updater.exe";
            string extractPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM///Updater/upd";
            DialogResultConverter dialogResultConverter = new DialogResultConverter();

            WebClient webClient = new WebClient();
            if (webClient.DownloadString("https://stardomaio.ru/update").Contains("3.1.5"))
            {
                if (MessageBox.Show("Похоже вышло обновление!Установить?", "Stardom updater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) ;

                WebClient webClient1 = new WebClient();
                string lastVersionPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Updater/updates/Updater.zip";
                string exe = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/STARDAPP.exe";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom";

                webClient1.DownloadFile("http://stardomaio.ru/Updater.zip", lastVersionPath);

                if (System.IO.Directory.Exists(extractPath))
                {

                    System.IO.Directory.Delete(extractPath, true);
                }


                ZipFile.ExtractToDirectory(lastVersionPath, extractPath);

                try
                {
                    DirectoryInfo directoryinfo = new DirectoryInfo(extractPath);
                    if (directoryinfo.Exists) directoryinfo.Delete(true);
                    ZipFile.ExtractToDirectory(lastVersionPath, extractPath);
                }
                catch (System.IO.IOException b) { Console.WriteLine(b.Message); }
                System.Threading.Thread.Sleep(2000);

                bool exists = false;
                while (exists == false)
                {

                    Console.WriteLine("г");

                    if (System.IO.File.Exists(updaterPath))
                    {
                        Console.WriteLine("б");
                        exists = true;

                        System.Threading.Thread.Sleep(5000);
                        Process.Start(updaterPath);

                        Environment.Exit(0);
                        Console.WriteLine("обнова");
                    }
                    else
                    {

                        exists = false;

                    }


                    System.Threading.Thread.Sleep(300);
                }


            }
        }

        private void SaveCapMonsterApi_Click(object sender, RoutedEventArgs e)
        {
            string Path2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/CapMonster.api";
            string Cap = CaptchaApi.Text;
            System.IO.File.WriteAllText(Path2, Cap);
            Messages.ShowAlertMessageSettings("Capmonster API key saved", AlertStack);
        }

        private void TestCapMonster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(pathCapMonster) != true)
                {
                    Messages.ShowAlertMessageSettings("Save API key first", AlertStack);
                    return;
                }

                var request = new HttpRequest();

                string API_KEY = File.ReadAllText(pathCapMonster);
                var jsondata = new
                {
                    clientKey = API_KEY
                };

                string json_data = JsonConvert.SerializeObject(jsondata);
                HttpResponse response = request.Post("https://api.capmonster.cloud/getBalance", json_data, "application/json");
                JObject joResponse = JObject.Parse(response.ToString());
                Console.WriteLine(joResponse);
                string re_str = joResponse.ToString();
                JToken BalanceShow = JObject.Parse(re_str)["balance"];
                Messages.ShowAlertMessageSettings("Balance - " + BalanceShow.ToString(), AlertStack);
            }
            catch
            {
                Messages.ShowAlertMessageSettings("Error in posting to API", AlertStack);
            }
        }
    }
}