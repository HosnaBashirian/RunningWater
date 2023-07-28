using System;
using System.Collections.Generic;
using _Scripts.Controller.General;
using _Scripts.Models.Choice;
using _Scripts.Models.General;
using _Scripts.Utils;
using RTLTMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Controller.UI
{
    public class ChoicePopUp : MonoBehaviour
    {
        [SerializeField] private AnimatedPopUp popUp;
        [SerializeField] private RTLTextMeshPro title;
        [SerializeField] private RTLTextMeshPro description;
        [SerializeField] private RTLTextMeshPro goodChoiceText;
        [SerializeField] private RTLTextMeshPro badChoiceText;

        [SerializeField] private List<ChoiceResourceTextHolder> goodChoiceResources;
        [SerializeField] private List<ChoiceResourceTextHolder> badChoiceResources;

        private ChoiceDto _choiceDto;

        private bool flipped = false;
        
        public void Set(ChoiceDto data)
        {
            flipped = Random.Range(0, 100) > 50;
            _choiceDto = data;
            // title.text = data.title;
            title.text = "";
            description.text = data.description;
            goodChoiceText.text = flipped ? data.goodChoiceText : data.badChoiceText;
            badChoiceText.text = flipped ? data.badChoiceText : data.goodChoiceText;

            goodChoiceResources.ForEach(x => x.gameObject.SetActive(false));
            badChoiceResources.ForEach(x => x.gameObject.SetActive(false));

            var plus = "+";
            var minus = "-";
            
            foreach (var resource in flipped ? data.goodChoiceResources : data.badChoiceResources)
            {
                var r = goodChoiceResources.Find(x => x.resourceType == resource.resourceType);
                if (r == null) continue;
                r.gameObject.SetActive(true);
                r.text.text = $"{(resource.amount > 0 ? plus : minus)}{Mathf.Abs(resource.amount)}";
            }

            foreach (var resource in flipped ? data.badChoiceResources : data.goodChoiceResources)
            {
                var r = badChoiceResources.Find(x => x.resourceType == resource.resourceType);
                if (r == null) continue;
                r.gameObject.SetActive(true);
                r.text.text = $"{(resource.amount > 0 ? plus : minus)}{Mathf.Abs(resource.amount)}";
            }
        }

        public void GoodChoice()
        {
            foreach (var resource in flipped ? _choiceDto.goodChoiceResources : _choiceDto.badChoiceResources)
            {
                GameManager.Instance.AddResource(resource.amount, resource.resourceType);
            }
            GameHub.Instance.Player.IsPlayerPaused = false;
            popUp.onDisable = () => { gameObject.SetActive(false); };
            popUp.Disable();
        }
        
        public void BadChoice()
        {
            foreach (var resource in flipped ? _choiceDto.badChoiceResources : _choiceDto.goodChoiceResources)
            {
                GameManager.Instance.AddResource(resource.amount, resource.resourceType);
            }
            GameHub.Instance.Player.IsPlayerPaused = false;
            popUp.onDisable = () => { gameObject.SetActive(false); };
            popUp.Disable();
        }
    }

    [Serializable]
    public class ChoiceResourceTextHolder
    {
        public ResourceType resourceType;
        public GameObject gameObject;
        public RTLTextMeshPro text;
    }
}