using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Linq;
using System.Configuration;

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for Fertilizer.xaml
    /// </summary>
    public partial class FertilizerPage : Window
    {
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private string query = null;
        string connstring = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        DataRowView r;

        public FertilizerPage()
        {
            InitializeComponent();

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            LoadDataFromDatabase();

        }
        private void LoadDataFromDatabase()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                // Buat command untuk eksekusi query
                cmd = new NpgsqlCommand("SELECT * FROM Fertilizer", conn);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);

                // Set ItemsSource dari DataGrid dengan DataTable
                dgTabel.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                // Membuka koneksi
                conn.Open();
                query = @"INSERT INTO Fertilizer (name, description, nitrogen, phosphor, kalium) VALUES (@name, @description, @nitrogen, @phosphor, @kalium)";


                // Membuat command
                cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@description", txtDesc.Text);
                cmd.Parameters.AddWithValue("@nitrogen", Convert.ToInt32(txtNitrogen.Text)); // Diasumsikan nitrogen adalah integer
                cmd.Parameters.AddWithValue("@phosphor", Convert.ToInt32(txtPhosphor.Text)); // Diasumsikan phosphor adalah integer
                cmd.Parameters.AddWithValue("@kalium", Convert.ToInt32(txtKalium.Text)); // Diasumsikan kalium adalah 


                int result = (int)cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Berhasil menambahkan data pupuk baru!");
                    txtKalium.Text = txtDesc.Text = txtName.Text = txtPhosphor.Text = txtNitrogen.Text = null;
                    conn.Close();
                    LoadDataFromDatabase();
                }
            }
            catch (Exception ex)
            {
                // Menampilkan pesan error jika ada
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Tutup koneksi
                // Hanya menutup koneksi jika koneksi masih terbuka
                conn.Close();
            }
        }

        private void dgTabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTabel.SelectedItem == null) return;

            DataRowView row = (DataRowView)dgTabel.SelectedItem;
            txtName.Text = row["Name"].ToString();
            txtDesc.Text = row["Description"].ToString();
            txtNitrogen.Text = row["Nitrogen"].ToString();
            txtKalium.Text = row["Kalium"].ToString();
            txtPhosphor.Text = row["Phosphor"].ToString();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = (DataRowView)dgTabel.SelectedItem;
            if (selectedRow.Row == null)
            {
                MessageBox.Show("Pilih baris yang akan anda ubah!", "Good!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                conn.Open();
                var key = selectedRow.Row[0].ToString();
                query = "UPDATE Fertilizer SET name = @name, description = @description, nitrogen = @nitrogen, phosphor = @phosphor, kalium = @kalium WHERE id_fertilizer = @idfertilizer";
                MessageBox.Show(key);
                cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@description", txtDesc.Text);
                cmd.Parameters.AddWithValue("@nitrogen", Convert.ToInt32(txtNitrogen.Text));
                cmd.Parameters.AddWithValue("@phosphor", Convert.ToInt32(txtPhosphor.Text));
                cmd.Parameters.AddWithValue("@kalium", Convert.ToInt32(txtKalium.Text));
                cmd.Parameters.AddWithValue("@idfertilizer", Convert.ToInt32(key));
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                    conn.Close();
                    LoadDataFromDatabase();
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Data gagal diperbarui. Cek lagi input Anda!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = (DataRowView)dgTabel.SelectedItem;
            if (selectedRow == null)
            {
                MessageBox.Show("Pilih baris yang akan Anda hapus!");
                return;
            }
            try
            {
                conn.Open();
                var key = selectedRow.Row[0].ToString();

                query = "DELETE FROM Fertilizer WHERE id_fertilizer = @id";
                cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(key));

                if ((int)cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Pupuk berhasil dihapus", "Great Job!", MessageBoxButton.OK, MessageBoxImage.Information);
                    conn.Close();
                    LoadDataFromDatabase();

                    txtKalium.Text = txtDesc.Text = txtName.Text = txtPhosphor.Text = txtNitrogen.Text = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
