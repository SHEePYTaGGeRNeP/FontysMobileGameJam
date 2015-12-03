using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.ObjectPool
{
    class PoolList
    {
        private List<ObjectData> list;
        private GameObjectType type;

        public PoolList(GameObjectType type)
        {
            this.type = type;
            this.list = new List<ObjectData>();
        }

        public GameObjectType GetGameObjectType()
        {
            return this.type;
        }

        public GameObject GetNextAvaiableObject()
        {
            // Check the list for an object that is not used
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].IsBeschikbaar())
                {
                    return this.list[i].obj;
                }
            }

            // No object is avaiable, create a new one
            return ObjectPool.GetInstance().CreateNewObject(this.type);
        }

        public void AddGameObject(GameObject obj)
        {
            this.list.Add(new ObjectData(obj));
        }

        public int Contains(GameObject obj)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].obj == obj)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetBeschikbaar(int obj)
        {
            this.list[obj].transform.position = new Vector3(this.list[obj].transform.position.x, this.list[obj].transform.position.y, -10);
            this.list[obj].transform.parent = MapGenerator.GetInstance().ObjectPoolHolder.transform;
        }

        public int GetObjectCount()
        {
            return this.list.Count;
        }
    }
}
