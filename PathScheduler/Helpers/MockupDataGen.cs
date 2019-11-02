using PathScheduler.Models;
using System;

namespace PathScheduler.Helpers
{
    /// <summary>
    /// Used to generate a grid of points and handle distance matrix generation.
    /// </summary>
    public class MockupDataGen : PointDataSource<MapPoint>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pointNumber">Target number of points.</param>
        /// <param name="minX">Minimal X-coordinate value.</param>
        /// <param name="minY">Minimal Y-coordinate value.</param>
        /// <param name="maxX">Maximal X-coordinate value.</param>
        /// <param name="maxY">Maximal Y-coordinate value.</param>
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

        /// <summary>
        /// Generates the distance matrix, where distance is
        /// the Euclidean distance between points (MapPoint) lying
        /// on the Cartesian coordinate plane.
        /// </summary>
        protected override void GenerateDistanceMatrix()
        {
            int pointNumber = _pointList.Count;
            _distanceMatrix = new double[pointNumber, pointNumber];

            for (int x = 0; x < pointNumber; x++)
            {
                MapPoint pointA = _pointList[x];
                for (int y = 0; y < pointNumber; y++)
                {
                    MapPoint pointB = _pointList[y];
                    _distanceMatrix[x, y] = Math.Sqrt(Math.Pow(pointB.CoordX - pointA.CoordX, 2) + Math.Pow(pointB.CoordY - pointA.CoordY, 2));
                }
            }
        }
    }
}
