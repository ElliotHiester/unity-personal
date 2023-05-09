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

    private bool colliding;

    public enum States
    {
        Idle,
        Aggressive
    }

    // Start is called before the first frame update
    void Start()
    {
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
        idleMoveTimer += Time.deltaTime;
        if(!idleMove && idleMoveTimer >= idleMoveRate)
        {
            idleMove = true;
            idleDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * idleMoveSpeed;
            idleMoveTimer = 0;
        }
        else if(idleMove && idleMoveTimer >= idleMoveLength)
        {
            idleMove = false;
            idleMoveTimer = 0;
        }

        if(idleMove)
        {
            transform.position += idleDirection;
        }
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
