using UnityEngine;

namespace _Scripts.Utils
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance != null) return _instance;
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                return _instance;
            }
        }
    }
}