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
    public class Record : ViewModelBase
    {
        public Record()
        {
            SleeperNo = -1;
            SleeperParent = -1;
            successfulSegmentation = true;
        }
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {

                if (fileName != value)
                {
                    fileName = value;
                    SetFrameNo();
                    RaisePropertyChanged(nameof(FileName));
                }
            }
        }
        private int frameNo;
        public int FrameNo { get { return frameNo; } }

        private int session;
        public int Session { get { return session; } }
        public string FileFullPath { get; set; } = "NOT SET";

        private bool normal = false;
        public bool Normal
        {
            get { return normal; }
            set
            {
                if (normal != value)
                {
                    normal = value;
                    RaisePropertyChanged(nameof(Normal));
                    CheckSet();
                }
            }
        }
        private bool chipped = false;
        public bool Chipped
        {
            get { return chipped; }
            set
            {
                if (chipped != value)
                {
                    chipped = value;
                    RaisePropertyChanged(nameof(Chipped));
                    CheckSet();
                }
            }
        }

        private bool criticalChipped = false;
        public bool CriticalChipped
        {
            get { return criticalChipped; }
            set
            {
                if (criticalChipped != value)
                {
                    criticalChipped = value;
                    RaisePropertyChanged(nameof(CriticalChipped));
                    CheckSet();
                }
            }
        }

        private bool cracks = false;
        public bool Cracks
        {
            get { return cracks; }
            set
            {
                if (cracks != value)
                {
                    cracks = value;
                    RaisePropertyChanged(nameof(Cracks));
                    CheckSet();
                }
            }
        }

        private bool criticalCracks;
        public bool CriticalCracks
        {
            get { return criticalCracks; }
            set
            {
                if (criticalCracks != value)
                {
                    criticalCracks = value;
                    RaisePropertyChanged(nameof(CriticalCracks));
                    CheckSet();
                }
            }
        }

        private bool ballastFull = false;
        public bool BallastFull
        {
            get { return ballastFull; }
            set
            {
                if (ballastFull != value)
                {
                    ballastFull = value;
                    RaisePropertyChanged(nameof(BallastFull));
                    CheckSet();
                }
            }
        }

        private bool ballastPartial = false;
        public bool BallastPartial
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
                    CheckSet();
                }
            }
        }

        private bool skewed = false;
        public bool Skewed
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
                    CheckSet();
                }
            }
        }

        private bool criticalSkewed = false;
        public bool CriticalSkewed
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
                    CheckSet();
                }
            }
        }

        private bool turnout = false;
        public bool Turnout
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

        private string side = "NOT SET";
        public string Side
        {
            get
            {
                return side;
            }
            set
            {
                if (side != value)
                {
                    side = value;
                    RaisePropertyChanged(nameof(Side));
                }
            }
        }

        private bool flipped = false;
        public bool Flipped
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
                    CheckSet();
                }
            }
        }

        private bool successfulSegmentation = true;
        public bool SuccessfulSegmentation
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

        private bool notSet = true;
        public bool NotSet
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

        private void CheckSet()
        {
            if (Normal ||
                Chipped || CriticalChipped ||
                Cracks || CriticalCracks ||
                BallastFull || BallastPartial ||
                Skewed || CriticalSkewed
                || Flipped)
            {
                NotSet = false;
            }
            else
            {
                NotSet = true;
            }
        }

        // public int Number { get; set; } //numer podkładu na odcinku przejazdu
        private void SetFrameNo()
        {
            try
            {

                var splited = FileName.Split('_');

                if (splited.Count() > 1)
                {
                    string fileFrameNo = splited[1];
                    int number = -1;
                    int.TryParse(fileFrameNo, out number);
                    this.frameNo = number;

                    int session = int.Parse(splited[2]);
                    this.session = session;
                }
            }
            catch (Exception ex)
            {
                this.frameNo = 0;
                this.session = 0;
            }
        }

        private int sleeperNo;
        public int SleeperNo
        {
            get { return sleeperNo; }
            set
            {
                if (sleeperNo != value)
                {
                    sleeperNo = value;
                    RaisePropertyChanged(nameof(SleeperNo));
                }
            }
        }

        private int sleeperParent;
        public int SleeperParent
        {
            get { return sleeperParent; }
            set
            {
                if (sleeperParent != value)
                {
                    sleeperParent = value;
                    RaisePropertyChanged(nameof(SleeperParent));
                }
            }
        }

        public bool IsDefected
        {
            get { return SetIsDefected(); }
        }

        public bool IsDefectedWithoutBalasstedAsDefect
        {
            get { return SetIsDefectedWithoutBalasstedAsDefect(); }
        }

        public bool IsCritical
        {
            get { return SetIsCritical(); }
        }

        public bool IsCriticalWithoutBalasstedAsDefect
        {
            get { return SetIsCriticalWithoutBalasstedAsDefect(); }
        }

        private bool SetIsDefected()
        {
            if (!Normal && 
                (Chipped || CriticalChipped ||
                 Cracks || CriticalCracks ||
                 BallastFull || BallastPartial))
            {
                return true;
            }
            else
            {  
                return false;
            }
        }

        private bool SetIsDefectedWithoutBalasstedAsDefect()
        {
            if (!Normal &&
                (Chipped || CriticalChipped ||
                 Cracks || CriticalCracks 
                 ))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SetIsCritical()
        {
            if (CriticalChipped ||
                 CriticalCracks ||
                 BallastFull)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SetIsCriticalWithoutBalasstedAsDefect()
        {
            if (CriticalChipped ||
                 CriticalCracks )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
