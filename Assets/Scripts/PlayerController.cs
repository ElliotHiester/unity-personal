using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10f;

    private Rigidbody2D rb;

    private Vector2 movement;

    private UIManager UIManager;

    public int maxHealth;
    public int health;

    private bool gameOver;

    [System.NonSerialized] public int killCombo = 0;
    private float killComboTimer;
    [SerializeField] private float maxComboTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        killComboTimer += Time.deltaTime;
        
        if(killComboTimer >= maxComboTime)
        {
            killCombo = 0;
        }

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
            TakeDamage();
        }
    }

    public void TakeDamage(int amount = 1)
    {
        health -= amount;
        UIManager.UpdateHearts();

        if (health <= 0)
        {
            gameOver = true; //TEMPORARY
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Chest"))
        {
            var chestScript = collision.gameObject.GetComponent<Chest>();
            chestScript.Opened();
        }
    }

    public void KilledEnemy()
    {
        killCombo++;
        killComboTimer = 0;
        UIManager.Combo(maxComboTime);
    }
}
