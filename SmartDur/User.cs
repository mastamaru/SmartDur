using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

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

        private static int loggedInUserId;
        public static int LoggedInUserId
        {
            get { return loggedInUserId; }
        }

        public int GetIdUser()
        {
            
            return loggedInUserId;
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
                        GetUserLoginId(username);
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

        public void GetUserLoginId(string username)
        {

            try
            {
                

                using (var cmd = new NpgsqlCommand("SELECT id_user FROM pengguna WHERE username = @username", conn))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    var result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        loggedInUserId = Convert.ToInt32(result);
                        MessageBox.Show("User ID Retrieved: " + loggedInUserId); // Output for debugging
                    }
                    else
                    {
                        MessageBox.Show("User ID not found for username: " + username);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving user ID: " + ex.Message); // Output for debugging
                                                                              // Handle the exception, log it, or raise an alert as needed
            }
            //return !string.IsNullOrEmpty(loggedInUser);
            //return 0;
        }
    }
}
