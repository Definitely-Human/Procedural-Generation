using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SmartRoomGenerator))]
public class SmartDungeonGenerator : AbstractDungeonGenerator
{
    private List<Vector2Int> _dungeonFloorTiles;
    private RectInt _dungeonSize;
    private AbstractGraphGenerator _graphGenerator;
    private SmartRoomGenerator _smartRoomGenerator;
    private SmartCorridorGenerator _smartCorridorGenerator;

    [Header("Smoothing Settings")]
    [SerializeField] private bool applyDungeonSmoothing;
    [SerializeField, Range(0,25)] private int cellAutIterations = 0;
    [SerializeField, Range(3,5)] private int celAutThreshold = 4;

    [Header("Background Settings")] [SerializeField]
    private int backgroundMargin = 50;

    protected override void RunProceduralGeneration()
    {
        CreateAbstraction();

        GenerateRooms();

        GenerateCorridors();
        
        CollectDungeon();
        
        CalculateDungeonSize();
        
        
        if(applyDungeonSmoothing && cellAutIterations > 0)
            _dungeonFloorTiles = SmoothDungeon(_dungeonFloorTiles);

        
        dungeonVisualizer.PaintFloorTiles(_dungeonFloorTiles);
        WallGenerator.CreateWalls(new HashSet<Vector2Int>(_dungeonFloorTiles), dungeonVisualizer);

        // CalculateAndPaintBackground();
    }

    private void CalculateAndPaintBackground()
    {
        List<Vector2Int> backgroundTiles = new List<Vector2Int>();
        for (int i = _dungeonSize.min.x - backgroundMargin; i < _dungeonSize.max.x + backgroundMargin; i++)
        {
            for (int j = _dungeonSize.min.y - backgroundMargin; j < _dungeonSize.max.y + backgroundMargin; j++)
            {
                var backgroundTile = new Vector2Int(i, j);
                if(_dungeonFloorTiles.Contains(backgroundTile)) continue;
                backgroundTiles.Add(backgroundTile);
            }
        }
        dungeonVisualizer.PaintBackgroundTiles(backgroundTiles);
    }

    private void GenerateCorridors()
    {
        if (_smartCorridorGenerator == null)
        {
            _smartCorridorGenerator = GetComponent<SmartCorridorGenerator>();
            if (_smartCorridorGenerator == null)
            {
                throw new Exception("Smart corridor generator not found.");
            }
        }
        
        _smartCorridorGenerator.GenerateCorridors(_graphGenerator,_smartRoomGenerator.Rooms);
    }

    private void GenerateRooms()
    {
        if (_smartRoomGenerator == null)
        {
            _smartRoomGenerator = GetComponent<SmartRoomGenerator>();
            if (_smartRoomGenerator == null)
            {
                throw new Exception("Smart room generator not found.");
            }
        }

        _smartRoomGenerator.Generate(_graphGenerator.DungeonGraph);
    }

    

    private void CalculateDungeonSize()
    {
        _dungeonSize = Room.CalculateRoomBounds(_dungeonFloorTiles);
    }

    private List<Vector2Int> SmoothDungeon(List<Vector2Int> floor)
    {
        bool[,] floorMatrix = ConvertVectorEnumerableToBoolArray(_dungeonFloorTiles, _dungeonSize.width, _dungeonSize.height,
            _dungeonSize.min);
        ProceduralGenerationAlgorithms.CellularAutomaton(floorMatrix, cellAutIterations, celAutThreshold);
        return ConvertBoolArrayToVectorList(floorMatrix, _dungeonSize.min);
    }

    

    private void CreateAbstraction()
    {
        if (_graphGenerator == null)
            _graphGenerator = GetComponent<AbstractGraphGenerator>();

        _graphGenerator.GenerateGraph();
    }
    
    private void CollectDungeon()
    {
        _dungeonFloorTiles = new List<Vector2Int>();
        foreach (var room in _smartRoomGenerator.Rooms)
        {
            foreach (var tile in room.Tiles)
            {
                _dungeonFloorTiles.Add(tile + room.Origin);
            }

        }

        foreach (var corridor in _smartCorridorGenerator.Corridors)
        {
            foreach (var tile in corridor.Tiles)
            {
                _dungeonFloorTiles.Add(tile + corridor.Origin);
            }
        }
    }
}
