using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTriggers : MonoBehaviour
{
    public GameObject LevelCube;

    [Header("RotationValues")]
    [SerializeField]
    float rotation;
    [SerializeField]
    float duration;
    float delayRotation = 10;

    [Header("Directions")]
    [SerializeField]
    bool Left;
    [SerializeField]
    bool Right;
    [SerializeField]
    bool Back;
    [SerializeField]
    bool Front;

    PlayerMovement _Player;
    CubeManager _CubeManager;
    void Awake()
    {
        _Player = FindObjectOfType<PlayerMovement>();
        _CubeManager = FindObjectOfType<CubeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_CubeManager.isRotating)
        {
            if (Left)
                StartCoroutine(RotateRoomLeft());
            if (Right)
                StartCoroutine(RotateRoomRight());
            if (Back)
                StartCoroutine(RotateRoomBack());
            if (Front)
                StartCoroutine(RotateRoomFront());
        }
    }

    void AdjustRotation(Vector3 cubeRotation)
    {
        float rotationDifference1 = cubeRotation[0] % 90;

        if (rotationDifference1 != 0)
        {
            int rotations = (int)(cubeRotation[0] - rotationDifference1) / 90;

            if (rotationDifference1 >= 10)
                rotations++;

            LevelCube.transform.eulerAngles = new Vector3(rotations * 90, LevelCube.transform.eulerAngles.y, LevelCube.transform.eulerAngles.z);
        }

        float rotationDifference2 = cubeRotation[1] % 90;

        if (rotationDifference2 != 0)
        {
            int rotations = (int)(cubeRotation[1] - rotationDifference2) / 90;

            if (rotationDifference2 >= 10)
                rotations++;

            LevelCube.transform.eulerAngles = new Vector3(LevelCube.transform.eulerAngles.x, rotations * 90, LevelCube.transform.eulerAngles.z);
        }

        float rotationDifference3 = cubeRotation[2] % 90;

        if (rotationDifference3 != 0)
        {
            int rotations = (int)(cubeRotation[2] - rotationDifference3) / 90;

            if (rotationDifference3 >= 10)
                rotations++;

            LevelCube.transform.eulerAngles = new Vector3(LevelCube.transform.eulerAngles.x, LevelCube.transform.eulerAngles.y, rotations * 90);
        }
    }

    IEnumerator RotateRoomRight()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(-rotation))
        {
            int diffRotate = (int)(Time.deltaTime * (-rotation / duration));
            LevelCube.transform.Rotate(0, 0, diffRotate, Space.World);
            amountRotated = amountRotated + diffRotate;
            yield return null;
        }
        AdjustRotation(LevelCube.transform.eulerAngles);
        _Player.transform.rotation = Quaternion.identity;
        _Player.isAbleToMove = true;
        yield return new WaitForSeconds(delayRotation);
        _CubeManager.isRotating = false;
    }

    IEnumerator RotateRoomLeft()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(rotation))
        {
            int diffRotate = (int)(Time.deltaTime * (rotation / duration));
            LevelCube.transform.Rotate(0, 0, diffRotate, Space.World);
            amountRotated = amountRotated + diffRotate;
            yield return null;
        }
        AdjustRotation(LevelCube.transform.eulerAngles);
        _Player.transform.rotation = Quaternion.identity;
        _Player.isAbleToMove = true;
        yield return new WaitForSeconds(delayRotation);
        _CubeManager.isRotating = false;
    }

    IEnumerator RotateRoomFront()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(rotation))
        {
            int diffRotate = (int)(Time.deltaTime * (rotation / duration));
            LevelCube.transform.Rotate(diffRotate, 0, 0, Space.World);
            amountRotated = amountRotated + diffRotate;
            yield return null;
        }
        AdjustRotation(LevelCube.transform.eulerAngles);
        _Player.transform.rotation = Quaternion.identity;
        _Player.isAbleToMove = true;
        yield return new WaitForSeconds(delayRotation);
        _CubeManager.isRotating = false;
    }

    IEnumerator RotateRoomBack()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(-rotation))
        {
            int diffRotate = (int)(Time.deltaTime * (-rotation / duration));
            LevelCube.transform.Rotate(diffRotate, 0, 0, Space.World);
            amountRotated = amountRotated + diffRotate;
            yield return null;
        }
        AdjustRotation(LevelCube.transform.eulerAngles);
        _Player.transform.rotation = Quaternion.identity;
        _Player.isAbleToMove = true;
        yield return new WaitForSeconds(delayRotation);
        _CubeManager.isRotating = false;
    }
}
