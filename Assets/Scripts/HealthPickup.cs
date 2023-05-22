using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var playerScript = player.GetComponent<PlayerController>();

            if(playerScript.health != playerScript.maxHealth)
                playerScript.TakeDamage(-1);

            Destroy(gameObject);
        }
    }
}
