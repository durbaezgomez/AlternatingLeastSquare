using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using MiscUtil;

namespace zadanie3
{
    public class Matrix<T>
    {
        //TODO: Przeorganizuj klasę tak, żeby konstruktor przyjmował odpowiednie arraye dla obliczeń dla POTRZEBNYCH konfiguracji - macierz-macierz i macierz-wektor. NIE KOMBINUJ!
        public T[,] MatrixA;
        public T[,] MatrixB;
        public T[] VectorA;
        public T[] VectorB;
        public T[] VectorXGauss;
        public int Dimensions;
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
            CalculateResult();
        }

        public void CalculateStep(int n)
        {
            for (int y = n; y < MatrixA.GetLength(0); y++)
            {
                T a = Operator.Divide(MatrixA[ColumnA[n - 1], y], MatrixA[ColumnA[n - 1], n - 1]);
                for (int x = n - 1; x < Dimensions; x++)
                    MatrixA[ColumnA[x], y] = Operator.Subtract(MatrixA[ColumnA[x], y], Operator.Multiply(a, MatrixA[ColumnA[x], n - 1]));
                VectorA[y] = Operator.Subtract(VectorA[y], Operator.Multiply(a, VectorA[n - 1]));
            }
        }

        public void CalculateResult()
        {
            for (int y = Dimensions - 1; y >= 0; y--)
            {
                VectorXGauss[ColumnA[y]] = Operator.Divide(VectorA[y], MatrixA[ColumnA[y], y]);
                for (int x = Dimensions - 1; x > y; x--)
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


        public T[] Multiplication(T[,] matrix, T[] vector)
        {
            for (var y = 0; y < Dimensions; y++)
                for (var x = 0; x < Dimensions; x++)
                    vector[y] = Operator.Add(vector[y], Operator.Multiply(matrix[x, y], vector[x]));
            return vector;
        }

        public T[,] Multiplication(T[,] matrixA, T[,] matrixB)
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

        public T[,] Summing(T[,] matrixA, T[,] matrixB)
        {
            T[,] result = new T[matrixA.GetLength(0), matrixA.GetLength(1)];
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

        public T[] GetColumnFromMatrix(int index, T[,] matrix)
        {
            T[] column = new T[matrix.GetLength(0)];
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                column[i] = matrix[i, index];
            }

            return column;
        }



        public void SetMatrixA(int x, int y, T value) { MatrixA[x, y] = value; }
        public void SetMatrixB(int x, int y, T value) { MatrixB[x, y] = value; }

        public void SetVectorA(int y, T value) { VectorA[y] = value; }
        public void SetVectorB(int y, T value) { VectorB[y] = value; }
    }
}
