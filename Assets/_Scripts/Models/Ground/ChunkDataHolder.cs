using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Models.Ground
{
    [CreateAssetMenu(fileName = "Chunks Data", menuName = "GameData/Chunks", order = 0)]
    public class ChunkDataHolder : ScriptableObject
    {
        public List<ChunkData> chunks;
        public DifficultyData easyDifficulty;
        public DifficultyData mediumDifficulty;
        public DifficultyData hardDifficulty;
    }
}