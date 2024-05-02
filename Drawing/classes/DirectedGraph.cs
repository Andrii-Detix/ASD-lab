using System;
using System.Drawing;

namespace Drawing.classes
{
    public class DirectedGraph 
    {
        public DirectedGraph(int variant, double[] coefs, float width, float height)
        {
            int[] numbers = HelpMethods.GetNumbers(variant);
            Length = 10 + numbers[3];
            Matrix = CreateMatrix(Length, numbers, coefs);
            NodePoints = CreatePoints(width, height);
            IsDirected = CheckDirection();
        }

        public DirectedGraph(int[,] matrix, float width, float height)
        {
            Matrix = matrix;
            Length = matrix.GetLength(0);
            NodePoints = Length ==4? CreateFourPoints(width,height):CreatePoints(width, height);
            IsDirected = CheckDirection();
        }



        public int Length { get; }
        public PointF[] NodePoints { get; }
        public int[,] Matrix { get; }
        public readonly bool IsDirected  ;


        protected double[,] FillMatrix(double[,] matrix, int seed)
        {
            Random random = new Random(seed);
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    matrix[i, j] = random.NextDouble() + random.Next(2);
                }
            }

            return matrix;
        }
        

        protected int[,] MulMatrix(double[,] matrix, double k)
        {
            int[,] intMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    matrix[i, j] *= k;
                    intMatrix[i, j] = (int)Math.Floor(matrix[i, j]);
                }
            }

            return intMatrix;
        }

        protected virtual int[,] CreateMatrix(int length, int[] varianNumbers, double[] coefs)
        {
            double[,] matrix = new double[length, length];
            matrix = FillMatrix(matrix, varianNumbers[0]);
            double k = coefs[0] ;
            for (int i = 1; i < varianNumbers.Length; i++)
                k += coefs[i] * varianNumbers[i];
            return MulMatrix(matrix, k);
        }

        private PointF[] CreatePoints(float width, float height)
        {
            float borderCoef = 0.15f;
            PointF[] points = new PointF[Length];
            bool isOdd = Length % 2 == 0;
            int helper = Length - (isOdd ? 2 : 1);
            int lineIntervalsNum = (int)Math.Ceiling(helper / 4m);
            int columnIntervalsNum = helper / 2 - lineIntervalsNum;
            float posX = width * borderCoef;
            float posY = height * borderCoef;
            float distX = width * (1 - 2 * borderCoef) / lineIntervalsNum;
            float distY = height * (1 - 2 * borderCoef) / columnIntervalsNum;
            int counter = 0;
            for (int i = 0; i < lineIntervalsNum; i++)
            {
                points[counter] = new PointF(posX, posY);
                posX += distX;
                counter++;
            }

            for (int i = 0; i < columnIntervalsNum; i++)
            {
                points[counter] = new PointF(posX, posY);
                posY += distY;
                counter++;
            }
            
            if (isOdd)
            {
                distX *= (float)lineIntervalsNum / (lineIntervalsNum + 1);
                lineIntervalsNum++;
            }

            for (int i = 0; i < lineIntervalsNum; i++)
            {
                points[counter] = new PointF(posX, posY);
                posX -= distX;
                counter++;
            }

            posX = points[0].X;
            for (int i = 0; i < columnIntervalsNum; i++)
            {
                points[counter] = new PointF(posX, posY);
                posY -= distY;
                counter++;
            }

            points[counter] = new PointF(width / 2, height / 2);
            return points;
        }
        private PointF[] CreateFourPoints(float width, float height)
        {
            float borderCoef = 0.15f;
            PointF[] points = new PointF[4];
            float posX = width * borderCoef;
            float posY = height * borderCoef;
            float distX = width - width * (1-2 * borderCoef);
            float distY = height - height * (1 - 2 * borderCoef);
            points[0] = new PointF(posX, posY);
            posX += distX;
            points[1] = new PointF(posX, posY);
            posY += distY;
            points[2] = new PointF(posX, posY);
            points[3] = new PointF(points[0].X, posY);
            return points;
        }

        protected virtual bool CheckDirection() => true;

        public virtual EdgeLinkedList ToLinkedList()
        {
            EdgeLinkedList list = new EdgeLinkedList();
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if(Matrix[i,j]==1)
                        list.AddFirst(new Edge(i,j,0));
                }
            }

            return list;
        }
    }


    public class UndirectedGraph : DirectedGraph
    {
        public UndirectedGraph(int variant,double[]coefs, float width, float height) : base(variant, coefs, width, height)
        {
            
        }

        public UndirectedGraph(int[,] matrix, float width, float height) : base(matrix, width, height)
        {
            
        }

        protected override int[,] CreateMatrix(int length, int[] variantNumbers, double[] coefs)
        {
            int[,] matrix = base.CreateMatrix(length, variantNumbers, coefs);
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (matrix[i, j] == 1)
                        matrix[j, i] = matrix[i, j];
                }
            }

            return matrix;
        }

        protected override bool CheckDirection() => false;
    }

    public class WeightedGraph : UndirectedGraph
    {
        public WeightedGraph(int variant,double[]coefs, float width, float height) : base(variant, coefs, width, height)
        {
            WeightedMatrix = GetWeightMatrix(Length, variant);
        }

        public WeightedGraph(int[,] adjMatrix, int[,] weightedMatrix, float width, float height):base(adjMatrix,width,height)
        {
            WeightedMatrix = weightedMatrix;
        }
        
        public int[,] WeightedMatrix { get;  }

        protected int[,] GetWeightMatrix(int length, int variant)
        {
            double[,] B = new double[length, length];
            B = FillMatrix(B, variant);
            int[,] C = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    C[i, j] = (int)Math.Ceiling(B[i, j] * 100 * Matrix[i, j]);
                }
            }

            int[,] D = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    D[i, j] = (C[i, j] == 0 ? 0 : 1);
                }
            }

            int[,] H = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = i; j < length; j++)
                {
                    int num = (D[i, j] == D[j, i] ? 0 : 1);
                    H[i, j] = num;
                    H[j, i] = num;
                }
            }

            int[,] Tr = ActMatrix.HighTriangleUnitMatrix(length);
            int[,] weighted = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = i; j < length; j++)
                {
                    int value = (D[i, j] + H[i, j] * Tr[i, j]) * C[i, j];
                    weighted[i, j] = value;
                    weighted[j, i] = value;
                }
            }

            return weighted;
        }

        public override EdgeLinkedList ToLinkedList()
        {
            EdgeLinkedList list = new EdgeLinkedList();
            for (int i = 0; i < Length; i++)
            {
                for (int j = i; j < Length; j++)
                {
                    if(Matrix[i,j] == 1)
                        list.AddFirst(new Edge(i,j,WeightedMatrix[i,j]));
                }
            }

            return list;
        }
    }
}
