using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public int Sanity;

    public void DecreaseSanity()
    {
        Sanity += 1;
    }
}
