using _Scripts.Controller.General;
using _Scripts.Models.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Controller.Items
{
    public class InteractableObstacle : BaseInteractable
    {
        [SerializeField] private int punishment = -5;
        public override void Interact()
        {
            if (GameManager.Instance.shieldPowerUpActive) return;
            GameManager.Instance.AddResource(punishment);
            transform.DOShakePosition(0.5f, new Vector3(0.2f, 0.2f, 1f) * 0.5f, 25);
            GameHub.Instance.AudioPlayer.PlayOneShot(SoundType.Impact);
        }
    }
}