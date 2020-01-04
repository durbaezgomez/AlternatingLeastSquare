using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            Console.WriteLine("End of Print");
        }

        public static void PrintTime(T[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + "   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("End of Print");
        }

        public static void PrintData(T[,,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        Console.Write(array[i, j, k] + "   ");
                    }
                    Console.WriteLine("End of different same and d");
                }
                Console.WriteLine("End of i");
            }
            Console.WriteLine("End of Print");
        }

        public static void PrintMatrixComparison(int[,] array_org, float[,] array_mod)
        {
            for (int i = 0; i < array_org.GetLength(0); i++)
            {
                for (int j = 0; j < array_org.GetLength(1); j++)
                {
                    Console.Write(array_org[i, j] + " | " + String.Format("{0:.00}", array_mod[i, j]) + "   " );
                }
                Console.WriteLine();
            }
            Console.WriteLine("End of Print");
        }

        public static void PrintComparisonOnZeros( int[,] array_test, int [,] array_learning, float [,] array_mod, int [,] pairs)
        {
            for(int i = 0; i<pairs.GetLength(0); i++)
            {
                var j = pairs[i, 0];
                var k = pairs[i, 1];
                Console.WriteLine("Pair : " + j + " , " + k);
                Console.WriteLine(array_test[j, k] + " | " + array_learning[j, k] + " | " + array_mod[j, k] + "    ");
            }
        }

        public static float CountError(int[,] array_test, float[,] array_mod, int[,] pairs)
        {
            float summing_error = 0;
            for (int i = 0; i < pairs.GetLength(0); i++)
            {
                var j = pairs[i, 0];
                var k = pairs[i, 1];
                summing_error += Math.Abs(array_mod[j, k] - array_test[j, k] );
            }
            return summing_error;
        }

        public static void PrintFlatArray(T[] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {             
                Console.Write(array[i] + "   ");
            }
            Console.WriteLine("End of Print");
            Console.WriteLine();
        }

        public static void PrintFlatList(List<T> list)
        {
            T[] new_list = list.ToArray();
            for (int i = 0; i < new_list.GetLength(0); i++)
            {
                Console.Write(new_list[i] + "   ");
            }
            Console.WriteLine("End of Print");
            Console.WriteLine();
        }
    }
}
