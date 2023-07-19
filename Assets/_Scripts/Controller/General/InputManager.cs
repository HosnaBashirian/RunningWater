using System;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Controller.General
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        public event Action<float> OnHorizontalSwipe;
        private Vector3 _lastMousePosition;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                 _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                var delta = Input.mousePosition - _lastMousePosition;
                if (Mathf.Abs(delta.x) > 0.1f)
                {
                    OnHorizontalSwipe?.Invoke(delta.x);
                }
                _lastMousePosition = Input.mousePosition;
            }
        }
    }
}