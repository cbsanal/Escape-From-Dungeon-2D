using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHit : MonoBehaviour
{
    [SerializeField] PlayerSoundEffectController soundController;
    [SerializeField] Animator anim;
    CameraActionController cam;
    void Awake()
    {
        cam = GameObject.FindWithTag("CinemachineCamera").GetComponent<CameraActionController>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            bool isAttackRotationRight = transform.parent.transform.position.x < other.transform.position.x;
            string currentPlayingAnimation = CommonUtils.GetCurrentAnimationName(anim);
            other.GetComponent<EnemyActionController>().GetHit(20, isAttackRotationRight);
            soundController.PlaySound(currentPlayingAnimation);
            cam.StartShaking();
        }
    }
}
