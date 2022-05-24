using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VI.AOD.NeuralNetworkDataManager.Model
{
    public class SleeperImage : ViewModelBase
    {
        private Record record;
        public Record Record
        {
            get { return record; }
            set
            {
                if (record != value)
                {
                    record = value;
                }

                RaisePropertyChanged(nameof(Record));
            }
        }

        public string FileName
        {
            get { return Record.FileName ?? string.Empty; }
        }

        public string FilePath
        {
            get { return Record.FileFullPath ?? string.Empty; }
        }

        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                }

                RaisePropertyChanged(nameof(record));
            }
        }

        public SleeperImage(Record record, int index)
        {
            this.Record = record;
            if (this.record.SleeperNo == -1)
            {
                this.record.SleeperNo = index;
            }
            this.Index = index;
        }

        public void RewriteRecordData(Record record)
        {
            if (record != null)
            {
                this.record.Normal = record.Normal;

                this.record.Chipped = record.Chipped;
                this.record.CriticalChipped = record.CriticalChipped;

                this.record.Cracks = record.Cracks;
                this.record.CriticalCracks = record.CriticalCracks;

                this.record.Skewed = record.Skewed;
                this.record.CriticalSkewed = record.CriticalSkewed;

                this.record.BallastPartial = record.BallastPartial;
                this.record.BallastFull = record.BallastFull;

                this.record.Turnout = record.Turnout;
                this.record.Flipped = record.Flipped;
                this.record.SuccessfulSegmentation = record.SuccessfulSegmentation;

                this.record.SleeperParent = record.SleeperNo;
            }
        }

        internal void ClearRecordData()
        {
            this.record.Normal = false;

            this.record.Chipped = false;
            this.record.CriticalChipped = false;

            this.record.Cracks = false;
            this.record.CriticalCracks = false;

            this.record.Skewed = false;
            this.record.CriticalSkewed = false;

            this.record.BallastPartial = false;
            this.record.BallastFull = false;

            this.record.Turnout = false;
            this.record.Flipped = false;
            this.record.SuccessfulSegmentation = true;

            this.record.SleeperParent = -1;
        }
    }
}
