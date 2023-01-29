using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MagicBoard
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                    Debug.LogError(typeof(T).ToString() + " is Null!");
                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this as T;

            Init();
        }

        
        public virtual void Init()
        {
            //in case you need to init something in awake
        }
    }
}