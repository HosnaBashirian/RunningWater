using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Controller.UI;
using _Scripts.Models.General;
using _Scripts.Models.Ground;
using _Scripts.Models.PowerUp;
using _Scripts.Utils;
using MyBox;
using UnityEngine;

namespace _Scripts.Controller.General
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        private GameObject currentLevel = null;
        [SerializeField, Range(10, 50)] private int levelLength = 15;
        [SerializeField] private GroundDifficulty difficulty = GroundDifficulty.Easy;

        public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>()
        {
            {ResourceType.Water, 100},
            {ResourceType.Energy, 50},
        };


        public bool IsGameStarted { get; private set; } = false;
        public bool IsGameFinished { get; private set; } = false;

        public bool x2PowerUpActive = false;
        public bool x2PowerUpUsed = false;
        public bool shieldPowerUpActive = false;
        public bool shieldPowerUpUsed = false;

        private float _powerUpTimeLeft;
        public float PowerUpTimeLeft
        {
            get
            {
                var duration = GameHub.Instance.PowerUpDataHolder.powerUpDuration;
                var remaining = Mathf.Clamp(_powerUpTimeLeft, 0, duration);
                return remaining / duration;
            }
        }

        public event Action<PowerUpType> OnPowerUpActive;
        public event Action OnPowerUpExpire;

        private void Start()
        {
            InitializeGame();
        }

        private void Update()
        {
            if (!IsGameStarted && !IsGameFinished)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    IsGameStarted = true;
                }
            }

            if (!GameHub.Instance.Player.IsPlayerPaused)
            {
                _powerUpTimeLeft -= Time.deltaTime;
                if (_powerUpTimeLeft <= 0)
                {
                    OnPowerUpExpire?.Invoke();
                    shieldPowerUpActive = false;
                    x2PowerUpActive = false;
                }
            }
        }

        public void InitializeGame()
        {
            var playerStartPos = new Vector3(0, 0.6f, 0);
            GameHub.Instance.Player.transform.position = playerStartPos;
            CreateLevel();
            IsGameStarted = false;
            IsGameFinished = false;
            resources[ResourceType.Water] = 100;
            resources[ResourceType.Energy] = 50;
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Water], ResourceType.Water);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Energy], ResourceType.Energy);
            GameHub.Instance.UIManager.HudController.SetResourceText(GameHub.Instance.Coins, ResourceType.Coin);
            GameHub.Instance.UIManager.ChoicePopUp.gameObject.SetActive(false);
            shieldPowerUpActive = false;
            shieldPowerUpUsed = false;
            x2PowerUpActive = false;
            x2PowerUpUsed = false;
            OnPowerUpExpire?.Invoke();
            DelayedReset();
        }

        private async void DelayedReset()
        {
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
            GameHub.Instance.UIManager.HudController.HideGameOver();
            IsGameStarted = false;
            IsGameFinished = false;
            resources[ResourceType.Water] = 100;
            resources[ResourceType.Energy] = 50;
            GameHub.Instance.UIManager.ChoicePopUp.gameObject.SetActive(false);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Water], ResourceType.Water);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Energy], ResourceType.Energy);
            GameHub.Instance.UIManager.HudController.SetResourceText(GameHub.Instance.Coins, ResourceType.Coin);
        }

        [ButtonMethod()]
        public void CreateLevel()
        {
            if (currentLevel != null)
            {
                DestroyImmediate(currentLevel);
            }

            currentLevel = GameHub.Instance.GroundGenerator.GenerateGround(levelLength, difficulty);
        }

        public void MultiplyResource(float value, ResourceType resourceType = ResourceType.Water)
        {
            // print("MultiplyResource by " + value + "");
            if (resourceType == ResourceType.Coin)
            {
                GameHub.Instance.Coins = (int) (Convert.ToSingle(GameHub.Instance.Coins)* value);
                if (GameHub.Instance.Coins <= 0)
                {
                    GameHub.Instance.Coins = 0;
                }
                GameHub.Instance.UIManager.HudController.SetResourceText(GameHub.Instance.Coins, resourceType);
                return;
            }
            resources[resourceType] = (int) (Convert.ToSingle(resources[resourceType]) * value);
            if (resources[resourceType] <= 0)
            {
                resources[resourceType] = 0;
                GameOver();
            }

            GameHub.Instance.UIManager.HudController.SetResourceText(resources[resourceType], resourceType);
        }

        public void AddResource(int value, ResourceType resourceType = ResourceType.Water)
        {
            if (resourceType == ResourceType.Coin)
            {
                GameHub.Instance.Coins += value;
                if (GameHub.Instance.Coins <= 0)
                {
                    GameHub.Instance.Coins = 0;
                }
                GameHub.Instance.UIManager.HudController.SetResourceText(GameHub.Instance.Coins, resourceType);
                return;
            }
            resources[resourceType] += value;
            if (resources[resourceType] <= 0)
            {
                resources[resourceType] = 0;
                GameOver();
            }

            GameHub.Instance.UIManager.HudController.SetResourceText(resources[resourceType], resourceType);
        }

        public void GameOver()
        {
            IsGameStarted = false;
            IsGameFinished = true;
            GameHub.Instance.UIManager.HudController.SetResourceText(0);
            GameHub.Instance.UIManager.HudController.ShowGameOver();
            GameHub.Instance.UIManager.ChoicePopUp.gameObject.SetActive(false);
        }

        public void GameWin()
        {
            IsGameStarted = false;
            IsGameFinished = true;
            GameHub.Instance.UIManager.HudController.ShowGameWin(resources[ResourceType.Water]);
            GameHub.Instance.UIManager.ChoicePopUp.gameObject.SetActive(false);
            GameHub.Instance.Player.Win();
        }

        public void UseShield()
        {
            if (shieldPowerUpUsed) return;
            if (shieldPowerUpActive) return;
            
            var requirement =
                GameHub.Instance.PowerUpDataHolder.requirements.First(x => x.type == PowerUpType.Shield);
            if (resources[requirement.requiredResource] < requirement.resourceAmount) return;
            resources[requirement.requiredResource] -= requirement.resourceAmount;
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[requirement.requiredResource],
                requirement.requiredResource);

            OnPowerUpActive?.Invoke(PowerUpType.Shield);
            shieldPowerUpActive = true;
            shieldPowerUpUsed = true;
            _powerUpTimeLeft = GameHub.Instance.PowerUpDataHolder.powerUpDuration;
        }

        public void UseX2()
        {
            if (x2PowerUpUsed) return;
            if (x2PowerUpActive) return;
            
            var requirement =
                GameHub.Instance.PowerUpDataHolder.requirements.First(x => x.type == PowerUpType.X2);
            if (resources[requirement.requiredResource] < requirement.resourceAmount) return;
            resources[requirement.requiredResource] -= requirement.resourceAmount;
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[requirement.requiredResource],
                requirement.requiredResource);

            OnPowerUpActive?.Invoke(PowerUpType.X2);
            x2PowerUpActive = true;
            x2PowerUpUsed = true;
            _powerUpTimeLeft = GameHub.Instance.PowerUpDataHolder.powerUpDuration;
        }
    }
}