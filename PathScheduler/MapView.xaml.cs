using System.Windows;
using PathScheduler.Helpers;
using PathScheduler.Models;
using Microsoft.Maps.MapControl.WPF;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace PathScheduler
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : Window
    {
        /// <summary>
        /// Reference to the data source.
        /// </summary>
        PointDataSource<MapPoint> _dataSource;

        public MapView(PointDataSource<MapPoint> dataSource)
        {
            this._dataSource = dataSource;
            InitializeComponent();
            InitPinsFromData();
            MatrixRequest();
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

        private void MatrixRequest()
        {
            string requestURL = "https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?origins=52.48,17.66;50.51,19.96;50.85,17.05&destinations=52.48,17.66;50.51,19.96;50.85,17.05&travelMode=driving&key=AkCCTWkX8-FpNuz3LXlVFG5yrQBq2R6p2Efl2TXG4vXSBu4k0OxvLwgCjO5G5TZK";
            WebRequest matrixWebRequest = WebRequest.Create(requestURL);
            Stream matrixDataStream = matrixWebRequest.GetResponse().GetResponseStream();
            var dataReader = new StreamReader(matrixDataStream);
            string matrixResponseString = dataReader.ReadToEnd();
            DistanceMatrixResponse matrixResponse = JsonConvert.DeserializeObject<DistanceMatrixResponse>(matrixResponseString);
        }

        private void InitPinsFromData()
        {
            _dataSource?.Points.ForEach(point =>
            {
                Pushpin pin = new Pushpin();
                pin.Location = new Location(point.CoordX, point.CoordY);
                this.appMap.Children.Add(pin);
            });
        }

        public void AddPin(double lat, double lng)
        {
            Pushpin pin = new Pushpin();
            pin.Location = new Location(lat, lng);
            this.appMap.Children.Add(pin);
        }
    }
}
