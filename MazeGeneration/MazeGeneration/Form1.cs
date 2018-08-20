using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeGeneration
{
    public partial class Form1 : Form
    {
        protected int c_width;
        protected static int cols, rows;
        protected Spot[,] grid;
        protected Stack<Spot> stack = new Stack<Spot>();

        protected Spot currentSpot;
        protected Spot next;

        Graphics g;
        Pen blackPen;
        Pen whitePen;
        SolidBrush blackBrush;

        protected int inputInt; //input to set cols and rows
        //protected bool canDrawGrid; 

        public Form1()
        {
            InitializeComponent();

            g = Canvas.CreateGraphics();
            blackPen = new Pen(Color.Black, 2);
            whitePen = new Pen(Color.White, 5);
            blackBrush = new SolidBrush(Color.Black);
        }

        public class Spot : Form1
        {
            public int i;
            public int j;
            public bool[] walls = { true, true, true, true };
            public List<Spot> neighbors;
            public bool visited = false;

            public Spot(int i, int j)
            {
                this.i = i;
                this.j = j;
            }
            public Spot CheckNeighbors(Spot[,] grid)
            {
                int i = this.i;
                int j = this.j;

                neighbors = new List<Spot>();

                Spot right, up, down, left;
                if (i < cols - 1)
                {
                    //right adjacent spot
                    right = grid[i + 1, j];
                    if (right != null && !right.visited)
                    {
                        neighbors.Add(right);
                    }
                }
                if (i > 0)
                {
                    left = grid[i - 1, j];
                    //left adjacent spot
                    if (left != null && !left.visited)
                    {
                        neighbors.Add(left);
                    }
                }
                if (j < rows - 1)
                {
                    down = grid[i, j + 1];

                    //down adjacent spot
                    if (down != null && !down.visited)
                    {
                        neighbors.Add(down);
                    }
                }
                if (j > 0)
                {
                    up = grid[i, j - 1];
                    //up adjacent spot
                    if (up != null && !up.visited)
                    {
                        neighbors.Add(up);
                    }
                }
                
                if(neighbors.Count > 0)
                {
                    RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
                    var r = RandomInteger(Rand, 0, neighbors.Count);
                    return neighbors[r];
                }
                else
                {
                    return null;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private int RandomInteger(RNGCryptoServiceProvider Rand, int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                Rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }

        private void RemoveWalls(Spot a, Spot b)
        {
            var x = a.i - b.i;
            if (x == 1)
            {
                a.walls[3] = false;
                b.walls[1] = false;
            }

            if (x == -1)
            {
                a.walls[1] = false;
                b.walls[3] = false;
            }
            var y = a.j - b.j;
            if (y == 1)
            {
                a.walls[0] = false;
                b.walls[2] = false;
            }

            if (y == -1)
            {
                a.walls[2] = false;
                b.walls[0] = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out inputInt))
            {
                if (inputInt == 0)
                {
                    string message = "Cannot use 0 as integer. Please Enter another Integer number.";
                    string caption = "Error Detected in Input";

                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    result = MessageBox.Show(message, caption, buttons);

                    //if (result == DialogResult.OK)
                    //{

                    //}
                }
                else
                {
                    cols = inputInt;
                    rows = inputInt;

                    c_width = Canvas.Width / cols;

                    grid = new Spot[cols, rows];

                    //building grid
                    for (int i = 0; i < cols; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            grid[i, j] = new Spot(i, j);
                        }
                    }

                    currentSpot = grid[0, 0];

                    while (true)
                    {
                        currentSpot.visited = true;

                        //Using recursive backtracker algorithm
                        //https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker

                        //step 1
                        next = currentSpot.CheckNeighbors(grid);

                        if (next != null)
                        {
                            next.visited = true;

                            //step 2
                            stack.Push(currentSpot);
                            //Console.WriteLine($"stack count is {stack.Count}");

                            //step 3
                            RemoveWalls(currentSpot, next);

                            //step 4
                            currentSpot = next;
                            //Console.WriteLine($"currentSpot is {currentSpot.i} and {currentSpot.j}");
                        }
                        else if (stack.Count > 0)
                        {
                            currentSpot = stack.Pop();
                            //Console.WriteLine($"pop a stack and count is {stack.Count}");
                            //Console.WriteLine($"currentSpot after pop is {currentSpot.i} and {currentSpot.j}");

                        }
                        else if (next == null)
                        {
                            break;
                        }

                        //Call DrawGrid here (inside while loop) when we want to see how it works step by step. Really slow down the program
                        //because we used 2d array and how each element is accessed in 2d array(?) and how WF implements draw graphics.
                        //DrawGrid();
                    }

                    //Call DrawGrid outside while loop when the maze has already been generated. Really fast visualization
                    DrawGrid();
                }             
            }
        }

        private void DrawGrid()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int x = i * c_width;
                    int y = j * c_width;

                    if (grid[i, j].walls[0])
                    {
                        g.DrawLine(whitePen, x, y, x + c_width, y);
                    }
                    if (grid[i, j].walls[1])
                    {

                        g.DrawLine(whitePen, x + c_width, y, x + c_width, y + c_width);
                    }
                    if (grid[i, j].walls[2])
                    {

                        g.DrawLine(whitePen, x + c_width, y + c_width, x, y + c_width);
                    }
                    if (grid[i, j].walls[3])
                    {

                        g.DrawLine(whitePen, x, y + c_width, x, y);
                    }

                    if (grid[i, j].visited)
                    {
                        Rectangle Rect = new Rectangle(x, y, c_width, c_width);

                        g.FillRectangle(blackBrush, Rect);
                    }
                }
            }
        }
        
        #region extra
        //suppressing the alt button causing the form to be redrawn
        protected override void WndProc(ref Message m)
        {
            // Suppress the WM_UPDATEUISTATE message
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }
        #endregion
    }
}
