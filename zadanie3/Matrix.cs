using MiscUtil;

namespace zadanie3
{
    public class Matrix<T>
    {
        //TODO: Przeorganizuj klasę tak, żeby konstruktor przyjmował odpowiednie arraye dla obliczeń dla POTRZEBNYCH konfiguracji - macierz-macierz i macierz-wektor. NIE KOMBINUJ!
        public T[,] MatrixA;
        public T[,] MatrixB;
        public T[] VectorB;
        public T[] VectorXGauss;
        public int Dimensions;
        public int[] Column;

        public Matrix(T[,] matrixA, T[,] matrixB)
        {

            Dimensions = dimensions;
            VectorXGauss = new T[dimensions];
            Column = new int[];
            for (int i = 0; i < dimensions; i++)
            {
                Column[i] = i;
            }
        }

        public void CalculatePG(T[,] matrix, T[] vector)
        {


            for (int n = 1; n < matrix.GetLength(0); n++)
            {
                int number = n - 1;
                T max = matrix[Column[n - 1], n - 1];
                for (int i = n - 1; i < matrix.GetLength(0); i++)
                {
                    T actual = Absolute(matrix[Column[n - 1], i]);
                    if (Operator.GreaterThan<T>(actual, max))
                    {
                        max = actual;
                        number = i;
                    }
                }
                if (Operator.NotEqual(number, n - 1))
                {
                    for (int l = n - 1; l < matrix.GetLength(0); l++)
                    {
                        T tempM = matrix[Column[l], number];
                        matrix[Column[l], number] = matrix[Column[l], n - 1];
                        matrix[Column[l], n - 1] = tempM;
                    }
                    T tempV = vector[number];
                    vector[number] = vector[n - 1];
                    vector[n - 1] = tempV;
                }
                CalculateStep(n, matrix, vector);
            }
            CalculateResult(matrix, vector);
        }

        public void CalculateStep(int n, T[,] matrix, T[] vector)
        {
            for (int y = n; y < matrix.GetLength(0); y++)
            {
                T a = Operator.Divide(matrix[Column[n - 1], y], matrix[Column[n - 1], n - 1]);
                for (int x = n - 1; x < Dimensions; x++)
                    matrix[Column[x], y] = Operator.Subtract(matrix[Column[x], y], Operator.Multiply(a, matrix[Column[x], n - 1]));
                vector[y] = Operator.Subtract(vector[y], Operator.Multiply(a, vector[n - 1]));
            }
        }

        public void CalculateResult(T[,] matrix, T[] vector)
        {
            for (int y = Dimensions - 1; y >= 0; y--)
            {
                VectorXGauss[Column[y]] = Operator.Divide(vector[y], matrix[Column[y], y]);
                for (int x = Dimensions - 1; x > y; x--)
                {
                    matrix[Column[x], y] = Operator.Divide(matrix[Column[x], y], matrix[Column[y], y]);
                    VectorXGauss[Column[y]] = Operator.Subtract(VectorXGauss[Column[y]], Operator.Multiply(matrix[Column[x], y], VectorXGauss[Column[x]]));
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
            for (int y = 0; y < Dimensions; y++)
                for (int x = 0; x < Dimensions; x++)
                    vector[y] = Operator.Add(vector[y], Operator.Multiply(matrix[x, y], vector[x]));
            return vector;
        }

        public void SetMatrixA(int x, int y, T value) { MatrixA[x, y] = value; }
        public void SetVectorB(int y, T value) { VectorB[y] = value; }
        public void SetVectorX(int y, T value) { VectorX[y] = value; }
    }
}
