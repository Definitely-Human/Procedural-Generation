//using UnityEngine;


using System.Collections.Generic;

namespace WaveFunctionCollapse.Patterns
{
    /// <summary>
    /// 
    /// </summary>
//[System.Serializable]
    public class PatternNeighbours
    {

        #region Fields

        private Dictionary<Direction, HashSet<int>> _directionPatternNeighbourDictionary =
            new Dictionary<Direction, HashSet<int>>();
        #endregion


        #region Lifecycle

        public PatternNeighbours()
        {
            
        }

        #endregion


        #region Public API
        public HashSet<int> GetNeighboursInDirection(Direction dir)
        {
            if (_directionPatternNeighbourDictionary.ContainsKey(dir))
            {
                return _directionPatternNeighbourDictionary[dir];
            }
            return new HashSet<int>();
            
        }

        public void AddPatternToDictionary(Direction dir, int patternIndex)
        {
            if (_directionPatternNeighbourDictionary.ContainsKey(dir))
            {
                _directionPatternNeighbourDictionary[dir].Add(patternIndex);
            }
            else
            {
                _directionPatternNeighbourDictionary.Add(dir,new HashSet<int>(){patternIndex});
            }
        }

        public void AddNeighbour(PatternNeighbours neighbours)
        {
            foreach (var item in _directionPatternNeighbourDictionary)
            {
                if (_directionPatternNeighbourDictionary.ContainsKey(item.Key) == false)
                {
                    _directionPatternNeighbourDictionary.Add(item.Key, new HashSet<int>());
                }
                _directionPatternNeighbourDictionary[item.Key].UnionWith(item.Value);
            }
        }
        
        #endregion

        
    }
}
