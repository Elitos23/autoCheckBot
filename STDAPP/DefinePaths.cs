using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STDAPP
{
    class DefinePaths
    {
        public static string AccountsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/ACCS/"; // appdata акаунты 
        public static string ProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/PROF/";// appdata профиля
        public static string ProxysPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/PROX/";// appdata прокси
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/.local-chromium/win64-869685/chrome-win/chrome.exe"; // executable chromium
        public static string pathProf = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/profiles/"; // Chrome profiles
        public static string NodePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/Node/"; // Node for JS scripts
        public static string scriptPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/data/popa.js"; // Executable Editor script 
        public static string scriptPathAutoreg = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Stardom/data/autoreg.js";// Executable Autoreg script 
        public static string pathCapMonster = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/CapMonster.api";//Capmonster api key path 
        public static string TasksPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/Tasks.xml";//Capmonster api key path 
    }
}
 