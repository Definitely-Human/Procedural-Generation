using System.Collections;
using System.Collections.Generic;
using QuickGraph;
using UnityEngine;

public class RoomNode
{
    private RoomType _roomType;

    public RoomType RoomType => _roomType;

    public RoomNode(RoomType roomType)
    {
        _roomType = roomType;
    }
}

public enum RoomType
{
    Entrance,
    Enemy,
    Boss,
}