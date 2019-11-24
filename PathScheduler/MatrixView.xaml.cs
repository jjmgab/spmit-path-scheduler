using System;
using System.Collections.Generic;
using System.Data;
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
using PathScheduler.Helpers;
using PathScheduler.Models;

namespace PathScheduler
{
    /// <summary>
    /// Interaction logic for MatrixView.xaml
    /// </summary>
    public partial class MatrixView : Window
    {
        public MatrixView()
        {
            InitializeComponent();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add();
            foreach (string label in MapDataSource.DistanceMatrix.Labels)
            {
                dataTable.Columns.Add();
            }

            dataTable.Rows.Add(new string[] { "" }.Concat(MapDataSource.DistanceMatrix.Labels).ToArray());

            for (int i = 0; i < MapDataSource.Points.Count; i++)
            {
                string[] label = new string[] { MapDataSource.DistanceMatrix.Labels[i] };
                string[] row = Enumerable.Range(0, MapDataSource.DistanceMatrix.Matrix.GetLength(1))
                                .Select(x => MapDataSource.DistanceMatrix.Matrix[i, x].ToString())
                                .ToArray();
                
                dataTable.Rows.Add(label.Concat(row).ToArray());
            }

            dataGridMatrix.ItemsSource = dataTable.DefaultView;
        }
    }
}
