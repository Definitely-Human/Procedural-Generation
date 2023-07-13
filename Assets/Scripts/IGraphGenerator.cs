using System.Collections;
using System.Collections.Generic;
using QuickGraph;
using UnityEngine;

public abstract class AbstractGraphGenerator: MonoBehaviour
{
    public BidirectionalGraph<RoomNode, Edge<RoomNode>> DungeonGraph { get; protected set; }
    public abstract void GenerateGraph();
}
