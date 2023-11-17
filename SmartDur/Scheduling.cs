using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
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

        public double GrowthPeriod
        {
            get { return growthPeriod; }
            set { growthPeriod = value; }
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






    }
}
