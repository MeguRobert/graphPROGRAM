using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static System.Math;
//Versiune: LAB
namespace graphPROGRAM
{
    public class Graph
    {
        public List<Vertice> vertices = new List<Vertice>();
        public List<Edge> edges = new List<Edge>();
        public string SPA { get; set; }
        public string SPL { get; set; }

        public void load(string file_name)
        {
            TextReader data_load = new StreamReader(@"..\..\" + file_name);
            int n = int.Parse(data_load.ReadLine());
            for (int i = 0; i < n; i++)
            {
                string data = data_load.ReadLine();
                string name = "";
                Point map_location;
                Color fill_color;
                int coordinatesStart = data.IndexOf("("), coordinatesEnd = data.IndexOf(")");
                int colorStart = data.IndexOf("["), colorEnd = data.IndexOf("]");
                int x = -1, y = -1;
                if (coordinatesStart > -1)
                {
                    name = data.Substring(0, coordinatesStart).Trim();
                    string coordinates = data.Substring(coordinatesStart + 1, coordinatesEnd - coordinatesStart - 1);
                    x = int.Parse(coordinates.Substring(0, coordinates.IndexOf(",")).Trim());
                    y = int.Parse(coordinates.Substring(coordinates.IndexOf(",") + 1).Trim());
                    map_location = new Point(x, y);
                }
                else
                {
                    map_location = GetValidRandomLocation();
                }
                if (colorStart > -1)
                {
                    string[] colors = data.Substring(colorStart + 1, colorEnd - colorStart - 1).Split(',');
                    int r = int.Parse(colors[0].Trim());
                    int g = int.Parse(colors[1].Trim());
                    int b = int.Parse(colors[2].Trim());
                    fill_color = Color.FromArgb(r, g, b);
                    if (coordinatesStart == -1)
                    {
                        name = data.Substring(0, colorStart);
                    }
                }
                else
                {
                    Random random = new Random();
                    int r = random.Next(256);
                    int g = random.Next(256);
                    int b = random.Next(256);
                    fill_color = Color.FromArgb(r, g, b);
                    if (coordinatesStart == -1)
                    {
                        name = data;
                    }
                }
                vertices.Add(new Vertice(name, map_location, fill_color));
            }


            string buffer;
            while ((buffer = data_load.ReadLine()) != null)
            {
                edges.Add(new Edge(buffer, vertices));
            }
        }

        public Point GetValidRandomLocation()
        {
            int x, y;
            Random random = new Random();
            bool isValid;
            Point p;
            do
            {
                x = random.Next(Engine.pictureBox.Width - Vertice.size - 3);
                y = random.Next(Engine.pictureBox.Height - Vertice.size - 3);
                p = new Point(x, y);
                isValid = true;
                foreach (Vertice vertice in vertices)
                {
                    if (GetDistance(vertice.location, p) < 3 * Vertice.size)
                    {
                        isValid = false;
                        break;
                    }
                }
            } while (!isValid);
            return p;
        }

        public double GetDistance(Point p1, Point p2)
        {
            return Sqrt(Pow(p1.X - p2.X, 2) + Pow(p1.Y - p2.Y, 2));
        }


        bool islink(int i, int j)
        {
            foreach (Edge edg in edges)
            {
                if (edg.begin_vertice == vertices[i] && edg.end_vertice == vertices[j])
                    return true;
                if (edg.begin_vertice == vertices[j] && edg.end_vertice == vertices[i])
                    return true;
            }
            return false;
        }
        bool islink(Vertice v1, Vertice v2)
        {
            foreach (Edge edg in edges)
            {
                if (edg.begin_vertice == v1 && edg.end_vertice == v2)
                    return true;
                if (edg.begin_vertice == v2 && edg.end_vertice == v1)
                    return true;
            }
            return false;
        }
        //greedy
        //pentru colorarea unei harti(graf)
        public void color()
        {

            //ordinea conteaza
            //sa orodoban prima data nodurile in functie de grad -> descrescator
            Color[] colors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Violet, Color.Sienna };
            int[] C = new int[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
                C[i] = -1;
            C[0] = 0;

            for (int i = 1; i < vertices.Count; i++)
            {
                bool[] colors_idx = new bool[vertices.Count];
                for (int j = 0; j < vertices.Count; j++)
                    if (i != j && C[j] != -1 && islink(i, j))
                    {
                        colors_idx[C[j]] = true;
                    }
                int idx = 0;
                while (colors_idx[idx] == true)
                    idx++;
                C[i] = idx;
            }
            for (int i = 0; i < vertices.Count; i++)
                vertices[i].color = colors[C[i]];
        }

        public int[,] GenerateMatrix()
        {
            int[,] matrix = new int[vertices.Count, vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    matrix[i, j] = islink(i, j) ? 1 : 0;
                }
            }
            return matrix;
        }

        public void ParcurgereInAdancime(Vertice vertice)
        {
            SPA += " " + vertice.name;
            vertice.visited = true;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (!vertices[i].visited && islink(vertices[i], vertice))
                {
                    ParcurgereInAdancime(vertices[i]);
                }
            }
        }

        public void ParcurgereInLatime(Vertice vertice)
        {
            Queue<Vertice> queue = new Queue<Vertice>();
            queue.Enqueue(vertice);
            vertice.visited = true;
            while (queue.Count > 0)
            {
                vertice = queue.Dequeue();
                SPL += " " + vertice.name;
                for (int i = 0; i < vertices.Count; i++)
                {
                    if (!vertices[i].visited && islink(vertice, vertices[i]))
                    {
                        vertices[i].visited = true;
                        queue.Enqueue(vertices[i]);
                    }
                }

            }

        }


        public void draw(Graphics handler)
        {

            foreach (Edge e in edges)
                e.draw(handler);
            foreach (Vertice v in vertices)
                v.draw(handler);
        }
    }
}
