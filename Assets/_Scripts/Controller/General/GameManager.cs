using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Controller.UI;
using _Scripts.Models.General;
using _Scripts.Models.Ground;
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
            {ResourceType.Coin, 0},
            {ResourceType.Happiness, 50},
        };


        public bool IsGameStarted { get; private set; } = false;
        public bool IsGameFinished { get; private set; } = false;

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
            resources[ResourceType.Coin] = 0;
            resources[ResourceType.Happiness] = 50;
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Water], ResourceType.Water);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Energy], ResourceType.Energy);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Coin], ResourceType.Coin);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Happiness], ResourceType.Happiness);
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
            resources[ResourceType.Coin] = 0;
            resources[ResourceType.Happiness] = 50;
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Water], ResourceType.Water);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Energy], ResourceType.Energy);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Coin], ResourceType.Coin);
            GameHub.Instance.UIManager.HudController.SetResourceText(resources[ResourceType.Happiness], ResourceType.Happiness);
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
        }

        public void GameWin()
        {
            IsGameStarted = false;
            IsGameFinished = true;
            GameHub.Instance.UIManager.HudController.ShowGameWin(resources[ResourceType.Water]);
        }
    }
}