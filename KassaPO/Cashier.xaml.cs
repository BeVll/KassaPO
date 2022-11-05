using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace KassaPO
{
    /// <summary>
    /// Логика взаимодействия для Cashier.xaml
    /// </summary>

    public partial class Cashier : Page
    {
        DBConnector con = new DBConnector();
        bool refr = false;
        UserContext db = new UserContext();
        double discount = 0;
        User User;
        double total = 0.00;
        double payment = 0.00;
        public Cashier()
        {
            InitializeComponent();
            List<Goods> goods = db.Goods.ToList();
            List<GoodItem> goodItems = new List<GoodItem>();
        }
        public Cashier(User user)
        {
            InitializeComponent();
            List<Goods> goods = db.Goods.ToList();
            List<GoodItem> goodItems = new List<GoodItem>();
            User = user;
            SetTotal();
        }
        
        private List<GoodItem> GoodsToGoodItems(List<Goods> g)
        {
            List<GoodItem> gItems = new List<GoodItem>();
            foreach (Goods g2 in g)
            {
                GoodItem goodItem = new GoodItem();
                goodItem.Id = g2.Id;
                goodItem.ProductCode = g2.ProductCode;
                goodItem.Quantity = 1;
                goodItem.Price = g2.Price;
                goodItem.Amount = g2.Price;
                goodItem.Name = g2.Name;
                goodItem.Discount = 0;
                gItems.Add(goodItem);
            }
            return gItems;
        }

        private GoodItem GoodToGoodItem(Goods g)
        {

            GoodItem goodItem = new GoodItem();
            goodItem.Id = g.Id;
            goodItem.ProductCode = g.ProductCode;
            goodItem.Quantity = 1;
            goodItem.Price = g.Price;
            goodItem.Amount = g.Price;
            goodItem.Name = g.Name;
            goodItem.Discount = 0;

            return goodItem;
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            

        }

        private void lv_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
        }

        
        private void lv_CellEditEnding_1(object sender, DataGridCellEditEndingEventArgs e)
        {
            
            
    

    
                

           
        }


        private void lv_CurrentCellChanged_1(object sender, EventArgs e)
        {
           
            if (lv.SelectedItem != null)
            {
                lv.CommitEdit();
                GoodItem item = (GoodItem)lv.SelectedItem;

                item.Amount = item.Quantity * item.Price;
                item.Amount -= item.Amount * (item.Discount / 100.0);
                lv.Items.Refresh();
                SetTotal();
            }

        }

        private void lv_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void lv_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lv.CommitEdit();
                if (lv.SelectedItem != null)
                {
                    GoodItem item = lv.SelectedItem as GoodItem;

                    item.Amount = item.Quantity * item.Price;
                    item.Amount -= item.Amount * (item.Discount / 100.0);
                    lv.Items.Refresh();
                    SetTotal();
                }
            }
        }

        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<GoodItem> goodItems = new List<GoodItem>();
            goodItems = GoodsToGoodItems(db.Goods.Where(s => s.Name.Contains(Search.Text) == true).ToList());
            List<GoodItem> g = GoodsToGoodItems(db.Goods.Where(s => s.ProductCode.ToString().Contains(Search.Text) == true).ToList());
            foreach(GoodItem item in g)
            {
                GoodItem goodItem = goodItems.Where(s => s.Id == item.Id).FirstOrDefault();
                if (goodItem == null)
                    goodItems.Add(item);
            }
            Search.ItemsSource = goodItems;
        }

        private void SetTotal()
        {
            List<GoodItem> items = lv.Items.Cast<GoodItem>().ToList();
            if (items.Count > 0)
            {
                payment = items.Sum(s => s.Amount);
                Math.Round(payment, 2);
                total = payment - (payment * (discount / 100.0));
                Math.Round(total, 2);
            }
            else
            {
                total = 0;
                payment = 0;
            }
            Total.Text = total.ToString() + "грн";
            Payment.Text = payment.ToString();
        }

        private void ItemsPresenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GoodItem g = new GoodItem();
            if (e.AddedItems.Count > 0)
                g = e.AddedItems[0] as GoodItem;
            if (g != null && g.Name != null)
            {
                List<GoodItem> items = lv.Items.Cast<GoodItem>().ToList();
                items.Add(g);
                lv.ItemsSource = items;
                Search.Text = "";
                SetTotal();
            }
            
        }

        private void Search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lv_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            SetTotal();
        }

        private void lv_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            SetTotal();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Sell> sells = new List<Sell>();
            List<GoodItem> list = lv.Items.Cast<GoodItem>().ToList();
            foreach(GoodItem g in list)
            {
                Sell sell = new Sell();
                sell.Product_Id = g.Id;
                sell.Name = g.Name;
                sell.Price = g.Price;
                sell.Quantity = g.Quantity;
                sell.Discount = g.Discount;
                sell.Amount = g.Amount;
                sell.Cashier_id = User.Id;
                sell.DateTime = DateTime.Now;
                sell.ProductCode = g.ProductCode;
                con.AddSell(sell);
                
            }
            
                lv.ItemsSource = new List<GoodItem>();
                SetTotal();
             
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lv.ItemsSource = new List<GoodItem>();
            SetTotal();
        }
    }
}
