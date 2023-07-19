using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Controller.Items
{
    public class BaseInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] protected float radius = 1;
        [SerializeField] protected LayerMask layerMask;
        private bool _isInteracted = false;

        private void FixedUpdate()
        {
            if (!_isInteracted)
            {
                if (CheckCollision())
                {
                    _isInteracted = true;
                    Interact();
                }
            }
        }

        public virtual bool CheckCollision()
        {
            return Physics.CheckSphere(transform.position, radius, layerMask);
        }

        public virtual void Interact()
        {
            transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}