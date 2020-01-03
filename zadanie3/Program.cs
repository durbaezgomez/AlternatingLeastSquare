using System;

namespace zadanie3
    
{
    class Program
    {
        static void Main(string[] args)
        {

            int prodAmount = 10;
            int userAmount = 5;

            var matrixProvider = new MatrixProvider(prodAmount, userAmount); // RatingsMatrix ustalony przy inicjalizacji na 40x20

            AlsRun(prodAmount, userAmount, matrixProvider); // dla malego zbioru

            prodAmount = 100;
            userAmount = 50;
            AlsRun(prodAmount, userAmount, matrixProvider); // dla sredniego zbioru


            //prodAmount = 1000;
            //userAmount = 500;
            //matrixProvider.GetCroppedDataFromDataProvider(prodAmount, userAmount);
            //AlsRun(prodAmount, userAmount, matrixProvider); // dla duzego zbioru



        }



        private static void AlsRun(int prodAmount, int userAmount, MatrixProvider matrixProvider)
        {
            
            var alsObject = new ALS();
           
            var lambdaArray = new float[] { 0.1F, 0.5F, 1.0F };
            var dimensionArray = new float[] { 3, 8, 13 };

            var ratings = matrixProvider.GetCroppedDataFromDataProvider(prodAmount, userAmount);

            foreach ( var reg in lambdaArray)
            {
                foreach ( var dimension in dimensionArray)
                {
                    var matrixU = matrixProvider.CreateMatrixU((int)dimension, userAmount);
                    var matrixP = matrixProvider.CreateMatrixP((int)dimension, prodAmount);

                    var matrixU_clone = matrixU.Clone() as float[,];
                    var matrixP_clone = matrixP.Clone() as float[,];

                    
                    for (var i = 0; i < 100; i++)
                    {

                        for (int userIndex = 0; userIndex < ratings.GetLength(0); userIndex++)
                        {
                            var I_u = alsObject.FlatNoZeroOnRow(userIndex, ratings);
                            var P_I_u = alsObject.TakeIndexValues(I_u, matrixP_clone, dimension);
                            var P_I_u_T = Matrix.Transpose(P_I_u);

                            var E_p = alsObject.CreateEye(dimension);

                            var A_u = Matrix.Summing(Matrix.Multiplication(P_I_u, P_I_u_T), Matrix.Multiplication(E_p, reg));
                            var V_u = alsObject.Count_V_u(I_u, P_I_u_T, ratings, userIndex);

                            var usingMatrix = new Matrix(A_u, V_u);
                            usingMatrix.CalculatePG(A_u, V_u);
                            var solutionOnU = usingMatrix.VectorXGauss;

                            matrixU_clone = alsObject.SwitchGaussColumn(userIndex, matrixU_clone, solutionOnU);
                        }

                        for (var productIndex = 0; productIndex < ratings.GetLength(1); productIndex++)
                        {

                            var I_p = alsObject.FlatNoZeroOnColumn(productIndex, ratings);
                            var U_I_p = alsObject.TakeIndexValues(I_p, matrixU_clone, dimension);
                            var U_I_p_T = Matrix.Transpose(U_I_p);

                            var E_u = alsObject.CreateEye(dimension);

                            var B_u = Matrix.Summing(Matrix.Multiplication(U_I_p, U_I_p_T), Matrix.Multiplication(E_u, reg));

                            var W_p = alsObject.Count_W_p(I_p, U_I_p_T, ratings, productIndex);

                            var usingMatrix = new Matrix(B_u, W_p);

                            usingMatrix.CalculatePG(B_u, W_p);
                            var solutionOnP = usingMatrix.VectorXGauss;

                            matrixP_clone = alsObject.SwitchGaussColumn(productIndex, matrixP_clone, solutionOnP);
                        }

                    }

                    var matrixU_clone_T = Matrix.Transpose(matrixU_clone);

                    var final_result = Matrix.Multiplication(matrixU_clone_T, matrixP_clone);
                    Console.WriteLine("For lambda = " + reg + " and d = " + dimension);
                    Utility<float>.PrintMatrixComparison(ratings, final_result);


                }
            }         

        }
    }
}