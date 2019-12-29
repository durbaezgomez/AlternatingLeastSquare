using System;

namespace zadanie3
    
{
    class Program
    {
        static void Main(string[] args)
        {
            var alsObject = new ALS();
            var matrixProvider = new MatrixProvider();

            var matrixU = matrixProvider.MatrixU;
            var matrixP = matrixProvider.MatrixP;

            var matrixU_clone = matrixU.Clone() as float[,];
            var matrixP_clone = matrixP.Clone() as float[,];

            var ratings = new int[,] { { 0, 0, 0, 0, 4, 0, 5, 4, 0, 0 },
                                    { 4, 0, 4, 0, 0, 4, 0, 0, 0, 4 },
                                    { 5, 4, 5, 5, 0, 5, 5, 5, 5, 5 },
                                    { 0, 5, 5, 0, 5, 0, 0, 5, 0, 5 },
                                    { 0, 5, 5, 0, 5, 0, 0, 5, 0, 5 } };

            float dimension = 3;
            float reg = 0.1F; 

            for (var i = 0; i < 200; i++) 
            {

                for (int userIndex = 0; userIndex < ratings.GetLength(0); userIndex++)
                {
                    Console.WriteLine("usert index = " + userIndex);
                    var I_u = alsObject.FlatNoZeroOnRow(userIndex, ratings);
                    Utility<int>.PrintFlatList(I_u);


                    var P_I_u = alsObject.TakeIndexValues(I_u, matrixP_clone, dimension);
                    Utility<float>.PrintMatrix(P_I_u);

                    var P_I_u_T = Matrix.Transpose(P_I_u);
                    Utility<float>.PrintMatrix(P_I_u_T);

                    var E_p = alsObject.CreateEye(dimension);
                    Utility<float>.PrintMatrix(E_p);

                    var A_u = Matrix.Summing(Matrix.Multiplication(P_I_u, P_I_u_T), Matrix.Multiplication(E_p, reg));
                    Utility<float>.PrintMatrix(A_u);

                    var V_u = alsObject.Count_V_u(I_u, P_I_u_T, ratings, userIndex);
                    Utility<float>.PrintFlatArray(V_u);

                    var usingMatrix = new Matrix(A_u, V_u);
                    usingMatrix.CalculatePG(A_u, V_u);
                    var solutionOnU = usingMatrix.VectorXGauss;
                    Utility<float>.PrintFlatArray(solutionOnU);

                    matrixU_clone = alsObject.SwitchGaussColumn(userIndex, matrixU, solutionOnU);
                    Utility<float>.PrintMatrix(matrixU_clone);
                    Console.WriteLine("kolejna petla");
                }

                for (var productIndex = 0; productIndex < ratings.GetLength(1); productIndex++) 
                {
                    Console.WriteLine("product index = " + productIndex);

                    var I_p = alsObject.FlatNoZeroOnColumn(productIndex, ratings);
                    Utility<int>.PrintFlatList(I_p);

                    var U_I_p = alsObject.TakeIndexValues(I_p, matrixU_clone, dimension);
                    Utility<float>.PrintMatrix(U_I_p);

                    var U_I_p_T = Matrix.Transpose(U_I_p);
                    Utility<float>.PrintMatrix(U_I_p_T);

                    var E_u = alsObject.CreateEye(dimension);
                    Utility<float>.PrintMatrix(E_u);

                    var B_u = Matrix.Summing(Matrix.Multiplication(U_I_p, U_I_p_T), Matrix.Multiplication(E_u, reg));
                    Utility<float>.PrintMatrix(B_u);

                    var W_p = alsObject.Count_W_p(I_p, U_I_p_T, ratings, productIndex);
                    Utility<float>.PrintFlatArray(W_p);

                    var usingMatrix = new Matrix(B_u, W_p);

                    usingMatrix.CalculatePG(B_u, W_p);
                    var solutionOnP = usingMatrix.VectorXGauss;

                    Utility<float>.PrintFlatArray(solutionOnP);

                    matrixP_clone = alsObject.SwitchGaussColumn(productIndex, matrixP, solutionOnP);
                    Utility<float>.PrintMatrix(matrixP_clone);
                }

            }
            Utility<float>.PrintMatrix(matrixU_clone);
            Utility<float>.PrintMatrix(matrixP_clone);
            var matrixU_clone_T = Matrix.Transpose(matrixU_clone);
  
            var final_result = Matrix.Multiplication(matrixU_clone_T, matrixP_clone);
            Utility<float>.PrintMatrix(final_result);

        }
    }
}