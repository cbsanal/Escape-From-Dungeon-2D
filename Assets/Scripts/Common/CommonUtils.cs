using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CommonUtils
{
    public static string GetCurrentAnimationName(Animator anim)
    {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
    public static bool CheckIfAnimationPlaying(Animator anim, string animationName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
    public static void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(attackArea.position, attackAreaRadius);
    }
}

