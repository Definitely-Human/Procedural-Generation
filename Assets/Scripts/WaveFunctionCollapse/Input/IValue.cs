using System;
using System.Collections.Generic;

namespace WaveFunctionCollapse
{
    public interface IValue<T> : IEqualityComparer<IValue<T>>, IEquatable<IValue<T>>
    {
        public T Value { get;  }
    }
}