using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    PlayerSanityManager _SanityManager;

    private void Awake()
    {
        _SanityManager = FindObjectOfType<PlayerSanityManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _SanityManager.DecreaseSanity();
        }
    }
}
