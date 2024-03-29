using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Drawing.classes;

namespace Drawing.classes
{
    class DirectedGraph
    {
        public DirectedGraph(int variant, float width, float height) 
        {
            int[] numbers = HelpMethods.GetNumbers(variant);
            Length = 10 + numbers[3];
            Matrix = CreateMatrix(Length, numbers);
            NodePoints = CreatePoints(width, height);
        }
       
        public int Length;
        public PointF[] NodePoints { get;  }
        public int[,] Matrix { get; private set; }
        

        protected double[,] FillMatrix(double[,] matrix, int seed)
        {
            Random random = new Random(seed);
            for (int i = 0; i < Length; i++) 
            { 
                for (int j = 0; j < Length; j++)
                {
                    matrix[i,j] = random.NextDouble() + random.Next(2);
                }
            }
            return matrix;
        }
        protected int[,] MulMatrix(double[,] matrix, double k)
        { 
            int[,] intMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for(int i = 0;i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    matrix[i,j] *= k;
                    intMatrix[i,j] = (int)Math.Floor(matrix[i,j]);
                }
            }
            return intMatrix;
        }
        protected virtual int[,] CreateMatrix(int length, int[] variantNumbers)
        {
            double[,] matrix = new double[length, length];
            matrix = FillMatrix(matrix, variantNumbers[0]);
            double k = 1 - 0.02 * variantNumbers[3] - 0.005 * variantNumbers[4] - 0.25;
            return MulMatrix(matrix, k);
        }
        private PointF[] CreatePoints(float width, float height)
        {
            float borderCoef = 0.15f;
            PointF[] points = new PointF[Length];
            int lineIntervalsNum, columnIntervalsNum;
            bool isOdd = Length % 2 == 0;
            int helper = Length - (isOdd ? 2 : 1);
            lineIntervalsNum = (int)Math.Ceiling(helper / 4m) ;
            columnIntervalsNum = helper / 2 - lineIntervalsNum;
            float posX = width * borderCoef;
            float posY = height * borderCoef;
            float distX = width * (1 - 2 * borderCoef) / lineIntervalsNum;
            float distY = height * (1 - 2 * borderCoef) / columnIntervalsNum;
            int counter = 0;
            for (int i = 0;i<lineIntervalsNum;i++)
            {
                points[counter] = new PointF(posX, posY);
                posX +=distX;
                counter++;
            }
            for (int i = 0; i<columnIntervalsNum; i++)
            {
                points[counter] = new PointF(posX, posY);
                posY +=distY;
                counter++;
            }
            if (isOdd)
            {
                distX *= (float)lineIntervalsNum / (lineIntervalsNum+1);
                lineIntervalsNum++;
            }
            for (int i = 0; i< lineIntervalsNum; i++)
            {
                points[counter] = new PointF(posX, posY);
                posX -= distX;
                counter++;
            }
            for(int i=0;i<columnIntervalsNum; i++)
            {
                points[counter] = new PointF(posX, posY);
                posY -= distY;
                counter++;
            }
            points[counter] = new PointF(width / 2, height / 2);
            return points;
        }
    } 
}

class UndirectedGraph : DirectedGraph
{
    public UndirectedGraph(int variant, float width, float height) : base(variant, width, height)
    {

    }
    protected override int[,] CreateMatrix(int length, int[] variantNumbers)
    {
       int[,] matrix = base.CreateMatrix(length, variantNumbers);
       int index = 0;
       for (int i = 0;i < Length;i++)
        {
            for (int j = 0;j < index;j++)
            {
                matrix[j,i] = matrix[i,j];
            }
            index++;
        }
       return matrix;
    }
}

