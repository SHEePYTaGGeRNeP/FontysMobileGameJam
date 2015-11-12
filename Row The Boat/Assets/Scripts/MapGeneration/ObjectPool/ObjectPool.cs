using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.ObjectPool
{
    class ObjectPool
    {
        private List<PoolList> objectList;

        public Vector3 Poolpoint = new Vector3(0, 0, -10);

        private static ObjectPool instance;
        public static ObjectPool GetInstance()
        {
            if (instance == null)
            {
                instance = new ObjectPool();
                instance.objectList = new List<PoolList>();
            }

            return instance;
        }

        public GameObject GetObject(GameObjectType type)
        {
            PoolList list = GetList(type);
            if (list != null)
            {
                return list.GetNextAvaiableObject();
            }

            objectList.Add(new PoolList(type));
            return GetObject(type);
        }

        public GameObject CreateNewObject(GameObjectType type)
        {
            GameObject go = null;
            switch (type)
            {
                case GameObjectType.Dirt:
                    go = GameObject.Instantiate(MapGenerator.GetInstance().DirtObject);
                    break;
                case GameObjectType.Water:
                    go = GameObject.Instantiate(MapGenerator.GetInstance().WaterObject);
                    break;
                case GameObjectType.Oever:
                    go = GameObject.Instantiate(MapGenerator.GetInstance().DirtSideObject);
                    break;
            }

            if (go != null)
            {
                PoolList list = GetList(type);
                if (list != null)
                {
                    list.AddGameObject(go);
                }
            }

            return go;
        }

        public void SetBeschikbaar(GameObject obj)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                int result = objectList[i].Contains(obj);
                if (result != -1)
                {
                    objectList[i].SetBeschikbaar(result);
                    return;
                }
            }
        }

        private PoolList GetList(GameObjectType type)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i].GetGameObjectType() == type)
                {
                    return objectList[i];
                }
            }

            return null;
        }
    }
}
