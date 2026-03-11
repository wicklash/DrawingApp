using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp
{
    internal class Hexagon:Shape
    {
        private Point[] points = new Point[6];

        private void CreatePoints()
        {
            int a = width / 4;           
            int aSqRoot3 = height / 2;      

            points = new Point[]
            {
                new Point(x, y + aSqRoot3),                     // Sol orta köşe
                new Point(x + a, y),                         // Üst sol köşe
                new Point(x + (3 * a), y),                   // Üst sağ köşe
                new Point(x + width, y + aSqRoot3),             // Sağ orta köşe
                new Point(x + (3 * a), y + height),          // Alt sağ köşe
                new Point(x + a, y + height)                 
            };
        }
        private float AreaOfTriangle(Point p1, Point p2, Point p3)
        {
            return Math.Abs(p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y)) / 2.0f;
        }

        private bool IsPointInTriangle(Point p, Point p1, Point p2, Point p3)
        {
            float mainArea = AreaOfTriangle(p1, p2, p3);
            float a1 = AreaOfTriangle(p, p1, p2);
            float a2 = AreaOfTriangle(p, p2, p3);
            float a3 = AreaOfTriangle(p, p3, p1);
            return Math.Abs(mainArea - (a1 + a2 + a3)) < 1.0f;
        }
        public override void Draw(Graphics g)
        {
            CreatePoints();
            Shadow(g);
 
            using (SolidBrush brush = new SolidBrush(this.color))
            {
                g.FillPolygon(brush, points);
            }

        }
        public override bool IsHit(int mouseX, int mouseY)
        {
            CreatePoints();
            int centerX = x + width / 2;
            int centerY = y + height / 2;

            Point center = new Point(centerX, centerY);
            Point mousePos = new Point(mouseX, mouseY);

            for (int i = 0; i < 6; i++)
            {
                if (IsPointInTriangle(mousePos, center, points[i], points[(i + 1) % 6]))
                {
                    return true;
                }
            }
            return false;
        }
        protected override void Shadow(Graphics g)
        {
            if (this.isSelected)
            {
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, Color.Black)))
                {
                    g.FillRectangle(shadowBrush, x - 1 , y-1 , width+2 , height+2 );
                }
            }
        }
    }
}
