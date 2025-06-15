namespace Life.Components.Pages
{
    public partial class Game_life
    {
        private const int rows = 30;
        private const int cols = 30;
        private bool[,] boolMatrix = new bool[rows, cols];
        private bool initialized = false;

        protected override void OnInitialized()
        {
            boolMatrix = new bool[rows, cols];
            initialized = true;
        }

        private void ToggleValue(int row, int col)
        {
            try
            {
                // Validate array bounds
                if (row >= 0 && row < rows && col >= 0 && col < cols)
                {
                    boolMatrix[row, col] = !boolMatrix[row, col];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling value: {ex.Message}");
            }
        }

        private string GetCellStyle(int row, int col)
        {
            return boolMatrix[row, col]
                ? "background-color: red; width: 30px; height: 30px; border: 1px solid #444;"
                : "background-color: black; width: 30px; height: 30px; border: 1px solid #444;";
        }
    }
}