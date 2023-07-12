using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : IComparable<Room>
{
    public int RoomSize => Tiles.Count;
    public HashSet<Vector2Int> Tiles { get; private set; }

    public List<Vector2Int> EdgeTiles { get; private set; }

    private List<Corridor> Corridors { get; set; }

    public RoomNode RoomNode { get; private set; }
    
    public RoomParamsSO RoomParams { get; private set; }
    
    public RectInt Bounds { get; private set; }
    
    public Vector2Int Origin { get; set; }

    public Room(HashSet<Vector2Int> tiles, RoomParamsSO roomParams)
    {
        Corridors = new List<Corridor>();
        RoomParams = roomParams;
        
        UpdateTiles(tiles);
    }

    public void UpdateTiles(HashSet<Vector2Int> tiles)
    {
        Tiles = tiles;
        CalculateEdgeTiles();
        Bounds = CalculateRoomBounds(tiles);
    }
    
    public Room(HashSet<Vector2Int> tiles, RoomParamsSO roomParams, RoomNode roomNode) :this(tiles, roomParams)
    {
        RoomNode = roomNode;
    }

    private void CalculateEdgeTiles()
    {
        EdgeTiles = new List<Vector2Int>();
        foreach (var tile in Tiles)
        {
            foreach (var direction in Direction2D.CardinalDirections)
            {
                if (!Tiles.Contains(tile + direction))
                {
                    EdgeTiles.Add(tile);
                    break;
                }
            }
        }
    }

    public void AddCorridor(Corridor corridor)
    {
        Corridors.Add(corridor);
    }

    public bool IsConnected(Room room)
    {
        return Corridors.Exists((corridor) => corridor.ConnectedRooms.Contains(room));
    }
    
    public int CompareTo(Room other)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return RoomNode.ToString();
    }

    public static RectInt CalculateRoomBounds(IEnumerable<Vector2Int> tiles)
    {
        int maxTileX = int.MinValue;
        int minTileX = int.MaxValue;
        int maxTileY = int.MinValue;
        int minTileY = int.MaxValue;
        foreach (var tile in tiles)
        {
            if (tile.x > maxTileX) maxTileX = tile.x;
            if (tile.x < minTileX) minTileX = tile.x;
            if (tile.y > maxTileY) maxTileY = tile.y;
            if (tile.y < minTileY) minTileY = tile.y;
        }

        return new RectInt(minTileX, minTileY, maxTileX - minTileX, maxTileY - minTileY);
    }
}
