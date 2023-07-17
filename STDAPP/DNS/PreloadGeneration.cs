using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STDAPP
{
    class PreloadGeneration
    {

        public static int Generate(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0: return 0;
                case 1: return 120000;
                case 2: return 180000;
                case 3: return 240000;
                case 4: return 300000;
                case 5: return 360000;
                case 6: return 420000;
                case 7: return 480000;
                case 8: return 540000;
                case 9: return 600000;
                case 10: return 660000;
                case 11: return 720000;
                case 12: return 780000;
                case 13: return 840000;
                case 14: return 900000;
                case 15: return 960000;
                default: return 0;
            }
        }
    }
}