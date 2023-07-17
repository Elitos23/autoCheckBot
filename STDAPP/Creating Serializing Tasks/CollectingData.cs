using System;
using System.Collections.Generic;
using System.IO;

namespace STDAPP
{
    class CollectingData
    {
        delegate void AnyMode(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator);

        private static AnyMode[] ModsArray =
        {
            new AnyMode(SignatureData.SetSnkrsData),
            new AnyMode(SignatureData.SetDnsData),
            new AnyMode(SignatureData.SetLamodaData),
            new AnyMode(SignatureData.SetBinanceData),
            new AnyMode(SignatureData.SetCitilinkData)
        };
        public static void SetDataByMode(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator)
        {
            ModsArray[DataSource.Shop.SelectedIndex](DataToTask, DataSource, mytask, Indicator);
        }

    }

    class SignatureData
    {
        private static List<string> AccountBlackList = new List<string>(); // список заюзаных аков которые надо будет пропускать
        private static void CheckNullTextBox(string message, SourcePage DataSource, string TextBoxText)
        {
            if (TextBoxText == "")
            {
                Messages.ShowAlertMessage(message, DataSource.AlertStack);
                throw new InvalidOperationException("Null field");
            }
        }
        private static void SetAccountData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, List<string> AccountBlackList, string Indicator, TaskPage mytask)
        {
            if (DataSource.Accounts.SelectedItem == null)
            {
                Messages.ShowAlertMessage(Messages.NullProfileOrAccount, DataSource.AlertStack);
                throw new InvalidOperationException("Null field");
            }

            if (Indicator == "Edit")
            {
                if (DataSource.AccountsChoose.SelectedIndex == 0)
                {
                    DataToTask.Account = mytask.Account.Content.ToString();
                }
                else
                {
                    DataToTask.Account = DataSource.AccountsChoose.SelectedItem.ToString();
                }
                return;
            }

            if ((DataSource.AccountsChoose.SelectedIndex != 0))
            {
                DataToTask.Account = DataSource.AccountsChoose.SelectedItem.ToString();
                AccountBlackList.Add(DataSource.AccountsChoose.SelectedItem.ToString());
                return;
            }

            int AccIndex = AccountData.GiveAccountIndexSrc(DataSource.Accounts.SelectedItem.ToString());
            DataToTask.Account = AccountData.GiveMeAccount(AccIndex, DataSource.Accounts.SelectedItem.ToString());
            while (AccountBlackList.Contains(DataToTask.Account))
            {
                AccIndex = AccountData.GiveAccountIndexSrc(DataSource.Accounts.SelectedItem.ToString());
                DataToTask.Account = AccountData.GiveMeAccount(AccIndex, DataSource.Accounts.SelectedItem.ToString());
            }

            Console.WriteLine(AccountData.GiveMeAccount(AccIndex, DataSource.Accounts.SelectedItem.ToString()));
            if (AccountData.GiveMeAccount(AccIndex, DataSource.Accounts.SelectedItem.ToString()) == "ERROR")
            {
                Messages.ShowAlertMessage(Messages.NeedMoreAccounts, DataSource.AlertStack);
                throw new InvalidOperationException("Null field");
            };


        }

