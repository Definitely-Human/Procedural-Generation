using System;
using System.Collections.Generic;
using System.Linq;

namespace WaveFunctionCollapse
{
    public class ValuesManager<T>
    {
        private int[,] _grid;
        private Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        private int index = 0;

        public ValuesManager(IValue<T>[,] gridOfValue)
        {
            CreateGridOfIndices(gridOfValue);
        }

        private void CreateGridOfIndices(IValue<T>[,] gridOfValues)
        {
            _grid = new int[gridOfValues.GetLength(0),gridOfValues.GetLength(1)];
            for (int row = 0; row < gridOfValues.GetLength(0); row++)
            {
                for (int col = 0; col < gridOfValues.GetLength(1); col++)
                {
                    SetIndexToGridPosition(gridOfValues, row, col);
                }
            }
        }

        private void SetIndexToGridPosition(IValue<T>[,] gridOfValues, int row, int col)
        {
            if (valueIndexDictionary.ContainsValue(gridOfValues[row, col]))
            {
                var key = valueIndexDictionary.FirstOrDefault(
                    x => x.Value.Equals(gridOfValues[row,col])
                    );
                _grid[row, col] = key.Key;
            }
            else
            {
                _grid[row, col] = index;
                valueIndexDictionary.Add(_grid[row,col],gridOfValues[row,col]);
                index++;
            }
        }

        public int GetGridValue(int x, int y)
        {
            if (x >= _grid.GetLength(1) || x < 0 || y >= _grid.GetLength(0) || y < 0)
            {
                throw new Exception($"Grid does not contain x:{x} y:{y}.");
            }
            return _grid[y,x];
        }

        public IValue<T> GetValueFromIndex(int index)
        {
            if (!valueIndexDictionary.ContainsKey(index)) throw new Exception("No index " + index + " in valueDictionary.");
            return valueIndexDictionary[index];
        }

        public int GetGridValuesIncludingOffset(int x, int y)
        {
            int yMax = _grid.GetLength(0);
            int xMax = _grid.GetLength(1);
            x = GetOffset(x, xMax);
            y = GetOffset(y, yMax);
            return GetGridValue(x, y);
        }
        
        private int GetOffset(int value, int max)
        {
            value = value % max;
            if (value < 0)
            {
                value = max + value;
            }
            return value;
        }

        public int[,] GetPatternValuesFromGridAt(int x, int y, int patternSize)
        {
            int[,] pattern = new int[patternSize, patternSize];
            for (int row = 0; row < patternSize; row++)
            {
                for (int col = 0; col < patternSize; col++)
                {
                    pattern[row, col] = GetGridValuesIncludingOffset(x + col, y + row);
                }
            }

            return pattern;
        }
    }
}
