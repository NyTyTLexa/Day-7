using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
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
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        TradeEntities db = new TradeEntities();
        public AddProduct()
        {
            InitializeComponent();
            UniqueArticle();
            Category.ItemsSource = db.Category.Select(a => a.name).ToList();
            Manufactory.ItemsSource = db.manufactory.Select(a => a.Name).ToList();
            Unit.ItemsSource = db.Unit.Select(a => a.name).ToList();
        }
        public AddProduct(string article)
        {
            InitializeComponent();
            Category.ItemsSource = db.Category.Select(a => a.name).ToList();
            Manufactory.ItemsSource = db.manufactory.Select(a => a.Name).ToList();
            Unit.ItemsSource = db.Unit.Select(a => a.name).ToList();
            Article.Text = article;
            var Product = db.Product.First(a => a.ProductArticleNumber == article);
            Name.Text = Product.ProductName;
            Description.Text = Product.ProductDescription;
            Category.Text = db.Category.First(a => a.id == Product.ProductCategory).name;
            Manufactory.Text = db.manufactory.First(a => a.id == Product.ProductManufacturer).Name;
            Price.Text = Product.ProductCost.ToString();
            Count.Text = Product.ProductDiscountAmount.ToString();
            Unit.Text = db.Unit.First(a => a.id == Product.ProductUnity).name;
            Picture.Source = Product.PictureProduct;
            NamePicture.Text = Product.ProductPhotoPath;
        }

        public void UniqueArticle()
        {
            var article = GenerateArticle();
            if (db.Product.Any(a => a.ProductArticleNumber == article) == false)
            {
                Article.Text = article;
            }
            else
            {
                UniqueArticle();
            }
        }

        static string GenerateArticle()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] captchaArray = new char[6];

            for (int i = 0; i < captchaArray.Length; i++)
            {
                captchaArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(captchaArray);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool IsNumberTextBlock(TextBox textBlock)
        {
            if (double.TryParse(textBlock.Text, out double number))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool NegativeTextBlock(TextBox textBlock)
        {
            if (Convert.ToInt32(textBlock.Text) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool EmptyTextblock(string text)
        {
            if (text == string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (EmptyTextblock(Name.Text) || EmptyTextblock(Description.Text) || EmptyTextblock(Category.Text)
                 || EmptyTextblock(Manufactory.Text) || EmptyTextblock(Price.Text) || EmptyTextblock(Count.Text)
                 || EmptyTextblock(Unit.Text))
            {
                MessageBox.Show("Необходимо заполнить все поля");
                return;
            }
            if (NegativeTextBlock(Price))
            {
                MessageBox.Show("Цена не может \n быть отрицательным числом");
                return;
            }
            if (IsNumberTextBlock(Price))
            {
                MessageBox.Show("Цена должно быть числом");
                return;
            }
            if (NegativeTextBlock(Count))
            {
                MessageBox.Show("Количество не может \n быть отрицательным числом");
                return;
            }
            if (IsNumberTextBlock(Count))
            {
                MessageBox.Show("Количество должно быть числом");
                return;
            }
            Product product = new Product();
            product.ProductArticleNumber = Article.Text;
            product.ProductName = Name.Text;
            product.ProductDescription = Description.Text;
            product.ProductCategory = db.Category.First(a => a.name == Category.Text).id;
            product.ProductManufacturer = db.manufactory.First(a => a.Name == Manufactory.Text).id;
            product.ProductCost = Convert.ToDouble(Price.Text);
            product.ProductDiscountAmount = Convert.ToInt32(Count.Text);
            product.ProductUnity = db.Unit.First(a => a.name == Unit.Text).id;
            product.ProductPhotoPath = NamePicture.Text;
            db.Product.AddOrUpdate(product);
            db.SaveChanges();
            this.DialogResult = true;
        }

        string FilePath = null;
        string FileName = null;
        string ShortFileName = null;
        string Expansion = null;
        string DestinationPath = null;

        public bool CheckWidthHeight(string imagePath)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;
            if(width<=300||height<=200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void LoadPicture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    FileName = openFileDialog.FileName;
                    if (CheckWidthHeight(FileName))
                    {
                        DestinationPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                        "Picture", System.IO.Path.GetFileName(FileName));
                        ShortFileName = System.IO.Path.GetFileNameWithoutExtension(FileName);
                        Expansion = System.IO.Path.GetExtension(FileName);
                        FilePath = $@"Picture{ShortFileName}{Expansion}";
                        NamePicture.Text = ShortFileName+Expansion;
                        if (FileName != DestinationPath)
                        {
                            File.Copy(FileName, DestinationPath, true);
                            Picture.Source = new BitmapImage(new Uri(FileName));
                        }
                        else
                        {
                            Picture.Source = new BitmapImage(new Uri(DestinationPath));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Необходимо выбрать картинку 200х300");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка получения фотографии.", ex.Message,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
            
       

