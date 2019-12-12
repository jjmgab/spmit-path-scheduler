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
using PathScheduler.Helpers;
using Syncfusion.Windows.Shared;
using System.IO;

namespace PathScheduler
{
    /// <summary>
    /// Interaction logic for SA_Algorithm.xaml
    /// </summary>
    public partial class SA_Algorithm : Window
    {
        private double[,] _distanceMatrix;
        SA_Solution _solution;
        StreamWriter loggerFile;
        public SA_Algorithm()
        {
            this._distanceMatrix = MapDataSource.DistanceMatrix.Matrix;

            try
            {
                if (File.Exists(@"logs.txt"))
                {
                    File.Delete(@"logs.txt");
                    Console.WriteLine("File deleted.");
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
            this.loggerFile = new StreamWriter(@"logs.txt");
            InitializeComponent();
        }

        private void confirmCalulation_Click(object sender, RoutedEventArgs e)
        {
            if (tempValue.Value != null && maxIterationsValue.Value != null && tempDecresasingValue.Value != null && algorithmRepetitionValue.Value != null)
            {
                double? initTemp = tempValue.Value;
                double? tempDecreasingCoefficient = tempDecresasingValue.Value;
                long? maxRepetitionsWithoutImprovement = maxIterationsValue.Value;
                long? algorithmRepetitions = algorithmRepetitionValue.Value;

                resultView.Text = "(calculating)";
                resultView.InvalidateVisual();


                _solution = runSaAlgorithmRepetitively(initTemp, tempDecreasingCoefficient, maxRepetitionsWithoutImprovement, algorithmRepetitions);
                double result = _solution.objectiveFunction(_distanceMatrix);
                viewMapButton.IsEnabled = true;
                resultView.Text = result.ToString() + " km";
                resultView.InvalidateVisual();
            } else
            {
                resultView.Text = "Uzupełnij dane";
            }

        }

        private SA_Solution runSaAlgorithmRepetitively(double? initTemp, double? tempDecreasingCoefficient, long? maxRepetitionsWithoutImprovement, long? algorithmRepetitions)
        {
            SA_Solution bestSolution = runSaAlgorithm(initTemp, tempDecreasingCoefficient, maxRepetitionsWithoutImprovement);
            for (int i = 0; i < algorithmRepetitions - 1; ++i)
            {
                SA_Solution solution = runSaAlgorithm(initTemp, tempDecreasingCoefficient, maxRepetitionsWithoutImprovement);
                if (solution.objectiveFunction(_distanceMatrix) < bestSolution.objectiveFunction(_distanceMatrix))
                {
                    bestSolution = solution;
                }
            }
            return bestSolution;
        }

        private SA_Solution runSaAlgorithm(double? initTemp, double? tempDecreasingCoefficient, long? maxRepetitionsWithoutImprovement)
        {
            Random random = new Random();

            SA_Solution initSolution = new SA_Solution(_distanceMatrix.GetLength(0), true, random);
            SA_Solution oldSolution = initSolution;
            SA_Solution bestSolution = new SA_Solution(initSolution.getCopyOfPath());

            double? temp = initTemp;
            double oldResult;
            double newResult;

            int repetitionsWithoutImprovement = 0;

            while (repetitionsWithoutImprovement < maxRepetitionsWithoutImprovement)
            {
                SA_Solution newSolution = oldSolution.createNewSolutionWithSwappedTwoElements(random);
                oldResult = oldSolution.objectiveFunction(_distanceMatrix);
                newResult = newSolution.objectiveFunction(_distanceMatrix);

                logger(loggerFile, oldSolution.getCopyOfPath(), newSolution.getCopyOfPath(), oldResult, newResult);

                if (newResult < bestSolution.objectiveFunction(_distanceMatrix))
                {
                    bestSolution = newSolution;
                }

                if (newResult < oldResult)
                {
                    oldSolution = newSolution;
                    repetitionsWithoutImprovement = 0;
                }
                else if (random.NextDouble() < functionP(oldResult, newResult, temp))
                {
                    oldSolution = newSolution;
                    repetitionsWithoutImprovement++;
                }
                else
                {
                    repetitionsWithoutImprovement++;
                }
                temp *= tempDecreasingCoefficient;
            }
            return bestSolution;
        }

        private void logger (System.IO.StreamWriter file, List<int> oldSolutionP, List<int> newSolutionP, double oldResult, double newResult)
        {
            string oldSolutionPath = "";
            string newSolutionPath = "";

            for (int i = 0; i < oldSolutionP.Count(); ++i)
            {
                oldSolutionPath += oldSolutionP[i].ToString() + ">";
                newSolutionPath += newSolutionP[i].ToString() + ">";
            }

            file.WriteLine("OLD: " + oldSolutionPath + "(" + oldResult.ToString() + " km)" + "\nNEW: " + newSolutionPath + "(" + newResult.ToString() + " km)\n\n");
        }

        private double functionP(double oldResult, double newResult, double? temp)
        {
            double? toExp = -((newResult - oldResult) / temp);
            return Math.Exp(toExp ?? 0);
        }

        private void viewMapForSolution_Click(object sender, RoutedEventArgs e)
        {
            MapDataSource.MapViewWindow.clearRoutes();
            List<int> path = _solution.getCopyOfPath();
            GeoData[] points = MapDataSource.DistanceMatrix.GeoPoints;

            for (int i = 0; i < path.Count() - 1; ++i)
            {
                GeoData point1 = points[path[i]];
                GeoData point2 = points[path[i + 1]];
                MapDataSource.MapViewWindow.drawRouteForLocation(point1.latitude, point1.longitude, point2.latitude, point2.longitude, getPathColor(path, i, i + 1));
            }

            GeoData retPoint1 = points[path[path.Count() - 1]];
            GeoData retPoint2 = points[path[0]];
            MapDataSource.MapViewWindow.drawRouteForLocation(retPoint1.latitude, retPoint1.longitude, retPoint2.latitude, retPoint2.longitude, getPathColor(path, path.Count() - 1, 0));

            MapDataSource.MapViewWindow.Show();
        }

        private Color getPathColor(List<int> path, int ind1, int ind2)
        {
            Color pathColor;
            if (path[ind1] == 0)
            {
                pathColor = Colors.Orange;
            }
            else if (path[ind2] == 0)
            {
                pathColor = Colors.Red;
            }
            else
            {
                pathColor = Colors.Blue;
            }
            return pathColor;
        }
    }
}
