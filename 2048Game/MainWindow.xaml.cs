using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _2048Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush BackColor = new SolidColorBrush(Color.FromRgb(187, 173, 160));
        SolidColorBrush ForeColor = new SolidColorBrush(Color.FromRgb(205, 193, 180));
        SolidColorBrush _2 = new SolidColorBrush(Color.FromRgb(238, 228, 218));
        SolidColorBrush _4 = new SolidColorBrush(Color.FromRgb(237, 224, 200));
        SolidColorBrush _8 = new SolidColorBrush(Color.FromRgb(242, 177, 121));
        SolidColorBrush _16 = new SolidColorBrush(Color.FromRgb(238, 143, 96));
        SolidColorBrush _32 = new SolidColorBrush(Color.FromRgb(246, 124, 95));
        SolidColorBrush _64 = new SolidColorBrush(Color.FromRgb(246, 94, 59));
        SolidColorBrush _128 = new SolidColorBrush(Color.FromRgb(237, 207, 114));
        SolidColorBrush _256 = new SolidColorBrush(Color.FromRgb(237, 204, 97));
        SolidColorBrush _512 = new SolidColorBrush(Color.FromRgb(237, 200, 80));
        SolidColorBrush _1024 = new SolidColorBrush(Color.FromRgb(237, 197, 63));
        SolidColorBrush _2048 = new SolidColorBrush(Color.FromRgb(237, 194, 46));

        Size CanvasSize = new Size(600, 600);
        const int BorderRadius = 10;
        const int BlockMargin = 20;
        readonly SolidColorBrush LowValueColor = new SolidColorBrush(Color.FromRgb(119, 110, 101));
        readonly SolidColorBrush HighValueColor = new SolidColorBrush(Color.FromRgb(249, 246, 242));
        const int CellsCount = 4;

        bool LockKeyPress = false;

        public SolidColorBrush GetColor(uint value)
        {
            if (value == 0)
            {
                return ForeColor;
            }
            else if (value == 2)
            {
                return _2;
            }
            else if (value == 4)
            {
                return _4;
            }
            else if (value == 8)
            {
                return _8;
            }
            else if (value == 16)
            {
                return _16;
            }
            else if (value == 32)
            {
                return _32;
            }
            else if (value == 64)
            {
                return _64;
            }
            else if (value == 128)
            {
                return _128;
            }
            else if (value == 2560)
            {
                return _256;
            }
            else if (value == 512)
            {
                return _512;
            }
            else if (value == 1024)
            {
                return _1024;
            }
            else
            {
                return _2048;
            }
        }

        public enum Direction
        {
            Left, Right, Up, Down
        }

        class Cell
        {
            public Cell(uint value, int x, int y)
            {
                Value = value;
                Position = new Point(x, y);
            }
            private uint value;
            public uint Value
            {
                get
                {
                    return value;
                }
                set
                {
                    this.value = value;
                }
            }
            public Point Position
            {
                get; set;
            }
            public Viewbox Content
            {
                get; set;
            }
            public Rectangle Fill
            {
                get; set;
            }
        }

        Cell[,] Cells = new Cell[CellsCount, CellsCount];

        bool Moved = false;
        int FilledCells = 0;
        int AnimatedCells = 0;

        public void Move(int x, int y, Direction dir)
        {
            Point StartPos = new Point(x, y);
            Point EndPos = new Point(x, y);
            var val = Cells[x, y].Value;
            var color = Cells[x, y].Fill.Fill;
            int xOffset = 0;
            int yOffset = 0;
            if (dir == Direction.Left)
            {
                xOffset = -1;
                if (x == 0)
                {
                    return;
                }
            }
            else if (dir == Direction.Right)
            {
                xOffset = 1;
                if (x == CellsCount - 1)
                {
                    return;
                }
            }
            else if (dir == Direction.Up)
            {
                yOffset = -1;
                if (y == 0)
                {
                    return;
                }
            }
            else if (dir == Direction.Down)
            {
                yOffset = 1;
                if (y == CellsCount - 1)
                {
                    return;
                }
            }
            if (Cells[x + xOffset, y + yOffset].Value == Cells[x, y].Value)
            {
                var value = Cells[x, y].Value;
                Cells[x, y].Value = 0;
                Cells[x + xOffset, y + yOffset].Value = value * 2;
                EndPos.X = x + xOffset;
                EndPos.Y = y + yOffset;
                Moved = true;
            }
            else
            {
                bool end = false;
                for (; !end;)
                {
                    if (Cells[x + xOffset, y + yOffset].Value == 0)
                    {
                        var value = Cells[x, y].Value;
                        Cells[x, y].Value = 0;
                        Cells[x + xOffset, y + yOffset].Value = value;
                        EndPos.X = x + xOffset;
                        EndPos.Y = y + yOffset;
                        x += xOffset;
                        y += yOffset;                       
                        Moved = true;
                    }
                    else
                    {
                        if (Cells[x + xOffset, y + yOffset].Value == Cells[x, y].Value)
                        {
                            var value = Cells[x, y].Value;
                            Cells[x, y].Value = 0;
                            Cells[x + xOffset, y + yOffset].Value = value * 2;
                            EndPos.X = x + xOffset;
                            EndPos.Y = y + yOffset;
                            Moved = true;
                        }
                        break;
                    }
                    if (dir == Direction.Left && x <= 0)
                    {
                        end = true;
                    }
                    else if (dir == Direction.Right && x >= CellsCount - 1)
                    {
                        end = true;
                    }
                    else if (dir == Direction.Up && y <= 0)
                    {
                        end = true;
                    }
                    else if (dir == Direction.Down && y >= CellsCount - 1)
                    {
                        end = true;
                    }
                }
            }
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);

            var width = CanvasSize.Width - 2 * BlockMargin;
            var height = CanvasSize.Height - 2 * BlockMargin;

            var cellWidth = width / CellsCount - (CellsCount - 1.0) / CellsCount * BlockMargin;
            var cellHeight = height / CellsCount - (CellsCount - 1.0) / CellsCount * BlockMargin;

            var x1 = StartPos.X * cellWidth + BlockMargin * (StartPos.X + 1);
            var y1 = StartPos.Y * cellWidth + BlockMargin * (StartPos.Y + 1);

            var x2 = EndPos.X * cellWidth + BlockMargin * (EndPos.X + 1);
            var y2 = EndPos.Y * cellWidth + BlockMargin * (EndPos.Y + 1);

            var itteration = 0;
            var itterations = 8;

            var cellFill = new Rectangle();

            cellFill.Fill = GetColor(val);

            cellFill.Width = cellWidth;
            cellFill.Height = cellHeight;

            cellFill.SetValue(LeftProperty, (x2 - x1) * itteration / (double)itterations + x1);
            cellFill.SetValue(TopProperty, (y2 - y1) * itteration / (double)itterations + y1);

            cellFill.RadiusX = BorderRadius;
            cellFill.RadiusY = BorderRadius;                       

            var label = new Label();
            label.Content = val.ToString();

            var viewBox = new Viewbox();

            label.Foreground = ((Label)Cells[(int)StartPos.X, (int)StartPos.Y].Content.Child).Foreground;

            viewBox.Child = label;

            viewBox.Width = cellWidth;
            viewBox.Height = cellHeight;

            viewBox.SetValue(LeftProperty, (x2 - x1) * itteration / (double)itterations + x1);
            viewBox.SetValue(TopProperty, (y2 - y1) * itteration / (double)itterations + y1);

            timer.Tick += (object sender, EventArgs e) =>
            {
                var newX = (x2 - x1) * itteration / (double)itterations + x1;
                var newY = (y2 - y1) * itteration / (double)itterations + y1;

                cellFill.SetValue(LeftProperty, newX);
                cellFill.SetValue(TopProperty, newY);
                viewBox.SetValue(LeftProperty, newX);
                viewBox.SetValue(TopProperty, newY);

                if (itteration >= itterations)
                {                   
                    Playground.Children.Remove(cellFill);
                    Playground.Children.Remove(viewBox);
                    timer.Stop();
                    UpdateGrid();
                    if(AnimatedCells >= FilledCells)
                    {
                        LockKeyPress = false;
                    }
                }

                itteration++;
            };            

            if(val!=0)
            {
                AnimatedCells++;
                Playground.Children.Add(cellFill);
                Playground.Children.Add(viewBox);                
                timer.Start();
                Cells[(int)StartPos.X, (int)StartPos.Y].Fill.Fill = ForeColor;
                ((Label)Cells[(int)StartPos.X, (int)StartPos.Y].Content.Child).Content = "";
            }            
        }

        public void CreateGrid()
        {
            var background = new Rectangle();

            background.Fill = BackColor;

            background.Width = CanvasSize.Width;
            background.Height = CanvasSize.Height;

            background.RadiusX = BorderRadius;
            background.RadiusY = BorderRadius;

            Playground.Children.Add(background);

            var width = CanvasSize.Width - 2 * BlockMargin;
            var height = CanvasSize.Height - 2 * BlockMargin;

            var cellWidth = width / CellsCount - (CellsCount - 1.0) / CellsCount * BlockMargin;
            var cellHeight = height / CellsCount - (CellsCount - 1.0) / CellsCount * BlockMargin;

            for (int x = 0; x < CellsCount; x++)
            {
                for (int y = 0; y < CellsCount; y++)
                {
                    var cell = new Rectangle();

                    cell.Fill = ForeColor;

                    cell.Width = cellWidth;
                    cell.Height = cellHeight;

                    cell.SetValue(LeftProperty, x * cellWidth + BlockMargin * (x + 1));
                    cell.SetValue(TopProperty, y * cellHeight + BlockMargin * (y + 1));

                    cell.RadiusX = BorderRadius;
                    cell.RadiusY = BorderRadius;

                    var cellFill = new Rectangle();

                    cellFill.Fill = ForeColor;

                    cellFill.Width = cellWidth;
                    cellFill.Height = cellHeight;

                    cellFill.SetValue(LeftProperty, x * cellWidth + BlockMargin * (x + 1));
                    cellFill.SetValue(TopProperty, y * cellHeight + BlockMargin * (y + 1));

                    cellFill.RadiusX = BorderRadius;
                    cellFill.RadiusY = BorderRadius;

                    var label = new Label();
                    label.Content = "";

                    var viewBox = new Viewbox();

                    viewBox.Child = label;

                    viewBox.Width = cellWidth;
                    viewBox.Height = cellHeight;

                    viewBox.SetValue(LeftProperty, x * cellWidth + BlockMargin * (x + 1));
                    viewBox.SetValue(TopProperty, y * cellHeight + BlockMargin * (y + 1));

                    Cells[x, y] = new Cell(0, x, y);
                    Cells[x, y].Content = viewBox;
                    Cells[x, y].Fill = cell;

                    Playground.Children.Add(cell);
                    Playground.Children.Add(viewBox);
                }
            }
        }

        public void CreateRandomCell()
        {
            var rand = new Random();
            uint value = 2;
            if (rand.NextDouble() > 0.9)
            {
                value = 4;
            }
            var index = new Random();
            var emptyCells = new List<Cell>();
            foreach (var cell in Cells)
            {
                if (cell.Value == 0)
                {
                    emptyCells.Add(cell);
                }
            }
            var selectedCell = emptyCells[index.Next(emptyCells.Count)];
            selectedCell.Value = value;
        }

        public void UpdateGrid()
        {
            for (int x = 0; x < CellsCount; x++)
            {
                for (int y = 0; y < CellsCount; y++)
                {
                    var cell = Cells[x, y];
                    cell.Fill.Fill = GetColor(cell.Value);
                    if (cell.Value != 0)
                    {
                        ((Label)cell.Content.Child).Content = cell.Value.ToString();
                        if (cell.Value < 8)
                        {
                            ((Label)cell.Content.Child).Foreground = LowValueColor;
                        }
                        else
                        {
                            ((Label)cell.Content.Child).Foreground = HighValueColor;
                        }
                    }
                    else
                    {
                        ((Label)cell.Content.Child).Content = "";
                    }
                }
            }
        }

        public void Clear()
        {
            foreach(var cell in Cells)
            {
                cell.Fill.Fill = ForeColor;
                ((Label)cell.Content.Child).Content = "";
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Playground.Width = CanvasSize.Width;
            Playground.Height = CanvasSize.Height;

            CreateGrid();
           
            Cells[0, 2].Value = 2;      
            
            UpdateGrid();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(!LockKeyPress)
            {
                AnimatedCells = 0;
                FilledCells = 0;                
                if (e.Key == Key.Right)
                {
                    LockKeyPress = true;
                    foreach (var cell in Cells)
                    {
                        if (cell.Value != 0&&cell.Position.X<CellsCount-1)
                        {
                            FilledCells += 1;
                        }
                    }
                    for (int x = CellsCount - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < CellsCount; y++)
                        {
                            var cell = Cells[x, y];
                            Move((int)cell.Position.X, (int)cell.Position.Y, Direction.Right);
                        }
                    }
                }
                if (e.Key == Key.Left)
                {
                    foreach (var cell in Cells)
                    {
                        if (cell.Value != 0 && cell.Position.X >0)
                        {
                            FilledCells += 1;
                        }
                    }
                    LockKeyPress = true;
                    for (int x = 0; x < CellsCount; x++)
                    {
                        for (int y = 0; y < CellsCount; y++)
                        {
                            var cell = Cells[x, y];
                            Move((int)cell.Position.X, (int)cell.Position.Y, Direction.Left);
                        }
                    }
                }
                if (e.Key == Key.Up)
                {
                    foreach (var cell in Cells)
                    {
                        if (cell.Value != 0 && cell.Position.Y > 0)
                        {
                            FilledCells += 1;
                        }
                    }
                    LockKeyPress = true;
                    for (int x = 0; x < CellsCount; x++)
                    {
                        for (int y = 0; y < CellsCount; y++)
                        {
                            var cell = Cells[x, y];
                            Move((int)cell.Position.X, (int)cell.Position.Y, Direction.Up);
                        }
                    }
                }
                if (e.Key == Key.Down)
                {
                    foreach (var cell in Cells)
                    {
                        if (cell.Value != 0 && cell.Position.Y<CellsCount-1)
                        {
                            FilledCells += 1;
                        }
                    }
                    LockKeyPress = true;
                    for (int x = 0; x < CellsCount; x++)
                    {
                        for (int y = CellsCount - 1; y >= 0; y--)
                        {
                            var cell = Cells[x, y];
                            Move((int)cell.Position.X, (int)cell.Position.Y, Direction.Down);
                        }
                    }
                }
                if (Moved)
                {
                    CreateRandomCell();
                    Moved = false;
                }
            }            
        }
    }
}
