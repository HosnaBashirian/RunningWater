using System;
using _Scripts.Controller.General;
using _Scripts.Models.General;
using _Scripts.Models.Utils;
using UnityEngine;

namespace _Scripts.Controller.Items
{
    public class MovingEnemy : BaseInteractable
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float playerMovementRange = 1f;
        [SerializeField] private int punishment = -5;
        [SerializeField] private int reward = 5;


        public override void Interact()
        {
            if (GameManager.Instance.shieldPowerUpActive) return;
            GameManager.Instance.AddResource(punishment, ResourceType.Water);
            GameHub.Instance.ParticleData.SpawnParticle("enemyHit", transform.position, Quaternion.identity);
            GameHub.Instance.AudioPlayer.PlayOneShot(SoundType.Impact);
            Destroy(gameObject);
        }

        private void Update()
        {
            var position = transform.position;
            position.x = Mathf.Clamp(
                position.x + moveSpeed * Time.deltaTime,
                -playerMovementRange, playerMovementRange);
            transform.position = position;
            const float tolerance = 0.01f;
            if ((Math.Abs(position.x - (-playerMovementRange)) < tolerance && moveSpeed < 0) ||
                (Math.Abs(position.x - playerMovementRange) < tolerance && moveSpeed > 0))
            {
                moveSpeed *= -1;
            }

            if (GameHub.Instance.Player.transform.position.z > transform.position.z + 0.75f)
            {
                GameHub.Instance.ParticleData.SpawnParticle("enemyPass", transform.position, Quaternion.identity);
                GameManager.Instance.AddResource(reward, ResourceType.Energy);
                Destroy(gameObject);
            }
        }
    }
}