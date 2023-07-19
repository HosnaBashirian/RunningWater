using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Models.Choice
{
    [CreateAssetMenu(fileName = "Choice Data", menuName = "GameData/ChoiceData", order = 0)]
    public class ChoiceDataHolder : ScriptableObject
    {
        public List<ChoiceDto> choices;
    }
}