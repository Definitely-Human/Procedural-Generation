using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap, backgroundTilemap, debugTilemap;

    [SerializeField] private TileSettingsSO tileSettings;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, tileSettings.FloorTile);
    }
    
    public void PaintBackgroundTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, backgroundTilemap, tileSettings.BackgroundTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap,tile,position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void PaintSingleColoredTile( Vector2Int position, Color color)
    {
        var tilePosition = debugTilemap.WorldToCell((Vector3Int)position);
        debugTilemap.SetTile(tilePosition, tileSettings.FloorTile);
        debugTilemap.SetTileFlags(tilePosition, TileFlags.None);
        debugTilemap.SetColor(tilePosition, color);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        debugTilemap.ClearAllTiles();
        backgroundTilemap.ClearAllTiles();
        
    }

    public void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.WallTop.Contains(typeAsInt))
        {
            tile = tileSettings.WallTop;
        }
        else if(WallTypesHelper.WallSideRight.Contains(typeAsInt))
        {
            tile = tileSettings.WallSideRight;
        }
        else if(WallTypesHelper.WallSideLeft.Contains(typeAsInt))
        {
            tile = tileSettings.WallSideLeft;
        }
        else if(WallTypesHelper.WallBottom.Contains(typeAsInt))
        {
            tile = tileSettings.WallBottom;
        }
        else if(WallTypesHelper.WallFull.Contains(typeAsInt))
        {
            tile = tileSettings.WallFull;
        }
        
        else if (WallTypesHelper.WallSingle.Contains(typeAsInt))
        {
            tile = tileSettings.WallSingle;
        }
        
        
        if(tile != null)
            PaintSingleTile(wallTilemap,tile,position);
    }

    public void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.WallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = tileSettings.WallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.WallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = tileSettings.WallInnerCornerDownRight;
        }
        else if (WallTypesHelper.WallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = tileSettings.WallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.WallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = tileSettings.WallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.WallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = tileSettings.WallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.WallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = tileSettings.WallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.WallFullEightDirections.Contains(typeAsInt))
        {
            tile = tileSettings.WallFull;
        }
        else if (WallTypesHelper.WallBottomEightDirections.Contains(typeAsInt))
        {
            tile = tileSettings.WallBottom;
        }
        else if (WallTypesHelper.WallLedgeLeftEightDirections.Contains(typeAsInt))
        {
            tile = tileSettings.WallLedgeLeft;
        }
        else if (WallTypesHelper.WallLedgeRightEightDirections.Contains(typeAsInt))
        {
            tile = tileSettings.WallLedgeRight;
        }
        else if (WallTypesHelper.WallLedgeBottomEightDirections.Contains(typeAsInt))
        {
            tile = tileSettings.WallLedgeBottom;
        }
        else if (WallTypesHelper.WallLedgeTopEightDirections.Contains(typeAsInt))
        {
            tile = tileSettings.WallLedgeTop;
        }
        else if (WallTypesHelper.WallInnerCornerUpRight.Contains(typeAsInt))
        {
            tile = tileSettings.WallInnerCornerUpRight;
        }
        else if (WallTypesHelper.WallInnerCornerUpLeft.Contains(typeAsInt))
        {
            tile = tileSettings.WallInnerCornerUpLeft;
        }
        else if (WallTypesHelper.WallTBottom.Contains(typeAsInt))
        {
            tile = tileSettings.WallTBottom;
        }
        else if (WallTypesHelper.WallTRight.Contains(typeAsInt))
        {
            tile = tileSettings.WallTRight;
        }
        else if (WallTypesHelper.WallTLeft.Contains(typeAsInt))
        {
            tile = tileSettings.WallTLeft;
        }
        else if (WallTypesHelper.WallTTop.Contains(typeAsInt))
        {
            tile = tileSettings.WallTTop;
        }
        else if (WallTypesHelper.WallHorizontal.Contains(typeAsInt))
        {
            tile = tileSettings.WallHorizontal;
        }
        else if (WallTypesHelper.WallVertical.Contains(typeAsInt))
        {
            tile = tileSettings.WallVertical;
        }
        
        if(tile!= null)
            PaintSingleTile(wallTilemap,tile,position);
    }
}
