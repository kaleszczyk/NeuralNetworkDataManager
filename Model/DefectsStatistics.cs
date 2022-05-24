using System;

namespace VI.AOD.NeuralNetworkDataManager.Model
{
    public class DefectsStatistics
    {
        private double normal;
        public double Normal
        {
            get { return normal; }
            set
            {
                if (normal != value)
                {
                    normal = value;
                }
            }
        }

        private double chipped;
        public double Chipped
        {
            get { return chipped; }
            set
            {
                if (chipped != value)
                {
                    chipped = value;
                }
            }
        }

        private double cracked;
        public double Cracked
        {
            get { return cracked; }
            set
            {
                if (cracked != value)
                {
                    cracked = value;
                }
            }
        }

        private double balast;
        public double Balast
        {
            get { return balast; }
            set
            {
                if (balast != value)
                {
                    balast = value;
                }
            }
        }

        public DefectsStatistics()
        {
            normal = 0;
            chipped = 0;
            cracked = 0;
            balast = 0;
        }

        public override string ToString()
        {
            string statistics = string.Empty;
            statistics = $"Normal  - {Math.Round(Normal,2)}\n" +
                         $"Chipped - {Math.Round(Chipped, 2)}\n" +
                         $"Cracked - {Math.Round(Cracked,2)}\n" +
                         $"Balast  - {Math.Round(Balast,2)}\n"; 
            return statistics;     
        }
    }
}
