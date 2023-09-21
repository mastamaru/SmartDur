using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDur
{
    internal class Crop
    {
        private string name;
        private string description;
        private int growthPeriod;

        public string getName()
        {
            // Note: Need to get name from database
            return name;
        }

        public string getDescription()
        {
            // Note: Need to get description from database
            return description;
        }

        public int getGrowthPeriod()
        {
            // Note: Need to get growth period from database
            return growthPeriod;
        }
    }
}
