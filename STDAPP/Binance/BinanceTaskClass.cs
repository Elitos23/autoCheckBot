using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace STDAPP
{
    class BinanceTaskClass
    {

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/.local-chromium/win64-869685/chrome-win/chrome.exe";
        private Dictionary<string, string> headers = new Dictionary<string, string>(8);
        private Dictionary<string, string> body_fields = new Dictionary<string, string>(8);

        private async Task ShowStatus(string ShowLog, TaskPage mytask)
        {
            await App.Current.Dispatcher.Invoke(async () =>
            {
                mytask.Status.Content = ShowLog;

            });
        }


        private List<string> CaptchBank = new List<string>();

        private void FarmCaptcha(CancellationToken token, string productID)
        {
            Task Farming = new Task(() =>
            {
                while (true)
                {
                    string cap = BinanceCapthcaSorage.TaskCapthca(productID);
                    if (cap != "Error" & cap != null)
                    {
                        Debug.WriteLine("Add captcha to bank - " + cap);
                        CaptchBank.Add(cap);
                        System.Threading.Thread.Sleep(5000);
                    }

                    if (token.IsCancellationRequested)
                    {
                        Debug.WriteLine("Stoped");
                        break;
                    }
                }
            });
            Farming.Start();
        }
        private async Task WriteLog(string ShowLog, LogWindow log)
        {
            await App.Current.Dispatcher.Invoke(async () =>
            {
                log.LogTextBlock.Text += DateTime.Now + " - " + ShowLog + "\n";

            });
        }

        public async void Src(DataSourceStructures.DataGive dataGive, TaskPage mytask, CancellationToken token, LogWindow log)
        {
            try
            {
                void CheckStopedTask()
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Stoped");
                        token.ThrowIfCancellationRequested();
                    }
                }

                // Debug.WriteLine(nikeData.);
                await ShowStatus("Starting", mytask);
                await WriteLog("Starting", log);
                string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";  // discord hook
                string api = System.IO.File.ReadAllText(pathApi);
                if (dataGive.Proxy == null)
                {
                    throw new InvalidOperationException("No proxyPool");
                }

                string[] ProxyList = dataGive.Proxy.Split('\n');


                foreach (string pr in ProxyList)
                {
                    Debug.WriteLine(pr);
                }

                var client = new RestClient("https://www.binance.com/bapi/accounts/v1/public/authcenter/auth");
                var request = new RestRequest(Method.POST);

                await ShowStatus("Seting Data", mytask);
                Debug.WriteLine("Seting Data");
                await WriteLog("Seting Data", log);
                string[] Headers = dataGive.Headers.Split('\n');
                string cookie = dataGive.Cookies;
                string[] CookieArray = cookie.Split(';');
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82";

                foreach (string Cok in CookieArray)
                {
                    string[] KeyValue = Cok.Split('=');
                    request.AddCookie(HttpUtility.UrlEncode(KeyValue[0]).Substring(1), HttpUtility.UrlEncode(KeyValue[1]));
                }

                foreach (string Header in Headers)
                {
                    string[] KeyValue = Header.Split(':');
                    if (KeyValue[0] != "cookie" & KeyValue[0] != "referer" & KeyValue[0] != "origin" & KeyValue[0] != "sec-ch-ua" & KeyValue[0] != "sec-ch-ua-platform")
                    {
                        KeyValue[1] = KeyValue[1].Trim();
                        request.AddHeader(KeyValue[0], KeyValue[1]);
                        Debug.WriteLine(KeyValue[0]);
                        Debug.WriteLine(KeyValue[1]);
                    }
                }


                IRestResponse response = client.Execute(request);
                Debug.WriteLine(response.Content);
                await WriteLog("Auth response - " + response.Content, log);

                FarmCaptcha(token, dataGive.ProductLink);
                await WriteLog("Farm Captcha", log);
                CheckStopedTask();
                string DropTime = dataGive.DropTime[0] + ":" + dataGive.DropTime[1] + ":" + dataGive.DropTime[2] + ":" + dataGive.DropTime[3];
                if (DropTime != null)
                {
                    Debug.WriteLine(DropTime);
                    string[] DT = DropTime.Split(':');
                    CheckStopedTask();
                    DateTime drop = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(DT[0]), int.Parse(DT[1]), int.Parse(DT[2]), int.Parse(DT[3]));
                    Debug.WriteLine("Sleeping");
                    await ShowStatus("Sleeping", mytask);
                    await WriteLog("Sleeping", log);
                    await Task.Delay(drop - DateTime.Now);
                }
                Debug.WriteLine("Dudos");
                await ShowStatus("Dudos", mytask);
                await WriteLog("Checkouting", log);
                CheckStopedTask();


                while (true)
                {

                    foreach (string prox in ProxyList)
                    {
                        string[] wordsP = prox.Split(':');
                        Debug.WriteLine(prox);
                        WebProxy myproxy = new WebProxy(wordsP[0] + ":" + wordsP[1]);
                        myproxy.Credentials = new NetworkCredential(wordsP[2].Trim(), wordsP[3].Trim());


                        client = new RestClient("https://www.binance.com/bapi/nft/v1/private/nft/mystery-box/purchase");
                        client.Proxy = myproxy;
                        request = new RestRequest(Method.POST);
                        client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82";
                        foreach (string Cok in CookieArray)
                        {
                            string[] KeyValue = Cok.Split('=');
                            request.AddCookie(HttpUtility.UrlEncode(KeyValue[0]).Substring(1), HttpUtility.UrlEncode(KeyValue[1]));
                        }

                        foreach (string Header in Headers)
                        {
                            string[] KeyValue = Header.Split(':');
                            if (KeyValue[0] != "cookie" & KeyValue[0] != "referer" & KeyValue[0] != "origin" & KeyValue[0] != "sec-ch-ua" & KeyValue[0] != "sec-ch-ua-platform")
                            {
                                KeyValue[1] = KeyValue[1].Trim();
                                request.AddHeader(KeyValue[0], KeyValue[1]);
                                //Debug.WriteLine(KeyValue[0]);
                                // Debug.WriteLine(KeyValue[1]);
                            }
                        }

                        if (CaptchBank.Count != 0)
                        {
                            Debug.WriteLine("Take Captcha");
                            string CapTok = CaptchBank[CaptchBank.Count - 1];
                            request.AddHeader("x-nft-checkbot-sitekey", "6LeUPckbAAAAAIX0YxfqgiXvD3EOXSeuq0OpO8u_");
                            request.AddHeader("x-nft-checkbot-token", CapTok);
                        }
                        // request.AddHeader("cookie", "cid=GWRJFZtq; home-ui-ab=A; bnc-uuid=4d4052e6-783f-4b2b-8f77-7bfc9860dc0e; source=organic; campaign=www.google.com; _ga=GA1.2.387741376.1632244783; _gid=GA1.2.743278221.1632244783; sajssdk_2015_cross_new_user=1; sensorsdata2015jssdkcross=%7B%22distinct_id%22%3A%2217c095ed6d04c1-0e8d41b0e2c677-a7d173c-2764800-17c095ed6d1ced%22%2C%22first_id%22%3A%22%22%2C%22props%22%3A%7B%22%24latest_traffic_source_type%22%3A%22%E8%87%AA%E7%84%B6%E6%90%9C%E7%B4%A2%E6%B5%81%E9%87%8F%22%2C%22%24latest_search_keyword%22%3A%22%E6%9C%AA%E5%8F%96%E5%88%B0%E5%80%BC%22%2C%22%24latest_referrer%22%3A%22https%3A%2F%2Fwww.google.com%2F%22%7D%2C%22%24device_id%22%3A%2217c095ed6d04c1-0e8d41b0e2c677-a7d173c-2764800-17c095ed6d1ced%22%7D; BNC_FV_KEY_EXPIRE=1632331184248; BNC_FV_KEY=31f9d3536322ab60afaeaf12a140548e7ea12386; gtId=2170455b-6277-430a-b6dd-26b75a0ebc62; s9r1=DF2981D6921C60279096775FEA83EA42; lang=ru; cr00=3466D505533F460EB4D29527318DF307; d1og=web.183224229.2A1F6893573F98B1C03EA42A4F5ADC18; r2o1=web.183224229.14D8F6AAB3DD6E3A8DE07B240800685A; f30l=web.183224229.E71D6DABA868AEB2682BAD8F47873324; isAccountsLoggedIn=y; logined=y; p20t=web.183224229.A6C91201F705DA527F1067B7EAEE6F75; userPreferredCurrency=USD_USD; fiat-prefer-currency=RUB; nft-init-compliance=true");

                        request.AddParameter("application/json", $"{{\"number\":{dataGive.Amount},\"productId\":{dataGive.ProductLink}}}", ParameterType.RequestBody);
                        response = client.Execute(request);
                        Debug.WriteLine(response.Content);
                        if (response.Content.ToString().Contains("\"success\":true"))
                        {
                            Debug.WriteLine("Trahnull");
                            await ShowStatus("Trahnull", mytask);
                            await WriteLog("Trahnull", log);
                            string json = JsonConvert.SerializeObject(new
                            {
                                username = "STARDOM",
                                avatar_url = "https://postila.ru/data/2a/8f/b9/b7/2a8fb9b743e7cbb656006965245a64c42ec571aaf2edd010540a3fb42b54bca8.png",
                                embeds = new[]
                                        {
                               new {
                                    title = $"Binance Success",
                                    color = "65280",
                                    timestamp = DateTime.Now.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                    thumbnail = new
                                        {
                                            url = "https://postila.ru/data/2a/8f/b9/b7/2a8fb9b743e7cbb656006965245a64c42ec571aaf2edd010540a3fb42b54bca8.png"
                                        },
                                    fields = new[]
                                    {
                                        new
                                        {
                                            name = "Amount",
                                            value = dataGive.Amount
                                        },
                                        new
                                        {
                                            name = "Id",
                                            value = dataGive.ProductLink
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
                            return;
                        }
                        CheckStopedTask();
                        System.Threading.Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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