using System;
using System.Drawing;

namespace Drawing.classes
{
    internal static class GraphDrawing
    {
        /// <summary>
        /// Виводить матрицю на екран
        /// </summary>
        /// <param name="graph">граф, матрицю якого потрібно вивести</param>
        /// <param name="g">екземпляр класу Graphics</param>
        /// <param name="startPos">точка, з якої починається виведення матриці</param>
        /// <param name="endPos">крайня можлива точка для виведення матриці</param>
        public static void DrawMatrix(DirectedGraph graph, Graphics g, PointF startPos, PointF endPos)
        {
            string str = string.Empty;
            int lastIndex = graph.Length - 1;
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    str += Convert.ToString(graph.Matrix[i, j]);
                    if (j == lastIndex) break;
                    str += " ";
                }

                if (i == lastIndex) break;
                str += "\n";
            }

            SizeF size = g.MeasureString(str, new Font(Constants.FamilyName, 1));
            float width = (endPos.X - startPos.X) / size.Width;
            float height = (endPos.Y - startPos.Y) / size.Height;
            Font font = width <= height
                ? new Font(Constants.FamilyName, width)
                : new Font(Constants.FamilyName, height);
            g.DrawString(str, font, Brushes.Black, startPos);
        }

        /// <summary>
        /// Зображує графічно граф
        /// </summary>
        /// <param name="graph">граф, який потрібно зобразити</param>
        /// <param name="g">екземпляр класу Graphics</param>
        /// <param name="isUndirected">вказує на те, чи граф ненапрямлений</param>
        public static void DrawGraph(DirectedGraph graph, Graphics g, bool isUndirected)
        {
            DrawConnection(graph, g, isUndirected);
            DrawNodes(graph, g);
        }

        /// <summary>
        /// Зображує вершини графу
        /// </summary>
        /// <param name="graph">граф, який потрібно зобразити</param>
        /// <param name="g">екземпляр класу Graphics</param>
        private static void DrawNodes(DirectedGraph graph, Graphics g)
        {
            string str;
            PointF pos = new PointF();
            for (int i = 0; i < graph.Length; i++)
            {
                pos.X = graph.NodePoints[i].X - Constants.Radius;
                pos.Y = graph.NodePoints[i].Y - Constants.Radius;
                str = Convert.ToString(i + 1);
                g.FillEllipse(Brushes.Black, pos.X, pos.Y, Constants.Diameter, Constants.Diameter);
                pos.X += Constants.Radius / 4;
                pos.Y += Constants.Radius / 4;
                g.DrawString(str, Constants.Font, Brushes.White, pos);
            }
        }

        /// <summary>
        /// Зображує ребра, які з'єднують вершини, якщо між вершинами є зв'язок
        /// </summary>
        /// <param name="graph">граф, який потрібно зобразити</param>
        /// <param name="g">екземпляр класу Graphics</param>
        /// <param name="isUndirected">вказує на те, чи граф ненапрямлений</param>
        private static void DrawConnection(DirectedGraph graph, Graphics g, bool isUndirected = false)
        {
            int count = 0;
            Pen pen = new Pen(Brushes.Blue, 1);
            if (!isUndirected)
                pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 4);
            int index = 0;
            int lastIndex = graph.Length - 1;
            // dif - різниця між номерами вершин
            int dif;
            //from - початкова точка, to - кінцева точка, lastP - кооридинати центра останньої вершини
            PointF from, to, lastP;
            lastP = graph.NodePoints[lastIndex];
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = index; j < graph.Length; j++)
                {
                    if (graph.Matrix[i, j] == 1)
                    {
                        count++;
                        from = graph.NodePoints[i];
                        to = graph.NodePoints[j];
                        dif = j - i;

                        //Малює з'єднання вершини з собою
                        if (i == j)
                        {
                            g.DrawArc(pen, from.X, from.Y - Constants.Diameter, Constants.Diameter, Constants.Diameter,
                                180, 270);
                        }
                        //Малює пряму, якщо різниця між номерами вершин рівна 1, або якщо одне з ребер останнє
                        else if (dif == 1 || dif == -1 || j == lastIndex || i == lastIndex)
                        {
                            DrawLine(pen, from, to, g);
                        }
                        else
                        {
                            //Знаходимо сусідні вершини до початкової та кінцевої точки, щоб вони знаходилися 
                            //в прямокутному трикутнику обмеженому почоковою та кінцевою точками.
                            PointF firstNeighbour = graph.NodePoints[HelpMethods.CheckIndexLim(i + 1, lastIndex - 1)];
                            PointF lastNeighbour = graph.NodePoints[HelpMethods.CheckIndexLim(j - 1, lastIndex - 1)];
                            if (!(CheckMidPos(from, to, firstNeighbour) &&
                                  CheckMidPos(from, to, lastNeighbour)))
                            {
                                firstNeighbour = graph.NodePoints[HelpMethods.CheckIndexLim(i - 1, lastIndex - 1)];
                                lastNeighbour = graph.NodePoints[HelpMethods.CheckIndexLim(j + 1, lastIndex - 1)];
                            }

                            //Знаходимо точки на колах вершин так, щоб пряма, яка їх з'єднує була паралельна
                            //найкоротншій відстані
                            PointF[] points = ParallelDisPlaceOnCircles(from, to, 45);
                            // first, second - допоміжні точки
                            PointF first = points[0];
                            PointF second = points[1];
                            //Перевіряємо, чи пряма, утвореними цими точками, не перетинає центральну вершину
                            //та вершини сусідніх точок
                            if (!(LineIntersectsCircle(first, second, firstNeighbour) ||
                                  LineIntersectsCircle(first, second, lastNeighbour) ||
                                  LineIntersectsCircle(first, second, lastP)))
                                g.DrawLine(pen, first, second);

                            else
                            {
                                float coef = 0.7f;
                                PointF middle = new PointF();
                                PointF[] drawPoints;
                                bool changeEndPos = true;
                                //Якщо точки знаходяться на одній стороні, зображуємо ламану
                                if (from.X == to.X)
                                {
                                    float bias = (to.Y - from.Y) * coef;
                                    middle.Y = from.Y + bias;
                                    if (to.Y > from.Y)
                                    {
                                        from.X -= Constants.Radius;
                                        middle.X = from.X - Constants.Diameter;
                                    }
                                    else
                                    {
                                        from.X += Constants.Radius;
                                        middle.X = from.X + Constants.Diameter;
                                    }
                                }
                                //Якщо точки знаходяться на одній стороні, зображуємо ламану
                                else if (from.Y == to.Y)
                                {
                                    float bias = (to.X - from.X) * coef;
                                    middle.X = from.X + bias;
                                    if (to.X > from.X)
                                    {
                                        from.Y -= Constants.Radius;
                                        middle.Y = from.Y - Constants.Diameter;
                                    }
                                    else
                                    {
                                        from.Y += Constants.Radius;
                                        middle.Y = from.Y + Constants.Diameter;
                                    }
                                }

                                else
                                {
                                    coef = 0.6f + count % 10 / 50f;
                                    float width = to.X - from.X;
                                    float height = to.Y - from.Y;
                                    //Перевіряємо, чи усі точки знаходяться по один бік по довжині від
                                    //центральної вершини
                                    if (HelpMethods.CheckSamePos(from.X, to.X, lastP.X))
                                    {
                                        middle.Y = from.Y + height * coef;
                                        middle.X = (from.X + to.X + lastP.X) / 3;
                                        //Змінюємо довжину, допоки ламана, побудована на точках,
                                        //перетинатиме центральну вершину
                                        while (CheckCenterIntersect(from, middle, to, lastP))
                                        {
                                            middle.X += (from.X < lastP.X || to.X < lastP.X)
                                                ? -Constants.Diameter
                                                : Constants.Diameter;
                                        }
                                    }
                                    else
                                    {
                                        //Перевіряємо, чи усі точки знаходяться по один бік по висоті від
                                        //центральної вершини
                                        if (HelpMethods.CheckSamePos(from.Y, to.Y, lastP.Y))
                                        {
                                            middle.Y = (from.Y + to.Y + lastP.Y) / 3;
                                            middle.X = from.X + width * coef;
                                            //Змінюємо висоту, допоки ламана, побудована на точках,
                                            //перетинатиме центральну вершину
                                            while (CheckCenterIntersect(from, middle, to, lastP))
                                            {
                                                middle.Y += (from.Y < lastP.Y || to.Y < lastP.Y)
                                                    ? -Constants.Diameter
                                                    : Constants.Diameter;
                                            }
                                        }
                                        //Точки знаходяться в протилежних чвертях
                                        else
                                        {
                                            middle.Y = from.Y + height * coef;
                                            middle.X = (from.X + lastP.X) / 2;
                                            first = middle;
                                            second = middle;
                                            //Змінюємо довжину чи висоту допоки ламана, побудована на точках,
                                            //перетинатиме центральну вершину
                                            while (true)
                                            {
                                                if (CheckCenterIntersect(from, second, to, lastP))
                                                {
                                                    first.Y += from.Y < lastP.Y
                                                        ? -Constants.Diameter
                                                        : Constants.Diameter;
                                                    if (CheckCenterIntersect(from, first, to, lastP))
                                                    {
                                                        second.X += from.X < lastP.X
                                                            ? -Constants.Diameter
                                                            : Constants.Diameter;
                                                    }
                                                    else
                                                    {
                                                        middle = first;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    middle = second;
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    //Якщо ламана перетинатиме сусідні вершини, то змінюємо кути нахилу прямих
                                    //з яких складається дана ламана
                                    if (LineIntersectsCircle(from, middle, firstNeighbour))
                                        from = BiasPointF(from, middle, firstNeighbour);
                                    if (LineIntersectsCircle(to, middle, lastNeighbour))
                                    {
                                        to = BiasPointF(to, middle, lastNeighbour);
                                        changeEndPos = false;
                                    }
                                }

                                //Зображуємо ламану
                                if (changeEndPos)
                                    to = FindPointOnContour(middle, to);
                                drawPoints = new PointF[] { from, middle, to };
                                g.DrawLines(pen, drawPoints);
                            }
                        }
                    }
                }

                if (isUndirected) index++;
            }
        }

        /// <summary>
        /// Допоміжна функція, щоб дізнатися, чи ламана не перетинає вершину
        /// </summary>
        /// <param name="from">початкова точка</param>
        /// <param name="middle">середня точка</param>
        /// <param name="to">кінцева точка</param>
        /// <param name="center">координати вершини</param>
        /// <returns></returns>
        private static bool CheckCenterIntersect(PointF from, PointF middle, PointF to, PointF center)
        {
            return LineIntersectsCircle(from, middle, center) ||
                   LineIntersectsCircle(middle, to, center);
        }

        /// <summary>
        /// Знаходить точку на контурі кола, в якій пряма лінія,з координатою в центрі кола, перетинає коло
        /// </summary>
        /// <param name="from">координата початка лінії прямої</param>
        /// <param name="to">координата центра кола, до якої йде пряма</param>
        /// <returns></returns>
        private static PointF FindPointOnContour(PointF from, PointF to)
        {
            float width = to.X - from.X;
            float height = to.Y - from.Y;
            float dist = (float)Math.Sqrt(width * width + height * height);
            float coef = (dist - Constants.Radius) / dist;
            return new PointF(from.X + width * coef, from.Y + height * coef);
        }

        /// <summary>
        /// Зображує пряму лінію,відхилену від заданих точок на певний кут по колу, паралельну до найкоротшої відстані
        /// </summary>
        /// <param name="pen">екземпляр класу Pen</param>
        /// <param name="from">початкова незміщена точка</param>
        /// <param name="to">кінцева незміщена точка</param>
        /// <param name="g">екземпляр класу Graphics</param>
        private static void DrawLine(Pen pen, PointF from, PointF to, Graphics g)
        {
            int biasAngle = 30;
            PointF[] points = ParallelDisPlaceOnCircles(from, to, biasAngle);
            g.DrawLine(pen, points[0], points[1]);
        }

        /// <summary>
        /// Дає масив зі зміщеними точками так, щоб утворювали пряму, паралельну до найкоротшої відстані 
        /// </summary>
        /// <param name="first">початкова точка</param>
        /// <param name="second">кінцева точка</param>
        /// <param name="biasAngle">кут зміщення проти годинникової стрілки в градусах</param>
        /// <returns></returns>
        private static PointF[] ParallelDisPlaceOnCircles(PointF first, PointF second, int biasAngle)
        {
            float width = second.X - first.X;
            float height = second.Y - first.Y;
            double difAngle = (Math.PI * biasAngle / 180);

            double angle = Math.Atan2(height, width);
            angle -= difAngle;

            float biasY = (float)Math.Sin(angle) * Constants.Radius;
            float biasX = (float)Math.Cos(angle) * Constants.Radius;

            first.Y += biasY;
            first.X += biasX;

            angle += Math.PI + 2 * difAngle;
            biasY = (float)Math.Sin(angle) * Constants.Radius;
            biasX = (float)Math.Cos(angle) * Constants.Radius;

            second.X += biasX;
            second.Y += biasY;
            return new PointF[] { first, second };
        }

        /// <summary>
        /// Перевіряє, чи пряма, утворена початковою та кінцевою точками, перетинають коло
        /// </summary>
        /// <param name="start">початкова точка</param>
        /// <param name="end">кінцева точка</param>
        /// <param name="center">координати вершини кола</param>
        /// <returns></returns>
        private static bool LineIntersectsCircle(PointF start, PointF end, PointF center)
        {
            float width = end.X - start.X;
            float height = end.Y - start.Y;
            float diagonal = (float)Math.Sqrt(height * height + width * width);
            float val = (center.X - start.X) * height - (center.Y - start.Y) * width;
            float dist = Math.Abs(val) / diagonal;
            return dist <= Constants.Radius;
        }

        /// <summary>
        /// Перевіряє, чи точка знаходиться між іншими двома
        /// </summary>
        /// <param name="first">перша контрольна точка</param>
        /// <param name="second">друга контрольна точка</param>
        /// <param name="middle">точка, яку перевіряємо на розташування між іншими двома</param>
        /// <returns></returns>
        private static bool CheckMidPos(PointF first, PointF second, PointF middle)
        {
            if (first.X > second.X)
            {
                float a = first.X;
                first.X = second.X;
                second.X = a;
            }

            if (first.Y > second.Y)
            {
                float a = first.Y;
                first.Y = second.Y;
                second.Y = a;
            }

            bool isMiddleX = middle.X >= first.X && middle.X <= second.X;
            bool isMiddleY = middle.Y >= first.Y && middle.Y <= second.Y;
            return isMiddleX && isMiddleY;
        }

        /// <summary>
        /// Зміщує початкову точку так, щоб пряма лінія, яка виходить з цієї вершини, не перетинала коло, з вершиною
        /// </summary>
        /// <param name="from">початкова точка прямої</param>
        /// <param name="to">кінцева точка прямої</param>
        /// <param name="center">центр кола</param>
        /// <returns></returns>
        private static PointF BiasPointF(PointF from, PointF to, PointF center)
        {
            double mainAngle = Math.Atan2(to.Y - from.Y, to.X - from.X);
            double length = center.X - from.X;
            if (length == 0)
                length = center.Y - from.Y;
            double secondAngle = Math.Atan2(center.Y - from.Y, center.X - from.X);
            double biasAngle = Math.Asin(Constants.Radius / Math.Abs(length)) + 15 * Math.PI / 180;
            if (Math.Abs(mainAngle - secondAngle) > Math.PI)
            {
                if (mainAngle > secondAngle)
                {
                    secondAngle += 2 * Math.PI;
                }
                else
                {
                    mainAngle += 2 * Math.PI;
                }
            }

            mainAngle = secondAngle + (mainAngle > secondAngle ? biasAngle : -biasAngle);
            from.X = (float)(from.X + Math.Cos(mainAngle) * Constants.Radius);
            from.Y = (float)(from.Y + Math.Sin(mainAngle) * Constants.Radius);
            return from;
        }
    }
}