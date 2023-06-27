using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionController : MonoBehaviour
{
    float direction;
    bool facingRight = true, isRunning = false;
    bool isRolling = false, isRollingAnimationPlaying = false, isBlocking = false, isJumping = false;
    int latestAttackType = 3; // latestAttackType = 3 so that attack1 will be triggered first
    public bool isPlayerOnTheGround = true;
    public int currentHealth, maxHealth = 100, jumpForce;
    Rigidbody2D rb;
    Animator anim;
    GameObject[] enemies;
    [SerializeField] float movementSpeed, rollDistance;
    [SerializeField] int knockBackX, knockBackY;
    Image healthBar;
    [SerializeField] PlayerSoundEffectController soundController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        healthBar = GameObject.FindWithTag("PlayerHealthBar").GetComponent<Image>();
        currentHealth = maxHealth;
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
        Flip();
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isPlayerOnTheGround)
            {
                isJumping = true;
            }
        }
    }
    void FixedUpdate()
    {
        Run();
        Roll();
        VerticalMovement();
    }
    void Run()
    {
        if (!isBlocking)
        {
            if (isRunning)
            {
                // rb.velocity = new Vector2(direction * movementSpeed, rb.velocity.y);
                rb.AddForce((new Vector2(direction * movementSpeed, rb.velocity.y) - rb.velocity), ForceMode2D.Impulse);
            }
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

    }
    void Flip()
    {
        if (facingRight && direction < 0 || !facingRight && direction > 0)
        {
            // GetComponent<SpriteRenderer>().flipX = facingRight;
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
            // rb.velocity = new Vector2(facingRight ? movementSpeed : -movementSpeed, rb.velocity.y);
            rb.AddForce((new Vector2(facingRight ? movementSpeed : -movementSpeed, 0) - rb.velocity), ForceMode2D.Impulse);
        }
        if (isRolling && !isRollingAnimationPlaying)
        {
            isRollingAnimationPlaying = true;
            anim.SetTrigger("Roll");
        }
    }
    public void GetHit(int damage, bool isEnemyFacingRight)
    {
        if (!isRolling)
        {
            if (isBlocking)
            {
                anim.SetTrigger("BlockShaking");
                anim.SetBool("IsBlockShaking", true);
                rb.AddForce(new Vector2(isEnemyFacingRight ? knockBackX / 2 : -knockBackX / 2, knockBackY / 2), ForceMode2D.Force);
                soundController.PlaySound("Block");
            }
            if (!isBlocking || (isBlocking && facingRight == isEnemyFacingRight))
            {
                currentHealth -= 20;
                healthBar.fillAmount = (float)currentHealth / maxHealth;
                anim.SetTrigger("GetHit");
                rb.AddForce(new Vector2(isEnemyFacingRight ? knockBackX : -knockBackX, knockBackY), ForceMode2D.Force);
                soundController.PlaySound("GetHit");
            }
            if (currentHealth <= 0)
            {
                anim.SetTrigger("Dead");
                rb.bodyType = RigidbodyType2D.Static;
                Destroy(GetComponent<BoxCollider2D>());
                GetComponent<PlayerActionController>().enabled = false;
            }
        }
    }
    void Block()
    {
        FreezePlayer();
        anim.SetBool("IsBlocking", true);
        isBlocking = true;
    }
    void StopBlocking()
    {
        anim.SetBool("IsBlocking", false);
        anim.SetBool("IsBlockShaking", false);
        isBlocking = false;
    }
    public void StopRolling()
    {
        FreezePlayer();
        isRollingAnimationPlaying = false;
        isRolling = false;
    }
    public void TakeHealthPosion(int healAmouth)
    {
        if (currentHealth + healAmouth > 100)
        {
            currentHealth = 100;
        }
        else
        {
            currentHealth += healAmouth;
        }
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    void VerticalMovement()
    {
        if (isJumping && isPlayerOnTheGround)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = false;
        }
        if (!isJumping && !isPlayerOnTheGround)
        {
            anim.SetFloat("VelocityY", rb.velocity.y);
        }
    }
    void FreezePlayer()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

}
