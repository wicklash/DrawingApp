using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp
{
    internal class Circle:Shape
    {
        public override void Draw(Graphics g)
        {

            Shadow(g);
            using (Brush fillBrush = new SolidBrush(this.color))
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)x, (int)y, width, height);
                g.FillEllipse(fillBrush, rect);
            }
        }
        public override bool IsHit(int mouseX, int mouseY)
        {
            int radius = width;
            double centerX = x + (radius / 2.0);
            double centerY = y + (radius / 2.0);
            double r = radius / 2.0;
            double dx = mouseX - centerX;
            double dy = mouseY - centerY;
            double distanceOfMause = (dx * dx) + (dy * dy);
            return distanceOfMause <= (r * r);
        }
    }
}
