using System.Collections;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public interface IInputReader<T>
    {
        IValue<T>[,] ReadInputToGrid(); 
    }
}


