using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static bool[,] ConvertVectorListToBoolArray(List<Vector2Int> tileList, int x, int y)
    {
        bool[,] tilemap = new bool[y,x];
        
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                tilemap[i, j] = false;  
            }
        }
        foreach (var tile in tileList)
        {
            tilemap[ tile.y, tile.x] = true;
        }
        
        return tilemap;
    }

    public static List<Vector2Int> ConvertBoolArrayToVectorList(bool[,] tileMap)
    {
        // for (int i = tileMap.GetLength(0) - 1; i >= 0; i--)
        // {
        //     string row = "";
        //     for (int j = 0; j < tileMap.GetLength(1); j++)
        //     {
        //         row += tileMap[i,j]?"1":"0";
        //     }
        //     Debug.Log(row);
        // }
        List<Vector2Int> tileList = new List<Vector2Int>();
        Debug.Log(tileMap.GetLength(0) + " " + tileMap.GetLength(1));
        for (int i = 0; i < tileMap.GetLength(0); i++)
        {
            for (int j = 0; j < tileMap.GetLength(1); j++)
            {
                if (tileMap[i, j])
                {
                    // Debug.Log(i + " " + j);
                    tileList.Add(new Vector2Int(i, j));
                }
            }
        }
        Debug.Log(tileMap.Length);

        return tileList;
    }


    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
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

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomQueue, BoundsInt room)
    {
        int xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y,room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x-xSplit,room.size.y,room.size.z));
        roomQueue.Enqueue(room1);
        roomQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomQueue, BoundsInt room)
    {
        int ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x,room.size.y-ySplit,room.size.z));
        roomQueue.Enqueue(room1);
        roomQueue.Enqueue(room2);
    }
}