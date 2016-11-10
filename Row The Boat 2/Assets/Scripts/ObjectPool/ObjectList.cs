using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ObjectList
{
    #region "Fields"

    private GameObject prefab;
    private string name;
    private List<GameObject> available;
    private int uid;

    private Transform parent;

    private static int nextUID = 0;

    #endregion

    #region "Constructors"

    public ObjectList(PoolObject source, Transform parent)
    {
        prefab = source.prefab;
        name = source.name;
        available = new List<GameObject>();
        this.parent = parent;

        uid = nextUID;
        nextUID++;

        for (int i = 0; i < source.defaultAmount; i++)
        {
            CreateNewInstance();
        }
    }

    #endregion

    #region "Properties"

    public string Name
    {
        get { return name; }
    }

    public int UID
    {
        get { return uid; }
    }

    #endregion

    #region "Methods"

    private void CreateNewInstance()
    {
        GameObject newObj = UnityEngine.Object.Instantiate(prefab);
        newObj.AddComponent<PoolIdentifier>().UID = uid;
        newObj.transform.parent = parent;
        newObj.SetActive(false);

        available.Add(newObj);
    }

    public GameObject GetNext()
    {
        GameObject next = available.FirstOrDefault();
        available.Remove(next);

        if (next == null)
        {
            CreateNewInstance();
            next = GetNext();
        }

        next.SetActive(true);
        next.transform.parent = null;

        return next;
    }

    public void GiveBackObject(GameObject obj)
    {
        available.Add(obj);
        obj.SetActive(false);
        obj.transform.parent = parent;
    }

    #endregion

    #region "Static Methods"



    #endregion

    #region "Inherited Methods"



    #endregion
}
