using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10f;

    private Rigidbody2D rb;

    private Vector2 movement;

    private UIManager UIManager;
    private PlayerGunManager gunManager;

    public int maxHealth;
    public int health;

    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver)
        {
            UIManager ??= GameObject.FindWithTag("UIManager")?.GetComponent<UIManager>();

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }  
    }

    private void FixedUpdate()
    {
        if(!gameOver)
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            health--;
            UIManager.UpdateHearts();

            if(health <= 0)
            {
                gameOver = true; //TEMPORARY
            }
        }
    }
}
