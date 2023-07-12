using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SmartDungeonGenerator : AbstractDungeonGenerator
{
    private BidirectionalGraph<RoomNode, Edge<RoomNode>> _dungeonGraph;
    private List<Room> _rooms;
    private List<Corridor> _corridors;
    private List<Vector2Int> _dungeonFloorTiles;
    private RectInt _dungeonSize;
    
    [SerializeField] private List<RoomParamsSO> roomParams;
    [SerializeField, Range(0,10)]
    private int roomMargin = 1;
    
    [Range(3,10), SerializeField] private int minDungeonLenght;
    [Range(4,10), SerializeField] private int maxDungeonLength;

    [Header("Smoothing Pass")] 
    [SerializeField] private bool smoothIndividualRooms;
    [SerializeField] private bool applyDungeonSmoothing;
    [SerializeField, Range(0,25)] private int cellAutIterations = 0;
    [SerializeField, Range(3,5)] private int celAutThreshold = 4;
    
    [SerializeField, Range(1,5)] private int corridorRadius = 1;

    protected override void RunProceduralGeneration()
    {
        CreateAbstraction();
        GenerateRooms();

        
        if(smoothIndividualRooms && cellAutIterations > 0)
            SmoothRooms();

        GenerateCorridors();
        
        CollectDungeon();
        
        CalculateDungeonSize();
        
        
        if(applyDungeonSmoothing && cellAutIterations > 0)
            _dungeonFloorTiles = SmoothDungeon(_dungeonFloorTiles);

        
        dungeonVisualizer.PaintFloorTiles(_dungeonFloorTiles);
        WallGenerator.CreateWalls(new HashSet<Vector2Int>(_dungeonFloorTiles), dungeonVisualizer);
    }

    private void GenerateCorridors()
    {
        _corridors = new List<Corridor>();
        foreach (Edge<RoomNode> edge in _dungeonGraph.Edges)
        {
            var roomA = _rooms.Find((room)=>room.RoomNode == edge.Source);
            var roomB = _rooms.Find((room)=>room.RoomNode == edge.Target);
            if(!roomA.IsConnected(roomB))
                CreateCorridor(roomA,roomB);
        }
    }
    

    private void CreateCorridor(Room roomA, Room roomB)
    {
        int bestDistance = Int32.MaxValue;
        Vector2Int bestTileA = new Vector2Int();
        Vector2Int bestTileB = new Vector2Int();
        for (int i = 0; i < roomA.EdgeTiles.Count; i++)
        {
            Vector2Int tileA = roomA.EdgeTiles[i] + roomA.Origin; 
            for (int j = 0; j < roomB.EdgeTiles.Count; j++)
            {
                Vector2Int tileB = roomB.EdgeTiles[j] + roomB.Origin;
                int distanceBetweenRooms = (int)(Mathf.Pow(tileA.x - tileB.x, 2) + Mathf.Pow(tileA.y - tileB.y,2));
                if (distanceBetweenRooms < bestDistance)
                {
                    bestDistance = distanceBetweenRooms;
                    bestTileA = tileA;
                    bestTileB = tileB;
                }
                        
            }
        }

        List<Vector2Int> corridorLine = GetLine(bestTileA, bestTileB);
        HashSet<Vector2Int> corridorTiles = new HashSet<Vector2Int>();
        
        Vector2Int corridorOrigin = bestTileA;
        
        foreach (Vector2Int tile in corridorLine)
        {
            corridorTiles.UnionWith(DrawCircle(tile - corridorOrigin,corridorRadius)); 
            // Subtract corridor origin from tile to make position relative to origin.
        }

        Corridor corridor = new Corridor(roomA, roomB, corridorTiles);
        corridor.Origin = corridorOrigin;
        _corridors.Add(corridor);
    }

    private HashSet<Vector2Int> DrawCircle(Vector2Int center, int radius)
    {
        HashSet<Vector2Int> circle = new HashSet<Vector2Int>();

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y < radius * radius)
                {
                    int drawX = center.x + x;
                    int drawY = center.y + y;
                    circle.Add(new Vector2Int(drawX, drawY));
                }
            }
        }

        return circle;
    }

    List<Vector2Int> GetLine(Vector2Int from, Vector2Int to) {
        List<Vector2Int> line = new List<Vector2Int> ();

        int x = from.x;
        int y = from.y;

        int dx = to.x - from.x;
        int dy = to.y - from.y;

        bool inverted = false;
        int step = Math.Sign (dx);
        int gradientStep = Math.Sign (dy);

        int longest = Mathf.Abs (dx);
        int shortest = Mathf.Abs (dy);

        if (longest < shortest) {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign (dy);
            gradientStep = Math.Sign (dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i =0; i < longest; i ++) {
            line.Add(new Vector2Int(x,y));

            if (inverted) {
                y += step;
            }
            else {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest) {
                if (inverted) {
                    x += gradientStep;
                }
                else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    private void CalculateDungeonSize()
    {
        _dungeonSize = Room.CalculateRoomBounds(_dungeonFloorTiles);
    }

    private List<Vector2Int> SmoothDungeon(List<Vector2Int> floor)
    {
        bool[,] floorMatrix = ConvertVectorEnumerableToBoolArray(_dungeonFloorTiles, _dungeonSize.width, _dungeonSize.height,
            _dungeonSize.min);
        ProceduralGenerationAlgorithms.CellularAutomaton(floorMatrix, cellAutIterations, celAutThreshold);
        return ConvertBoolArrayToVectorList(floorMatrix, _dungeonSize.min);
    }

    private void SmoothRooms()
    {
        foreach (Room room in _rooms)
        {
            if(!room.RoomParams.ApplySmoothing) continue;
            bool[,] floorMatrix = ConvertVectorEnumerableToBoolArray(room.Tiles, room.Bounds.width, room.Bounds.height,
                room.Bounds.min);
            ProceduralGenerationAlgorithms.CellularAutomaton(floorMatrix, 
                room.RoomParams.CellAutIterations, room.RoomParams.CelAutThreshold);
            room.UpdateTiles(new HashSet<Vector2Int>(ConvertBoolArrayToVectorList(floorMatrix, room.Bounds.min)));
        }
    }

    private void CreateAbstraction()
    {
        _dungeonGraph = new BidirectionalGraph<RoomNode, Edge<RoomNode>>();
        
        var entryRoom = new RoomNode(RoomType.Entrance);
        _dungeonGraph.AddVertex(entryRoom);
        

        int dungeonLength = Random.Range(minDungeonLenght, maxDungeonLength + 1);

        List<RoomType> generetableRooms = new List<RoomType>() { 
            RoomType.Safe, RoomType.EnemyGiant,RoomType.EnemyLarge,RoomType.EnemyMid };
        for (int i = 0; i < dungeonLength - 2; i++)
        {
            RoomType roomType = generetableRooms[Random.Range(0, generetableRooms.Count)];
            RoomNode room = new RoomNode(roomType);
            Edge<RoomNode> connection = new Edge<RoomNode>(_dungeonGraph.Vertices.Last(), room);
            _dungeonGraph.AddVertex(room);
            _dungeonGraph.AddEdge(connection);
        }

        var bossRoom = new RoomNode(RoomType.Boss);
        Edge<RoomNode> bossConnection = new Edge<RoomNode>(_dungeonGraph.Vertices.Last(), bossRoom);
        _dungeonGraph.AddVertex(bossRoom);
        _dungeonGraph.AddEdge(bossConnection);
    }

    private void GenerateRooms()
    {
        _rooms = new List<Room>();
        
        Vector2Int currentRoomOrigin = Vector2Int.zero;
        foreach (var vertex in _dungeonGraph.Vertices)
        {
            
            RoomParamsSO param =  roomParams.Find((param) => param.Type == vertex.RoomType);
            HashSet<Vector2Int> roomTiles = GenerateRoomTiles(param);
            Room room = new Room(roomTiles, param, vertex);
            currentRoomOrigin.x += (room.RoomParams.RoomMaxWidth / 2 + roomMargin);
            room.Origin = new Vector2Int(currentRoomOrigin.x,currentRoomOrigin.y);
            currentRoomOrigin.x += (room.RoomParams.RoomMaxWidth / 2 + roomMargin);
            _rooms.Add(room);
            
        }

        
    }

    private HashSet<Vector2Int> GenerateRoomTiles(RoomParamsSO roomParams)
    {
        HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();

        var roomCenter = new Vector2Int(0, 0);
        var roomFloor = RunRandomWalk(roomParams, roomCenter);
        foreach (var position in roomFloor)
        {
            if (position.x >=  -(roomParams.RoomMaxWidth/2) && position.x <= (roomParams.RoomMaxWidth / 2)  &&
                position.y >= -(roomParams.RoomMaxHeight/2) && position.y <= (roomParams.RoomMaxHeight /2)
               )
            {
                tiles.Add(position);
            }
        }
        
        return tiles;
    }
    
    private void CollectDungeon()
    {
        _dungeonFloorTiles = new List<Vector2Int>();
        foreach (var room in _rooms)
        {
            foreach (var tile in room.Tiles)
            {
                _dungeonFloorTiles.Add(tile + room.Origin);
            }

        }

        foreach (var corridor in _corridors)
        {
            foreach (var tile in corridor.Tiles)
            {
                _dungeonFloorTiles.Add(tile + corridor.Origin);
            }
        }
    }
}
