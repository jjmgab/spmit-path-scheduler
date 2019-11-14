using PathScheduler.Helpers;
using PathScheduler.Models;
using System.Data;
using System.Linq;
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

            this.Focus();

            this._dataSource = dataSource;
            this.listViewEntries.ItemsSource = this._dataSource.Points;
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

            this._dataSource.AddPoint(point);

            // rerender the view
            this.listViewEntries.Items.Refresh();
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel == false)
            {
                e.Cancel = true;
            }
            this.Hide();
        }
    }
}
