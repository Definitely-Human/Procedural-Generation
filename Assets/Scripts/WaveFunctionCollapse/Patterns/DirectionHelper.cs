namespace WaveFunctionCollapse.Patterns
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public static class DirectionHelper
    {
        public static Direction GetOppositeDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return direction;
            }
        }
    }
}