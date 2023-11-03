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
using Npgsql;

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=5432;Username=postgres;Password=postgres123;Database=SmartDur";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new NpgsqlConnection(connstring);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            Admin admin = new Admin(conn);
            User user = new User(conn);
            if (admin.Login(username, password))
            {
                // TODO: Navigasi ke halaman admin
                MessageBox.Show("Selamat datang " + username + "!!");
                AdminMenu adminmenu = new AdminMenu();
                this.Close();
                adminmenu.Show();
            }
            else if (user.Login(username, password))
            {
                MessageBox.Show("Selamat datang " + username + "!!");
                UserMenu userMenu = new UserMenu();
                this.Close();
                userMenu.Show();
            }
            else { 
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
    }
}