        private static void SetProfileData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource)
        {
            if (DataSource.Profile.SelectedItem == null)
            {
                Messages.ShowAlertMessage(Messages.NullProfileOrAccount, DataSource.AlertStack);
                throw new InvalidOperationException("Null field"); // Пустое поле 
            }

            DataToTask.Prof = ProfileClass.GetProfileInfo(DataSource.Profile.SelectedItem.ToString());
        }

        private static void SetProxyData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator)
        {
            if (DataSource.Proxy.SelectedIndex == 0)
                return;

            if (Indicator == "Edit")
            {
                if (DataSource.ProxyChoose.SelectedIndex == 0)
                {
                    DataToTask.Proxy = mytask.Proxy.Content.ToString();
                }
                else
                {
                    DataToTask.Proxy = DataSource.ProxyChoose.SelectedItem.ToString();
                }
                return;
            }

            if (DataSource.ProxyChoose.SelectedIndex != 0)
            {
                DataToTask.Proxy = DataSource.ProxyChoose.Text;
                return;
            }

            int ProxyIndex = AccountData.GiveProxyIndexSrc(DataSource.Proxy.SelectedItem.ToString());
            DataToTask.Proxy = AccountData.GiveMeProxy(ProxyIndex, DataSource.Proxy.SelectedItem.ToString());
            if (AccountData.GiveMeProxy(ProxyIndex, DataSource.Proxy.SelectedItem.ToString()) == "ERROR")
            {
                Messages.ShowAlertMessage(Messages.NeedMoreProxy, DataSource.AlertStack); // другой алерт 
                throw new InvalidOperationException("Null field");
            };

        }

        public static void SetSnkrsData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator)
        {
            Console.WriteLine("Set Snkrs Data");
            CheckNullTextBox(Messages.NullLink, DataSource, DataSource.link.Text);
            CheckNullTextBox(Messages.NullSize, DataSource, DataSource.Size.txtContent.Text);
            SetAccountData(DataToTask, DataSource, AccountBlackList, Indicator, mytask);
            SetProfileData(DataToTask, DataSource);
            SetProxyData(DataToTask, DataSource, mytask, Indicator);

            if ((DataSource.DropTimeHou.Text == "" || DataSource.DropTimeMin.Text == "" || DataSource.DropTimeSec.Text == "") & DataSource.ChekoutMod.SelectedIndex != 2 & DataSource.Shop.SelectedIndex == 0)
            {
                Messages.ShowAlertMessage(Messages.NullDropTime, DataSource.AlertStack);

            }

            DataToTask.Size = DataSource.Size.txtContent.Text;
            DataToTask.TimeMode = DataSource.TimeMod.Text;
            DataToTask.ChekoutMode = DataSource.ChekoutMod.SelectedIndex;
            DataToTask.ProductLink = DataSource.link.Text;
            if (DataSource.TimeMod.SelectedIndex != 0)
            {
                DataToTask.Preload = PreloadTimeGeneration.GeneratePreload(DataSource.PreloadT.Text).Split(':');
            }

            if (DataSource.ChekoutMod.SelectedIndex != 2)
            {
                DataToTask.DropTime = (DataSource.DropTimeHou.Text + ":" + DataSource.DropTimeMin.Text + ":" + DataSource.DropTimeSec.Text + ":" + DataSource.DropTimeMili.Text).Split(':');

                if (DataSource.Delay.SelectedIndex != 0)
                {                                                                                                  // Время с применением Delay
                    DataToTask.DropTime[3] = (int.Parse(DataToTask.DropTime[3]) + (int.Parse(DataSource.Delay.Text))).ToString();
                    if (int.Parse(DataToTask.DropTime[3]) == 1000)
                    {
                        DataToTask.DropTime[2] = (int.Parse(DataToTask.DropTime[2]) + 1).ToString();
                        DataToTask.DropTime[3] = "0";
                    }
                    else
                    {
                        if (int.Parse(DataToTask.DropTime[3]) > 1000)
                        {
                            int remant = int.Parse(DataToTask.DropTime[3]) - 1000;
                            DataToTask.DropTime[2] = (int.Parse(DataToTask.DropTime[2]) + 1).ToString();
                            DataToTask.DropTime[3] = remant.ToString();
                        }

                    }
                    Console.WriteLine(DataToTask.DropTime[3] + "NEW MILISEC");
                    DataSource.DropTimeHou.Text = DataToTask.DropTime[0]; DataSource.DropTimeMin.Text = DataToTask.DropTime[1]; DataSource.DropTimeSec.Text = DataToTask.DropTime[2]; DataSource.DropTimeMili.Text = DataToTask.DropTime[3];
                }
            }

            mytask.Account.Content = DataToTask.Account;
            mytask.Profile.Content = DataSource.Profile.SelectedItem.ToString();
            mytask.Size.Content = DataToTask.Size;
            if (DataSource.ChekoutMod.SelectedIndex != 2)
            {
                mytask.Time.Content = DataToTask.DropTime[0] + ":" + DataToTask.DropTime[1] + ":" + DataToTask.DropTime[2] + ":" + DataToTask.DropTime[3];
            }
            else
            {
                mytask.Time.Content = "Auto";
            }

            if (DataSource.Proxy.SelectedIndex == 0)
            {
                mytask.Proxy.Content = "Localhost";
            }
            else
            {
                mytask.Proxy.Content = DataToTask.Proxy;
            }

        }
        public static void SetDnsData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator)
        {
            Console.WriteLine("Set Dns Data");
            CheckNullTextBox(Messages.NullLink, DataSource, DataSource.link.Text);
            SetAccountData(DataToTask, DataSource, AccountBlackList, Indicator, mytask);
            SetProxyData(DataToTask, DataSource, mytask, Indicator);
            CheckNullTextBox("Write your city", DataSource, DataSource.DnsCity.Text);
            DataToTask.CityToDns = DataSource.DnsCity.Text;
            DataToTask.DnsModeToDns = DataSource.Mode.SelectedIndex;
            DataToTask.ShopIdToDns = DataSource.ShopId.Text;
            DataToTask.ProductLink = DataSource.link.Text;
            DataToTask.DnsPreload = DataSource.PreloadTimeDns.SelectedIndex;

            mytask.Account.Content = DataToTask.Account;
            mytask.Profile.Content = "None";
            mytask.Size.Content = "None";
            mytask.Time.Content = "Auto";

            if (DataSource.Proxy.SelectedIndex == 0)
            {
                mytask.Proxy.Content = "Localhost";
            }
            else
            {
                mytask.Proxy.Content = DataToTask.Proxy;
            }


        }
        public static void SetLamodaData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator)
        {
            Console.WriteLine("Set Lamoda Data");
            CheckNullTextBox(Messages.NullLink, DataSource, DataSource.link.Text);
            CheckNullTextBox(Messages.NullSize, DataSource, DataSource.Size.txtContent.Text);
            SetAccountData(DataToTask, DataSource, AccountBlackList, Indicator, mytask);
            SetProfileData(DataToTask, DataSource);
            SetProxyData(DataToTask, DataSource, mytask, Indicator);
            DataToTask.Size = DataSource.Size.txtContent.Text;
            DataToTask.LamodaPaymentMode = DataSource.LamodaMode.SelectedIndex;
            DataToTask.ProductLink = DataSource.link.Text;

            mytask.Account.Content = DataToTask.Account;
            mytask.Profile.Content = DataSource.Profile.SelectedItem.ToString();
            mytask.Size.Content = DataToTask.Size;
            if (DataSource.Proxy.SelectedIndex == 0)
            {
                mytask.Proxy.Content = "Localhost";
            }
            else
            {

                mytask.Proxy.Content = DataToTask.Proxy;
            }
        }

        public static void SetBinanceData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator)
        {
            Console.WriteLine("Set binance Data");
            CheckNullTextBox(Messages.NullLink, DataSource, DataSource.link.Text);
            // SetProxyData(DataToTask, DataSource, mytask, Indicator);
            DataToTask.Cookies = DataSource.Cookies.Text;
            DataToTask.Headers = DataSource.Headers.Text;
            DataToTask.Amount = DataSource.Amount.Text;
            DataToTask.ProductLink = DataSource.link.Text;
            DataToTask.DropTime = (DataSource.DropTimeHou.Text + ":" + DataSource.DropTimeMin.Text + ":" + DataSource.DropTimeSec.Text + ":" + DataSource.DropTimeMili.Text).Split(':');

            mytask.Account.Content = "---";
            mytask.Profile.Content = "---";
            mytask.Size.Content = "---";
            mytask.Time.Content = DataToTask.DropTime[0] + ":" + DataToTask.DropTime[1] + ":" + DataToTask.DropTime[2] + ":" + DataToTask.DropTime[3];

            if (DataSource.Proxy.SelectedIndex == 0)
            {
                mytask.Proxy.Content = "Localhost";
            }
            else
            {
                DataToTask.Proxy = File.ReadAllText(DefinePaths.ProxysPath + DataSource.Proxy.SelectedItem.ToString() + ".txt");
                mytask.Proxy.Content = DataSource.Proxy.SelectedItem.ToString();
            }
        }

        public static void SetCitilinkData(DataSourceStructures.DataGive DataToTask, SourcePage DataSource, TaskPage mytask, string Indicator)
        {
            Console.WriteLine("Set citilink Data");
            SetAccountData(DataToTask, DataSource, AccountBlackList, Indicator, mytask);
            SetProxyData(DataToTask, DataSource, mytask, Indicator);
            DataToTask.CityToDns = DataSource.DnsCity.Text;
            DataToTask.PsVersion = DataSource.PSVersion.SelectedIndex;

            mytask.Account.Content = DataToTask.Account;
            mytask.Profile.Content = "---";
            mytask.Size.Content = "---";
            mytask.Time.Content = "---";

            if (DataSource.Proxy.SelectedIndex == 0)
            {
                mytask.Proxy.Content = "Localhost";
            }
            else
            {
                mytask.Proxy.Content = DataToTask.Proxy;
            }
        }

    }


}