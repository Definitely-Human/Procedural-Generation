using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLenght)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLenght; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLenght)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLenght; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }

        return corridor;
    }

    public static void CellularAutomaton(bool[,] originalGrid, int iterations, int threshold = 4)
    {
        for (int it = 0; it < iterations; it++)
        {
            bool[,] tempGrid = (bool[,])originalGrid.Clone();
            for (int i = 0; i < originalGrid.GetLength(0); i++)
            {
                for (int j = 0; j < originalGrid.GetLength(1); j++)
                {
                    int neighbourWallCount = 0;
                    for (int y = i - 1; y <= i+1; y++)
                    {
                        for (int x = j - 1; x <= j+1; x++)
                        {
                            if (IsInsideGrid(originalGrid, x, y))
                            {
                                if (y != i || x != j)
                                {
                                    if (!tempGrid[y, x])
                                        neighbourWallCount++;
                                }
                            }
                            else
                            {
                                neighbourWallCount++;
                            }
                        }
                    }
                    if (neighbourWallCount > threshold)
                    {
                        originalGrid[i, j] = false;
                    }
                    else
                    {
                        originalGrid[i, j] = true;
                    }
                }
            }
            
        }
        
    }

    private static bool IsInsideGrid(bool[,] originalGrid, int x, int y)
    {
        return (x >= 0 && y >= 0) && (x < originalGrid.GetLength(1) && y < originalGrid.GetLength(0));
    }

    public static List<RectInt> BinarySpacePartitioning(RectInt spaceToSplit, List<RoomParamsSO> roomParams, int roomMargin)
    {
        roomMargin *= 2;
        List<RoomParamsSO> roomsLeft = new List<RoomParamsSO>(roomParams);
        Queue<RectInt> roomQueue = new Queue<RectInt>();
        List<RectInt> roomsList = new List<RectInt>();
        roomQueue.Enqueue(spaceToSplit);
        while (roomQueue.Count > 0 && roomsLeft.Count > 0)
        {
            RectInt room = roomQueue.Dequeue();
            bool roomAdded = false;
            for (int i = 0; i < roomsLeft.Count; i++)
            {
                if (room.size.y >= roomsLeft[i].RoomMinHeight + roomMargin && room.size.x >= roomsLeft[i].RoomMinWidth + roomMargin &&
                    room.size.y <= roomsLeft[i].RoomMaxHeight + roomMargin && room.size.x <= roomsLeft[i].RoomMaxWidth + roomMargin)
                {
                    roomsList.Add(room);
                    roomsLeft.RemoveAt(i);
                    roomAdded = true;
                    break;
                }
            }

            if (roomAdded) continue;
            int minWidth = Int32.MaxValue, minHeight = Int32.MaxValue;
            foreach (var roomParam in roomsLeft)
            {
                if (roomParam.RoomMinWidth < minWidth) minWidth = roomParam.RoomMinWidth;
                if (roomParam.RoomMinHeight < minHeight) minHeight = roomParam.RoomMinHeight;
            }

            minWidth += roomMargin;
            minHeight += roomMargin;

            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {

                        SplitVertically(minWidth, roomQueue, room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {

                        SplitVertically(minWidth, roomQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);
                    }
                }
            }
        }

        return roomsList;
    }

    public static List<RectInt> BinarySpacePartitioning(RectInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<RectInt> roomQueue = new Queue<RectInt>();
        List<RectInt> roomsList = new List<RectInt>();
        roomQueue.Enqueue(spaceToSplit);
        while (roomQueue.Count > 0)
        {
            var room = roomQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);
                    }
                    else if(room.size.x >= minWidth * 2)
                    {
                        
                        SplitVertically( minWidth, roomQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if(room.size.x >= minWidth * 2)
                    {
                        
                        SplitVertically( minWidth, roomQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<RectInt> roomQueue, RectInt room)
    {
        int xSplit = Random.Range(1, room.size.x);
        RectInt room1 = new RectInt(room.min, new Vector2Int(xSplit, room.size.y));
        RectInt room2 = new RectInt(new Vector2Int(room.min.x + xSplit, room.min.y),
            new Vector2Int(room.size.x-xSplit,room.size.y));
        roomQueue.Enqueue(room1);
        roomQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<RectInt> roomQueue, RectInt room)
    {
        int ySplit = Random.Range(1, room.size.y);
        RectInt room1 = new RectInt(room.min, new Vector2Int(room.size.x, ySplit));
        RectInt room2 = new RectInt(new Vector2Int(room.min.x, room.min.y + ySplit),
            new Vector2Int(room.size.x,room.size.y-ySplit));
        roomQueue.Enqueue(room1);
        roomQueue.Enqueue(room2);
    }
}