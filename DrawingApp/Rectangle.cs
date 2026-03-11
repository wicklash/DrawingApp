using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp
{
    internal class Rectangle : Shape
    {
        public override void Draw(Graphics g)
        {
            Shadow(g);
            using (Brush fillBrush = new SolidBrush(this.color))
            {
                g.FillRectangle(fillBrush, x, y, width, height);
            }
        }
        public override bool IsHit(int mouseX,int mouseY)
        {
            return mouseX >= x && mouseX <= (x + width) &&
               mouseY >= y && mouseY <= (y + height);
        }
    }
}
