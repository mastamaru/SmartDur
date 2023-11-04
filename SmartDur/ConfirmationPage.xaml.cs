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

namespace SmartDur
{
    /// <summary>
    /// Interaction logic for ConfirmationPage.xaml
    /// </summary>
    public partial class ConfirmationPage : Page
    {
        public ConfirmationPage(string prediction)
        {
            InitializeComponent();
            cropLabel.Content = prediction;

            Crop crop = Crop.LoadCropByName(prediction);
            growthLabel.Content = crop.GetGrowthPeriod();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Schedule schedulePage = new Schedule();
            this.NavigationService.Navigate(schedulePage);
        }
    }
}
