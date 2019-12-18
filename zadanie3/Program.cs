namespace zadanie3
{
    class Program
    {
        static void Main(string[] args)
        {
            var alsObject = new ALS();
            var matrixProvider = new MatrixProvider();

            Utility<int>.PrintMatrix(matrixProvider.RatingsMatrix);

            var matrixU = matrixProvider.MatrixU;
            var matrixP = matrixProvider.MatrixP;

            var matrixU_clone = matrixU.Clone() as float[,];
            var matrixP_clone = matrixP.Clone() as float[,];

            var ratings = matrixProvider.RatingsMatrix;

            float dimension = 3;
            float reg = 0.1F; 

            for (var i = 0; i < 100; i++) 
            {

                for (int userIndex = 0; userIndex < ratings.GetLength(0); userIndex++)
                { 

                    var I_u = alsObject.FlatNoZeroOnRow(userIndex, ratings);

                    var P_I_u = alsObject.TakeIndexValues(I_u, matrixP_clone, dimension);
                    var P_I_u_T = Matrix<float>.Transpose(P_I_u);
                   
                    var E_p = alsObject.CreateEye(dimension);

                    var A_u = Matrix<float>.Summing(Matrix<float>.Multiplication(P_I_u, P_I_u_T), Matrix<float>.Multiplication(E_p, reg)); 

                    var V_u = alsObject.Count_V_u(I_u, P_I_u, ratings, userIndex);

                    var usingMatrix = new Matrix<float>(A_u, V_u);
                    usingMatrix.CalculatePG(A_u, V_u);
                    var solutionOnU = usingMatrix.VectorXGauss;

                    matrixU_clone = alsObject.SwitchGaussColumn(userIndex, matrixU, solutionOnU);
                }

                for (var productIndex = 0; productIndex < ratings.GetLength(1); productIndex++) 
                {
                    var I_p = alsObject.FlatNoZeroOnColumn(productIndex, ratings);
                    var U_I_p = alsObject.TakeIndexValues(I_p, matrixU_clone, dimension);
                     var U_I_p_T = Matrix<float>.Transpose(U_I_p);
                    var E_u = alsObject.CreateEye(dimension);

                    var B_u = Matrix<float>.Summing(Matrix<float>.Multiplication(U_I_p, U_I_p_T), Matrix<float>.Multiplication(E_u, reg));

                    var W_p = alsObject.Count_W_p(I_p, U_I_p, ratings, productIndex);

                    var usingMatrix = new Matrix<float>(B_u, W_p);
                    usingMatrix.CalculatePG(B_u, W_p);
                    var solutionOnP = usingMatrix.VectorXGauss;

                    matrixP_clone = alsObject.SwitchGaussColumn(productIndex, matrixP, solutionOnP);
                }

            }

        }
    }
}