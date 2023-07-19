using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Utils
{
    public class FloatingGameObject : MonoBehaviour
    {
        [SerializeField] private float range = 0.5f;

        private void Start()
        {
            transform.DOMoveY(transform.position.y + range, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}