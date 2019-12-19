using MiscUtil;
using System;

namespace zadanie3
{
    public class Matrix
    {
        public float[,] MatrixA;
        public float[,] MatrixB;
        public float[] VectorA;
        public float[] VectorB;
        public float[] VectorXGauss;
        public int[] ColumnA;
        public int[] ColumnB;

        public Matrix(float[,] matrixA, float[,] matrixB)
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

        public Matrix(float[,] matrixA, float[] vectorA)
        {
            MatrixA = matrixA;
            VectorA = vectorA;

            ColumnA = new int[MatrixA.GetLength(0)];
            for (int i = 0; i < MatrixA.GetLength(0); i++)
            {
                ColumnA[i] = i;
            }

        }

        public void CalculatePG(float[,] MatrixA, float[] VectorA)
        {
            for (int n = 1; n < MatrixA.GetLength(0); n++)
            {
                int number = n - 1;
                float max = MatrixA[ColumnA[n - 1], n - 1];
                for (int i = n - 1; i < MatrixA.GetLength(0); i++)
                {
                    float actual = Absolute(MatrixA[ColumnA[n - 1], i]);
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
                        float tempM = MatrixA[ColumnA[l], number];
                        MatrixA[ColumnA[l], number] = MatrixA[ColumnA[l], n - 1];
                        MatrixA[ColumnA[l], n - 1] = tempM;
                    }
                    float tempV = VectorA[number];
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
                float a = Operator.Divide(MatrixA[ColumnA[n - 1], y], MatrixA[ColumnA[n - 1], n - 1]);
                for (int x = n - 1; x < MatrixA.GetLength(0); x++)
                    MatrixA[ColumnA[x], y] = Operator.Subtract(MatrixA[ColumnA[x], y], Operator.Multiply(a, MatrixA[ColumnA[x], n - 1]));
                VectorA[y] = Operator.Subtract(VectorA[y], Operator.Multiply(a, VectorA[n - 1]));
            }
        }

        public void CalculateResult(int dim)
        {
            VectorXGauss = new float[ColumnA.Length];
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


        public static float[] Multiplication(float[,] matrix, float[] vector)
        {
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(0); x++)
                    vector[y] = Operator.Add(vector[y], Operator.Multiply(matrix[x, y], vector[x]));
            return vector;
        }

        public static float[,] Multiplication(float[,] matrix, float arg)
        {
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(0); x++)
                    matrix[y, x] = Operator.Multiply(matrix[y,x], arg);
            return matrix;
        }

        public static float[,] Multiplication(float[,] matrixA, float[,] matrixB)
        {
            if (matrixA.GetLength(0) != matrixB.GetLength(1))
                throw new Exception("Matrices have incorrect dimensions");

            float[,] result = new float[matrixA.GetLength(0), matrixB.GetLength(0)];

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

        public static float[,] Summing(float[,] matrixA, float[,] matrixB)
        {
            float[,] result = new float[matrixA.GetLength(0), matrixA.GetLength(1)];
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

        public float CountDifference(float[,] original, float[,] changed)
        {
            float difference = 0;
            var counter = 0;
            for (var i = 0; i < original.GetLength(0); i++)
            {
                for (var j = 0; j < original.GetLength(1); j++)
                {
                    if (original[i, j] != 0)
                    {
                        difference += Math.Abs((original[i, j] - changed[i, j]));
                        counter++;
                    }
                }
            }

            return difference / counter;
        }

        public static float[,] Transpose(float[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            var result = new float[columns, rows];

            for (var c = 0; c < columns; c++)
            {
                for (var r = 0; r < rows; r++)
                {
                    result[c, r] = matrix[r, c];
                }
            }

            return result;
        }

        public float Absolute(float obj)
        {
            float zero = Operator.Subtract(obj, obj);
            if (Operator.LessThan(obj, zero))
                obj = Operator.Subtract(obj, Operator.Add(obj, obj));
            return obj;
        }

        public void SetMatrixA(int x, int y, float value) { MatrixA[x, y] = value; }
        public void SetMatrixB(int x, int y, float value) { MatrixB[x, y] = value; }

        public void SetVectorA(int y, float value) { VectorA[y] = value; }
        public void SetVectorB(int y, float value) { VectorB[y] = value; }
    }
        
}
