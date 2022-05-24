using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.AOD.NeuralNetworkDataManager.Model;

namespace VI.AOD.NeuralNetworkDataManager.Services
{
    public class InitializeReordService
    {
        private struct RecordOrder
        {
            public Record Record;
            public int Frame;
            public int SleeperNo;
            public string Side;
            public int CameraFile;
            public int Camera;
            public int PartAB;
        }

        public List<SleeperImage> Intialize(string sourcePath, out string csvFilePath)
        {
            try
            {
                var csvFile = Directory.GetFiles(sourcePath).FirstOrDefault(x => x.Contains(".csv"));
                var csvFileWithIndexes = Directory.GetFiles(sourcePath).Where(x => x.Contains(".csv")).FirstOrDefault(x => x.Contains("WithIndexes"));
                var records = new List<Record>();
                var recordOrders = new List<RecordOrder>();

                if (!string.IsNullOrEmpty(csvFile))
                {
                    if (!string.IsNullOrWhiteSpace(csvFileWithIndexes))
                    {
                        records = DataLoadingService.GetRecordsFromFile(csvFileWithIndexes);
                        csvFilePath = csvFileWithIndexes;
                    }
                    else
                    {
                        records = DataLoadingService.GetRecordsFromFile(csvFile);
                        csvFilePath = csvFile;
                    }
                }
                else
                {
                    records = DataLoadingService.GetRecordsFromDisc(sourcePath);
                    csvFilePath = Path.Combine(sourcePath, "results.csv");
                    DuplicateDataSavingService.SaveRecords(records, csvFilePath);
                }

                foreach (var record in records)
                {
                    var recordOrder = SetOrderNumbers(record);
                    recordOrders.Add(recordOrder);
                }

                recordOrders = recordOrders.OrderBy(x => x.SleeperNo).OrderBy(x => x.Frame).OrderBy(x => x.CameraFile).ToList();
                int index = 0; 
                return ConvetToSleeperImage(recordOrders, ref index);
            }
            catch (Exception ex)
            {
                ;
            }
            csvFilePath = string.Empty;
            return new List<SleeperImage>();
        }

        public List<SleeperImage> Intialize(string csvFile)
        {
            try
            {
                var sourcePath = Path.GetDirectoryName(csvFile);
                var records = new List<Record>();
                var recordOrders = new List<RecordOrder>();

                if (!string.IsNullOrEmpty(csvFile))
                {    
                    records = DataLoadingService.GetRecordsFromFile(csvFile);   
                }

                foreach (var record in records)
                {
                    var recordOrder = SetOrderNumbers(record);
                    recordOrders.Add(recordOrder);
                }
                 
                var left = recordOrders.Where(x=>x.Camera==0).OrderBy(x => x.SleeperNo).OrderBy(x => x.Frame).OrderBy(x => x.CameraFile).ToList();
                var right = recordOrders.Where(x => x.Camera == 1).OrderBy(x => x.SleeperNo).OrderBy(x => x.Frame).OrderBy(x => x.CameraFile).ToList();

                int index = 0;
                List<SleeperImage> result = new List<SleeperImage>();

                result.AddRange(ConvetToSleeperImage(left, ref index));
                result.AddRange(ConvetToSleeperImage(right, ref index));

                return result;
            }
            catch (Exception ex)
            {
                ;
            }
            return new List<SleeperImage>();
        }

        private List<SleeperImage> ConvetToSleeperImage(List<RecordOrder> recordOrders, ref int index)
        {
            List<SleeperImage> sleeperImages = new List<SleeperImage>();
            int frame = 0;
            int number = 0;
            

            foreach (var item in recordOrders)
            {
                if (item.SleeperNo > number || item.Frame > frame)
                {
                    index++;
                }
                number = item.SleeperNo;
                frame = item.Frame;
                sleeperImages.Add(new SleeperImage(item.Record, index));
            }

            return sleeperImages;
        }


        private RecordOrder SetOrderNumbers(Record record)
        {
            RecordOrder recordOrder = new RecordOrder();

            try
            {
                var splited = record.FileName.Split('_');
                int frame = int.Parse(splited[1]);
                int number = int.Parse(splited[6].Replace(".png", "").Replace("A", "").Replace("B", ""));
                string side = splited[5];
                int cameraNo = int.Parse(splited[4]);
                int cameraFileNo = int.Parse(splited[3]);
                int part = splited[6].Contains("B") ? 1 : 0;

                recordOrder.Record = record;
                recordOrder.Frame = frame;
                recordOrder.CameraFile = cameraFileNo;
                recordOrder.Camera = cameraNo;
                recordOrder.SleeperNo = number;
                recordOrder.Side = side;
                recordOrder.PartAB = part;
            }
            catch (Exception ex)
            {
                ;
            }

            return recordOrder;
        }
    }
}
