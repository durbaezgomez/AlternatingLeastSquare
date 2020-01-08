using System;
using System.Collections.Generic;

namespace zadanie3
{
    public static class Utility
    {
        public static void PrintMatrix(double[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + "   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("End of Print");
        }

        public static void PrintTime(double[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + "   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("End of Print");
        }

        public static void PrintData(double[,,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    for (var k = 0; k < array.GetLength(2); k++)
                    {
                        Console.Write(array[i, j, k] + "   ");
                    }
                    Console.WriteLine("End of different same and d");
                }
                Console.WriteLine("End of i");
            }
            Console.WriteLine("End of Print");
        }

        public static void PrintMatrixComparison(int[,] arrayOrg, double[,] arrayMod)
        {
            for (var i = 0; i < arrayOrg.GetLength(0); i++)
            {
                for (var j = 0; j < arrayOrg.GetLength(1); j++)
                {
                    Console.Write(arrayOrg[i, j] + " | " + String.Format("{0:.00}", arrayMod[i, j]) + "   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("End of Print");
        }

        public static void PrintComparisonOnZeros(int[,] arrayTest, int[,] arrayLearning, double[,] arrayMod, int[,] pairs)
        {
            for (var i = 0; i < pairs.GetLength(0); i++)
            {
                var j = pairs[i, 0];
                var k = pairs[i, 1];
                Console.WriteLine("Pair : " + j + " , " + k);
                Console.WriteLine(arrayTest[j, k] + " | " + arrayLearning[j, k] + " | " + arrayMod[j, k] + "    ");
            }
        }

        public static (double, double) CountError(int[,] arrayTest, double[,] arrayMod, int[,] pairs)
        {
            double squareError = 0;
            double summingError = 0;
            for (var i = 0; i < pairs.GetLength(0); i++)
            {
                var j = pairs[i, 0];
                var k = pairs[i, 1];
                summingError += Math.Abs(arrayMod[j, k] - arrayTest[j, k]);
                squareError += Math.Pow(arrayMod[j, k] - arrayTest[j, k], 2);
            }
            return (summingError, squareError);
        }



        public static void PrintFlatArray(double[] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                Console.Write(array[i] + "   ");
            }
            Console.WriteLine("End of Print");
            Console.WriteLine();
        }

        public static void PrintFlatList(List<double> list)
        {
            var newList = list.ToArray();
            for (var i = 0; i < newList.GetLength(0); i++)
            {
                Console.Write(newList[i] + "   ");
            }
            Console.WriteLine("End of Print");
            Console.WriteLine();
        }
    }
}
