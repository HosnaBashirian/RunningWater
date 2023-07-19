using System;
using _Scripts.Controller.General;
using _Scripts.Models.General;
using _Scripts.Utils;
using TMPro;
using UnityEngine;

namespace _Scripts.Controller.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waterResourceText;
        [SerializeField] private TextMeshProUGUI coinResourceText;
        [SerializeField] private TextMeshProUGUI energyResourceText;
        [SerializeField] private TextMeshProUGUI happinessResourceText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverTitleText;
        [SerializeField] private TextMeshProUGUI gameWinScoreText;
        
        public void SetResourceText(int value, ResourceType resourceType = ResourceType.Water)
        {
            switch (resourceType)
            {
                case ResourceType.Water:
                    waterResourceText.DoNumberText(value, 0.2f);
                    break;
                case ResourceType.Coin:
                    coinResourceText.DoNumberText(value, 0.2f);
                    break;
                case ResourceType.Energy:
                    energyResourceText.DoNumberText(value, 0.2f);
                    break;
                case ResourceType.Happiness:
                    happinessResourceText.DoNumberText(value, 0.2f);
                    break;
            }
        }
        
        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
            gameOverTitleText.text = $"Game Over";
            gameWinScoreText.text = $"";
        }
        
        public void ShowGameWin(int score)
        {
            gameOverPanel.SetActive(true);
            gameOverTitleText.text = $"Victory!";
            gameWinScoreText.text = $"congrats";
        }
        
        public void HideGameOver()
        {
            gameOverPanel.SetActive(false);
        }
        
        public void OnRequestRestart()
        {
            HideGameOver();
            GameManager.Instance.InitializeGame();
        }
    }
}