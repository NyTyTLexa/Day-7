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
using WpfApp1.Data;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для SelectProduct.xaml
    /// </summary>
    public partial class SelectProduct : Window
    {
        public List<Product> products;
        public SelectProduct(int id)
        {
            InitializeComponent();
            products = trade.Product.ToList();
            FIO.Text = trade.User.Find(id).FullName;
            ListProduct.ItemsSource = products.ToList();
            CountList.Text = products.Count().ToString();
            FilterManufactory.ItemsSource = trade.manufactory.Select(a => a.Name).ToList();
        }
        TradeEntities trade = new TradeEntities();
        public SelectProduct(int id,bool admin)
        {
            InitializeComponent();
            AdminPanel.Visibility = Visibility.Visible;
            products = trade.Product.ToList();
            FIO.Text = trade.User.Find(id).FullName;
            ListProduct.ItemsSource = products.ToList();
            CountList.Text = products.Count().ToString();
            FilterManufactory.ItemsSource = trade.manufactory.Select(a => a.Name).ToList();
        }

        public void JoinMethod()
        {
            filterManufactory();
            Sortprice();
            ListProduct.ItemsSource = products.ToList();
            CountList.Text = products.Count().ToString();
        }

        public void NatureSearch()
        {
            var resultFilter = products.Where(x => x.GetType().GetProperties().
        Any(p => p.GetValue(x, null)?.ToString().ToLower().
        Contains(SearchProduct.Text) ?? false)).ToList();
            ListProduct.ItemsSource = resultFilter.ToList();
            if (resultFilter.Count==0)
            {
                SearchLabel.Visibility = Visibility.Visible;
            }
            else
            {
                SearchLabel.Visibility = Visibility.Hidden;
            }
            CountList.Text = products.Count().ToString();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Product)ListProduct.SelectedItem;
            AddProduct addProduct = new AddProduct(selected.ProductArticleNumber);
            var result = addProduct.ShowDialog();
            if (result.Value == true)
            {
                trade = new TradeEntities();
                ListProduct.ItemsSource = trade.Product.ToList();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addProduct = new AddProduct();
          var result =   addProduct.ShowDialog();
            if(result.Value == true)
            {
                trade = new TradeEntities();
                ListProduct.ItemsSource = trade.Product.ToList();
            }
        }

        private void Drop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var resultmessage = MessageBox.Show("Удаление данного продукта без возратно", "Вы точно хотите удалить?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultmessage == MessageBoxResult.Yes)
                {
                    var selecteditems = (Product)ListProduct.SelectedItem;
                    trade.Product.Remove(selecteditems);
                    trade.SaveChanges();
                    trade = new TradeEntities();
                    ListProduct.ItemsSource = trade.Product.ToList();
                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить запись, так как она связана с заказом");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void Sortprice()
        {
            switch (SortPrice.SelectedIndex)
            {
                case 0:
                    products = trade.Product.OrderByDescending(a => a.ProductCost).ToList();
                    break;
                case 1:
                    products = trade.Product.OrderBy(a => a.ProductCost).ToList();
                    break;
                case 2:
                    products = trade.Product.ToList();
                    break;
                case -1:
                    break;
            }
        }
        private void SortPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JoinMethod();
        }

        public void filterManufactory()
        {
            if (FilterManufactory.SelectedItem != null)
            {
                var select = FilterManufactory.SelectedItem.ToString();
                products = trade.Product.Where(a => a.manufactory.Name == select).ToList();
            }
        }
        private void FilterManufactory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JoinMethod();
        }

        private void SearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            NatureSearch();
        }
    }
}
