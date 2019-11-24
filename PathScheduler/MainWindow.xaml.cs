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
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            CredentialsProvider.Initialize("AkCCTWkX8-FpNuz3LXlVFG5yrQBq2R6p2Efl2TXG4vXSBu4k0OxvLwgCjO5G5TZK");
            MapDataSource.Initialize(true);
        }

        private void MenuItemPointList_Click(object sender, RoutedEventArgs e)
        {
            MapDataSource.EntryListWindow.Show();
        }

        private void MenuItemMapView_Click(object sender, RoutedEventArgs e)
        {
            MapDataSource.MapViewWindow.Show();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}