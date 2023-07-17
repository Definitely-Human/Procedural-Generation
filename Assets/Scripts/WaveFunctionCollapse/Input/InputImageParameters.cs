using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class InputImageParameters
    {
        private Vector2Int? bottomLeftTile = null;
        private Vector2Int? topRightTile = null;
        private TileBase[] inputTilemapTilesArray;
        private Queue<TileContainer> _queueOfTiles = new Queue<TileContainer>();
        private int _width = 0, _height = 0;
        private Tilemap _inputTilemap;
        
        private BoundsInt InputTileMapBounds => _inputTilemap.cellBounds;
        public Queue<TileContainer> QueueOfTiles
        {
            get => _queueOfTiles;
            set => _queueOfTiles = value;
        }

        public int Width => _width;

        public int Height => _height;

        public InputImageParameters(Tilemap inputTilemap)
        {
            _inputTilemap = inputTilemap;
            inputTilemapTilesArray = _inputTilemap.GetTilesBlock(this.InputTileMapBounds);
            ExtractNotEmptyTiles();
            VerifyInputTiles();
        }

        private void VerifyInputTiles()
        {
            if (topRightTile == null || bottomLeftTile == null)
                throw new Exception("WFC: Input tilemap is empty!");
            int minX = bottomLeftTile.Value.x;
            int maxX = topRightTile.Value.x;
            int minY = bottomLeftTile.Value.y;
            int maxY = topRightTile.Value.y;

            _width = Math.Abs(maxX - minX) + 1;
            _height = Math.Abs(maxY - minY) + 1;

            int tileCount = _width * _height;
            if (_queueOfTiles.Count != tileCount)
            {
                throw new Exception("WFC: Tilemaps has empty fields!");
            }

            if (_queueOfTiles.Any(tile => tile.X > maxX || tile.X < minX || tile.Y > maxY || tile.Y < minY))
            {
                throw new Exception("WFC: Input tilemap should be a filled rectangle!");
            }
        }

        private void ExtractNotEmptyTiles()
        {
            for (int row = 0; row< InputTileMapBounds.size.y; row++)
            {
                for (int col = 0; col < InputTileMapBounds.size.x; col++)
                {
                    int index = col + (row * InputTileMapBounds.size.x);

                    TileBase tile = inputTilemapTilesArray[index];
                    if (bottomLeftTile == null && tile != null)
                    {
                        bottomLeftTile = new Vector2Int(col, row);
                    }

                    if (tile != null)
                    {
                        _queueOfTiles.Enqueue(new TileContainer(tile,col,row));
                        topRightTile = new Vector2Int(col, row);
                    }
                }
            }
        }
    }
}