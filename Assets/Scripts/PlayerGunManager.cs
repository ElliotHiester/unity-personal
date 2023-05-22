using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{
    [System.NonSerialized] public List<GameObject> gunList = new();

    public GameObject startGun;

    [System.NonSerialized] public int currentGunIndex;

    [SerializeField] private int maxNumGuns;

    [SerializeField] private float clipRechargeTime;
    private bool clipRecharged = false;

    private UIManager UIManager;
    private UIGunManager UIGunManager;

    private bool isPickupPressed = false;

    Coroutine rechargeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        UIManager = GameObject.FindWithTag("UIManager")?.GetComponent<UIManager>();
        UIGunManager = GameObject.FindWithTag("UIGunManager")?.GetComponent<UIGunManager>();

        AddGun(startGun);
    }

    


    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isPickupPressed = true;
        } 
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isPickupPressed = false;
        }

        int previousSelected = currentGunIndex;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentGunIndex >= gunList.Count - 1)
                currentGunIndex = 0;
            else
                currentGunIndex++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentGunIndex <= 0)
                currentGunIndex = gunList.Count - 1;
            else
                currentGunIndex--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentGunIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2) && gunList.Count >= 2)
            currentGunIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3) && gunList.Count >= 3)
            currentGunIndex = 2;

        if(previousSelected != currentGunIndex)
        {
            SelectGun();
        }
            
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WeaponPickup") && isPickupPressed)
        {
            var gunScript = collision.gameObject.GetComponent<WeaponPickup>();

            AddGun(gunScript.weapon, gunScript.storedAmmo);

            Destroy(collision.gameObject);

            isPickupPressed = false;
        }
    }

    public void AddGun(GameObject gun, int storedAmmo = -1)
    {
        if(gunList.Count >= maxNumGuns)
        {
            var previousSelectedGun = gunList[currentGunIndex];
            var previousGunScript = previousSelectedGun.transform.GetChild(0)?.GetComponent<PlayerShooting>();

            gunList.RemoveAt(currentGunIndex);
            var gunPickup = Instantiate(previousGunScript.gunPickup, transform.position, Quaternion.identity); // Instantiate pickup for gun (drop pickup when removed from inventory)
            var gunPickupScript = gunPickup.GetComponent<WeaponPickup>(); //get gun pickup script
            gunPickupScript.storedAmmo = previousGunScript.currentAmmo; //store the ammount of ammo in the pickup 
            Destroy(previousSelectedGun);
        } 

        var newGun = Instantiate(gun, transform.position, Quaternion.identity);
        var newGunScript = newGun.transform.GetChild(0)?.GetComponent<PlayerShooting>();

        if (storedAmmo >= 0 && newGunScript is not null) // if storedAmmo is set and newGunScript is not null
            newGunScript.currentAmmo = storedAmmo; // set the new gun ammo equal to the ammo stored in the pickup from last use
            
        newGun.transform.SetParent(transform);
        gunList.Add(newGun);
        currentGunIndex = gunList.Count - 1;
        SelectGun();
    }

    IEnumerator ClipAmmoRecharge()
    {
        yield return new WaitForSeconds(clipRechargeTime);
        clipRecharged = true;
    }

    public void SelectGun()
    {
        var currentGunScript = gunList[currentGunIndex].transform.GetChild(0)?.GetComponent<PlayerShooting>();

        if(currentGunScript.isReloading)
            currentGunScript.StopReload();

        UIManager.StopReload();

        if(rechargeCoroutine is not null)
            StopCoroutine(rechargeCoroutine);

        rechargeCoroutine = StartCoroutine(ClipAmmoRecharge());

        for(int i = 0; i < gunList.Count; i++)
        {
            if (i != currentGunIndex)
            {
                gunList[i].SetActive(false);
            }
            else
            {
                gunList[i].SetActive(true); //if the gun is the selected gun

                if(clipRecharged)
                {
                    var newGunScript = gunList[i].transform.GetChild(0)?.GetComponent<PlayerShooting>();

                    newGunScript.RechargeAmmo();

                    clipRecharged = false;
                }
            }
        }

        UIGunManager.ChangeGun();
    }

    public void AddAmmo()
    {
        var currentGunScript = gunList[currentGunIndex].transform.GetChild(0)?.GetComponent<PlayerShooting>();
        int ammo = Convert.ToInt32(currentGunScript.maxAmmo / 7); 
        if(currentGunScript.currentAmmo + ammo > currentGunScript.maxAmmo)
        {
            currentGunScript.currentAmmo = currentGunScript.maxAmmo;
        }
        else
        {
            currentGunScript.currentAmmo += ammo;
        }
    }
}
