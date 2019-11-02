using PathScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler.Helpers
{
    public class MockupDataGen : PointDataSource
    {
        public MockupDataGen(int pointNumber, int minX, int minY, int maxX, int maxY) : base()
        {
            if (pointNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pointNumber", "Values > 0 only allowed.");
            }
            if (minX >= maxX || minY >= maxY)
            {
                throw new ArgumentOutOfRangeException();
            }

            Random rand = new Random();

            for (int i = 0; i < pointNumber; i++)
            {
                _pointList.Add(new MapPoint
                {
                    Name = $"Punkt {i + 1}",
                    CoordX = minX + rand.Next(maxX - minX),
                    CoordY = minY + rand.Next(maxY - minY),
                });
            }
        }
    }
}
