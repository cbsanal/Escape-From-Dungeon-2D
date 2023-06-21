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

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() { }
    public void GetHit(int damageValue)
    {
        if (player.position.x < transform.position.x)
        {
            rb.AddForce(new Vector2(knockBackX, knockBackY), ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(new Vector2(-knockBackX, knockBackY), ForceMode2D.Force);
        }
        health -= damageValue;
        anim.SetTrigger("GetHit");
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
