using System.Collections;
using System.Collections.Generic;
using QuickGraph;
using UnityEngine;

public class SmartRoomGenerator : MonoBehaviour
{
    [Header("Room Settings"),SerializeField] 
    private List<RoomParamsSO> roomParams;
    [SerializeField, Range(0,10)]
    private int roomMargin = 1;
    [SerializeField] private bool smoothIndividualRooms;

    public List<Room> Rooms { get; private set; }

    public void Generate(BidirectionalGraph<RoomNode, Edge<RoomNode>> graph)
    {
        GenerateRooms(graph);
        
        if(smoothIndividualRooms)
            SmoothRooms();
        
        
    }
    
    private void GenerateRooms(BidirectionalGraph<RoomNode, Edge<RoomNode>> graph)
    {
        Rooms = new List<Room>();
        
        Vector2Int currentRoomOrigin = Vector2Int.zero;
        foreach (var vertex in graph.Vertices)
        {
            
            RoomParamsSO param =  roomParams.Find((param) => param.Type == vertex.RoomType);
            HashSet<Vector2Int> roomTiles = GenerateRoomTiles(param);
            Room room = new Room(roomTiles, param, vertex);
            currentRoomOrigin.x += (room.RoomParams.RoomMaxWidth / 2 + roomMargin);
            room.Origin = new Vector2Int(currentRoomOrigin.x,currentRoomOrigin.y);
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
            if (position.x >=  -(roomParams.RoomMaxWidth/2) && position.x <= (roomParams.RoomMaxWidth / 2)  &&
                position.y >= -(roomParams.RoomMaxHeight/2) && position.y <= (roomParams.RoomMaxHeight /2)
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
            if(!room.RoomParams.ApplySmoothing) continue;
            bool[,] floorMatrix = AbstractDungeonGenerator.ConvertVectorEnumerableToBoolArray(room.Tiles, room.Bounds.width, room.Bounds.height,
                room.Bounds.min);
            ProceduralGenerationAlgorithms.CellularAutomaton(floorMatrix, 
                room.RoomParams.CellAutIterations, room.RoomParams.CelAutThreshold);
            room.UpdateTiles(new HashSet<Vector2Int>(AbstractDungeonGenerator.ConvertBoolArrayToVectorList(floorMatrix, room.Bounds.min)));
        }
    }
}
