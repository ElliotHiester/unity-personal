using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : PlayerShooting
{
    [SerializeField] private int numBullets;
    protected override void Shoot()
    {
        currentClipAmmo--;
        currentAmmo--;
        shootParticle.Play();
        cameraShake.Shake(shakePreset);

        shootPos.Rotate(0, 0, (shotSpread * numBullets) / 2);
        for (int i = 0; i < numBullets; i++)
        {
            var bullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
            var bulletRb = bullet.GetComponent<Rigidbody2D>();
            var bulletScript = bullet.GetComponent<Bullet>();

            bulletRb.AddForce(-shootPos.right * bulletForce, ForceMode2D.Impulse);
            bulletScript.decreaseSpeed = true;

            playerRb.AddForce(shootPos.transform.right * kickBack, ForceMode2D.Force);

            shootPos.Rotate(0, 0, -shotSpread);

        }

        shootPos.Rotate(0, 0, (shotSpread * numBullets) / 2);
    }
}
