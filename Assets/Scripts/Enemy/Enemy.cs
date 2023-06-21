using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    int health = 100;
    Camera cam;
    [SerializeField] int knockBackX, knockBackY;
    [SerializeField] Transform player;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetHit()
    {
        if (player.position.x < transform.position.x)
        {
            rb.AddForce(new Vector2(knockBackX, knockBackY), ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(new Vector2(-knockBackX, knockBackY), ForceMode2D.Force);
        }
        health -= 20;
        anim.SetTrigger("GetHit");
        cam.StartShaking(3f, 0.75f);
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
            // GetComponent<Collider2D>().isTrigger = true;
            // Invoke(Destroy, 5);
        }
    }
}
