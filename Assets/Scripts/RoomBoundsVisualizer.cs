using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomBoundsVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap roomBoundsTilemap;
    [SerializeField] private TileBase tileBase;
    
    public void VisualizeRoomBounds(List<RectInt> roomBounds)
    {
        foreach (RectInt roomBoundary in roomBounds)
        {
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.5f);
            //Debug.Log(roomBoundary.xMin + " " + roomBoundary.xMax + " " + roomBoundary.yMin + " " + roomBoundary.yMax);
            for (int i = roomBoundary.xMin; i < roomBoundary.xMax; i++)
            {
                for (int j = roomBoundary.yMin; j < roomBoundary.yMax; j++)
                {
                    var tilePosition = roomBoundsTilemap.WorldToCell(new Vector3Int(j,i));
                    roomBoundsTilemap.SetTile(tilePosition, tileBase);
                    roomBoundsTilemap.SetTileFlags(tilePosition, TileFlags.None);
                    roomBoundsTilemap.SetColor(tilePosition,color);
                }
            }
        }
    }

    public void Clear()
    {
        roomBoundsTilemap.ClearAllTiles();
    }
}
