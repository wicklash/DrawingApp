using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingApp
{

    public partial class Form1 : Form
    {
        enum EditorMode { None, Drawing, Selecting }
        List<Shape> shapes = new List<Shape>();
        Shape selectedShapeType=null;
        Shape currentShape;
        Shape selectedShape = null; 
        Point startPoint;
        Color selectedColor = Color.Black;
        EditorMode currentMode=EditorMode.None;
        Point lastMousePos;
        List<Button> shapeButtons;
        List<Button> colorButtons;

        public Form1()
        {
            InitializeComponent();
            shapeButtons = new List<Button> { btnRectangle, btnCircle, btnTriangle, btnHexagon };
            colorButtons = new List<Button> { btnRed, btnBlue, btnGreen, btnOrange, btnBlack, btnYellow, btnPurple, btnBrown, btnWhite };
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (Shape shape in shapes)  
            {
                shape.Draw(e.Graphics);
            }

            if (currentShape != null)
            {
                currentShape.Draw(e.Graphics);
            }
        }

        private void drawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentMode==EditorMode.Drawing)
            { 
                startPoint = e.Location;
                currentShape = (Shape)Activator.CreateInstance(selectedShapeType.GetType());
                currentShape.x = e.X;
                currentShape.y= e.Y;
                currentShape.color = selectedColor;
            }
            if(currentMode==EditorMode.Selecting)
            {
                bool found = false;
                for (int i = shapes.Count - 1; i >= 0; i--)
                {
                    shapes[i].isSelected = false; 

                    if (!found && shapes[i].IsHit(e.X, e.Y))
                    {
                        shapes[i].isSelected = true;
                        found = true;
                        selectedShape = shapes[i]; 
                        lastMousePos = e.Location;
                        SelectedShapeAtributes();
                    }
                }
                drawingPanel.Invalidate();
            }
        }

        private void drawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            //Drawing 
            if (currentMode==EditorMode.Drawing && currentShape != null )
            {
                //Noktanın  panel içinde olması
                int clampedX = Math.Max(0, Math.Min(e.X, drawingPanel.Width));
                int clampedY = Math.Max(0, Math.Min(e.Y, drawingPanel.Height));
                //Noktanın ilk noktadan uzaklığı
                int radiusX = Math.Abs(clampedX - startPoint.X);
                int radiusY = Math.Abs(clampedY - startPoint.Y);
                //Nokta Hangi tarafa daha yakın
                int maxRadiusX = Math.Min(startPoint.X, drawingPanel.Width - startPoint.X);
                int maxRadiusY = Math.Min(startPoint.Y, drawingPanel.Height - startPoint.Y);
                //
                radiusX = Math.Min(radiusX, maxRadiusX);
                radiusY = Math.Min(radiusY, maxRadiusY);

                if (currentShape is Rectangle)
                {
                    currentShape.width=radiusX*2;
                    currentShape.height=radiusY*2;
                    currentShape.x = startPoint.X - radiusX;
                    currentShape.y = startPoint.Y - radiusY;
                }
                else if (currentShape is Circle)
                {
                    int radius = Math.Min(radiusX, radiusY);
                    currentShape.width = radius * 2;
                    currentShape.height = radius * 2;
                    currentShape.x = startPoint.X - radius;
                    currentShape.y = startPoint.Y - radius;
                }

                else if (currentShape is Triangle)
                {
                    int baseOfTriangle = radiusX*2; 
                    int heightOfTriangle = (int)(baseOfTriangle * 0.866);

                    currentShape.width = baseOfTriangle;
                    currentShape.height = heightOfTriangle;
                    currentShape.x = startPoint.X - (baseOfTriangle / 2);
                    currentShape.y = startPoint.Y - (heightOfTriangle / 2); 
                }

                else if (currentShape is Hexagon)
                {
                    int horizontalRadius = radiusX;
                    int verticalRadius = (int)(horizontalRadius * 0.866);
                    currentShape.width = horizontalRadius * 2;
                    currentShape.height = verticalRadius * 2;
                    currentShape.x = startPoint.X - horizontalRadius;
                    currentShape.y = startPoint.Y - verticalRadius;
                }

                drawingPanel.Invalidate();
            }
            //Dragging
            if (currentMode == EditorMode.Selecting && selectedShape != null && e.Button == MouseButtons.Left)
            {

                int dX = e.X - lastMousePos.X;
                int dY = e.Y - lastMousePos.Y;
                int newX = selectedShape.x + dX;
                int newY = selectedShape.y + dY;

                if (newX < 0)
                {
                    newX = 0;
                }
                if (newY < 0)
                {
                    newY = 0;
                } 
                if (newX + selectedShape.width > drawingPanel.Width)
                {
                    newX = drawingPanel.Width - selectedShape.width;
                }
                if (newY + selectedShape.height > drawingPanel.Height)
                {
                    newY = drawingPanel.Height - selectedShape.height;
                }
                selectedShape.x = newX;
                selectedShape.y = newY;
                lastMousePos = e.Location;
                drawingPanel.Invalidate();
            }
        }

        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentMode==EditorMode.Drawing && currentShape != null)
            {

                if (currentShape.x + currentShape.width <= drawingPanel.Width &&
                    currentShape.y + currentShape.height <= drawingPanel.Height &&
                    currentShape.x >= 0 && currentShape.y >= 0)
                {
                    shapes.Add(currentShape); 
                }

                currentShape = null;
                drawingPanel.Invalidate();
            }
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            selectedShapeType = new Rectangle();
            currentMode = EditorMode.Drawing;
            HighlightButton(btnRectangle, shapeButtons);
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            selectedShapeType = new Circle();
            currentMode = EditorMode.Drawing;
            HighlightButton(btnCircle, shapeButtons);
        }

        private void btnTriangle_Click(object sender, EventArgs e)
        {
            selectedShapeType = new Triangle();
            currentMode = EditorMode.Drawing;
            HighlightButton(btnTriangle, shapeButtons);
        }

        private void btnHexagon_Click(object sender, EventArgs e)
        {
            selectedShapeType = new Hexagon();
            currentMode = EditorMode.Drawing;
            HighlightButton(btnHexagon, shapeButtons);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            currentMode = EditorMode.Selecting;

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                if (shapes[i].isSelected == true)
                {
                    shapes.RemoveAt(i);
                }
            }
            currentMode=EditorMode.None;
            drawingPanel.Invalidate();
        }
        private void btnDeleteBoard_Click(object sender, EventArgs e)
        {
            currentMode=EditorMode.None; 
            shapes.Clear();
            drawingPanel.Invalidate();
        }
        private void selectColorEvent(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn==null)
            {
                return;
            }
            selectedColor = btn.BackColor;
            if (selectedShape != null && currentMode==EditorMode.Selecting)
            {
                selectedShape.color = selectedColor;
                drawingPanel.Invalidate();
            }
            HighlightButton(btn, colorButtons);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog saveFileDialog = new OpenFileDialog ();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                shapes = FileManager.LoadPanel(saveFileDialog.FileName);
                drawingPanel.Invalidate();
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                FileManager.SavePanel(shapes,saveFileDialog.FileName);
            }
        }
        private void HighlightButton(Button targetButton, List<Button> group)
        {

            foreach (var btn in group)
            {
                btn.FlatAppearance.BorderSize = 0;
            }
            targetButton.FlatAppearance.BorderSize = 2;
            targetButton.FlatAppearance.BorderColor = Color.SkyBlue;
        }
        
        private void SelectedShapeAtributes()
        {
            if (selectedShape!=null)
            {
                string shapeTypeName = selectedShape.GetType().Name;
                foreach (Button btn in shapeButtons)
                {
                    if (btn.Name.Contains(shapeTypeName))
                    {
                         HighlightButton(btn, shapeButtons); 
                         break;
                    }
                }
            }
            string shapeColorName = selectedShape.color.Name;
            foreach (var btn in colorButtons)
            {
                if (btn.Name.Contains(shapeColorName))
                {
                    HighlightButton(btn, colorButtons);
                    break;
                }
            }
        }

    }
}
