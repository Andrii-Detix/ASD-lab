using Drawing.classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Drawing
{
    public partial class Form3 : Form
    {
        public Form3(DirectedGraph graph)
        {
            InitializeComponent();
            g = CreateGraphics();
            this.graph = new DirectedGraph(graph.Matrix, Width*0.6f, panel1.Top);
            
        }
        bool continueDrawing= false;
        private Graphics g;
        private DirectedGraph graph;
        private void BFS(DirectedGraph graph, Graphics g)
        {
            GraphDrawing.DrawAllNodes(graph, g);
            int[] nodeInfo = new int[graph.Length];
            Queue<int> openNodes = new Queue<int>();
            Brush brush = GraphDrawing.GetRandomColor();
            Pen pen = new Pen(brush, 1);
            pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 4);
            for (int i = 0; i < graph.Length; i++)
            {
                if (nodeInfo[i] == 0)
                {
                    for (int j = 0; j < graph.Length; j++)
                    {
                        if (nodeInfo[j] == 0 && graph.Matrix[i, j] == 1 && i != j)
                        {
                            openNodes.Enqueue(i);
                            nodeInfo[i] = 1;
                            break;
                        }
                    }
                }
                while (openNodes.Count != 0)
                {
                    int node = openNodes.Peek();
                    GraphDrawing.DrawNode(graph, node, g, Brushes.Brown);
                    for (int j = 0; j < graph.Length; j++)
                    {
                        if (nodeInfo[j] == 1 || j == node || graph.Matrix[node, j] == 0)
                            continue;

                        nodeInfo[j] = 1;
                        openNodes.Enqueue(j);
                        WaitForNextStep();
                        GraphDrawing.DrawConnection(graph, node, j, g, pen);
                        GraphDrawing.DrawNode(graph, j, g, Brushes.Blue);
                        
                        
                    }
                    WaitForNextStep();
                    GraphDrawing.DrawNode(graph, node, g, Brushes.Indigo);
                    openNodes.Dequeue();
                    brush = GraphDrawing.GetRandomColor();
                    pen.Brush = brush;
                }

            }
            DrawRemainingVertexes(graph, nodeInfo, g);
            
        }
        private void DFS(DirectedGraph graph, Graphics g)
        {
            GraphDrawing.DrawAllNodes(graph, g);
            int[] nodeInfo = new int[graph.Length];
            Pen pen = new Pen(Brushes.Black, 1);
            pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 4);
            for (int i = 0; i < graph.Length; i++)
            {
                if (nodeInfo[i] == 0)
                {
                    for (int j = 0; j < graph.Length; j++)
                    {
                        if (nodeInfo[j] == 0 && graph.Matrix[i, j] == 1 && i != j)
                        {
                            nodeInfo[i] = 1;
                            DepthSearch(graph, i, nodeInfo, g, pen);
                            break;
                        }
                    }
                }
            }
            DrawRemainingVertexes(graph, nodeInfo, g);
        }

        private void DepthSearch(DirectedGraph graph, int node, int[] nodeInfo, Graphics g, Pen pen)
        {
            GraphDrawing.DrawNode(graph, node, g, Brushes.Brown);
            Brush brush = GraphDrawing.GetRandomColor();
            pen.Brush = brush;
            for (int i = 0; i < graph.Length; i++)
            {
                if(nodeInfo[i] == 1 || i == node || graph.Matrix[node, i] == 0)
                    continue;
                nodeInfo[i] = 1;
                WaitForNextStep();
                GraphDrawing.DrawConnection(graph,node,i,g,pen);
                GraphDrawing.DrawNode(graph,i,g,Brushes.Blue);
                WaitForNextStep();
                GraphDrawing.DrawNode(graph, node, g, Brushes.Blue);
                DepthSearch(graph, i, nodeInfo, g, pen);
                GraphDrawing.DrawNode(graph, node, g, Brushes.Brown);
                pen.Brush = brush;
                
            }
            WaitForNextStep();
            GraphDrawing.DrawNode(graph, node, g, Brushes.Indigo);
        }

        private void DrawRemainingVertexes(DirectedGraph graph, int[] nodeInfo, Graphics g)
        {
            for(int i = 0; i<graph.Length;i++)
            {
                
                if (nodeInfo[i] == 0)
                {
                    WaitForNextStep();
                    GraphDrawing.DrawNode(graph, i, g, Brushes.Brown);
                    WaitForNextStep();
                    GraphDrawing.DrawNode(graph, i, g, Brushes.Indigo);
                    nodeInfo[i] = 1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            continueDrawing = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            continueDrawing = false;
            int[,] searchMatrix = null;
            int[] offsetVertices = null;
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    (searchMatrix, offsetVertices)  = GraphCharacteristics.BFS(graph);
                    BFS(graph, g);
                    break;
                case 1 :
                    DFS(graph, g);
                    (searchMatrix, offsetVertices) = GraphCharacteristics.DFS(graph);
                    break;
            }

            string vector = "Vector: ";
            for (int i = 1; i <= graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (offsetVertices[j] == i)
                    {
                        vector += $"{j + 1}";
                        break;
                    }
                }
                if(i != graph.Length)
                    vector += " -> ";
            }

            Console.WriteLine(vector);
            string str = string.Empty;
            for (int i = 0; i < graph.Length; i++)
            {
                str += $"New number of vertex {i + 1} is {offsetVertices[i]}\r\n";
            }
            

            Console.WriteLine(str);
            GraphDrawing.DrawMatrix(searchMatrix,g,"Пошуку",new PointF(0.6f*Width,0),new PointF(Width,panel1.Top));
        }

        private void WaitForNextStep()
        {
            while (!continueDrawing)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
            continueDrawing = false;
        }
    }
}
