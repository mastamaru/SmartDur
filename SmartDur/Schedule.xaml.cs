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

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for Schedule.xaml
    /// </summary>
    /// Scheduler
    public partial class Schedule : Page
    {

        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        //private string query = null;
        string connstring = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
        double growthPeriod = 0.0;
        //private DateIsInRangeConverter converter = new DateIsInRangeConverter();

        public Schedule()
        {   
            InitializeComponent();

            // Attach the SelectedDateChanged event handler
            datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;

            //Crop crop = Crop.LoadCropByName(prediction);
            // growthLabel.Content = crop.GetGrowthPeriod();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected date from the DatePicker
            if (datePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = datePicker.SelectedDate.Value;

                // Set the selected date in the Calendar
                calendar.SelectedDate = selectedDate;
                calendar.DisplayDate = selectedDate;

                conn = new NpgsqlConnection(connstring);

                User user = new User(conn);
                int userid = user.GetIdUser();

                Scheduling scheduling = new Scheduling(userid, conn);
                scheduling.GetUserGrowthPeriod();
                growthPeriod = scheduling.GrowthPeriod;

                // Calculate the end date based on the growth period
                DateTime endDate = selectedDate.AddDays(growthPeriod);


                // Set the date range in the converter
                DateIsInRangeConverter converter = (DateIsInRangeConverter)Resources["DateIsInRangeConverter"];
                converter.StartDate = selectedDate;
                converter.EndDate = endDate;

                MessageBox.Show(converter.EndDate.ToString());
                //MessageBox.Show(endDate.ToString());


            }

            
        }




    }
}
