using System.Collections.Generic;

namespace WaveFunctionCollapse.Patterns
{
    public class Pattern
    {
        private int _index;
        private int[,] _grid;

        public string HashIndex { get; set; }

        public int Index => _index;

        public Pattern(int[,] grid, int index, string hashCode)
        {
            _index = index;
            _grid = grid;
            HashIndex = hashCode;
        }

        public bool CompareToAnother(Direction dir, Pattern dataPattern)
        {
            int[,] grid = GetGridValuesInDirection(dir);
            int[,] oppositeGrid = dataPattern.GetGridValuesInDirection(dir.GetOppositeDirection());

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] != oppositeGrid[row, col])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int[,] GetGridValuesInDirection(Direction dir)
        {
            int[,] gridPartToCompare;
            switch (dir)
            {
                case Direction.Up:
                    gridPartToCompare = new int[_grid.GetLength(0) - 1, _grid.GetLength(0)];
                    CreatePartOfGrid(0, _grid.GetLength(0), 1, _grid.GetLength(0), gridPartToCompare);
                    break;
                case Direction.Down:
                    gridPartToCompare = new int[_grid.GetLength(0) - 1, _grid.GetLength(0)];
                    CreatePartOfGrid(0, _grid.GetLength(0), 0, _grid.GetLength(0)-1, gridPartToCompare);
                    break;
                case Direction.Right:
                    gridPartToCompare = new int[_grid.GetLength(0), _grid.GetLength(0) -1];
                    CreatePartOfGrid(0, _grid.GetLength(0)-1, 0, _grid.GetLength(0), gridPartToCompare);
                    break;
                case Direction.Left:
                    gridPartToCompare = new int[_grid.GetLength(0), _grid.GetLength(0) -1];
                    CreatePartOfGrid(1, _grid.GetLength(0), 0, _grid.GetLength(0), gridPartToCompare);
                    break;
                default:
                    return _grid;
            }

            return gridPartToCompare;
        }

        private void CreatePartOfGrid(int xMin, int xMax, int yMin, int yMax, int[,] gridPartToCompare)
        {
            List<int> tempList = new List<int>();
            for (int row = yMin; row < yMax; row++)
            {
                for (int col = xMin; col < xMax; col++)
                {
                    tempList.Add(_grid[row,col]);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                int x = i % gridPartToCompare.GetLength(0);
                int y = i / gridPartToCompare.GetLength(0);
                gridPartToCompare[x, y] = tempList[i];
            }
        }

        public void SetGridValue(int x, int y, int value)
        {
            _grid[y, x] = value;
        }

        public int GetGridValue(int x, int y)
        {
            return _grid[y, x];
        }

        public bool CheckValueAtPosition(int x, int y, int value)
        {
            return value.Equals(GetGridValue(x, y));
        }
        
        
    }
}