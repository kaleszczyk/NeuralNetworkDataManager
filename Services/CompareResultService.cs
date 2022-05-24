using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.AOD.NeuralNetworkDataManager.Enums;
using VI.AOD.NeuralNetworkDataManager.Model;

namespace VI.AOD.NeuralNetworkDataManager.Services
{
    public class CompareResultService
    {
        public List<Record> RealResultRecords;
        public List<Record> NeuralNetworkResultRecords;

        public void CompareResults(NeuralNetworkReport report, string resultPath, bool saveImages)
        {
            try
            {
                Dictionary<string, Record> realRecords = RealResultRecords.ToDictionary(x => x.FileName);
                Dictionary<string, Record> neuralNetworkResultRecords = NeuralNetworkResultRecords.ToDictionary(x => x.FileName);

                var files = RealResultRecords.Select(x => x.FileName);
                int i = 0;

                foreach (var file in files)
                {
                    if (neuralNetworkResultRecords.ContainsKey(file))
                    {
                        var realRecord = realRecords[file];
                        var nnRecord = neuralNetworkResultRecords[file];
                        i++;
                        SetRecordsForRecordsResult(nameof(report.GeneralResult), report.GeneralResult, realRecord, nnRecord);
                        SetRecordsForRecordsResult(nameof(report.CriticalResult), report.CriticalResult, realRecord, nnRecord);
                        //SetRecordsForRecordsResult(nameof(report.LowerResult), report.LowerResult, realRecord, nnRecord);
                        if (saveImages)
                        {
                            SetGeneralDefectResult(realRecord,  nnRecord, report.ImagesSource, resultPath, file, report.SuccessfullSegmentation);
                        }
                    }
                    else
                    {
                        report.MissingRecords += $"{file}\n";
                    }
                }

                report.ComparedRecords = i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetRecordsForRecordsResult(string type, Dictionary<DefectType, Result> results, Record realRecord, Record nnRecord)
        {
            ResultType result;
            var resultSummary = results[DefectType.Summary];

            var resultNormal = results[DefectType.Normal];

            result = GetResult(type, realRecord.Normal, false, nnRecord.Normal);
            resultSummary.GetResult(result);
            resultNormal.GetResult(result);
            //SaveResultImage(imagesPath, fileName, type, result, DefectType.Normal, successfullSegmentation);

            var resultChipping = results[DefectType.Chipping];
            result = GetResult(type, realRecord.Chipped, realRecord.CriticalChipped, nnRecord.Chipped);
            resultSummary.GetResult(result);
            resultChipping.GetResult(result);
            //SaveResultImage(imagesPath, fileName, type, result, DefectType.Chipping, successfullSegmentation);

            var resultCrack = results[DefectType.Crack];
            result = GetResult(type, realRecord.Cracks, realRecord.CriticalCracks, nnRecord.Cracks);
            resultSummary.GetResult(result);
            resultCrack.GetResult(result);
            //SaveResultImage(imagesPath, fileName, type, result, DefectType.Crack, successfullSegmentation);

            var resultBallast = results[DefectType.Ballast];
            result = GetResult(type, (realRecord.BallastPartial || realRecord.BallastFull), realRecord.BallastFull, nnRecord.BallastPartial);
            resultSummary.GetResult(result);
            resultBallast.GetResult(result);
            //SaveResultImage(imagesPath, fileName, type, result, DefectType.Ballast, successfullSegmentation);

            //var resultCrackWithoutBallast = results[DefectType.CrackWithoutBallast];
            //result = GetResult(type, !(realRecord.BallastPartial || realRecord.BallastFull) && realRecord.Cracks, !(realRecord.BallastPartial || realRecord.BallastFull) && realRecord.CriticalCracks, nnRecord.Cracks);
            //resultSummary.GetResult(result);
            //resultCrackWithoutBallast.GetResult(result);
            ////SaveResultImage(imagesPath, fileName, type, result, DefectType.Ballast, successfullSegmentation);

            //var resultChipWithoutBallast = results[DefectType.ChippedWithoutBallast];
            //result = GetResult(type, !(realRecord.BallastPartial || realRecord.BallastFull) && realRecord.Chipped, !(realRecord.BallastPartial || realRecord.BallastFull) && realRecord.CriticalChipped, nnRecord.Chipped);
            //resultSummary.GetResult(result);
            //resultChipWithoutBallast.GetResult(result);
            ////SaveResultImage(imagesPath, fileName, type, result, DefectType.Ballast, successfullSegmentation);

            var resultGeneralDefect = results[DefectType.GeneralDefect];
            result = GetResult(type, realRecord.IsDefected, (realRecord.IsDefected && realRecord.IsCritical), nnRecord.IsDefected);
            resultGeneralDefect.GetResult(result);
            //SaveResultImage(imagesPath, fileName, type, result, DefectType.Ballast, successfullSegmentation);

            var resultDefectWithoutBallast = results[DefectType.GeneralDefectWithoutBallast];
            result = GetResult(type, realRecord.IsDefectedWithoutBalasstedAsDefect, (realRecord.IsDefectedWithoutBalasstedAsDefect && realRecord.IsCriticalWithoutBalasstedAsDefect), nnRecord.IsDefectedWithoutBalasstedAsDefect);
            resultDefectWithoutBallast.GetResult(result);
            //SaveResultImage(imagesPath, fileName, type, result, DefectType.Ballast, successfullSegmentation);

        }

        private void SetGeneralDefectResult(Record realRecord, Record nnRecord, string imagesPath, string destinationDirectory, string fileName, bool successfullSegmentation)
        {
            var result = ResultType.None;
            var realResult = realRecord.IsDefected;
            var nnResult = nnRecord.IsDefected;

            if (!realResult && !nnResult)
            {
                result = ResultType.TN;
            }

            if (!realResult && nnResult)
            {
                result = ResultType.FP;
            }

            if (realResult && !nnResult)
            {
                result = ResultType.FN;
            }

            if (realResult && nnResult)
            {
                result = ResultType.TP;
            }

            if (realRecord.IsCritical)
            {
                SaveResultImage(imagesPath, destinationDirectory, fileName, "Critical", result, successfullSegmentation);
            }
            else
            {
                SaveResultImage(imagesPath, destinationDirectory, fileName, "General", result, successfullSegmentation);
            }
        }

        private ResultType GetResult(string type, bool realResult, bool realCriticalResult, bool nnResult)
        {
            switch (type)
            {
                case "GeneralResult":
                    {
                        return SetGeneralResult(realResult, realCriticalResult, nnResult);
                    }
                case "CriticalResult":
                    {
                        return SetCriticalResult(realResult, realCriticalResult, nnResult);
                    }
                case "LowerResult":
                    {
                        return SetLowerResult(realResult, realCriticalResult, nnResult);
                    }
                default:
                    return ResultType.None;
            }
        }

        public ResultType SetGeneralResult(bool realResult, bool realCriticalResult, bool nnResult)
        {
            if (!(realResult || realCriticalResult) && !nnResult)
            {
                return ResultType.TN;
            }

            if (!(realResult || realCriticalResult) && nnResult)
            {
                return ResultType.FP;
            }

            if ((realResult || realCriticalResult) && !nnResult)
            {
                return ResultType.FN;
            }

            if ((realResult || realCriticalResult) && nnResult)
            {
                return ResultType.TP;
            }

            return ResultType.None;
        }

        public ResultType SetCriticalResult(bool realResult, bool realCriticalResult, bool nnResult)
        {
            //if ((!realResult && !realCriticalResult) && !nnResult)
            //{
            //    return ResultType.TN;
            //}

            //if (!(realResult || realCriticalResult) && nnResult)
            //{
            //    return ResultType.FP;
            //}

            if (realCriticalResult && !nnResult)
            {
                return ResultType.FN;
            }

            if (realCriticalResult && nnResult)
            {
                return ResultType.TP;
            }

            return ResultType.None;
        }


        public ResultType SetLowerResult(bool realResult, bool realCriticalResult, bool nnResult)
        {
            if (!(realResult && !realCriticalResult) && !nnResult)
            {
                return ResultType.TN;
            }

            if (!(realResult && !realCriticalResult) && nnResult)
            {
                return ResultType.FP;
            }

            if ((realResult && !realCriticalResult) && !nnResult)
            {
                return ResultType.FN;
            }

            if ((realResult && !realCriticalResult) && nnResult)
            {
                return ResultType.TP;
            }

            return ResultType.None;
        }

        private void SaveResultImage(string sourcePath, string destinationDirectory, string fileName, string ResultType, ResultType result, bool successfullSegmentation)
        {
            if (result == Enums.ResultType.FN || result == Enums.ResultType.FP)
            {
                var source = Path.Combine(sourcePath, fileName);
                var destination = Path.Combine(destinationDirectory, $"Raport{(successfullSegmentation ? "SegementationOk" : "WrongSegmentation")}", ResultType, result.ToString(), fileName);
                Directory.CreateDirectory(Path.Combine(destinationDirectory, $"Raport{(successfullSegmentation ? "SegementationOk" : "WrongSegmentation")}", ResultType, result.ToString()));
                File.Copy(source, destination);
            }
        }
    }
}