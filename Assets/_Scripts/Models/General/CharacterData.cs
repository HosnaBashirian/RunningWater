using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Models.General
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "GameData/Characters", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public List<CharacterDto> characters;

        public string selectedCharacter = "casual_male";
        public List<string> purchasedCharacters = new List<string>() {"casual_male", "casual_girl"};
        
    }

    [Serializable]
    public class CharacterDto
    {
        public string key;
        public Sprite image;
        public int price;
    }
}