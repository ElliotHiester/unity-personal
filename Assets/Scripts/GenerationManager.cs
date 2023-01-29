using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    private Vector3 direction;
    private float speed = 40f; //speed of entire generation process 
    private float bounds = 40.5f; //bounds in all directions (square)

    //timer vars in seconds
    private float repeatRate = 0.5f;
    private float startDelay = 0.5f; 
    private float timer = 0.0f;

    private float globalTimer; //time since generator spawned in seconds

    private float randScaleFactor;



    // Start is called before the first frame update
    void Start()
    {
        repeatRate /= speed * Time.deltaTime; //scaling repeatRate with the speed
        startDelay /= speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;

        if (globalTimer >= 6.0f / (speed * Time.deltaTime)) //destroy after a set amount of seconds determined by the generation speed
        {
            Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation); //spawn enemy on destroy **TEMPORARY**
            Instantiate(playerPrefab, new Vector3(0, 0, 0), playerPrefab.transform.rotation); //spawn player on destroy
            Destroy(gameObject);
        }

        //timer start
        timer += Time.deltaTime; //increment timer in seconds

        if (timer > startDelay && startDelay != -1) //if timer is greater than startDelay and if startDelay is not deactivated
        {
            ChangeDirection();
            timer = 0;
            startDelay = -1; //deactivate startDelay
        }
        if (timer > repeatRate && startDelay == -1) //if timer is greater than repeatRate and if startDelay is deactivated
        {
            ChangeDirection();
            ChangeSize();
            timer = 0;

        } //timer end

        //reverse direction if out of bounds
        if (transform.position.x > bounds || transform.position.x < -bounds || transform.position.y > bounds || transform.position.y < -bounds) 
        {
            direction *= -1;
        }

        transform.Translate(direction * speed * Time.deltaTime * 1.7f); //move generator in direction based on generation speed
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Walls")) //destroy walls
        {
            Destroy(collision.gameObject);
        }
    }

    public void ChangeDirection()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    }

    public void ChangeSize()
    {
        Vector3 scale = transform.localScale;

        randScaleFactor = scale.x >= 5.0f ? Random.Range(2.5f, 4.0f) : Random.Range(6.0f, 10.0f);

        transform.localScale = new Vector3(randScaleFactor, randScaleFactor, 1);
    }

    /*
     * Chests
     * Shops
     * Enemies
     * 
     * 
     * 
     * */
}
