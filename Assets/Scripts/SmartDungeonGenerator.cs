using System.Collections;
using System.Collections.Generic;
using QuickGraph;
using UnityEngine;

public class SmartDungeonGenerator : AbstractDungeonGenerator
{
    private BidirectionalGraph<RoomNode, Edge<RoomNode>> _dungeonGraph;
    [SerializeField] private Dictionary<RoomType, RoomParamsSO> _roomDefinitions;

    protected override void RunProceduralGeneration()
    {
        CreateAbstraction();
    }

    private void CreateAbstraction()
    {
        _dungeonGraph = new BidirectionalGraph<RoomNode, Edge<RoomNode>>();
        
        var entryRoom = new RoomNode(RoomType.Entrance);
        _dungeonGraph.AddVertex(entryRoom);

        var roomA = new RoomNode(RoomType.Enemy);
        _dungeonGraph.AddVertex(roomA);
        _dungeonGraph.AddEdge(new Edge<RoomNode>(entryRoom, roomA));
        
        var roomB = new RoomNode(RoomType.Enemy);
        _dungeonGraph.AddVertex(roomB);
        _dungeonGraph.AddEdge(new Edge<RoomNode>(roomA, roomB));
        
        var roomC = new RoomNode(RoomType.Enemy);
        _dungeonGraph.AddVertex(roomC);
        _dungeonGraph.AddEdge(new Edge<RoomNode>(roomB, roomC));
    }
}
