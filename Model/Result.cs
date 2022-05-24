using System;
using VI.AOD.NeuralNetworkDataManager.Enums;

namespace VI.AOD.NeuralNetworkDataManager.Model
{
    public class Result
    {
        private int tn;
        public int TN
        {
            get { return tn; }
            set
            {
                if (tn != value)
                {
                    tn = value;
                }
            }
        }

        private int fp;
        public int FP
        {
            get { return fp; }
            set
            {
                if (fp != value)
                {
                    fp = value;
                }
            }
        }

        private int fn;
        public int FN
        {
            get { return fn; }
            set
            {
                if (fn != value)
                {
                    fn = value;
                }
            }
        }

        private int tp;
        public int TP
        {
            get { return tp; }
            set
            {
                if (tp != value)
                {
                    tp = value;
                }
            }
        }

        public Result()
        {
            tn = 0;
            tp = 0;
            fn = 0;
            fp = 0;
        }


        public void GetResult(ResultType result)
        {
            switch (result)
            {
                case ResultType.TN:
                    TN++;
                    break;
                case ResultType.FP:
                    FP++;
                    break;
                case ResultType.FN:
                    FN++;
                    break;
                case ResultType.TP:
                    TP++;
                    break;
            }
        }

        public override string ToString()
        {
            string result = string.Empty;
            result = $"TN {TN}\n" +
                     $"FP {FP}\n" +
                     $"FN {FN}\n" +
                     $"TP {TP}\n" +
                     $"Sensitivty {Math.Round((float)TP / (TP + FN),3) }\n" +
                     $"Precision {Math.Round((float)TP / (TP + FP), 3)}\n" +
                     $"Specificity {Math.Round((float)TN / (TN + FP), 3)}\n";

            return result;
        }
    }
}
