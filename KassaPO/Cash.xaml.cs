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
using System.Windows.Shapes;
using DateBase.Models;
using DateBase;

namespace KassaPO
{
    /// <summary>
    /// Логика взаимодействия для Cash.xaml
    /// </summary>
    public partial class Cash : Window
    {
        DBConnector con = new DBConnector();
        List<Sell> Sells = new List<Sell>();
        public Cash(List<Sell> sells, double total)
        {
            InitializeComponent();
            Sells = sells;
            Payment.Text = total.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach(Sell s in Sells)
                con.AddSell(s);
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.check = true;
            App.Current.MainWindow = w;
            Close();
            
        }

        private void Rec_TextChanged(object sender, TextChangedEventArgs e)
        {
            Rest.Text = (Convert.ToDouble(Rec.Text) - Convert.ToDouble(Payment.Text)).ToString();
        }
    }
}
