using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject placeholder;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] [Range(0f, 100f)] private float chestPercentage;
    [SerializeField] private float maxDistanceFromPlayer;

    private Vector3 direction;
    private float speed = 40f; //speed of entire generation process 
    private float bounds = 40.5f; //bounds in all directions (square)

    //timer vars in seconds
    private float repeatRate = 0.5f;
    private float startDelay = 0.5f; 
    private float timer = 0.0f;

    private float globalTimer; //time since generator spawned in seconds

    private float randScaleFactor;

    private List<GameObject> placeholders = new List<GameObject>();

    //enemy spawning vars
    private float enemyTimer;
    [SerializeField] [Min(0.02f)] private float enemySpawnRate;

    [SerializeField] private float difficulty;

    // Start is called before the first frame update
    void Start()
    {
        repeatRate /= speed * Time.deltaTime; //scaling repeatRate with the speed
        startDelay /= speed * Time.deltaTime;

        enemySpawnRate -= 0.02f * difficulty;
    }

    // Update is called once per frame
    void Update()
    {
        globalTimer += Time.deltaTime;

        if ((globalTimer >= 6.0f / (speed * Time.deltaTime)) || Input.GetKeyDown(KeyCode.DownArrow)) //destroy after a set amount of seconds determined by the generation speed
        {
            SpawnPlaceholder();
            ActivatePlaceholders();            
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

        enemyTimer += Time.deltaTime;

        if(enemyTimer > enemySpawnRate)
        {
            SpawnPlaceholder();
            enemyTimer = 0;
        }

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
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }

    public void ChangeSize()
    {
        Vector3 scale = transform.localScale;

        randScaleFactor = scale.x >= 5.0f ? Random.Range(2.5f, 4.0f) : Random.Range(6.0f, 10.0f);

        transform.localScale = new Vector3(randScaleFactor, randScaleFactor, 1);
    }

    public void SpawnPlaceholder()
    {
        var placeholderGameObject = Instantiate(placeholder, transform.position, Quaternion.identity);
        placeholders.Add(placeholderGameObject);
    }

    public void ActivatePlaceholders()
    {
        List<Tuple<float, GameObject, GameObject>> distances = new List<Tuple<float, GameObject, GameObject>>();
        GameObject playerPlaceholder = null;
        GameObject chestPlaceholder = null;

        foreach (var currPlaceholder in placeholders)
        {
            foreach(var loopPlaceholder in placeholders)
            {
                var difference = loopPlaceholder.transform.position - currPlaceholder.transform.position;
                var distance = difference.magnitude;

                distances.Add(new Tuple<float, GameObject, GameObject>(distance, currPlaceholder, loopPlaceholder));
            }
        }

        var farthestDistance = 0.0f;

        foreach(var distance in distances)
        {
            if (distance.Item1 > farthestDistance)
            {
                farthestDistance = distance.Item1;
                playerPlaceholder = distance.Item2;
                chestPlaceholder = distance.Item3;
            }
        }
        List<GameObject> tooClosePlaceholders = new List<GameObject>();
        foreach(var placeholder in placeholders)
        {
            var distance = (placeholder.transform.position - playerPlaceholder.transform.position).magnitude;

            if(distance <= maxDistanceFromPlayer)
            {
                Destroy(placeholder);
                tooClosePlaceholders.Add(placeholder);
            }
        }

        foreach(var placeholder in tooClosePlaceholders)
            placeholders.Remove(placeholder);

        Instantiate(playerPrefab, playerPlaceholder.transform.position, playerPrefab.transform.rotation);
        placeholders.Remove(playerPlaceholder);
        Destroy(playerPlaceholder);
        

        var chestChance = Random.Range(0f, 100f);
        if(chestChance <= chestPercentage)
        {
            Instantiate(chestPrefab, chestPlaceholder.transform.position, Quaternion.identity);
            placeholders.Remove(chestPlaceholder);
            Destroy(chestPlaceholder);
        }        

        foreach(var placeholder in placeholders) 
        {
            Instantiate(enemyPrefab, placeholder.transform.position, Quaternion.identity);
            Destroy(placeholder);
        }
    }
}
