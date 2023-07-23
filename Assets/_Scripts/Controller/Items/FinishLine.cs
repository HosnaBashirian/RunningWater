using System;
using _Scripts.Controller.General;
using UnityEngine;

namespace _Scripts.Controller.Items
{
    public class FinishLine : BaseInteractable
    {
        [SerializeField] private Vector3 boxSize = Vector3.one;
        [SerializeField] private bool isInteracted = false;

        private void OnEnable()
        {
            isInteracted = false;
        }

        public override bool CheckCollision()
        {
            return Physics.CheckBox(transform.position, boxSize, Quaternion.identity, layerMask);
        }

        public override void Interact()
        {
            if (!GameManager.Instance.IsGameFinished && !isInteracted)
            {
                isInteracted = true;
                GameManager.Instance.GameWin();
            }
        }
        
        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}