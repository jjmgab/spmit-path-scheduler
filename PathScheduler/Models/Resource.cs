using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler
{
    public class Resource
    {
        public string __type { get; set; }
        public List<Double> bbox { get; set; }
        public string id { get; set; }
        public string distanceUnit { get; set; }
        public string durationUnit { get; set; }
        public RoutePath routePath { get; set; }
        public List<GeoData> destinations { get; set; }
        public string errorMessage { get; set; }
        public List<GeoData> origins { get; set; }
        public List<MatrixCalculationResult> results { get; set; }
    }
}
