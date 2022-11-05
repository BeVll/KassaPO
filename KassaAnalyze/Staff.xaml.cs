using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DateBase;

namespace KassaAnalyze
{
    /// <summary>
    /// Логика взаимодействия для Staff.xaml
    /// </summary>
    public partial class Staff : Page
    {
        User user;
        DBConnector con = new DBConnector();
        public Staff()
        {
            InitializeComponent();
            Posada.Items.Add("Cashier");
            Posada.Items.Add("StoreWorker");
            Posada.Items.Add("Manager");
            Mode_user.Text = "User add";
            save_upd_btn.Content = "Add";
            Posada.SelectedIndex = 0;
        }

        public Staff(User user)
        {
            InitializeComponent();
            this.user = user;
            Name.Text = user.Name;
            Login.Text = user.Login;
            Password.Password = user.Password;
            Posada.SelectedItem = user.Posada;
            Payment.Text = user.Payment.ToString();
            Posada.Items.Add("Cashier");
            Posada.Items.Add("StoreWorker");
            Posada.Items.Add("Manager");
            save_upd_btn.Content = "Save";


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Login.Text != "" && Password.Password != "" && Name.Text != "" && Payment.Text != "" && Posada.SelectedItem != null)
            {
                if (save_upd_btn.Content == "Save")
                {
                    if (con.GetUsers().Where(s => s.Login == Login.Text).FirstOrDefault() == null)
                    {
                        if (Password.Password.Length >= 6)
                        {
                            user.Posada = Posada.SelectedItem as string;
                            user.Login = Login.Text;
                            user.Password = Password.Password;
                            user.Payment = Convert.ToDouble(Payment.Text);
                            user.Name = Name.Text;
                            con.UpdateUser(user);
                            ErrorMes.Text = "";
                            NavigationService.GoBack();
                        }
                        else
                        {
                            ErrorMes.Text = "The password must contain at least 6 characters!";
                        }
                    }
                    else
                    {
                        ErrorMes.Text = "This login is already use!";
                    }
                }
                else
                {
                    if (con.GetUsers().Where(s => s.Login == Login.Text).FirstOrDefault() == null)
                    {
                        User user2 = new User(); 
                        user2.Posada = Posada.SelectedItem as string;
                        user2.Login = Login.Text;
                        user2.Password = Password.Password;
                        user2.Payment = Convert.ToDouble(Payment.Text);
                        user2.Name = Name.Text;
                        con.AddUser(user2);
                        ErrorMes.Text = "";
                        NavigationService.GoBack();
                    }
                    else
                    {
                        ErrorMes.Text = "This login is already use!";
                    }
                }
            }
            else
            {
                ErrorMes.Text = "There are empty fields!";
            }
        }

        private void Payment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
