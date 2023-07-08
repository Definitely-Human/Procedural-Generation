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
    [Space]
    [SerializeField] private TileBase wallInnerCornerDownLeft;
    [SerializeField] private TileBase wallInnerCornerDownRight;
    [Space] 
    [SerializeField] private TileBase wallDiagonalCornerDownRight;
    [SerializeField] private TileBase wallDiagonalCornerDownLeft;
    [SerializeField] private TileBase wallDiagonalCornerUpRight;
    [SerializeField] private TileBase wallDiagonalCornerUpLeft;

    public TileBase FloorTile => floorTile;

    public TileBase WallTop => wallTop;

    public TileBase WallSideRight => wallSideRight;

    public TileBase WallSideLeft => wallSideLeft;

    public TileBase WallBottom => wallBottom;

    public TileBase WallFull => wallFull;

    public TileBase WallInnerCornerDownLeft => wallInnerCornerDownLeft;

    public TileBase WallInnerCornerDownRight => wallInnerCornerDownRight;

    public TileBase WallDiagonalCornerDownRight => wallDiagonalCornerDownRight;

    public TileBase WallDiagonalCornerDownLeft => wallDiagonalCornerDownLeft;

    public TileBase WallDiagonalCornerUpRight => wallDiagonalCornerUpRight;

    public TileBase WallDiagonalCornerUpLeft => wallDiagonalCornerUpLeft;
}