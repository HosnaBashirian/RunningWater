using System;
using UnityEngine;

namespace _Scripts.Utils
{
    public class TransformFollower : MonoBehaviour
    {
        [SerializeField] public Transform followingObject;
        [SerializeField] public Vector3 offset;

        private void Update()
        {
            if (followingObject.gameObject.activeInHierarchy)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
            
            transform.position = followingObject.position + offset;
        }
    }
}