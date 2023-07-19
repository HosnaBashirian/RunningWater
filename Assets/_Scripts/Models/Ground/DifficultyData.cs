using System;
using UnityEngine.Serialization;

namespace _Scripts.Models.Ground
{
    [Serializable]
    public class DifficultyData
    {
        public GroundDifficulty difficulty;
        // public float obstacleChance;
        // public float resourceChance;
        // public float choiceChance;
        public float minObstacleDistance;
        public float minResourceDistance;
        public float minChoiceDistance;
    }
    
    public enum GroundDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}