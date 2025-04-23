using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {
        private Grid current;
        private Grid next;
        private Timer timer;
        private const int cols = 50;
        private const int rows = 25;
        private const int cellSize = 20;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Game of Life by Noah Hathout";
            this.Size = new Size(1033, 640);
            this.StartPosition = FormStartPosition.CenterScreen;

            current = new Grid(cols, rows);
            next = new Grid(cols, rows);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;
            
            button1.Size = new System.Drawing.Size(95, 23);
            button1.Text = "Next Generation";
            button2.Text = "Clear";
            button3.Text = "Start";
            button4.Text = "Stop";

            groupBox1.Text = "Timer";

            button3.Enabled = true;
            button4.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;

            this.DoubleBuffered = true;
        }

        public class Grid
        {
            private bool[,] grid;
            private int cols;
            private int rows;

            public Grid(int cols, int rows)
            {
                this.cols = cols;
                this.rows = rows;
                grid = new bool[cols, rows];
            }

            public int Rows
            {
                get
                {
                    return rows;
                }
            }

            public int Cols
            {
                get
                {
                    return cols;
                }
            }

            public bool this[int x, int y]
            {
                get
                {
                    if (x < 0 || y < 0 || x >= cols || y >= rows) return false;
                    return grid[x, y];
                }
                set
                {
                    if (x < 0 || y < 0 || x >= cols || y >= rows) return;
                    grid[x, y] = value;
                }
            }

            public void Clear()
            {
                for(int i = 0; i < cols; i++)
                {
                    for(int j = 0; j < rows; j++)
                    {
                        grid[i, j] = false;
                    }
                }
            }

            public int CountNeighbors(int x, int y)
            {
                int neighbors = 0;

                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        int nx = x + i;
                        int ny = y + j;

                        if (nx < 0 || ny < 0 || nx >= cols || ny >= rows) continue;

                        if (grid[nx, ny]) neighbors++;
                    }
                }

                return neighbors;
            }
            
            public void Generate(Grid next)
            {
                for(int i = 0; i < cols; i++)
                {
                    for(int j = 0; j < rows; j++)
                    {
                        int neighbors = CountNeighbors(i, j);
                        bool isathing = grid[i, j];

                        if (!isathing && neighbors == 3)
                        {
                            next[i, j] = true;
                        }
                        else if(isathing && (neighbors < 2 || neighbors > 3))
                        {
                            next[i, j] = false;
                        } 
                        else
                        {
                            next[i, j] = isathing;
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            current.Generate(next);

            current = next;
            next = new Grid(50, 25);

            Invalidate();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawGrid(e.Graphics);
        }

        private void DrawGrid(Graphics g)
        {
            for(int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (current[x, y])
                    {
                        g.FillRectangle(Brushes.Black, x * cellSize + 10, y  * cellSize + 60, cellSize, cellSize);
                    } 
                    else
                    {
                        g.DrawRectangle(Pens.Black, x * cellSize + 10, y * cellSize + 60, cellSize, cellSize);
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (e.X - 10) / cellSize;
            int y = (e.Y - 60) / cellSize;
            
            if (e.Button == MouseButtons.Left && x >= 0 && y >= 0 && x < cols && y < rows)
            {
                current[x, y] = true;
            }
            else if(e.Button == MouseButtons.Right && x >= 0 && y >= 0 && x < cols && y < rows)
            {
                current[x, y] = false;
            }

            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            current.Generate(next);

            current = next;
            next = new Grid(50, 25);

            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            current.Clear();

            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer.Start();

            button3.Enabled = false;
            button4.Enabled = true;

            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer.Stop();

            button3.Enabled = true;
            button4.Enabled = false;

            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
