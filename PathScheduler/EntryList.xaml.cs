using PathScheduler.Helpers;
using PathScheduler.Models;
using System.Data;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace PathScheduler
{
    /// <summary>
    /// Logika interakcji dla klasy EntryList.xaml
    /// </summary>
    public partial class EntryList : Window
    {
        /// <summary>
        /// Reference to the data source.
        /// </summary>
        PointDataSource<MapPoint> _dataSource;
        MapView _map;
        MatrixView _matrixViewWindow;
        string _apiKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSource">Reference to the data source.</param>
        public EntryList(PointDataSource<MapPoint> dataSource, MapView map)
        {
            InitializeComponent();

            this._map = map;
            this._dataSource = dataSource;
            this.listViewEntries.ItemsSource = this._dataSource.Points;
            this._apiKey = "AkCCTWkX8-FpNuz3LXlVFG5yrQBq2R6p2Efl2TXG4vXSBu4k0OxvLwgCjO5G5TZK";
        }

        /// <summary>
        /// On acceptButton click. Hides the window, without destroying it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// On addButton click. Adds a new point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBoxName.Text == "")
            {
                return;
            }

            MapPoint point = CheckAddedPointValidity(new MapPoint
            {
                Name = this.textBoxName.Text,
                CoordX = this.upDownX.Value == null ? 0 : (int)this.upDownX.Value,
                CoordY = this.upDownY.Value == null ? 0 : (int)this.upDownY.Value
            });

            if (point == null)
            {
                this.ClearInputControls();
                return;
            }

            this._map.AddPin(point.CoordX, point.CoordY);
            this._dataSource.AddPoint(point);

            // rerender the view
            this.listViewEntries.Items.Refresh();
        }

        private void viewMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixViewWindow == null)
            {
                _matrixViewWindow = new MatrixView(GetDistanceMatrix(_dataSource.Points), _dataSource.Points);
            }
            // this.IsEnabled = false;
            this._matrixViewWindow.ShowDialog();
            // this.IsEnabled = true;
        }

        /// <summary>
        /// Clears point input controls.
        /// </summary>
        private void ClearInputControls()
        {
            this.textBoxName.Text = "";
            this.upDownX.Value = 0;
            this.upDownY.Value = 0;
        }

        /// <summary>
        /// Called when window visibility was changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Hidden)
            {
                this.ClearInputControls();
            }
        }

        /// <summary>
        /// Verify if the point is eligible to be added.
        /// Checks for name or (both) coordinate duplicates.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private MapPoint CheckAddedPointValidity(MapPoint point)
        {
            // check for name
            MapPoint nameDuplPoint = (from p in this._dataSource.Points
                                      where p.Name == point.Name
                                      select p).FirstOrDefault();
            if (nameDuplPoint != null)
            {
                MessageBox.Show("Istnieje już punkt o takiej nazwie", "Błąd: duplikat", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            // check for duplicate coordinates
            MapPoint nameDuplCoords = (from p in this._dataSource.Points
                                       where p.CoordX == point.CoordX && p.CoordY == point.CoordY
                                       select p).FirstOrDefault();
            if (nameDuplCoords != null)
            {
                MessageBox.Show("Istnieje już punkt o takich współrzędnych", "Błąd: duplikat", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return point;
        }

        private DistanceMatrixResponse GetDistanceMatrix(List<MapPoint> points)
        {
            List<GeoData> geoPoints = GetGeoDataFromMapPointsList(points);
            string geoPointsString = CreateGeoPointsString(geoPoints);
            string requestUrl = CreateDistanceMatrixRequestUrl(geoPointsString, "driving", _apiKey);
            WebRequest matrixWebRequest = WebRequest.Create(requestUrl);
            Stream matrixDataStream = matrixWebRequest.GetResponse().GetResponseStream();
            var dataReader = new StreamReader(matrixDataStream);
            string matrixResponseString = dataReader.ReadToEnd();
            return JsonConvert.DeserializeObject<DistanceMatrixResponse>(matrixResponseString);
        }

        private List<GeoData> GetGeoDataFromMapPointsList(List<MapPoint> points)
        {
            List<GeoData> pointsGeoData = new List<GeoData>();
            foreach (var point in points)
            {
                pointsGeoData.Add(new GeoData(point.CoordX, point.CoordY));
            }
            return pointsGeoData;
        }

        private string CreateGeoPointsString(List<GeoData> geoPoints)
        {
            NumberFormatInfo format = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
            List<string> geoPointsStrings = new List<string>();
            foreach (var geoPoint in geoPoints)
            {
                geoPointsStrings.Add(geoPoint.latitude.ToString(format) + "," + geoPoint.longitude.ToString(format));
            }
            return string.Join(";", geoPointsStrings);
        }

        private string CreateDistanceMatrixRequestUrl(string geoPointsString, string travelMode, string apiKey)
        {
            return "https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix"
                    + "?origins="
                    + geoPointsString
                    + "&destinations="
                    + geoPointsString
                    + "&travelMode="
                    + travelMode
                    + "&key="
                    + apiKey;
        }
    }
}
