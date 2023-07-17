using System.Collections.Generic;
using WaveFunctionCollapse.Patterns.Strategies;

namespace WaveFunctionCollapse.Patterns
{
    public class PatternManager
    {
        private Dictionary<int, PatternData> _patternDataIndexDictionary;
        private Dictionary<int, PatternNeighbours> _patternPossibleNeighbours;
        private int _patternSize = -1;
        private IFindNeighbourStrategy _strategy;
    }
}

