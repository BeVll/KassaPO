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

namespace KassaAnalyze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User user;
        public MainWindow()
        {
            InitializeComponent();
            pFrame.Navigate(new Login());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pFrame.Navigate(new MainPage(user));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            pFrame.Navigate(new Staff(user));
        }
    }
}
