using System;
using System.Collections.Generic;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Controller.UI
{
    public class CharacterSelectionPopUp : MonoBehaviour
    {
        [SerializeField] private AnimatedPopUp animatedPopUp;
        [SerializeField] private List<CharacterCard> cards;
        
        public void SetAllCards()
        {
            cards.ForEach(x=>x.Initialize());
        }

        public void Close()
        {
            animatedPopUp.onDisable = () => { gameObject.SetActive(false); };
            animatedPopUp.Disable();
        }
    }
}