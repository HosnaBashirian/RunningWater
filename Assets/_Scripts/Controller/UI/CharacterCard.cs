using System;
using System.Linq;
using _Scripts.Controller.General;
using _Scripts.Models.General;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Controller.UI
{
    public class CharacterCard : MonoBehaviour
    {
        public string key;
        [SerializeField] private Image image;
        [SerializeField] private GameObject selected;
        [SerializeField] private GameObject select;
        [SerializeField] private GameObject buy;
        [SerializeField] private RTLTextMeshPro price;

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            var data = GameHub.Instance.CharacterDataHolder.characters.First(x => x.key == key);
            image.sprite = data.image;
            if (GameHub.Instance.CharacterDataHolder.selectedCharacter == key)
            {
                selected.SetActive(true);
                select.SetActive(false);
                buy.SetActive(false);
            }
            else
            {
                selected.SetActive(false);
                if (GameHub.Instance.CharacterDataHolder.purchasedCharacters.Contains(key))
                {
                    select.SetActive(true);
                    buy.SetActive(false);
                }
                else
                {
                    select.SetActive(false);
                    buy.SetActive(true);
                    price.text = data.price.ToString();
                }
            }
        }

        public void OnClick()
        {
            if (GameHub.Instance.CharacterDataHolder.selectedCharacter == key)
            {
                return;
            }

            if (GameHub.Instance.CharacterDataHolder.purchasedCharacters.Contains(key))
            {
                SelectCharacter();
                GameHub.Instance.UIManager.CharacterSelectionPopUp.SetAllCards();
            }
            else
            {
                if (GameHub.Instance.Coins >=
                    GameHub.Instance.CharacterDataHolder.characters.First(x => x.key == key).price)
                {
                    GameHub.Instance.Coins -=
                        GameHub.Instance.CharacterDataHolder.characters.First(x => x.key == key).price;
                    GameHub.Instance.UIManager.HudController.SetResourceText(GameHub.Instance.Coins, ResourceType.Coin);
                    GameHub.Instance.CharacterDataHolder.purchasedCharacters.Add(key);
                    SelectCharacter();
                    GameHub.Instance.UIManager.CharacterSelectionPopUp.SetAllCards();
                }
            }
        }

        private void SelectCharacter()
        {
            GameHub.Instance.CharacterDataHolder.selectedCharacter = key;
            GameHub.Instance.Player.ActiveSkin = GameHub.Instance.Player.Skins.FindIndex(x => x.key == key);
            GameHub.Instance.Player.SetCharacter();
        }
    }
}