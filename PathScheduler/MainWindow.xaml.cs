using PathScheduler.Helpers;
using PathScheduler.Models;
using System.Windows;
using System.Windows.Input;

namespace PathScheduler
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Point data source.
        /// </summary>
        private PointDataSource<MapPoint> _dataSource;

        /// <summary>
        /// Handler to the EntryList window.
        /// </summary>
        private EntryList _entryListWindow;
        private MapView _mapViewWindow;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _dataSource = new MockupDataGen(30, 50, 15, 54, 23);
        }

        /// <summary>
        /// On entryListButton click. Opens the EntryList window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryListButton_Click(object sender, RoutedEventArgs e)
        {
            if (_entryListWindow == null)
            {
                if (_mapViewWindow == null)
                {
                    _mapViewWindow = new MapView(_dataSource);
                }
                _entryListWindow = new EntryList(_dataSource, _mapViewWindow);
            }
            this.IsEnabled = false;
            this._entryListWindow.ShowDialog();
            this.IsEnabled = true;
        }

        /// <summary>
        /// On mapListButton click. Opens the Map window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mapViewWindow == null)
            {
                _mapViewWindow = new MapView(_dataSource);
            }
            this.IsEnabled = false;
            this._mapViewWindow.ShowDialog();
            this.IsEnabled = true;
        }

        /// <summary>
        /// On closeButton click. Closes the window (and the application).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._entryListWindow != null)
            {
                this._entryListWindow.Close();
            }
            this.Close();
        }

        /// <summary>
        /// On upper dockPanel left mouse button press.
        /// Allows dragging the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// On maximizeButton click. Maximizes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
