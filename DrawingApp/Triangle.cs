using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp
{
    internal  class Triangle:Shape
    {
        private Point[] points=new Point[3]; 
        private void CreatePoints()
        {
            points[0] = new Point(x + (width / 2), y);
            points[1] = new Point(x, y + height);
            points[2] = new Point(x + width, y + height);
        }
        private float AreaOfTriangle(Point p1, Point p2, Point p3)
        {
            return Math.Abs(p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y)) / 2.0f;
        }
        private bool IsPointInTriangle(Point p, Point p1, Point p2, Point p3)
        {
            float mainArea = AreaOfTriangle(p1, p2, p3);
            float area1 = AreaOfTriangle(p, p1, p2);
            float area2 = AreaOfTriangle(p, p2, p3);
            float area3 = AreaOfTriangle(p, p3, p1);

            return Math.Abs(mainArea - (area1 + area2 + area3)) < 1.0f;
        }
        public override void Draw(Graphics g)
        {
            CreatePoints();
            Shadow(g);
            using (SolidBrush brush=new SolidBrush(this.color))
            {
                g.FillPolygon(brush,points);
            }
        }
        public override bool IsHit(int mouseX, int mouseY)
        {
            CreatePoints();
            Point mouse = new Point(mouseX, mouseY);
            return IsPointInTriangle(mouse, points[0], points[1], points[2]);
        }
        protected override void Shadow(Graphics g)
        {
            if (this.isSelected)
            {
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, Color.Black)))
                {
                    g.FillRectangle(shadowBrush, x , y , width , height );
                }
            }
        }
    }
}
