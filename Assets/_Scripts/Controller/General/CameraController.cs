using System;
using UnityEngine;

namespace _Scripts.Controller.General
{
    public class CameraController : MonoBehaviour
    {
        private Vector3 _offset;
        
        private void Start()
        {
            var player = GameHub.Instance.Player.transform;
            _offset = transform.position - player.position;
        }
        
        private void LateUpdate()
        {
            var player = GameHub.Instance.Player.transform;
            var position = player.position + _offset;
            // set x to 0
            position.x = 0;
            transform.position = position;
        }
    }
}