using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDur
{
    internal class Scheduling
    {
        //declaring attribute for scheduling
        private int recommendedAmount;
        private int fertilizerAmount;

        private Crop crop;
        private Fertilizer fertilizer;

        public int getRecommendedAmount()
        {
            return recommendedAmount;
        }
        public int getFertilizerAmount()
        {
            return fertilizerAmount;
        }
        public Crop getCrop()
        {
            return crop;
        }

        public Fertilizer getFertilizer() { return fertilizer; }
    }
}
