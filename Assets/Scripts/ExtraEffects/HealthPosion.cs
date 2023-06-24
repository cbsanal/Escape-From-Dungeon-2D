using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPosion : MonoBehaviour
{
    PlayerActionController player;
    [SerializeField] PlayerSoundEffectController soundController;
    [SerializeField] int healAmouth = 50;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerActionController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeHealthPosion(healAmouth);
            soundController.PlaySound("Heal");
            Destroy(gameObject);
        }
    }
}
