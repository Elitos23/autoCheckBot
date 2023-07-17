using System;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DiscordRPC;
using System.IO;

namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для load.xaml
    /// </summary>
    public partial class load : Window
    {
        static string num;
        public DiscordRpcClient client;
        public load()
        {
            InitializeComponent();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {


            client = new DiscordRpcClient("745254482472075366");
            client.Initialize();
            client.SetPresence(new DiscordRPC.RichPresence()
            {
                Details = "Version 3.2.6",
                State = "It's STARDOM time",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = "16",
                    LargeImageText = "STARDOM",
                    //SmallImageKey = "8787"
                }
            });
            Console.WriteLine("Хуй");
            string x64Result = string.Empty;
            string x86Result = string.Empty;
            RegistryKey keyBaseX64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey keyBaseX86 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey keyX64 = keyBaseX64.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", RegistryKeyPermissionCheck.ReadSubTree);
            RegistryKey keyX86 = keyBaseX86.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", RegistryKeyPermissionCheck.ReadSubTree);
            object resultObjX64 = keyX64.GetValue("MachineGuid", (object)"default");
            object resultObjX86 = keyX86.GetValue("MachineGuid", (object)"default");
            keyX64.Close();
            keyX86.Close();
            keyBaseX64.Close();
            keyBaseX86.Close();
            keyX64.Dispose();
            keyX86.Dispose();
            keyBaseX64.Dispose();
            keyBaseX86.Dispose();
            keyX64 = null;
            keyX86 = null;
            keyBaseX64 = null;
            keyBaseX86 = null;



            if (resultObjX64 != null && resultObjX64.ToString() != "default")
            {
                num = resultObjX64.ToString();






            }

            if (resultObjX86 != null && resultObjX86.ToString() != "default")
            {
                num = resultObjX86.ToString();






            }




            string dbkey = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/dbkey.txt";


            // Для работы TripleDES требуется вектор инициализации (IV) и ключ (Key)
            // Операции шифрования/деширования должны использовать одинаковые значения IV и Key
            if (System.IO.File.Exists(dbkey))
            {

                DB db = new DB();
                string loginUser = System.IO.File.ReadAllText(dbkey);
                DataTable table = new DataTable();

                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand command = new MySqlCommand("SELECT * FROM `botkeys2` WHERE `botkey` = @uL", db.getConnection());





                if (resultObjX64 != null && resultObjX64.ToString() != "default")
                {

                    command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;


                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                    num = resultObjX64.ToString();
                }
                if (resultObjX86 != null && resultObjX86.ToString() != "default")
                {


                    command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;


                    adapter.SelectCommand = command;
                    adapter.Fill(table);

                    num = resultObjX86.ToString();


                }





                string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/key.txt";
                DB db1 = new DB();

                DataTable table1 = new DataTable();

                MySqlDataAdapter adapter1 = new MySqlDataAdapter();

                MySqlCommand command1 = new MySqlCommand("SELECT * FROM `keysbot` WHERE `pckey` = @numb", db1.getConnection());

                command1.Parameters.Add("@numb", MySqlDbType.VarChar).Value = num;

                adapter1.SelectCommand = command1;
                adapter1.Fill(table1);
                if (table1.Rows.Count > 0)
                {
                    if (System.IO.File.Exists(filename))
                    {

                        if (File.ReadAllText(filename).ToString().Equals(num))
                        {
                            Console.WriteLine("Пароль не нужен");


                            MainWindow mn = new MainWindow();
                            mn.Show();


                        }
                    }
                    else
                    {

                        this.Hide();
                        login log = new login();
                        log.Show();
                    }

                }
                else
                {
                    this.Hide();
                    login log = new login();
                    log.Show();
                }
                Console.WriteLine("Ключ найден");





            }
            else
            {
                this.Hide();
                login log = new login();
                log.Show();
            }




        }
        public void sosat()
        {
            this.Hide();



        }

        public static Boolean IsUserExists()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `keysbot` WHERE `pckey` = @num", db.getConnection());

            command.Parameters.Add("@num", MySqlDbType.VarChar).Value = num;


            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MainWindow mn = new MainWindow();
                mn.Show();


                return true;


            }
            else
            {


                login log = new login();
                log.Show();


                return false;

            }

        }

    }
}
