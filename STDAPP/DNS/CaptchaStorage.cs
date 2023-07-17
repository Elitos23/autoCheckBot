using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using xNet;

namespace STDAPP
{
    class CaptchaStorage
    {
        public static List<string> CaptchaBankReady = new List<string>();
        public static bool Continue = true;
        public static string pathCapMonster = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/CapMonster.api";
        public async static void DoCapthaAndAdd()
        {
            try
            {

                while (Continue)
                {
                    Console.WriteLine("Make Captcha");

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

                    var jsondata_2 = new
                    {
                        clientKey = API_KEY,
                        task = new
                        {
                            type = "NoCaptchaTaskProxyless",
                            websiteURL = "https://www.dns-shop.ru/checkout/",
                            websiteKey = "6LdJlQ4TAAAAAApDn0TPJA28DfOgtv6AGtY9EAfo"
                        }
                    };
                    json_data = JsonConvert.SerializeObject(jsondata_2);
                    response = request.Post("https://api.capmonster.cloud/createTask", json_data, "application/json");
                    joResponse = JObject.Parse(response.ToString());
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
                        response = request.Post("https://api.capmonster.cloud/getTaskResult/", json_data, "application/json");
                        joResponse = JObject.Parse(response.ToString());
                        Console.WriteLine(joResponse);
                        if (joResponse["status"].ToString() == "ready")
                        {
                            captchaResp = joResponse["solution"]["gRecaptchaResponse"].ToString();
                            break;
                        }
                        await Task.Delay(50);
                    }

                    CaptchaStorage.CaptchaBankReady.Add(captchaResp);

                }
            }
            catch
            {

            }
        }


        public static string TaskCapthca()
        {
            try
            {
                Console.WriteLine("Make Captcha");

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

                var jsondata_2 = new
                {
                    clientKey = API_KEY,
                    task = new
                    {
                        type = "NoCaptchaTaskProxyless",
                        websiteURL = "https://www.dns-shop.ru/checkout/",
                        websiteKey = "6LdJlQ4TAAAAAApDn0TPJA28DfOgtv6AGtY9EAfo"
                    }
                };
                json_data = JsonConvert.SerializeObject(jsondata_2);
                response = request.Post("https://api.capmonster.cloud/createTask", json_data, "application/json");
                joResponse = JObject.Parse(response.ToString());
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
                    response = request.Post("https://api.capmonster.cloud/getTaskResult/", json_data, "application/json");
                    joResponse = JObject.Parse(response.ToString());
                    Console.WriteLine(joResponse);
                    if (joResponse["status"].ToString() == "ready")
                    {
                        captchaResp = joResponse["solution"]["gRecaptchaResponse"].ToString();
                        break;
                    }
                    System.Threading.Thread.Sleep(50);
                }

                return captchaResp;
            }
            catch
            {
                return "Error";
            }
        }

    }
}