using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VI.AOD.NeuralNetworkDataManager.Model;
using VI.AOD.NeuralNetworkDataManager.Services;

namespace VI.AOD.NeuralNetworkDataManager.ViewModel
{
    public class MergeResultsViewModel : ViewModelBase
    {
        private string sourcePath;
        public string SourcePath
        {
            get
            {
                return sourcePath;
            }
            set
            {
                sourcePath = value;
                RaisePropertyChanged(nameof(SourcePath));
            }
        }

        private string destPath;
        public string DestPath
        {
            get
            {
                return destPath;
            }
            set
            {
                destPath = value;
                RaisePropertyChanged(nameof(DestPath));
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

        private bool isFiledCopies = true;
        public bool IsFiledCopies
        {
            get
            {
                return isFiledCopies;
            }
            set
            {
                isFiledCopies = value;
                RaisePropertyChanged(nameof(IsFiledCopies));
            }
        }

        private double copyProgress = 0;
        public double CopyProgress
        {
            get
            {
                return copyProgress;
            }
            set
            {
                copyProgress = value;
                RaisePropertyChanged(nameof(CopyProgress));
            }
        }

        private double pBMax = 0;
        public double PBMax
        {
            get
            {
                return pBMax;
            }
            set
            {
                pBMax = value;
                RaisePropertyChanged(nameof(PBMax));
            }
        }

        private RelayCommand setSourcePathPathCommand;
        public RelayCommand SetSourcePathPathCommand
        {
            get
            {
                return setSourcePathPathCommand ?? (setSourcePathPathCommand = new RelayCommand(() => OpenSourcePath()));
            }
        }

        private void OpenSourcePath()
        {
            SourcePath = GetCsvFilePath();
        }

        private RelayCommand setDestPathPathCommand;
        public RelayCommand SetDestPathPathCommand
        {
            get
            {
                return setDestPathPathCommand ?? (setDestPathPathCommand = new RelayCommand(() => OpenDestPath()));
            }
        }

        private void OpenDestPath()
        {
            DestPath = GetCsvFilePath();
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
                MergeCSV(SourcePath, DestPath);
                var sourceDirectory = Path.GetDirectoryName(SourcePath);
                var destDirectory = Path.GetDirectoryName(DestPath);
                CopyFiles(sourceDirectory, destDirectory);

            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error");
            }
        }

        private void MergeCSV(string source, string dest)
        {
            var baseRecords = new List<Record>();
            if (File.Exists(dest))
            {
                baseRecords = DataLoadingService.GetRecordsFromFile(dest);
            }

            var toAddRecords = DataLoadingService.GetRecordsFromFile(source);

            var records = new List<Record>(baseRecords);
            records.AddRange(toAddRecords);

            DataSavingService.SaveRecords(records, dest);
        }

        private void CopyFiles(string source, string dest)
        {
            var files = Directory.GetFiles(source, "*.png", SearchOption.TopDirectoryOnly);
            CopyProgress = 0;

            PBMax = files.Count();

            if (IsFiledCopies)
            {
                Task.Run(() =>
                {
                    foreach (var file in files)
                    {
                        File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
                        CopyProgress++;
                    }
                });
            }
            else
            {  
                Task.Run(() =>
                {
                    foreach (var file in files)
                    {
                        File.Move(file, Path.Combine(dest, Path.GetFileName(file)));
                        CopyProgress++;
                    }
                });
            }
        }
    }
}
