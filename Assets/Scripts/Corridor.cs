using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Corridor
{
    public HashSet<Vector2Int> Tiles { get; private set; }
    public List<Room> ConnectedRooms { get; private set; }
    
    public Vector2Int Origin { get; set; }

    public Corridor(Room RoomA, Room RoomB, HashSet<Vector2Int> tiles)
    {
        ConnectedRooms = new List<Room>();
        ConnectedRooms.Add(RoomA);
        RoomA.AddCorridor(this);
        ConnectedRooms.Add(RoomB);
        RoomB.AddCorridor(this);
        Tiles = tiles;
    }

    public override string ToString()
    {
        return "Origin: " + Origin + "\nRooms:" + String.Join(" ", ConnectedRooms.Select(i => i.ToString())) + "\nLength:" + Tiles.Count;
    }
}
