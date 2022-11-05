using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace KassaAnalyze
{
    /// <summary>
    /// Логика взаимодействия для Cashier.xaml
    /// </summary>

    public partial class MainPage : Page
    {
        DBConnector con = new DBConnector();
        public ISeries[] Series { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public MainPage()
        {
            InitializeComponent();
            
        }
        public MainPage(User user)
        {
            List<double> values = new List<double>();
            InitializeComponent();
            StartSet();
            Thread upd = new Thread(UpdateStaffs);
            upd.Start();
        }
        private void UpdateStaffs()
        {
            while (true)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(delegate { staffs.ItemsSource = con.GetUsers().Where(s => s.Name.Contains(search_staff.Text) == true); }));
                    Thread.Sleep(1000);
                }
                catch
                {

                }
            }
        }
        private void StartSet()
        {
            filter3.SelectedIndex = 0;
            filter4.SelectedIndex = 0;
            List<Goods> goods = con.GetGoods();
            Goods g = new Goods();
            g.Name = "All goods";

            goods.Insert(0, g);
            filter1.ItemsSource = goods;
            filter1.DisplayMemberPath = "Name";
            filter1.SelectedIndex = 0;
            filter2.ItemsSource = goods;
            filter2.DisplayMemberPath = "Name";
            filter2.SelectedIndex = 0;
            Filter1Set(g);
            Labels = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            YFormatter = value => value.ToString("C");
            DataContext = this;
            List<Sell> sells = con.GetSells();
            List<Sell> prybutok = new List<Sell>();
            foreach (Sell ss in sells)
            {
                if (prybutok.Exists(u => u.Product_Id == ss.Product_Id) == true)
                {
                    int index = prybutok.IndexOf(prybutok.Where(t => t.Product_Id == ss.Product_Id).First());
                    prybutok[index].Amount += ss.Amount;
                }
                else
                    prybutok.Add(ss);
            }
            List<Delievery> delieveries = con.GetDelieveries();
            List<Delievery> delies = new List<Delievery>();
            foreach (Delievery ss in delieveries)
            {
                if (delies.Exists(u => u.Product_Id == ss.Product_Id) == true)
                {
                    int index = delies.IndexOf(delies.Where(t => t.Product_Id == ss.Product_Id).First());
                    delies[index].Price += ss.Price;
                }
                else
                    delies.Add(ss);
            }
            foreach (Sell s in prybutok)
            {
                s.Amount -= delieveries.Where(t => t.Product_Id == s.Product_Id).FirstOrDefault().Price;
            }
            prybutok = prybutok.OrderByDescending(t => t.Amount).ToList();
            popular.AlternationCount = prybutok.Count + 1;
            popular.ItemsSource = prybutok;
            staffs.ItemsSource = con.GetUsers();
            search_staff.Text = "";
        }
        private void filter1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter1 != null)
            {
                Goods goods = filter1.SelectedItem as Goods;
                Filter1Set(goods);
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            staffs.ItemsSource = con.GetUsers().Where(s => s.Name.Contains(search_staff.Text) == true);
        }
        private void Filter1Set(Goods g)
        {
            List<Sell> sells = new List<Sell>();
            List<double> values = new List<double>();
            if (g.Name == "All goods")
                sells = con.GetSells().Where(s => s.DateTime.Year == DateTime.Today.Year).ToList();
            else
                sells = con.GetSells().Where(s => s.DateTime.Year == DateTime.Today.Year && s.Product_Id == g.Id).ToList();


            for (int i = 1; i <= 12; i++)
            {
                values.Add(sells.Where(s => s.DateTime.Month == i).ToList().Sum(s => s.Amount));
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Sales",
                    Values = values.AsChartValues(),
                    LineSmoothness = 0.1,
                    PointGeometrySize = 10
                },

            };
            Graph1.Series = SeriesCollection;
            Graph1.Update(true, true);
        }
        private void Filter2Set(Goods g)
        {
            List<Delievery> delieveries = new List<Delievery>();
            List<double> values = new List<double>();
            if (g.Name == "All goods")
                delieveries = con.GetDelieveries().Where(s => s.Date.Year == DateTime.Today.Year).ToList();
            else
                delieveries = con.GetDelieveries().Where(s => s.Date.Year == DateTime.Today.Year && s.Product_Id == g.Id).ToList();


            for (int i = 1; i <= 12; i++)
            {
                values.Add(delieveries.Where(s => s.Date.Month == i).ToList().Sum(s => s.Price));
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Delieveries",
                    Values = values.AsChartValues(),
                    LineSmoothness = 0.1,
                    PointGeometrySize = 10
                },

            };

            
            Graph2.Series = SeriesCollection;
            Graph2.Update(true, true);
        }

        private void filter2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Goods goods = filter2.SelectedItem as Goods;
            Filter2Set(goods);
        }

        private void filter3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter3 != null && Income != null)
            {
                int index = filter3.SelectedIndex;
                double inc = 0;
                if (index == 0)
                {
                    List<Sell> se = con.GetSells();
                    inc = se.Sum(s => s.Amount);
                }
                    
                else if (index == 1)
                    inc = con.GetSells().Where(s => s.DateTime >= DateTime.Now.AddDays(-1)).Sum(s => s.Amount);
                else if (index == 2)
                    inc = con.GetSells().Where(s => s.DateTime >= DateTime.Now.AddMonths(-1)).Sum(s => s.Amount);
                else if (index == 3)
                    inc = con.GetSells().Where(s => s.DateTime >= DateTime.Now.AddYears(-1)).Sum(s => s.Amount);
                Income.Text = Math.Round(inc, 2).ToString() + " грн";
            }
        }

        private void filter4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter4 != null && Income != null)
            {
                int index = filter4.SelectedIndex;
                double inc = 0;
                if (index == 0)
                {
                    List<Delievery> se = con.GetDelieveries();
                    inc = se.Sum(s => s.Price);
                }
                else if (index == 1)
                    inc = con.GetSells().Where(s => s.DateTime >= DateTime.Now.AddDays(-1)).Sum(s => s.Amount);
                else if (index == 2)
                    inc = con.GetSells().Where(s => s.DateTime >= DateTime.Now.AddMonths(-1)).Sum(s => s.Amount);
                else if (index == 3)
                    inc = con.GetSells().Where(s => s.DateTime >= DateTime.Now.AddYears(-1)).Sum(s => s.Amount);
                Outcome.Text = Math.Round(inc, 2).ToString() + " грн";
            }
        }

        private void staffs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            User user = staffs.SelectedItem as User;
            if(user != null)
            {
                NavigationService.Navigate(new Staff(user));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Staff());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(staffs.SelectedItem != null)
            {
                User user = staffs.SelectedItem as User;
                con.DeleteUser(user);
                staffs.ItemsSource = con.GetUsers();
            }
        }
    }
}
