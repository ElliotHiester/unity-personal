using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnemyGun : MonoBehaviour
{
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        player = player != null ? player : GameObject.FindWithTag("Player");
    }

    public void Rotate()
    {
        if (player != null)
        {
            var difference = (player.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        }
    }

    public void Rotate(Vector3 direction)
    {
        direction.Normalize();
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, -angle);
    }

    public void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -angle);
    }
}
