using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Dots")]
    public Transform[] Dots;
    public GameObject DotsHolder;
    private int dotAmount;
    private int placeInArray;
    private Transform currentDot;

    [Header("Variables")]
    private bool isMoving;
    private bool isLatched;
    private bool isAttacking;
    private float direction;
    public int amountWalked;

    public float walkingSpeed;
    public float attackingSpeed;

    [Header("Trackables")]
    NavMeshAgent _Enemy;
    PlayerMovement _Player;

    private void Awake()
    {
        _Enemy = GetComponent<NavMeshAgent>();
        _Player = FindObjectOfType<PlayerMovement>();

        direction = Random.Range(0, 2);
    }

    private void Start()
    {
        dotAmount = DotsHolder.transform.childCount;

        Dots = new Transform[dotAmount];

        for (int i = 0; i < dotAmount; i++)
        {
            Dots[i] = DotsHolder.transform.GetChild(i).transform;
            placeInArray = i;
        }
    }

    Transform GetClosestDot(Transform[] Dots)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in Dots)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        currentDot = bestTarget;
        return bestTarget;
    }

    void Update()
    {
        MoveToClosestDot();
        MoveToNextDot();
    }

    void MoveToClosestDot()
    {
        if (!isMoving && !isLatched)
        {
            isMoving = true;

            Vector3 newDestination = GetClosestDot(Dots).transform.position;
            _Enemy.SetDestination(newDestination);

            StartCoroutine(CheckPathEnd());
        }
    }

    void MoveToNextDot()
    {
        if (!isMoving && isLatched)
        {
            isMoving = true;

            _Enemy.ResetPath();

            RunThroughArrayDots();
            Vector3 newDestination = Dots[placeInArray].transform.position;
            _Enemy.SetDestination(newDestination);

            if (_Enemy.pathPending)
                StopAllCoroutines();
                StartCoroutine(CheckPathEnd());
        }
    }

    IEnumerator CheckPathEnd()
    {
        yield return new WaitForSeconds(1);

        if (!isLatched)
        {
            for (int i = 0; i < dotAmount; i++)
            {
                if (Dots[i] == currentDot)
                {
                    placeInArray = i;
                }
            }
        }

        bool flag = false;
        while (isMoving && !flag)
        {
            if (_Enemy.remainingDistance <= _Enemy.stoppingDistance)
            {
                flag = true;

                isLatched = true;
                yield return new WaitForSeconds(0.2f);
                amountWalked += 1;
            }
            yield return null;
        }

        while (isAttacking)
        {
            isAttacking = false;

            if (_Enemy.remainingDistance <= _Enemy.stoppingDistance)
            {
                Destroy(gameObject);
            }

            yield return null;
        }

        isMoving = false;
    }


    void RunThroughArrayDots()
    {
        if (amountWalked == 3)
        {
            isAttacking = true;
            _Enemy.speed = attackingSpeed;

            bool flag = true;

            if (placeInArray == 0 && flag)
            {
                flag = false;
                placeInArray = 2;
            }

            if (placeInArray == 2 && flag)
            {
                flag = false;
                placeInArray = 0;
            }

            if (placeInArray == 1 && flag)
            {
                flag = false;
                placeInArray = 3;
            }

            if (placeInArray == 3 && flag)
            {
                flag = false;
                placeInArray = 1;
            }
        }
        else
        {
            _Enemy.speed = walkingSpeed;

            if (direction == 0)
            {
                if (placeInArray == 0)
                    placeInArray = 3;
                else
                    placeInArray -= 1;
            }

            if (direction == 1)
            {
                if (placeInArray == 3)
                    placeInArray = 0;
                else
                    placeInArray += 1;
            }
        }

    }
}
