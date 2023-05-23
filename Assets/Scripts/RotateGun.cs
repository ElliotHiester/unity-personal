using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    private Camera cam;
    private PlayerController playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cam = Camera.main;
    }
    void FixedUpdate()
    {
        if(!playerScript.gameOver)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = mousePos - transform.position;
            difference.Normalize();

            float angleZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 180f;

            transform.rotation = Quaternion.Euler(0f, 0f, angleZ);
        }
    }
}
