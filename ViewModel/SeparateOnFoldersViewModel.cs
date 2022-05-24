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
    public class SeparateOnFoldersViewModel: ViewModelBase
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

        private RelayCommand setSourceSleeperPathCommand;
        public RelayCommand SetSourceSleeperPathCommand
        {
            get
            {
                return setSourceSleeperPathCommand ?? (setSourceSleeperPathCommand = new RelayCommand(() => OpenSource()));
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
                    var csvFile = Directory.GetFiles(path).FirstOrDefault(x => x.Contains(".csv"));

                    if (!string.IsNullOrEmpty(csvFile))
                    {
                        records = DataLoadingService.GetRecordsFromFile(csvFile);
                        SeparateFiles();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SeparateFiles()
        {
            string separatedDirectory = Path.Combine(SourceSleeperPath, "Separated");

            CreateDirectories(separatedDirectory); 

            if (records.Any())
            {
                foreach (var record in records)
                {  
                    CopyFile(record, separatedDirectory);
                }
            }
        }

        private void CreateDirectories(string separatedDirectory)
        {
            Directory.CreateDirectory(separatedDirectory);

            Directory.CreateDirectory(Path.Combine(separatedDirectory, "Normal"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "Chipped"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "CriticalChipped"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "Cracks"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "CriticalCracks"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "Skewed"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "CriticalSkewed"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "BallastPartial"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "BallastFull"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "Turnout"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "Flipped"));
            Directory.CreateDirectory(Path.Combine(separatedDirectory, "BadSegmentation")); 
        }


        private void CopyFile(Record record, string separatedDirectory)
        {

            if (record.Normal)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "Normal", record.FileName));
            }

            if (record.Chipped)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "Chipped", record.FileName));
            }

            if(record.CriticalChipped)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "CriticalChipped", record.FileName));
            }

            if(record.Cracks)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "Cracks", record.FileName));
            }

            if(record.CriticalCracks)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "CriticalCracks", record.FileName));
            }

            if (record.Skewed)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "Skewed", record.FileName));
            }

            if (record.CriticalSkewed)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "CriticalSkewed", record.FileName));
            }

            if (record.BallastPartial)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "BallastPartial", record.FileName));
            }

            if (record.BallastFull)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "BallastFull", record.FileName));
            }

            if (record.Turnout)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "Turnout", record.FileName));
            }

            if (record.Flipped)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "Flipped", record.FileName));
            }

            if (!record.SuccessfulSegmentation)
            {
                File.Copy(record.FileFullPath, Path.Combine(separatedDirectory, "BadSegmentation", record.FileName));
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
