using System.Xml.Schema;
using System.Timers;

namespace Life.Components.Pages
{
    public partial class Game_life
    {
        private const int height = 25;
        private const int width = 40;
        private Cell[,] grid = new Cell[height, width];
        private HashSet<Cell> livingCells = new HashSet<Cell>();
        private HashSet<Cell> potentialCells = new HashSet<Cell>();
        private bool initialized = false;
        private int generation = 0;
        private System.Timers.Timer timer;
        private bool isRunning = false;
        private string lastTickTime;

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
            timer = new System.Timers.Timer(1000); // 2 second interval
            timer.Elapsed += OnTimerTick;
            timer.AutoReset = true;
        }
         private async void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            await InvokeAsync(() => 
            {
                Tick(); // Your tick function
                lastTickTime = DateTime.Now.ToString("HH:mm:ss.fff");
                StateHasChanged(); // Update UI
            });
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

        private void ToggleTimer()
        {
            if (isRunning)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
            isRunning = !isRunning;
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
                this.generation += 1;
            }
        }
    }
}