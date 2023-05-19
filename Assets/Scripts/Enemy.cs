using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;

    [System.NonSerialized] public States currentState;

    //idle vars
    private float idleMoveRate; //time in between moving
    private float idleMoveLength; //how long the enemy moves for in idle
    [SerializeField] private float idleMoveSpeed;

    //aggressive vars
    private float aggMoveRate;
    private float aggMoveLength;
    [SerializeField] private float aggMoveSpeed;

    //flee vars
    [SerializeField] private float fleeDistance;
    [SerializeField] private float fleeSpeed;
    private Vector3 fleeDirection;
    private float fleeMoveRate;

    //wary vars
    private bool isWary = false;
    private float waryTimer;
    [SerializeField] private float waryLength;

    [SerializeField] private float rayDistance;
    private bool move;
    private Vector3 direction;
    private float moveTimer = 0;

    private GameObject player;

    private Vector2 playerDir;

    private bool colliding;

    public enum States
    {
        Idle,
        Aggressive,
        Flee
    }

    // Start is called before the first frame update
    void Start()
    {
        //player = player != null ? player : GameObject.FindWithTag("Player");

        currentState = States.Idle;

        idleMoveRate = Random.Range(0f, 8f);
        idleMoveLength = Random.Range(0.3f, 2f);
        aggMoveLength = Random.Range(0.2f, 2f);
        aggMoveRate = Random.Range(0f, 3f);
        fleeMoveRate = Random.Range(0.5f, 1.5f);
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
                    isWary = true;
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



            if (playerDir.magnitude <= fleeDistance) //if player is close enough, set state to flee
            {
                currentState = States.Flee;
                direction = -(player.transform.position - transform.position).normalized * fleeSpeed;
            }

            switch (currentState)
            {
                case States.Idle:
                    Idle();
                    break;

                case States.Aggressive:
                    Aggressive();
                    break;

                case States.Flee:
                    Flee();
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
        if(!move && moveTimer >= idleMoveRate)
        {
            move = true;
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * idleMoveSpeed; //randomize, then normalize a vector (magnitude of one), and multiply by speed

            if(!isWary)
            {
                var gunScript = transform.GetChild(0).GetComponent<RotateEnemyGun>();
                gunScript.Rotate(direction);
            }

            moveTimer = 0;
            idleMoveRate = Random.Range(0f, 7f); //randomize time IN BETWEEN moving
        }
        else if(move && moveTimer >= idleMoveLength)
        {
            move = false;
            idleMoveLength = Random.Range(0.3f, 2f); //randomize HOW LONG the enemy is moving

            moveTimer = Random.Range(1, 7) == 1 ? idleMoveRate : 0; //chance to immediately move again after 
        }

        if(move)
        {
            transform.Translate(direction);
        }

        if(isWary)
            waryTimer += Time.deltaTime;

        if (waryTimer >= waryLength)
        {
            currentState = States.Idle;
            isWary = false;
            waryTimer = 0;
        }
    }
    public void Aggressive()
    {
        if (colliding)
        {
            direction *= -1;
            colliding = false;
        }

        moveTimer += Time.deltaTime;
        if (!move && moveTimer >= aggMoveRate)
        {
            move = true;
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * aggMoveSpeed; //randomize, then normalize a vector (magnitude of one), and multiply by speed
            moveTimer = 0;
            aggMoveRate = Random.Range(0f, 4f); //randomize time IN BETWEEN moving
        }
        else if (move && moveTimer >= aggMoveLength)
        {
            move = false;
            aggMoveLength = Random.Range(0.2f, 1.8f); //randomize HOW LONG the enemy is moving

            moveTimer = Random.Range(1, 6) == 1 ? aggMoveRate : 0; //chance to immediately move again after 
        }

        if (move)
        {
            transform.Translate(direction);
        }

        var gunScript = transform.GetChild(0).GetComponent<RotateEnemyGun>();
        gunScript.Rotate();
    }

    public void Flee()
    {
        if (colliding)
        {
            fleeDirection *= -1;
            colliding = false;
        }

        moveTimer += Time.deltaTime;
        if(moveTimer >= fleeMoveRate)
        {
            fleeDirection = -(player.transform.position - transform.position).normalized * fleeSpeed; // direction away from player
            fleeMoveRate = Random.Range(0.5f, 1.5f);
            moveTimer = 0;
        }

        transform.Translate(fleeDirection);

        var gunScript = transform.GetChild(0).GetComponent<RotateEnemyGun>();
        gunScript.Rotate();
    }

    public void TakeDamage(int amount = 1)
    {
        health -= amount;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Walls"))
            colliding = true;

        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
            if(health <= 0) 
            {
                Destroy(gameObject);
            }
        }
            
    }
    

    /*public void Move(float speed)
    {
        var ranDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;

        transform.position += ranDirection;
    }*/
}
