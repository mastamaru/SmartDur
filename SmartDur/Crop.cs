using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDur
{
    internal class Crop
    {
        private string name;
        private string description;
        private double growthPeriod;

        public Crop(string name, string description, double growthPeriod)
        {
            this.name = name;
            this.description = description;
            this.growthPeriod = growthPeriod;
        }

        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }

        public double GetGrowthPeriod()
        {
            return growthPeriod;
        }

        public static Crop LoadCropByName(string cropName)
        {
            var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var dotenv = System.IO.Path.Combine(root, ".env");
            DotEnv.Load(dotenv);
            string host = Environment.GetEnvironmentVariable("Host");
            string port = Environment.GetEnvironmentVariable("Port");
            string username = Environment.GetEnvironmentVariable("Username");
            string password = Environment.GetEnvironmentVariable("Password");
            string database = Environment.GetEnvironmentVariable("Database");

            string connectionString = string.Format("Host={0};Port={1};Username={2};Password={3};Database={4}", host, port, username, password, database);

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT name, description, growth_period FROM crop WHERE name = @cropName";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("cropName", cropName);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader.GetString(0);
                            string description = reader.GetString(1);
                            double growthPeriod = reader.GetDouble(2);

                            return new Crop(name, description, growthPeriod);
                        }
                    }
                }
            }

            return null; // Crop not found
        }
    }
}
