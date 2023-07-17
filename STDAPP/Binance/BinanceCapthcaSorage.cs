using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STDAPP
{
    class BinanceCapthcaSorage
    {
        private static string pathCapMonster = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/CapMonster.api";
        public static string TaskCapthca(string ProductID)
        {
            try
            {
                Console.WriteLine("Make Captcha");

                var request = new HttpRequest();

                string API_KEY = "9ca9c402f22d2e2947e30ad76323fab3";
                var jsondata = new
                {
                    clientKey = API_KEY
                };

                string json_data = JsonConvert.SerializeObject(jsondata);
                HttpResponse response = request.Post("https://api.capmonster.cloud/getBalance", json_data, "application/json");
                JObject joResponse = JObject.Parse(response.ToString());
                Debug.WriteLine(joResponse);

                var jsondata_2 = new
                {
                    clientKey = API_KEY,
                    task = new

                    {
                        type = "RecaptchaV3TaskProxyless",
                        websiteURL = $"https://www.binance.com/ru/nft/blindBox/detail?productId={ProductID}",
                        websiteKey = "6LeUPckbAAAAAIX0YxfqgiXvD3EOXSeuq0OpO8u_",
                        minScore = 0.3,
                        pageAction = "submit"
                    }

                };
                json_data = JsonConvert.SerializeObject(jsondata_2);
                response = request.Post("https://api.capmonster.cloud/createTask", json_data, "application/json");
                joResponse = JObject.Parse(response.ToString());
                Debug.WriteLine(joResponse);
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
                    Debug.WriteLine(joResponse);
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