using System;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse.Patterns.Strategies;

namespace WaveFunctionCollapse.Patterns
{
    public static class PatternFinder
    {
        public static PatternDataResults GetPatternResultFromGrid<T>(ValuesManager<T> valuesManager, int patternSize, bool equalWeights)
        {
            Dictionary<string, PatternData> patternHashCodes = new Dictionary<string, PatternData>();
            Dictionary<int, PatternData> patternIndexDictionary = new Dictionary<int, PatternData>();
            Vector2Int sizeOfGrid = valuesManager.GetGridSize();
            int patternGridSizeX, patternGridSizeY;
            int rowMin = -1, rowMax = -1, colMin = -1, colMax = -1;
            if (patternSize < 3)
            {
                patternGridSizeX = (int)sizeOfGrid.x + 3 - patternSize;
                patternGridSizeY = (int)sizeOfGrid.y + 3 - patternSize;
                rowMax = patternGridSizeY - 1;
                colMax = patternGridSizeX - 1;
            }
            else
            {
                patternGridSizeX = sizeOfGrid.x + patternSize - 1;
                patternGridSizeY = sizeOfGrid.y + patternSize - 1;
                rowMin = 1 - patternSize;
                colMin = 1 - patternSize;
                rowMax = (int)sizeOfGrid.y;
                colMax = (int)sizeOfGrid.x;
            }

            int[,] patternIndicesGrid = new int[patternGridSizeY,patternGridSizeX];
            int totalFrequency = 0, patternIndex = 0;
            for (int row = rowMin; row < rowMax; row++)
            {
                for (int col = colMin; col < colMax; col++)
                {
                    int[,] gridValues = valuesManager.GetPatternValuesFromGridAt(col, row, patternSize);
                    // string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);
                }
            }

            throw new Exception();
        }

        public static Dictionary<int, PatternNeighbours> FindPossibleNeighboursForAllPatterns(IFindNeighbourStrategy strategy, PatternDataResults patternFinderResult)
        {
            throw new System.NotImplementedException();
        }
    }
}