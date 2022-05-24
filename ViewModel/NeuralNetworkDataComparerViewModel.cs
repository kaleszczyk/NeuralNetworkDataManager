﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Text;
using VI.AOD.NeuralNetworkDataManager.Model;
using VI.AOD.NeuralNetworkDataManager.Services;

namespace VI.AOD.NeuralNetworkDataManager.ViewModel
{
    public class NeuralNetworkDataComparerViewModel : ViewModelBase
    {
        private string manualResultPath;
        public string ManualResultPath
        {
            get
            {
                return manualResultPath;
            }
            set
            {
                manualResultPath = value;
                RaisePropertyChanged(nameof(ManualResultPath));
            }
        }

        private string neuralNetworkResultPath;
        public string NeuralNetworkResultPath
        {
            get
            {
                return neuralNetworkResultPath;
            }
            set
            {
                neuralNetworkResultPath = value;
                RaisePropertyChanged(nameof(NeuralNetworkResultPath));
            }
        }

        private string resultPath;
        public string ResultPath
        {
            get
            {
                return resultPath;
            }
            set
            {
                resultPath = value;
                RaisePropertyChanged(nameof(ResultPath));
            }
        }

        private bool successfullSegmentation = true;
        public bool SuccessfullSegmentation
        {
            get
            {
                return successfullSegmentation;
            }
            set
            {
                successfullSegmentation = value;
                RaisePropertyChanged(nameof(SuccessfullSegmentation));
            }
        }

        private bool saveImages = false;
        public bool SaveImages
        {
            get
            {
                return saveImages;
            }
            set
            {
                saveImages = value;
                RaisePropertyChanged(nameof(SaveImages));
            }
        }

        private RelayCommand setManualResultPathCommand;
        public RelayCommand SetManualResultPathCommand
        {
            get
            {
                return setManualResultPathCommand ?? (setManualResultPathCommand = new RelayCommand(() => OpentManualResultPath()));
            }
        }

        private void OpentManualResultPath()
        {
            ManualResultPath = GetCsvFilePath();
        }

        private RelayCommand setNeuralNetworkResultPathCommand;
        public RelayCommand SetNeuralNetworkResultPathCommand
        {
            get
            {
                return setNeuralNetworkResultPathCommand ?? (setNeuralNetworkResultPathCommand = new RelayCommand(() => OpenNeuralNetworkResultSource()));
            }
        }

        private void OpenNeuralNetworkResultSource()
        {
            NeuralNetworkResultPath = GetCsvFilePath();
        }

        private string GetCsvFilePath()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Save to existing CSV file";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = "csv";
            openFileDialog.Filter = "CSV file (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }

        private RelayCommand setStatisticsResultPathCommand;
        public RelayCommand SetStatisticsResultPathCommand
        {
            get
            {
                return setStatisticsResultPathCommand ?? (setStatisticsResultPathCommand = new RelayCommand(() => OpenStatisticsResultPathSource()));
            }
        }

        private void OpenStatisticsResultPathSource()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Select Folder.";
            if (openFileDialog.ShowDialog() == true)
            {
                ResultPath = Path.GetDirectoryName(openFileDialog.FileName);
            }
        }

        private RelayCommand runCommand;
        public RelayCommand RunCommand
        {
            get
            {
                return runCommand ?? (runCommand = new RelayCommand(() => Run()));
            }
        }

        private void Run()
        {
            try
            {
                NeuralNetworkReport neuralNetworkReport = new NeuralNetworkReport();

                CalculateStatistiscService calculateStatistiscService = new CalculateStatistiscService();
                CompareResultService compareResultService = new CompareResultService();
                neuralNetworkReport.SuccessfullSegmentation = SuccessfullSegmentation;
                neuralNetworkReport.ImagesSource = Path.GetDirectoryName(ManualResultPath);
                neuralNetworkReport.RealResultStatistics = calculateStatistiscService.CalculateStatistics(ManualResultPath, out neuralNetworkReport.RealResultRecords, SuccessfullSegmentation);
                neuralNetworkReport.RealResultStatisticsPercent = calculateStatistiscService.CalculateStatistics(neuralNetworkReport.RealResultStatistics);
                neuralNetworkReport.NeuralNetworkResultStatistics = calculateStatistiscService.CalculateStatistics(NeuralNetworkResultPath, out neuralNetworkReport.NeuralNetworkResultRecords, true);
                neuralNetworkReport.NeuralNetworkResultStatisticsPercent = calculateStatistiscService.CalculateStatistics(neuralNetworkReport.NeuralNetworkResultStatistics);

                compareResultService.RealResultRecords = neuralNetworkReport.RealResultRecords;
                compareResultService.NeuralNetworkResultRecords = neuralNetworkReport.NeuralNetworkResultRecords;

                compareResultService.CompareResults(neuralNetworkReport, ResultPath, SaveImages);


                var reportPath = Path.Combine(ResultPath, $"Raport{(SuccessfullSegmentation ? "SegementationOk" : "WrongSegmentation")}", $"ResultReport.txt");


                Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }

                File.WriteAllText(reportPath, neuralNetworkReport.ToString(), Encoding.UTF8);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
