using System;
namespace zadanie3
{
    public static class Utility<T>
    {
        public static void PrintMatrix(T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + "   ");
                }

                Console.WriteLine();
            }
        }
    }
}
