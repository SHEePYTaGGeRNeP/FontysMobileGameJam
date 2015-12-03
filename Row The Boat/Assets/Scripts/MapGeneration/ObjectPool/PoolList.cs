using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.ObjectPool
{
    class PoolList
    {
        private List<ObjectData> _list;
        private GameObjectType _type;

        public PoolList(GameObjectType type)
        {
            this._type = type;
            this._list = new List<ObjectData>();
        }

        public GameObjectType GetGameObjectType()
        {
            return this._type;
        }

        public GameObject GetNextAvaiableObject()
        {
            // Check the list for an object that is not used
            for (int i = 0; i < this._list.Count; i++)
            {
                if (this._list[i].IsBeschikbaar())
                {
                    return this._list[i].Obj;
                }
            }

            // No object is avaiable, create a new one
            return ObjectPool.GetInstance().CreateNewObject(this._type);
        }

        public void AddGameObject(GameObject obj)
        {
            this._list.Add(new ObjectData(obj));
        }

        public int Contains(GameObject obj)
        {
            for (int i = 0; i < this._list.Count; i++)
            {
                if (this._list[i].Obj == obj)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetBeschikbaar(int obj)
        {
            this._list[obj].Transform.position = new Vector3(this._list[obj].Transform.position.x, this._list[obj].Transform.position.y, -10);
            this._list[obj].Transform.parent = MapGenerator.GetInstance().ObjectPoolHolder.transform;
        }

        public int GetObjectCount()
        {
            return this._list.Count;
        }
    }
}
