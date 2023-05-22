using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform shootPos;
    [SerializeField] private GameObject origin;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotSpread;
    [SerializeField] private float bulletForce;
    [SerializeField] private ParticleSystem shootParticle;
    private GameObject enemyObj;

    // Start is called before the first frame update
    void Start()
    {
        enemyObj = origin.transform.parent.gameObject;
    }

    public void Shoot()
    {
        shootParticle.Play();
        GameObject bullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        var accuracy = Random.Range(-shotSpread, shotSpread);

        shootPos.Rotate(0, 0, accuracy);
        bulletRb.AddForce(-shootPos.right * bulletForce, ForceMode2D.Impulse);
        shootPos.Rotate(0, 0, -accuracy);

        //kickback
    }
}
