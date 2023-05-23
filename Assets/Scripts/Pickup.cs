using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 10f;
    private float destroyTimer = 0.0f;
    private bool isFlashing = false;

    protected GameObject player;

    [SerializeField] private float attractDistance;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime;

        if (!isFlashing && destroyTimer >= (timeToDestroy / 2))
        {
            StartCoroutine(DisappearFlash());
            isFlashing = true;
        }

        if(isFlashing && destroyTimer >= timeToDestroy)
        {
            Destroy(gameObject);
        }

        player = player != null ? player : GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var distance = (player.transform.position - transform.position).magnitude;

            if (distance <= attractDistance)
            {
                var step = speed * Time.deltaTime / distance;

                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
            }
        }
    }

    IEnumerator DisappearFlash()
    {
        var renderer = GetComponent<SpriteRenderer>();

        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.3f);
        yield return new WaitForSeconds(0.5f);
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DisappearFlash());
        
    }
    protected abstract void OnCollisionEnter2D(Collision2D collision);
}
