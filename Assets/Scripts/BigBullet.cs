using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : MonoBehaviour
{
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private int numSpawned;
    [SerializeField] private float bulletForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Bullet"))
        {
            SpawnCluster();
        }
    }

    public void SpawnCluster()
    {
        for(int i = 0; i < numSpawned; i++) 
        {
            float rotateAmount = 360 / numSpawned;
            transform.Rotate(0, 0, rotateAmount);
            var bullet = Instantiate(enemyBullet, transform.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(-transform.right * bulletForce, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}
