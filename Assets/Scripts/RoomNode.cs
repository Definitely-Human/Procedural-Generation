using System.Collections;
using System.Collections.Generic;
using QuickGraph;
using UnityEditor;
using UnityEngine;

public class RoomNode
{
    private RoomType _roomType;
    private GUID _guid;

    public RoomType RoomType => _roomType;

    public GUID Guid => _guid;

    public RoomNode(RoomType roomType)
    {
        _roomType = roomType;
        _guid = GUID.Generate();
    }

    public override string ToString()
    {
        return _roomType + " " + Guid.ToString().Substring(0,6);
    }
}

public enum RoomType
{
    Entrance,
    Safe,
    EnemyMid,
    EnemyLarge,
    EnemyGiant,
    Boss,
}
