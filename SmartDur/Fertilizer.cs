using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDur
{
    internal class Fertilizer
    {
        private string name;
        private string description;
        private double nitrogen;
        private double phosphor;
        private double potassium;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public double Nitrogen
        {
            get { return nitrogen; }
            set { nitrogen = value; }
        }

        public double Phosphor
        {
            get { return phosphor; }
            set { phosphor = value; }
        }

        public double Potassium
        {
            get { return potassium; }
            set { potassium = value; }
        }

        public static List<Fertilizer> LoadAllFertilizers()
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

            List<Fertilizer> fertilizers = new List<Fertilizer>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT name, description, nitrogen, phosphor, kalium FROM fertilizer";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Fertilizer fertilizer = new Fertilizer
                        {
                            Name = reader.GetString(0),
                            Description = reader.GetString(1),
                            Nitrogen = reader.GetDouble(2),
                            Phosphor = reader.GetDouble(3),
                            Potassium = reader.GetDouble(4)
                        };
                        fertilizers.Add(fertilizer);
                    }
                }
            }

            return fertilizers;
        }
    }
}
