using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler.Models
{
    class SA_Solution
    {
        private List<int> _pathForMatrix;

        public SA_Solution(List<int> pathForMatrix)
        {
            if (pathForMatrix.Count() < 2)
            {
                throw new InvalidOperationException("Can't create path with less than two points.");
            } else
            {
                _pathForMatrix = pathForMatrix;
            }
        }

        public SA_Solution(int distanceMatrixLength, Boolean generateRandomPath, Random random)
        {
            if (distanceMatrixLength < 2)
            {
                throw new InvalidOperationException("Can't create path with less than two points.");
            } else
            {
                _pathForMatrix = new List<int>();

                for (int i = 0; i < distanceMatrixLength; ++i)
                {
                    _pathForMatrix.Add(i);
                }

                if (generateRandomPath)
                {
                    Shuffle(random);
                }
            } 
        }

        public List<int> getCopyOfPath()
        {
            return new List<int>(_pathForMatrix);
        }
        public double objectiveFunction(double[,] distanceMatrix)
        {
            if (_pathForMatrix.Count() != distanceMatrix.GetLength(0))
            {
                throw new InvalidOperationException("Distance matrix length doesn't match path length.");
            } else
            {
                double result = 0;
                for (int i = 0; i < (_pathForMatrix.Count() - 1); ++i)
                {
                    result += distanceMatrix[_pathForMatrix[i], _pathForMatrix[i + 1]];
                }
                result += distanceMatrix[_pathForMatrix[_pathForMatrix.Count() - 1], _pathForMatrix[0]];
                return result;
            }
        }

        private void Shuffle(Random random)
        {
            int n = _pathForMatrix.Count();
            while (n > 1)
            {
                n--;
                int i = random.Next(n + 1);
                int temp = _pathForMatrix[i];
                _pathForMatrix[i] = _pathForMatrix[n];
                _pathForMatrix[n] = temp;
            }
        }

        public SA_Solution createNewSolutionWithSwappedTwoElements(Random random)
        {
            int el1 = random.Next(_pathForMatrix.Count());
            int el2;
            do
            {
                el2 = random.Next(_pathForMatrix.Count());
            } while (el1 == el2);
            List<int> newPath = new List<int>(_pathForMatrix);
            int tmp = newPath[el1];
            newPath[el1] = newPath[el2];
            newPath[el2] = tmp;

            return new SA_Solution(newPath);
        }
    }
}
