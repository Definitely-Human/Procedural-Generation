using System;
using System.Collections;
using System.Collections.Generic;
using QuickGraph;
using UnityEngine;

public class SmartCorridorGenerator : MonoBehaviour
{
    public List<Corridor> Corridors { get; private set; }
    [Header("Corridor Settings"),SerializeField, Range(1,5)] 
    private int corridorRadius = 1;

    public void GenerateCorridors(AbstractGraphGenerator graphGenerator, List<Room> rooms)
    {
        Corridors = new List<Corridor>();
        foreach (Edge<RoomNode> edge in graphGenerator.DungeonGraph.Edges)
        {
            var roomA = rooms.Find((room)=>room.RoomNode == edge.Source);
            var roomB = rooms.Find((room)=>room.RoomNode == edge.Target);
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

        List<Vector2Int> corridorLine = TileTools.GetLine(bestTileA, bestTileB);
        HashSet<Vector2Int> corridorTiles = new HashSet<Vector2Int>();
        
        Vector2Int corridorOrigin = bestTileA;
        
        foreach (Vector2Int tile in corridorLine)
        {
            corridorTiles.UnionWith(TileTools.DrawCircle(tile - corridorOrigin,corridorRadius)); 
            // Subtract corridor origin from tile to make position relative to origin.
        }

        Corridor corridor = new Corridor(roomA, roomB, corridorTiles);
        corridor.Origin = corridorOrigin;
        Corridors.Add(corridor);
    }
}
