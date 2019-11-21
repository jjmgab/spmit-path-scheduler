using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler
{
    public class GeoData
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

        public GeoData(double lat, double lng)
        {
            latitude = lat;
            longitude = lng;
        }
    }
}
