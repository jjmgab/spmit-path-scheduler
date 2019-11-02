using PathScheduler.Helpers;
using PathScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PathScheduler
{
    /// <summary>
    /// Logika interakcji dla klasy EntryList.xaml
    /// </summary>
    public partial class EntryList : Window
    {
        PointDataSource _dataSource;

        public EntryList(PointDataSource dataSource)
        {
            InitializeComponent();

            this._dataSource = dataSource;
            listViewEntries.ItemsSource = this._dataSource.MapPoints;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this._dataSource.MapPoints.Add(new MapPoint
            {
                Name = "Punkt dodany ręcznie",
                CoordX = 0,
                CoordY = 0,
            });
            listViewEntries.Items.Refresh();
        }
    }
}
