using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Controller.UI
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private HudController hudController;
        public HudController HudController => hudController;
        
    }
}