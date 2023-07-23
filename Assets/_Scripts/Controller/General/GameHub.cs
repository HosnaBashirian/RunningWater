using _Scripts.Controller.Ground;
using _Scripts.Controller.Player;
using _Scripts.Controller.UI;
using _Scripts.Models.Choice;
using _Scripts.Models.General;
using _Scripts.Models.Ground;
using _Scripts.Models.PowerUp;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Controller.General
{
    public class GameHub : MonoBehaviourSingleton<GameHub>
    {
        [SerializeField] private ChunkDataHolder chunksData;
        public ChunkDataHolder ChunksData => chunksData;

        private GroundGenerator _groundGenerator;
        public GroundGenerator GroundGenerator => _groundGenerator ??= new GroundGenerator();

        [SerializeField] private Camera mainCamera;
        public Camera MainCamera => mainCamera;
        
        [SerializeField] private PlayerController player;
        public PlayerController Player => player;
        
        [SerializeField] private UIManager uiManager;
        public UIManager UIManager => uiManager;
        
        [SerializeField] private ChoiceDataHolder choiceDataHolder;
        public ChoiceDataHolder ChoiceDataHolder => choiceDataHolder;

        [SerializeField] private PowerUpDataHolder powerUpDataHolder;
        public PowerUpDataHolder PowerUpDataHolder => powerUpDataHolder;

        [SerializeField] private ParticleData particleData;
        public ParticleData ParticleData => particleData;

        public int Coins
        {
            get
            {
                var amount = PlayerPrefs.GetInt("Coins", 0);
                return amount;
            }
            set => PlayerPrefs.SetInt("Coins", value);
        }


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}