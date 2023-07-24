using System;
using _Scripts.Controller.General;
using UnityEngine;

namespace _Scripts.Controller.Items
{
    public class FinishLine : BaseInteractable
    {
        [SerializeField] private Vector3 boxSize = Vector3.one;
        [SerializeField] private bool isInteracted = false;

        private void Start()
        {
            isInteracted = true;
        }

        private void OnEnable()
        {
            isInteracted = false;
        }

        public override bool CheckCollision()
        {
            if (Vector3.Distance(GameHub.Instance.Player.transform.position, transform.position) > 10)
            {
                isInteracted = false;
                return false;
            }
            return Physics.CheckBox(transform.position, boxSize, Quaternion.identity, layerMask);
        }

        public override void Interact()
        {
            if (!GameManager.Instance.IsGameFinished && GameManager.Instance.IsGameStarted && !isInteracted)
            {
                print("Finish Line");
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