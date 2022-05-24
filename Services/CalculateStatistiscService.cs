using System;
using System.Collections.Generic;
using System.Linq;
using VI.AOD.NeuralNetworkDataManager.Model;

namespace VI.AOD.NeuralNetworkDataManager.Services
{
    public class CalculateStatistiscService
    {

        public ResultStatistics CalculateStatistics(string csvFile, out List<Record>records, bool successfullSegmentation)
        {
            ResultStatistics resultStatistisc = new ResultStatistics();

            if (!string.IsNullOrEmpty(csvFile))
            {
                records = DataLoadingService.GetRecordsFromFile(csvFile).Where(x=>x.SuccessfulSegmentation==successfullSegmentation).ToList();
                var statistics = CalculateStatistics(records);
                resultStatistisc.RecordsCount = statistics.ImagesCount; 
                resultStatistisc.CiticalStatistic = CalculateCriticalStatistics(statistics);
                resultStatistisc.GeneralStatistic = CalculateGeneralStatistics(statistics);
                resultStatistisc.LowerDefectsStatistic = CalculateLowerDefectsStatistics(statistics);
            }
            else
            {
              records = new List<Record>();
            }

            return resultStatistisc;
        }

        private DefectsStatistics CalculateCriticalStatistics(RecordsStatistics recordsStatistics)
        {
            DefectsStatistics defectsStatistics = new DefectsStatistics();

            defectsStatistics.Normal = 0;
            defectsStatistics.Chipped = recordsStatistics.CriticalChipped;
            defectsStatistics.Cracked = recordsStatistics.CriticalCracks;
            defectsStatistics.Balast = recordsStatistics.BallastFull;

            return defectsStatistics;
        }

        private DefectsStatistics CalculateGeneralStatistics(RecordsStatistics recordsStatistics)
        {
            DefectsStatistics defectsStatistics = new DefectsStatistics();

            defectsStatistics.Normal = recordsStatistics.Normal;
            defectsStatistics.Chipped = recordsStatistics.Chipped;
            defectsStatistics.Cracked = recordsStatistics.Cracks;
            defectsStatistics.Balast = recordsStatistics.BallastPartial+recordsStatistics.BallastFull;

            return defectsStatistics;
        }

        private DefectsStatistics CalculateLowerDefectsStatistics(RecordsStatistics recordsStatistics)
        {
            DefectsStatistics defectsStatistics = new DefectsStatistics();          

            defectsStatistics.Normal = recordsStatistics.Normal;
            defectsStatistics.Chipped = recordsStatistics.Chipped - recordsStatistics.CriticalChipped;
            defectsStatistics.Cracked = recordsStatistics.Cracks - recordsStatistics.CriticalCracks;
            defectsStatistics.Balast = recordsStatistics.BallastPartial + recordsStatistics.BallastFull;

            return defectsStatistics;
        }

        public ResultStatistics CalculateStatistics(ResultStatistics resultStatistics)
        {
            var resultStatistiscPercent = new ResultStatistics();

            resultStatistiscPercent.RecordsCount = resultStatistics.RecordsCount;
            resultStatistiscPercent.CiticalStatistic = DefectsStatisticsPercent(resultStatistics.CiticalStatistic, resultStatistics.RecordsCount);
            resultStatistiscPercent.GeneralStatistic = DefectsStatisticsPercent(resultStatistics.GeneralStatistic, resultStatistics.RecordsCount);
            resultStatistiscPercent.LowerDefectsStatistic = DefectsStatisticsPercent(resultStatistics.LowerDefectsStatistic, resultStatistics.RecordsCount);

            return resultStatistiscPercent;
        }

        private DefectsStatistics DefectsStatisticsPercent(DefectsStatistics defectsStatistics, int recordsCount)
        {
            DefectsStatistics defectsStatisticsPercent = new DefectsStatistics();

            defectsStatisticsPercent.Normal = defectsStatistics.Normal / recordsCount * 100;
            defectsStatisticsPercent.Chipped = defectsStatistics.Chipped / recordsCount * 100;
            defectsStatisticsPercent.Cracked = defectsStatistics.Cracked / recordsCount * 100;
            defectsStatisticsPercent.Balast = defectsStatistics.Balast / recordsCount * 100;
            return defectsStatisticsPercent;                  
        }

        private RecordsStatistics CalculateStatistics(List<Record> records)
        {
            RecordsStatistics recordsStatistic = new RecordsStatistics();

            recordsStatistic.ImagesCount = records.Count(x => !x.NotSet);
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

    }
}
