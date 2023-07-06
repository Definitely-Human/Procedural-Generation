using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.min.y,room.min.z));
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

public static class Direction2D
{
    public static List<Vector2Int> CardinalDirections = new ()
    {
        Vector2Int.up,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.right,
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return CardinalDirections[Random.Range(0, CardinalDirections.Count)];
    }
}