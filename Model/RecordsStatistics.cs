using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VI.AOD.NeuralNetworkDataManager.Model
{
    public class RecordsStatistics : ViewModelBase
    {
        public RecordsStatistics()
        { }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {

                if (fileName != value)
                {
                    fileName = value;
                    RaisePropertyChanged(nameof(FileName));
                }
            }
        }
        private int imagesCount;
        public int ImagesCount
        {
            get
            {
                return imagesCount;
            }
            set
            {
                if(imagesCount!=value)
                {
                    imagesCount = value;
                }
            }
        }

        public string FileFullPath { get; set; } = "NOT SET";

        private double normal = 0;
        public double Normal
        {
            get { return normal; }
            set
            {
                if (normal != value)
                {
                    normal = value;
                    RaisePropertyChanged(nameof(Normal));
                }
            }
        }
        private double chipped = 0;
        public double Chipped
        {
            get { return chipped; }
            set
            {
                if (chipped != value)
                {
                    chipped = value;
                    RaisePropertyChanged(nameof(Chipped));
                }
            }
        }

        private double criticalChipped = 0;
        public double CriticalChipped
        {
            get { return criticalChipped; }
            set
            {
                if (criticalChipped != value)
                {
                    criticalChipped = value;
                    RaisePropertyChanged(nameof(CriticalChipped));
                }
            }
        }

        private double cracks = 0;
        public double Cracks
        {
            get { return cracks; }
            set
            {
                if (cracks != value)
                {
                    cracks = value;
                    RaisePropertyChanged(nameof(Cracks));
                }
            }
        }

        private double criticalCracks;
        public double CriticalCracks
        {
            get { return criticalCracks; }
            set
            {
                if (criticalCracks != value)
                {
                    criticalCracks = value;
                    RaisePropertyChanged(nameof(CriticalCracks));
                }
            }
        }

        private double ballastFull = 0;
        public double BallastFull
        {
            get { return ballastFull; }
            set
            {
                if (ballastFull != value)
                {
                    ballastFull = value;
                    RaisePropertyChanged(nameof(BallastFull));
                }
            }
        }

        private double ballastPartial = 0;
        public double BallastPartial
        {
            get
            {
                return ballastPartial;
            }
            set
            {
                if (ballastPartial != value)
                {
                    ballastPartial = value;
                    RaisePropertyChanged(nameof(BallastPartial));
                }
            }
        }

        private double skewed = 0;
        public double Skewed
        {
            get
            {
                return skewed;
            }
            set
            {
                if (skewed != value)
                {
                    skewed = value;
                    RaisePropertyChanged(nameof(Skewed));
                }
            }
        }

        private double criticalSkewed = 0;
        public double CriticalSkewed
        {
            get
            {
                return criticalSkewed;
            }
            set
            {
                if (criticalSkewed != value)
                {
                    criticalSkewed = value;
                    RaisePropertyChanged(nameof(CriticalSkewed));
                }
            }
        }

        private double turnout = 0;
        public double Turnout
        {
            get
            {
                return turnout;
            }
            set
            {
                if (turnout != value)
                {
                    turnout = value;
                    RaisePropertyChanged(nameof(Turnout));
                }
            }
        }

        private double flipped = 0;
        public double Flipped
        {
            get
            {
                return flipped;
            }
            set
            {
                if (flipped != value)
                {
                    flipped = value;
                    RaisePropertyChanged(nameof(Flipped));
                }
            }
        }

        private double successfulSegmentation = 0;
        public double SuccessfulSegmentation
        {
            get
            {
                return successfulSegmentation;
            }
            set
            {
                if (successfulSegmentation != value)
                {
                    successfulSegmentation = value;
                    RaisePropertyChanged(nameof(SuccessfulSegmentation));
                }
            }
        }

        private double notSet = 0;
        public double NotSet
        {
            get { return notSet; }
            set
            {
                if (notSet != value)
                {
                    notSet = value;
                    RaisePropertyChanged(nameof(NotSet));
                }
            }
        }
    }
}
