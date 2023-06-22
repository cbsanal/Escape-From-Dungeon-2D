using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    public int health, damage, speed;
    Transform player;
    [SerializeField] int knockBackX, knockBackY;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    void Start()
    {
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update() { }
    void FixedUpdate()
    {
        Attack();
    }
    public void GetHit(int damageValue, bool isEnemyFacingRight)
    {
        rb.AddForce(new Vector2(isEnemyFacingRight ? knockBackX : -knockBackX, knockBackY), ForceMode2D.Force);
        health -= damageValue;
        anim.SetTrigger("GetHit");
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
    void Attack()
    {
        anim.SetTrigger("Attack1");
    }
}
