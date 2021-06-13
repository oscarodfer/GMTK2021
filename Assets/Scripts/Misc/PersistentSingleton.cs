using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

    }
}


