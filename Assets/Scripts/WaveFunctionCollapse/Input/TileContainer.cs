using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class TileContainer
    {
        public TileBase Tile { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public TileContainer(TileBase tile, int x, int y)
        {
            Tile = tile;
            X = x;
            Y = y;
        }
    }
}