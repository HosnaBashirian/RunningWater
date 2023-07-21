using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Controller.Player
{
    [RequireComponent(typeof(Animation))]
    public class PlayerSkin : MonoBehaviour
    {
        public string key;
        public AnimationClip idle;
        public AnimationClip run;
        public AnimationClip victory;

        private Animation _animation;
        private Animation Animation
        {
            get
            {
                if (_animation == null)
                {
                    _animation = GetComponent<Animation>();
                }

                return _animation;
            }
        }
        
        private async void Start()
        {
            await Task.Delay(100);
            Animation.AddClip(idle, "idle");
            Animation.AddClip(run, "run");
            Animation.AddClip(victory, "victory");
            Animation.Play("idle");
        }

        public void Run()
        {
            Animation.CrossFade("run", 0.25f);
        }

        public void Idle()
        {
            Animation.CrossFade("idle", 0.25f);
        }

        public void Victory()
        {
            Animation.CrossFade("victory", 0.25f);
        }
    }

}