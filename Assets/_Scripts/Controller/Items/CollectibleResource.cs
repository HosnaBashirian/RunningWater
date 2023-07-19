using _Scripts.Controller.General;
using _Scripts.Controller.UI;

namespace _Scripts.Controller.Items
{
    public class CollectibleResource : BaseInteractable
    {
        public override void Interact()
        {
            GameManager.Instance.AddResource(2);
            base.Interact();
        }
    }
}