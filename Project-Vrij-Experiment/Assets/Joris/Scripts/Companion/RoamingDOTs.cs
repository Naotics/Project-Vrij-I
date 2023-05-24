using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingDOTs : MonoBehaviour
{
    PlayerMovement _Player;
    private void Awake()
    {
        _Player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        transform.position = _Player.transform.position;
    }
}
