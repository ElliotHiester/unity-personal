using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlaceholder : MonoBehaviour
{
    [SerializeField] private GameObject endGameObject;

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            StartCoroutine(SpawnEndDelay());
        }
    }

    IEnumerator SpawnEndDelay()
    {
        yield return new WaitForSeconds(1f);
        SpawnEnd();
    }

    public void SpawnEnd()
    {
        Instantiate(endGameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
