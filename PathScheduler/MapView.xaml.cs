using System.Windows;
using PathScheduler.Helpers;
using PathScheduler.Models;
using Microsoft.Maps.MapControl.WPF;

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
