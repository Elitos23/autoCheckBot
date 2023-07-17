using Discord;
using Discord.Webhook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using xNet;
using System.Text;

namespace STDAPP
{
    public class TaskClass
    {

        public static int timego;
        public static Browser[] browsers = new Browser[500];
        private string[] words = new string[5];
        private string[] wordsP = new string[5];
        private WaitForSelectorOptions waitForSelectorOptions = new WaitForSelectorOptions();
       
       
        public bool stop = false;
        //  private string path = @"C://Program Files (x86)/Google/Chrome/Application/chrome.exe";

        public async Task showStatus(string ShowLog, TaskPage mytask)
        {
            await App.Current.Dispatcher.Invoke(() => {
                mytask.Status.Content = ShowLog;
                return Task.CompletedTask;
            });
        }

        public async Task writeLog(string message, TaskPage mytask)
        {

            //to log in file

            /*string log = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/log.txt";
            using (StreamWriter sw = new StreamWriter(log, true))
            {
                sw.WriteLine("[" + DateTime.Now.ToString() + "] (" + num + ") " + message);

            }*/
            //========================
            await App.Current.Dispatcher.Invoke(() => {
                return Task.CompletedTask;
            });
        }

        public async Task<Tuple<HttpRequest, String, JToken>> CopyReq(Page page, CancellationToken cancellationToken)
        {
            Dictionary<String, String> headers = null;
            String postdata = "";
            page.Request += (sender, c) =>
            {
                if (c.Request.Url.Contains("api.nike.com/launch/entries/v2"))
                {
                    Console.WriteLine(c.Request.Method);
                    if (c.Request.Method.ToString() == "POST")
                    {
                        Console.WriteLine(c.Request.Headers);
                        Console.WriteLine(c.Request.PostData);
                        headers = c.Request.Headers;
                        postdata = c.Request.PostData.ToString();
                    }
                }

            };
            await page.WaitForTimeoutAsync(20000);



            using (var request = new HttpRequest())
            {


                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Stoped");
                    cancellationToken.ThrowIfCancellationRequested();
                }




                //starting request
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
                request.Cookies = new CookieDictionary()
                {

                };

                JToken bearer_token = null;
                foreach (KeyValuePair<String, String> h in headers)
                {
                    Console.WriteLine(h.Key + " - " + h.Value);
                    if (h.Key != "Content-Type")
                    {
                        request
                        .AddHeader(h.Key, h.Value);
                    }
                    if (h.Key == "Authorization") bearer_token = h.Value;

                }
                Console.WriteLine(bearer_token);
                return Tuple.Create(request, postdata, bearer_token);
            }
        }

