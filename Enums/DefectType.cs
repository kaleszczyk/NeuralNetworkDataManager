using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VI.AOD.NeuralNetworkDataManager.Enums
{
    public enum DefectType
    {
        Summary = 0,
        Normal = 1,
        Chipping = 2,
        Crack = 3,
        Ballast = 4,
        CrackWithoutBallast = 5,
        ChippedWithoutBallast = 6,
        GeneralDefect = 7,
        GeneralDefectWithoutBallast = 8
    }
}
