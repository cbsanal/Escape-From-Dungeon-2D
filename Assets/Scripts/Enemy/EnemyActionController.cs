using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyActionController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    [SerializeField] int currentHealth, maxHealth = 100, damage, knockBackX, knockBackY;
    [SerializeField] float movementSpeed;
    bool facingRight = false;
    int direction = -1;
    [SerializeField] bool isWalking = false, isWalkingAnimationPlaying = false;
    Transform player;
    Image healthBar;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        healthBar = GameObject.FindWithTag("EnemyHealthBar").GetComponent<Image>();
        currentHealth = maxHealth;
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
        rb.AddForce(new Vector2(0, knockBackY), ForceMode2D.Force);
        currentHealth -= damageValue;
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        anim.SetTrigger("GetHit");
        if (currentHealth <= 0)
        {
            anim.SetTrigger("Dead");
            GetComponent<EnemyActionController>().enabled = false;
        }
    }
    void Flip()
    {
        if ((!facingRight && player.transform.position.x > transform.position.x) || (facingRight && player.transform.position.x < transform.position.x))
        {
            facingRight = !facingRight;
            direction = -direction;
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
            rb.AddForce((new Vector2(facingRight ? movementSpeed : -movementSpeed, 0) - rb.velocity), ForceMode2D.Impulse);
            // rb.velocity = new Vector2(facingRight ? movementSpeed : -movementSpeed, transform.position.y);
        }
        if (!isWalking && isWalkingAnimationPlaying)
        {
            isWalkingAnimationPlaying = false;
        }
    }
    void DetectPlayer()
    {
        if (isWalking && Mathf.Abs(player.transform.position.x - transform.position.x) < 2)
        {
            StopWalking();
            Attack();
        }
        else if (!isWalking && Mathf.Abs(player.transform.position.x - transform.position.x) > 2)
        {
            isWalking = true;
        }
    }
    void StopWalking()
    {
        isWalking = false;
        isWalkingAnimationPlaying = false;
        anim.SetBool("IsWalking", false);
        rb.velocity = Vector2.zero;
    }
    void Attack()
    {
        anim.SetTrigger("Attack1");
    }
}
