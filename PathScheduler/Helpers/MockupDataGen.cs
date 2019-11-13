using PathScheduler.Models;
using System;
using System.Linq;

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

            GeneratePointList(pointNumber, minX, minY, maxX, maxY);
        }

        /// <summary>
        /// Generates a list of points.
        /// </summary>
        /// <param name="pointNumber">Target number of points.</param>
        /// <param name="minX">Minimal X-coordinate value.</param>
        /// <param name="minY">Minimal Y-coordinate value.</param>
        /// <param name="maxX">Maximal X-coordinate value.</param>
        /// <param name="maxY">Maximal Y-coordinate value.</param>
        private void GeneratePointList(int pointNumber, int minX, int minY, int maxX, int maxY)
        {
            Random rand = new Random();

            for (int i = 0; i < pointNumber; i++)
            {
                
                MapPoint newPoint = new MapPoint
                {
                    Name = $"Punkt {i + 1}",
                    CoordX = Math.Round((minX + rand.NextDouble() * (maxX - minX)), 6),
                    CoordY = Math.Round((minY + rand.NextDouble() * (maxY - minY)), 6),
                };

                // check for duplicate coordinates
                MapPoint nameDuplCoords = (from p in this._pointList
                                           where p.CoordX == newPoint.CoordX && p.CoordY == newPoint.CoordY
                                           select p).FirstOrDefault();

                // repeat current iteration if a duplicate was found
                if (nameDuplCoords != null)
                {
                    i--;
                    continue;
                }

                _pointList.Add(newPoint);
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
