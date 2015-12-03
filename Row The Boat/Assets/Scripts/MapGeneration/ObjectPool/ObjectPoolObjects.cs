using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.ObjectPool
{
    class ObjectPoolObjects : MonoBehaviour
    {
        public List<GameObject> Trees;
        public List<GameObject> Stones;

        public static ObjectPoolObjects Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }
    }
}
