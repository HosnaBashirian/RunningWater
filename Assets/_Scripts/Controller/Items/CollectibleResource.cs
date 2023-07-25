using _Scripts.Controller.General;
using _Scripts.Controller.UI;
using _Scripts.Models.General;
using Unity.Mathematics;

namespace _Scripts.Controller.Items
{
    public class CollectibleResource : BaseInteractable
    {
        public override void Interact()
        {
            var amount = GameManager.Instance.x2PowerUpActive ? 2 : 1;
            GameManager.Instance.AddResource(amount, ResourceType.Coin);
            GameHub.Instance.ParticleData.SpawnParticle("coinCollect", transform.position, quaternion.identity);
            base.Interact();
        }
    }
}