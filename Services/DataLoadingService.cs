using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.AOD.NeuralNetworkDataManager.Model;
using VI.AOD.NeuralNetworkDataManager.Services;


namespace VI.AOD.NeuralNetworkDataManager.Services
{
    public static class DataLoadingService
    {
        public static List<Record> GetRecords(string selectedPath, bool fromFile = true)
        {
            
            if (fromFile) return GetRecordsFromFile(selectedPath);

            return GetRecordsFromDisc(selectedPath);
        }

        public static List<Record> GetRecordsFromDisc(string selectedPath)
        {
            List<Record> records = new List<Record>();
                    
            var files = Directory.EnumerateFiles(selectedPath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg"));

            foreach (string filePath in files)
            {
                records.Add(new Record
                {
                    FileName = Path.GetFileName(filePath),
                    FileFullPath = filePath,
                    Normal = filePath.IsNormal(),
                    Chipped = filePath.IsChipped(),
                    Cracks = filePath.IsCracks(),
                    BallastFull = filePath.IsBallastFull(),
                    BallastPartial = filePath.IsBallastPartial(),
                    Skewed = filePath.IsSkewed(),
                    Turnout = filePath.IsTurnout(),
                    Side = filePath.ExcludeSide()
                });
            }

            records = new List<Record>(records.OrderBy(x => x.FrameNo).OrderBy(x=>x.Session));

            return records;
        }

        public static List<Record> GetRecordsFromFile(string selectedPath)
        {
           bool header = true; 
            List<Record> records = new List<Record>();

            using (var reader = new StreamReader(selectedPath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (header)
                    {
                        header = false;
                        continue;
                    }
                    
                    Record record = new Record();                   
                    var values = line.Split(';');

                    record.FileName = values[0];
                    record.FileFullPath = Path.Combine(Path.GetDirectoryName(selectedPath), record.FileName);
                    record.Normal =  values[1] == "1" ? true : false ;
                    record.Chipped = values[2] == "1" ? true : false;
                    record.CriticalChipped = values[3] == "1" ? true : false;
                    record.Cracks = values[4] == "1" ? true : false;
                    record.CriticalCracks = values[5] == "1" ? true : false; 
                    record.BallastFull = values[6] == "1" ? true : false;
                    record.BallastPartial = values[7] == "1" ? true : false;
                    record.Skewed = values[8] == "1" ? true : false;
                    record.CriticalSkewed = values[9] == "1" ? true : false;
                    record.Turnout = values[10] == "1" ? true : false;
                    record.Side = values[11];
                    record.Flipped = values[12] == "1" ? true : false;
                    record.SuccessfulSegmentation = values[13] == "1" ? true : false;
                    record.SleeperNo = values.Count()>14 ? (string.IsNullOrWhiteSpace(values[14]) ? -1 : int.Parse(values[14])) : -1;
                    record.SleeperParent = values.Count() > 15 ? (string.IsNullOrWhiteSpace(values[15]) ? -1 : int.Parse(values[15])) : -1;
                    records.Add(record); 
                    
                }
            }

            records = new List<Record>(records.OrderBy(x => x.FrameNo).OrderBy(x => x.Session));
            return records;
        }
    }
}
