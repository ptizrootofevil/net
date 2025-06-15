namespace Life.Components.Pages
{
    public partial class Game_life
    {
        private const int height = 25;
        private const int width = 40;
        private bool[,] grid = new bool[height, width];
        private bool initialized = false;

        protected override void OnInitialized()
        {
            grid = new bool[height, width];
            initialized = true;
        }

        private void ChangeCell(int row, int col)
        {
            try
            {
                if (row >= 0 && row < height && col >= 0 && col < width)
                {
                    grid[row, col] = !grid[row, col];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling value: {ex.Message}");
            }
        }
    }
}