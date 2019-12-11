using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace zadanie3
{
    public class ALS
    {
        private Product[] products;

        public ALS()
        {
        }

        private List<int> FlatNoZero(int u, float[,] array) //  In [38]:    arg: ilosc uzytkownikow, array z ocenami. Zwraca array z indeksami != 0 dla wiersza = u. 
        {
            var listOfIndexes = new List<int>();
            // ma przeszukac wiersz u i znalezc            
            for (int j = 0; j < array.GetLength(u); j++) // skopiowany z Utility, nie rozumiem tego: j < array.GetLength(1)
            {
                if (array[u, j] != 0)
                    listOfIndexes.Add(j);
            }
            return listOfIndexes;
        }// sprawdza ktore indeksy sa rozne od 0

        private List<float> TakeIndexValues(List<int> listOfIndexes, float[,] array)
        {  //In [39]
            // czyli kolumny z macierzy P o indeksach w _I_u_)
            var arrayIndexValues = new List<float>();
            // np listOfIndexes to [4,6,7] to wynikiem ma byc dwuwymiarowa tablica wartosci o indexach [ [ [0,4],[0,6],[0,7] ] , [ [1,4], [1,6], [1,7] ] , ... , [ [n,4], [n,6], [n,7] ] ],
            //a wartosci pod tymi indeksami wziete z array, a array to lista random_product
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j] + "   ");
                }

                Console.WriteLine();
            }

            return arrayIndexValues;

        }

        private int[,] createEye(int size) // stworz macierz jednostowa o zadanym rozmiarze
        {
            var Eye = new int[,] { };
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; i++)
                {
                    //(i == j) ? Eye[i, j] = 1 : Eye[i, j] = 0; // cos nie dziala
                }
            }
            return Eye;
            // return macierz_jednostkowa
        }

        //A_u = np.matmul(P_I_u, P_I_u_T) + reg* E 
        // do tego w sumie potrzebna metoda transponowania, a reszte w sumie mamy. mozna to eventualnie zamknac w metode dokonujaca tego mnozenia kilku elementow.


        /*Następnie liczymy _V_u_, czyli musimy dodać do siebie:

        kolumnę 4 z macierzy P pomnożoną przez 4 (ocena)
        kolumnę 6 z macierzy P pomnożoną przez 5 (ocena)
        kolumnę 7 z macierzy P pomnożoną przez 4 (ocena)
        */

        private float[,] count_V_u_(int[,] arrayOfIndexes, float[,] MatrixP)
        {
            var V_u = new float[,] { };
            for (int i = 0; i < arrayOfIndexes.GetLength(0); i++)
            {

            }
            return V_u;

            //czyli chcemy uzyskac w tym przypadku macierz 1x3 np [ 1, 2, 3]
        }

        //To teraz mnożymy rating przez P_i, dodajemy i wychodzi nam:

        //In[44]:
        //V_u = np.zeros(d).T
        //for i in I_u:
        //    V_u += ratings_matrix[u][i] * P[:, i] 

        //V_u




    }

}
