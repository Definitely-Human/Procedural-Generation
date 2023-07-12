using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected DungeonVisualizer dungeonVisualizer;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;
    [SerializeField] protected bool isSetSeed;
    [SerializeField, ConditionalHide("isSetSeed")] protected int seed = 42;

    public void GenerateDungeon()
    {
        if(isSetSeed)
            Random.InitState(seed);
        dungeonVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
    
    public static HashSet<Vector2Int> RunRandomWalk(RoomParamsSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.RWalkIterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.RWalkLength);
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
    
    public static List<Vector2Int> ConvertBoolArrayToVectorList(bool[,] tileMap, Vector2Int origin = new Vector2Int())
    {
        List<Vector2Int> tileList = new List<Vector2Int>();
        for (int i = 0; i < tileMap.GetLength(1); i++)
        {
            for (int j = 0; j < tileMap.GetLength(0); j++)
            {
                if (tileMap[j, i])
                {
                    // Debug.Log(i + " " + j);
                    tileList.Add(new Vector2Int(j, i) + origin);
                }
            }
        }

        return tileList;
    }
    
    public static bool[,] ConvertVectorEnumerableToBoolArray(IEnumerable<Vector2Int> tileList, 
        int width, int height, Vector2Int origin = new Vector2Int())
    {
        bool[,] tilemap = new bool[width+1,height+1];
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tilemap[i, j] = false;  
            }
        }
        foreach (var tile in tileList)
        {
            tilemap[tile.x - origin.x, tile.y - origin.y] = true;
        }
        
        return tilemap;
    }
    
}
