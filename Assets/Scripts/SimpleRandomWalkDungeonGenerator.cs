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
        dungeonVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions,dungeonVisualizer);
    }

    
}
