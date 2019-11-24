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
        public MapView()
        {
            InitializeComponent();
            InitPinsFromData();
            MatrixRequest();
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
            MapDataSource.Points.ForEach(point =>
            {
                Pushpin pin = new Pushpin
                {
                    Location = new Location(point.CoordX, point.CoordY)
                };
                this.appMap.Children.Add(pin);
            });
        }

        public void AddPin(double lat, double lng)
        {
            Pushpin pin = new Pushpin
            {
                Location = new Location(lat, lng)
            };
            this.appMap.Children.Add(pin);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Normal;
                 
            Hide();
        }
    }
}
