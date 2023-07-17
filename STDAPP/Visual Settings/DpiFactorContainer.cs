using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace STDAPP
{
    public static class DpiFactorContainer
    {
        public const double DPI = 1.25;

        public static void Resize(Grid MainGrid)
        {
            double dpiFactor = DpiFactorContainer.DPI;
            Console.WriteLine(dpiFactor);
            double ScaleCount;
            if (dpiFactor == 1.25)
            {
                ScaleCount = 0.90;
            }
            else
            {
                ScaleCount = 1;
            }
            MainGrid.LayoutTransform = new ScaleTransform(ScaleCount, ScaleCount, 0, 0);
        }


    }
}
