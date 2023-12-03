using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Xml.Linq;

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for ConfirmationPage.xaml
    /// </summary>
    public partial class ConfirmationPage : Page
    {
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private string query = null;
        string connstring = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        public ConfirmationPage(string prediction)
        {
            InitializeComponent();
            cropLabel.Content = prediction;

            Crop crop = Crop.LoadCropByName(prediction);
            growthLabel.Content = crop.GetGrowthPeriod();
            //var idcrop = crop.GetIdCrop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            LoadDataToDatabase();

            Schedule schedulePage = new Schedule(Convert.ToDouble(growthLabel.Content));
            this.NavigationService.Navigate(schedulePage);
        }

        private void LoadDataToDatabase()
        {
            Crop crop = Crop.LoadCropByName(cropLabel.Content.ToString());
            try
            {
                User user = new User(conn);
                int userid = user.GetIdUser();


                conn.Open();
                query = "INSERT INTO user_data (id_user, id_crop) VALUES (@userId, @cropId)";
                cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userid);
                cmd.Parameters.AddWithValue("@cropId", crop.GetIdCrop());
                int result = (int)cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Berhasil menambahkan data ke jadwal!");
                }
                //MessageBox.Show("User ID: " + userid.ToString() + ", Crop ID: " + crop.GetIdCrop().ToString());
            }
            catch(Exception ex)
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
