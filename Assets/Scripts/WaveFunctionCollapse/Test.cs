using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;

public class Test : MonoBehaviour
{
    public Tilemap input;
    
    [ContextMenu("Run test")]
    void RunTest()
    {
        InputReader reader = new InputReader(input);
        var grid = reader.ReadInputToGrid();

        ValuesManager<TileBase> valuesManager = new ValuesManager<TileBase>(grid);
        StringBuilder builder;
        List<string> list = new List<string>();
        for (int row = -1; row <= grid.GetLength(0); row++)
        {
            builder = new StringBuilder();
            for (int col = -1; col <= grid.GetLength(1); col++)
            {
                builder.Append(valuesManager.GetGridValuesIncludingOffset(col, row) + " ");
            }
            list.Add(builder.ToString());
        }
        list.Reverse();
        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }

}
