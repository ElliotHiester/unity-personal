using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;

    private Rigidbody2D rb;

    private States currentState;

    private float idleMoveTimer = 0;
    [SerializeField] private float idleMoveRate;
    [SerializeField] private float idleMoveLength;
    [SerializeField] private float idleMoveSpeed;
    private bool idleMove;
    private Vector3 idleDirection;

    private GameObject player;

    private bool colliding;

    public enum States
    {
        Idle,
        Aggressive
    }

    // Start is called before the first frame update
    void Start()
    {
        player ??= GameObject.FindWithTag("Player");

        currentState = States.Idle;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch(currentState)
        {
            case States.Idle:
                Idle();
                break;

            case States.Aggressive:
                Aggressive();
                break;
        }
    }

    public void Idle()
    {
        if(colliding)
        {
            idleDirection *= -1;
            colliding = false;
        }

        idleMoveTimer += Time.deltaTime;
        if(!idleMove && idleMoveTimer >= idleMoveRate)
        {
            idleMove = true;
            idleDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized * idleMoveSpeed;
            idleMoveTimer = 0;
        }
        else if(idleMove && idleMoveTimer >= idleMoveLength)
        {
            idleMove = false;
            idleMoveTimer = 0;
        }

        if(idleMove)
        {
            transform.Translate(idleDirection);
        }

        Vector2 playerDir = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDir, Mathf.Infinity);

        if(hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Walls"))
            colliding = true;
    }
    public void Aggressive()
    {

    }

    /*public void Move(float speed)
    {
        var ranDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;

        transform.position += ranDirection;
    }*/
}
