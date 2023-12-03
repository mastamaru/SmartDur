using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Npgsql;

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NpgsqlConnection conn;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var root = Directory.GetCurrentDirectory();
            var dotenv = System.IO.Path.Combine(root, ".env");
            DotEnv.Load(dotenv);
            string host = Environment.GetEnvironmentVariable("Host");
            string port = Environment.GetEnvironmentVariable("Port");
            string username = Environment.GetEnvironmentVariable("Username");
            string password = Environment.GetEnvironmentVariable("Password");
            string database = Environment.GetEnvironmentVariable("Database");

            string connstring = string.Format("Host={0};Port={1};Username={2};Password={3};Database={4}", host, port, username, password, database);

            conn = new NpgsqlConnection(connstring);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();

            Admin admin = new Admin(conn);
            User user = new User(conn);

            if (admin.Login(username, password))
            {
                // TODO: Navigasi ke halaman admin
                MessageBox.Show("Selamat datang " + username + "!!");
                AdminMenu adminmenu = new AdminMenu();
                FertilizerPage fertilizerPage = new FertilizerPage();
                this.Close();
                //adminmenu.Show();
                fertilizerPage.Show();
            }
            else if (user.Login(username, password))
            {
                int userId = user.GetIdUser();
                if(user.CheckUserDataExist(userId))
                {
                    MessageBox.Show("Selamat datang " + username + "!!" + "\n Mengarahkan ke Pejadwalan");
                    Schedule schedulePage = new Schedule();
                    //this.Close();
                    MainFrame.NavigationService.Navigate(schedulePage);
                    //schedulePage.Show();
                }
                else
                {
                    MessageBox.Show("Selamat datang " + username + "!!");
                    UserMenu userMenu = new UserMenu();
                    this.Close();
                    userMenu.Show();
                }

                //MessageBox.Show("Selamat datang " + username + "!!");
                //UserMenu userMenu = new UserMenu();
                //this.Close();
                //userMenu.Show();
            }
            else
            {
                MessageBox.Show("Username / Password Anda salah.");
            }

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPassword_TextInput(object sender, TextCompositionEventArgs e)
        {

        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            this.Close();
            register.Show();
        }

    }
}
