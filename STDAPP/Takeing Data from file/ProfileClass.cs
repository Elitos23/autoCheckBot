using System;
using System.IO;


namespace STDAPP
{
    public class ProfileClass
    {
       
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }

        public string Adress2 { get; set; }
        public string City { get; set; }
        public string Index { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string CardNumber { get; set; }
        public string MMYY { get; set; }
        public string CVC { get; set; }


        private static string ProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/PROF/";// appdata профиля
        public  static ProfileClass GetProfileInfo(string FileName)
        {
            StreamReader sr = new StreamReader(ProfilePath + FileName + ".txt");
            //Read all text
            string Content = sr.ReadToEnd();
            string[] ProfileData = Content.Split('\n');
            ProfileClass profile = new ProfileClass();
            profile.Name = ProfileData[0];
            profile.SecondName = ProfileData[1];
            profile.LastName = ProfileData[2];
            profile.Adress = ProfileData[3];
            profile.Adress2 = ProfileData[4];
            profile.City = ProfileData[5];
            profile.Index = ProfileData[6];
            profile.Phone = ProfileData[7];
            profile.Email = ProfileData[8];
            profile.CardNumber = ProfileData[9];
            profile.CVC = ProfileData[10];
            profile.MMYY = ProfileData[11];
            return profile;
        }
    }
}
