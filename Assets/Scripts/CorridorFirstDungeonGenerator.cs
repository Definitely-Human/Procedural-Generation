using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14;
    [SerializeField]
    private int corridorCount = 5;

    [SerializeField,Range(0.05f,1f)]
    private float roomPercent;
    
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
        
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        //HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        //List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        //CreateRoomsAtDeadEnds(deadEnds, roomPositions);

        //floorPositions.UnionWith(roomPositions);

        for (int i = 0; i < corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }
            

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previewDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previewDirection != Vector2Int.zero &&
                directionFromCell != previewDirection)
            {//handle corners
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previewDirection = directionFromCell;
            }
            else
            {
                Vector2Int newCorridorTileOffset = GetDirection90DegFrom(directionFromCell);
                newCorridor.Add(corridor[i-1]);
                newCorridor.Add(corridor[i-1] + newCorridorTileOffset);
            }
        }

        return newCorridor;
    }

    private Vector2Int GetDirection90DegFrom(Vector2Int direction)
    {
        if(direction == Vector2Int.up)
            return Vector2Int.right;
        if (direction == Vector2Int.right) 
            return Vector2Int.down;
        if(direction == Vector2Int.down)
            return Vector2Int.left;
        if(direction == Vector2Int.left)
            return Vector2Int.up;
        return Vector2Int.zero; // Possibly throw index out of range exception.
    }

    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (!roomFloors.Contains(position))
            {
                var room = RunRandomWalk(randomWalkParams, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighborsCount = 0;
            foreach (var direction in Direction2D.CardinalDirections)
            {
                if (floorPositions.Contains(position + direction))
                    neighborsCount++;
                
            }

            if (neighborsCount == 1)
                deadEnds.Add(position);
        }

        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParams, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        Vector2Int currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount ; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition,corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[^1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
        return corridors;
    }
}
