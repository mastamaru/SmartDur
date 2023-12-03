using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
using System.IO;
using Newtonsoft.Json;

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for InputPage.xaml
    /// </summary>
    public partial class InputPage : Page
    {
        public InputPage()
        {
            InitializeComponent();

            PopulateLocationComboBox();
            PopulateFertilizerComboBox();
        }

        private void PopulateLocationComboBox()
        {
            // Create a list of Indonesian provinces.
            List<string> provinces = new List<string>
            {
                "Aceh",
                "Bali",
                "Banten",
                "Bengkulu",
                "Gorontalo",
                "Jakarta",
                "Jambi",
                "Jawa Barat",
                "Jawa Tengah",
                "Jawa Timur",
                "Kalimantan Barat",
                "Kalimantan Selatan",
                "Kalimantan Tengah",
                "Kalimantan Timur",
                "Kalimantan Utara",
                "Kepulauan Bangka Belitung",
                "Kepulauan Riau",
                "Lampung",
                "Maluku",
                "Maluku Utara",
                "Nusa Tenggara Barat",
                "Nusa Tenggara Timur",
                "Papua",
                "Papua Barat",
                "Riau",
                "Sulawesi Barat",
                "Sulawesi Selatan",
                "Sulawesi Tengah",
                "Sulawesi Tenggara",
                "Sulawesi Utara",
                "Sumatera Barat",
                "Sumatera Selatan",
                "Sumatera Utara",
                "Yogyakarta"
            };

            // Bind the list of provinces to the ComboBox.
            locationBox.ItemsSource = provinces;
        }

        private (int N, int P, int K) CalculateNutrientValues(string selectedLocation, string selectedFertilizer)
        {
            // Load all fertilizer data
            List<Fertilizer> fertilizers = Fertilizer.LoadAllFertilizers();

            // Create a dictionary to store fertilizer composition based on their names
            Dictionary<string, (int N, int P, int K)> fertilizerData = new Dictionary<string, (int N, int P, int K)>();

            foreach (var fertilizer in fertilizers)
            {
                fertilizerData[fertilizer.Name] = (Convert.ToInt32(fertilizer.Nitrogen), Convert.ToInt32(fertilizer.Phosphor), Convert.ToInt32(fertilizer.Potassium));
            }

            // Sample data mapping locations to N, P, and K values
            var locationNutrientData = new Dictionary<string, (int N, int P, int K)>
            {
                { "Aceh", (70, 40, 55) },
                { "Bali", (90, 60, 70) },
                { "Banten", (65, 35, 50) },
                { "Bengkulu", (75, 45, 60) },
                { "Gorontalo", (70, 50, 55) },
                { "Jakarta", (80, 55, 65) },
                { "Jambi", (75, 40, 60) },
                { "Jawa Barat", (70, 45, 55) },
                { "Jawa Tengah", (75, 50, 60) },
                { "Jawa Timur", (80, 55, 65) },
                { "Kalimantan Barat", (70, 40, 55) },
                { "Kalimantan Selatan", (75, 45, 60) },
                { "Kalimantan Tengah", (70, 40, 55) },
                { "Kalimantan Timur", (80, 55, 65) },
                { "Kalimantan Utara", (75, 45, 60) },
                { "Kepulauan Bangka Belitung", (65, 35, 50) },
                { "Kepulauan Riau", (70, 40, 55) },
                { "Lampung", (75, 45, 60) },
                { "Maluku", (80, 55, 65) },
                { "Maluku Utara", (70, 40, 55) },
                { "Nusa Tenggara Barat", (75, 45, 60) },
                { "Nusa Tenggara Timur", (80, 55, 65) },
                { "Papua", (70, 40, 55) },
                { "Papua Barat", (75, 45, 60) },
                { "Riau", (80, 55, 65) },
                { "Sulawesi Barat", (70, 40, 55) },
                { "Sulawesi Selatan", (75, 45, 60) },
                { "Sulawesi Tengah", (80, 55, 65) },
                { "Sulawesi Tenggara", (80, 50, 60) },
                { "Sulawesi Utara", (70, 40, 55) },
                { "Sumatera Barat", (75, 45, 60) },
                { "Sumatera Selatan", (80, 55, 65) },
                { "Sumatera Utara", (70, 40, 55) },
                { "Yogyakarta", (75, 45, 60) }
            };


            // Get N, P, K values based on user selections
            var locationValues = locationNutrientData.GetValueOrDefault(selectedLocation, (0, 0, 0));
            var fertilizerValues = fertilizerData.GetValueOrDefault(selectedFertilizer, (0, 0, 0));

            // Calculate the total N, P, K values
            int totalN = locationValues.Item1 + fertilizerValues.Item1;
            int totalP = locationValues.Item2 + fertilizerValues.Item2;
            int totalK = locationValues.Item3 + fertilizerValues.Item3;

            return (totalN, totalP, totalK);
        }



        private void PopulateFertilizerComboBox()
        {
            List<Fertilizer> fertilizers = Fertilizer.LoadAllFertilizers();

            // Bind the list of names to the ComboBox (fertilizerBox).
            fertilizerBox.ItemsSource = fertilizers.Select(f => f.Name).ToList();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void fertilizerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var root = Directory.GetCurrentDirectory();
            var dotenv = System.IO.Path.Combine(root, ".env");
            DotEnv.Load(dotenv);
            string ml_backend = Environment.GetEnvironmentVariable("ML_Backend");

            string selectedLocation = (string)locationBox.SelectedItem;
            string selectedFertilizer = (string)fertilizerBox.SelectedItem;

            // Calculate N, P, K values
            var (totalN, totalP, totalK) = CalculateNutrientValues(selectedLocation, selectedFertilizer);

            // Create the JSON payload
            var payload = new
            {
                N = totalN,
                P = totalP,
                K = totalK,
                location = selectedLocation
            };

            // Convert the payload to JSON
            string jsonPayload = JsonConvert.SerializeObject(payload);

            // Send the JSON data to your backend API using HttpClient
            // Replace "yourApiUrl" with the actual URL of your backend API
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ml_backend);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send the JSON payload to the API
                HttpResponseMessage response = client.PostAsync("/predict", new StringContent(jsonPayload, Encoding.UTF8, "application/json")).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Handle a successful response
                    string responseContent = response.Content.ReadAsStringAsync().Result;

                    // Deserialize the JSON response
                    dynamic responseJson = JsonConvert.DeserializeObject(responseContent);
                    string prediction = responseJson.prediction;

                    // Pass the prediction data to the ConfirmationPage
                    ConfirmationPage confirmationPage = new ConfirmationPage(prediction);
                    this.NavigationService.Navigate(confirmationPage);
                }
                else
                {
                    // Handle an error response
                    MessageBox.Show("An error occurred while sending data to the backend.");
                }
            }
        }
    }
}
