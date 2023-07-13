using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class LinearGraphGenerator : AbstractGraphGenerator
{
    [Header("Lenght Settings"),Range(3,10), SerializeField] 
    private int minDungeonLenght;
    [Range(4,10), SerializeField] 
    private int maxDungeonLength;

    public override void GenerateGraph()
    {
        BidirectionalGraph<RoomNode, Edge<RoomNode>> graph = new BidirectionalGraph<RoomNode, Edge<RoomNode>>();
        
        var entryRoom = new RoomNode(RoomType.Entrance);
        graph.AddVertex(entryRoom);
        

        int dungeonLength = Random.Range(minDungeonLenght, maxDungeonLength + 1);

        List<RoomType> generetableRooms = new List<RoomType>() { 
            RoomType.Safe, RoomType.EnemyGiant,RoomType.EnemyLarge,RoomType.EnemyMid };
        for (int i = 0; i < dungeonLength - 2; i++)
        {
            RoomType roomType = generetableRooms[Random.Range(0, generetableRooms.Count)];
            RoomNode room = new RoomNode(roomType);
            Edge<RoomNode> connection = new Edge<RoomNode>(graph.Vertices.Last(), room);
            graph.AddVertex(room);
            graph.AddEdge(connection);
        }

        var bossRoom = new RoomNode(RoomType.Boss);
        Edge<RoomNode> bossConnection = new Edge<RoomNode>(graph.Vertices.Last(), bossRoom);
        graph.AddVertex(bossRoom);
        graph.AddEdge(bossConnection);
        DungeonGraph = graph;
    }
}
