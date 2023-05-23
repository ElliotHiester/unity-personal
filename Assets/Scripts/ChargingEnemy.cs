using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    [SerializeField] private int health;

    [System.NonSerialized] public States currentState;

    //idle vars
    private float idleMoveRate; //time in between moving
    private float idleMoveLength; //how long the enemy moves for in idle
    [SerializeField] private float idleMoveSpeed;

    //aggressive vars
    [SerializeField] private float aggMoveSpeed;

    [SerializeField] private float rayDistance;
    private bool move;
    private Vector3 direction;
    private float moveTimer = 0;

    private GameObject player;

    private Vector2 playerDir;

    private bool colliding;

    [SerializeField] private GameObject ammoPickup;
    [SerializeField] private GameObject healthPickup;
    [SerializeField] private float maxPickupChance;
    [SerializeField] private float pickupChanceFactor;
    [SerializeField] private float healthChanceFactor;
    [SerializeField] private float maxHealthChance;

    [SerializeField] private ParticleSystem deathParticle;

    [SerializeField] private Color enemyColor;
    [SerializeField] private Color hitColor;

    public enum States
    {
        Idle,
        Aggressive
    }

    // Start is called before the first frame update
    void Start()
    {
        //player = player != null ? player : GameObject.FindWithTag("Player");

        currentState = States.Idle;

        idleMoveRate = Random.Range(0f, 8f);
        idleMoveLength = Random.Range(0.3f, 2f);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        player = player != null ? player : GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerDir = player.transform.position - transform.position;

            var layerMask = ~((1 << 2) | (1 << 6) | (1 << 7) | (1 << 8)); // ray looks for every layer EXCEPT the 'Ignore Raycast', 'Bullet', and 'EnemyBullet'

            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDir.normalized, rayDistance, layerMask); //Shoot ray at player
            if (hit.collider != null)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Player")) //if enemy can see player set aggressive state
                {
                    currentState = States.Aggressive;
                }
                else if (!hit.collider.gameObject.CompareTag("Player") && currentState == States.Aggressive) //if player goes out of sight
                {
                    currentState = States.Idle;
                }
                else
                {
                    currentState = States.Idle; //otherwise set idle state
                }
            }
            else
            {
                currentState = States.Idle; //if player is too far away, set to idle
            }

            switch (currentState)
            {
                case States.Idle:
                    Idle();
                    break;

                case States.Aggressive:
                    Aggressive();
                    break;
            }
        }
    }

    public void Idle()
    {
        if(colliding)
        {
            direction *= -1;
            colliding = false;
        }

        moveTimer += Time.deltaTime;
        if (!move && moveTimer >= idleMoveRate)
        {
            move = true;
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * idleMoveSpeed; //randomize, then normalize a vector (magnitude of one), and multiply by speed

            moveTimer = 0;
            idleMoveRate = Random.Range(0f, 7f); //randomize time IN BETWEEN moving
        }
        else if (move && moveTimer >= idleMoveLength)
        {
            move = false;
            idleMoveLength = Random.Range(0.3f, 2f); //randomize HOW LONG the enemy is moving

            moveTimer = Random.Range(1, 7) == 1 ? idleMoveRate : 0; //chance to immediately move again after 
        }

        if (move)
        {
            transform.Translate(direction);
        }
    }
    public void Aggressive()
    {
        var step = aggMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }

    public void TakeDamage(int amount = 1)
    {
        health -= amount;
        StartCoroutine(DamageFlash());

        if (health <= 0)
        {
            Die(true);
        }
    }

    public void Die(bool spawnPickup = true)
    {
        var endPlaceholder = GameObject.FindGameObjectWithTag("EndPlaceholder");

        endPlaceholder.transform.position = transform.position;

        if (spawnPickup)
            SpawnPickup();
        var playerScript = player.GetComponent<PlayerController>();
        playerScript.KilledEnemy();
        var particle = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(particle, 3f);
        particle.Play();
        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        gameObject.GetComponent<SpriteRenderer>().color = hitColor;
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<SpriteRenderer>().color = enemyColor;
    }

    public void SpawnPickup()
    {
        var playerScript = player.GetComponent<PlayerController>();
        var minPickupChance = Mathf.Pow(pickupChanceFactor, playerScript.killCombo);

        var pickupChance = Random.Range(minPickupChance, maxPickupChance);

        if(pickupChance >= maxPickupChance - 1f)
        {
            if(playerScript.health != playerScript.maxHealth)
            {
                var minHealthChance = Mathf.Pow(healthChanceFactor, playerScript.killCombo);
                var healthPickupChance = Random.Range(minHealthChance, maxHealthChance);
                if(healthPickupChance >= maxHealthChance - 1f)
                {
                    Instantiate(healthPickup, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(ammoPickup, transform.position, Quaternion.identity);
                }
            } 
            else
            {
                Instantiate(ammoPickup, transform.position, Quaternion.identity);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Walls"))
            colliding = true;

        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            var playerScript = player.GetComponent<PlayerController>();
            playerScript.TakeDamage();
            Die(false);
        }
            
    }
    

    /*public void Move(float speed)
    {
        var ranDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;

        transform.position += ranDirection;
    }*/
}
