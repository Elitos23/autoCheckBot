using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace STDAPP
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private string ProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/STARDOM/PROF/";
        public ProfilePage()
        {
            InitializeComponent();
            DpiFactorContainer.Resize(PrifileGrid);
        }

        public void SavingFileForProfile(string Name, string[] Content)
        {
            if (Name != "")
            {
                try
                {
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(ProfilePath + Name + ".txt");
                    //Write a line of text
                    sw.WriteLine(Content[0]);
                    sw.WriteLine(Content[1]);
                    sw.WriteLine(Content[2]);
                    sw.WriteLine(Content[3]);
                    sw.WriteLine(Content[4]);
                    sw.WriteLine(Content[5]);
                    sw.WriteLine(Content[6]);
                    sw.WriteLine(Content[7]);
                    sw.WriteLine(Content[8]);
                    sw.WriteLine(Content[9]);
                    sw.WriteLine(Content[10]);
                    sw.WriteLine(Content[11]);
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
                Messages.ShowAlertMessageProfile("Give a name to your profile", AlertStack);
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = ProfileName.Text;
            string[] ProfileData = new string[12];
            ProfileData[0] = Name.Text;
            ProfileData[1] = SecondName.Text;
            ProfileData[2] = LastName.Text;
            ProfileData[3] = Address1.Text;
            ProfileData[4] = Address2.Text;
            ProfileData[5] = City.Text;
            ProfileData[6] = Index.Text;
            ProfileData[7] = Phone.Text;
            ProfileData[8] = Email.Text;
            ProfileData[9] = CardNumber.Text;
            ProfileData[10] = CVC.Text;
            ProfileData[11] = MM.Text + YY.Text;
            SavingFileForProfile(name, ProfileData);
            if (name != "")
            {
                Messages.ShowAlertMessageProfile("Profile - " + name + " saved", AlertStack);
            }

        }

        private void CBLoadProfile_DropDownOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OPENED");
            try
            {
                if (Directory.Exists(ProfilePath))
                {
                    string[] files = Directory.GetFiles(ProfilePath);
                    foreach (string s in files)
                    {
                        Console.WriteLine(s);
                        string NameToAddInCb = Regex.Match(s, @"PROF/(.*?).txt").Groups[1].Value;
                        Console.WriteLine("Adding - " + NameToAddInCb);
                        if (CBLoadProfile.Items.Contains(NameToAddInCb))
                        {
                            Console.WriteLine("Already exists");
                        }
                        else
                        {
                            CBLoadProfile.Items.Add(NameToAddInCb);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void CBLoadProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(CBLoadProfile.SelectedItem);
            string FileName = "";
            if (CBLoadProfile.SelectedItem != null)
            {
                FileName = CBLoadProfile.SelectedItem.ToString();
                ProfileName.Text = FileName;
            }
            else
            {
                return;
            }
            try
            {
                string Content;
                
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(ProfilePath + FileName + ".txt");
                //Read all text
                Content = sr.ReadToEnd();
                Console.WriteLine(Content);
                string MMYY = MM.Text + YY.Text;
                string[] ProfileData = Content.Split('\n');

                string mm = ProfileData[11];
                Name.Text = ProfileData[0];
                SecondName.Text = ProfileData[1];
                LastName.Text = ProfileData[2];
                Address1.Text = ProfileData[3];
                Address2.Text = ProfileData[4];
                City.Text = ProfileData[5];
                Index.Text = ProfileData[6];
                Phone.Text = ProfileData[7];
                Email.Text = ProfileData[8];
                CardNumber.Text = ProfileData[9];
                CVC.Text = ProfileData[10];
                MM.Text = mm[0].ToString() + mm[1].ToString();
                YY.Text = mm[2].ToString() + mm[3].ToString();

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



        private void ClearAllFields()
        {
            Name.Clear();
            SecondName.Clear();
            LastName.Clear();
            Address1.Clear();
            Address2.Clear();
            City.Clear();
            Index.Clear();
            Phone.Clear();
            Email.Clear();
            CardNumber.Clear();
            CVC.Clear();
            MM.Clear();
            YY.Clear();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)  // delete button click 
        {
           
        }

        private void ProfileDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Deleting ");
                string ProfName = CBLoadProfile.Text;
                string del = ProfilePath + CBLoadProfile.Text + ".txt";
                System.IO.File.Delete(del);
                CBLoadProfile.Items.Remove(CBLoadProfile.Text);
                ClearAllFields();
                Messages.ShowAlertMessageProfile("Profile - " + ProfName + " deleted", AlertStack);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Messages.ShowAlertMessageProfile("Deleting error", AlertStack);
            }

        }

        private void MM_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MM.Text.Length == 2)
            {
                YY.Focus();
            }
        }

        private void MM_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (MM.Text.Length == 2)
            {
                YY.Focus();
            }
        }
    }
}
