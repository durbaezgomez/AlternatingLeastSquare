using System.Collections.Generic;

namespace zadanie3
{
    public class ALS
    {
        private Product[] products;

        public List<int> FlatNoZeroOnRow(int userIndex, int[,] array)
        {
            var listOfIndexes = new List<int>();
            for (var j = 0; j < array.GetLength(1); j++)
            {
                if (array[userIndex, j] != 0)
                    listOfIndexes.Add(j);
            }
            return listOfIndexes;
        }
        public List<int> FlatNoZeroOnColumn(int productIndex, int[,] array)
        {
            var listOfIndexes = new List<int>();
            for (var j = 0; j < array.GetLength(0); j++)
            {
                if (array[j, productIndex] != 0)
                    listOfIndexes.Add(j);
            }
            return listOfIndexes;
        }


        public double[,] TakeIndexValues(List<int> listOfIndexes, double[,] array, double dimension)
        {
            var arrayIndexValues = new double[(int)dimension, listOfIndexes.Count];
            for (int i = 0, j = 0; i < dimension; i++)
            {
                foreach (var index in listOfIndexes)
                {
                    arrayIndexValues[i, j++] = array[i, index];
                }
                j = 0;
            }
            return arrayIndexValues;
        }


        public double[,] CreateEye(double size)
        {
            var eye = new double[(int)size, (int)size];
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    eye[i, j] = (i == j) ? 1 : 0;
                }
            }
            return eye;
        }


        public double[] Count_V_u(List<int> listOfIndexes, double[,] arrayIndexValues, int[,] ratingsMatrix, int userIndex)
        {
            var dim = arrayIndexValues.GetLength(1);

            var V_u = new double[dim];

            for (var i = 0; i < dim; i++)
            {
                V_u[i] = 0;
            }

            for (int i = 0, j = 0; j < dim; j++)
            {
                foreach (var index in listOfIndexes)
                {
                    V_u[j] += arrayIndexValues[i++, j] * ratingsMatrix[userIndex, index];
                }
                i = 0;
            }
            return V_u;
        }

        public double[] Count_W_p(List<int> listOfIndexes, double[,] arrayIndexValues, int[,] ratingsMatrix, int productIndex)
        {
            var dim = arrayIndexValues.GetLength(1);
            var W_p = new double[dim];

            for (var i = 0; i < dim; i++)
            {
                W_p[i] = 0;
            }


            for (int j = 0, i = 0; j < dim; j++)
            {
                foreach (var index in listOfIndexes)
                {
                    W_p[j] += arrayIndexValues[i++, j] * ratingsMatrix[index, productIndex];
                }
                i = 0;
            }
            return W_p;
        }

        public double[,] SwitchGaussColumn(int userIndex, double[,] matrix, double[] gaussResult)
        {
            var arrayAfterSwitch = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0, index = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    arrayAfterSwitch[i, j] = (j == userIndex) ? gaussResult[index++] : matrix[i, j];
                }
            }
            return arrayAfterSwitch;
        }


    }

}
