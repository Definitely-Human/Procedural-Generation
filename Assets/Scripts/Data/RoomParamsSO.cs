using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "PCG/SimpleRandomWalkData", fileName = "Simple Random Walk Parameters")]
public class RoomParamsSO : ScriptableObject
{
    [SerializeField] private RoomType roomType;
    [SerializeField] private int roomMaxWidth;
    [SerializeField] private int roomMaxHeight;

    [Header("Random Walk Parameters"), SerializeField] 
    private int rWalkIterations;
    [SerializeField] private int rWalkLength;
    [SerializeField] private bool startRandomlyEachIteration;
    
    [Header("Smoothing Settings")] 
    [SerializeField] private bool applySmoothing = true;
    [SerializeField, Range(0,25)] private int cellAutIterations = 6;
    [SerializeField, Range(3,5)] private int celAutThreshold = 4;

    
    public RoomType Type => roomType;
    
    /// <summary>
    /// How many individual iterations of random walk should be.
    /// </summary>
    public int RWalkIterations => rWalkIterations;

    /// <summary>
    /// Lenght of each walk iteration.
    /// Random walk can step on the same tile multiple times so actual length will be equal or less.
    /// </summary>
    public int RWalkLength => rWalkLength;

    /// <summary>
    /// Should random walk start from random position from ones it already stepped on or always from 0,0.
    /// </summary>
    public bool StartRandomlyEachIteration => startRandomlyEachIteration;

    /// <summary>
    /// Max width of a space where this room can be generated.
    /// </summary>
    public int RoomMaxWidth => roomMaxWidth;

    /// <summary>
    /// Max height of a space where this room can be generated.
    /// </summary>
    public int RoomMaxHeight => roomMaxHeight;


    public bool ApplySmoothing => applySmoothing;

    public int CellAutIterations => cellAutIterations;

    public int CelAutThreshold => celAutThreshold;
}
