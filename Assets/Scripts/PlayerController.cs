using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10f;

    private Rigidbody2D rb;

    private Vector2 movement;

    private UIManager UIManager;

    private GameObject shootPos;
    [SerializeField] private GameObject startGun;

    public int maxHealth;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        health = maxHealth;

        var gun = Instantiate(startGun, transform.position, Quaternion.identity);
        gun.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        shootPos ??= GameObject.FindWithTag("PlayerShootPos");
        UIManager ??= GameObject.FindWithTag("UIManager")?.GetComponent<UIManager>();

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        if (Input.GetKeyDown(KeyCode.K))
        {
           health--;
           UIManager.UpdateHearts();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            health++;
            UIManager.UpdateHearts();
        }
    }

    private void FixedUpdate() => rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement); 

    public void KickBack(float kickBack)
    {
        rb.AddForce(shootPos.transform.right * kickBack, ForceMode2D.Force);
    }
}
