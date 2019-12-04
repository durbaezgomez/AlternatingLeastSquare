using System;
namespace zadanie3
{
    public class Matrix<T> where T : new()
    {
        public T[,] MatrixContainer { get; }

        public Matrix(T[,] matrix)
        {
            MatrixContainer = matrix;
        }

        public int Rows => MatrixContainer.GetLength(0);

        public int Cols => MatrixContainer.GetLength(1);

        public T this[int row, int col]
        {
            get => MatrixContainer[row, col];
            set => MatrixContainer[row, col] = value;
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            dynamic dynamicA = (dynamic)a;
            dynamic dynamicB = (dynamic)b;
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("Matrix sizes are not equal.");

            var output = new T[a.Rows, a.Cols];
            for (var i = 0; i < a.Rows; i++)
            {
                for (var j = 0; j < a.Cols; j++)
                {
                    output[i, j] = dynamicA[i, j] + dynamicB[i, j];
                }
            }

            return new Matrix<T>(output);
        }

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            dynamic dynamicA = (dynamic)a;
            dynamic dynamicB = (dynamic)b;
            if (a.Cols != b.Rows)
                throw new ArgumentException("Matrix sizes are not equal.");

            var output = new T[a.Rows, b.Cols];
            for (var row = 0; row < a.Rows; row++)
            {
                for (var col = 0; col < b.Cols; col++)
                {
                    var sum = new T();
                    for (var k = 0; k < a.Cols; k++)
                    {
                        sum += dynamicA[row, k] * dynamicB[k, col];
                    }

                    output[row, col] = sum;
                }
            }

            return new Matrix<T>(output);
        }

        public static T[] operator *(Matrix<T> a, T[] b)
        {
            dynamic dynamicA = (dynamic)a;
            dynamic dynamicB = (dynamic)b;
            if (a.Cols != b.Length)
                throw new ArgumentException("Matrix sizes are not equal.");

            var output = new T[a.Rows];
            for (var row = 0; row < a.Rows; row++)
                output[row] = new T();

            for (var row = 0; row < a.Rows; row++)
            {
                for (var col = 0; col < b.Length; col++)
                {
                    output[row] += dynamicA[row, col] * dynamicB[col];
                }
            }

            return output;
        }

        public void SwapRow(int index1, int index2)
        {
            for (var i = 0; i < Cols; i++)
            {
                var temp = this[index2, i];
                this[index2, i] = this[index1, i];
                this[index1, i] = temp;
            }
        }

        public void SwapColumn(int index1, int index2)
        {
            for (var i = 0; i < Cols; i++)
            {
                var temp = this[i, index2];
                this[i, index2] = this[i, index1];
                this[i, index1] = temp;
            }
        }

        public int FindMaxInColumn(int selected)
        {
            var currentMaxRowIndex = selected;
            var currentMax = this[selected, selected];

            for (var i = selected; i < Rows; i++)
            {
                if (this[i, selected] > (dynamic)currentMax)
                {
                    currentMax = this[i, selected];
                    currentMaxRowIndex = i;
                }
            }

            return currentMaxRowIndex;
        }

        public Tuple<int, int> FindMax(int selected)
        {
            var currentMaxIndex = new Tuple<int, int>(selected, selected);
            var currentMax = this[selected, selected];

            for (var i = selected; i < Rows; i++)
            {
                for (var j = selected; j < Cols; j++)
                {
                    if (this[i, j] > (dynamic)currentMax || this[i, j] < -(dynamic)currentMax)
                    {
                        currentMax = this[i, j];
                        currentMaxIndex = new Tuple<int, int>(i, j);
                    }
                }
            }

            return currentMaxIndex;
        }

        public void GaussianReductionNoPivot(T[] vector)
        {
            ReduceLeftBottomTriangle(vector);
            CountResultBackwards(vector);
        }

        public void GaussianReductionPartialPivot(T[] vector)
        {
            ReduceLeftBottomTrianglePartialPivot(vector);
            CountResultBackwards(vector);
        }

        public void GaussianReductionFullPivot(T[] vector)
        {
            var columnOrder = new int[Cols];
            for (var i = 0; i < Cols; i++)
                columnOrder[i] = i;

            ReduceLeftBottomTriangleFullPivot(vector, columnOrder);
            CountResultBackwards(vector);

            var orderedVector = new T[Cols];
            for (var i = 0; i < Cols; i++)
                orderedVector[columnOrder[i]] = vector[i];

            for (var i = 0; i < Cols; i++)
                vector[i] = orderedVector[i];
        }

        public void ReduceLeftBottomTriangle(T[] vector)
        {
            for (var selected = 0; selected < Rows - 1; selected++)
            {
                EnsureNoLeadingZero(selected);

                for (var current = selected + 1; current < Rows; current++)
                {
                    ReduceRow(vector, selected, current);
                }
            }
        }

        public void ReduceLeftBottomTrianglePartialPivot(T[] vector)
        {
            for (var selected = 0; selected < Rows - 1; selected++)
            {
                EnsureNoLeadingZero(selected);
                ChoosePartialPivot(vector, selected);

                for (var current = selected + 1; current < Rows; current++)
                {
                    ReduceRow(vector, selected, current);
                }
            }
        }

        public void ChoosePartialPivot(T[] vector, int selected)
        {
            var maxRow = FindMaxInColumn(selected);

            if (selected != maxRow)
            {
                var temp = vector[selected];
                vector[selected] = vector[maxRow];
                vector[maxRow] = temp;

                SwapRow(selected, maxRow);
            }
        }

        public void ReduceLeftBottomTriangleFullPivot(T[] vector, int[] columnOrder)
        {
            for (var selected = 0; selected < Rows - 1; selected++)
            {
                var max = FindMax(selected);

                var tempOrd = columnOrder[selected];
                columnOrder[selected] = columnOrder[max.Item2];
                columnOrder[max.Item2] = tempOrd;
                SwapColumn(selected, max.Item2);

                var temp = vector[selected];
                vector[selected] = vector[max.Item1];
                vector[max.Item1] = temp;
                SwapRow(selected, max.Item1);

                for (var current = selected + 1; current < Rows; current++)
                {
                    ReduceRow(vector, selected, current);
                }
            }
        }

        private void EnsureNoLeadingZero(int selected)
        {
            if (this[selected, selected] == (dynamic)new T())
                throw new ArgumentException("Matrix diagonal contains zero! (leading zero detected)");
        }

        private void ReduceRow(T[] vector, int selected, int current)
        {
            if (this[current, selected] == (dynamic)new T())
                return;

            var scalar = this[current, selected] / (dynamic)this[selected, selected];

            for (var col = 0; col < Cols; col++)
            {
                this[current, col] -= this[selected, col] * scalar;
            }

            vector[current] -= vector[selected] * scalar;
        }

        public void CountResultBackwards(T[] v)
        {
            for (var i = v.Length - 1; i >= 0; i--)
            {
                dynamic sum = new T();

                for (int j = i + 1; j < v.Length; j++)
                {
                    sum += (dynamic)MatrixContainer[i, j] * v[j];
                }
                v[i] = (v[i] - sum) / MatrixContainer[i, i];
            }
        }

        public T[] MultiplyByVector(T[] v)
        {
            var result = new T[v.Length];
            for (int i = 0; i < Rows; i++)
            {
                dynamic sum = new T();
                for (int j = 0; j < Cols; j++)
                {
                    sum += (dynamic)this[i, j] * v[j];
                }
                result[i] = sum;
            }

            return result;
        }
    }
}