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
        }
        private void ChangeCell(int row, int col)
        {
            try
            {
                if (row >= 0 && row < height && col >= 0 && col < width)
                {
                    grid[row, col].IsAlive = !grid[row, col].IsAlive;
                    if (!livingCells.Remove(grid[row, col]) && grid[row, col].IsAlive) livingCells.Add(grid[row, col]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling value: {ex.Message}");
            }
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
                    this.grid[cell.X - 1, cell.Y - 1].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X - 1, cell.Y - 1]);

                    this.grid[cell.X - 1, cell.Y].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X - 1, cell.Y]);

                    this.grid[cell.X - 1, cell.Y + 1].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X - 1, cell.Y + 1]);

                    this.grid[cell.X, cell.Y - 1].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X, cell.Y - 1]);

                    this.grid[cell.X, cell.Y + 1].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X, cell.Y + 1]);

                    this.grid[cell.X + 1, cell.Y - 1].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X - 1, cell.Y - 1]);

                    this.grid[cell.X + 1, cell.Y].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X - 1, cell.Y]);

                    this.grid[cell.X + 1, cell.Y].LivingNeighbours += 1;
                    this.potentialCells.Add(this.grid[cell.X - 1, cell.Y + 1]);

                }

                potentialCells.UnionWith(this.livingCells);
                foreach (Cell cell in potentialCells)
                {
                    if (cell.IsAlive == false && cell.LivingNeighbours == 3)
                    {
                        cell.IsAlive = true;
                        this.livingCells.Add(cell);
                    }
                    else if (cell.LivingNeighbours < 1 || cell.LivingNeighbours > 3)
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