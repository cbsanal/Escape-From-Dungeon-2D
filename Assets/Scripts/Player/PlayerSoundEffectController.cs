using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffectController : MonoBehaviour
{
    [SerializeField] AudioSource attack1Source, attack2Source, attack3Source, blockSource, getHit;
    public void PlaySound(string animationName)
    {
        switch (animationName)
        {
            case "Attack1":
                attack1Source.Play();
                break;
            case "Attack2":
                attack2Source.Play();
                break;
            case "Attack3":
                attack3Source.Play();
                break;
            case "Block":
                blockSource.Play();
                break;
            case "GetHit":
                getHit.Play();
                break;
            default:
                break;
        }
    }
}
