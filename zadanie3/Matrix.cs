using MiscUtil;
using System;

namespace zadanie3
{
    public class Matrix
    {
        public double[,] MatrixA;
        public double[,] MatrixB;
        public double[] VectorA;
        public double[] VectorB;
        public double[] VectorXGauss;
        public int[] ColumnA;
        public int[] ColumnB;

        public Matrix(double[,] matrixA, double[,] matrixB)
        {
            MatrixA = matrixA;
            MatrixB = matrixB;

            ColumnA = new int[MatrixA.GetLength(0)];
            for (int i = 0; i < MatrixA.GetLength(0); i++)
            {
                ColumnA[i] = i;
            }

            ColumnB = new int[MatrixB.GetLength(0)];
            for (int i = 0; i < MatrixB.GetLength(0); i++)
            {
                ColumnB[i] = i;
            }
        }

        public Matrix(double[,] matrixA, double[] vectorA)
        {
            MatrixA = matrixA;
            VectorA = vectorA;

            ColumnA = new int[MatrixA.GetLength(0)];
            for (int i = 0; i < MatrixA.GetLength(0); i++)
            {
                ColumnA[i] = i;
            }

        }

        public void CalculatePG(double[,] MatrixA, double[] VectorA)
        {
            for (int n = 1; n < MatrixA.GetLength(0); n++)
            {
                int number = n - 1;
                double max = MatrixA[ColumnA[n - 1], n - 1];
                for (int i = n - 1; i < MatrixA.GetLength(0); i++)
                {
                    double actual = Absolute(MatrixA[ColumnA[n - 1], i]);
                    if (Operator.GreaterThan(actual, max))
                    {
                        max = actual;
                        number = i;
                    }
                }

                if (Operator.NotEqual(number, n - 1))
                {
                    for (int l = n - 1; l < MatrixA.GetLength(0); l++)
                    {
                        double tempM = MatrixA[ColumnA[l], number];
                        MatrixA[ColumnA[l], number] = MatrixA[ColumnA[l], n - 1];
                        MatrixA[ColumnA[l], n - 1] = tempM;
                    }
                    double tempV = VectorA[number];
                    VectorA[number] = VectorA[n - 1];
                    VectorA[n - 1] = tempV;
                }

                CalculateStep(n);
            }

            CalculateResult(MatrixA.GetLength(0));
        }

        public void CalculateStep(int n)
        {
            for (int y = n; y < MatrixA.GetLength(0); y++)
            {
                double a = Operator.Divide(MatrixA[ColumnA[n - 1], y], MatrixA[ColumnA[n - 1], n - 1]);
                for (int x = n - 1; x < MatrixA.GetLength(0); x++)
                    MatrixA[ColumnA[x], y] = Operator.Subtract(MatrixA[ColumnA[x], y], Operator.Multiply(a, MatrixA[ColumnA[x], n - 1]));
                VectorA[y] = Operator.Subtract(VectorA[y], Operator.Multiply(a, VectorA[n - 1]));
            }
        }

        public void CalculateResult(int dim)
        {
            VectorXGauss = new double[ColumnA.Length];
            for (int y = dim - 1; y >= 0; y--)
            {
                VectorXGauss[ColumnA[y]] = Operator.Divide(VectorA[y], MatrixA[ColumnA[y], y]);
                for (int x = dim - 1; x > y; x--)
                {
                    MatrixA[ColumnA[x], y] = Operator.Divide(MatrixA[ColumnA[x], y], MatrixA[ColumnA[y], y]);
                    VectorXGauss[ColumnA[y]] = Operator.Subtract(VectorXGauss[ColumnA[y]], Operator.Multiply(MatrixA[ColumnA[x], y], VectorXGauss[ColumnA[x]]));
                }
            }
        }

        public static double[] Multiplication(double[,] matrix, double[] vector)
        {
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(0); x++)
                    vector[y] = Operator.Add(vector[y], Operator.Multiply(matrix[x, y], vector[x]));
            return vector;
        }

        public static double[,] Multiplication(double[,] matrix, double arg)
        {
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(0); x++)
                    matrix[y, x] = Operator.Multiply(matrix[y, x], arg);
            return matrix;
        }

        public static double[,] Multiplication(double[,] matrixA, double[,] matrixB)
        {
            if (matrixA.GetLength(1) != matrixB.GetLength(0))
                throw new Exception("Matrices have incorrect dimensions");

            double[,] result = new double[matrixA.GetLength(0), matrixB.GetLength(1)];

            for (int m = 0; m < result.GetLength(0); m++)
            {
                for (int n = 0; n < result.GetLength(1); n++)
                {
                    for (int j = 0; j < matrixA.GetLength(1); j++)
                    {
                        result[m, n] += matrixA[m, j] * matrixB[j, n];
                    }
                }
            }

            return result;
        }

        public static double[,] Summing(double[,] matrixA, double[,] matrixB)
        {
            double[,] result = new double[matrixA.GetLength(0), matrixA.GetLength(1)];
            if (matrixA.GetLength(0) != matrixB.GetLength(0) || matrixA.GetLength(1) != matrixB.GetLength(1))
            {
                throw new Exception("Matrices have incorrect dimensions");
            }

            for (var i = 0; i < matrixA.GetLength(0); i++)
            {
                for (var j = 0; j < matrixA.GetLength(1); j++)
                {
                    result[i, j] = Operator.Add(matrixA[i, j], matrixB[i, j]);
                }

            }
            return result;

        }

        public double CountDifference(double[,] original, double[,] changed)
        {
            double difference = 0;
            var counter = 0;
            for (var i = 0; i < original.GetLength(0); i++)
            {
                for (var j = 0; j < original.GetLength(1); j++)
                {
                    if (original[i, j] == 0) continue;

                    difference += Math.Abs((original[i, j] - changed[i, j]));
                    counter++;
                }
            }

            return difference / counter;
        }

        public static double[,] Transpose(double[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            var result = new double[columns, rows];

            for (var c = 0; c < columns; c++)
            {
                for (var r = 0; r < rows; r++)
                {
                    result[c, r] = matrix[r, c];
                }
            }

            return result;
        }

        public double Absolute(double obj)
        {
            var zero = Operator.Subtract(obj, obj);
            if (Operator.LessThan(obj, zero))
                obj = Operator.Subtract(obj, Operator.Add(obj, obj));
            return obj;
        }

        public void SetMatrixA(int x, int y, double value) { MatrixA[x, y] = value; }
        public void SetMatrixB(int x, int y, double value) { MatrixB[x, y] = value; }

        public void SetVectorA(int y, double value) { VectorA[y] = value; }
        public void SetVectorB(int y, double value) { VectorB[y] = value; }
    }

}
