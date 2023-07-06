using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{

    [SerializeField] protected SimpleRandomWalkSO randomWalkParams;

    
    
    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParams, startPosition);
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions,tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.Iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.WalkLength);
            floorPositions.UnionWith(path);
            if (parameters.StartRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }

        }
        return floorPositions;
    }
}
