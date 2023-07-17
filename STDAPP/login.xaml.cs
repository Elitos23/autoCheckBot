using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
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
using System.Data;

namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }

        string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/key.txt";
        string dbkey = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/STARDOM/dbkey.txt";


        string num;


        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            string loginUser = loginField.Text;



            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `botkeys2` WHERE `botkey` = @uL", db.getConnection());

            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;


            adapter.SelectCommand = command;
            adapter.Fill(table);



            if (table.Rows.Count > 0)

            {

                this.Hide();
                // ALL form3 = new ALL();


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



                MySqlDataAdapter adapter1 = new MySqlDataAdapter();
                MySqlCommand command3 = new MySqlCommand("SELECT * FROM `keysbot` WHERE `pckey` = @key", db.getConnection());
                if (resultObjX64 != null && resultObjX64.ToString() != "default")
                {
                    num = resultObjX64.ToString();


                    if (IsMoreExists())
                    {

                        return;
                    }



                    System.IO.File.WriteAllText(filename, resultObjX64.ToString());
                    System.IO.File.WriteAllText(dbkey, loginUser);



                    MySqlCommand command1 = new MySqlCommand("INSERT INTO `keysbot` (`botkeys`,`pckey`) VALUES ( @key, @pckey)", db.getConnection());
                    command1.Parameters.Add("@key", MySqlDbType.VarChar).Value = loginUser;
                    command1.Parameters.Add("@pckey", MySqlDbType.VarChar).Value = resultObjX64.ToString();

                    db.openConnection();
                    MainWindow mn = new MainWindow();
                    if (command1.ExecuteNonQuery() == 1)
                    {

                        this.Close();

                        mn.Show();

                        Console.WriteLine("хуй хуй хуй хуй");

                    }
                    else
                    {

                        MessageBox.Show("Invalid key");
                        this.Close();

                    }








                }
                else
                {


                }



                if (resultObjX86 != null && resultObjX86.ToString() != "default")
                {
                    num = resultObjX86.ToString();


                    if (IsMoreExists())
                    {

                        return;
                    }


                    System.IO.File.WriteAllText(filename, resultObjX86.ToString());
                    System.IO.File.WriteAllText(dbkey, loginUser);

                    MySqlCommand command1 = new MySqlCommand("INSERT INTO `keysbot` (`botkeys`,`pckey`) VALUES ( @key, @pckey)", db.getConnection());
                    command1.Parameters.Add("@key", MySqlDbType.VarChar).Value = loginUser;
                    command1.Parameters.Add("@pckey", MySqlDbType.VarChar).Value = resultObjX86.ToString();

                    db.openConnection();

                    MainWindow mn = new MainWindow();
                    if (command1.ExecuteNonQuery() == 1)
                    {
                        this.Close();

                        mn.Show();

                        Console.WriteLine("хуй хуй хуй хуй");

                    }
                    else
                    {
                        MessageBox.Show("Invalid key");
                        this.Close();

                    }


                }
                else
                {


                }




            }
            else
            {


            }


        }






        public Boolean IsMoreExists()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `keysbot` WHERE `botkeys` = @botkey", db.getConnection());

            command.Parameters.Add("@botkey", MySqlDbType.VarChar).Value = loginField.Text;


            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Уже зарегестрированно более 1 пк, обратитесь к саппорту");
                this.Close();
                return true;

            }
            else
            {

                return false;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
