using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SmartDur
{
    internal class User
    {
        private String username;
        private String password;
        private String location;
        private String selectedFertilizer;
        private double landArea;
        private String selectedCrop;
        private int selectedFertilizerAmount;
        
        public User(String _username, String _password)
        {
            this.username = _username;
            this.password = _password;
        }

        public void login()
        {

        }
        
        public void selectLocation()
        {

        }

        public void selectFertilizer()
        {

        }

        public void enterLandArea()
        {

        }

        public List<string> RecommendCrop()
        {
            // Implement crop recommendation logic here and return a List<string>.
            return new List<string>();
        }

        public int selectFertilizerAmount()
        {
            return selectedFertilizerAmount;
        }

     //   public int estimateHarvest()
     //   {
     //       return
     //   }


        public void goToScheduling()
        {

        }
    }
}
