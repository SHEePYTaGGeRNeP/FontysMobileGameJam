using UnityEngine;
using System.Collections;

public class MessageOverlay : MonoBehaviour
{
    public GameObject[] Objects;

    public void Start()
    {
        this.SetActive(true);
    }

    public void OnJoinedRoom()
    {
        this.SetActive(false);
    }

    public void OnLeftRoom()
    {
        this.SetActive(true);
    }

    void SetActive(bool enable)
    {
        foreach (GameObject o in this.Objects)
        {
            #if UNITY_3_5
            o.SetActiveRecursively(enable);
            #else
            o.SetActive(enable);
            #endif
        }
    }
}
