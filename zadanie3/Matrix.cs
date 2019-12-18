using MiscUtil;
using System;
using System.Collections.Generic;

namespace zadanie3
{
    public class Matrix<T>
    {
        public T[,] MatrixA;
        public T[,] MatrixB;
        public T[] VectorA;
        public T[] VectorB;
        public T[] VectorXGauss;
        public int[] ColumnA;
        public int[] ColumnB;

        public Matrix(T[,] matrixA, T[,] matrixB)
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

        public Matrix(T[,] matrixA, T[] vectorA)
        {
            MatrixA = matrixA;
            VectorA = vectorA;

            ColumnA = new int[MatrixA.GetLength(0)];
            for (int i = 0; i < MatrixA.GetLength(0); i++)
            {
                ColumnA[i] = i;
            }

        }

        public void CalculatePG(T[,] MatrixA, T[] VectorA)
        {

            for (int n = 1; n < MatrixA.GetLength(0); n++)
            {
                int number = n - 1;
                T max = MatrixA[ColumnA[n - 1], n - 1];
                for (int i = n - 1; i < MatrixA.GetLength(0); i++)
                {
                    T actual = Absolute(MatrixA[ColumnA[n - 1], i]);
                    if (Operator.GreaterThan<T>(actual, max))
                    {
                        max = actual;
                        number = i;
                    }
                }
                if (Operator.NotEqual(number, n - 1))
                {
                    for (int l = n - 1; l < MatrixA.GetLength(0); l++)
                    {
                        T tempM = MatrixA[ColumnA[l], number];
                        MatrixA[ColumnA[l], number] = MatrixA[ColumnA[l], n - 1];
                        MatrixA[ColumnA[l], n - 1] = tempM;
                    }
                    T tempV = VectorA[number];
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
                T a = Operator.Divide(MatrixA[ColumnA[n - 1], y], MatrixA[ColumnA[n - 1], n - 1]);
                for (int x = n - 1; x < MatrixA.GetLength(0); x++)
                    MatrixA[ColumnA[x], y] = Operator.Subtract(MatrixA[ColumnA[x], y], Operator.Multiply(a, MatrixA[ColumnA[x], n - 1]));
                VectorA[y] = Operator.Subtract(VectorA[y], Operator.Multiply(a, VectorA[n - 1]));
            }
        }

        public void CalculateResult(int dim)
        {
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

        public T Absolute(T obj)
        {
            T zero = Operator.Subtract(obj, obj);
            if (Operator.LessThan(obj, zero))
                obj = Operator.Subtract(obj, Operator.Add(obj, obj));
            return obj;
        }


        public static T[] Multiplication(T[,] matrix, T[] vector)
        {
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(0); x++)
                    vector[y] = Operator.Add(vector[y], Operator.Multiply(matrix[x, y], vector[x]));
            return vector;
        }

        public static T[,] Multiplication(T[,] matrixA, T[,] matrixB)
        {
            if (matrixA.GetLength(1) != matrixB.GetLength(10))
                throw new Exception("Matrices have incorrect dimensions");

            T[,] result = new T[matrixA.GetLength(0), matrixB.GetLength(1)];
            List<T[]> columns = new List<T[]>();
            for (var i = 0; i < result.GetLength(1); i++)
            {
                var vectorB = GetColumnFromMatrix(i, matrixB);
                var resultColumn = Multiplication(matrixA, vectorB);
                columns.Add(resultColumn);
            }

            var columnCounter = 0;
            foreach (var column in columns)
            {
                for (var i = 0; i < column.Length; i++)
                {
                    result[i, columnCounter] = column[i];
                }
            }

            return result;
        }

        public static T[] GetColumnFromMatrix(int index, T[,] matrix)
        {
            T[] column = new T[matrix.GetLength(0)];
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                column[i] = matrix[i, index];
            }

            return column;
        }

        public static T[,] Transpose(T[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            var result = new T[columns, rows];

            for (var c = 0; c < columns; c++)
            {
                for (var r = 0; r < rows; r++)
                {
                    result[c, r] = matrix[r, c];
                }
            }

            return result;
        }

        public void SetMatrixA(int x, int y, T value) { MatrixA[x, y] = value; }
        public void SetMatrixB(int x, int y, T value) { MatrixB[x, y] = value; }

        public void SetVectorA(int y, T value) { VectorA[y] = value; }
        public void SetVectorB(int y, T value) { VectorB[y] = value; }
    }
}
