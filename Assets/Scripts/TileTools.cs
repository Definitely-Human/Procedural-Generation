using System;
using System.Collections.Generic;
using UnityEngine;

public class TileTools
{
    public static HashSet<Vector2Int> DrawCircle(Vector2Int center, int radius)
    {
        HashSet<Vector2Int> circle = new HashSet<Vector2Int>();

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y < radius * radius)
                {
                    int drawX = center.x + x;
                    int drawY = center.y + y;
                    circle.Add(new Vector2Int(drawX, drawY));
                }
            }
        }

        return circle;
    }

    public static List<Vector2Int> GetLine(Vector2Int from, Vector2Int to) {
        List<Vector2Int> line = new List<Vector2Int> ();

        int x = from.x;
        int y = from.y;

        int dx = to.x - from.x;
        int dy = to.y - from.y;

        bool inverted = false;
        int step = Math.Sign (dx);
        int gradientStep = Math.Sign (dy);

        int longest = Mathf.Abs (dx);
        int shortest = Mathf.Abs (dy);

        if (longest < shortest) {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign (dy);
            gradientStep = Math.Sign (dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i =0; i < longest; i ++) {
            line.Add(new Vector2Int(x,y));

            if (inverted) {
                y += step;
            }
            else {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest) {
                if (inverted) {
                    x += gradientStep;
                }
                else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }
}