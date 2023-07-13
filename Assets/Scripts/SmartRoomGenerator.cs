using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using UnityEngine;

public class SmartRoomGenerator : MonoBehaviour
{
    public List<Room> Rooms { get; private set; }

    [SerializeField] private List<RoomParamsSO> roomParams;
    [SerializeField, Range(0, 10)] private int roomMargin = 1;
    [SerializeField] private bool smoothIndividualRooms;


    public void Generate(BidirectionalGraph<RoomNode, Edge<RoomNode>> graph)
    {
        GenerateRooms(graph);

        if (smoothIndividualRooms)
            SmoothRooms();

        RemoveDisconnectedRegions();
        // RemoveDisconnectedRegionsAndWalls();
    }

    private void GenerateRooms(BidirectionalGraph<RoomNode, Edge<RoomNode>> graph)
    {
        Rooms = new List<Room>();

        Vector2Int currentRoomOrigin = Vector2Int.zero;
        foreach (var vertex in graph.Vertices)
        {

            RoomParamsSO param = roomParams.Find((param) => param.Type == vertex.RoomType);
            HashSet<Vector2Int> roomTiles = GenerateRoomTiles(param);
            Room room = new Room(roomTiles, param, vertex);
            currentRoomOrigin.x += (room.RoomParams.RoomMaxWidth / 2 + roomMargin);
            room.Origin = new Vector2Int(currentRoomOrigin.x, currentRoomOrigin.y);
            currentRoomOrigin.x += (room.RoomParams.RoomMaxWidth / 2 + roomMargin);
            Rooms.Add(room);

        }


    }

    private HashSet<Vector2Int> GenerateRoomTiles(RoomParamsSO roomParams)
    {
        HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();

        var roomCenter = new Vector2Int(0, 0);
        var roomFloor = AbstractDungeonGenerator.RunRandomWalk(roomParams, roomCenter);
        foreach (var position in roomFloor)
        {
            if (position.x >= -(roomParams.RoomMaxWidth / 2) && position.x <= (roomParams.RoomMaxWidth / 2) &&
                position.y >= -(roomParams.RoomMaxHeight / 2) && position.y <= (roomParams.RoomMaxHeight / 2)
               )
            {
                tiles.Add(position);
            }
        }

        return tiles;
    }

    private void SmoothRooms()
    {
        foreach (Room room in Rooms)
        {
            if (!room.RoomParams.ApplySmoothing) continue;
            bool[,] floorMatrix = AbstractDungeonGenerator.ConvertVectorEnumerableToBoolArray(room.Tiles,
                room.Bounds.width, room.Bounds.height,
                room.Bounds.min);
            ProceduralGenerationAlgorithms.CellularAutomaton(floorMatrix,
                room.RoomParams.CellAutIterations, room.RoomParams.CelAutThreshold);
            room.UpdateTiles(
                new HashSet<Vector2Int>(
                    AbstractDungeonGenerator.ConvertBoolArrayToVectorList(floorMatrix, room.Bounds.min)));
        }
    }

    private void RemoveDisconnectedRegions()
    {
        foreach (Room room in Rooms)
        {
            List<Vector2Int> tilesToCheck = new List<Vector2Int>(room.Tiles);
            List<HashSet<Vector2Int>> regions = new List<HashSet<Vector2Int>>();
            while (tilesToCheck.Count > 0)
            {
                HashSet<Vector2Int> region = new HashSet<Vector2Int>();
                Queue<Vector2Int> queue = new Queue<Vector2Int>();
                queue.Enqueue(tilesToCheck[0]);
                tilesToCheck.RemoveAt(0);

                while (queue.Count > 0)
                {
                    Vector2Int tile = queue.Dequeue();
                    region.Add(tile);
                    foreach (var direction in Direction2D.CardinalDirections)
                    {
                        Vector2Int checkedTile = tile + direction;
                        if (tilesToCheck.Contains(checkedTile))
                        {
                            tilesToCheck.Remove(checkedTile);
                            queue.Enqueue(checkedTile);
                        }
                    }
                }

                regions.Add(region);

            }

            regions.Sort((regA, regB) => -regA.Count.CompareTo(regB.Count));
            room.UpdateTiles(regions[0]); // Change room tiles to largest region found.
            // regions.ForEach((reg)=> Debug.Log(reg.Count));
            // Debug.Log(".");
        }

    }

    private void RemoveDisconnectedRegionsAndWalls()
    {
        foreach (var room in Rooms)
        {
            bool[,] map = AbstractDungeonGenerator.ConvertVectorEnumerableToBoolArray(room.Tiles, room.Bounds.width,
                room.Bounds.height, room.Bounds.min);
            List<List<Vector2Int>> wallRegions = GetRegions(false, room.Bounds.width,room.Bounds.height,map); 
            wallRegions.Sort((regA, regB) => -regA.Count.CompareTo(regB.Count));
            Debug.Log(wallRegions.Count);
            // for (int i = 5; i < wallRegions.Count; i++)
            // {
            //     room.Tiles.UnionWith(wallRegions[i]);
            // }
            // room.UpdateTiles(room.Tiles);
        }
    }


    List<List<Vector2Int>> GetRegions(bool tileType, int width, int height, bool[,] map) {
        List<List<Vector2Int>> regions = new List<List<Vector2Int>> ();
        int[,] mapFlags = new int[width,height];

        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y ++) {
                if (mapFlags[x,y] == 0 && map[x,y] == tileType) {
                    List<Vector2Int> newRegion = GetRegionTiles(x,y,width,height,map);
                    regions.Add(newRegion);

                    foreach (Vector2Int tile in newRegion) {
                        mapFlags[tile.x, tile.y] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Vector2Int> GetRegionTiles(int startX, int startY, int width, int height, bool[,] map) {
        List<Vector2Int> tiles = new List<Vector2Int> ();
        int[,] mapFlags = new int[width,height];
        bool tileType = map [startX, startY];

        Queue<Vector2Int> queue = new Queue<Vector2Int> ();
        queue.Enqueue (new Vector2Int (startX, startY));
        mapFlags [startX, startY] = 1;

        while (queue.Count > 0) {
            Vector2Int tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.x - 1; x <= tile.x + 1; x++) {
                for (int y = tile.y - 1; y <= tile.y + 1; y++) {
                    if (IsInMapRange(x,y,width,height) && (y == tile.y || x == tile.x)) {
                        if (mapFlags[x,y] == 0 && map[x,y] == tileType) {
                            mapFlags[x,y] = 1;
                            queue.Enqueue(new Vector2Int(x,y));
                        }
                    }
                }
            }
        }

        return tiles;
    }
    
    bool IsInMapRange(int x, int y, int width, int height) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
