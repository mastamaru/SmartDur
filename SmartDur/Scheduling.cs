using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using static SmartDur.Crop;

namespace SmartDur
{
    internal class Scheduling
    {
        //declaring attribute for scheduling
        //private int recommendedAmount;
        //private int fertilizerAmount;

        //private Crop crop;
        //private Fertilizer fertilizer;

        //public int getRecommendedAmount()
        //{
        //  return recommendedAmount;
        //}
        //public int getFertilizerAmount()
        //{
        //return fertilizerAmount;
        //}
        //public Crop getCrop()
        //{
        //return crop;
        //}

        //public Fertilizer getFertilizer() { return fertilizer; }

        private int userId;
        private NpgsqlConnection conn;
        private double growthPeriod;
        private string cropName;
        private string cropDesc;
        private DateTime selectedDate;

        public double GrowthPeriod
        {
            get { return growthPeriod; }
            set { growthPeriod = value; }
        }

        public string CropName
        {
            get { return cropName; }
            set { cropName = value; }
        }

        public string CropDesc
        {
            get { return cropDesc; }
            set { cropDesc = value; }
        }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
            }
        }

        public Scheduling(int userId, NpgsqlConnection conn)
        {
            this.userId = userId;
            this.conn = conn;
        }

        public void GetUserGrowthPeriod()
        {
            //double growthPeriod = 0.0;

            try
            {
                conn.Open();
                using(var cmd = new NpgsqlCommand("SELECT id_crop FROM user_data WHERE id_user = @userId", conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    int cropId = Convert.ToInt32(cmd.ExecuteScalar());

                    using(var growthPeriodCmd = new NpgsqlCommand("SELECT growth_period FROM crop WHERE id_crop = @idCrop", conn))
                    {
                        growthPeriodCmd.Parameters.AddWithValue("idCrop", cropId);
                        object result = growthPeriodCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            growthPeriod = Convert.ToDouble(result);
                            //MessageBox.Show("Growth Period " + growthPeriod.ToString());
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Getting Growth Period" + ex);
            }
            finally
            {
                conn.Close();
            }

        }

        public void GetUserCropName()
        {
            //double growthPeriod = 0.0;

            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id_crop FROM user_data WHERE id_user = @userId", conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    int cropId = Convert.ToInt32(cmd.ExecuteScalar());

                    using (var nameCmd = new NpgsqlCommand("SELECT name FROM crop WHERE id_crop = @idCrop", conn))
                    {
                        nameCmd.Parameters.AddWithValue("idCrop", cropId);
                        object result = nameCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            cropName = Convert.ToString(result);
                            //MessageBox.Show("Growth Period " + growthPeriod.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Getting Crop Name" + ex);
            }
            finally
            {
                conn.Close();
            }

        }

        public void GetUserCropDesc()
        {
            //double growthPeriod = 0.0;

            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT id_crop FROM user_data WHERE id_user = @userId", conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    int cropId = Convert.ToInt32(cmd.ExecuteScalar());

                    using (var nameCmd = new NpgsqlCommand("SELECT description FROM crop WHERE id_crop = @idCrop", conn))
                    {
                        nameCmd.Parameters.AddWithValue("idCrop", cropId);
                        object result = nameCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            cropDesc = Convert.ToString(result);
                            //MessageBox.Show("Growth Period " + growthPeriod.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Getting Crop Description" + ex);
            }
            finally
            {
                conn.Close();
            }

        }

        public void InsertPlantingAndHarvestDates(DateTime plantingDate, DateTime harvestDate)
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO scheduling (id_user, planting_date, harvest_date) VALUES (@userId, @plantingDate, @harvestDate)", conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId); // Add id_user value
                    cmd.Parameters.AddWithValue("plantingDate", plantingDate);
                    cmd.Parameters.AddWithValue("harvestDate", harvestDate);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Planting and harvest dates inserted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert dates.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting dates: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        public bool CheckUserDatesExist()
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT planting_date, harvest_date FROM scheduling WHERE id_user = @userId", conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Dates exist for the user, retrieve and use them
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                            {
                                selectedDate = reader.GetDateTime(0);
                                growthPeriod = (reader.GetDateTime(1) - selectedDate).TotalDays;
                                return true; // Dates found
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking user dates: " + ex.Message);
            }
            finally { conn.Close(); }

            return false; // Dates not found
        }
    }
}
