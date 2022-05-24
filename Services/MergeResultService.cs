using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.AOD.NeuralNetworkDataManager.Model;

namespace VI.AOD.NeuralNetworkDataManager.Services
{
    public class MergeResultForWholeSleeperService
    {
        private InitializeReordService initializeReordService;

        public MergeResultForWholeSleeperService()
        {
            initializeReordService = new InitializeReordService();
        }

        public List<Record>GetMergedRecords(string csvPath, string destination, bool saveImages)
        {
            var sleeperImages = initializeReordService.Intialize(csvPath);

            var sleepersByIndex = sleeperImages.GroupBy(x => x.Index);

            List<MergedSleeperResult> mergedSleepersResult = new List<MergedSleeperResult>();

            foreach (var sleeperByIndex in sleepersByIndex)
            {
                mergedSleepersResult.Add(new MergedSleeperResult(sleeperByIndex.ToList(), sleeperByIndex.Key, destination, saveImages));
            }

            return mergedSleepersResult.Select(x => x.RecordResult).ToList();
        }                                                                                                                     
    }
}
