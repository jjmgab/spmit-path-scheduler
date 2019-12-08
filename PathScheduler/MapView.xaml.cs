using System.Windows;
using PathScheduler.Helpers;
using PathScheduler.Models;
using Microsoft.Maps.MapControl.WPF;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Media;

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
            // MatrixRequest();
            RouteRequest();
        }

        //private void MatrixRequest()
        //{
        //    string requestURL = "https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?origins=52.48,17.66;50.51,19.96;50.85,17.05&destinations=52.48,17.66;50.51,19.96;50.85,17.05&travelMode=driving&key=AkCCTWkX8-FpNuz3LXlVFG5yrQBq2R6p2Efl2TXG4vXSBu4k0OxvLwgCjO5G5TZK";
        //    WebRequest matrixWebRequest = WebRequest.Create(requestURL);
        //    Stream matrixDataStream = matrixWebRequest.GetResponse().GetResponseStream();
        //    var dataReader = new StreamReader(matrixDataStream);
        //    string matrixResponseString = dataReader.ReadToEnd();
        //    DistanceMatrixResponse matrixResponse = JsonConvert.DeserializeObject<DistanceMatrixResponse>(matrixResponseString);
        //}

        private void RouteRequest()
        {
            string l1 = MapDataSource.Points[0].CoordX.ToString() + "," + MapDataSource.Points[0].CoordY.ToString();
            string l2 = MapDataSource.Points[1].CoordX.ToString() + "," + MapDataSource.Points[1].CoordY.ToString();

            // 52.48,17.66
            // 50.51,19.96

            string requestURL = "http://dev.virtualearth.net/REST/v1/Routes/Driving?wayPoint.1=" + l1 + "&waypoint.2=" + l2 + "&optimize=distance&routeAttributes=routePath&routePathOutput=Points&key=AkCCTWkX8-FpNuz3LXlVFG5yrQBq2R6p2Efl2TXG4vXSBu4k0OxvLwgCjO5G5TZK";
            WebRequest routeWebRequest = WebRequest.Create(requestURL);
            Stream routeDataStream = routeWebRequest.GetResponse().GetResponseStream();
            var dataReader = new StreamReader(routeDataStream);
            string routeResponseString = dataReader.ReadToEnd();
            RouteResponse routeResponse = JsonConvert.DeserializeObject<RouteResponse>(routeResponseString);

            LocationCollection locs = new LocationCollection(); ;
            List<List<double>> coords = routeResponse.resourceSets[0].resources[0].routePath.line.coordinates;

            for (int i = 0; i < coords.Count; i++)
            {
                if (coords[i].Count >= 2)
                {
                    locs.Add(new Microsoft.Maps.MapControl.WPF.Location(coords[i][0], coords[i][1]));
                }
            }

            MapPolyline routeLine = new MapPolyline()
            {
                Locations = locs,
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 5
            };

            appMap.Children.Add(routeLine);
            // appMap.SetView(locs, new Thickness(30), 0);
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