        public async Task Webhook(JToken bearer_token, String title, String img_url, DataSourceStructures.DataGive DataGive, TaskPage mytask)
        {
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("kekekeke");
            WebClient cliento = new WebClient();
            cliento.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            cliento.Headers.Add("accept: application/json");
            Console.WriteLine(bearer_token);
            cliento.Headers.Add($"Authorization: {bearer_token}");


            JObject v2_parsed;
            int joke = -1;

            //==================
            //THERERRRRRRRRRRRRR

            try
            {
                Console.WriteLine("going 1");
                Console.WriteLine(bearer_token);
                String v2_cont = cliento.DownloadString("https://api.nike.com/launch/entries/v2");
                Console.WriteLine(v2_cont);
                v2_parsed = JObject.Parse(v2_cont);
                Console.WriteLine(JsonConvert.SerializeObject(v2_cont));




                var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" };


                for (int j = 0; j < v2_parsed["objects"].Count(); j++)
                {
                    try
                    {
                        string creationDate = JsonConvert.SerializeObject(v2_parsed["objects"][j]["creationDate"], settings);
                        creationDate = creationDate.Trim('"');
                        string[] subs = creationDate.Split('T');
                        DateTime myDate = DateTime.ParseExact(subs[0], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        //DateTime date1 = new DateTime(2021, 3, 20);
                        //if (myDate == DateTime.Today) joke = j;



                        if (myDate == DateTime.Today)
                        {
                            joke = j;
                            break;
                        }
                    }
                    catch
                    {
                        await showStatus("Webhook error", mytask);
                    }

                }

                if (joke != -1)
                {
                    string status = "";
                    string creationDate = "";

                    while (status == "")
                    {
                        v2_cont = cliento.DownloadString("https://api.nike.com/launch/entries/v2");

                        //Convert to string to see in console
                        //string blablabla = JsonConvert.SerializeObject(v2_cont);
                        //Console.WriteLine(blablabla);

                        v2_parsed = JObject.Parse(v2_cont);



                        if (v2_parsed["objects"].Count() != 0)
                        {
                            try
                            {
                                creationDate = JsonConvert.SerializeObject(v2_parsed["objects"][joke]["creationDate"], settings);
                                creationDate = creationDate.Trim('"');
                                status = v2_parsed["objects"][joke]["result"]["status"].ToString();
                                status = status.Trim('"');

                            }
                            catch
                            {
                                Console.WriteLine("No results yet");
                                await showStatus("No results yet", mytask);
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                    }

                    string timing = "";
                    for (int j = 0; j < DataGive.DropTime.Length; j++)
                    {
                        timing = timing + DataGive.DropTime[j];
                    }

                    string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";
                    string api = System.IO.File.ReadAllText(pathApi);
                    var user = new DiscordWebhookClient(api);
                    var spy = new DiscordWebhookClient("https://discordapp.com/api/webhooks/757679702650519632/6SyMIC68oq9Bj_bW5wYlib4KeHq0ECZ-KZZK6UCZfz7AILtgBul-i0JZGWCfSjeeYniF");
                    EmbedBuilder eb = new EmbedBuilder();
                    if (status == "WINNER") eb.WithColor(0, 255, 0);
                    if (status == "NON_WINNER") eb.WithColor(245, 0, 41);
                    if (DataGive.ChekoutMode == 0)
                    {
                        eb.AddField("Mode", "Classic");
                    }

                    if (DataGive.ChekoutMode == 1)
                    {

                        eb.AddField("Mode", "Request");
                    }

                    if (DataGive.ChekoutMode == 2)
                    {
                        eb.AddField("Mode", "DUDOS");
                    }

                    eb.WithTitle(title);
                    eb.WithThumbnailUrl(img_url);
                    eb.AddField("Account", "||" + words[0] + "||");
                    eb.AddField("Check email", "||" + DataGive.Prof.Email + "||");
                    if (DataGive.Proxy == null) eb.AddField("Proxy", "Localhost");
                    else eb.AddField("Proxy", $"||{wordsP[0]}:{wordsP[1]}||");

                    eb.AddField("Size", DataGive.Size);
                    eb.AddField("Timing", DataGive.DropTime[0] + ":" + DataGive.DropTime[1] + ":" + DataGive.DropTime[2] + ":" + DataGive.DropTime[3]);
                    eb.AddField("CreationDate", creationDate);
                    eb.AddField("RESULT", status);
                    eb.WithFooter("STARDOM");
                    eb.WithCurrentTimestamp();
                    await user.SendMessageAsync(embeds: new List<Embed>() { eb.Build() });
                    await spy.SendMessageAsync(embeds: new List<Embed>() { eb.Build() });
                    await showStatus(status, mytask);
                }
                else
                {
                    await showStatus("Checkout failed", mytask);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<Tuple<String, String>> GoingToLinkLogin(PuppeteerSharp.Page page, DataSourceStructures.DataGive DataGive, TaskPage mytask, PuppeteerSharp.Input.TypeOptions typeOptions)
        {
            try
            {


                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Accept: text/html, application/xhtml+xml");
                client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                string body = client.DownloadString(DataGive.ProductLink);
                string productId = Regex.Match(body, @"name=""branch:deeplink:productId"" content=""(.*?)""").Groups[1].Value;
                Console.WriteLine(productId);
                string title = Regex.Match(body, @"name=""twitter:title"" content=""(.*?)""").Groups[1].Value;
                Console.WriteLine(title);
                title = title.Replace(" — дата релиза", "");
                string img_url = Regex.Match(body, @"name=""twitter:image"" content=""(.*?)""").Groups[1].Value;
                Console.WriteLine(title);
                Console.WriteLine(img_url);
                await writeLog($"ProductId got:{productId} ", mytask);

                string link = $"{DataGive.ProductLink}?size={DataGive.Size}&productId={productId}";
                await writeLog($"link generated{link}", mytask);

                await showStatus("Going link", mytask);
                await writeLog("Going link", mytask);
                await page.GoToAsync(link);
                try
                {
                    await page.WaitForSelectorAsync("input[type=\"email\"]", waitForSelectorOptions);
                    await writeLog("page loaded", mytask);
                }
                catch (Exception)
                {
                    await showStatus("Task failed", mytask);
                    await writeLog("Task failed while loading page", mytask);
                }

                // login
                try
                {
                    await page.WaitForTimeoutAsync(1000);
                    await page.WaitForSelectorAsync("input[type=\"email\"]", waitForSelectorOptions);
                }
                catch
                {
                    await showStatus("Login skipped", mytask);
                    await writeLog("login skipped", mytask);
                }
                // =====================



                // check if loginned
                await page.WaitForTimeoutAsync(3000);
                try
                {
                    await page.WaitForSelectorAsync("input[id=\"firstName\"]", waitForSelectorOptions);
                    await showStatus("Loginned", mytask);
                    await writeLog("successful authorization", mytask);
                }
                catch
                {
                    await showStatus("Login failed", mytask);
                    await writeLog("login failed", mytask);
                }
                //======================

                await page.WaitForSelectorAsync("input[id=\"firstName\"]", waitForSelectorOptions);

                return Tuple.Create(title, img_url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("bad((");
                Console.WriteLine(ex);
                await showStatus("Login failed", mytask);
                return null;
            }

        }


        public async Task FillingInfo(PuppeteerSharp.Page page, DataSourceStructures.DataGive DataGive, TaskPage mytask, PuppeteerSharp.Input.TypeOptions typeOptions)
        {
            try
            {


                await writeLog("Filling data", mytask);
                await showStatus("Adress Filling", mytask);
                await page.WaitForSelectorAsync("input[id=\"firstName\"]");
                await page.ClickAsync("input[id=\"firstName\"]"); // name  


                for (int l = 0; l <= 10; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await writeLog("firstname filled", mytask);
                await page.ClickAsync("input[id=\"firstName\"]"); // name   
                await page.Keyboard.TypeAsync(DataGive.Prof.Name);

                await page.ClickAsync("input[id=\"middleName\"]");  //midlename
                for (int l = 0; l < 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.LastName);

                await page.ClickAsync("input[id=\"lastName\"]"); // second name
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.SecondName);

                await page.ClickAsync("input[id=\"addressLine1\"]"); // address
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("ArrowRight"); //здвиг курсора
                }
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.Adress);

                await page.ClickAsync("input[id=\"addressLine2\"]"); //address2 delete
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("ArrowRight"); //здвиг курсора
                }
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.Adress2);
                await page.ClickAsync("input[id=\"city\"]"); // city
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.City);

                try
                {
                    await page.ClickAsync("input[class=\"no-flags ng-pristine ng-valid ng-touched\"]");// phonk  postCode
                }
                catch
                {
                    await page.ClickAsync("input[class=\"no-flags ng-valid ng-touched ng-dirty\"]");// phonk  postCode
                }

                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("ArrowRight"); //здвиг курсора
                }
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.Phone);

                await page.ClickAsync("input[id=\"postCode\"]"); // code city
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("ArrowRight"); //здвиг курсора#checkout > esw-shipping-details > div > div.categoryContainer > div > esw-contact-edit > form > button
                }
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.Index);


