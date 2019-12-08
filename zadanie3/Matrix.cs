using MiscUtil;

namespace zadanie3
{
    public class Matrix<T>
    {
        public T[,] MatrixA;
        public T[] VectorB;
        public T[] VectorX;
        public T[] VectorXGauss;
        public int Dimensions;
        public int[] Column;

        public Matrix(int dimensions)
        {
            Dimensions = dimensions;
            MatrixA = new T[dimensions, dimensions];
            VectorX = new T[dimensions];
            VectorB = new T[dimensions];
            VectorXGauss = new T[dimensions];
            Column = new int[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                Column[i] = i;
            }
        }

        public void CalculatePG()
        {
            for (int n = 1; n < Dimensions; n++)
            {
                int number = n - 1;
                T max = MatrixA[Column[n - 1], n - 1];
                for (int i = n - 1; i < Dimensions; i++)
                {
                    T actual = Absolute(MatrixA[Column[n - 1], i]);
                    if (Operator.GreaterThan<T>(actual, max))
                    {
                        max = actual;
                        number = i;
                    }
                }
                if (Operator.NotEqual(number, n - 1))
                {
                    for (int l = n - 1; l < Dimensions; l++)
                    {
                        T tempA = MatrixA[Column[l], number];
                        MatrixA[Column[l], number] = MatrixA[Column[l], n - 1];
                        MatrixA[Column[l], n - 1] = tempA;
                    }
                    T tempB = VectorB[number];
                    VectorB[number] = VectorB[n - 1];
                    VectorB[n - 1] = tempB;
                }
                CalculateStep(n);
            }
            CalculateResult();
        }

        public void CalculateStep(int n)
        {
            for (int y = n; y < Dimensions; y++)
            {
                T a = Operator.Divide(MatrixA[Column[n - 1], y], MatrixA[Column[n - 1], n - 1]);
                for (int x = n - 1; x < Dimensions; x++)
                    MatrixA[Column[x], y] = Operator.Subtract(MatrixA[Column[x], y], Operator.Multiply(a, MatrixA[Column[x], n - 1]));
                VectorB[y] = Operator.Subtract(VectorB[y], Operator.Multiply(a, VectorB[n - 1]));
            }
        }

        public void CalculateResult()
        {
            for (int y = Dimensions - 1; y >= 0; y--)
            {
                VectorXGauss[Column[y]] = Operator.Divide(VectorB[y], MatrixA[Column[y], y]);
                for (int x = Dimensions - 1; x > y; x--)
                {
                    MatrixA[Column[x], y] = Operator.Divide(MatrixA[Column[x], y], MatrixA[Column[y], y]);
                    VectorXGauss[Column[y]] = Operator.Subtract(VectorXGauss[Column[y]], Operator.Multiply(MatrixA[Column[x], y], VectorXGauss[Column[x]]));
                }
            }
            for (int i = 0; i < Dimensions; i++)
                VectorB[i] = VectorXGauss[i];
        }

        public T Absolute(T obj)
        {
            T zero = Operator.Subtract(obj, obj);
            if (Operator.LessThan(obj, zero))
                obj = Operator.Subtract(obj, Operator.Add(obj, obj));
            return obj;
        }

        public T CalculateDiff()
        {
            T sum = Operator.Subtract(VectorX[0], VectorX[0]);
            for (int y = 0; y < Dimensions; y++)
            {
                T diff = Absolute(Operator.Subtract(VectorX[y], VectorB[y]));
                sum = Operator.Add(sum, diff);
            }
            return sum;
        }

        public void Multiplication()
        {
            for (int y = 0; y < Dimensions; y++)
                for (int x = 0; x < Dimensions; x++)
                    VectorB[y] = Operator.Add(VectorB[y], Operator.Multiply(MatrixA[x, y], VectorX[x]));
        }

        public void SetMatrixA(int x, int y, T value) { MatrixA[x, y] = value; }
        public void SetVectorB(int y, T value) { VectorB[y] = value; }
        public void SetVectorX(int y, T value) { VectorX[y] = value; }
    }
}
