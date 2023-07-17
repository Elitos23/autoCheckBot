using System;
using System.Collections.Generic;
using System.IO;

namespace STDAPP
{
    class AccountData
    {
        
        public  struct AccountTable      // структура для пула аккаунтов
        {
            public int NumberOfOccupied;
            public string CbName;

        }

        private static List<AccountTable> accountTableListSrc = new List<AccountTable>(); // лист со структурой 
        private static List<AccountTable> accountTableListEditor = new List<AccountTable>(); // лист со структурой 

        private static List<AccountTable> ProxyTableListSrc = new List<AccountTable>(); // лист со структурой для прокси
        public static int GiveAccountIndexSrc(string FileName)
        {
            try
            {
                int ItemExists = 0;
                AccountTable example;
                example.CbName = "";
                example.NumberOfOccupied = 0;
                foreach (var item in accountTableListSrc)
                {
                    if (item.CbName == FileName)
                    {
                        example = item;
                        ItemExists++;
                        break;
                    }

                }
                if (ItemExists == 0)
                {
                    example.CbName = FileName;
                    example.NumberOfOccupied = 0;
                    accountTableListSrc.Add(example);
                }

                Console.WriteLine(example.CbName + "CbName");
                Console.WriteLine(example.NumberOfOccupied + "NumberOfOccupied");

                int NumberInList = accountTableListSrc.IndexOf(example);
                int StrNumber = accountTableListSrc[NumberInList].NumberOfOccupied;
                accountTableListSrc.RemoveAt(NumberInList);
                example.NumberOfOccupied++;
                accountTableListSrc.Add(example);
                return StrNumber;
            }
            catch
            {
                return 1488;
            }
        }

        public static int GiveProxyIndexSrc(string FileName)
        {
            try
            {
                int ItemExists = 0;
                AccountTable example;
                example.CbName = "";
                example.NumberOfOccupied = 0;
                foreach (var item in ProxyTableListSrc)
                {
                    if (item.CbName == FileName)
                    {
                        example = item;
                        ItemExists++;
                        break;
                    }

                }
                if (ItemExists == 0)
                {
                    example.CbName = FileName;
                    example.NumberOfOccupied = 0;
                    ProxyTableListSrc.Add(example);
                }

                Console.WriteLine(example.CbName + "CbName");
                Console.WriteLine(example.NumberOfOccupied + "NumberOfOccupied");

                int NumberInList = ProxyTableListSrc.IndexOf(example);
                int StrNumber = ProxyTableListSrc[NumberInList].NumberOfOccupied;
                ProxyTableListSrc.RemoveAt(NumberInList);
                example.NumberOfOccupied++;
                ProxyTableListSrc.Add(example);
                return StrNumber;
            }
            catch
            {
                return 1488;
            }
        }

        public static int GiveAccountIndexAutoreg(string FileName)
        {
            try
            {
                int ItemExists = 0;
                AccountTable example;
                example.CbName = "";
                example.NumberOfOccupied = 0;
                foreach (var item in accountTableListSrc)
                {
                    if (item.CbName == FileName)
                    {
                        example = item;
                        ItemExists++;
                        break;
                    }

                }
                if (ItemExists == 0)
                {
                    example.CbName = FileName;
                    example.NumberOfOccupied = 0;
                    accountTableListSrc.Add(example);
                }

                Console.WriteLine(example.CbName + "CbName");
                Console.WriteLine(example.NumberOfOccupied + "NumberOfOccupied");

                int NumberInList = accountTableListSrc.IndexOf(example);
                int StrNumber = accountTableListSrc[NumberInList].NumberOfOccupied;
                accountTableListSrc.RemoveAt(NumberInList);
                example.NumberOfOccupied++;
                accountTableListSrc.Add(example);
                return StrNumber;
            }
            catch
            {
                return 1488;
            }
        }
        public static int GiveAccountIndexEditor(string FileName)
        {
            try
            {
                int ItemExists = 0;
                AccountTable example;
                example.CbName = "";
                example.NumberOfOccupied = 0;
                foreach (var item in accountTableListEditor)
                {
                    if (item.CbName == FileName)
                    {
                        example = item;
                        ItemExists++;
                        break;
                    }

                }
                if (ItemExists == 0)
                {
                    example.CbName = FileName;
                    example.NumberOfOccupied = 0;
                    accountTableListEditor.Add(example);
                }

                Console.WriteLine(example.CbName + "CbName");
                Console.WriteLine(example.NumberOfOccupied + "NumberOfOccupied");

                int NumberInList = accountTableListEditor.IndexOf(example);
                int StrNumber = accountTableListEditor[NumberInList].NumberOfOccupied;
                accountTableListEditor.RemoveAt(NumberInList);
                example.NumberOfOccupied++;
                accountTableListEditor.Add(example);
                return StrNumber;
            }
            catch
            {
                return 1488;
            }
        }
        public static string GiveMeAccount(int StrNumber, string FileName)
        {
            try
            {
                string AccountPass;
                StreamReader sr = new StreamReader(DefinePaths.AccountsPath + FileName + ".txt");
                //Read all text
                string Content = sr.ReadToEnd();

                string[] AccountData = Content.Split('\n');
                AccountPass = AccountData[StrNumber];
                string[] AccountPassFinal = new string[2];
                AccountPassFinal = AccountPass.Split('\r');
                Console.WriteLine(AccountPassFinal[0]);
                if (AccountPassFinal[0] != "")
                {
                    return AccountPassFinal[0];
                }
                else
                {
                    return "ERROR";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return "ERROR";
            }
        }

        public static string GiveMeProxy(int StrNumber, string FileName)
        {
            try
            {
                string AccountPass;
                StreamReader sr = new StreamReader(DefinePaths.ProxysPath + FileName + ".txt");
                //Read all text
                string Content = sr.ReadToEnd();

                string[] AccountData = Content.Split('\n');
                AccountPass = AccountData[StrNumber];
                string[] AccountPassFinal = new string[2];
                AccountPassFinal = AccountPass.Split('\r');
                Console.WriteLine(AccountPassFinal[0]);
                if (AccountPassFinal[0] != "")
                {
                    return AccountPassFinal[0];
                }
                else
                {
                    return "ERROR";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return "ERROR";
            }
        }
    }
}
