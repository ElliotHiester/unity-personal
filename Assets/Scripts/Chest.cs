using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private GameObject gunList;
    public void Opened()
    {
        gunList = GameObject.FindGameObjectWithTag("GunList");
        var gunListScript = gunList.GetComponent<GunList>();

        GameObject gunPickup = gunListScript.GetRandomGun();

        Instantiate(gunPickup, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
