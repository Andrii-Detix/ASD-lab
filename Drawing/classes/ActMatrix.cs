
namespace Drawing.classes
{
    public static class ActMatrix
    {

        public static int[,] MultMatrix(int[,] matA, int[,] matB)
        {
            int firInd = matA.GetLength(0);
            int secInd = matB.GetLength(1);
            int midInd = matA.GetLength(1);
            int[,] result = new int[firInd, secInd];
            int val;
            for (int i = 0; i < firInd; i++)
            {
                for (int j = 0; j < secInd; j++)
                {
                    val = 0;
                    for (int k = 0; k < midInd; k++)
                    {
                        val += matA[i, k] * matB[k, j];
                    }

                    result[i, j] = val;
                }
            }
            
            return result;
        }

        public static int[,] PowMatrix(int[,] matrix, int pow)
        {
            int[,] result = CopyMatrix(matrix);

            for (int i = 1; i < pow; i++)
            {
                result = MultMatrix(result, matrix);
            }

            return result;

        }

        public static int[,] CopyMatrix(int[,] matrix)
        {
            int first = matrix.GetLength(0);
            int second = matrix.GetLength(1);
            int[,] result = new int[first, second];
            for (int i = 0; i < first; i++)
            {
                for (int j = 0; j < second; j++)
                {
                    result[i, j] = matrix[i, j];
                }
            }

            return result;
        }
        public static int[,] TransposeMatrix(int[,] matrix)
        {
            int length = matrix.GetLength(0);
            int[,] result = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }
        public static int[,] GetUnitMatrix(int length)
        {
            int[,] matrix = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                matrix[i, i] = 1;
            }

            return matrix;
        }
        public static int[,] AddMatrix(int[,] matrixA, int[,] matrixB)
        {
            int firstInd = matrixA.GetLength(0);
            int secondInd = matrixA.GetLength(1);
            int[,] result = new int[firstInd, secondInd];
            for (int i = 0; i < firstInd; i++)
            {
                for (int j = 0; j < secondInd; j++)
                {
                    result[i, j] = matrixA[i, j] + matrixB[i, j];
                }
            }

            return result;
        }
        public static int[,] BooleanMapping(int[,] matrix)
        {
            int firstInd = matrix.GetLength(0);
            int secondInd = matrix.GetLength(1);
            int[,] result = new int[firstInd, secondInd];
            for (int i = 0; i < firstInd; i++)
            {
                for (int j = 0; j < secondInd; j++)
                {
                    result[i, j] = matrix[i, j] == 0 ? 0 : 1;

                }
            }

            return result;
        }
    }
}