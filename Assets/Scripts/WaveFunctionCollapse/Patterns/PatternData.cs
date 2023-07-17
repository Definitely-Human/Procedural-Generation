using System;
using UnityEngine;

namespace WaveFunctionCollapse.Patterns
{
    public class PatternData
    {
        private Pattern _pattern;
        private int _frequency;
        private float _frequencyRelative;
        private float _frequencyRelativeLog2;

        public Pattern Pattern => _pattern;

        public float FrequencyRelative => _frequencyRelative;

        public float FrequencyRelativeLog2 => _frequencyRelativeLog2;

        public PatternData(Pattern pattern, int frequency, float frequencyRelative, float frequencyRelativeLog2)
        {
            _pattern = pattern;
            _frequency = frequency;
            _frequencyRelative = frequencyRelative;
            _frequencyRelativeLog2 = frequencyRelativeLog2;
        }

        public void AddToFrequency()
        {
            _frequency++;
        }

        public void CalculateRelativeFrequency(int total)
        {
            _frequencyRelative = (float)_frequency / total;
            _frequencyRelativeLog2 = Mathf.Log(_frequencyRelative, 2);
        }

        public bool CompareGrid(Direction dir, PatternData data)
        {
            return _pattern.CompareToAnother(dir, data.Pattern);
        }
    }
}