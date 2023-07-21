using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Controller.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HudController hudController;
        public HudController HudController => hudController;
        
        [SerializeField] private ChoicePopUp choicePopUp;
        public ChoicePopUp ChoicePopUp => choicePopUp;
        
        
    }
}