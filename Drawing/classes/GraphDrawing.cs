using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Drawing.classes
{
    internal static class GraphDrawing
    {
        static public void DrawNodes(DirectedGraph graph, Graphics g)
        {
            string str;
            PointF pos = new PointF();
            for (int i = 0; i<graph.Length; i++)
            {
                pos.X = graph.NodePoints[i].X - Constants.radius;
                pos.Y = graph.NodePoints[i].Y - Constants.radius;
                str = Convert.ToString(i+1);
                g.FillEllipse(Brushes.Black, pos.X, pos.Y, Constants.diameter, Constants.diameter);
                pos.X += Constants.radius / 4;
                pos.Y += Constants.radius / 4;
                g.DrawString(str, Constants.font, Brushes.White, pos);
            }
        }
        static public void DrawMatrix(DirectedGraph graph, Graphics g, PointF startPos, PointF endPos)
        {
            string str = string.Empty;
            int lastIndex = graph.Length - 1;
            for (int i = 0;i<graph.Length;i++)
            {

                for (int j = 0;j<graph.Length;j++)
                {
                    str += Convert.ToString(graph.Matrix[i, j]);
                    if (j == lastIndex) break;
                    str += " ";
                }
                if (i == lastIndex) break;
                str += "\n";
            }
            float width = endPos.X - startPos.X;
            float height = endPos.Y - startPos.Y;
            float size = (width >height ? width : height) / (2*graph.Length);
            Font font = new Font("Times New Roman", size);
            g.DrawString(str,font,Brushes.Black,startPos);
        }
        static public void DrawConnection(DirectedGraph graph, Graphics g, bool isUndirected = false)
        {
            int count = 0;
            Pen pen = new Pen(Brushes.Blue, 1);
            if(!isUndirected)
                pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3,3);
            int index = 0;
            int lenHalf = graph.Length / 2;
            int lastInd = graph.Length - 1;
            int dif;
            float coef;
            PointF from, to;
            for (int i = 0; i<graph.Length; i++)
            {
                for (int j = index; j < graph.Length; j++)
                {
                    if (graph.Matrix[i, j] == 1)
                    {
                        count++;
                        from = graph.NodePoints[i];
                        to = graph.NodePoints[j];
                        dif = j - i;
                        if (i == j)
                        {
                            g.DrawArc(pen, from.X, from.Y - Constants.diameter, Constants.diameter, Constants.diameter, 180, 270);
                        }
                        else if (dif == 1 || dif == -1 && graph.Matrix[j, i] == 0)
                        {
                            if (from.X == to.X)
                            {
                                to.Y+= (to.Y > from.Y ? -Constants.radius : Constants.radius);
                                g.DrawLine(pen, from.X, from.Y, to.X, to.Y);

                            }
                            else if (from.Y == to.Y)
                            {
                                to.X += (to.X > from.X ? -Constants.radius : Constants.radius);
                                g.DrawLine(pen, from.X, from.Y,to.X, to.Y);
                            }

                        }
                        else if (j == lastInd ||
                            (dif > lenHalf + 1 || dif < lenHalf - 1) &&
                            dif>1 &&from.X !=to.X && from.Y != to.Y)
                        {
                            to = FindPointOnContour(from, to);
                            g.DrawLine(pen, from, to);
                        }
                        else
                        {
                            coef = 0.7f;
                            PointF middle = new PointF();
                            PointF[] points;
                            if (from.X == to.X)
                            {
                                float bias = (to.Y-from.Y)* coef;
                                middle.Y = from.Y + bias;
                                if(to.Y> from.Y)
                                {

                                    from.X -= Constants.radius;
                                    middle.X = from.X - Constants.diameter;
                                }
                                else
                                {
                                    from.X += Constants.radius;
                                    middle.X = from.X + Constants.diameter;
                                }

                            }
                            else if (from.Y == to.Y)
                            {
                                float bias = (to.X - from.X) * coef;
                                middle.X = from.X + bias;
                                if (to.X > from.X)
                                {
                                    from.Y -= Constants.radius;
                                    middle.Y = from.Y - Constants.diameter; 
                                }
                                else
                                {
                                    from.Y += Constants.radius;
                                    middle.Y = from.Y + Constants.diameter;
                                }
                            }
                            else if(i == lastInd)
                            {
                                middle.X = (to.X+from.X)/2 + (to.X>from.X? -Constants.radius : Constants.radius);
                                middle.Y = (to.Y + from.X) / 2 + (to.Y > from.Y ? -Constants.radius : Constants.radius);
                                to = FindPointOnContour(middle, to);
                                points = new PointF[] { from, middle, to };
                                g.DrawLines(pen, points);
                            }
                            else
                            {
                                coef = 0.6f + count % 10 / 50f;
                                float width = to.X - from.X;
                                float height = to.Y - from.Y;
                                if (HelpMethods.CheckDifPos(from.X, to.X, graph.NodePoints[lastInd].X))
                                {
                                    if (HelpMethods.CheckDifPos(from.Y, to.Y, graph.NodePoints[lastInd].Y))
                                    {
                                        middle.X = from.X + width * coef;
                                        float enderPos = from.Y == graph.NodePoints[lastInd].Y ? to.Y : from.Y;
                                        middle.Y = (enderPos + graph.NodePoints[lastInd].Y) / 2;

                                    }
                                    else
                                    {
                                        middle.X = from.X + width * coef;
                                        float enderPos = from.Y == graph.NodePoints[lastInd].Y ? to.Y : graph.NodePoints[lastInd].Y;
                                        middle.Y = (from.Y + enderPos) / 2;
                                    }
                                }
                                else
                                {
                                    if (HelpMethods.CheckDifPos(from.Y, to.Y, graph.NodePoints[lastInd].Y))
                                    {
                                        middle.Y = from.Y + height * coef;
                                        middle.X = (from.X + to.X) / 2;
                                    }
                                    else
                                    {
                                        middle.X = from.X + width * coef;
                                        float enderPos = from.Y == to.Y ? graph.NodePoints[lastInd].Y : to.Y;
                                        middle.Y = (from.Y + to.Y) / 2;
                                    }
                                    
                                }
                            }
                            to = FindPointOnContour(middle, to);
                            points = new PointF[] { from, middle, to };
                            g.DrawLines(pen, points);
                        }
                        
                    }
                }
                if (isUndirected) index++;
            }
        }
        static private PointF FindPointOnContour(PointF from, PointF to)
        {
            float width = to.X - from.X;
            float height = to.Y - from.Y;
            float dist = (float)Math.Sqrt(width * width + height * height);  
            float coef = (dist - Constants.radius) / dist;
            return new PointF(from.X + width*coef,from.Y+ height*coef);
        }
        static public void DrawGraph(DirectedGraph graph, Graphics g, bool isUndirected)
        {
            DrawConnection(graph, g, isUndirected);
            DrawNodes(graph, g);
        }
    }
}
