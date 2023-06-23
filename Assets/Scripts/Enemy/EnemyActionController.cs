using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    public int health, damage, movementSpeed;
    bool facingRight = false;
    [SerializeField] bool isWalking = false, isWalkingAnimationPlaying = false;
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
    void Update()
    {
        Flip();
    }
    void FixedUpdate()
    {
        // Attack();
        Walk();
        DetectPlayer();
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
    void Flip()
    {
        if ((!facingRight && player.transform.position.x > transform.position.x) || (facingRight && player.transform.position.x < transform.position.x))
        {
            facingRight = !facingRight;
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }
    }
    void Walk()
    {
        if (isWalking)
        {
            if (!isWalkingAnimationPlaying)
            {
                anim.SetBool("IsWalking", true);
                isWalkingAnimationPlaying = true;
            }
            rb.velocity = new Vector2(facingRight ? movementSpeed : -movementSpeed, transform.position.y);
        }
        if (!isWalking && isWalkingAnimationPlaying)
        {
            StopWalking();
        }
    }

    void DetectPlayer()
    {
        if (isWalking && Mathf.Abs(player.transform.position.x - transform.position.x) < 2)
        {
            StopWalking();
        }
        else if (!isWalking)
        {
            isWalking = true;
        }
    }
    void StopWalking()
    {
        isWalking = false;
        isWalkingAnimationPlaying = false;
        anim.SetBool("IsWalking", false);
        rb.velocity = new Vector2(0, transform.position.y);
    }


    void Attack()
    {
        // anim.SetTrigger("Attack1");
    }
}
