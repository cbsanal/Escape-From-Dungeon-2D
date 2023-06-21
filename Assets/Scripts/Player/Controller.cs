using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float speed, attackAreaRadius;
    float direction;
    bool facingRight = true, isRunning = false, isRolling = false;
    int latestAttackType = 3; // So that attack1 will be triggered first
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] Transform attackArea;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] SoundAction soundController;
    [SerializeField] Collider2D enemyCollider;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyCollider);
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
        string currentPlayingAnimation = GetCurrentAnimationName();
        if (currentPlayingAnimation != "Attack1" && currentPlayingAnimation != "Attack2" && currentPlayingAnimation != "Attack3")
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
    void PlayAttackAnimation(string animationName, bool isAttackHitEnemy, int attackNumber)
    {
        anim.SetTrigger(animationName.ToLower());
        latestAttackType = attackNumber;
        if (isAttackHitEnemy)
        {
            soundController.PlaySound(animationName);
        }
    }
    void Attack()
    {
        string currentPlayingAnimation = GetCurrentAnimationName();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackArea.position, attackAreaRadius, enemyLayers);
        bool isAttackHitEnemy = false;
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().GetHit();
            isAttackHitEnemy = true;
        }
        if (latestAttackType == 3 && currentPlayingAnimation != "Attack3")
        {
            PlayAttackAnimation("Attack1", isAttackHitEnemy, 1);
        }
        else if (latestAttackType == 1 && currentPlayingAnimation != "Attack1")
        {
            PlayAttackAnimation("Attack2", isAttackHitEnemy, 2);
        }
        else if (latestAttackType == 2 && currentPlayingAnimation != "Attack2")
        {
            PlayAttackAnimation("Attack3", isAttackHitEnemy, 3);
        }
    }
    void Roll()
    {
        if (isRolling)
        {
            anim.SetTrigger("roll");
            rb.AddForce(new Vector2(5000, transform.position.y), ForceMode2D.Force);
            isRolling = false;
        }
    }
    void Block() { }
    string GetCurrentAnimationName()
    {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
    bool CheckIfAnimationPlaying(string animationName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackArea.position, attackAreaRadius);
    }
}
