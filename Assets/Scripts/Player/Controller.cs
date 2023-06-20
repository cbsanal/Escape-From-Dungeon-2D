using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float speed, attackDistance;
    float direction;
    bool facingRight = true, isRunning = false;
    int latestAttackType = 0;
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    void FixedUpdate()
    {
        Flip();
        Run();
    }
    void Run()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        if (!isRunning && (direction > 0 || direction < 0))
        {
            isRunning = true;
            anim.SetBool("isRunning", isRunning);
        }
        else if (isRunning && direction == 0)
        {
            isRunning = false;
            anim.SetBool("isRunning", isRunning);
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().GetHit();
        }
        if ((latestAttackType == 0 || latestAttackType == 3) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            anim.SetTrigger("attack1");
            latestAttackType = 1;
        }
        else if (latestAttackType == 1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            anim.SetTrigger("attack2");
            latestAttackType = 2;
        }
        else if (latestAttackType == 2 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            anim.SetTrigger("attack3");
            latestAttackType = 3;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
