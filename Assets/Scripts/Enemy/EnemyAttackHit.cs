using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHit : MonoBehaviour
{
    PlayerActionController player;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerActionController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool isAttackRotationRight = transform.parent.transform.position.x < other.transform.position.x;
            // string currentPlayingAnimation = CommonUtils.GetCurrentAnimationName(anim);
            player.GetHit(20, isAttackRotationRight);
            // soundController.PlaySound(currentPlayingAnimation);
            // cam.StartShaking();
        }
    }

}
