using System;
using UnityEngine;

namespace _Scripts.Models.Ground
{
    [Serializable]
    public class ChunkData
    {
        public GameObject chunk;
        public ChunkType type;
    }

    public enum ChunkType
    {
        Empty = 0,
        Obstacle = 1,
        Resource = 2,
        Choice = 3,
        Finish = 4,
    }
}