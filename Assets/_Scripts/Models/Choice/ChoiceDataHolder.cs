using System.Collections.Generic;
using _Scripts.Models.General;
using MyBox;
using UnityEngine;

namespace _Scripts.Models.Choice
{
    [CreateAssetMenu(fileName = "Choice Data", menuName = "GameData/ChoiceData", order = 0)]
    public class ChoiceDataHolder : ScriptableObject
    {
        public List<ChoiceDto> choices;

        [ButtonMethod]
        public void RemoveResource()
        {
            var resourceToRemove = ResourceType.Energy;
            foreach (var choice in choices)
            {
                choice.badChoiceResources.RemoveAll(x => x.resourceType == resourceToRemove);
                choice.goodChoiceResources.RemoveAll(x => x.resourceType == resourceToRemove);
            }
        }
    }
}