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
        }

        Graphics g;
        DirectedGraph graph;

        private void textBox1_Start(object sender, EventArgs e)
        {
            g = CreateGraphics();
            g.Clear(Color.White);
            bool isUndirected = false;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    graph = new UndirectedGraph(int.Parse(textBox1.Text), 0.6f * Width, panel1.Top);
                    isUndirected = true;
                    break;
                case 1:
                    graph = new DirectedGraph(int.Parse(textBox1.Text), 0.6f * Width, panel1.Top);
                    break;
            }

            GraphDrawing.DrawMatrix(graph, g, new PointF(0.6f * Width, 0), new PointF(Width, panel1.Top));
            GraphDrawing.DrawGraph(graph, g, isUndirected);
        }
    }
}