using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    int health = 100;
    Camera cam;
    void Start()
    {
        anim = GetComponent<Animator>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetHit()
    {
        health -= 20;
        anim.SetTrigger("GetHit");
        cam.StartShaking(5f, 0.5f);
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }
}
