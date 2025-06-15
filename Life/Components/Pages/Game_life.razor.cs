namespace Life.Components.Pages
{
    public partial class Game_life
    {
        private const int height = 25;
        private const int width = 40;
        private Cell[,] grid = new Cell[height, width];
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
            public int x { get; set; }
            public int y { get; set; }

            public Cell(int row, int col)
            {
                this.x = row;
                this.y = col;
                this.IsAlive = false;
            }
        }

        private void Tick()
        {

        }
    }
}