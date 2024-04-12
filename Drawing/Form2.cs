using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing.classes;

namespace Drawing
{
    public partial class Form2 : Form
    {
        public Form2(DirectedGraph graph)
        {
            InitializeComponent();
            g = CreateGraphics();
            mainInfo = GraphCharacteristics.ShowMainInfo(graph);
            if (graph.IsDirected)
            {
                maxPageIndex = 4;
                ways = GraphCharacteristics.ShowWays(graph, 2) + "\r\n" + GraphCharacteristics.ShowWays(graph, 3);
                connectComponents = GraphCharacteristics.ShowConnectComponents(graph);
                connectMatrix = GraphCharacteristics.GetStrongConnectMatrix(graph);
                reachMatrix = GraphCharacteristics.GetReachabilityMatrix(graph);
                int[,] condensMatrix = GraphCharacteristics.GetCondensMatrix(graph);
                condensGraph = new DirectedGraph(condensMatrix, 0.6f * Width, panel1.Top);
            }
            else
            {
                maxPageIndex = 0;
            }
            textBox1.Text = mainInfo;
        }
        Graphics g;
        private readonly int maxPageIndex;
        private int count = 0;
        private string mainInfo;
        private string ways;
        private string connectComponents;
        int[,] reachMatrix;
        int[,] connectMatrix;
        DirectedGraph condensGraph;

        private void button1_Click(object sender, EventArgs e)
        {
            count = HelpMethods.CheckIndexLim(count + 1, maxPageIndex);

            switch (count)
            {
                case 0:
                    g.Clear(Color.White);
                    textBox1.Visible = true;
                    textBox1.Text = mainInfo;
                    break;
                case 1:
                    g.Clear(Color.White);
                    textBox1.Visible = true;
                    textBox1.Text = ways;
                    break;
                case 2:
                    g.Clear(Color.White);
                    textBox1.Visible = false;
                    GraphDrawing.DrawMatrix(reachMatrix, g, "Досяжності", new PointF(0, 0), new PointF(0.4f * Width, panel1.Top));
                    GraphDrawing.DrawMatrix(connectMatrix, g, "Сильн. зв'язності", new PointF(0.5f * Width, 0), new PointF(0.9f*Width, panel1.Top));
                    break;
                case 3:
                    g.Clear(Color.White);
                    textBox1.Visible = true;
                    textBox1.Text = connectComponents;
                    break;
                case 4:
                    g.Clear(Color.White);
                    textBox1.Visible = false;
                    GraphDrawing.DrawGraph(condensGraph, g);
                    GraphDrawing.DrawMatrix(condensGraph.Matrix,g, "Конденсації", new PointF(0.6f*Width,0), new PointF(0.9f*Width,panel1.Top));
                    break;
            }
        }
    }
}
