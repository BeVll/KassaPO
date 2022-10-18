using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DateBase;
using DateBase.Models;

namespace KassaPOadmins
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {

        DBConnector con = new DBConnector();
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text == "")
                login.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#E95356");
            else if (password.Password == "")
                password.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#E95356");
            else
            {
                User user = con.GetUsers().Where(s => s.Login == login.Text).FirstOrDefault();
                if (user.Posada != "StoreWorker")
                    login.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#E95356");
                else if (user.Password == password.Password)
                    NavigationService.Navigate(new StoreWorker(user));
                else
                    password.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#E95356");
            }
        }

        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            login.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#4BA090");

        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#4BA090");
        }
    }
}
