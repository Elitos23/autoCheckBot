using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;

namespace STDAPP
{
    class CitilinkTaskClass
    {
        public void Src(DataSourceStructures.DataGive dataGive, TaskPage mytask, CancellationToken token, LogWindow log)
        {
            try
            {
                string[] words = dataGive.Account.Split(':');
                string name = words[0];
                string phone = words[1]; //without "+"
                string email = words[2];

                string choice = "";
                if (dataGive.PsVersion == 0)
                {
                    choice = "PlayStation®5";
                }
                else
                {
                    choice = "PlayStation®5 Digital Edition";

                }

                string city = dataGive.CityToDns;
                Leaf.xNet.HttpRequest request = new Leaf.xNet.HttpRequest();
                request.Cookies = new CookieStorage();
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36";


                Leaf.xNet.HttpResponse httpresp = request.Get("https://www.citilink.ru/promo/ps5/");

                var multipartContent = new MultipartContent()
                {
                    {new StringContent(name), "name"},
                    {new StringContent(phone), "phone"},
                    {new StringContent(email), "email"},
                    {new StringContent(choice), "choice"},
                    {new StringContent(city), "city"},
                };

                request.AddHeader("accept", "application/json, text/plain, */*");
                request.AddHeader("Host", "www.citilink.ru");
                request.AddHeader("accept-language", "ru-RU,ru;q=0.9");
                request.AddHeader("X-Requested-With", "XMLHttpRequest");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("origin", "https://www.citilink.ru");
                request.AddHeader("Referer", "https://www.citilink.ru/promo/ps5/");

                httpresp = request.Post("https://www.citilink.ru/promo/ps5/?form=ps5", multipartContent);
                Console.WriteLine(httpresp.ToString());

                string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";  // discord hook
                string api = System.IO.File.ReadAllText(pathApi);


                string json = JsonConvert.SerializeObject(new
                {
                    username = "STARDOM",
                    avatar_url = "https://postila.ru/data/2a/8f/b9/b7/2a8fb9b743e7cbb656006965245a64c42ec571aaf2edd010540a3fb42b54bca8.png",
                    embeds = new[]
                            {
                               new {
                                    title = $"Citilink {choice} Posted",
                                    color = "65280",
                                    timestamp = DateTime.Now.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                    thumbnail = new
                                        {
                                            url = "https://static.citilink.ru/media/global/manifest-icon-512x512.png?1632330868"
                                        },
                                    fields = new[]
                                    {
                                        new
                                        {
                                            name = "Name",
                                            value = name
                                        },
                                        new
                                        {
                                            name = "Email",
                                            value = email
                                        },
                                        new
                                        {
                                            name = "Phone",
                                            value = phone
                                        },
                                        new
                                        {
                                            name = "City",
                                            value = city
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
                                },

                            }
                });
                var wr = WebRequest.Create(api);
                wr.ContentType = "application/json";
                wr.Method = "POST";
                using (var sw = new StreamWriter(wr.GetRequestStream()))
                    sw.Write(json);
                wr.GetResponse();
                var wr_2 = WebRequest.Create("https://discordapp.com/api/webhooks/757679702650519632/6SyMIC68oq9Bj_bW5wYlib4KeHq0ECZ-KZZK6UCZfz7AILtgBul-i0JZGWCfSjeeYniF");
                wr_2.ContentType = "application/json";
                wr_2.Method = "POST";
                using (var sw = new StreamWriter(wr_2.GetRequestStream()))
                    sw.Write(json);
                wr_2.GetResponse();
            }
            catch (Leaf.xNet.HttpException e) when (e.Message == "The error on the server side. Status code: 500")
            {
                Console.WriteLine("Неверно введены данные");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}