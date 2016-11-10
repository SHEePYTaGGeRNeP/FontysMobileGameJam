using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseClass : MonoBehaviour
    {
        // Use something like this instead of Singleton if you want to access an object.

        //private GameManager _gameManager;
        //protected GameManager GameManager
        //{
        //    get
        //    {
        //        // Searching for it takes a long time. Might not want to do it here.
        //        if (this._gameManager == null)
        //            this.SetGameManager();
        //        return this._gameManager;
        //    }
        //}

        public static GameObject DynamicObjects { get; private set; }

        protected virtual void Awake()
        {
            if (DynamicObjects == null)
                MakeDynamicObjects();
        }
        protected virtual void Start()
        {
        }

        //protected void SetGameManager()
        //{
        //    if (this._gameManager == null)
        //        this._gameManager = GameObject.FindObjectOfType<GameManager>();
        //}

        public static void DestroyDynamicObjects()
        {
            if (DynamicObjects == null) return;
            Destroy(DynamicObjects);
            DynamicObjects = null;
        }
        public static void MakeDynamicObjects()
        {
            if (DynamicObjects != null)
                DestroyDynamicObjects();
            DynamicObjects = new GameObject("Dynamic Objects") { hideFlags = HideFlags.None };
        }

        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected GameObject InstantiateCustom(GameObject objectToInstantiate, Transform parent = null)
        {
            return InstantiateCustomStatic(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected GameObject InstantiateCustom(GameObject objectToInstantiate, Vector3 pos, Transform parent = null)
        {
            return InstantiateCustomStatic(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected GameObject InstantiateCustom(GameObject objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null)
        {
            return InstantiateCustomStatic(objectToInstantiate, pos, quaternion, parent);
        }

        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected T InstantiateCustomOfType<T>(T objectToInstantiate, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateCustomOfTypeStatic(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected T InstantiateCustomOfType<T>(T objectToInstantiate, Vector3 pos, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateCustomOfTypeStatic(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected T InstantiateCustomOfType<T>(T objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateCustomOfTypeStatic(objectToInstantiate, pos, quaternion, parent);
        }


        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static GameObject InstantiateCustomStatic(GameObject objectToInstantiate, Transform parent = null)
        {
            return InstantiateCustomStatic(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static GameObject InstantiateCustomStatic(GameObject objectToInstantiate, Vector3 pos, Transform parent = null)
        {
            return InstantiateCustomStatic(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static GameObject InstantiateCustomStatic(GameObject objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null)
        {
            if (objectToInstantiate == null)
                throw new NullReferenceException("objectToInstantiate");
            GameObject newGameObject = (GameObject)GameObject.Instantiate(objectToInstantiate, pos, quaternion);
            if (parent != null)
                newGameObject.transform.SetParent(parent);
#if UNITY_EDITOR
            else
            {
                if (DynamicObjects == null)
                    MakeDynamicObjects();
                newGameObject.transform.SetParent(DynamicObjects.transform);
            }
#endif
            return newGameObject;
        }

        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static T InstantiateCustomOfTypeStatic<T>(T objectToInstantiate, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateCustomOfTypeStatic(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static T InstantiateCustomOfTypeStatic<T>(T objectToInstantiate, Vector3 pos, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateCustomOfTypeStatic(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static T InstantiateCustomOfTypeStatic<T>(T objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null) where T : MonoBehaviour
        {
            if (objectToInstantiate == null)
                throw new NullReferenceException("objectToInstantiate");
            T newGameObject = GameObject.Instantiate<T>(objectToInstantiate);
            if (parent != null)
                newGameObject.transform.SetParent(parent);
#if UNITY_EDITOR
            else
            {
                if (DynamicObjects == null)
                    MakeDynamicObjects();
                newGameObject.transform.SetParent(DynamicObjects.transform);
            }
#endif
            newGameObject.transform.rotation = quaternion;
            newGameObject.transform.position = pos;
            return newGameObject;
        }
    }
}
