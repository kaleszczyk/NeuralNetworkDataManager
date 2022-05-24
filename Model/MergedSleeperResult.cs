using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VI.AOD.NeuralNetworkDataManager.Model
{
    public class MergedSleeperResult
    {
        public SleeperImage OuterA;
        public SleeperImage OuterB;
        public SleeperImage InnerA;
        public SleeperImage InnerB;
        public Bitmap SleeperImage;
        public string FileName;
        public string FilePath;
        public Record RecordResult;

        public MergedSleeperResult(List<SleeperImage> sleeperImages, int index, string destinationFileName, bool saveImages)
        {   
            SetImages(sleeperImages);
            SetFileName(sleeperImages.FirstOrDefault().FileName, index, destinationFileName);
            if (saveImages)
            {
                SetMergedImage();
            }

            SetRecordResult();
        }

        private void SetFileName(string orgFileName, int index, string destinationFileName)
        {
            var splited = orgFileName.Split('_');
            var newName = string.Join("_", splited[0], splited[1], splited[2], splited[3], splited[4], index.ToString());
            FileName = $"{newName}.png";

            FilePath = Path.Combine(destinationFileName, FileName);
        }

        private void SetMergedImage()
        {
            var outA = OuterA != null ? new Bitmap(OuterA.FilePath) : null;
            var outB = OuterB != null ? new Bitmap(OuterB.FilePath) : null;
            var innA = InnerA != null ? new Bitmap(InnerA.FilePath) : null;
            var innB = InnerB != null ? new Bitmap(InnerB.FilePath) : null;

            List<Bitmap> images = new List<Bitmap>();
            if (outA != null)
            {
                images.Add(outA);
            }

            if (outB != null)
            {
                images.Add(outB);
            }

            if (innA != null)
            {
                images.Add(innA);
            }

            if (innB != null)
            {
                images.Add(innB);
            }
            var height = images.Max(x => x.Height);
            var width = images.Sum(x => x.Width);
            Bitmap mergedImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(mergedImage))
            {
                int start = 0;
                g.Clear(Color.Black);

                foreach (var image in images)
                {
                    g.DrawImage(image, new Point(start, 0));
                    start += image.Width;
                }
            }

            SleeperImage = new Bitmap(mergedImage, width / 2, height / 2);
            if (!File.Exists(FilePath))
            {
                SleeperImage.Save(FilePath);
            }
            outA?.Dispose();
            outB?.Dispose();
            innA?.Dispose();
            innB?.Dispose();
        }

        private void SetImages(List<SleeperImage> sleeperImages)
        {
            var inners = sleeperImages?.Where(x => x.FileName.Contains("inner"));
            InnerA = inners?.FirstOrDefault(x => !x.FileName.Contains("B"));
            InnerB = inners?.FirstOrDefault(x => x.FileName.Contains("B"));

            var outers = sleeperImages?.Where(x => x.FileName.Contains("outer"));
            OuterA = outers?.FirstOrDefault(x => !x.FileName.Contains("B"));
            OuterB = outers?.FirstOrDefault(x => x.FileName.Contains("B"));
        }

        private void SetRecordResult()
        {
            RecordResult = new Record();

            RecordResult.FileName = FileName;
            RecordResult.FileFullPath = FilePath;

            if ((InnerA?.Record.Normal ?? true) && (InnerB?.Record.Normal ?? true) && (OuterA?.Record.Normal ?? true) && (OuterB?.Record.Normal ?? true))
            {
                RecordResult.Normal = true;
                return;
            }

            if ((InnerA?.Record.Chipped ?? false) || (InnerB?.Record.Chipped ?? false) || (OuterA?.Record.Chipped ?? false) || (OuterB?.Record.Chipped ?? false))
            {
                RecordResult.Chipped = true;
            }

            if ((InnerA?.Record.CriticalChipped ?? false) || (InnerB?.Record.CriticalChipped ?? false) || (OuterA?.Record.CriticalChipped ?? false) || (OuterB?.Record.CriticalChipped ?? false))
            {
                RecordResult.CriticalChipped = true;
            }

            if ((InnerA?.Record.Cracks ?? false) || (InnerB?.Record.Cracks ?? false) || (OuterA?.Record.Cracks ?? false) || (OuterB?.Record.Cracks ?? false))
            {
                RecordResult.Cracks = true;
            }

            if ((InnerA?.Record.CriticalCracks ?? false) || (InnerB?.Record.CriticalCracks ?? false) || (OuterA?.Record.CriticalCracks ?? false) || (OuterB?.Record.CriticalCracks ?? false))
            {
                RecordResult.CriticalCracks = true;
            }


            if ((InnerA?.Record.BallastFull ?? true) && (InnerB?.Record.BallastFull ?? true) && (OuterA?.Record.BallastFull ?? true) && (OuterB?.Record.BallastFull ?? true))
            {
                RecordResult.BallastFull = true;
            }

            if (!RecordResult.BallastFull && ((InnerA?.Record.BallastPartial ?? false) || (InnerB?.Record.BallastPartial ?? false) || (OuterA?.Record.BallastPartial ?? false) || (OuterB?.Record.BallastPartial ?? false)))
            {
                RecordResult.BallastPartial = true;
            }
        }
    }
}
