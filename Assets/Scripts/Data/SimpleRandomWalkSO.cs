using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PCG/SimpleRandomWalkData", fileName = "Simple Random Walk Parameters")]
public class SimpleRandomWalkSO : ScriptableObject
{
    [SerializeField] private int iterations = 10;
    [SerializeField] private int walkLength = 10;
    [SerializeField] private bool startRandomlyEachIteration;

    public int Iterations => iterations;

    public int WalkLength => walkLength;

    public bool StartRandomlyEachIteration => startRandomlyEachIteration;
}
