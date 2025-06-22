using System.Xml.Schema;
using System.Timers;

namespace Life.Components.Pages
{
    public partial class Game_life
    {
        private const int height = 30;
        private const int width = 60;
        private Cell[,] grid = new Cell[height, width];
        private HashSet<Cell> livingCells = new HashSet<Cell>();
        private HashSet<Cell> potentialCells = new HashSet<Cell>();
        private bool initialized = false;
        private System.Timers.Timer timer = new System.Timers.Timer(1000);
        private bool isRunning = false;

        protected override void OnInitialized()
        {
            grid = new Cell[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grid[i, j] = new Cell(i, j);
                }
            }
            initialized = true;
            timer.Elapsed += OnTimerTick;
            timer.AutoReset = true;
        }
        class Cell
        {
            public bool IsAlive { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int LivingNeighbours { get; set; }
            public Cell(int row, int col)
            {
                this.X = row;
                this.Y = col;
                this.IsAlive = false;
                this.LivingNeighbours = 0;
            }
        }
        private void ChangeCell(int row, int col)
        {
            try
            {
                if (row >= 0 && row < height && col >= 0 && col < width)
                {
                    grid[row, col].IsAlive = !grid[row, col].IsAlive;
                    if (grid[row, col].IsAlive) livingCells.Add(grid[row, col]);
                    else livingCells.Remove(grid[row, col]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling value: {ex.Message}");
            }
        }
         private async void OnTimerTick(object sender, ElapsedEventArgs? e)
        {
            await InvokeAsync(() =>
            {
                Tick();
                StateHasChanged();
            });
        }
        

        private void ToggleTimer()
        {
            if (isRunning)
            {
                this.timer.Stop();
            }
            else
            {
                this.timer.Start();
            }
            isRunning = !isRunning;
        }

        private void ResetGrid()
        {
            if (isRunning) ToggleTimer();

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    grid[row, col].IsAlive = false;
                }
            }
            livingCells.Clear();
        }

        private void Tick()
        {
            if (!initialized) return;
            else
            {
                foreach (Cell cell in livingCells)
                {
                    if (cell.X > 0 && cell.Y > 0)
                    {
                        this.grid[cell.X - 1, cell.Y - 1].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X - 1, cell.Y - 1]);
                    }
                    if (cell.X > 0)
                    {
                        this.grid[cell.X - 1, cell.Y].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X - 1, cell.Y]);
                    }
                    if (cell.X > 0 && cell.Y + 1 < width)
                    {
                        this.grid[cell.X - 1, cell.Y + 1].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X - 1, cell.Y + 1]);
                    }
                    if (cell.Y > 0)
                    {
                        this.grid[cell.X, cell.Y - 1].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X, cell.Y - 1]);
                    }
                    if (cell.Y + 1 < width)
                    {
                        this.grid[cell.X, cell.Y + 1].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X, cell.Y + 1]);
                    }
                    if (cell.X + 1 < height && cell.Y > 0)
                    {
                        this.grid[cell.X + 1, cell.Y - 1].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X + 1, cell.Y - 1]);
                    }
                    if (cell.X + 1 < height)
                    {
                        this.grid[cell.X + 1, cell.Y].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X + 1, cell.Y]);
                    }
                    if (cell.X + 1 < height && cell.Y + 1 < width)
                    {
                        this.grid[cell.X + 1, cell.Y + 1].LivingNeighbours += 1;
                        this.potentialCells.Add(this.grid[cell.X + 1, cell.Y + 1]);
                    }

                }

                potentialCells.UnionWith(this.livingCells);
                foreach (Cell cell in potentialCells)
                {
                    if (cell.IsAlive == false && cell.LivingNeighbours == 3)
                    {
                        cell.IsAlive = true;
                        this.livingCells.Add(cell);
                    }
                    else if (cell.LivingNeighbours < 2 || cell.LivingNeighbours > 3)
                    {
                        cell.IsAlive = false;
                        this.livingCells.Remove(cell);
                    }
                    cell.LivingNeighbours = 0;
                }
                potentialCells.Clear();
            }
        }
    }
}