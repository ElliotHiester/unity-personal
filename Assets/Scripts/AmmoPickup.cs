using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerGunScript = player.GetComponent<PlayerGunManager>();
            playerGunScript.AddAmmo();

            Destroy(gameObject);
        }
    }
}
