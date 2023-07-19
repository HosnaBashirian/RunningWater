using _Scripts.Controller.General;
using _Scripts.Utils;
using TMPro;
using UnityEngine;

namespace _Scripts.Controller.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resourceText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverTitleText;
        [SerializeField] private TextMeshProUGUI gameWinScoreText;
        
        public void SetResourceText(int value)
        {
            resourceText.DoNumberText(value, 0.2f);
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
            gameWinScoreText.text = $"You gained {score} liters of water.";
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