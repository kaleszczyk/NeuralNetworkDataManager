using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAPICodePack.Dialogs;
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
    public class CopyOnlyCheckedViewModel : ViewModelBase
    {
        private List<Record> records = new List<Record>();

        private string sourceSleeperPath;
        public string SourceSleeperPath
        {
            get { return sourceSleeperPath; }
            set
            {
                if (sourceSleeperPath != value)
                {
                    sourceSleeperPath = value;
                    RaisePropertyChanged(nameof(SourceSleeperPath));
                }
            }
        }

        private string destinationSleeperPath;
        public string DestinationSleeperPath
        {
            get { return destinationSleeperPath; }
            set
            {
                if (destinationSleeperPath != value)
                {
                    destinationSleeperPath = value;
                    RaisePropertyChanged(nameof(DestinationSleeperPath));
                }
            }
        }

        private RelayCommand setSourceSleeperPathCommand;
        public RelayCommand SetSourceSleeperPathCommand
        {
            get
            {
                return setSourceSleeperPathCommand ?? (setSourceSleeperPathCommand = new RelayCommand(() => OpenSource()));
            }
        }

        private RelayCommand setDestinationSleeperPathCommand;
        public RelayCommand SetDestinationSleeperPathCommand
        {
            get
            {
                return setDestinationSleeperPathCommand ?? (setDestinationSleeperPathCommand = new RelayCommand(() => OpenDestination()));
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
            var csvFile = Directory.GetFiles(SourceSleeperPath).FirstOrDefault(x => x.Contains(".csv"));

            if (!string.IsNullOrEmpty(csvFile))
            {
                records = DataLoadingService.GetRecordsFromFile(csvFile).Where(x=>!x.NotSet).ToList();

                foreach (var record in records)
                {
                    try
                    {
                        var destPath = Path.Combine(DestinationSleeperPath, record.FileName);

                        if (!File.Exists(record.FileFullPath))
                        {
                            MessageBox.Show(record.FileName.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        if (!File.Exists(destPath) && File.Exists(record.FileFullPath))
                        {         
                            File.Copy(record.FileFullPath, destPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }                  
            }
        }

        private void OpenDestination()
        {
            try
            {
                var path = GetPath();
                if (!string.IsNullOrEmpty(path))
                {
                    DestinationSleeperPath = path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenSource()
        {
            try
            {
                var path = GetPath();
                if (!string.IsNullOrEmpty(path))
                {
                    SourceSleeperPath = path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetPath()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                try
                {
                    if (!string.IsNullOrEmpty(dialog.FileName))
                    {
                        return dialog.FileName;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return string.Empty;
        }
    }
}
