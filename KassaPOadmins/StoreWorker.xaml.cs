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
    /// Логика взаимодействия для StoreWorker.xaml
    /// </summary>
    public partial class StoreWorker : Page
    {
        User User = new User();
        DBConnector con = new DBConnector();
        
        public StoreWorker(User user)
        {
            InitializeComponent();
            this.User = user;
            SearchGoods();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Search.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#4BA090");
            Search.BorderThickness = new Thickness(3, 3, 3, 3);
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            Search.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A5A2A2");
            Search.BorderThickness = new Thickness(2, 2, 2, 2);

        }
        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
        private void lv_KeyUp(object sender, KeyEventArgs e)
        {
            List<Goods> goods = lv.Items.Cast<Goods>().ToList();
            con.UpdateGoods(goods);
        }
        private void lv_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
        }
        private void lv_CurrentCellChanged_1(object sender, EventArgs e)
        {

            List<Goods> goods = lv.Items.Cast<Goods>().ToList();
            con.UpdateGoods(goods);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Goods goods = new Goods();
            goods.ProductCode = 123456789;
            goods.Price = 0;
            goods.Count = 0;
            goods.Name = "Name";
            con.AddGood(goods);
            SearchGoods();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(lv.SelectedItem != null)
            {
                Goods goods = lv.SelectedItem as Goods;
                con.DeleteGood(goods);
                List<Goods> goods1 = lv.Items.Cast<Goods>().ToList();
                goods1.Remove(goods);
                lv.ItemsSource = goods1;
            }
        }
        private void SearchGoods()
        {
            List<Goods> goodItems = new List<Goods>();
            goodItems = con.GetGoods().Where(s => s.Name.Contains(Search.Text) == true).ToList();
            List<Goods> g = con.GetGoods().Where(s => s.ProductCode.ToString().Contains(Search.Text) == true).ToList();
            foreach (Goods item in g)
            {
                Goods goodItem = goodItems.Where(s => s.Id == item.Id).FirstOrDefault();
                if (goodItem == null)
                    goodItems.Add(item);
            }
            lv.ItemsSource = goodItems;
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchGoods();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SearchGoods();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Import(User));
        }
    }
}
