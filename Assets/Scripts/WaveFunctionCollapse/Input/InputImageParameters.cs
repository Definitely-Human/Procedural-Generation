using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        private void ExtractNotEmptyTiles()
        {
            throw new System.NotImplementedException();
        }
    }
}