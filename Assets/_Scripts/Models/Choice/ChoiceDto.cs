using System;
using System.Collections.Generic;
using _Scripts.Models.General;
using UnityEngine;

namespace _Scripts.Models.Choice
{
    [Serializable]
    public class ChoiceDto 
    {
        // public string title;
        public string description;
        public string goodChoiceText;
        public string badChoiceText;
        public List<ChoiceResourceDto> goodChoiceResources;
        public List<ChoiceResourceDto> badChoiceResources;
    }
    
    [Serializable]
    public class ChoiceResourceDto
    {
        public ResourceType resourceType;
        public int amount;
    }
}