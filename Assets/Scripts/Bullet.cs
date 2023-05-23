using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [System.NonSerialized] public bool decreaseSpeed;
    private float speedDecreaseFactor;

    private void Start()
    {
        speedDecreaseFactor = Random.Range(993f, 998f);
    }
    private void Update()
    {
        if (decreaseSpeed)
        {
            GetComponent<Rigidbody2D>().velocity *= speedDecreaseFactor / 1000f;

            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < 5 && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 5)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
            Destroy(gameObject);
    }
}
