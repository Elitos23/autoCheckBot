using System.Threading.Tasks;
using System.Windows.Controls;

namespace STDAPP
{
    public  class Messages
    {
        public const string NeedMoreAccounts = "Аккаунтов меньше создаваемых тасков";
        public const string NullProfileOrAccount = "Нужно выбрать профиль или аккаунты таска";
        public const string NullDropTime = "Нужно ввести DropTime";
        public const string NullPreloadTime = "Нужно ввести PreloadTime";
        public const string NullNumberOfTasks = "Нужно ввести количество тасков";
        public const string NullLink = "Нужно ввести ссылку на продукт";
        public const string NullSize = "Нужно выбрать размер";
        public const string NullProxyChoose = "Нужно выбрать прокси";
        public const string NeedMoreProxy = "Прокси меньше создаваемых тасков";
        public static async void ShowAlertMessage(string message,StackPanel AlertStack)
        {
            AlertPage alert = new AlertPage();
            alert.LabelAlertMessage.Content = message;
            Frame FrameForAlert = new Frame();
            FrameForAlert.Content = alert;
            AlertStack.Children.Add(FrameForAlert);
            await Task.Delay(3000);
            AlertStack.Children.Remove(FrameForAlert);

        }


        public static async void ShowAlertMessageSettings(string message, StackPanel AlertStack)
        {
            AlertPageSettings alert = new AlertPageSettings();
            alert.LabelAlertMessage.Content = message;
            Frame FrameForAlert = new Frame();
            FrameForAlert.Content = alert;
            AlertStack.Children.Add(FrameForAlert);
            await Task.Delay(3000);
            AlertStack.Children.Remove(FrameForAlert);

        }

        public static async void ShowAlertMessageProfile(string message, StackPanel AlertStack)
        {
            AlertPageProfile alert = new AlertPageProfile();
            alert.LabelAlertMessage.Content = message;
            Frame FrameForAlert = new Frame();
            FrameForAlert.Content = alert;
            AlertStack.Children.Add(FrameForAlert);
            await Task.Delay(3000);
            AlertStack.Children.Remove(FrameForAlert);
        }

        public static async void ShowAlertMessageAccountsProxy(string message, StackPanel AlertStack)
        {
            AccountsProxyAlertPage alert = new AccountsProxyAlertPage();
            alert.LabelAlertMessage.Content = message;
            Frame FrameForAlert = new Frame();
            FrameForAlert.Content = alert;
            AlertStack.Children.Add(FrameForAlert);
            await Task.Delay(3000);
            AlertStack.Children.Remove(FrameForAlert);
        }
    }
}
