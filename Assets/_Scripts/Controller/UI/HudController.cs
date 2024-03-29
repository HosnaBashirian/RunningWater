using System;
using System.Linq;
using _Scripts.Controller.General;
using _Scripts.Models.General;
using _Scripts.Models.PowerUp;
using _Scripts.Utils;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Controller.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject resourcesPanel;
        [SerializeField] private GameObject powerUpPanel;
        [SerializeField] private TextMeshProUGUI waterResourceText;
        [SerializeField] private TextMeshProUGUI coinResourceText;
        [SerializeField] private TextMeshProUGUI energyResourceText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private RTLTextMeshPro gameOverTitleText;
        [SerializeField] private RTLTextMeshPro gameWinScoreText;

        [SerializeField] private GameObject powerUps;
        public GameObject PowerUps => powerUps;
        [SerializeField] private Button x2PowerUpButton;
        [SerializeField] private Button shieldPowerUpButton;
        [SerializeField] private RTLTextMeshPro x2PowerUpPrice;
        [SerializeField] private RTLTextMeshPro shieldPowerUpPrice;

        [SerializeField] private GameObject activePowerUp;
        [SerializeField] private RTLTextMeshPro activePowerUpText;
        [SerializeField] private Slider activePowerUpSlider;
        
        [SerializeField] private GameObject characterSelectionPanel;

        [SerializeField] private RTLTextMeshPro gameOverButtonText;
        [SerializeField] private RTLTextMeshPro levelText;
        

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
            }
        }

        private void OnEnable()
        {
            levelText.text = $"Level {GameHub.Instance.Level}";
        }

        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
            gameOverTitleText.text = $"GAME OVER";
            gameWinScoreText.text = $"";
            gameOverButtonText.text = $"Try Again";
            isWin = false;
        }

        private bool isWin = false;

        public void ShowGameWin(int score)
        {
            gameOverPanel.SetActive(true);
            gameOverTitleText.text = $"VICOTRY";
            gameWinScoreText.text = " AWESOME! " + "You Saved " + score + " Litres of Water." + " " + score + " Coins Earned!";
            gameOverButtonText.text = $"Next Level";
            isWin = true;
        }

        public void HideGameOver()
        {
            gameOverPanel.SetActive(false);
        }

        public void OnRequestRestart()
        {
            GameHub.Instance.Level += isWin ? 1 : 0;
            HideGameOver();
            GameManager.Instance.InitializeGame();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGameStarted)
            {
                startPanel.SetActive(false);
                resourcesPanel.SetActive(true);
                powerUpPanel.SetActive(true);
                UpdatePowerUpButtons();
            }
            else
            {
                if (GameManager.Instance.IsGameFinished)
                {
                    startPanel.SetActive(false);
                    resourcesPanel.SetActive(false);
                    powerUpPanel.SetActive(false);
                    activePowerUp.SetActive(false);
                }
                else
                {
                    startPanel.SetActive(true);
                    levelText.text = $"Level {GameHub.Instance.Level}";
                    resourcesPanel.SetActive(true);
                    powerUpPanel.SetActive(false);
                    activePowerUp.SetActive(false);
                }
            }
        }

        private void UpdatePowerUpButtons()
        {
            if (!GameManager.Instance.IsGameStarted)
            {
                powerUps.SetActive(false);
                powerUpPanel.SetActive(false);
                activePowerUp.SetActive(false);
                return;
            }
            activePowerUp.SetActive(false);
            powerUps.SetActive(true);
            if (GameManager.Instance.shieldPowerUpActive || GameManager.Instance.x2PowerUpActive)
            {
                activePowerUp.SetActive(true);
                activePowerUpText.text = GameManager.Instance.shieldPowerUpActive ? $"Shield" : $"x2";
                activePowerUpSlider.value = GameManager.Instance.PowerUpTimeLeft;
            }
            var x2Requirement = GameHub.Instance.PowerUpDataHolder.requirements.First(x => x.type == PowerUpType.X2);
            x2PowerUpPrice.text = $"{x2Requirement.resourceAmount}";
            x2PowerUpButton.interactable =
                GameHub.Instance.Coins >= x2Requirement.resourceAmount &&
                !GameManager.Instance.x2PowerUpUsed;

            var shieldRequirement =
                GameHub.Instance.PowerUpDataHolder.requirements.First(x => x.type == PowerUpType.Shield);
            shieldPowerUpPrice.text = $"{shieldRequirement.resourceAmount}";
            shieldPowerUpButton.interactable =
                GameHub.Instance.Coins >=
                shieldRequirement.resourceAmount &&
                !GameManager.Instance.shieldPowerUpUsed;
        }

        public void UseShield()
        {
            GameManager.Instance.UseShield();
        }
        
        public void UseX2()
        {
            GameManager.Instance.UseX2();
        }

        public void OpenCharacterSelectionPanel()
        {
            characterSelectionPanel.SetActive(true);
        }
    }
}