using System.Collections.Generic;
using DG.Tweening;
using TMPro;

namespace _Scripts.Utils
{
    public static class Extensions
    {
        public static T Random<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        
        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static Tweener DoNumberText(this TextMeshProUGUI text, int value, float duration)
        {
            _ = int.TryParse(text.text, out var current);
            return DOTween.To(() => current, x =>
            {
                current = x;
                text.text = x.ToString();
            }, value, duration);
        }
    }
}