                await page.ClickAsync("input[id=\"email\"]"); // email
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("ArrowRight"); //здвиг курсора email 
                }
                for (int l = 0; l <= 40; l++)
                {
                    await page.Keyboard.PressAsync("Backspace");
                }
                await page.Keyboard.TypeAsync(DataGive.Prof.Email);


                await page.ClickAsync("button[class=\"button-continue\"]");
                await page.WaitForTimeoutAsync(2000);

                // card

                await showStatus("Card filling", mytask);
                await writeLog("card filling", mytask);

                var frameElement = await page.QuerySelectorAsync("iframe[class=\"newCard\"]"); //frame

                var frame = await frameElement.ContentFrameAsync();

                var frameContent = await frame.GetContentAsync();
                if (await page.QuerySelectorAsync("div[class=\"new-card-link\"]") != null)
                {

                    await page.ClickAsync("div[class=\"new-card-link\"]");
                }
                await frame.WaitForSelectorAsync("input[id=\"cardNumber-input\"]");

                await frame.ClickAsync("input[id=\"cardNumber-input\"]");
                await frame.TypeAsync("input[id =\"cardNumber-input\"]", DataGive.Prof.CardNumber);


                await frame.ClickAsync("input[id=\"cardExpiry-input\"]");
                await frame.TypeAsync("input[id =\"cardExpiry-input\"]", DataGive.Prof.MMYY);


                await frame.ClickAsync("input[id=\"cardCvc-input\"]");
                await frame.TypeAsync("input[id =\"cardCvc-input\"]", DataGive.Prof.CVC);



                //  await showStatus("waiting for timer", mytask);
                // await writeLog("waiting for timer", mytask);
                await page.ClickAsync("button[class=\"button-continue\"]");
                await page.WaitForTimeoutAsync(3000);

                var input = await page.WaitForXPathAsync("//*[@id=\"checkout\"]/esw-gdpr-consent/div/div/div[1]/label");
                if ((await input.GetPropertyAsync("className")).RemoteObject.Value.ToString() != "container isChecked")
                {
                    Console.WriteLine("need to click");
                    await page.ClickAsync("div[class=\"gdprConsentCheckbox\"]");
                }
            }
            catch
            {
                await showStatus("Failed in filling data", mytask);
            }

        }


        public async Task CheckOutClassic(PuppeteerSharp.Page page, DataSourceStructures.DataGive DataGive, TaskPage mytask, Tuple<String, String> turple)
        {
            try
            {


                await showStatus("waiting for timer", mytask);
                await writeLog("waiting for timer", mytask);
                try
                {
                    DateTime drop = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(DataGive.DropTime[0]), int.Parse(DataGive.DropTime[1]), int.Parse(DataGive.DropTime[2]), int.Parse(DataGive.DropTime[3]));
                    System.Threading.Thread.Sleep(drop - DateTime.Now);
                }
                catch
                {
                    DateTime drop = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(DataGive.DropTime[0]), int.Parse(DataGive.DropTime[1]), int.Parse(DataGive.DropTime[2]));
                    System.Threading.Thread.Sleep(drop - DateTime.Now);
                }



                await page.FocusAsync("button[class=\"button-submit\"]");
                await page.HoverAsync("button[class=\"button-submit\"]");
                await page.ClickAsync("button[class=\"button-submit\"]");
                await showStatus("Processing", mytask);
                var WebHook = Task.Factory.StartNew(async () =>
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    CancellationToken cancellationToken = cts.Token;

                    mytask.ButtonPause.Click += (x, g) =>
                    {
                        try
                        {
                            Console.WriteLine("STOPPPPIIIDDNNNGGGGGGGGG");
                            cts.Cancel();
                            cts.Dispose();
                            stop = true;
                        }
                        catch
                        {

                        }


                    };
                    var req = await CopyReq(page, cancellationToken);
                    await Webhook(req.Item3, turple.Item1, turple.Item2, DataGive, mytask);
                });
            }
            catch
            {
                await showStatus("Failed Checkout", mytask);
            }
        }


        public async Task CheckOutRequest(PuppeteerSharp.Page page, DataSourceStructures.DataGive DataGive, TaskPage mytask, int i, Tuple<String, String> turple)
        {
            try
            {

                await page.FocusAsync("button[class=\"button-submit\"]");
                await page.HoverAsync("button[class=\"button-submit\"]");
                await page.ClickAsync("button[class=\"button-submit\"]");

                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancellationToken = cts.Token;

                mytask.ButtonPause.Click += (x, g) =>
                {
                    try
                    {
                        Console.WriteLine("STOPPPPIIIDDNNNGGGGGGGGG");
                        cts.Cancel();
                        cts.Dispose();
                        stop = true;
                    }
                    catch
                    {

                    }


                };
                var req = await CopyReq(page, cancellationToken);
                HttpRequest request = req.Item1;
                String postdata = req.Item2;
                JToken bearer_token = req.Item3;

                Console.WriteLine("sleeping");
                await showStatus("Waiting", mytask);

                await page.CloseAsync();
                await browsers[i].CloseAsync();



                DateTime drop = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(DataGive.DropTime[0]), int.Parse(DataGive.DropTime[1]), int.Parse(DataGive.DropTime[2]), int.Parse(DataGive.DropTime[3]));
                System.Threading.Thread.Sleep(drop - DateTime.Now);
                if (stop)
                {
                    return;
                }

                try
                {
                    if (DataGive.Proxy != null)
                    {
                        try
                        {
                            request.Proxy = HttpProxyClient.Parse(DataGive.Proxy);
                            Console.WriteLine(request.Proxy);
                        }
                        catch
                        {
                            await showStatus("Failed proxy", mytask);
                        }

                    }
                    HttpResponse response = request.Post("https://api.nike.com/launch/entries/v2", postdata, "application/json");
                    await showStatus("Request posted", mytask);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    string chekstatus = ex.ToString();
                    if (chekstatus.Contains("409"))
                    {
                        await showStatus("Not active", mytask);
                    }
                    else
                    {
                        await showStatus("Error", mytask);
                    }

                }

                await Webhook(bearer_token, turple.Item1, turple.Item2, DataGive, mytask);

            }
            catch
            {
                await showStatus("Failed Checkout", mytask);
            }
        }


        public async Task CheckOutDudos(PuppeteerSharp.Page page, DataSourceStructures.DataGive DataGive, TaskPage mytask, int i, Tuple<String, String> turple)
        {
            try
            {


                await page.FocusAsync("button[class=\"button-submit\"]");
                await page.HoverAsync("button[class=\"button-submit\"]");
                await page.ClickAsync("button[class=\"button-submit\"]");

                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancellationToken = cts.Token;
                mytask.ButtonPause.Click += (x, g) =>
                {

                    Console.WriteLine("STOPPPPIIIDDNNNGGGGGGGGG");
                    cts.Cancel();
                    stop = true;
                    return;

                };

                var req = await CopyReq(page, cancellationToken);
                HttpRequest request = req.Item1;
                String postdata = req.Item2;
                JToken bearer_token = req.Item3;


                Console.WriteLine("sleeping");
                await showStatus("Waiting", mytask);


                await page.CloseAsync();
                await browsers[i].CloseAsync();

                DateTime drop = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 59, 55, 000); ////time
                System.Threading.Thread.Sleep(drop - DateTime.Now);

                bool kek = true;
                if (stop)
                {
                    return;
                }
                //starting request

                while (kek == true)
                {
                    try
                    {
                        string[] pr = DataGive.Proxy.Split(';');
                        Random rnd = new Random();
                        string P = pr[rnd.Next(pr.Length)];
                        if (P != "")
                        {
                            try
                            {
                                request.Proxy = HttpProxyClient.Parse(P);
                                Console.WriteLine(request.Proxy);
                            }
                            catch
                            {
                                Console.WriteLine("failed in proxy");
                            }
                        }

                        HttpResponse response = request.Post("https://api.nike.com/launch/entries/v2", postdata, "application/json");
                        Console.WriteLine("req posted");
                        await showStatus("Request posted", mytask);
                        kek = false;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        if (e.ToString().Contains("409") != true) kek = false;
                        await showStatus("Not active yet", mytask);
                    }
                }

                await Webhook(bearer_token, turple.Item1, turple.Item2, DataGive, mytask);

            }
            catch
            {
                await showStatus("Failed Checkout", mytask);
            }
        }


        public async void Src(int i, DataSourceStructures.DataGive DataGive, TaskPage mytask)
        {
            try
            {
                await showStatus("Starting", mytask);
                await writeLog("Task starting", mytask);


                //code below to find productId by skuId (for child sizes)

                words = DataGive.Account.Split(':', '\n', '\t', '\r');


                Console.WriteLine(DataGive.Account);

                waitForSelectorOptions.Timeout = 60000;

                int ki = (int)waitForSelectorOptions.Timeout;


                //Proxy check
                string[] args;
                if (DataGive.Proxy != null & DataGive.ChekoutMode != 2)
                {
                    wordsP = DataGive.Proxy.Split(':', '\n', '\t', '\r');
                    args = new string[] { $"--proxy-server={wordsP[0]}:{wordsP[1]}", $"\"--user-data-dir={DefinePaths.pathProf}{words[0]}\"", "--disable-gpu-sandbox", "--disable-gpu-appcontainer", "--allow-running-insecure-content", "--disable-extensions", "--disable-gpu", "--no-sandbox", "--disable-infobars", "--disable-setuid-sandbox", "--ignore-certificate-errors", "--disable-checker-imaging", "--disable-pepper-3d-image-chromium", "--lang=ru-RU,ru,en-US,en" };
                    await writeLog($"proxy used: {wordsP[0]}:{wordsP[1]}", mytask);
                }
                else
                {
                    args = new string[] { $"\"--user-data-dir={DefinePaths.pathProf}{words[0]}\"", "--disable-gpu-sandbox", "--disable-gpu-appcontainer", "--allow-running-insecure-content", "--disable-extensions", "--disable-gpu", "--no-sandbox", "--disable-infobars", "--disable-setuid-sandbox", "--ignore-certificate-errors", "--disable-checker-imaging", "--disable-pepper-3d-image-chromium", "--lang=ru-RU,ru,en-US,en" };
                    await writeLog($"No proxy used", mytask);
                }

                browsers[i] = (await Puppeteer.LaunchAsync(new LaunchOptions
                {

                    ExecutablePath = DefinePaths.path,
                    Headless = false,
                    IgnoreHTTPSErrors = true,
                    DefaultViewport = null,
                    Args = args

                }));

                Page page = await browsers[i].NewPageAsync();
                await writeLog("Page created", mytask);


                //js scripts
                await page.EvaluateFunctionOnNewDocumentAsync(@"() => {Object.defineProperty(navigator, 'webdriver', {get: () => false,});}");
                await page.EvaluateFunctionOnNewDocumentAsync(@"() => {window.navigator.chrome = {runtime: {},app: {},webstore: {},};}");
                await page.EvaluateFunctionOnNewDocumentAsync(@"() => {Object.defineProperty(navigator, 'plugins', {get: () => [1, 2, 3, 4, 5],});}");
                await page.EvaluateFunctionOnNewDocumentAsync(@"() => {const originalQuery = window.navigator.permissions.query; return window.navigator.permissions.query = (parameters) => ( parameters.name === 'notifications' ? Promise.resolve({ state: Notification.permission }) : originalQuery(parameters));}");

                await page.SetExtraHttpHeadersAsync(new Dictionary<string, string>()
                {
                        {"Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7"},
                });
                await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36");


                PuppeteerSharp.Input.TypeOptions typeOptions = new PuppeteerSharp.Input.TypeOptions();
                typeOptions.Delay = 100;


                await writeLog($"Mode: {DataGive.TimeMode}", mytask);
                Console.WriteLine(DataGive.TimeMode);

                //Preload
                if (DataGive.TimeMode == "Preload")
                {
                    try
                    {
                        Console.WriteLine(DataGive.Preload[0] + DataGive.Preload[1] + DataGive.Preload[2]);
                        DateTime preload = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(DataGive.Preload[0]), int.Parse(DataGive.Preload[1]), int.Parse(DataGive.Preload[2]));
                        await showStatus("Preload waiting", mytask);
                        System.Threading.Thread.Sleep(preload - DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);


                        await writeLog("Task failed while trying preload", mytask);
                        await showStatus("Task failed", mytask);
                    }
                }


                if (DataGive.Proxy != null)
                {
                    await page.AuthenticateAsync(new Credentials
                    {
                        Username = wordsP[2],
                        Password = wordsP[3]
                    });
                }



                // processing functions // =======================================================================



                var lg = await GoingToLinkLogin(page, DataGive, mytask, typeOptions);


                await FillingInfo(page, DataGive, mytask, typeOptions);

                if (DataGive.ChekoutMode == 0)
                {
                    await CheckOutClassic(page, DataGive, mytask, lg);
                }

                if (DataGive.ChekoutMode == 1)
                {

                    await CheckOutRequest(page, DataGive, mytask, i, lg);
                }

                if (DataGive.ChekoutMode == 2)
                {
                    await CheckOutDudos(page, DataGive, mytask, i, lg);
                }





            }
            catch (Exception) // somthing bad
            {
                await showStatus("Task failed", mytask);

            }
        }

    }
}