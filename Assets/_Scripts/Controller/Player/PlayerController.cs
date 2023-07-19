using System;
using _Scripts.Controller.General;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Controller.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float playerMovementRange = 1f;
        [SerializeField] private float playerHorizontalSpeed = 0.005f;
        [SerializeField] private float playerForwardSpeed = 0.1f;

        public bool IsPlayerPaused { get; set; } = false;

        private void Awake()
        {
            InputManager.Instance.OnHorizontalSwipe += OnHorizontalSwipe;
        }
        
        private void OnDestroy()
        {
            InputManager.Instance.OnHorizontalSwipe -= OnHorizontalSwipe;
        }
        
        private void OnHorizontalSwipe(float delta)
        {
            var position = transform.position;
            position.x = Mathf.Clamp(
                position.x + delta * playerHorizontalSpeed,
                -playerMovementRange, playerMovementRange);
            transform.position = position;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameStarted) return;
            if (IsPlayerPaused) return;
            var position = transform.position;
            position.z += playerForwardSpeed * Time.deltaTime;
            transform.position = position;
        }
        
    }
}
