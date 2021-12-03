using System.Drawing;

namespace graphPROGRAM
{
    public class Vertice
    {
        public static int size = 17;
        public Point location;
        public Color color;
        public string name;
        public bool visited;

        public Vertice(string name, Point location, Color color)
        {
            this.name = name;
            this.location = location;
            this.color = color;
        }


        public void draw(Graphics graphics)
        {
            graphics.FillEllipse(new SolidBrush(color), location.X - size, location.Y - size, 2 * size + 1, 2 * size + 1);
            graphics.DrawEllipse(new Pen(Color.Black), location.X - size, location.Y - size, 2 * size + 1, 2 * size + 1);
            graphics.DrawString(name, new Font("Comic Sans MS", 10, FontStyle.Italic), new SolidBrush(Color.Black), location.X - size / 2, location.Y - size / 2);
        }
    }
}
