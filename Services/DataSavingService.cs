using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.AOD.NeuralNetworkDataManager.Model;

namespace VI.AOD.NeuralNetworkDataManager.Services
{
    public class DataSavingService
    {
        //public static string header = "FileName;FilePath;Normal;Chipped;Cracks;BallastFull;BallastPartial;Skewed;Turnout;Side";
        public static string header = "FileName;Normal;Chipped;CriticalChipped;Cracks;CriticalCracks;BallastFull;BallastPartial;Skewed;CriticalSkewed;Turnout;Side;Flipped;SuccessfulSegmentation";
        public static void SaveRecords(List<Record> records, string selectedPath, bool addToexisting = false)
        {
            if (!File.Exists(selectedPath))
            {
                File.Create(selectedPath).Close();
            }
            var csv = new StringBuilder();

            if (!addToexisting)
            {
                csv.AppendLine(header);
            }

            foreach (var record in records)
            {              
                var newLine = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}",
                    record.FileName,
                   // record.FileFullPath,
                    record.Normal ? 1 : 0,
                    record.Chipped ? 1 : 0,
                    record.CriticalChipped ? 1 : 0,
                    record.Cracks ? 1 : 0,
                    record.CriticalCracks ? 1 : 0,
                    record.BallastFull ? 1 : 0,
                    record.BallastPartial ? 1 : 0,
                    record.Skewed ? 1 : 0,
                    record.CriticalSkewed ? 1 : 0,
                    record.Turnout ? 1 : 0,
                    record.Side, 
                    record.Flipped ? 1 : 0,
                    record.SuccessfulSegmentation ? 1 : 0
                    );
                csv.AppendLine(newLine);
            }

            if (addToexisting)
            {
                File.AppendAllText(selectedPath, csv.ToString());
            }
            else
            {
                File.WriteAllText(selectedPath, csv.ToString());
            }
        }

        public static void CopyImages(List<Record> records, string selectedPath)
        {
            if (!Directory.Exists(selectedPath))
            {
                Directory.CreateDirectory(selectedPath);
            }

            foreach (var record in records)
            {
                File.Copy(record.FileFullPath, Path.Combine(selectedPath, record.FileName), true);
            }
        }
    }
}
