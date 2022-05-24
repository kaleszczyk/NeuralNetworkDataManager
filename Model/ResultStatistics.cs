using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VI.AOD.NeuralNetworkDataManager.Model
{
    public class ResultStatistics
    {    
        public int RecordsCount;
        public DefectsStatistics GeneralStatistic;
        public DefectsStatistics CiticalStatistic;
        public DefectsStatistics LowerDefectsStatistic;

        public override string ToString()
        {
            string defectsStatistics = string.Empty;
            defectsStatistics = $"\n{nameof(RecordsCount)} - {RecordsCount}" +
                                $"\n{nameof(GeneralStatistic)}\n{GeneralStatistic.ToString()}" +
                                $"\n{nameof(CiticalStatistic)}\n{CiticalStatistic.ToString()}" +
                                $"\n{nameof(LowerDefectsStatistic)}\n{LowerDefectsStatistic.ToString()}\n";
            return defectsStatistics;
        }
    }
}
