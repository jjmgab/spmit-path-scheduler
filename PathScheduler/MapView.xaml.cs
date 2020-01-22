using System.Windows;
using System.Windows.Controls;
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
        }

        public void drawRouteForLocation(double lat1, double lng1, double lat2, double lng2, Color color)
        {
            string requestUrl = routeRequestUrl(lat1, lng1, lat2, lng2);

            WebRequest routeWebRequest = WebRequest.Create(requestUrl);
            Stream routeDataStream = routeWebRequest.GetResponse().GetResponseStream();
            var dataReader = new StreamReader(routeDataStream);
            string routeResponseString = dataReader.ReadToEnd();
            RouteResponse routeResponse = JsonConvert.DeserializeObject<RouteResponse>(routeResponseString);

            LocationCollection locs = new LocationCollection(); ;
            List<List<double>> coords = routeResponse.resourceSets[0].resources[0].routePath.Line.coordinates;

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
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 5
            };

            appMap.Children.Add(routeLine);
            // appMap.SetView(locs, new Thickness(30), 0);
        }

        public void clearRoutes()
        {
            UIElement component = null;
            for (int i = appMap.Children.Count - 1; i >= 0; i--)
            {
                component = appMap.Children[i];
                if (component.GetType() == typeof(MapPolyline))
                {
                    appMap.Children.Remove(component);
                }
            }
        }

        private string routeRequestUrl(double lat1, double lng1, double lat2, double lng2)
        {
            string loc1 = lat1.ToString() + "," + lng1.ToString();
            string loc2 = lat2.ToString() + "," + lng2.ToString();
            return "http://dev.virtualearth.net/REST/v1/Routes/Driving?"
                    + "wayPoint.1="
                    + loc1
                    + "&waypoint.2="
                    + loc2
                    + "&optimize=distance&routeAttributes=routePath&routePathOutput=Points&key=AkCCTWkX8-FpNuz3LXlVFG5yrQBq2R6p2Efl2TXG4vXSBu4k0OxvLwgCjO5G5TZK";
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
