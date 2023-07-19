using System;
using _Scripts.Controller.General;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Controller.Items
{
    public class InteractableChoice : BaseInteractable
    {
        [SerializeField] private Vector3 boxSize = Vector3.one;
        [SerializeField] private float multiplier = 2f;
        [SerializeField] private bool rightIsCorrect = true;
        
        public override bool CheckCollision()
        {
            return Physics.CheckBox(transform.position, boxSize, Quaternion.identity, layerMask);
        }

        public override void Interact()
        {
            var inverseMultiplier = 1f / multiplier;
            var playerPos = GameHub.Instance.Player.transform.position;
            if (playerPos.x < 0)
            {
                GameManager.Instance.MultiplyResource(rightIsCorrect ? inverseMultiplier : multiplier);
            }
            else
            {
                GameManager.Instance.MultiplyResource(rightIsCorrect ? multiplier : inverseMultiplier);
            }

            Destroy(gameObject);
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}