using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler
{
    public class MatrixCalculationResult
    {
        public int destinationIndex { get; set; }
        public int originIndex { get; set; }
        public double totalWalkDuration { get; set; }
        public double travelDistance { get; set; }
        public double travelDuration { get; set; }
    }
}
