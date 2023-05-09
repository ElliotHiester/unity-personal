using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGunManager : MonoBehaviour
{
    private GameObject player;
    private GameObject overlay = null;

    // Update is called once per frame
    void Update()
    {
        player = player != null ? player : GameObject.FindWithTag("Player");
    }

    public void ChangeGun() 
    {
        var playerGunManager = player?.GetComponent<PlayerGunManager>(); // ADD START METHOD WITH WAY TO FIX THIS PROBLEM

        if (playerGunManager != null)
        {
            var gunList = playerGunManager.gunList;
            var currentGunIndex = playerGunManager.currentGunIndex;
            var currentGunScript = gunList[currentGunIndex].transform.GetChild(0).GetComponent<PlayerShooting>();
            var currentGunOverlay = currentGunScript.gunOverlay;

            if (overlay != null)
                Destroy(overlay);

            overlay = Instantiate(currentGunOverlay, transform.position, Quaternion.identity);
            overlay.transform.SetParent(transform);
            overlay.transform.localScale = transform.localScale;
        }
    }
}
