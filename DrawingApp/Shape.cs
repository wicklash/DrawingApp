using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DrawingApp
{
    internal abstract class Shape
    {
        public int x {  get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Color color { get; set; }
        public bool isSelected { get; set; }  
        public Type ShapeType 
        {
            get { return this.GetType(); }
        }
        public  abstract void Draw(Graphics g);
        public abstract bool IsHit(int mouseX,int mouseY);

        protected virtual void Shadow(Graphics g)
        {
            if (this.isSelected)
            {
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, Color.Black)))
                {
                    
                    g.FillRectangle(shadowBrush, x-5 , y-5 , width+10, height+10);
                }
            }
        }
    }
}
