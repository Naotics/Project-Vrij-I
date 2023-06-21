using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    float rotation = 180;
    float duration = 1f;
    public void DecreaseSanity()
    {
        StartCoroutine(RotatePlayer());
    }

    IEnumerator RotatePlayer()
    {
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(rotation))
        {
            int diffRotate = (int)(Time.deltaTime * (rotation / duration));
            transform.Rotate(0, diffRotate, 0, Space.World);
            amountRotated = amountRotated + diffRotate;
            yield return null;
        }
        transform.rotation = Quaternion.identity;
    }
}
