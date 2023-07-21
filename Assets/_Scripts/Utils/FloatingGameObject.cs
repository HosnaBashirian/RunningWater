using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Utils
{
    public class FloatingGameObject : MonoBehaviour
    {
        [SerializeField] private float range = 0.5f;
        [SerializeField] private float rotationSpeed = 180;

        private void Start()
        {
            transform.DOMoveY(transform.position.y + range, 1f).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(0,0.2f));
        }

        private void Update()
        {
            transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}