using System;
using System.Windows.Forms;

namespace graphPROGRAM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Graph g;

        private void Form1_Load(object sender, EventArgs e)
        {
            Engine.init_graph(pictureBox1);

            g = new Graph();
            g.load("graph_demo.txt");
            g.draw(Engine.graphics);
            int[,] matrix = g.GenerateMatrix();
            for (int i = 0; i < g.vertices.Count; i++)
            {
                string s = "";
                for (int j = 0; j < g.vertices.Count; j++)
                {
                    s += " " + matrix[i, j];

                }
                listBox1.Items.Add(s);
            }

            foreach (var v in g.vertices)
            {
                v.visited = false;
            }

            g.ParcurgereInAdancime(g.vertices[0]);

            listBox1.Items.Add("----------");
            listBox1.Items.Add(g.SPA);

            foreach (var v in g.vertices)
            {
                v.visited = false;
            }

            g.ParcurgereInLatime(g.vertices[0]);

            listBox1.Items.Add("----------");
            listBox1.Items.Add(g.SPL);

            Engine.refresh();

        }


    }
}
