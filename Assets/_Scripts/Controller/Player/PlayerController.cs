using System;
using System.Collections.Generic;
using _Scripts.Controller.General;
using _Scripts.Models.PowerUp;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Controller.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float playerMovementRange = 1f;
        [SerializeField] private float playerHorizontalSpeed = 0.005f;
        [SerializeField] private float playerForwardSpeed = 0.1f;
        [SerializeField] private GameObject shield;
        [SerializeField] private List<PlayerSkin> skins;
        public List<PlayerSkin> Skins => skins;

        public bool IsPlayerPaused { get; set; } = false;

        public int ActiveSkin
        {
            get => PlayerPrefs.GetInt("ActiveCharacter", 0);
            set => PlayerPrefs.SetInt("ActiveCharacter", value);
        }
        

        private void Awake()
        {
            InputManager.Instance.OnHorizontalSwipe += OnHorizontalSwipe;
            GameManager.Instance.OnPowerUpActive += OnPowerUpActive;
            GameManager.Instance.OnPowerUpExpire += OnPowerUpExpire;
            SetCharacter();
        }

        private void OnEnable()
        {
            SetCharacter();
        }

        public void SetCharacter()
        {
            var activeCharacter = ActiveSkin;

            skins.ForEach(x=>x.gameObject.SetActive(false));
            skins[activeCharacter].gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnHorizontalSwipe -= OnHorizontalSwipe;
            GameManager.Instance.OnPowerUpActive -= OnPowerUpActive;
            GameManager.Instance.OnPowerUpExpire -= OnPowerUpExpire;
        }

        private void OnPowerUpActive(PowerUpType powerUpType)
        {
            if (powerUpType == PowerUpType.Shield)
            {
                ActivateShield();
            }
        }

        private void OnHorizontalSwipe(float delta)
        {
            var position = transform.position;
            position.x = Mathf.Clamp(
                position.x + delta * playerHorizontalSpeed,
                -playerMovementRange, playerMovementRange);
            transform.position = position;
        }

        public void Win()
        {
            skins[ActiveSkin].Victory();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGameFinished)
            {
                return;
            }
            if (!GameManager.Instance.IsGameStarted)
            {
                if (running)
                {
                    running = false;
                    skins[ActiveSkin].Idle();
                }
                return;
            }
            if (IsPlayerPaused)
            {
                if (running)
                {
                    running = false;
                    skins[ActiveSkin].Idle();
                }
                return;
            }
            if (!running)
            {
                running = true;
                skins[ActiveSkin].Run();
            }
            var position = transform.position;
            position.z += playerForwardSpeed * Time.deltaTime;
            transform.position = position;
        }

        private bool running = false;

        public void ActivateShield()
        {
            shield.SetActive(true);
        }

        private void OnPowerUpExpire()
        {
            shield.SetActive(false);
        }
    }
}