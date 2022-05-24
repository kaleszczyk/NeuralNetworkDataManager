using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.AOD.NeuralNetworkDataManager.Enums;

namespace VI.AOD.NeuralNetworkDataManager.Model
{
    public class NeuralNetworkReport
    {
        public List<Record> RealResultRecords;
        public string ImagesSource;
        public ResultStatistics RealResultStatistics;
        public ResultStatistics RealResultStatisticsPercent;

        public List<Record> NeuralNetworkResultRecords;
        public ResultStatistics NeuralNetworkResultStatistics;
        public ResultStatistics NeuralNetworkResultStatisticsPercent;

        public Dictionary<DefectType, Result> GeneralResult;
        public Dictionary<DefectType, Result> CriticalResult;
        //public Dictionary<DefectType, Result> LowerResult;

        public bool SuccessfullSegmentation;
        public int ComparedRecords;
        public string MissingRecords = string.Empty;

        public NeuralNetworkReport()
        {
            GeneralResult = InitializeDictionary();
            CriticalResult = InitializeDictionary();
            //LowerResult = InitializeDictionary();
        }

        private Dictionary<DefectType, Result> InitializeDictionary()
        {
            var results = new Dictionary<DefectType, Result>();
            results.Add(DefectType.Summary, new Result());
            results.Add(DefectType.Normal, new Result());
            results.Add(DefectType.Chipping, new Result());
            results.Add(DefectType.Crack, new Result());
            results.Add(DefectType.Ballast, new Result());
            //results.Add(DefectType.ChippedWithoutBallast, new Result());
            //results.Add(DefectType.CrackWithoutBallast, new Result());
            results.Add(DefectType.GeneralDefect, new Result());
            results.Add(DefectType.GeneralDefectWithoutBallast, new Result());
            return results;
        }

        private string ResultInfo(Dictionary<DefectType, Result> results)
        {
            string report = string.Empty;

            foreach (var result in results)
            {
                report += $"{result.Key.ToString()}\n{result.Value.ToString()}\n";
            }

            return report;
        }
        public override string ToString()
        {
            string report = string.Empty;
            report = $"Summary compared images:\n{ComparedRecords.ToString()}\n" +
                     $"{nameof(RealResultStatistics)}\n{RealResultStatistics.ToString()}\n" +
                     $"{nameof(RealResultStatisticsPercent)}\n{RealResultStatisticsPercent.ToString()}\n" +
                     $"{nameof(NeuralNetworkResultStatistics)}\n{NeuralNetworkResultStatistics.ToString()}\n" +
                     $"{nameof(NeuralNetworkResultStatisticsPercent)}\n{NeuralNetworkResultStatisticsPercent.ToString()}\n" +
                     $"==================================================================================================\n" +
                     $"{nameof(GeneralResult)}\n{ ResultInfo(GeneralResult)}\n" +
                     $"----------------------------------------------------------------------------------------------------\n" +
                     $"{nameof(CriticalResult)}\n{ResultInfo(CriticalResult)}\n"+
                     $"{nameof(MissingRecords)}\n{MissingRecords}";

                     //$"----------------------------------------------------------------------------------------------------\n" +
                     //$"{nameof(LowerResult)}\n{ResultInfo(LowerResult)}\n";
            return report;
        }
    }
}
