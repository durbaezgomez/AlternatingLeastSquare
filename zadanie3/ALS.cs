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

        public List<int> FlatNoZeroOnRow(int u, int[,] array) //  In [38]:    arg: ilosc uzytkownikow, array z ocenami. Zwraca array z indeksami != 0 dla wiersza = u. 
        {
            var listOfIndexes = new List<int>();
            // ma przeszukac wiersz u i znalezc            
            for (int j = 0; j < array.GetLength(1); j++) // skopiowany z Utility, nie rozumiem tego: j < array.GetLength(1)
            {
                if (array[u, j] != 0)
                    listOfIndexes.Add(j);
            }
            return listOfIndexes;
        }// sprawdza ktore indeksy sa rozne od 0

        public List<int> FlatNoZeroOnColumn(int p, int[,] array) //  In [38]:    arg: ilosc uzytkownikow, array z ocenami. Zwraca array z indeksami != 0 dla wiersza = u. 
        {
            var listOfIndexes = new List<int>();
            // ma przeszukac wiersz u i znalezc            
            for (int j = 0; j < array.GetLength(0); j++) // skopiowany z Utility, nie rozumiem tego: j < array.GetLength(1)
            {
                if (array[j, p] != 0)
                    listOfIndexes.Add(j);
            }
            return listOfIndexes;
        }// sprawdza ktore indeksy sa rozne od 0


        public float[,] TakeIndexValues(List<int> listOfIndexes, float[,] array, float d)
        {

            // czyli kolumny z macierzy P o indeksach w _I_u_)
            var arrayIndexValues = new float[(int)d, listOfIndexes.Count];
            // np listOfIndexes to [4,6,7] to wynikiem ma byc dwuwymiarowa tablica wartosci o indexach [ [ [0,4],[0,6],[0,7] ] , [ [1,4], [1,6], [1,7] ] , ... , [ [n,4], [n,6], [n,7] ] ],
            //a wartosci pod tymi indeksami wziete z array, a array to lista random_product
            for (int i = 0; i < d; i++)
            {
                var j = 0;
                foreach (var index in listOfIndexes)
                {
                    arrayIndexValues[i, j++] = array[i, index];
                }
            }
            return arrayIndexValues;

        }


       

        public float[,] CreateEye(float size) // stworz macierz jednostowa o zadanym rozmiarze
        {
            var eye = new float[,] { };
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; i++)
                {
                    if (i == j) eye[i, j] = 1;
                    else eye[i, j] = 0;
                }
            }
            return eye;
            // return macierz_jednostkowa
        }


        //TO DO
        //A_u = np.matmul(P_I_u, P_I_u_T) + reg* E 
        // do tego w sumie potrzebna metoda transponowania, a reszte w sumie mamy. mozna to eventualnie zamknac w metode dokonujaca tego mnozenia kilku elementow.


        /*Następnie liczymy _V_u_, czyli musimy dodać do siebie:

        kolumnę 4 z macierzy P pomnożoną przez 4 (ocena)
        kolumnę 6 z macierzy P pomnożoną przez 5 (ocena)
        kolumnę 7 z macierzy P pomnożoną przez 4 (ocena)
        */
        public float[,] createCloneArray(float[,] matrix)
        {
            var newArray = new float[matrix.GetLength(0), matrix.GetLength(1)];

            for( var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    newArray[i, j] = matrix[i, j];
                }

            }
            return newArray;
        }

        public float count_difference(float [,] original, float [,] changed)
        {
            float diff = 0;

            for (var i = 0; i < original.GetLength(0); i++)
            {
                for (var j = 0; j < original.GetLength(1); j++)
                {
                    diff += Math.Abs(original[i, j] - changed[i, j]);
                }

            }

            return diff / (original.GetLength(0) * original.GetLength(1) );
        }

        public float[] Count_V_u(List<int> listOfIndexes, float[,] arrayIndexValues, int[,] RatingsMatrix, int u)
        {
            var V_u = new float[listOfIndexes.Count];
            var j = 0;

            foreach (var index in listOfIndexes)
            {
                V_u[j++] = 0;
            }

            var i = 0;
            j = 0;
            for (; j < listOfIndexes.Count; j++)
            {
                foreach (var index in listOfIndexes) // oceny : 4, 5, 4
                {
                    V_u[j] += arrayIndexValues[i++, j] * RatingsMatrix[u, index];
                }
                i = 0;
            }

            return V_u;

            //czyli chcemy uzyskac w tym przypadku macierz 1x3 np [ 1, 2, 3]
        }

        public float[] Count_W_p(List<int> listOfIndexes, float[,] arrayIndexValues, int[,] RatingsMatrix, int p)
        {
            var W_p = new float[listOfIndexes.Count];
            var k = 0;

            foreach (var index in listOfIndexes)
            {
                W_p[k++] = 0;
            }

            var i = 0;
            for (var j = 0; j < listOfIndexes.Count; j++)
            {
                foreach (var index in listOfIndexes) // oceny : 4, 5, 4
                {
                    W_p[j] += arrayIndexValues[i++, j] * RatingsMatrix[index, p];
                }
                i = 0;
            }

            return W_p;

            //czyli chcemy uzyskac w tym przypadku macierz 1x3 np [ 1, 2, 3]
        }

        public float[,] SwitchGaussColumn(int u, float[,] matrix, float[] GaussResult)
        {

            var ArrayAfterSwitch = new float[matrix.GetLength(0), matrix.GetLength(1)];

            for( var i = 0; i < matrix.GetLength(0); i++)
            {
                for ( var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i == u) ArrayAfterSwitch[i, j] = GaussResult[i];
                    else ArrayAfterSwitch[i, j] = matrix[i, j];
                }
            }
            return ArrayAfterSwitch;
        }
 

    }

}
