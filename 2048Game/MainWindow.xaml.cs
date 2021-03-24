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
using System.Windows.Media.Animation;

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

        uint Score = 0;

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
            public bool Joined
            {
                get; set;
            }
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
            if (Cells[x + xOffset, y + yOffset].Value == Cells[x, y].Value && !Cells[x + xOffset, y + yOffset].Joined && !Cells[x, y].Joined && Cells[x, y].Value != 0)
            {
                var value = Cells[x, y].Value;
                Cells[x, y].Value = 0;
                Cells[x + xOffset, y + yOffset].Value = value * 2;
                Score += value * 2;
                EndPos.X = x + xOffset;
                EndPos.Y = y + yOffset;
                Cells[x + xOffset, y + yOffset].Joined = true;
                if (value > 0)
                {
                    Moved = true;
                }
            }
            else
            {
                bool end = false;
                for (; !end;)
                {
                    if (Cells[x + xOffset, y + yOffset].Value == 0 && Cells[x, y].Value != 0)
                    {
                        var value = Cells[x, y].Value;
                        Cells[x, y].Value = 0;
                        Cells[x + xOffset, y + yOffset].Value = value;
                        if (Cells[x, y].Joined)
                        {
                            Cells[x, y].Joined = false;
                            Cells[x + xOffset, y + yOffset].Joined = true;
                        }
                        if (!Cells[x, y].Joined)
                        {
                            Cells[x + xOffset, y + yOffset].Joined = false;
                        }
                        EndPos.X = x + xOffset;
                        EndPos.Y = y + yOffset;
                        x += xOffset;
                        y += yOffset;
                        if (value > 0)
                        {
                            Moved = true;
                        };
                    }
                    else
                    {
                        if (Cells[x + xOffset, y + yOffset].Value == Cells[x, y].Value && !Cells[x + xOffset, y + yOffset].Joined && !Cells[x, y].Joined && Cells[x, y].Value != 0)
                        {
                            var value = Cells[x, y].Value;
                            Cells[x, y].Value = 0;
                            Cells[x + xOffset, y + yOffset].Value = value * 2;
                            Score += value * 2;
                            EndPos.X = x + xOffset;
                            EndPos.Y = y + yOffset;
                            Cells[x + xOffset, y + yOffset].Joined = true;
                            if (value > 0)
                            {
                                Moved = true;
                            }
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

            var width = CanvasSize.Width - 2 * BlockMargin;
            var height = CanvasSize.Height - 2 * BlockMargin;

            var cellWidth = width / CellsCount - (CellsCount - 1.0) / CellsCount * BlockMargin;
            var cellHeight = height / CellsCount - (CellsCount - 1.0) / CellsCount * BlockMargin;

            var x1 = StartPos.X * cellWidth + BlockMargin * (StartPos.X + 1);
            var y1 = StartPos.Y * cellWidth + BlockMargin * (StartPos.Y + 1);

            var x2 = EndPos.X * cellWidth + BlockMargin * (EndPos.X + 1);
            var y2 = EndPos.Y * cellWidth + BlockMargin * (EndPos.Y + 1);

            var itteration = 0;
            var itterations = 10;

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

            if (val != 0)
            {
                AnimatedCells++;
                Playground.Children.Add(cellFill);
                Playground.Children.Add(viewBox);

                var animationX = new DoubleAnimation(x1, x2, new Duration(TimeSpan.FromSeconds(0.2)));
                var animationY = new DoubleAnimation(y1, y2, new Duration(TimeSpan.FromSeconds(0.2)));

                animationX.Completed += (object sender, EventArgs e) =>
                {
                    Playground.Children.Remove(cellFill);
                    Playground.Children.Remove(viewBox);
                    UpdateGrid((int)EndPos.X, (int)EndPos.Y);
                    if (AnimatedCells >= FilledCells)
                    {
                        LockKeyPress = false;
                        UpdateGrid();
                    }
                    var soltionExists = CheckCells();
                    if (!soltionExists)
                    {
                        LockKeyPress = true;
                        Menu.Visibility = Visibility.Visible;
                        Fill.Visibility = Visibility.Visible;
                        MenuMessage.Content = "Game over";
                        var animation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.2)));
                        Menu.BeginAnimation(OpacityProperty, animation);
                        Fill.BeginAnimation(OpacityProperty, animation);
                    }
                    foreach(var cell in Cells)
                    {
                        if(cell.Value==2048)
                        {
                            LockKeyPress = true;
                            Menu.Visibility = Visibility.Visible;
                            Fill.Visibility = Visibility.Visible;
                            MenuMessage.Content = "You win";
                            var animation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.2)));
                            Menu.BeginAnimation(OpacityProperty, animation);
                            Fill.BeginAnimation(OpacityProperty, animation);
                        }
                    }
                };

                cellFill.BeginAnimation(LeftProperty, animationX);
                cellFill.BeginAnimation(TopProperty, animationY);

                viewBox.BeginAnimation(LeftProperty, animationX);
                viewBox.BeginAnimation(TopProperty, animationY);

                Cells[(int)StartPos.X, (int)StartPos.Y].Fill.Fill = ForeColor;
                ((Label)Cells[(int)StartPos.X, (int)StartPos.Y].Content.Child).Content = "";
            }
        }

        public void UpdateScore()
        {
            ScoreLabel.Content = Score.ToString();
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

        public void UpdateGrid(int x, int y)
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

        public void Clear()
        {
            foreach (var cell in Cells)
            {
                cell.Fill.Fill = ForeColor;
                ((Label)cell.Content.Child).Content = "";
            }
        }

        public void Reload()
        {
            LockKeyPress = true;
            var animation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.2)));
            animation.Completed += (object sender, EventArgs e) =>
            {
                Menu.Visibility = Visibility.Hidden;
                Fill.Visibility = Visibility.Hidden;
                LockKeyPress = false;
            };
            Menu.BeginAnimation(OpacityProperty, animation);
            Fill.BeginAnimation(OpacityProperty, animation);
            Moved = false;

            FilledCells = 0;

            Score = 0;

            AnimatedCells = 0;

            Cells = new Cell[4, 4];

            Playground.Children.Clear();

            CreateGrid();

            CreateRandomCell();
            CreateRandomCell();

            UpdateGrid();
        }

        public void ClearCells()
        {
            foreach (var cell in Cells)
            {
                cell.Joined = false;
            }
        }

        public bool CheckCells()
        {
            bool hasEmptyCell = false;
            foreach (var cell in Cells)
            {
                if (cell.Value == 0)
                {
                    hasEmptyCell = true;
                }
            }
            if (hasEmptyCell)
            {
                return true;
            }
            bool hasSolution = false;
            for (int x = 0; x < CellsCount; x++)
            {
                for (int y = 0; y < CellsCount; y++)
                {
                    if (x > 0)
                    {
                        if (Cells[x - 1, y].Value == Cells[x, y].Value)
                        {
                            hasSolution = true;
                        }
                    }
                    if (y > 0)
                    {
                        if (Cells[x, y - 1].Value == Cells[x, y].Value)
                        {
                            hasSolution = true;
                        }
                    }
                    if (x < CellsCount - 1)
                    {
                        if (Cells[x + 1, y].Value == Cells[x, y].Value)
                        {
                            hasSolution = true;
                        }
                    }
                    if (y < CellsCount - 1)
                    {
                        if (Cells[x, y + 1].Value == Cells[x, y].Value)
                        {
                            hasSolution = true;
                        }
                    }
                }
            }
            return hasSolution;
        }

        public MainWindow()
        {
            InitializeComponent();
            Playground.Width = CanvasSize.Width;
            Playground.Height = CanvasSize.Height;

            CreateGrid();

            CreateRandomCell();
            CreateRandomCell();

            UpdateGrid();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!LockKeyPress)
            {
                AnimatedCells = 0;
                FilledCells = 0;
                if (e.Key == Key.Right)
                {
                    ClearCells();
                    foreach (var cell in Cells)
                    {
                        if (cell.Value != 0 && cell.Position.X < CellsCount - 1)
                        {
                            FilledCells += 1;
                        }
                    }
                    LockKeyPress = true;
                    for (int x = CellsCount - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < CellsCount; y++)
                        {
                            var cell = Cells[x, y];
                            Move((int)cell.Position.X, (int)cell.Position.Y, Direction.Right);
                        }
                    }
                    Console.WriteLine("Right");
                }
                if (e.Key == Key.Left)
                {
                    ClearCells();
                    foreach (var cell in Cells)
                    {
                        if (cell.Value != 0 && cell.Position.X > 0)
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
                    Console.WriteLine("Left");
                }
                if (e.Key == Key.Up)
                {
                    ClearCells();
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
                    Console.WriteLine("Up");
                }
                if (e.Key == Key.Down)
                {
                    ClearCells();
                    foreach (var cell in Cells)
                    {
                        if (cell.Value != 0 && cell.Position.Y < CellsCount - 1)
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
                    Console.WriteLine("Down");
                }
                if (Moved)
                {
                    CreateRandomCell();
                    Moved = false;
                }
                UpdateScore();
            }
        }

        private void Retry_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Reload();
        }
    }
}
