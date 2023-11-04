using Npgsql;
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
using System.Configuration;

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private NpgsqlConnection conn;
        private string connstring = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        private string sqlquery;
        NpgsqlCommand cmd;
        public Register()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = pbPassword.Password.ToString();
            try
            {
                conn.Open();
                string sqlquery = @"SELECT * FROM insert_pengguna(:_username, :_password)";

                cmd = new NpgsqlCommand(sqlquery, conn);
                // Tambahkan parameter yang diperlukan
                cmd.Parameters.AddWithValue("_username", username);
                cmd.Parameters.AddWithValue("_password", password);

                // Eksekusi perintah dan ambil hasilnya
                int result = (int)cmd.ExecuteScalar();

                // Periksa hasil
                if (result == 1)
                {
                    MessageBox.Show("Data berhasil dimasukkan!");
                }
                else
                {
                    MessageBox.Show("Data gagal dimasukkan!");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
