using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using VI.AOD.NeuralNetworkDataManager.Model;
using VI.AOD.NeuralNetworkDataManager.Services;

namespace VI.AOD.NeuralNetworkDataManager.ViewModel
{
    public class ShowStatisticsViewModel : ViewModelBase
    {
        private List<RecordsStatistics> recordsStatistics = new List<RecordsStatistics>();
        private List<RecordsStatistics> recordsPercentStatistics = new List<RecordsStatistics>();

        public List<RecordsStatistics> RecordsStatistics
        {
            get { return recordsStatistics; }
            set
            {
                if(recordsStatistics!=value)
                {
                    recordsStatistics = value;
                    RaisePropertyChanged(nameof(RecordsStatistics));
                }
            }
        }

        public List<RecordsStatistics> RecordsPercentStatistics
        {
            get { return recordsPercentStatistics; }
            set
            {
                if (recordsPercentStatistics != value)
                {
                    recordsPercentStatistics = value;
                    RaisePropertyChanged(nameof(RecordsPercentStatistics));
                }
            }
        }

        private string sourcePath;
        public string SourcePath
        {
            get { return sourcePath; }
            set
            {
                if (sourcePath != value)
                {
                    sourcePath = value;
                    RaisePropertyChanged(nameof(SourcePath));
                }
            }
        }

        private RelayCommand setSourcePathCommand;
        public RelayCommand SetSourcePathCommand
        {
            get
            {
                return setSourcePathCommand ?? (setSourcePathCommand = new RelayCommand(() => OpenSource()));
            }
        }

        private void OpenSource()
        {
            List<Record> records = new List<Record>();
            List<Record> summaryRecords = new List<Record>();

            try
            {
                var path = GetPath();
               

                if (!string.IsNullOrEmpty(path))
                {
                    var recStat = new List<RecordsStatistics>();
                    var recPerc = new List<RecordsStatistics>();

                    SourcePath = path;
                    var csvFiles = Directory.GetFiles(path, "*.csv", SearchOption.AllDirectories).Where(x => x.Contains(".csv"));

                    foreach (var csvFile in csvFiles)
                    {
                        if (!string.IsNullOrEmpty(csvFile))
                        {
                            records = DataLoadingService.GetRecordsFromFile(csvFile);
                            var fileName =  Path.GetFileNameWithoutExtension(csvFile);
                            var stat = CalculateStatistics(records, fileName);
                            var perc = CalculateStatistics(stat, fileName);

                            recStat.Add(stat);
                            recPerc.Add(perc);

                            summaryRecords.AddRange(records);
                        }
                    }

                    var statSum = CalculateStatistics(summaryRecords, "Summary");
                    var percSum = CalculateStatistics(statSum, "Summary");

                    recStat.Add(statSum);
                    recPerc.Add(percSum);

                    RecordsStatistics = recStat;
                    RecordsPercentStatistics = recPerc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RecordsStatistics CalculateStatistics(List<Record> records, string fileName)
        {
            RecordsStatistics recordsStatistic = new RecordsStatistics();

            recordsStatistic.FileName = fileName;
            recordsStatistic.ImagesCount = records.Count(x=>!x.NotSet);
            recordsStatistic.Normal = records.Count(x => x.Normal);
            recordsStatistic.Chipped = records.Count(x => x.Chipped);
            recordsStatistic.CriticalChipped = records.Count(x => x.CriticalChipped);
            recordsStatistic.Cracks = records.Count(x => x.Cracks);
            recordsStatistic.CriticalCracks = records.Count(x => x.CriticalCracks);
            recordsStatistic.Skewed = records.Count(x => x.Skewed);
            recordsStatistic.CriticalSkewed = records.Count(x => x.CriticalSkewed);
            recordsStatistic.BallastPartial = records.Count(x => x.BallastPartial);
            recordsStatistic.BallastFull = records.Count(x => x.BallastFull);
            recordsStatistic.Turnout = records.Count(x => x.Turnout);
            recordsStatistic.Flipped = records.Count(x => x.Flipped);
            recordsStatistic.SuccessfulSegmentation = records.Count(x => x.SuccessfulSegmentation && !x.NotSet);

            return recordsStatistic;
            
        }

        private RecordsStatistics CalculateStatistics(RecordsStatistics recordsStatistic, string fileName)
        {
            RecordsStatistics recordsPercent = new RecordsStatistics();

            recordsPercent.FileName = fileName;
            recordsPercent.ImagesCount = recordsStatistic.ImagesCount;
            recordsPercent.Normal = Math.Round(recordsStatistic.Normal / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.Chipped = Math.Round(recordsStatistic.Chipped / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.CriticalChipped = Math.Round(recordsStatistic.CriticalChipped / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.Cracks = Math.Round(recordsStatistic.Cracks / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.CriticalCracks = Math.Round(recordsStatistic.CriticalCracks / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.Skewed = Math.Round(recordsStatistic.Skewed / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.CriticalSkewed = Math.Round(recordsStatistic.CriticalSkewed / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.BallastPartial = Math.Round(recordsStatistic.BallastPartial / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.BallastFull = Math.Round(recordsStatistic.BallastFull / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.Turnout = Math.Round(recordsStatistic.Turnout / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.Flipped = Math.Round(recordsStatistic.Flipped / recordsStatistic.ImagesCount * 100, 3);
            recordsPercent.SuccessfulSegmentation = Math.Round(recordsStatistic.SuccessfulSegmentation / recordsStatistic.ImagesCount * 100, 3);

            return recordsPercent;
        }

        private string GetPath()
        {                     
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = "Select Folder.";
                if (openFileDialog.ShowDialog() == true)
                {
                    var selectedPath = Path.GetDirectoryName(openFileDialog.FileName);
                    return selectedPath;
                }
            }
            catch (Exception e)
            {
               MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return string.Empty;
        }
    }
}
