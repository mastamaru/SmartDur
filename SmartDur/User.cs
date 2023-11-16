using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
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
        private NpgsqlConnection conn;
        
        public User(String _username, String _password)
        {
            this.username = _username;
            this.password = _password;
        }

        public User(NpgsqlConnection connection)
        {
            this.conn = connection;
        }

        private static string loggedInUser; 

        public static string LoggedInUser
        {
            get { return loggedInUser; }
        }
        public bool Login(string username, string password)
        {
            try
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT password FROM pengguna WHERE username = @username", conn))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    var result = cmd.ExecuteScalar()?.ToString();

                    if (result != null && result == password)
                    {
                        loggedInUser = username;
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // Exception handling, log it as needed
            }
            finally
            {
                conn.Close();
            }
            return false;
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

        public static bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(loggedInUser); 
        }
    }
}
