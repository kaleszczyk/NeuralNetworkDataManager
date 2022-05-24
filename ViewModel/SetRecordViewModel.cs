using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using VI.AOD.NeuralNetworkDataManager.Model;
using VI.AOD.NeuralNetworkDataManager.Services;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace VI.AOD.NeuralNetworkDataManager.ViewModel
{
    public class SetRecordViewModel : ViewModelBase
    {
        private OpenFileDialog openFileDialog;
        private Microsoft.Win32.SaveFileDialog saveFileDialog;
        private FolderBrowserDialog folderBrowserDialog;
        private string selectedPath;
        private string selectedFileFullPath;
        private List<Record> records;
        private Record selectedRecord;

        public SetRecordViewModel()
        {
            records = new List<Record>();
            LoadSourceDataCommand = new RelayCommand(LoadSourceDataMethod);
            LoadManagedDataCommand = new RelayCommand(LoadManagedDataMethod);
            SaveChangesCommand = new RelayCommand(SaveChangesMethod);
            AddRecordsToExistingFileCommand = new RelayCommand(AddRecordsToExistingFileMethod);
            CreateNewFileCommand = new RelayCommand(CreateNewFileMethod);
            CopyImagesCommand = new RelayCommand(CopyImagesMethod);
            ReloadManagedDataCommand = new RelayCommand(ReloadData);
            RemoveRecordCommand = new RelayCommand(RemoveFile);
        }



        public ICommand LoadSourceDataCommand { get; private set; }
        public ICommand LoadManagedDataCommand { get; private set; }
        public ICommand ReloadManagedDataCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand AddRecordsToExistingFileCommand { get; private set; }
        public ICommand CreateNewFileCommand { get; private set; }
        public ICommand CopyImagesCommand { get; private set; }
        public ICommand RemoveRecordCommand { get; private set; }


        public String SelectedPath
        {
            get
            {
                return selectedPath;
            }
            set
            {
                selectedPath = value;
                RaisePropertyChanged("SelectedPath");
            }
        }

        public String SelectedFileFullPath
        {
            get
            {
                return selectedFileFullPath;
            }
            set
            {
                selectedFileFullPath = value;
                if (File.Exists(selectedFileFullPath))
                {

                    ImageToShow = BitmapToBitmapSource(new Bitmap(selectedFileFullPath));
                }
                else
                {
                    ImageToShow = null;
                }
                RaisePropertyChanged(nameof(SelectedFileFullPath));
            }
        }

        public List<Record> Records
        {
            get
            {
                return records;
            }
            set
            {
                records = value;
                RaisePropertyChanged(nameof(Records));
            }
        }

        public int CheckedCount
        {
            get
            {
                return Records?.Count(x => !x.NotSet) ?? 0;
            }
        }

        public int AllCount
        {
            get
            {
                return Records?.Count() ?? 0;
            }
        }

        private BitmapSource imageToShow;
        public BitmapSource ImageToShow
        {
            get
            {
                return imageToShow;
            }
            set
            {
                imageToShow = value;
                RaisePropertyChanged(nameof(ImageToShow));
            }
        }

        private void RemoveFile()
        {
            if (SelectedRecord != null)
            {
                try
                {
                    var path = SelectedRecord.FileFullPath;
                    var next = Records.IndexOf(SelectedRecord);

                    var cleanedList = new List<Record>(Records);
                    cleanedList.Remove(SelectedRecord);

                    if (next != 0)
                    {
                        SelectedRecord = Records[0];
                    }
                    else
                    {
                        SelectedRecord = Records[Records.Count() - 1];
                    }

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    Records = cleanedList;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                RaisePropertyChanged(nameof(Records));
                SaveChangesMethod();
            }
        }
        public Record SelectedRecord
        {
            get
            {
                return selectedRecord;
            }
            set
            {
                selectedRecord = value;
                SelectedFileFullPath = selectedRecord?.FileFullPath ?? string.Empty;
                RaisePropertyChanged(nameof(SelectedRecord));
                RaisePropertyChanged(nameof(AllCount));
                RaisePropertyChanged(nameof(CheckedCount));
            }
        }

        private RelayCommand<String> _CheckFromKey;
        public RelayCommand<String> CheckFromKey
        {
            get
            {
                return _CheckFromKey ?? (_CheckFromKey = new RelayCommand<string>(
                parameter =>
                {
                    switch (parameter)
                    {
                        case "F1":
                            SelectedRecord.Normal = !SelectedRecord.Normal;
                            break;
                        case "F2":
                            SelectedRecord.Chipped = !SelectedRecord.Chipped;
                            break;
                        case "F3":
                            SelectedRecord.CriticalChipped = !SelectedRecord.CriticalChipped;
                            break;
                        case "F4":
                            SelectedRecord.Cracks = !SelectedRecord.Cracks;
                            break;
                        case "F5":
                            SelectedRecord.CriticalCracks = !SelectedRecord.CriticalCracks;
                            break;
                        case "F6":
                            SelectedRecord.BallastFull = !SelectedRecord.BallastFull;
                            break;
                        case "F7":
                            SelectedRecord.BallastPartial = !SelectedRecord.BallastPartial;
                            break;
                        case "F8":
                            SelectedRecord.Skewed = !SelectedRecord.Skewed;
                            break;
                        case "F9":
                            SelectedRecord.CriticalSkewed = !SelectedRecord.CriticalSkewed;
                            break;
                        case "F10":
                            SelectedRecord.Turnout = !SelectedRecord.Turnout;
                            break;
                        case "F11":
                            SelectedRecord.Flipped = !SelectedRecord.Flipped;
                            break;
                        case "F12":
                            SelectedRecord.SuccessfulSegmentation = !SelectedRecord.SuccessfulSegmentation;
                            break;
                    }
                },
                parameter =>
                {
                    return SelectedRecord != null;
                }));
            }
        }
        public BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            var imgSource =
                          System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());

            source.Dispose();

            return imgSource;

        }

        public void LoadSourceDataMethod()
        {

            openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Select Folder.";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedPath = Path.GetDirectoryName(openFileDialog.FileName);
                records = DataLoadingService.GetRecords(selectedPath, false);
                this.RaisePropertyChanged(() => this.Records);
            }
        }

        public void LoadManagedDataMethod()
        {
            openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "CSV file (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedPath = openFileDialog.FileName;
                records = DataLoadingService.GetRecords(selectedPath);
                this.RaisePropertyChanged(() => this.Records);
            }
        }

        public void SaveChangesMethod()
        {
            try
            {
                if (selectedPath == null) return;
                if (File.GetAttributes(selectedPath).HasFlag(FileAttributes.Directory))
                {
                    System.Windows.MessageBox.Show("Data has been loaded from source, theres no .csv file to modify.");
                }
                else
                {
                    DataSavingService.SaveRecords(records, selectedPath);
                    System.Windows.MessageBox.Show("Changes in file " + selectedPath + " saved.");
                }
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show("Error", "File problem");
            }
        }

        public void AddRecordsToExistingFileMethod()
        {
            openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Save to existing CSV file";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = "csv";
            openFileDialog.Filter = "CSV file (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedPath = openFileDialog.FileName;
                DataSavingService.SaveRecords(records, selectedPath, true);
                System.Windows.MessageBox.Show("Records added to existing file.");
            }


        }

        public void CreateNewFileMethod()
        {
            saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Title = "Create CSV file";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                selectedPath = saveFileDialog.FileName;
                DataSavingService.SaveRecords(records, selectedPath);
                System.Windows.MessageBox.Show("Records saved to new file.");
            }


        }

        private void ReloadData()
        {
            var sourceDirectory = Path.GetDirectoryName(SelectedPath);

            var files = Directory.GetFiles(sourceDirectory).Where(x => Path.GetExtension(x) == ".png").ToList();
            var updatedRecords = new List<Record>(records);

            if (Records?.Any() == true)
            {
                foreach (var record in this.records)
                {

                    if (files.Contains(record.FileFullPath))
                    {
                        files.Remove(record.FileFullPath);
                    }
                    else
                    {
                        updatedRecords.Remove(record);
                    }
                }

                foreach (var file in files)
                {
                    updatedRecords.Add(new Record() { FileFullPath = file, FileName = Path.GetFileName(file), Side = file.ExcludeSide() });
                }

                Records = updatedRecords;
            }
        }

        public void CopyImagesMethod()
        {
            folderBrowserDialog = new FolderBrowserDialog();


            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {

                DataSavingService.CopyImages(records, folderBrowserDialog.SelectedPath);
                System.Windows.MessageBox.Show("Images copied to the new folder.");
            }
        }
    }
}

