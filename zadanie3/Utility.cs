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
