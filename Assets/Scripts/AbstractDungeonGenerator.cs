using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [FormerlySerializedAs("tilemapVisualizer")] [SerializeField] protected DungeonVisualizer dungeonVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        dungeonVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
    
    public static HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
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
    
    public static List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
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
                Vector2Int newCorridorTileOffset = Direction2D.GetDirection90DegFrom(directionFromCell);
                newCorridor.Add(corridor[i-1]);
                newCorridor.Add(corridor[i-1] + newCorridorTileOffset);
            }
        }

        return newCorridor;
    }
    
}
