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

        public SolidColorBrush GetColor(uint value)
        {
            if(value == 0)
            {
                return ForeColor;
            }
            else if(value==2)
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

        public void MoveLeft(int x, int y)
        {
            var cell = Cells[x, y];
            if (x == 0)
            {
                return;
            }
            if (Cells[x - 1, y].Value == Cells[x, y].Value)
            {
                var value = Cells[x, y].Value;
                Cells[x, y].Value = 0;
                Cells[x - 1, y].Value = value * 2;
            }
            else
            {
                for (; x > 0;)
                {
                    if (Cells[x - 1, y].Value == 0)
                    {
                        var value = Cells[x, y].Value;
                        Cells[x, y].Value = 0;
                        Cells[x - 1, y].Value = value;
                        x--;
                    }
                    else
                    {
                        if (Cells[x - 1, y].Value == Cells[x, y].Value)
                        {
                            var value = Cells[x, y].Value;
                            Cells[x, y].Value = 0;
                            Cells[x - 1, y].Value = value * 2;
                        }
                        break;
                    }
                }
            }
        }

        public void MoveRight(int x, int y)
        {
            var cell = Cells[x, y];
            if (x == CellsCount - 1)
            {
                return;
            }
            if (Cells[x + 1, y].Value == Cells[x, y].Value)
            {
                var value = Cells[x, y].Value;
                Cells[x, y].Value = 0;
                Cells[x + 1, y].Value = value * 2;
            }
            else
            {
                for (; x < CellsCount - 1;)
                {
                    if (Cells[x + 1, y].Value == 0)
                    {
                        var value = Cells[x, y].Value;
                        Cells[x, y].Value = 0;
                        Cells[x + 1, y].Value = value;
                        x++;
                    }
                    else
                    {
                        if (Cells[x + 1, y].Value == Cells[x, y].Value)
                        {
                            var value = Cells[x, y].Value;
                            Cells[x, y].Value = 0;
                            Cells[x + 1, y].Value = value * 2;
                        }
                        break;
                    }
                }
            }
        }

        public void MoveUp(int x, int y)
        {
            var cell = Cells[x, y];
            if (y == 0)
            {
                return;
            }
            if (Cells[x, y - 1].Value == Cells[x, y].Value)
            {
                var value = Cells[x, y].Value;
                Cells[x, y].Value = 0;
                Cells[x, y - 1].Value = value * 2;
            }
            else
            {
                for (; y > 0;)
                {
                    if (Cells[x, y - 1].Value == 0)
                    {
                        var value = Cells[x, y].Value;
                        Cells[x, y].Value = 0;
                        Cells[x, y - 1].Value = value;
                        y--;
                    }
                    else
                    {
                        if (Cells[x, y - 1].Value == Cells[x, y].Value)
                        {
                            var value = Cells[x, y].Value;
                            Cells[x, y].Value = 0;
                            Cells[x, y - 1].Value = value * 2;
                        }
                        break;
                    }
                }
            }
        }

        public void MoveDown(int x, int y)
        {
            var cell = Cells[x, y];
            if (y == CellsCount - 1)
            {
                return;
            }
            if (Cells[x, y + 1].Value == Cells[x, y].Value)
            {
                var value = Cells[x, y].Value;
                Cells[x, y].Value = 0;
                Cells[x, y + 1].Value = value * 2;
            }
            else
            {
                for (; y < CellsCount - 1;)
                {
                    if (Cells[x, y + 1].Value == 0)
                    {
                        var value = Cells[x, y].Value;
                        Cells[x, y].Value = 0;
                        Cells[x, y + 1].Value = value;
                        y++;
                    }
                    else
                    {
                        if (Cells[x, y + 1].Value == Cells[x, y].Value)
                        {
                            var value = Cells[x, y].Value;
                            Cells[x, y].Value = 0;
                            Cells[x, y + 1].Value = value * 2;
                        }
                        break;
                    }
                }
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

        public MainWindow()
        {
            InitializeComponent();
            Playground.Width = CanvasSize.Width;
            Playground.Height = CanvasSize.Height;

            CreateGrid();

            Cells[2, 2].Value = 16;
            Cells[0, 2].Value = 2;
            Cells[3, 2].Value = 16;
            UpdateGrid();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                for (int x = CellsCount - 1; x >= 0; x--)
                {
                    for (int y = 0; y < CellsCount; y++)
                    {
                        var cell = Cells[x, y];
                        MoveRight((int)cell.Position.X, (int)cell.Position.Y);
                    }
                }
            }
            if (e.Key == Key.Left)
            {
                for (int x = 0; x < CellsCount; x++)
                {
                    for (int y = 0; y < CellsCount; y++)
                    {
                        var cell = Cells[x, y];
                        MoveLeft((int)cell.Position.X, (int)cell.Position.Y);
                    }
                }
            }
            if (e.Key == Key.Up)
            {
                for (int x = 0; x < CellsCount; x++)
                {
                    for (int y = 0; y < CellsCount; y++)
                    {
                        var cell = Cells[x, y];
                        MoveUp((int)cell.Position.X, (int)cell.Position.Y);
                    }
                }
            }
            if (e.Key == Key.Down)
            {
                for (int x = 0; x < CellsCount; x++)
                {
                    for (int y = CellsCount-1; y >=0; y--)
                    {
                        var cell = Cells[x, y];
                        MoveDown((int)cell.Position.X, (int)cell.Position.Y);
                    }
                }
            }
            UpdateGrid();
        }
    }
}
