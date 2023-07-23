using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Models.General
{
    [CreateAssetMenu(fileName = "ParticleData", menuName = "GameData/Particle", order = 0)]
    public class ParticleData : ScriptableObject
    {
        [SerializeField] private List<ParticleDto> particles;

        public void SpawnParticle(string key, Vector3 position, Quaternion quaternion)
        {
            Instantiate(particles.Find(x => x.key == key).particle, position, quaternion);
        }
    }

    [Serializable]
    public class ParticleDto
    {
        public string key;
        public GameObject particle;
    }
}