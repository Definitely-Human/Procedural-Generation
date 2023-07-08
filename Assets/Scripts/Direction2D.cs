using System.Collections.Generic;
using UnityEngine;

public static class Direction2D
{
    public static readonly List<Vector2Int> CardinalDirections = new ()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
    };
    
    public static readonly List<Vector2Int> DiagonalDirections = new ()
    {
        new Vector2Int(1,1), // Up-Right
        new Vector2Int(1,-1), // Down-Right
        new Vector2Int(-1,-1), // Down-Left
        new Vector2Int(-1,1), // Up-Left
    };
    
    public static readonly List<Vector2Int> EightDirections = new ()
    {
        Vector2Int.up,
        new Vector2Int(1,1), // Up-Right
        Vector2Int.right,
        new Vector2Int(1,-1), // Down-Right
        Vector2Int.down,
        new Vector2Int(-1,-1), // Down-Left
        Vector2Int.left,
        new Vector2Int(-1,1), // Up-Left
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return CardinalDirections[Random.Range(0, CardinalDirections.Count)];
    }
}