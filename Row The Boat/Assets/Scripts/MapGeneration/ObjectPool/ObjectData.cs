using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.ObjectPool
{
    class ObjectData
    {
        public GameObject Obj;
        public Transform Transform;

        public ObjectData(GameObject obj)
        {
            this.Obj = obj;
            this.Transform = obj.transform;
        }

        public bool IsBeschikbaar()
        {
            return this.Transform.position.z <= -9;
        }
    }
}
