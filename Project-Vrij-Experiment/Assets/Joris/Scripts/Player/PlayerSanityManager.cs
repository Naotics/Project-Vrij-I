using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSanityManager : MonoBehaviour
{
    public GameObject LevelCube;

    public int Sanity;

    public int delayRotation;
    float duration = 0.4f;

    CubeManager _CubeManager;
    PlayerMovement _Player;
    private void Awake()
    {
        _Player = FindObjectOfType<PlayerMovement>();
        _CubeManager = FindObjectOfType<CubeManager>();
    }

    public void DecreaseSanity()
    {
        Sanity -= 1;
    }

    private void Update()
    {
        if (Sanity == 0)
        {
            PickRandomFunction();
            Sanity = 3;
        }
    }

    void PickRandomFunction()
    {
        int number;
        number = Random.Range(1, 4);

        if(number == 1)
            StartCoroutine(SanityRotateOne());
        if (number == 2)
            StartCoroutine(SanityRotateTwo());
        if (number == 3)
            StartCoroutine(SanityRotateThree());
        if (number == 4)
            StartCoroutine(SanityRotateFour());
    }

    IEnumerator SanityRotateOne()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(-90))
        {
            int diffRotate = (int)(Time.deltaTime * (-90 / duration));
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

    IEnumerator SanityRotateTwo()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(90))
        {
            int diffRotate = (int)(Time.deltaTime * (90 / duration));
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

    IEnumerator SanityRotateThree()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(-90))
        {
            int diffRotate = (int)(Time.deltaTime * (-90 / duration));
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

    IEnumerator SanityRotateFour()
    {
        if (_CubeManager.isRotating)
        {
            yield break;
        }
        _CubeManager.isRotating = true;
        _Player.isAbleToMove = false;
        float amountRotated = 0;
        while (Mathf.Abs(amountRotated) < Mathf.Abs(90))
        {
            int diffRotate = (int)(Time.deltaTime * (90 / duration));
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
}
