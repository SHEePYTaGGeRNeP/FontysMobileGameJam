﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.ObjectPool
{
    using Object = UnityEngine.Object;

    class ObjectPool
    {
        private List<PoolList> _objectList;

        public Vector3 Poolpoint = new Vector3(0, 0, -10);

        private static ObjectPool instance;
        public static ObjectPool GetInstance()
        {
            if (instance == null)
            {
                instance = new ObjectPool();
                instance._objectList = new List<PoolList>();
            }

            return instance;
        }

        public GameObject GetObject(GameObjectType type)
        {
            PoolList list = this.GetList(type);
            if (list != null)
            {
                return list.GetNextAvaiableObject();
            }

            this._objectList.Add(new PoolList(type));
            return this.GetObject(type);
        }

        public GameObject CreateNewObject(GameObjectType type)
        {
            GameObject go = null;
            switch (type)
            {
                case GameObjectType.Stone:
                    go = GameObject.Instantiate(ObjectPoolObjects.Instance.Stones[UnityEngine.Random.Range(0, ObjectPoolObjects.Instance.Stones.Count)]);
                    break;
                case GameObjectType.Tree:
                    go = GameObject.Instantiate(ObjectPoolObjects.Instance.Trees[UnityEngine.Random.Range(0, ObjectPoolObjects.Instance.Trees.Count)]);
                    break;
                case GameObjectType.Decoration:
                    //go = GameObject.Instantiate(MapGenerator.GetInstance().decoration[UnityEngine.Random.Range(0, MapGenerator.GetInstance().decoration.Count)]);
                    break;
            }

            if (go != null)
            {
                PoolList list = this.GetList(type);
                if (list != null)
                {
                    list.AddGameObject(go);
                }
            }

            return go;
        }

        public void SetBeschikbaar(GameObject obj)
        {
            for (int i = 0; i < this._objectList.Count; i++)
            {
                int result = this._objectList[i].Contains(obj);
                if (result != -1)
                {
                    this._objectList[i].SetBeschikbaar(result);
                    return;
                }
            }
        }

        private PoolList GetList(GameObjectType type)
        {
            for (int i = 0; i < this._objectList.Count; i++)
            {
                if (this._objectList[i].GetGameObjectType() == type)
                {
                    return this._objectList[i];
                }
            }

            return null;
        }
    }
}
