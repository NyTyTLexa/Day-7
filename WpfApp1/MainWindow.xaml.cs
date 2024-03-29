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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateCaptcha();
        }
        Data.TradeEntities db = new Data.TradeEntities();
        public void AutorizationRole()
        {
            int id_role = 0;
            try
            {
                var Check = db.User.FirstOrDefault(a => a.UserLogin == Login.Text && (a.UserPassword == PasswordClose.Password || a.UserPassword == PasswordOpen.Text));
                if (Check != null)
                {
                    id_role = Check.UserRole;
                    if (id_role == 1)
                    {
                        SelectProduct laborant = new SelectProduct(Check.UserID,true);
                        laborant.Show();
                        this.Close();
                    }
                    if (id_role == 2)
                    {
                        SelectProduct laborant = new SelectProduct(Check.UserID);
                        laborant.Show();
                        this.Close();
                    }
                    if (id_role == 3)
                    {
                        SelectProduct laborant = new SelectProduct(Check.UserID);
                        laborant.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный ввод данных");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка");
                return;
            }
            MessageBox.Show($"Успешный вход под ролью {db.Role.First(a => a.RoleID == id_role).RoleName}");
        }

        public int CountClick;
        public void Autorization()
        {
            CountClick++;
            if (CountClick > 1)
            {
                if (CreatedCaptcha == Captcha.Text)
                {
                    AutorizationRole();

                }
            }
            else
            {
                AutorizationRole();
            }
            if (CountClick == 1)
            {
                GridNoise.Visibility = Visibility.Visible;
                StackPanel.Visibility = Visibility.Visible;
            }
            if (CountClick == 2)
            {
                this.IsEnabled = false;
                LockedWindows();
                CountClick = 0;
            }
        }
        public int PointTick;
        void LockedWindows()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick1;
            timer.Start();
            PointTick = 0;
            if (PointTick == GlobalConst.LockedtimeSecond)
            {
                timer.Stop();
            }
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            // Обновляем текст Label текущим временем);
            PointTick++;
            if (PointTick == GlobalConst.LockedtimeSecond)
            {
                this.IsEnabled = true;
                this.Title = "Laboratory";
            }
            else
            {
                if (PointTick > 0)
                    this.Title = "Осталось до конца блокировки: " + Convert.ToString(GlobalConst.LockedtimeSecond - PointTick) + " секунд";
            }
        }

        public void CreateNoiseCaptcha()
        {
            var colorLists = new List<System.Windows.Media.Brush>() { System.Windows.Media.Brushes.Red, System.Windows.Media.Brushes.Blue, System.Windows.Media.Brushes.Black, System.Windows.Media.Brushes.Green };
            var random = new Random();
            for (int i = 0; i < random.Next(10, 20); i++)
            {
                var line = new Line();
                line.X1 = 200;
                line.Y1 = random.Next(115, 135);
                line.Y2 = random.Next(115, 135);
                line.Opacity = 0.5;
                line.Stroke = colorLists.GetRange(random.Next(4), 1).First();
                GridNoise.Children.Add(line);
            }
        }

        public string CreatedCaptcha = null;
        public void CreateCaptcha()
        {
            Captcha.Text = GenerateCaptcha();
            CreateNoiseCaptcha();
            CreatedCaptcha = Captcha.Text;
        }

        static string GenerateCaptcha()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] captchaArray = new char[4];

            for (int i = 0; i < captchaArray.Length; i++)
            {
                captchaArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(captchaArray);
        }

        private void UpdateCapcha_Click(object sender, RoutedEventArgs e)
        {
            CreateCaptcha();
        }

        private void AutorizationBT_Click(object sender, RoutedEventArgs e)
        {
            Autorization();
        }
    }
}

