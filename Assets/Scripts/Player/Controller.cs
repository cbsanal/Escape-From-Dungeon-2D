using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    float direction;
    bool facingRight = true;
    bool isRunning = false;
    public float speed;
    Rigidbody2D rb;
    Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
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
}
