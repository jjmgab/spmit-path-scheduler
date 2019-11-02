using PathScheduler.Helpers;
using PathScheduler.Models;
using System.Windows;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSource">Reference to the data source.</param>
        public EntryList(PointDataSource<MapPoint> dataSource)
        {
            InitializeComponent();

            this._dataSource = dataSource;
            listViewEntries.ItemsSource = this._dataSource.Points;
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
            this._dataSource.AddPoint(new MapPoint
            {
                Name = $"Punkt {this._dataSource.Points.Count + 1} (dodany ręcznie)",
                CoordX = 0,
                CoordY = 0,
            });

            // rerender the view
            listViewEntries.Items.Refresh();
        }
    }
}
