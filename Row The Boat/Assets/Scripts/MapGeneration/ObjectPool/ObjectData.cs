using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.ObjectPool
{
    class ObjectData
    {
        public GameObject obj;
        public Transform transform;

        public ObjectData(GameObject obj)
        {
            this.obj = obj;
            this.transform = obj.transform;
        }

        public bool IsBeschikbaar()
        {
            return transform.position.z <= -9;
        }
    }
}
