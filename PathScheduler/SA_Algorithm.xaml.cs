using PathScheduler.Helpers;
using PathScheduler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PathScheduler
{
    /// <summary>
    /// Interaction logic for SA_Algorithm.xaml
    /// </summary>
    public partial class SA_Algorithm : Window
    {
        private double[,] _distanceMatrix;
        private SA_Solution _solution;
        private StreamWriter _loggerFile;

        public SA_Algorithm()
        {
            this._distanceMatrix = MapDataSource.DistanceMatrix.Matrix;
            InitializeComponent();
        }

        #region Window-specific event handlers
        private void confirmCalulation_Click(object sender, RoutedEventArgs e)
        {
            this._distanceMatrix = MapDataSource.DistanceMatrix.Matrix;

            if (tempValue.Value != null && maxIterationsValue.Value != null && tempDecresasingValue.Value != null && algorithmRepetitionValue.Value != null)
            {
                double? initTemp = tempValue.Value;
                double? tempDecreasingCoefficient = tempDecresasingValue.Value;
                long? maxRepetitionsWithoutImprovement = maxIterationsValue.Value;
                long? algorithmRepetitions = algorithmRepetitionValue.Value;

                resultView.Text = "(calculating)";
                resultView.InvalidateVisual();

                ClearLogger();

                _solution = RunMultipleTimes(initTemp, tempDecreasingCoefficient, maxRepetitionsWithoutImprovement, algorithmRepetitions);
                double result = _solution.FunctionValue(_distanceMatrix);
                viewMapButton.IsEnabled = true;
                resultView.Text = result.ToString() + " km";
                resultView.InvalidateVisual();
            } else
            {
                resultView.Text = "Uzupełnij dane";
            }

        }

        private void viewMapForSolution_Click(object sender, RoutedEventArgs e)
        {
            MapDataSource.MapViewWindow.clearRoutes();
            List<int> path = _solution.Clone();
            GeoData[] points = MapDataSource.DistanceMatrix.GeoPoints;

            for (int i = 0; i < path.Count() - 1; ++i)
            {
                GeoData point1 = points[path[i]];
                GeoData point2 = points[path[i + 1]];
                MapDataSource.MapViewWindow.drawRouteForLocation(point1.latitude, point1.longitude, point2.latitude, point2.longitude, GetPathColor(path, i, i + 1));
            }

            GeoData retPoint1 = points[path[path.Count() - 1]];
            GeoData retPoint2 = points[path[0]];
            MapDataSource.MapViewWindow.drawRouteForLocation(retPoint1.latitude, retPoint1.longitude, retPoint2.latitude, retPoint2.longitude, GetPathColor(path, path.Count() - 1, 0));

            MapDataSource.MapViewWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Normal;

            Hide();
        }
        #endregion

        #region Algorithm methods
        private SA_Solution RunMultipleTimes(double? initTemp, double? tempDecreasingCoefficient, long? maxRepetitionsWithoutImprovement, long? algorithmRepetitions)
        {
            SA_Solution bestSolution = Run(initTemp, tempDecreasingCoefficient, maxRepetitionsWithoutImprovement);
            for (int i = 0; i < algorithmRepetitions - 1; ++i)
            {
                SA_Solution solution = Run(initTemp, tempDecreasingCoefficient, maxRepetitionsWithoutImprovement);
                if (solution.FunctionValue(_distanceMatrix) < bestSolution.FunctionValue(_distanceMatrix))
                {
                    bestSolution = solution;
                }
            }
            return bestSolution;
        }

        private SA_Solution Run(double? initTemp, double? tempDecreasingCoefficient, long? maxRepetitionsWithoutImprovement)
        {
            Random random = new Random();

            SA_Solution initSolution = new SA_Solution(_distanceMatrix.GetLength(0), true, random);
            SA_Solution oldSolution = initSolution;
            SA_Solution bestSolution = new SA_Solution(initSolution.Clone());

            double? temp = initTemp;
            double oldResult = 0;
            double newResult = 0;

            int repetitionsWithoutImprovement = 0;

            while (repetitionsWithoutImprovement < maxRepetitionsWithoutImprovement)
            {
                SA_Solution newSolution = oldSolution.CreateNewSolutionWithSwappedTwoElements(random);
                oldResult = oldSolution.FunctionValue(_distanceMatrix);
                newResult = newSolution.FunctionValue(_distanceMatrix);

                if (newResult < bestSolution.FunctionValue(_distanceMatrix))
                {
                    bestSolution = newSolution;
                }

                if (newResult < oldResult)
                {
                    oldSolution = newSolution;
                    repetitionsWithoutImprovement = 0;
                }
                else if (random.NextDouble() < CalculateProbability(oldResult, newResult, temp))
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

            PrepareLogger();
            _loggerFile.WriteLine("");
            LogPermutation(bestSolution.Permutation, bestSolution.FunctionValue(_distanceMatrix));
            LogDistancesForSolution(bestSolution);
            FinalizeLogger();

            return bestSolution;
        }

        private double CalculateProbability(double oldResult, double newResult, double? temp)
        {
            double? toExp = -((newResult - oldResult) / temp);
            return Math.Exp(toExp ?? 0);
        }
        #endregion

        #region Helpers
        private Color GetPathColor(List<int> path, int ind1, int ind2)
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

        private void PrepareLogger()
        {
            this._loggerFile = new StreamWriter(@"logs.txt", true);
        }

        private void FinalizeLogger()
        {
            this._loggerFile.Close();
        }

        private void ClearLogger()
        {
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
        }

        private string GetPermutationString(List<int> permutation)
        {
            StringBuilder builder = new StringBuilder();
            permutation.ForEach(item => builder.Append($"{ item } "));

            return builder.ToString();
        }

        private void LogPermutation(List<int> permutation, double value, string annotation = "")
        {
            string annotationString = string.Empty.Equals(annotation) ? annotation : annotation + ":";
            _loggerFile.WriteLine($"{ annotationString } (val={value})");
            _loggerFile.WriteLine(GetPermutationString(permutation));
        }

        private void LogOldAndNewPermutation(List<int> oldSolutionP, List<int> newSolutionP, double oldResult, double newResult)
        {
            LogPermutation(oldSolutionP, oldResult, "OLD");
            LogPermutation(newSolutionP, newResult, "NEW");
        }

        private void LogDistancesForSolution(SA_Solution solution)
        {
            for (int i = 1; i < solution.Permutation.Count; i++)
            {
                _loggerFile.WriteLine($"{ solution.Permutation[i - 1] } -> { solution.Permutation[i] }: { _distanceMatrix[solution.Permutation[i - 1], solution.Permutation[i]] }");
            }
        }
        #endregion
    }
}
