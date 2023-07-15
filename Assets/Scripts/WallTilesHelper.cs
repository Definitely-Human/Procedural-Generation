using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallTypesHelper
{
    public static readonly HashSet<int> WallTop = new HashSet<int>
    {
        0b0110,
        0b0011,
        0b0010,
        0b1010,
        0b1100,
        0b1110,
        0b1011,
        0b0111
    };

    public static readonly HashSet<int> WallSideLeft = new HashSet<int>
    {
        0b0100
    };

    public static readonly HashSet<int> WallSideRight = new HashSet<int>
    {
        0b0001
    };

    public static readonly HashSet<int> WallBottom = new HashSet<int>
    {
        0b1000
    };

    public static readonly HashSet<int> WallInnerCornerDownLeft = new HashSet<int>
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b11010001,
        0b10110001,
        0b10100001,
        0b10010000,
        0b00110001,
        0b10110000,
        0b00100001,
        0b10010001,
        0b11110100
    };

    public static readonly HashSet<int> WallInnerCornerDownRight = new HashSet<int>
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b01000101,
        0b11000101,
        0b10000101,
        0b01000111,
        0b11000110,
        0b11000010,
        0b10000100,
        0b01000110,
        0b10000110,
        0b11000100,
        0b01000010,
        0b11010111,
    };

    public static readonly HashSet<int> WallDiagonalCornerDownLeft = new HashSet<int>
    {
        0b01000000
    };

    public static readonly HashSet<int> WallDiagonalCornerDownRight = new HashSet<int>
    {
        0b00000001,
    };

    public static readonly HashSet<int> WallDiagonalCornerUpLeft = new HashSet<int>
    {
        0b00010000
    };

    public static readonly HashSet<int> WallDiagonalCornerUpRight = new HashSet<int>
    {
        0b00000100,
        0b00000101
    };

    public static readonly HashSet<int> WallFull = new HashSet<int>
    {
        0b1101,
        0b0101,
        0b1101,
        0b1001,
        
    };

    public static readonly HashSet<int> WallSingle = new()
    {
        0b1111,
    };

    public static readonly HashSet<int> WallFullEightDirections = new HashSet<int>
    {
        0b11100100,
        0b10010011,
        0b01110100,
        0b00010111,
        0b00010110,
        0b00110100,
        0b00010101,
        0b01010100,
        0b00010010,
        0b00100100,
        0b00010011,
        0b01100100,
        0b10010111,
        0b10010110,
        0b10110100,
        0b11100101,
        0b11010011,
        0b11110101,
        0b01110101,
        0b01010111,
        0b01100101,
        0b01010011,
        0b01010010,
        0b00100101,
        0b00110101,
        0b01010110,
        0b11010101,
        0b11010100,
        0b10010101,
        0b01000100,
        0b01010001,
        0b00010001,
    };

    public static readonly HashSet<int> WallBottomEightDirections = new()
    {
        
    };

    public static readonly HashSet<int> WallLedgeLeftEightDirections = new()
    {
        0b11111000,
        0b11111100,
        0b11111001,
        0b11111101,
        0b10111000,
    };
    
    public static readonly HashSet<int> WallLedgeRightEightDirections = new()
    {
        0b10001111,
        0b10011111,
        0b11001111,
        0b11011111,
    };
    
    public static readonly HashSet<int> WallLedgeBottomEightDirections = new()
    {
        0b11100011,
        0b11110011,
        0b11100111,
        0b11110111,
        0b10100011,
    };
    
    public static readonly HashSet<int> WallLedgeTopEightDirections = new()
    {
        0b00111110,
        0b00111111,
        0b01111111,
    };

    public static readonly HashSet<int> WallInnerCornerUpLeft = new()
    {
        0b00111100,
        0b00111000,
        0b01111100,
        0b01111000,
    };
    
    public static readonly HashSet<int> WallInnerCornerUpRight = new()
    {
        0b00011111,
        0b00011110,
        0b00001110,
        0b00001111
    };
    
    public static readonly HashSet<int> WallTRight = new()
    {
        0b01100001
    };
    
    public static readonly HashSet<int> WallTLeft = new()
    {
        0b01010000,
        0b01110001,
        0b01000011,
    };
    
    public static readonly HashSet<int> WallTBottom = new()
    {
        0b11010000,
        0b00010100
    };
    
    public static readonly HashSet<int> WallTTop = new()
    {
        0b01000001
    };
    
    public static readonly HashSet<int> WallHorizontal = new()
    {
        0b10001000,
        0b10001001,
        0b10001011,
        0b10001010,
        0b11001000,
        0b11011000,
        0b11011010,
        0b11011011,
        0b10011101,
        0b11011101,
    };
    
    public static readonly HashSet<int> WallVertical = new()
    {
        0b00110110,
        0b01110110,
    };
}