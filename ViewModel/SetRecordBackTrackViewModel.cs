using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Input;
using VI.AOD.NeuralNetworkDataManager.Services;

namespace VI.AOD.NeuralNetworkDataManager.ViewModel
{
    public class SetRecordBackTrackViewModel : ViewModelBase
    {
        private string sourceSleeperSetPath;
        public string SourceSleeperSetPath
        {
            get { return sourceSleeperSetPath; }
            set
            {
                if (sourceSleeperSetPath != value)
                {
                    sourceSleeperSetPath = value;
                    RaisePropertyChanged(nameof(SourceSleeperSetPath));
                }
            }
        }

        private string destinationSleeperSetPath;
        public string DestinationSleeperSetPath
        {
            get { return destinationSleeperSetPath; }
            set
            {
                if (destinationSleeperSetPath != value)
                {
                    destinationSleeperSetPath = value;
                    RaisePropertyChanged(nameof(DestinationSleeperSetPath));
                }
            }
        }

        public SetRecordBackTrackViewModel()
        {
            SourceSleeperSet = new SleeperImagesViewModel(false);
            DestinationSleeperSet = new SleeperImagesViewModel(true);
        }

        private SleeperImagesViewModel sourceSleeperSet;
        public SleeperImagesViewModel SourceSleeperSet
        {
            get { return sourceSleeperSet; }
            set
            {
                if (sourceSleeperSet != value)
                {
                    sourceSleeperSet = value;
                    RaisePropertyChanged(nameof(SourceSleeperSet));
                }
            }
        }
        private SleeperImagesViewModel destinationSleeperSet;
        public SleeperImagesViewModel DestinationSleeperSet
        {
            get { return destinationSleeperSet; }
            set
            {
                if (destinationSleeperSet != value)
                {
                    destinationSleeperSet = value;
                    RaisePropertyChanged(nameof(DestinationSleeperSet));
                }
            }
        }

        private RelayCommand setSourceSleeperSetPathCommand;
        public RelayCommand SetSourceSleeperSetPathCommand
        {
            get
            {
                return setSourceSleeperSetPathCommand ?? (setSourceSleeperSetPathCommand = new RelayCommand(() => OpenSource()));
            }
        }

        private void OpenSource()
        {
            var path = GetPath();
            if (!string.IsNullOrEmpty(path))
            {
                SourceSleeperSet.OpenRecords(path);
                SourceSleeperSet.SaveWithIndexes(SourceSleeperSet.CsvFilePath);
                SourceSleeperSetPath = path;
            }
        }


        private RelayCommand setDestinationSleeperSetPathCommand;
        public RelayCommand SetDestinationSleeperSetPathCommand
        {
            get
            {
                return setDestinationSleeperSetPathCommand ?? (setDestinationSleeperSetPathCommand = new RelayCommand(() => OpenDestination()));
            }
        }

        private RelayCommand copyRecordsSetCommand;
        public RelayCommand CopyRecordsSetCommand
        {
            get
            {
                return copyRecordsSetCommand ?? (copyRecordsSetCommand = new RelayCommand(() => CopyRecordsSet()));
            }
        }

        private RelayCommand clearRecordsSetCommand;
        public RelayCommand ClearRecordsSetCommand
        {
            get
            {
                return clearRecordsSetCommand ?? (clearRecordsSetCommand = new RelayCommand(() => ClearRecordsSet()));
            }
        }

        private void ClearRecordsSet()
        {
            DestinationSleeperSet.InnerA?.ClearRecordData();
            DestinationSleeperSet.InnerB?.ClearRecordData();
            DestinationSleeperSet.OuterA?.ClearRecordData();
            DestinationSleeperSet.OuterB?.ClearRecordData();
            var records = DestinationSleeperSet.SleeperImages.Select(x => x.Record).ToList();
            DuplicateDataSavingService.SaveRecords(records, DestinationSleeperSet.CsvFilePath);
            DestinationSleeperSet.IsCopied = false;
        }

        private void CopyRecordsSet()
        {
            DestinationSleeperSet.InnerA?.RewriteRecordData(SourceSleeperSet.InnerA?.Record);
            DestinationSleeperSet.InnerB?.RewriteRecordData(SourceSleeperSet.InnerB?.Record);
            DestinationSleeperSet.OuterA?.RewriteRecordData(SourceSleeperSet.OuterA?.Record);
            DestinationSleeperSet.OuterB?.RewriteRecordData(SourceSleeperSet.OuterB?.Record);

            var records = DestinationSleeperSet.SleeperImages.Select(x => x.Record).ToList();
            DuplicateDataSavingService.SaveRecords(records, DestinationSleeperSet.CsvFilePath);
            DestinationSleeperSet.IsCopied = true;
            NextRecord();
        }

        private RelayCommand nextRecordCommand;
        public RelayCommand NextRecordCommand
        {
            get
            {
                return nextRecordCommand ?? (nextRecordCommand = new RelayCommand(() => NextRecord()));
            }
        }

        private void NextRecord()
        {
            SourceSleeperSet.NextClick();
            DestinationSleeperSet.PrevClick();
        }

        private RelayCommand prevRecordCommand;
        public RelayCommand PrevRecordCommand
        {
            get
            {
                return prevRecordCommand ?? (prevRecordCommand = new RelayCommand(() => PrevRecord()));
            }
        }

        private void PrevRecord()
        {
            SourceSleeperSet.PrevClick();
            DestinationSleeperSet.NextClick();
        }
        private void SaveRecords()
        {
            var records = DestinationSleeperSet.SleeperImages.Select(x => x.Record).ToList();
            DuplicateDataSavingService.SaveRecords(records, DestinationSleeperSet.CsvFilePath);
            System.Windows.MessageBox.Show("Changes in file " + DestinationSleeperSet.CsvFilePath + " saved.");
        }

        private void OpenDestination()
        {
            var path = GetPath();
            if (!string.IsNullOrEmpty(path))
            {
                DestinationSleeperSet.OpenRecords(path);
                DestinationSleeperSetPath = path;
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
