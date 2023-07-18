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

        public PatternManager(int patternSize)
        {
            _patternSize = patternSize;
        }

        public void PrecessGrid<T>(ValuesManager<T> valuesManager, bool equalWeights, string strategyName = null)
        {
            NeighbourStrategyFactory strategyFactory = new NeighbourStrategyFactory();
            _strategy = strategyFactory.CreateInstance(strategyName ?? _patternSize.ToString());
            CreatePatterns(valuesManager, _strategy, equalWeights);
        }

        private void CreatePatterns<T>(ValuesManager<T> valuesManager, IFindNeighbourStrategy strategy, bool equalWeights)
        {
            PatternDataResults patternFinderResult = PatternFinder.GetPatternResultFromGrid(valuesManager, _patternSize, equalWeights);
            _patternDataIndexDictionary = patternFinderResult.PatternIndexDictionary;
            GetPatternNeighbours(patternFinderResult, strategy);
        }

        private void GetPatternNeighbours(PatternDataResults patternFinderResult, IFindNeighbourStrategy strategy)
        {
            _patternPossibleNeighbours =
                PatternFinder.FindPossibleNeighboursForAllPatterns(strategy, patternFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index)
        {
            return _patternDataIndexDictionary[index];
        }

        public HashSet<int> GetPossibleNeighboursForPatternInDirection(int patternIndex, Direction dir)
        {
            return _patternPossibleNeighbours[patternIndex].GetNeighboursInDirection(dir);
        }

        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelative;
        }
        
        public float GetPatternFrequencyLog2(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelativeLog2;
        }

        public int GetNumberOfPatterns()
        {
            return _patternDataIndexDictionary.Count;
        }
    }
}

