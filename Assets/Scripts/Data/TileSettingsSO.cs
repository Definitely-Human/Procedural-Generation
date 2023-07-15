using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "PCG/Tile Settings", fileName = "Tile Settings")]
public class TileSettingsSO : ScriptableObject
{
    [SerializeField] private TileBase floorTile;
    [Space]
    [SerializeField] private TileBase wallTop;
    [SerializeField] private TileBase wallSideRight;
    [SerializeField] private TileBase wallSideLeft;
    [SerializeField] private TileBase wallBottom;
    [SerializeField] private TileBase wallFull;
    [SerializeField] private TileBase wallSingle;
    [Space]
    [SerializeField] private TileBase wallHorizontal;
    [SerializeField] private TileBase wallVertical;
    [Space]
    [SerializeField] private TileBase wallInnerCornerDownLeft;
    [SerializeField] private TileBase wallInnerCornerDownRight;
    [SerializeField] private TileBase wallInnerCornerUpLeft;
    [SerializeField] private TileBase wallInnerCornerUpRight;
    [Space] 
    [SerializeField] private TileBase wallDiagonalCornerDownRight;
    [SerializeField] private TileBase wallDiagonalCornerDownLeft;
    [SerializeField] private TileBase wallDiagonalCornerUpRight;
    [SerializeField] private TileBase wallDiagonalCornerUpLeft;
    [Space] 
    [SerializeField] private TileBase wallLedgeLeft;
    [SerializeField] private TileBase wallLedgeRight;
    [SerializeField] private TileBase wallLedgeTop;
    [SerializeField] private TileBase wallLedgeBottom;
    [Space]
    [SerializeField] private TileBase wallTLeft;
    [SerializeField] private TileBase wallTRight;
    [SerializeField] private TileBase wallTTop;
    [SerializeField] private TileBase wallTBottom;
    [Space]
    [SerializeField] private TileBase backgroundTile;

    public TileBase FloorTile => floorTile;

    public TileBase WallTop => wallTop;

    public TileBase WallSideRight => wallSideRight;

    public TileBase WallSideLeft => wallSideLeft;

    public TileBase WallBottom => wallBottom;

    public TileBase WallFull => wallFull;

    public TileBase WallInnerCornerDownLeft => wallInnerCornerDownLeft;

    public TileBase WallInnerCornerDownRight => wallInnerCornerDownRight;
    
    public TileBase WallInnerCornerUpLeft => wallInnerCornerUpLeft;

    public TileBase WallInnerCornerUpRight => wallInnerCornerUpRight;

    public TileBase WallDiagonalCornerDownRight => wallDiagonalCornerDownRight;
    
    public TileBase WallDiagonalCornerDownLeft => wallDiagonalCornerDownLeft;

    public TileBase WallDiagonalCornerUpRight => wallDiagonalCornerUpRight;

    public TileBase WallDiagonalCornerUpLeft => wallDiagonalCornerUpLeft;

    public TileBase WallLedgeLeft => wallLedgeLeft;

    public TileBase WallLedgeRight => wallLedgeRight;

    public TileBase WallLedgeTop => wallLedgeTop;

    public TileBase WallLedgeBottom => wallLedgeBottom;
    
    public TileBase BackgroundTile => backgroundTile;

    public TileBase WallTLeft => wallTLeft;

    public TileBase WallTRight => wallTRight;

    public TileBase WallTTop => wallTTop;

    public TileBase WallTBottom => wallTBottom;

    public TileBase WallSingle => wallSingle;

    public TileBase WallHorizontal => wallHorizontal;

    public TileBase WallVertical => wallVertical;
}