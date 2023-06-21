using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHit : MonoBehaviour
{
    [SerializeField] PlayerSoundEffectController soundController;
    [SerializeField] Animator anim;
    CameraActionController cam;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CameraActionController>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            string currentPlayingAnimation = CommonUtils.GetCurrentAnimationName(anim);
            other.GetComponent<EnemyActionController>().GetHit(20);
            soundController.PlaySound(currentPlayingAnimation);
            cam.StartShaking();
        }
    }
}
