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
using PathScheduler.Models;

namespace PathScheduler
{
    /// <summary>
    /// Interaction logic for MatrixView.xaml
    /// </summary>
    public partial class MatrixView : Window
    {
        DistanceMatrixResponse _distanceMatrixResponse;
        public MatrixView(DistanceMatrixResponse distanceMatrixResponse, List<MapPoint> mapPoints)
        {
            InitializeComponent();
            this._distanceMatrixResponse = distanceMatrixResponse;
            List<List<string>> distanceMatrix = ConvertResponseToMatrix(_distanceMatrixResponse);
            List<List<string>> distanceMatrixWithNames = AddNamesToDistanceMatrix(distanceMatrix, mapPoints);
            this.pointsMatrixView.ItemsSource = distanceMatrixWithNames;
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private List<List<string>> ConvertResponseToMatrix(DistanceMatrixResponse matrixResponse)
        {
            GeoData[] destinations = matrixResponse.resourceSets[0].resources[0].destinations.ToArray();
            GeoData[] origins = matrixResponse.resourceSets[0].resources[0].origins.ToArray();
            MatrixCalculationResult[] results = matrixResponse.resourceSets[0].resources[0].results.ToArray();

            List<List<string>> distanceMatrix = new List<List<string>>();

            int resultsCounter = 0;
            for (int i = 0; i < destinations.Length; ++i)
            {
                List<string> distanceRow = new List<string>();
                for (int j = 0; j < origins.Length; ++j)
                {
                    distanceRow.Add(results[resultsCounter].travelDistance.ToString());
                    resultsCounter++;
                }
                distanceMatrix.Add(distanceRow);
            }
            return distanceMatrix;
        }

        private List<List<string>> AddNamesToDistanceMatrix(List<List<string>> distanceMatrix, List<MapPoint> mapPoints)
        {
            List<List<string>> distanceMatrixWithNames = new List<List<string>>(distanceMatrix);
            List<string> firstRow = new List<string>();
            firstRow.Add("pkt/km");
            for (int i = 0; i < mapPoints.Count; ++i)
            {
                firstRow.Add(mapPoints[i].Name);
                distanceMatrixWithNames[i].Insert(0, mapPoints[i].Name);
            }
            distanceMatrixWithNames.Insert(0, firstRow);
            return distanceMatrixWithNames;
        }
}
}
