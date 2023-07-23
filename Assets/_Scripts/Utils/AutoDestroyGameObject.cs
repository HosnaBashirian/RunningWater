using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Utils
{
    public class AutoDestroyGameObject : MonoBehaviour
    {
        [SerializeField] private float delay;

        private void Start()
        {
            Destroy(gameObject, delay);
        }
    }
}