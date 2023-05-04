using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private UIManager UIManager;
    
    [SerializeField] private Transform shootPos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public float bulletForce = 40f;
    
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    private float fireCounter = 0.0f;

    [SerializeField] private int maxClipAmmo;
    private int currentClipAmmo;

    [SerializeField] private int maxAmmo;
    private int currentAmmo;

    [SerializeField] private float shotSpread;
    private float accuracy;

    [SerializeField] private bool isAutomatic;
    
    private bool isReloading = false;
    Coroutine reloadDelay;

    protected virtual void Start()
    {
        UIManager = GameObject.FindWithTag("UIManager")?.GetComponent<UIManager>();

        currentAmmo = maxAmmo;
        currentClipAmmo = maxClipAmmo;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        fireCounter += Time.deltaTime;
        if((currentAmmo > 0) && !isReloading && (fireCounter > fireRate))
        {
            if(!isAutomatic && Input.GetMouseButtonDown(0))
            {
                fireCounter = 0.0f;
                Shoot();
            }
            
            if(isAutomatic && Input.GetMouseButton(0))
            {
                fireCounter = 0.0f;
                Shoot();
            }
        }

        if (!isReloading && (currentClipAmmo != maxClipAmmo) && ((currentClipAmmo <= 0 && currentAmmo != 0) || Input.GetMouseButtonDown(1)))
        {
            Reload();
        }
    }

    IEnumerator ReloadDelay()
    {
        isReloading = true;
        UIManager.Reload(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        currentClipAmmo = (currentAmmo > maxClipAmmo) ? maxClipAmmo : currentAmmo;
        isReloading = false;
    }

    protected virtual void Reload()
    {
        reloadDelay = StartCoroutine(ReloadDelay());
    }

    protected virtual void Shoot()
    {
        currentAmmo--;
        currentClipAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        accuracy = Random.Range(-shotSpread, shotSpread); 

        shootPos.Rotate(0, 0, accuracy); //randomize shootPos rotation for accuracy customization
        bulletRb.AddForce(-shootPos.right * bulletForce, ForceMode2D.Impulse);
        shootPos.Rotate(0, 0, -accuracy); //reset shootPos rotation
    }
}
