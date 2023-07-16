using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    
    public class TileBaseValue: IValue<TileBase>
    {
        private readonly TileBase _tileBase;
        public TileBase Value => _tileBase;
        public TileBaseValue(TileBase tileBase)
        {
            _tileBase = tileBase;
        }

        

        public bool Equals(IValue<TileBase> x, IValue<TileBase> y)
        {
            return x == y;
        }


        public bool Equals(IValue<TileBase> other)
        {
            return other.Value == this.Value;
        }

        public int GetHashCode(IValue<TileBase> obj)
        {
            return obj.GetHashCode();
        }
        
        public override int GetHashCode()
        {
            return _tileBase.GetHashCode();
        }

    }
}