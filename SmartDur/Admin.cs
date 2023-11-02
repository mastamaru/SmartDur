using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartDur
{
    internal class Admin
    {
        private String username;
        private String password;
        private NpgsqlConnection conn;

        public String getUsername()
        {
            return username;
        }

        public String getPassword()
        {
            return password;
        }

        public Admin(NpgsqlConnection connection)
        {
            this.conn = connection;
        }

        public bool Login(string username, string password)
        {
            try
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT password FROM admin WHERE username = @username", conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    var result = cmd.ExecuteScalar()?.ToString();

                    if (result != null && result == password)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Exception handling, log it as needed
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
      
        public void modifyDatabase() { }
    }
}
