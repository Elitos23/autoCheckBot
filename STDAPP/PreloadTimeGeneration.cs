using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STDAPP
{
    public static class PreloadTimeGeneration
    {

        public static string GeneratePreload(string val)
        {
            switch (val)
            {
                case "5 minutes before":
                    return "12:55:00";
                case "5:30 minutes before":
                    return "12:54:30";
                case "6 minutes before":
                    return "12:54:00";
                case "6:30 minutes before":
                    return "12:53:30";
                case "7 minutes before":
                    return "12:53:00";
                case "7:30 minutes before":
                    return "12:52:30";
                case "8 minutes before":
                    return "12:52:00";
                case "8:30 minutes before":
                    return "12:51:30";
                case "9 minutes before":
                    return "12:51:00";
                case "9:30 minutes before":
                    return "12:50:30";
                case "10 minutes before":
                    return "12:50:00";
                case "10:30 minutes before":
                    return "12:49:30";
                case "11 minutes before":
                    return "12:49:00";
                case "11:30 minutes before":
                    return "12:48:30";
                case "12 minutes before":
                    return "12:48:00";
                case "12:30 minutes before":
                    return "12:47:30";
                case "13 minutes before":
                    return "12:47:00";
                case "13:30 minutes before":
                    return "12:46:30";
                case "14 minutes before":
                    return "12:46:00";
                case "14:30 minutes before":
                    return "12:45:30";
                case "15 minutes before":
                    return "12:45:00";
                default:
                    return "ERROR";
            }

        }

    }

}

