using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private UIManager UIManager;
    
    [SerializeField] protected Transform shootPos;
    [SerializeField] protected GameObject bulletPrefab;
    public float bulletForce = 40f;
    
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    private float fireCounter = 0.0f;

    [SerializeField] private int maxClipAmmo;
    [System.NonSerialized] public int currentClipAmmo;

    public int maxAmmo;
    [System.NonSerialized] public int currentAmmo = -1;

    [SerializeField] protected float shotSpread;

    [SerializeField] protected float kickBack;

    [SerializeField] private bool isAutomatic;
    
    [System.NonSerialized] public bool isReloading = false;

    public GameObject gunOverlay;
    public GameObject gunPickup;

    protected Rigidbody2D playerRb;

    private Coroutine reloadCoroutine;

    protected virtual void Start()
    {       
        UIManager = GameObject.FindWithTag("UIManager")?.GetComponent<UIManager>();

        if (currentAmmo < 0) // if ammo has not been set by the pickup, set it to the max ammo
        {
            currentAmmo = maxAmmo;
        } 

        currentClipAmmo = maxClipAmmo;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        playerRb ??= GameObject.FindWithTag("Player")?.GetComponent<Rigidbody2D>();

        UIManager.maxAmmoDisplay.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        UIManager.clipAmmoSlider.maxValue = maxClipAmmo;
        UIManager.clipAmmoSlider.value = currentClipAmmo;

        fireCounter += Time.deltaTime;
        if ((currentAmmo > 0) && !isReloading && (fireCounter > fireRate)) 
        {
            if (currentClipAmmo == 0 && Input.GetMouseButton(0))
            {
                Reload();
            }
            else
            {
                if (!isAutomatic && Input.GetMouseButtonDown(0))
                {
                    fireCounter = 0.0f;
                    Shoot();
                }

                if (isAutomatic && Input.GetMouseButton(0))
                {
                    fireCounter = 0.0f;
                    Shoot();
                }
            }
        }

        if (!isReloading
            && (currentClipAmmo != maxClipAmmo)
            && ((currentClipAmmo < 0 && currentAmmo != 0 && Input.GetMouseButtonDown(0)) || Input.GetMouseButtonDown(1) && currentAmmo != 0))
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
        reloadCoroutine = StartCoroutine(ReloadDelay());
    }

    public virtual void StopReload()
    {
        isReloading = false;
        if(reloadCoroutine is not null)
            StopCoroutine(reloadCoroutine);
    }

    public virtual void RechargeAmmo() //after 5 seconds of gun not being used
    {
        currentClipAmmo = (currentAmmo > maxClipAmmo) ? maxClipAmmo : currentAmmo;
    }

    protected virtual void Shoot()
    {
        currentClipAmmo--;
        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
        //Destroy(bullet, 2f);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        var accuracy = Random.Range(-shotSpread, shotSpread);

        shootPos.Rotate(0, 0, accuracy); //randomize shootPos rotation for accuracy customization
        bulletRb.AddForce(-shootPos.right * bulletForce, ForceMode2D.Impulse);
        shootPos.Rotate(0, 0, -accuracy); //reset shootPos rotation

        playerRb.AddForce(shootPos.transform.right * kickBack, ForceMode2D.Force);
    }
}
