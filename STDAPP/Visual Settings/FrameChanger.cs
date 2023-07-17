using System.Windows;
using System.Windows.Controls;

namespace STDAPP
{
    public static class FrameChanger
    {
        public static Frame MainFrame;
        public static ProfileEditorPage EditorPage = new ProfileEditorPage();
        public static AutoregPage Autoreg = new AutoregPage();
        public static EldoradoRaflPage Eldorado = new EldoradoRaflPage();
        public static Page SourcePage;
       
        public static void SetSorcePage(SourcePage Source)
        {
            SourcePage = Source;
        }

        public static void SetEditorToFrame()
        {
            if (ThemeCondition.ThemeCon)
            {
                EditorPage.pic.Visibility = Visibility.Visible;
            }
            else
            {
                EditorPage.pic.Visibility = Visibility.Hidden;
            }
            MainFrame.Content = EditorPage;
        }

        public static void SetSourceToFrame()
        {
            MainFrame.Content = SourcePage;
        }
        public static void SetAutoregToFrame()
        {
            if (ThemeCondition.ThemeCon)
            {
                Autoreg.pic.Visibility = Visibility.Visible;
            }
            else
            {
                Autoreg.pic.Visibility = Visibility.Hidden;
            }
            MainFrame.Content = Autoreg;
        }


        public static void SetEldoradoToFrame()
        {
            if (ThemeCondition.ThemeCon)
            {
                Eldorado.pic.Visibility = Visibility.Visible;
            }
            else
            {
                Eldorado.pic.Visibility = Visibility.Hidden;
            }
            MainFrame.Content = Eldorado;
        }
    }
}
