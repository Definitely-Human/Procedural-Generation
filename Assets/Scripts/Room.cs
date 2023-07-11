using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : IComparable<Room>
{
    public int RoomSize => Tiles.Count;
    public HashSet<Vector2Int> Tiles { get; private set; }

    public HashSet<Vector2Int> EdgeTiles { get; private set; }

    public List<Room> ConnectedRooms { get; private set; }

    public Room(HashSet<Vector2Int> tiles)
    {
        Tiles = tiles;
        ConnectedRooms = new List<Room>();
        
        
        
        EdgeTiles = new HashSet<Vector2Int>();
        foreach (var tile in tiles)
        {
            foreach (var direction in Direction2D.CardinalDirections)
            {
                if (!tiles.Contains(tile + direction))
                {
                    EdgeTiles.Add(tile);
                    break;
                }
            }
        }
    }

    public static void ConnectRooms(Room roomA, Room roomB)
    {
        roomA.ConnectedRooms.Add(roomB);
        roomB.ConnectedRooms.Add(roomA);
    }

    public bool IsConnected(Room room)
    {
        return ConnectedRooms.Contains(room);
    }
    
    public int CompareTo(Room other)
    {
        throw new NotImplementedException();
    }
}
