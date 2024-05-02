using Drawing.classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Drawing
{
    public partial class Form4 : Form
    {
        public Form4(WeightedGraph graph)
        {
            InitializeComponent();
            g = CreateGraphics();
            this.graph = new WeightedGraph(graph.Matrix, graph.WeightedMatrix, Width, panel1.Top);
        }

        private Graphics g;
        private WeightedGraph graph;
        private bool continueDrawing = false;

        private void button2_Click(object sender, EventArgs e)
        {
            continueDrawing = true;
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

        private void button1_Click(object sender, EventArgs e)
        {
            continueDrawing = false;
            g.Clear(Color.White);
            GraphDrawing.DrawAllNodes(graph, g);
            EdgeLinkedList edges = GraphCharacteristics.GetMinSkeleton(graph);


            WriteMatrix(graph.WeightedMatrix);
            edges.WriteAll();
            Console.WriteLine("\r\n-------------------------------------------------");
            Console.WriteLine($"Min weight: {edges.TotalWeight()}");


            Node<Edge> node = edges.GetFirst();
            Pen pen = new Pen(Brushes.Black, 1);
            Brush brush;
            while (node != null)
            {
                brush = GraphDrawing.GetRandomColor();
                pen.Brush = brush;
                WaitForNextStep();
                GraphDrawing.DrawWeightedConnection(graph, node.value.Vertex1, node.value.Vertex2, g, pen);
                node = node.next;
            }
        }

        private void WriteMatrix(int[,] matrix)
        {
            string str = string.Empty;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                str += "\r\n";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    str += $"{matrix[i, j]}\t";
                }

                
            }

            Console.WriteLine(str);
        }
    }
}