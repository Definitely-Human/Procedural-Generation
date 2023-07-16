using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class InputReader : IInputReader<TileBase>
    {
        private Tilemap _inputTilemap;


        public InputReader(Tilemap inputTilemap)
        {
            _inputTilemap = inputTilemap;
        }

        public IValue<TileBase>[,] ReadInputToGrid()
        {
            var grid = ReadInputTilemap();

            TileBaseValue[,] gridOfValues = null;
            
            if (grid == null) return gridOfValues;
            
            gridOfValues = new TileBaseValue[grid.GetLength(0),grid.GetLength(1)];
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    gridOfValues[row, col] = new TileBaseValue(grid[row, col]); // TODO: convert to linq grid.ForEach
                }
            }

            return gridOfValues;
        }

        private TileBase[,] ReadInputTilemap()
        {
            InputImageParameters inputImageParameters = new InputImageParameters(_inputTilemap);
            return CreateTileBaseGrid(inputImageParameters);
        }

        private TileBase[,] CreateTileBaseGrid(InputImageParameters inputImageParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}

