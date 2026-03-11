using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp
{
    internal static class FileManager
    {
        public static void SavePanel(List<Shape> shapes, string filePath)
        {
            List <string> lines = new List<string>();
            foreach (var shape in shapes)
            {
                string line = $"{shape.GetType().Name}|{shape.x}|{shape.y}|{shape.width}|{shape.height}|{shape.color.ToArgb()}";
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines);
        }
        public static List<Shape> LoadPanel(string filePath)
        {
            List<Shape> shapes = new List<Shape>();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length < 6) continue;

                Shape shape = CreateShape(parts[0]);
                if (shape != null)
                {
                    shape.x = int.Parse(parts[1]);
                    shape.y = int.Parse(parts[2]);
                    shape.width = int.Parse(parts[3]);
                    shape.height = int.Parse(parts[4]);
                    shape.color = Color.FromArgb(int.Parse(parts[5]));
                    shapes.Add(shape);
                }
            }
            return shapes;
        }

        private static Shape CreateShape(string typeName)
        {
            switch (typeName)
            {
                case "Rectangle": return new Rectangle();
                case "Circle": return new Circle();
                case "Triangle": return new Triangle();
                case "Hexagon": return new Hexagon();
                default: return null;
            }
        }
    }
}
