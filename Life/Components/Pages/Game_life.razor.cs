namespace Life.Components.Pages
{
    public partial class Game_life
    {
        private bool[,] boolMatrix = new bool[25, 40]; // Initialize all to false

        private void ToggleValue(int row, int col)
        {
            boolMatrix[row, col] = !boolMatrix[row, col];
        }

        private string GetButtonClass(int row, int col)
        {
            return boolMatrix[row, col] ? "btn btn-success" : "btn btn-light";
        }
    }
}