using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingManager : MonoBehaviour
{
    private Enemy enemyScript;

    private GameObject gunObj;

    private float shootTimer;

    private float shootRate;

    private Enemy.States changeStateCheck;

    [SerializeField] private float fleeMin;
    [SerializeField] private float fleeMax;

    [SerializeField] private float aggMin;
    [SerializeField] private float aggMax;

    // Start is called before the first frame update
    void Start()
    {
        gunObj = transform.GetChild(0)?.GetChild(0)?.gameObject; // first child is origin empty, child of origin is gun obj
        enemyScript = GetComponent<Enemy>();
        shootRate = Random.Range(aggMin, aggMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(changeStateCheck != enemyScript.currentState) //checks if the state was changed last frame
        {
            shootTimer = 0;
            changeStateCheck = enemyScript.currentState;
        }

        if (enemyScript.currentState == Enemy.States.Aggressive || enemyScript.currentState == Enemy.States.Flee)
        {
            shootTimer += Time.deltaTime;
        }

        if(shootTimer >= shootRate)
        {
            var gunScript = gunObj?.GetComponent<EnemyShooting>();
            if(gunScript != null)
            {
                gunScript.Shoot();

                switch (enemyScript.currentState)
                {
                    case Enemy.States.Aggressive:
                        shootRate = Random.Range(aggMin, aggMax);
                        break;

                    case Enemy.States.Flee:
                        shootRate = Random.Range(fleeMin, fleeMax);
                        break;
                }
            }

            shootTimer = 0;   
        }
    }
}
