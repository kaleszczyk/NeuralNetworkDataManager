using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using VI.AOD.NeuralNetworkDataManager.Services;
using VI.AOD.NeuralNetworkDataManager.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Collections.Generic;

namespace VI.AOD.NeuralNetworkDataManager.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private SetRecordViewModel setRecordsTab;
        public SetRecordViewModel SetRecordsTab
        {
            get { return setRecordsTab; }
            set
            {
                if (setRecordsTab != value)
                {
                    setRecordsTab = value;
                }
                RaisePropertyChanged(nameof(SetRecordsTab));
            }
        }

        private SetRecordBackTrackViewModel setRecordBackTrackTab;
        public SetRecordBackTrackViewModel SetRecordBackTrackTab
        {
            get { return setRecordBackTrackTab; }
            set
            {
                if (setRecordBackTrackTab != value)
                {
                    setRecordBackTrackTab = value;
                }
                RaisePropertyChanged(nameof(SetRecordBackTrackTab));
            }
        }


        private SeparateOnFoldersViewModel separateOnFolders;
        public SeparateOnFoldersViewModel SeparateOnFolders
        {
            get { return separateOnFolders; }
            set
            {
                if (separateOnFolders != value)
                {
                    separateOnFolders = value;
                }
                RaisePropertyChanged(nameof(SeparateOnFolders));
            }
        }


        private CopyOnlyCheckedViewModel copyOnlyChecked;
        public CopyOnlyCheckedViewModel CopyOnlyChecked
        {
            get { return copyOnlyChecked; }
            set
            {
                if (copyOnlyChecked != value)
                {
                    copyOnlyChecked = value;
                }
                RaisePropertyChanged(nameof(CopyOnlyChecked));
            }
        }


        private ShowStatisticsViewModel showStatistics;
        public ShowStatisticsViewModel ShowStatistics
        {
            get { return showStatistics; }
            set
            {
                if (showStatistics != value)
                {
                    showStatistics = value;
                }
                RaisePropertyChanged(nameof(ShowStatistics));
            }
        }

        private NeuralNetworkDataComparerViewModel neuralNetworkDataComparer;
        public NeuralNetworkDataComparerViewModel NeuralNetworkDataComparer
        {
            get { return neuralNetworkDataComparer; }
            set
            {
                if (neuralNetworkDataComparer != value)
                {
                    neuralNetworkDataComparer = value;
                }
                RaisePropertyChanged(nameof(NeuralNetworkDataComparer));
            }
        }

        private MergeResultsViewModel mergeResults;
        public MergeResultsViewModel MergeResults
        {
            get { return mergeResults; }
            set
            {
                if (mergeResults != value)
                {
                    mergeResults = value;
                }
                RaisePropertyChanged(nameof(MergeResults));
            }
        }

        private MergedResultForWholeSleeperViewModel mergedResultForWholeSleeper;
        public MergedResultForWholeSleeperViewModel MergedResultForWholeSleeper
        {
            get { return mergedResultForWholeSleeper; }
            set
            {
                if (mergedResultForWholeSleeper != value)
                {
                    mergedResultForWholeSleeper = value;
                }
                RaisePropertyChanged(nameof(MergedResultForWholeSleeper));
            }
        }   

        public MainViewModel()
        {
            SetRecordsTab = new SetRecordViewModel();
            SetRecordBackTrackTab = new SetRecordBackTrackViewModel();
            SeparateOnFolders = new SeparateOnFoldersViewModel();
            CopyOnlyChecked = new CopyOnlyCheckedViewModel();
            ShowStatistics = new ShowStatisticsViewModel();
            NeuralNetworkDataComparer = new NeuralNetworkDataComparerViewModel();
            MergeResults = new MergeResultsViewModel();
            mergedResultForWholeSleeper = new MergedResultForWholeSleeperViewModel();
        }
    }
}