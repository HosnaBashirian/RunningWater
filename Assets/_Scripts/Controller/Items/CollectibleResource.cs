using _Scripts.Controller.General;
using _Scripts.Controller.UI;
using _Scripts.Models.General;

namespace _Scripts.Controller.Items
{
    public class CollectibleResource : BaseInteractable
    {
        public override void Interact()
        {
            GameManager.Instance.AddResource(2, ResourceType.Coin);
            base.Interact();
        }
    }
}