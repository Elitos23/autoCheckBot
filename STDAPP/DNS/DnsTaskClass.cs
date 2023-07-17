using Discord;
using Discord.Webhook;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xNet;
using PuppeteerSharp;
using System.Threading;
using Newtonsoft.Json.Linq;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace STDAPP
{
    public class DnsTaskClass
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
        private static String createJSForm(Dictionary<string, string> headers, string url, string method, Dictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"const coolFormData = new FormData();");
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                sb.Append($"coolFormData.append('{keyValue.Key}', '{keyValue.Value}');");
            }
            sb.Append($"let response = await fetch('{url}', {{method: '{method}', headers: {{");
            foreach (KeyValuePair<string, string> keyValue in headers)
            {
                sb.Append($"\"{keyValue.Key}\":\"{keyValue.Value}\",");
            }
            sb.Append($"}}, body:coolFormData}});");
            sb.Append("return response.text()");
            return sb.ToString();
        }

        private static String makeJS(Dictionary<string, string> headers, string url, string method, string body)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"await fetch('{url}', {{method: '{method}', headers: {{");
            foreach (KeyValuePair<string, string> keyValue in headers)
            {
                sb.Append($"\"{keyValue.Key}\":\"{keyValue.Value}\",");
            }
            if (body != "") sb.Append($"}}, body:'{body}'}})");
            else sb.Append("}})");
            sb.Append(".then(response => {return response.text();})");
            return sb.ToString();
        }

        private static String makeJSJson(Dictionary<string, string> headers, string url, string method, string body)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"await fetch('{url}', {{method: '{method}', headers: {{");
            foreach (KeyValuePair<string, string> keyValue in headers)
            {
                sb.Append($"\"{keyValue.Key}\":\"{keyValue.Value}\",");
            }
            if (body != "") sb.Append($"}}, body:'{body}'}})");
            else sb.Append("}})");
            sb.Append(".then(response => {return response.json();})");
            return sb.ToString();
        }



        public static Browser[] browsers = new Browser[500];
        private HttpRequest request = new HttpRequest();
        private WaitForSelectorOptions waitForSelectorOptions = new WaitForSelectorOptions();
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/.local-chromium/win64-869685/chrome-win/chrome.exe";
        private string[] wordsP = new string[5];
        private string[] words = new string[3];
        private string GetRandomCapthaFromBank(int mode)
        {
            string tok = "";
            try
            {
                Console.WriteLine("Geting captcha from bank");
                // Random r = new Random();
                //      tok = CaptchaStorage.CaptchaBankReady[r.Next(0, CaptchaStorage.CaptchaBankReady.Count)];
                int captchaIndex = -1;
                if (mode == 0)
                {
                    captchaIndex = CaptchBank.Count - 1;
                }
                else
                {
                    captchaIndex = CaptchaStorage.CaptchaBankReady.Count - 1;
                }

                if (captchaIndex == -1)
                {
                    Console.WriteLine("Captcha bank was null");
                    return "";
                }
                else
                {
                    if (mode == 0)
                    {
                        tok = CaptchBank[captchaIndex];
                    }
                    else
                    {
                        tok = CaptchaStorage.CaptchaBankReady[captchaIndex];
                    }

                }

                Console.WriteLine(tok);
            }
            catch
            {
                Console.WriteLine("Error in getting captcha solution");
            }

            return tok;
        }

        private Dictionary<string, string> headers = new Dictionary<string, string>(8);
        private Dictionary<string, string> body_fields = new Dictionary<string, string>(8);
        private string Req = "";
        private Newtonsoft.Json.Linq.JToken res;
        private string csrf0 = "";
        private string process = "";
        private string csrf = "";
        private string title = "";
        private string id = "";
        private List<string> CaptchBank = new List<string>();

        private void FarmCaptcha(CancellationToken token)
        {
            Task Farming = new Task(() =>
            {
                while (true)
                {
                    string cap = CaptchaStorage.TaskCapthca();
                    if (cap != "Error" & cap != null)
                    {
                        Console.WriteLine("Add captcha to bank - " + cap);
                        CaptchBank.Add(cap);
                        System.Threading.Thread.Sleep(5000);
                    }

                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Stoped");
                        break;
                    }
                }
            });
            Farming.Start();
        }

        public async void Src(int i, TaskPage mytask, DataSourceStructures.DataGive dataGive, LogWindow log, CancellationToken token)
        {
            try
            {
                Console.WriteLine(dataGive.DnsModeToDns);




                void CheckStopedTask()
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Stoped");
                        token.ThrowIfCancellationRequested();
                    }
                }

                string[] args;
                if (dataGive.Proxy != null)
                {
                    wordsP = dataGive.Proxy.Split(':', '\n', '\t', '\r');
                    args = new string[] { $"--proxy-server={wordsP[0]}:{wordsP[1]}", "--disable-gpu-sandbox", "--disable-gpu-appcontainer", "--allow-running-insecure-content", "--disable-extensions", "--disable-gpu", "--no-sandbox", "--disable-infobars", "--disable-setuid-sandbox", "--ignore-certificate-errors", "--disable-checker-imaging", "--disable-pepper-3d-image-chromium", "--lang=ru-RU,ru,en-US,en" };
                }
                else
                {
                    args = new string[] { "--disable-gpu-sandbox", "--disable-gpu-appcontainer", "--allow-running-insecure-content", "--disable-extensions", "--disable-gpu", "--no-sandbox", "--disable-infobars", "--disable-setuid-sandbox", "--ignore-certificate-errors", "--disable-checker-imaging", "--disable-pepper-3d-image-chromium", "--lang=ru-RU,ru,en-US,en" };
                }

                await ShowStatus("Starting...", mytask);
                await WriteLog("Starting", log);
                words = dataGive.Account.Split(':', '\n', '\t', '\r');
                Console.WriteLine(words[0] + " " + words[1]);
                Console.WriteLine(dataGive.ProductLink);
                Console.WriteLine("City - " + dataGive.CityToDns);
                Console.WriteLine("Mode - " + dataGive.DnsModeToDns);
                Console.WriteLine("Shop - " + dataGive.ShopIdToDns);

                request.UserAgent = Http.ChromeUserAgent();
                request.Cookies = new CookieDictionary();


                browsers[i] = (await Puppeteer.LaunchAsync(new LaunchOptions
                {

                    ExecutablePath = path,
                    Headless = false,
                    IgnoreHTTPSErrors = true,
                    DefaultViewport = null,
                    Args = args

                }));

                Page page = await browsers[i].NewPageAsync();

                if (dataGive.Proxy != null)
                {
                    await page.AuthenticateAsync(new Credentials
                    {
                        Username = wordsP[2],
                        Password = wordsP[3]
                    });
                }

                CheckStopedTask();
                await page.GoToAsync("https://www.dns-shop.ru");
                System.Threading.Thread.Sleep(2000);

                //login request
                //========================================================
                //========================================================
                headers.Add("Accept", "*/*");
                headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                headers.Add("X-Requested-With", "XMLHttpRequest");
                headers.Add("Origin", "https://www.dns-shop.ru");
                headers.Add("X-CSRF-Token", "_WSIiHPgpShwFIw1s9t4PWpVNbfc2B4JmX2MIv3Zg0a7DbnpRJbcbSZDtVL3lSpRDy1dgovoUkjtO-FxqrXIJw==");
                headers.Add("Referer", "https://www.dns-shop.ru/no-referrer");
                headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");

                body_fields.Add("LoginPasswordAuthorizationLoadForm[login]", words[0]);
                body_fields.Add("LoginPasswordAuthorizationLoadForm[password]", words[1]);
                body_fields.Add("LoginPasswordAuthorizationLoadForm[token]", GetRandomCapthaFromBank(dataGive.DnsModeToDns));
                await WriteLog("Login", log);
                Req = createJSForm(headers, "https://www.dns-shop.ru/auth/auth/login-password-authorization/", "POST", body_fields); // Login request 

                Console.WriteLine(Req);

                res = page.EvaluateFunctionAsync("async() =>{" + Req + "}").Result;
                Console.WriteLine(res.ToString());
                await WriteLog("Login response - " + res.ToString(), log);

                await ShowStatus("Login end", mytask);
                if (res.ToString().Contains("\"result\":true"))
                {
                    await ShowStatus("Successful login", mytask);
                    await WriteLog("Successful login", log);
                }
                else
                {
                    await ShowStatus("Error login", mytask);
                    await WriteLog("Error login", log);
                    throw new InvalidOperationException("Login error");
                }
                CheckStopedTask();
                //========================================================
                //========================================================
                //login request

                System.Threading.Thread.Sleep(1000);


                string link = dataGive.ProductLink;
                string cityId = "";
                string defShop = "";
                string CityMon = "";
                //========================================================
                //========================================================
                //get cityId
                Req = makeJS(headers, $"https://www.dns-shop.ru/ajax/region-nav-window/", "GET", "");
                Console.WriteLine(Req);
                res = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                Console.WriteLine(res.ToString());
                JObject joResponse = JObject.Parse(res.ToString());
                var parser = new HtmlParser();
                var document = parser.ParseDocument(joResponse["html"].ToString());

                string city = dataGive.CityToDns;
                foreach (IElement element in document.QuerySelector("ul[class='cities']").Children)
                {
                    Console.WriteLine(element.FirstElementChild.FirstElementChild.TextContent);
                    Console.WriteLine(element.FirstElementChild.GetAttribute("data-city-id"));
                    if (city == element.FirstElementChild.FirstElementChild.TextContent)
                    {
                        cityId = element.FirstElementChild.GetAttribute("data-city-id");
                        break;
                    }
                    Console.WriteLine("===");
                }

                await WriteLog("CityId got - " + cityId, log);
                await ShowStatus("CityId got", mytask);
                System.Threading.Thread.Sleep(2000);

                //get cityId
                //========================================================
                //========================================================


                //========================================================
                //========================================================
                //change city
                Req = makeJS(headers, $"https://www.dns-shop.ru/ajax/change-city/?city_guid={cityId}", "GET", "");
                Console.WriteLine(Req);
                res = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                Console.WriteLine(res.ToString());

                await WriteLog("City request response - " + res.ToString(), log);
                await ShowStatus("City selected", mytask);
                System.Threading.Thread.Sleep(2000);

                //change city
                //========================================================
                //========================================================

                //========================================================
                //========================================================
                //home request to get crsf token + cityId(2)
                headers.Clear();
                headers.Add("Accept", "application/json, text/plain, */*");
                headers.Add("Accept-Encoding", "gzip, deflate, br");
                headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                headers.Add("X-Requested-With", "XMLHttpRequest");
                headers.Add("Origin", "https://www.dns-shop.ru");
                headers.Add("Referer", "https://www.dns-shop.ru/no-referrer");
                headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");

                await WriteLog("Link request", log);
                await ShowStatus("Link request", mytask);

                Req = makeJS(headers, "https://www.dns-shop.ru/", "GET", "");  // Link request
                Console.WriteLine(Req);
                res = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                string RespLink = res.ToString();
                Console.WriteLine(RespLink);
                csrf0 = Regex.Match(RespLink, @"name=""csrf-token"" content=""(.*?)""").Groups[1].Value;
                CityMon = Regex.Match(RespLink, @"/\?cityId=(.*?)""").Groups[1].Value;
                Console.WriteLine(csrf0);
                Console.WriteLine(CityMon);

                //home request to get crsf token
                //========================================================
                //========================================================

                System.Threading.Thread.Sleep(1000);

                //========================================================
                //========================================================
                //link req to get poduct info
                await WriteLog("Getting data", log);
                Req = makeJSJson(headers, link, "GET", "");  // Link request
                Console.WriteLine(Req);
                res = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                string s = res.ToString().Replace("\\", "");
                Console.WriteLine(s);
                id = Regex.Match(s, @"class=""container product-card"" data-product-card=""(.*?)""").Groups[1].Value;
                title = Regex.Match(s, @"data-product-title>(.*?)<").Groups[1].Value;
                CheckStopedTask();
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine(id);
                Console.WriteLine(title);
                Console.WriteLine(csrf0);

                await WriteLog("id = " + id + "\n" + "title = " + title + "\n" + "csrf = " + csrf0, log);

                //link req to get poduct info
                //========================================================
                //========================================================

                //Preload
                //========================================================
                //========================================================

                if (dataGive.DnsPreload != 0)
                {
                    await WriteLog("Preload sleeping", log);
                    await ShowStatus("Preload sleeping", mytask);
                }
                System.Threading.Thread.Sleep(PreloadGeneration.Generate(dataGive.DnsPreload));


                if (dataGive.DnsModeToDns != 0)
                {
                    FarmCaptcha(token);
                    await ShowStatus("Farming task captcha", mytask);
                    System.Threading.Thread.Sleep(3000);
                }

                //========================================================
                //========================================================

                //========================================================
                //========================================================
                //monitoring
                if (dataGive.DnsModeToDns != 2)
                {
                    try
                    {
                        headers.Clear();
                        headers.Add("Accept", "application/json, text/plain, */*");
                        headers.Add("Accept-Encoding", "gzip, deflate, br");
                        headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                        headers.Add("charset", "UTF-8");
                        headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        headers.Add("Connection", "keep-alive");
                        headers.Add("Sec-Fetch-Dest", "empty");
                        headers.Add("X-Requested-With", "XMLHttpRequest");
                        headers.Add("X-CSRF-Token", csrf0);
                        headers.Add("Origin", "https://www.dns-shop.ru/");
                        headers.Add("Referer", "https://www.dns-shop.ru/");
                        headers.Add("Sec-Fetch-Mode", "cors");
                        headers.Add("Sec-Fetch-Site", "same-origin");
                        headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");

                        CheckStopedTask();
                        string body = $"data={{ \"type\": \"avails\", \"containers\": [{{ \"id\": \"{id}\", \"data\": {{ \"id\": \"{id}\", \"type\": 0, \"useNotInStock\": true }} }}] }}";

                        await WriteLog("Original monitoring", log);

                        await ShowStatus("Monitoring", mytask);
                        while (true)
                        {
                            Req = makeJS(headers, $"https://www.dns-shop.ru/ajax-state/avails/?cityId={CityMon}", "POST", body);  // \tТовара нет в наличии\t  доступен
                            Console.WriteLine(Req);
                            res = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                            Console.WriteLine(res.ToString());
                            CheckStopedTask();
                            if (res.ToString().Contains("Товара нет в наличии"))
                            {
                                Console.WriteLine("Not In Stock");
                                await WriteLog("Not In Stock", log);
                            }
                            else
                            {
                                Console.WriteLine("Find");
                                await WriteLog("Find item!!!", log);
                                break;
                            }
                            System.Threading.Thread.Sleep(300);
                        }
                    }
                    catch (Exception)
                    {
                        await ShowStatus("Monitoring error", mytask);
                        await WriteLog("Monitoring error", log);
                        throw new InvalidOperationException("Monitoring error");
                    }
                }

                //monitoring
                //========================================================
                //========================================================


                //========================================================
                //========================================================
                //go to checkout request (CHECKOUTING 1/2)
                headers.Clear();
                headers.Add("Accept", "application/json, text/plain, */*");
                headers.Add("Accept-Encoding", "gzip, deflate, br");
                headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                headers.Add("charset", "UTF-8");
                headers.Add("Content-Type", "application/x-www-form-urlencoded");
                headers.Add("Connection", "keep-alive");
                headers.Add("Sec-Fetch-Dest", "empty");
                headers.Add("X-Requested-With", "XMLHttpRequest");
                headers.Add("X-CSRF-Token", csrf0);
                headers.Add("Origin", "https://www.dns-shop.ru/");
                headers.Add("Referer", "https://www.dns-shop.ru/");
                headers.Add("Sec-Fetch-Mode", "cors");
                headers.Add("Sec-Fetch-Site", "same-origin");
                headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");
                CheckStopedTask();

                /* await ShowStatus("Checkouting (1/2)", mytask);
                  await WriteLog("Checkouting (1/2)", log);
                  Req = makeJS(headers, "https://www.dns-shop.ru/order/begin/go-to-checkout/?groupGuid=97cba11ad8fba2df257e7bf454a81295&separateParams=%7B%22isSeparated%22%3Afalse%2C%22hasDiscountLogistic%22%3Afalse%2C%22hasInstallment%22%3Afalse%7D&isUseDiscount=0", "POST", "");
                  Console.WriteLine(Req);
                  res = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                  Console.WriteLine(res.ToString());
                  System.Threading.Thread.Sleep(2000);*/
                //go to checkout request (CHECKOUTING 1/2)
                //========================================================
                //========================================================



                //========================================================
                //========================================================
                //go to checkout request to get tokens (CHECKOUTING 2/2)
                await ShowStatus("Checkouting", mytask);
                await WriteLog("Checkouting", log);
                Req = makeJS(headers, "https://www.dns-shop.ru/checkout/", "GET", "");  // "defaultShopId":"dc2a1ab0-8ba8-11eb-a242-00155de56609",
                Console.WriteLine(Req);
                res = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                string re_str = res.ToString();
                Console.WriteLine(res.ToString());
                if (dataGive.ShopIdToDns == "")
                {
                    defShop = Regex.Match(re_str, @"""defaultShopId"":""(.*?)""").Groups[1].Value;
                    Console.WriteLine("Default shop - " + defShop);
                }
                else
                {
                    defShop = dataGive.ShopIdToDns;
                    Console.WriteLine("Defined shop - " + defShop);
                }
                CheckStopedTask();
                if (defShop == "")
                {
                    await WriteLog("Shop not found ", log);
                }

                //      string shop_str = Regex.Match(re_str, @"class=""base-ui-link base-shop-view__address base-ui-link_blue"" target=""_self"" ispseudolink=""true"">(.*?)</a>").Groups[1].Value;

                await WriteLog("Checkout completed", log);

                process = Regex.Match(re_str, @"{""processId"":""(.*?)""").Groups[1].Value;
                csrf = Regex.Match(re_str, @"name=""csrf-token"" content=""(.*?)""").Groups[1].Value;

                Console.WriteLine("process: " + process);
                Console.WriteLine("csrf: " + csrf); // processing 
                Console.WriteLine("defShop: " + defShop); // processing 

                await WriteLog("Process - " + process + "\n" + "csrf - " + csrf + "\n" + "Shop - " + defShop, log);
                //go to checkout request (CHECKOUTING 2/2)
                //========================================================
                //========================================================

                System.Threading.Thread.Sleep(1000);
                CheckStopedTask();
                //========================================================
                //========================================================
                //processing
                try
                {
                    await WriteLog("Processing starting  " + defShop, log);
                    await ShowStatus("Processing...", mytask);

                    headers.Clear();
                    headers.Add("Accept", "*/*");
                    headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                    headers.Add("X-Requested-With", "XMLHttpRequest");
                    headers.Add("Origin", "https://www.dns-shop.ru");
                    headers.Add("X-CSRF-Token", csrf);
                    headers.Add("Referer", "https://www.dns-shop.ru/no-referrer");
                    headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Safari/537.36");

                    while (true)
                    {  // Proccessing loop
                        await WriteLog("Trying processing no captcha", log);
                        await ShowStatus("Processing", mytask);
                        Console.WriteLine("Trying processing");
                        body_fields.Clear();
                        //body_fields.Add("X-CSRF-Token", csrf);
                        body_fields.Add("ProductForm[0][id]", id);
                        body_fields.Add("ProductForm[0][count]", "1");
                        body_fields.Add("ProductForm[0][price]", "40999");
                        body_fields.Add("ProductForm[0][bundleId]", "");
                        body_fields.Add("ProductForm[0][bundleGroupId]", "0");
                        body_fields.Add("ProductForm[0][bonus]", "0");
                        body_fields.Add("ProcessingForm[processId]", process);
                        body_fields.Add("ProcessingForm[initCityId]", cityId);// city
                        body_fields.Add("ProcessingForm[phone]", words[2]);// phone
                        body_fields.Add("ProcessingForm[email]", words[0]);
                        body_fields.Add("ProcessingForm[appeal]", "");
                        body_fields.Add("ProcessingForm[shopId]", defShop);//shop
                        body_fields.Add("ProcessingForm[paymentMethodId]", "01083b5b-4661-4fd3-8f45-77ed33c17bf2");
                        body_fields.Add("ProcessingForm[sendCheckPhoneOrEmail]", "0");
                        body_fields.Add("ProcessingForm[useDl]", "0");
                        body_fields.Add("ProcessingForm[useInstantDiscount]", "0");
                        body_fields.Add("ProcessingForm[useDiscountForOnlinePayment]", "0");
                        body_fields.Add("ProcessingForm[discountTypeVisa]", "0");
                        body_fields.Add("ProcessingForm[paymentBonusActivated]", "0");
                        body_fields.Add("ProcessingForm[useBonusCount]", "0");
                        body_fields.Add("ProcessingForm[needPrepay]", "1");
                        body_fields.Add("ProcessingForm[isOperatorAsClient]", "0");
                        body_fields.Add("ProcessingForm[ignoreErrorCode]", "");
                        body_fields.Add("ProcessingForm[captchaToken]", "");
                        body_fields.Add("ProcessingForm[pzpMemberIsNew]", "0");
                        body_fields.Add("ProcessingForm[pzpMemberGenderMale]", "");
                        body_fields.Add("ProcessingForm[pzpMemberBirthDate]", "");
                        body_fields.Add("bundle", "undefined");

                        Req = createJSForm(headers, "https://www.dns-shop.ru/checkout/checkout/processing/", "POST", body_fields);
                        Console.WriteLine(Req);
                        res = page.EvaluateFunctionAsync("async() =>{" + Req + "}").Result;
                        Console.WriteLine(res.ToString());
                        if (res.ToString().Contains("\"result\":true"))
                        {
                            await WriteLog("Good processing - " + res.ToString(), log);
                            break;
                        }
                        else
                        {
                            await WriteLog("Bad processing - " + res.ToString(), log);
                            await WriteLog("Rerying processing bank captcha", log);
                            await ShowStatus("Rerying processing", mytask);
                            string CapTok = GetRandomCapthaFromBank(dataGive.DnsModeToDns);

                            if (CapTok == "")
                            {
                                CapTok = CaptchaStorage.TaskCapthca();
                            }

                            string initForm = CapTok[200].ToString() + CapTok[150].ToString() + CapTok[256].ToString();
                            initForm = initForm.ToLower();
                            Console.WriteLine(initForm);
                            Console.WriteLine(Regex.Replace(process, @"[^a-z]+", ""));
                            await WriteLog("init  = " + initForm, log);
                            body_fields.Clear();
                            //  body_fields.Add("X-CSRF-Token", csrf);
                            body_fields.Add("ProductForm[0][id]", id);
                            body_fields.Add("ProductForm[0][count]", "1");
                            body_fields.Add("ProductForm[0][price]", "40999");
                            body_fields.Add("ProductForm[0][bundleId]", "");
                            body_fields.Add("ProductForm[0][bundleGroupId]", "0");
                            body_fields.Add("ProductForm[0][bonus]", "0");
                            body_fields.Add("ProcessingForm[processId]", process);
                            body_fields.Add("ProcessingForm[initCityId]", cityId);// city
                            body_fields.Add("ProcessingForm[phone]", words[2]);// phone
                            body_fields.Add("ProcessingForm[email]", words[0]);
                            body_fields.Add("ProcessingForm[appeal]", "");//name
                            body_fields.Add("ProcessingForm[shopId]", defShop);//shop
                            body_fields.Add("ProcessingForm[paymentMethodId]", "01083b5b-4661-4fd3-8f45-77ed33c17bf2");
                            body_fields.Add("ProcessingForm[sendCheckPhoneOrEmail]", "0");
                            body_fields.Add("ProcessingForm[useDl]", "0");
                            body_fields.Add("ProcessingForm[useInstantDiscount]", "0");
                            body_fields.Add("ProcessingForm[useDiscountForOnlinePayment]", "0");
                            body_fields.Add("ProcessingForm[discountTypeVisa]", "0");
                            body_fields.Add("ProcessingForm[paymentBonusActivated]", "0");
                            body_fields.Add("ProcessingForm[useBonusCount]", "0");
                            body_fields.Add("ProcessingForm[needPrepay]", "1");
                            body_fields.Add("ProcessingForm[isOperatorAsClient]", "0");
                            body_fields.Add("ProcessingForm[ignoreErrorCode]", "666");
                            body_fields.Add("ProcessingForm[captchaToken]", CapTok);
                            body_fields.Add("ProcessingForm[initO" + initForm + "]", Regex.Replace(process, @"[^a-z]+", ""));
                            body_fields.Add("ProcessingForm[pzpMemberIsNew]", "0");
                            body_fields.Add("ProcessingForm[pzpMemberGenderMale]", "");
                            body_fields.Add("ProcessingForm[pzpMemberBirthDate]", "");
                            body_fields.Add("bundle", "undefined");

                            Req = createJSForm(headers, "https://www.dns-shop.ru/checkout/checkout/processing/", "POST", body_fields);
                            Console.WriteLine(Req);
                            res = page.EvaluateFunctionAsync("async() =>{" + Req + "}").Result;
                            Console.WriteLine(res.ToString());
                            if (res.ToString().Contains("\"result\":true"))
                            {
                                await WriteLog("Good processing - " + res.ToString(), log);
                                break;
                            }


                        }
                    }
                }
                catch (Exception)
                {
                    await ShowStatus("Processing error", mytask);
                    await WriteLog("Processing error", log);
                    throw new InvalidOperationException("Processing error");
                }
                //processing
                //========================================================
                //========================================================
                System.Threading.Thread.Sleep(5000);
                CheckStopedTask();
                //========================================================
                //========================================================
                //after processing + yoomoney + webhook POST (getting payment link)
                try
                {
                    await ShowStatus("Redirecting", mytask);
                    await WriteLog("Redirecting", log);
                    Req = makeJS(headers, "https://www.dns-shop.ru/checkout/checkout/after-processing-redirect/", "GET", ""); // after proccessing 
                    Console.WriteLine(Req);
                    var resAfeterProc = page.EvaluateFunctionAsync("async() =>" + Req).Result;
                    string resp_str = resAfeterProc.ToString();
                    Console.WriteLine(resAfeterProc.ToString());
                    System.Threading.Thread.Sleep(2000);

                    await ShowStatus("Getting payment link", mytask);
                    await WriteLog("Getting payment link", log);

                    request
                    .AddParam("_csrf", Regex.Match(resp_str.ToString(), @"name=""_csrf"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("scid", Regex.Match(resp_str.ToString(), @"name=""scid"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("shopId", Regex.Match(resp_str.ToString(), @"name=""shopId"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("paymentType", Regex.Match(resp_str.ToString(), @"name=""paymentType"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("orderNumber", Regex.Match(resp_str.ToString(), @"name=""orderNumber"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("customerNumber", Regex.Match(resp_str.ToString(), @"name=""customerNumber"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("sum", Regex.Match(resp_str.ToString(), @"name=""sum"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("cps_phone", Regex.Match(resp_str.ToString(), @"name=""cps_phone"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("shopSuccessURL", Regex.Match(resp_str.ToString(), @"name=""shopSuccessURL"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("shopFailURL", Regex.Match(resp_str.ToString(), @"name=""shopFailURL"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("shopDefaultUrl", Regex.Match(resp_str.ToString(), @"name=""shopDefaultUrl"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("actionUrl", Regex.Match(resp_str.ToString(), @"name=""actionUrl"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("transactionGuid", Regex.Match(resp_str.ToString(), @"name=""transactionGuid"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("order_guid", Regex.Match(resp_str.ToString(), @"name=""order_guid"" value=""(.*?)""").Groups[1].Value)
                    .AddParam("branch_guid", Regex.Match(resp_str.ToString(), @"name=""branch_guid"" value=""(.*?)""").Groups[1].Value);
                    request.Post("https://yoomoney.ru/eshop.xml"); // payment request
                    Console.WriteLine("Posting payment");
                    HttpResponse re = request.Response;

                    Console.WriteLine(re.StatusCode);
                    resp_str = re.ToString();

                    string payment = Regex.Match(resp_str, @"retpath"":""(.*?)""").Groups[1].Value;
                    Console.WriteLine("---------");
                    string pathApi = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/discord.hook";  // discord hook
                    string api = System.IO.File.ReadAllText(pathApi);


                    string json = JsonConvert.SerializeObject(new
                    {
                        username = "STARDOM",
                        avatar_url = "https://postila.ru/data/2a/8f/b9/b7/2a8fb9b743e7cbb656006965245a64c42ec571aaf2edd010540a3fb42b54bca8.png",
                        embeds = new[]
                                {
                               new {
                                    title = title,
                                    color = "65280",
                                    thumbnail = new
                                        {
                                            url = "https://postila.ru/data/2a/8f/b9/b7/2a8fb9b743e7cbb656006965245a64c42ec571aaf2edd010540a3fb42b54bca8.png"
                                        },
                                    fields = new[]
                                    {
                                        new
                                        {
                                            name = "Account",
                                            value = "||" + words[0] + "||"
                                        },
                                        new
                                        {
                                            name = "City",
                                            value = "||" + dataGive.CityToDns + "||"
                                        },
                                        new
                                        {
                                            name = "Link",
                                            value = "[Payment link](" + payment + ")"
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
                    var wr_2 = WebRequest.Create("https://discordapp.com/api/webhooks/757679702650519632/6SyMIC68oq9Bj_bW5wYlib4KeHq0ECZ-KZZK6UCZfz7AILtgBul-i0JZGWCfSjeeYniF");
                    wr_2.ContentType = "application/json";
                    wr_2.Method = "POST";
                    using (var sw = new StreamWriter(wr.GetRequestStream()))
                        sw.Write(json);
                    wr_2.GetResponse();

                    await ShowStatus("Check discord", mytask);
                    await WriteLog("Check discord", log);
                    CheckStopedTask();
                }
                catch (Exception)
                {
                    await ShowStatus("Payment error", mytask);
                    await WriteLog("Payment error", log);
                    throw new InvalidOperationException("Payment error");
                }
                //after processing + yoomoney + webhook POST (getting payment link)
                //========================================================
                //========================================================
            }
            catch (Exception ex)
            {
                headers.Clear();
                body_fields.Clear();
                dataGive.DnsPreload = 0;
                if (token.IsCancellationRequested)
                {
                    await ShowStatus("Task stoped", mytask);
                    await WriteLog("Task stoped", log);

                }
                else
                {
                    await ShowStatus("Task failed", mytask);
                    await WriteLog("Exeption -  " + ex.Message, log);
                    await ShowStatus("Retry", mytask);
                    await WriteLog("Retry", log);
                    await browsers[i].CloseAsync();
                    System.Threading.Thread.Sleep(3000);
                    Src(i, mytask, dataGive, log, token);
                }
            }



        }


    }
}