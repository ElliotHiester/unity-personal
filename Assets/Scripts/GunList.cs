using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunList : MonoBehaviour
{
    public List<GameObject> gunList = new List<GameObject>();

    public GameObject GetRandomGun()
    {
        return gunList[Random.Range(0, gunList.Count)];
    }
}
