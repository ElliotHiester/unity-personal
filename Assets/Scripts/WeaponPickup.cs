using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weapon;

    [System.NonSerialized] public int storedAmmo = -1;

    private void Start()
    {
        if(storedAmmo < 0)
        {
            var weaponScript = weapon.transform.GetChild(0).GetComponent<PlayerShooting>();
            storedAmmo = weaponScript.maxAmmo;
        }
    }
}
