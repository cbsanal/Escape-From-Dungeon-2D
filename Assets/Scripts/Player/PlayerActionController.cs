using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    float direction;
    bool facingRight = true, isRunning = false, isRolling = false;
    int latestAttackType = 3; // So that attack1 will be triggered first
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] float speed;
    [SerializeField] Collider2D enemyHitBox;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyHitBox);
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

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        if (!isRunning && (direction > 0 || direction < 0))
        {
            isRunning = true;
            anim.SetBool("IsRunning", isRunning);
        }
        else if (isRunning && direction == 0)
        {
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
    void Block() { }

}
