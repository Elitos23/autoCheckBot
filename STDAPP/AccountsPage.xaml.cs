using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для AccountsPage.xaml
    /// </summary>
    public partial class AccountsPage : Page
    {
        private string AccountsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/ACCS/";
        private string ProxysPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/PROX/";
        public AccountsPage()
        {
            InitializeComponent();
            DpiFactorContainer.Resize(AccountGrid);
        }

        public void SavingFileForAccounts(string Name, string Content)
        {
            if (Name != "")
            {

                try
                {
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(AccountsPath + Name + ".txt");
                    //Write a line of text
                    sw.WriteLine(Content);
                    //Close the file
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
            }
            else
            {
                Messages.ShowAlertMessageAccountsProxy("Give a name to your accounts pool", AlertStack);
                return;
            }

        }
        public void SavingFileForProxys(string Name, string Content)
        {
            if (Name != "")
            {


                try
                {
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(ProxysPath + Name + ".txt");
                    //Write a line of text
                    sw.WriteLine(Content);
                    //Close the file
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
            }
            else
            {
                Messages.ShowAlertMessageAccountsProxy("Give a name to your proxy pool", AlertStack);
                return;
            }

        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string Name = AccountName.Text;
            string Content = AccountsList.Text;
            SavingFileForAccounts(Name, Content);

            if (Name != "")
            {
                Messages.ShowAlertMessageAccountsProxy("Accounts - " + Name + " saved", AlertStack);
            }

        }

        private void CBLoadAccount_DropDownOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OPENED");
            try
            {
                if (Directory.Exists(AccountsPath))
                {

                    Console.WriteLine("ACCOUNTS: ");
                    string[] files = Directory.GetFiles(AccountsPath);
                    foreach (string s in files)
                    {
                        Console.WriteLine(s);
                        string NameToAddInCb = Regex.Match(s, @"ACCS/(.*?).txt").Groups[1].Value;
                        Console.WriteLine("Adding - " + NameToAddInCb);
                        if (CBLoadAccount.Items.Contains(NameToAddInCb))
                        {
                            Console.WriteLine("Already exists");
                        }
                        else
                        {
                            CBLoadAccount.Items.Add(NameToAddInCb);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

            }
        }

        private void CBLoadAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(CBLoadAccount.SelectedItem);
            string FileName = "";
            if (CBLoadAccount.SelectedItem != null)
            {
                FileName = CBLoadAccount.SelectedItem.ToString();
                AccountsList.Text = CBLoadAccount.Text;
                AccountName.Text = FileName;
            }
            else
            {
                return;
            }

            try
            {
                string Content;
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(AccountsPath + FileName + ".txt");
                //Read all text
                Content = sr.ReadToEnd();
                Console.WriteLine(Content);
                AccountsList.Text = Content;           
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        private void CBLoadProxy_DropDownOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OPENED");
            try
            {
                if (Directory.Exists(ProxysPath))
                {

                    Console.WriteLine("PROXYS: ");
                    string[] files = Directory.GetFiles(ProxysPath);
                    foreach (string s in files)
                    {
                        Console.WriteLine(s);
                        string NameToAddInCb = Regex.Match(s, @"PROX/(.*?).txt").Groups[1].Value;
                        Console.WriteLine("Adding - " + NameToAddInCb);
                        if (CBLoadProxy.Items.Contains(NameToAddInCb))
                        {
                            Console.WriteLine("Already exists");
                        }
                        else
                        {
                            CBLoadProxy.Items.Add(NameToAddInCb);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

            }

        }

        private void CBLoadProxy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(CBLoadProxy.SelectedItem);
            string FileName = "";
            if (CBLoadProxy.SelectedItem != null)
            {
                FileName = CBLoadProxy.SelectedItem.ToString();
                ProxyName.Text = FileName;
            }
            else
            {
                return;
            }

            try
            {
                string Content;
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(ProxysPath + FileName + ".txt");
                //Read all text
                Content = sr.ReadToEnd();
                Console.WriteLine(Content);
                ProxyList.Text = Content;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        private void ButtonSaveProxy_Click(object sender, RoutedEventArgs e)
        {
            string Name = ProxyName.Text;
            string Content = ProxyList.Text;
            SavingFileForProxys(Name, Content);
            if (Name != "")
            {
                Messages.ShowAlertMessageAccountsProxy("Proxy - " + Name + " saved", AlertStack);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e) // delete accounts
        {
            try
            {
                Console.WriteLine("Deleting ");
                string Name = CBLoadAccount.Text;
                string del = AccountsPath + CBLoadAccount.Text + ".txt";
                System.IO.File.Delete(del);
                CBLoadAccount.Items.Remove(CBLoadAccount.Text);
                AccountsList.Clear();
                Messages.ShowAlertMessageAccountsProxy("Accounts - " + Name + " deleted", AlertStack);
              
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Messages.ShowAlertMessageAccountsProxy("Deleting error", AlertStack);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // delete proxy
        {
            try
            {
                Console.WriteLine("Deleting ");
                string Name = CBLoadProxy.Text;
                string del = ProxysPath + CBLoadProxy.Text + ".txt";
                System.IO.File.Delete(del);
                CBLoadProxy.Items.Remove(CBLoadProxy.Text);
                ProxyList.Clear();
                Messages.ShowAlertMessageAccountsProxy("Proxy - " + Name + " deleted", AlertStack);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Messages.ShowAlertMessageAccountsProxy("Deleting error", AlertStack);
            }
        }
    }
}
