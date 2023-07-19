using System;
using System.Threading.Tasks;
using _Scripts.Controller.UI;
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
        private int currentResource = 10;
        
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
            currentResource = 10;
            UIManager.Instance.HudController.SetResourceText(currentResource);
            DelayedReset();
        }

        private async void DelayedReset()
        {
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
            UIManager.Instance.HudController.HideGameOver();
            IsGameStarted = false;
            IsGameFinished = false;
            currentResource = 10;
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
        
        public void MultiplyResource(float value)
        {
            // print("MultiplyResource by " + value + "");
            currentResource = (int) (Convert.ToSingle(currentResource) * value);
            if (currentResource <= 0)
            {
                currentResource = 0;
                GameOver();
            }
            UIManager.Instance.HudController.SetResourceText(currentResource);
        }
        
        public void AddResource(int value)
        {
            currentResource += value;
            if (currentResource <= 0)
            {
                currentResource = 0;
                GameOver();
            }
            UIManager.Instance.HudController.SetResourceText(currentResource);
        }
        
        public void GameOver()
        {
            IsGameStarted = false;
            IsGameFinished = true;
            UIManager.Instance.HudController.SetResourceText(0);
            UIManager.Instance.HudController.ShowGameOver();
        }
        
        public void GameWin()
        {
            IsGameStarted = false;
            IsGameFinished = true;
            UIManager.Instance.HudController.ShowGameWin(currentResource);
        }
    }
}