using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Models.Utils
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "GameData/Sounds", order = 0)]
    public class SoundDataHolder : ScriptableObject
    {
        [SerializeField] private List<SoundData> soundData;
        public AudioClip GetSound(SoundType soundType)
        {
            return soundData.Find(x => x.soundType == soundType).audioClip;
        }
    }
    
    [Serializable]
    public class SoundData
    {
        public SoundType soundType;
        public AudioClip audioClip;
    }

    public enum SoundType
    {
        Click,
        Win,
        Lose,
        Collect,
        Impact,
    }
}