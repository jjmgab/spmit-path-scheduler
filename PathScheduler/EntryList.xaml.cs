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
        private List<MapPoint> _pointList;

        public EntryList()
        {
            InitializeComponent();

            _pointList = new List<MapPoint>();

            Random rand = new Random();

            for (int i = 0; i < 30; i++)
            {
                _pointList.Add(new MapPoint
                {
                    Name = $"Punkt {i + 1}",
                    CoordX = -100 + rand.Next(200),
                    CoordY = -100 + rand.Next(200),
                });
            }

            listViewEntries.ItemsSource = _pointList;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _pointList.Add(new MapPoint
            {
                Name = "Punkt dodany ręcznie",
                CoordX = 0,
                CoordY = 0,
            });
            listViewEntries.Items.Refresh();
        }
    }
}
