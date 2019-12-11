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

        private List<int> FlatNoZero(int u, int[,] array) //  In [38]:    arg: ilosc uzytkownikow, array z ocenami. Zwraca array z indeksami != 0 dla wiersza = u. 
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

        private float[,] TakeIndexValues(List<int> listOfIndexes, float[,] array, int d)
        {

            // czyli kolumny z macierzy P o indeksach w _I_u_)
            var arrayIndexValues = new float[d, listOfIndexes.Count];
            // np listOfIndexes to [4,6,7] to wynikiem ma byc dwuwymiarowa tablica wartosci o indexach [ [ [0,4],[0,6],[0,7] ] , [ [1,4], [1,6], [1,7] ] , ... , [ [n,4], [n,6], [n,7] ] ],
            //a wartosci pod tymi indeksami wziete z array, a array to lista random_product
            for (int i = 0; i < d; i++)
            {
                var j = 0;
                foreach (var index in listOfIndexes)
                {
                    arrayIndexValues[d, j++] = array[d, index];
                }
            }
            return arrayIndexValues;

        }

        private int[,] CreateEye(int size) // stworz macierz jednostowa o zadanym rozmiarze
        {
            var eye = new int[,] { };
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

        private float[] Count_V_u_(List<int> listOfIndexes, float[,] arrayIndexValues, int[,] RatingsMatrix, int u)
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

        private float[,] SwitchGaussColumn(int u, float[,] matrixU, float[] GaussResult)
        {

            var ArrayAfterSwitch = new float[matrixU.GetLength(0), matrixU.GetLength(1)];

            for( var i = 0; i < matrixU.GetLength(0); i++)
            {
                for ( var j = 0; j < matrixU.GetLength(1); j++)
                {
                    if (i == u) ArrayAfterSwitch[i, j] = GaussResult[i];
                    else ArrayAfterSwitch[i, j] = matrixU[i, j];
                }
            }
            return ArrayAfterSwitch;
        }

        //To teraz mnożymy rating przez P_i, dodajemy i wychodzi nam:

        //In[44]:
        //V_u = np.zeros(d).T
        //for i in I_u:
        //    V_u += ratings_matrix[u][i] * P[:, i] 

        //V_u




    }

}
