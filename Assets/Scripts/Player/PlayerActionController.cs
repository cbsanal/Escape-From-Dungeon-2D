using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    float direction;
    bool facingRight = true, isRunning = false, isRolling = false, isBlocking = false;
    int latestAttackType = 3, health = 100; // So that attack1 will be triggered first
    Rigidbody2D rb;
    Animator anim;
    GameObject[] enemies;
    [SerializeField] float speed;
    [SerializeField] int knockBackX, knockBackY;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyCollider);
            }
        }
    }
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Block();
        }
        if (Input.GetMouseButtonUp(1))
        {
            StopBlocking();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRolling = true;
        }
    }
    void FixedUpdate()
    {
        Flip();
        Run();
        Roll();
    }
    void Run()
    {
        if (isRunning)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
        if (!isRunning && (direction > 0 || direction < 0))
        {
            isRunning = true;
            anim.SetBool("IsRunning", isRunning);
        }
        else if (isRunning && direction == 0)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            isRunning = false;
            anim.SetBool("IsRunning", isRunning);
        }
    }
    void Flip()
    {
        if (facingRight && direction < 0 || !facingRight && direction > 0)
        {
            facingRight = !facingRight;
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }
    }
    void Attack()
    {
        string currentPlayingAnimation = CommonUtils.GetCurrentAnimationName(anim);
        if (latestAttackType == 3 && currentPlayingAnimation != "Attack3")
        {
            anim.SetTrigger("Attack1");
            latestAttackType = 1;
        }
        else if (latestAttackType == 1 && currentPlayingAnimation != "Attack1")
        {
            anim.SetTrigger("Attack2");
            latestAttackType = 2;
        }
        else if (latestAttackType == 2 && currentPlayingAnimation != "Attack2")
        {
            anim.SetTrigger("Attack3");
            latestAttackType = 3;
        }
    }
    void Roll()
    {
        if (isRolling)
        {
            anim.SetTrigger("Roll");
            rb.AddForce(new Vector2(5000, transform.position.y), ForceMode2D.Force);
            isRolling = false;
        }
    }
    public void GetHit(int damage, bool isEnemyFacingRight)
    {
        health -= 20;
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(GetComponent<BoxCollider2D>());
            GetComponent<PlayerActionController>().enabled = false;
        }
        if (isBlocking)
        {
            anim.SetBool("IsBlockShaking", true);
            rb.AddForce(new Vector2(isEnemyFacingRight ? knockBackX / 2 : -knockBackX / 2, knockBackY / 2), ForceMode2D.Force);
        }
        else
        {
            anim.SetTrigger("GetHit");
            rb.AddForce(new Vector2(isEnemyFacingRight ? knockBackX : -knockBackX, knockBackY), ForceMode2D.Force);
        }
    }
    void Block()
    {
        anim.SetBool("IsBlocking", true);
        isBlocking = true;
    }
    void StopBlocking()
    {
        anim.SetBool("IsBlocking", false);
        anim.SetBool("IsBlockShaking", false);
        isBlocking = false;
    }
    public void StopBlockShaking()
    {
        anim.SetBool("IsBlockShaking", false);
    }
}
