using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VI.AOD.NeuralNetworkDataManager.Model;
using VI.AOD.NeuralNetworkDataManager.Services;

namespace VI.AOD.NeuralNetworkDataManager.ViewModel
{
    public class SleeperImagesViewModel : ViewModelBase
    {
        private InitializeReordService initializeReordService;

        private string csvFilePath;
        public string CsvFilePath
        {
            get { return csvFilePath; }
            set
            {
                if (csvFilePath != value)
                {
                    csvFilePath = value;
                    RaisePropertyChanged(nameof(CsvFilePath));
                }
            }
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
                RaisePropertyChanged(nameof(Index));
                SetImagesByIndex();
            }
        }

        private int frameNo;
        public int FrameNo
        {
            get { return frameNo; }
            set
            {
                if (frameNo != value)
                {
                    frameNo = value;
                }
                
                RaisePropertyChanged(nameof(FrameNo));
                SetImageByFrame();
            }
        }


        private int maxIndex;
        public int MaxIndex
        {
            get { return maxIndex; }
            set
            {
                if (maxIndex != value)
                {
                    maxIndex = value;
                }
                RaisePropertyChanged(nameof(MaxIndex));
            }
        }

        private int maxFrameNo;
        public int MaxFrameNo
        {
            get { return maxFrameNo; }
            set
            {
                if (maxFrameNo != value)
                {
                    maxFrameNo = value;
                }
                RaisePropertyChanged(nameof(MaxFrameNo));
            }
        }

        private List<SleeperImage> sleeperImages;
        public List<SleeperImage> SleeperImages
        {
            get { return sleeperImages; }
            set
            {
                if (sleeperImages != value)
                {
                    sleeperImages = value;
                }
                RaisePropertyChanged(nameof(SleeperImages));
            }
        }

        private SleeperImage outerA;
        public SleeperImage OuterA
        {
            get { return outerA; }
            set
            {
                if (outerA != value)
                {
                    outerA = value;
                }
                RaisePropertyChanged(nameof(OuterA));
            }
        }

        private SleeperImage outerB;
        public SleeperImage OuterB
        {
            get { return outerB; }
            set
            {
                if (outerB != value)
                {
                    outerB = value;
                }
                RaisePropertyChanged(nameof(OuterB));
            }
        }

        private SleeperImage innerA;
        public SleeperImage InnerA
        {
            get { return innerA; }
            set
            {
                if (innerA != value)
                {
                    innerA = value;
                }
                RaisePropertyChanged(nameof(InnerA));
            }
        }

        private SleeperImage innerB;
        public SleeperImage InnerB
        {
            get { return innerB; }
            set
            {
                if (innerB != value)
                {
                    innerB = value;
                }
                RaisePropertyChanged(nameof(InnerB));
            }
        }

        private bool isCopied;
        public bool IsCopied
        {
            get { return isCopied; }
            set
            {
                if (isCopied != value)
                {
                    isCopied = value;

                    if (isCopied && showIsCopied)
                    {
                        IsCopiedVisibility = Visibility.Visible;
                    }
                    else
                    {
                        IsCopiedVisibility = Visibility.Hidden;
                    }
                }
                RaisePropertyChanged(nameof(IsCopied));
            }              
        }

        public bool CheckIsCopied()
        {
            if ((InnerA?.Record?.NotSet ?? false) || (InnerB?.Record?.NotSet ?? false) || (OuterA?.Record?.NotSet ?? false) || (OuterB?.Record?.NotSet ?? false))
            {
                return false;
            }
            return true;
        }

        private bool showIsCopied;

        public SleeperImagesViewModel(bool showIsCopied)
        {
            this.showIsCopied = showIsCopied;
            initializeReordService = new InitializeReordService();
            index = -1;
            IsCopied = false;
            IsCopiedVisibility = Visibility.Collapsed;
        }

        private Visibility isCopiedVisibility;
        public Visibility IsCopiedVisibility
        {
            get
            {
                return isCopiedVisibility;
            }

            set
            {
                if (isCopiedVisibility != value)
                {
                    isCopiedVisibility = value;
                }
                RaisePropertyChanged(nameof(IsCopiedVisibility));
            }
        }

        public void OpenRecords(string path)
        {
            SleeperImages = initializeReordService.Intialize(path, out csvFilePath);
            MaxFrameNo = SleeperImages?.Max(x => x.Record.FrameNo) ?? -1;
            MaxIndex = SleeperImages?.Max(x => x.Index) ?? -1;
            Index = SleeperImages?.Min(x=>x.Index) ?? -1;
        }

        public void SaveWithIndexes(string csvFilePath)
        {
            var fileName = Path.GetFileName(csvFilePath);
            if (!fileName.Contains("WithIndexes"))
            {   
                var records = SleeperImages.Select(x => x.Record).ToList(); 
                var newFileName = csvFilePath.Replace(fileName, $"WithIndexes_{fileName}");
                DuplicateDataSavingService.SaveRecords(records, newFileName);
            }
        }

        private RelayCommand peviewIndexCommand;
        public RelayCommand PeviewIndexCommand
        {
            get
            {
                return peviewIndexCommand ?? (peviewIndexCommand = new RelayCommand(() => PrevClick()));
            }
        }

        public void PrevClick()
        {
            Index--;
        }

        private RelayCommand nextIndexCommand;
        public RelayCommand NextIndexCommand
        {
            get
            {
                return nextIndexCommand ?? (nextIndexCommand = new RelayCommand(() => NextClick()));
            }
        }

        public void NextClick()
        {
            Index++;
        }

        public void SetImageByFrame()
        {
            var elements = sleeperImages.Where(x => x.Record.FrameNo == FrameNo);
            var idx = elements?.Min(x => x.Index) ?? -1;
            Index = idx;
        }

        public void SetImagesByIndex()
        {
            var elements = sleeperImages.Where(x => x.Index == Index);

            var inners = elements?.Where(x => x.FileName.Contains("inner"));

            InnerA = inners?.FirstOrDefault(x => !x.FileName.Contains("B"));
            InnerB = inners?.FirstOrDefault(x => x.FileName.Contains("B"));


            var outers = elements?.Where(x => x.FileName.Contains("outer"));

            OuterA = outers?.FirstOrDefault(x => !x.FileName.Contains("B"));
            OuterB = outers?.FirstOrDefault(x => x.FileName.Contains("B"));

            if (elements.Any())
            {
                frameNo = elements.FirstOrDefault().Record.FrameNo;
            }
            else
            {
                frameNo = -1;
            }

            RaisePropertyChanged(nameof(FrameNo));
            IsCopied = CheckIsCopied();
        }
    }
}
