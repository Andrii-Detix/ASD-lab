using System;
using System.Drawing;
using System.Windows.Forms;
using Drawing.classes;

namespace Drawing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        Graphics g;
        DirectedGraph graph;

        private void textBox1_Start(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            double[] coefs = new[] { 1 - 0.05, 0, 0, -0.01, -0.005 };
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    graph = new UndirectedGraph(int.Parse(textBox1.Text), coefs, 0.6f * Width, panel1.Top);
                    break;
                case 1:
                    graph = new DirectedGraph(int.Parse(textBox1.Text), coefs, 0.6f * Width, panel1.Top);
                    break;
                case 2:
                    graph = new WeightedGraph(int.Parse(textBox1.Text), coefs, 0.6f * Width, panel1.Top);
                    break;
            }


            GraphDrawing.DrawMatrix(graph.Matrix, g, "Суміжності", new PointF(0.6f * Width, 0),
                new PointF(Width, panel1.Top));
            GraphDrawing.DrawGraph(graph, g);
            Form2 form2 = new Form2(graph);
            form2.Show();
            Form3 form3 = new Form3(graph);
            form3.Show();
            if (graph is WeightedGraph weightedGraph)
            {
                Form4 form4 = new Form4(weightedGraph);
                form4.Show();
            }
        }
    }
}
