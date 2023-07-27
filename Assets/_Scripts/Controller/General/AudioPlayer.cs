using _Scripts.Models.Utils;
using UnityEngine;

namespace _Scripts.Controller.General
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SoundDataHolder soundDataHolder;
        
        public void PlayOneShot(SoundType soundType)
        {
            print($"Playing {soundType}");
            var clip = soundDataHolder.GetSound(soundType);
            audioSource.PlayOneShot(clip);
        }
    }
}