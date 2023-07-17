using Leaf.xNet;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace STDAPP
{
    class InitializeUserData
    {
        public static string DiscordName { get; set; }
        public static string Key { get; set; }
        public static string RenewDate { get; set; }
        public static string DiscordAvatar { get; set; }

        public static bool GetUsersData()
        {
            try
            {
                string dbkey = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/dbkey.txt");
                Console.WriteLine(dbkey);
                DB db = new DB();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT * FROM `botkeys2` WHERE `botkey` = @uL", db.getConnection());
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = dbkey;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine(reader[4]);
                    string discordId = reader[4].ToString();
                    RenewDate = reader[5].ToString();
                    if (RenewDate == "") RenewDate = "-";

                    Key = dbkey;
                    HttpRequest request = new HttpRequest();
                    request.AddHeader("Authorization", "Bot NzQ1MjU0NDgyNDcyMDc1MzY2.XzvGUg.NtTbKhYbpMAUWvAR6qNAqvSz7ag");
                    HttpResponse resp = request.Get($"https://discord.com/api/v8/users/{discordId}");
                    JObject joResponse = JObject.Parse(resp.ToString());
                    DiscordName = $"{joResponse["username"]}#{joResponse["discriminator"]}";
                    DiscordAvatar = $"https://cdn.discordapp.com/avatars/{discordId}/{joResponse["avatar"]}";
                    Console.WriteLine(DiscordName);
                    Console.WriteLine(DiscordAvatar);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }
    }
}