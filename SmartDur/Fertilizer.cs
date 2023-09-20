using System;
using System.Collections.Generic;
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
        private double phospor;
        private double kalium;

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

        public (double, double, double) getComposition()
        {
            // Note: Need to get fertilizer composition from database
            return (nitrogen, phospor, kalium); 
        }
    }
}
