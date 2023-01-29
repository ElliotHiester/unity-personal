using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    void FixedUpdate()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = mousePos - transform.position;
        difference.Normalize();

        float angleZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 180f;

        transform.rotation = Quaternion.Euler(0f, 0f, angleZ);
    }
}
