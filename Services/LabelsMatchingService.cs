using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VI.AOD.NeuralNetworkDataManager.Services
{
    public static class LabelsMatchingService
    {
        public static bool IsNormal(this string filePath)
        {
            if (filePath.Contains("Normal") || filePath.Contains("normal")) return true;

            return false;

        }

        public static bool IsChipped(this string filePath)
        {
            if (filePath.Contains("Chip") || filePath.Contains("chip")) return true;

            return false;

        }
        public static bool IsCracks(this string filePath)
        {
            if (filePath.Contains("Crack") || filePath.Contains("crack")) return true;

            return false;

        }

        public static bool IsBallastFull(this string filePath)
        {
            if (filePath.Contains("ballastfull") || filePath.Contains("BallastFull") || filePath.Contains("ballast_full")) return true;

            return false;

        }

        public static bool IsBallastPartial(this string filePath)
        {
            if (filePath.Contains("ballastpartial") || filePath.Contains("BallastPartial") || filePath.Contains("ballast_partial")) return true;

            return false;

        }

        public static bool IsSkewed(this string filePath)
        {
            if (filePath.Contains("skew") || filePath.Contains("Skew") || filePath.Contains("przechylone") || filePath.Contains("Przechylone")) return true;

            return false;

        }

        public static bool IsTurnout(this string filePath)
        {
            if (filePath.Contains("turnout") || filePath.Contains("Turnout") || filePath.Contains("rozjazd") || filePath.Contains("Rozjazd")) return true;

            return false;

        }


        public static string ExcludeSide(this string filePath)
        {
            if (filePath.Contains("Inner") || filePath.Contains("inner") || Regex.Match(filePath, "1_[0-5].png$").Success || Regex.Match(filePath, "1_[0-5].jpg$").Success || Regex.Match(filePath, @"1_[0-5] \(2\).png$").Success || Regex.Match(filePath, @"1_[0-5] \(2\).jpg$").Success) return "inner";
            if (filePath.Contains("Outer") || filePath.Contains("outer") || Regex.Match(filePath, "0_[0-5].png$").Success || Regex.Match(filePath, "0_[0-5].jpg$").Success || Regex.Match(filePath, @"0_[0-5] \(2\).png$").Success || Regex.Match(filePath, @"0_[0-5] \(2\).jpg$").Success) return "outer";

            return "TO SET";

        }
    }
}
