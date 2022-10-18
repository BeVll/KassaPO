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
    public partial class Import : Page
    {
        User User = new User();
        DBConnector con = new DBConnector();
        
        public Import(User user)
        {
            InitializeComponent();
            this.User = user;
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
        private void ItemsPresenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GoodItem g = new GoodItem();
            if (e.AddedItems.Count > 0)
                g = e.AddedItems[0] as GoodItem;
            if (g != null && g.Name != null)
            {
                List<Delievery> items = lv.Items.Cast<Delievery>().ToList();
                Delievery delievery = new Delievery();
                delievery.Price = 0;
                delievery.Product_Id = g.Id;
                delievery.Product_Name = g.Name;
                delievery.Product_Code = g.ProductCode;
                delievery.Company = "Company";
                delievery.Count = 0;
                delievery.Date = DateTime.Now;
                con.AddDelievery(delievery);
                
                lv.ItemsSource = con.GetDelieveries();
                Search.Text = "";
            }

        }
        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<GoodItem> goodItems = new List<GoodItem>();
            goodItems = con.GoodsToGoodItems(con.GetGoods().Where(s => s.Name.Contains(Search.Text) == true).ToList());
            List<GoodItem> g = con.GoodsToGoodItems(con.GetGoods().Where(s => s.ProductCode.ToString().Contains(Search.Text) == true).ToList());
            foreach (GoodItem item in g)
            {
                GoodItem goodItem = goodItems.Where(s => s.Id == item.Id).FirstOrDefault();
                if (goodItem == null)
                    goodItems.Add(item);
            }
            Search.ItemsSource = goodItems;
        }
        private void lv_KeyUp(object sender, KeyEventArgs e)
        {
            List<Delievery> delieveries = lv.Items.Cast<Delievery>().ToList();
            con.UpdateDelieveries(delieveries);
        }
        private void lv_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
        }
        private void lv_CurrentCellChanged_1(object sender, EventArgs e)
        {

            List<Delievery> delieveries = lv.Items.Cast<Delievery>().ToList();
            con.UpdateDelieveries(delieveries);

        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(lv.SelectedItem != null)
            {
                Delievery delievery = lv.SelectedItem as Delievery;
                con.DeleteDelievery(delievery);
                List<Delievery> delieveries = lv.Items.Cast<Delievery>().ToList();
                delieveries.Remove(delievery);
                lv.ItemsSource = delieveries;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lv.ItemsSource = con.GetDelieveries();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
