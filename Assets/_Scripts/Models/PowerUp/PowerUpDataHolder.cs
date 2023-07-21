using System;
using System.Collections.Generic;
using _Scripts.Models.General;
using UnityEngine;

namespace _Scripts.Models.PowerUp
{
    [CreateAssetMenu(fileName = "PowerUp Data", menuName = "GameData/PowerUp", order = 0)]
    public class PowerUpDataHolder : ScriptableObject
    {
        public float powerUpDuration = 7f;
        public List<PowerUpRequirement> requirements;
    }

    [Serializable]
    public class PowerUpRequirement
    {
        public PowerUpType type;
        public ResourceType requiredResource;
        public int resourceAmount;
    }

    [Serializable]
    public enum PowerUpType
    {
        X2,
        Shield
    }
}