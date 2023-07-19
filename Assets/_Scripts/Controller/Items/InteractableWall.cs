using _Scripts.Controller.General;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Controller.Items
{
    public class InteractableWall : BaseInteractable
    {
        [SerializeField] private Vector3 boxSize = Vector3.one;
        
        public override bool CheckCollision()
        {
            return Physics.CheckBox(transform.position, boxSize, Quaternion.identity, layerMask);
        }

        public override void Interact()
        {
            GameHub.Instance.Player.IsPlayerPaused = true;
            GameHub.Instance.UIManager.ChoicePopUp.gameObject.SetActive(true);
            var randomChoice = GameHub.Instance.ChoiceDataHolder.choices.Random();
            GameHub.Instance.UIManager.ChoicePopUp.Set(randomChoice);
            Destroy(gameObject);
        }
        
        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, boxSize);
        }
    }
}