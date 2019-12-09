using PathScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PathScheduler.Helpers
{
    public static class MapDataSource
    {
        private static List<MapPoint> _points;
        public static List<MapPoint> Points
        {
            get
            {
                PedanticCheck();
                return _points;
            }
        }

        private static MapView _mapView;
        public static MapView MapViewWindow
        {
            get
            {
                PedanticCheck();
                return _mapView;
            }
        }

        private static EntryList _entryList;
        public static EntryList EntryListWindow
        {
            get
            {
                PedanticCheck();
                return _entryList;
            }
        }

        private static SA_Algorithm _saAlgorithm;
        public static SA_Algorithm SA_AlgorithmWindow
        {
            get
            {
                PedanticCheck();
                return _saAlgorithm;
            }
        }
        public abstract class DistanceMatrix
        {
            private static DistanceMatrixResponse _distanceMatrixResponse;

            public static void SetDistanceMatrixResponse(DistanceMatrixResponse response)
            {
                _distanceMatrixResponse = response;
                _matrix = ConvertResponseToMatrix(response);
                _labels = GenerateLabels();
            }

            private static double[,] _matrix;
            public static double[,] Matrix
            {
                get
                {
                    PedanticCheck();
                    return _matrix;
                }
            }

            private static GeoData[] _geoPoints;
            public static GeoData[] GeoPoints
            {
                get
                {
                    PedanticCheck();
                    return _geoPoints;
                }
            }

            private static string[] _labels;
            public static string[] Labels
            {
                get
                {
                    PedanticCheck();
                    return _labels;
                }
            }

            private static double[,] ConvertResponseToMatrix(DistanceMatrixResponse matrixResponse)
            {
                GeoData[] destinations = matrixResponse.resourceSets[0].resources[0].destinations.ToArray();
                _geoPoints = destinations;
                GeoData[] origins = matrixResponse.resourceSets[0].resources[0].origins.ToArray();
                MatrixCalculationResult[] results = matrixResponse.resourceSets[0].resources[0].results.ToArray();

                double[,] distanceMatrix = new double[destinations.Length, destinations.Length];

                int resultsCounter = 0;
                for (int i = 0; i < destinations.Length; ++i)
                {
                    for (int j = 0; j < origins.Length; ++j)
                    {
                        distanceMatrix[i, j] = results[resultsCounter].travelDistance;
                        resultsCounter++;
                    }
                }
                return distanceMatrix;
            }

            private static string[] GenerateLabels()
            {
                string[] labels = new string[_points.Count];

                for (int i = 0; i < _points.Count; ++i)
                    labels[i] = _points[i].Name;
                return labels;
            }
        }

        private static bool _isInit = false;

        public static void Initialize(bool generateRandomPoints = false)
        {
            _isInit = true;

            if (generateRandomPoints)
            {
                _points = new MockupDataGen(30, 50, 15, 54, 23).Points;
                GenerateDistanceMatrix();
            }
            else
                _points = new List<MapPoint>();

            _entryList = new EntryList();
            _mapView = new MapView();
            _saAlgorithm = new SA_Algorithm();
        }

        public static bool AddPoint(MapPoint point)
        {
            PedanticCheck();

            MapPoint newPoint = CheckAddedPointValidity(point);

            if (point == null)
                return false;

            _mapView.AddPin(point.CoordX, point.CoordY);
            _points.Add(newPoint);

            return true;
        }

        public static void GenerateDistanceMatrix()
        {
            PedanticCheck();
            DistanceMatrix.SetDistanceMatrixResponse(DistanceMatrixProvider.GetDistanceMatrix(_points));
        }

        public static void ShowDistanceMatrixDialog()
        {
            PedanticCheck();
            if (DistanceMatrix.Matrix != null)
            {
                MatrixView window = new MatrixView();
                window.ShowDialog();
            }
            else
                throw new InvalidOperationException("Distance matrix wasn't generated yet.");
        }

        private static void PedanticCheck()
        {
            if (!_isInit)
                throw new InvalidOperationException("Class MapDataSource was not initialized.");
        }

        /// <summary>
        /// Verify if the point is eligible to be added.
        /// Checks for name or (both) coordinate duplicates.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private static MapPoint CheckAddedPointValidity(MapPoint point)
        {
            // check for name
            MapPoint nameDuplPoint = (from p in _points
                                      where p.Name == point.Name
                                      select p).FirstOrDefault();
            if (nameDuplPoint != null)
            {
                MessageBox.Show("Istnieje już punkt o takiej nazwie", "Błąd: duplikat", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            // check for duplicate coordinates
            MapPoint nameDuplCoords = (from p in _points
                                       where p.CoordX == point.CoordX && p.CoordY == point.CoordY
                                       select p).FirstOrDefault();
            if (nameDuplCoords != null)
            {
                MessageBox.Show("Istnieje już punkt o takich współrzędnych", "Błąd: duplikat", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return point;
        }
    }
}
