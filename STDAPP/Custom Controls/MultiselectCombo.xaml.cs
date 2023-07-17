using System;
using System.Windows.Controls;

namespace STDAPP
{
    public partial class MultiselectCombo : UserControl
    {
        public MultiselectCombo()
        {
            InitializeComponent();
        }

        private void lstComboItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var textboxText = string.Empty;

            foreach (ListBoxItem selectedItem in lstComboItems.SelectedItems)
            {
                if (selectedItem.Content.ToString() == "Random(Lamoda)")
                {
                    lstComboItems.SelectedItems.Clear();
                    textboxText = "Random(Lamoda)";
                    break;
                }
                textboxText = string.Format("{0}{1}, ", textboxText, selectedItem.Content);
            }

            if (textboxText.Length > 2 & textboxText != "Random(Lamoda)")
                textboxText = textboxText.Substring(0, textboxText.Length - 2);


            this.txtContent.Text = textboxText;

        }

        private void TogelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Console.WriteLine("a[pdka[spd[pask[dkas[ksdk");
        }

        private void MainBorderClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(btnDropDown.IsChecked == true)
                btnDropDown.IsChecked = false;
            else
                btnDropDown.IsChecked = true;
            Console.WriteLine("a[pdka[spd[pask[dkas[ksdk");
        }
    }
}