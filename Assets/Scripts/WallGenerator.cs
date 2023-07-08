using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallDirections(floorPositions, Direction2D.CardinalDirections);
        var cornerWallPositions = FindWallDirections(floorPositions, Direction2D.DiagonalDirections);
        CreateBasicWalls(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.EightDirections)
            {
                var neighbourPosition = position + direction;
                neighboursBinaryType += floorPositions.Contains(neighbourPosition) ? "1" : "0";
            }
            tilemapVisualizer.PaintSingleCornerWall(position,neighboursBinaryType);
        }
    }

    private static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, 
        HashSet<Vector2Int> basicWallPositions,HashSet<Vector2Int> floorPositions )
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.CardinalDirections)
            {
                var neighbourPosition = position + direction;
                neighboursBinaryType += floorPositions.Contains(neighbourPosition) ? "1" : "0";
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)        
        {
            foreach (var direction in directionsList)
            {
                var neighborPosition = position + direction;
                if (!floorPositions.Contains(neighborPosition))
                    wallPositions.Add(neighborPosition);
            }
        }

        return wallPositions;
    }
}
