using System.Windows;
using System.Windows.Input;


namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        private void Close_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private void Close_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void MainGridMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
