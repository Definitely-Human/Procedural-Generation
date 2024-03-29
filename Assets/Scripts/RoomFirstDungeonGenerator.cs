using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : AbstractDungeonGenerator
{
    [Header("Dungeon Parameters")]
    [SerializeField] 
    private int dungeonWidth = 100;
    [SerializeField]
    private int dungeonHeight = 100;

    [Header("Room Parameters")] 
    [SerializeField]
    private bool differentSizedRooms;

    [SerializeField] private List<BasicRoomParamsSO> roomParams;

    [Space, Space]
    [SerializeField] protected RoomParamsSO randomWalkParams;
    
    [SerializeField]
    private int minRoomWidth = 10;
    [SerializeField]
    private int minRoomHeight = 10;

    [SerializeField, Range(0,10)]
    private int roomMargin = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    
    [Header("Smoothing Pass")]
    [SerializeField, Range(0,25)] private int cellAutIterations = 0;
    [SerializeField, Range(3,5)] private int celAutThreshold = 4;
    
    [Header("Debug")]
    [SerializeField] private bool visualizeRoomBounds = false;
    [SerializeField] private RoomBoundsVisualizer roomBoundsVisualizer;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        List<RectInt> roomList;
        RectInt spaceToSplit = new RectInt(startPosition,
            new Vector2Int(dungeonWidth,dungeonHeight));
        if (differentSizedRooms)
        {
            roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(spaceToSplit,roomParams,roomMargin);
        }
        else
        {
            roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(spaceToSplit,minRoomWidth,minRoomHeight);
        }
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();


        if (randomWalkRooms)
        {
            floor = CreateRandomWalkRooms(roomList);
        }
        else
        {
            floor = CreateSimpleRooms(roomList);
        }

        CreateCorridors(roomList, floor);

        floor = SmoothPass(floor);

        dungeonVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, dungeonVisualizer);
        
        
        roomBoundsVisualizer.Clear();
        if (visualizeRoomBounds)
        {
            roomBoundsVisualizer.VisualizeRoomBounds(roomList);
        }
    }

    private void CreateCorridors(List<RectInt> roomList, HashSet<Vector2Int> floor)
    {
        List<Vector2Int> roomCenters = FindRoomCenters(roomList);

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        corridors = new HashSet<Vector2Int>(IncreaseCorridorSizeByOne(corridors.ToList()));
        floor.UnionWith(corridors);
    }

    private HashSet<Vector2Int> SmoothPass(HashSet<Vector2Int> floor)
    {
        bool[,] floorMatrix = ConvertVectorEnumerableToBoolArray(floor, dungeonWidth, dungeonHeight);
        ProceduralGenerationAlgorithms.CellularAutomaton(floorMatrix, cellAutIterations, celAutThreshold);
        return new HashSet<Vector2Int>(ConvertBoolArrayToVectorList(floorMatrix));
    }

    private static List<Vector2Int> FindRoomCenters(List<RectInt> roomList)
    {
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenters.Add(Vector2Int.RoundToInt(room.center));
        }

        return roomCenters;
    }

    private HashSet<Vector2Int> CreateRandomWalkRooms(List<RectInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i<roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x),Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParams, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + roomMargin) && position.x <= (roomBounds.xMax - roomMargin) &&
                    position.y >= (roomBounds.yMin + roomMargin) && position.y <= (roomBounds.yMax - roomMargin)
                    )
                {
                    floor.Add(position);
                }
            }
        }

        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count> 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int origin, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = origin;
        corridor.Add(position);
        
        while (position.y != destination.y)
        {
            if (position.y < destination.y)
                position += Vector2Int.up;
            else if(position.y > destination.y)
                position += Vector2Int.down;
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (position.x < destination.x)
                position += Vector2Int.right;
            else if(position.x > destination.x)
                position += Vector2Int.left;
            corridor.Add(position);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        if (roomCenters.Count < 1) throw new ArgumentException();
        Vector2Int closest = Vector2Int.zero;
        float closestDistance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2Int.Distance(position, currentRoomCenter);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closest = position;
            }
        }
        
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<RectInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = roomMargin;col < room.size.x - roomMargin; col++)
            {
                for (int row = roomMargin;row < room.size.y - roomMargin; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col,row);
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
}
