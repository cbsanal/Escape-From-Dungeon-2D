using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] PlayerActionController player;
    [SerializeField] Animator anim;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (anim.GetBool("IsFalling"))
            {
                anim.SetBool("IsFalling", false);
            }
            player.isPlayerOnTheGround = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            player.isPlayerOnTheGround = false;
        }
    }

}
