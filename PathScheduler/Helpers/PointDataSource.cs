using PathScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler.Helpers
{
    public class PointDataSource
    {
        protected List<MapPoint> _pointList;

        public List<MapPoint> MapPoints { get { return _pointList; } }

        public PointDataSource() {
            this._pointList = new List<MapPoint>();
        }

        
    }
}
