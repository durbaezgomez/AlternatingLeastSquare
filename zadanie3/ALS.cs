﻿using System;
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

        public List<int> FlatNoZeroOnRow(int userIndex, int[,] array) 
        {
            var listOfIndexes = new List<int>();         
            for (int j = 0; j < array.GetLength(1); j++) 
            {
                if (array[userIndex, j] != 0)
                    listOfIndexes.Add(j);
            }
            return listOfIndexes;
        }
        public List<int> FlatNoZeroOnColumn(int productIndex, int[,] array) 
        {
            var listOfIndexes = new List<int>();        
            for (int j = 0; j < array.GetLength(0); j++) 
            {
                if (array[j, productIndex] != 0)
                    listOfIndexes.Add(j);
            }
            return listOfIndexes;
        }


        public float[,] TakeIndexValues(List<int> listOfIndexes, float[,] array, float dimension)
        {
            var arrayIndexValues = new float[(int)dimension, listOfIndexes.Count];     
            for (int i = 0, j = 0; i < dimension; i++)
            {
                foreach (var index in listOfIndexes)
                {
                    arrayIndexValues[i, j++] = array[i, index];
                }
                j = 0;
            }
            return arrayIndexValues;
        }
  

        public float[,] CreateEye(float size)
        {
            var eye = new float[(int)size, (int)size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    eye[i, j] = (i == j) ? 1 : 0;
                }
            }
            return eye;
        }


        public float[] Count_V_u(List<int> listOfIndexes, float[,] arrayIndexValues, int[,] RatingsMatrix, int userIndex)
        {
            var V_u = new float[listOfIndexes.Count];
            int j = 0;

            foreach (var index in listOfIndexes)
            {
                V_u[j++] = 0;
            }
            j = 0;

            for (int i = 0; j < listOfIndexes.Count; j++)
            {
                foreach (var index in listOfIndexes)
                {
                    V_u[j] += arrayIndexValues[i++, j] * RatingsMatrix[userIndex, index];
                }
                i = 0;
            }
            return V_u;
        }

        public float[] Count_W_p(List<int> listOfIndexes, float[,] arrayIndexValues, int[,] RatingsMatrix, int productIndex)
        {
            var W_p = new float[listOfIndexes.Count];
            var k = 0;

            foreach (var index in listOfIndexes)
            {
                W_p[k++] = 0;
            }

            for (int j = 0, i = 0; j < listOfIndexes.Count; j++)
            {
                foreach (var index in listOfIndexes)
                {
                    W_p[j] += arrayIndexValues[i++, j] * RatingsMatrix[index, productIndex];
                }
                i = 0;
            }
            return W_p;
        }

        public float[,] SwitchGaussColumn(int userIndex, float[,] matrix, float[] GaussResult)
        {
            var arrayAfterSwitch = new float[matrix.GetLength(0), matrix.GetLength(1)];
            for(var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    arrayAfterSwitch[i, j] = (i == userIndex) ? GaussResult[i] : matrix[i, j];                   
                }
            }
            return arrayAfterSwitch;
        }
 

    }

}
