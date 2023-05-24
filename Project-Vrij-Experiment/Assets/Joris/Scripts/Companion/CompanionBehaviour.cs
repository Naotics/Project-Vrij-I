using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CompanionBehaviour : MonoBehaviour
{
    private enum StateEnum { Roaming, Walking }
    private StateEnum state;

    [Header("Dots")]
    public Transform[] Dots;
    public GameObject DotsHolder;
    private int dotAmount;
    private int placeInArray;
    private Transform currentDot;

    [Header("CheckPoints")]
    public Transform[] Checkpoint;
    public GameObject CheckpointHolder;
    private int checkPointAmount;
    private int pointInArray;

    [Header("variables")]
    public float SearchingRange;
    private bool isMoving;
    private bool isLatched;
    public bool isPanicked;
    private bool isTrackingPlayer;

    [Header("Trackables")]
    PlayerMovement _Player;
    NavMeshAgent _Companion;

    private void Awake()
    {
        _Player = FindObjectOfType<PlayerMovement>();
        _Companion = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        dotAmount = DotsHolder.transform.childCount;

        Dots = new Transform[dotAmount];

        for (int i = 0; i < dotAmount; i++)
            Dots[i] = DotsHolder.transform.GetChild(i).transform;

        checkPointAmount = CheckpointHolder.transform.childCount;

        Checkpoint = new Transform[checkPointAmount];

        for (int i = 0; i < checkPointAmount; i++)
        {
            Checkpoint[i] = CheckpointHolder.transform.GetChild(i).transform;
            pointInArray = i;
        }

        pointInArray = 0;
    }

    void Update()
    {
        switch (state)
        {
            case StateEnum.Roaming: Roaming(); break;
            case StateEnum.Walking: Walking(); break;
        }
    }

    void Roaming()
    {
        MoveToClosestDot();
        MoveToNextDot();

        if (Vector3.Distance(_Player.transform.position, transform.position) < SearchingRange)
        {
            state = StateEnum.Walking;
            MoveToPlayer();
        }
    }

    void Walking()
    {
        Invoke("MoveToNextCheckPoint", 1f);

        if (Vector3.Distance(_Player.transform.position, transform.position) > SearchingRange && isPanicked)
        {
            state = StateEnum.Roaming;
            isLatched = false;
            isMoving = false;
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

    void MoveToClosestDot()
    {
        if (!isMoving && !isLatched)
        {
            isMoving = true;

            Vector3 newDestination = GetClosestDot(Dots).transform.position;
            _Companion.SetDestination(newDestination);

            StartCoroutine(CheckPathEnd());
        }
    }

    void MoveToNextDot()
    {
        if(!isMoving && isLatched)
        {
            isMoving = true;

            _Companion.ResetPath();

            RunThroughArrayDots();
            Vector3 newDestination = Dots[placeInArray].transform.position;
            _Companion.SetDestination(newDestination);

            if (_Companion.pathPending)
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
            if (_Companion.remainingDistance <= _Companion.stoppingDistance)
            {
                flag = true;

                if (isTrackingPlayer)
                {
                    isTrackingPlayer = false;
                    _Companion.ResetPath();
                    _Companion.stoppingDistance = 0.5f;
                }

                isLatched = true;
                yield return new WaitForSeconds(0.2f);
                isMoving = false;
            }
            yield return null;
        }
    }

    void RunThroughArrayDots()
    {
        int random = 1; //Random.Range(1, 3);

        if (random == 1)
            if (placeInArray == dotAmount-1)
                placeInArray = 0;
            else
                placeInArray += 1;
    }

    void MoveToPlayer()
    {
        isTrackingPlayer = true;
        _Companion.stoppingDistance = SearchingRange / 2;

        _Companion.ResetPath();

        Vector3 newDestination = _Player.transform.position;
        _Companion.SetDestination(newDestination);

        if (_Companion.pathPending)
            StopAllCoroutines();
            StartCoroutine(CheckPathEnd());
    }

    void MoveToNextCheckPoint()
    {
        if (!isMoving)
        {
            isMoving = true;

            _Companion.ResetPath();

            Vector3 newDestination = Checkpoint[pointInArray].transform.position;
            MoveThroughArrayCheckPoints();
            _Companion.SetDestination(newDestination);

            if (_Companion.pathPending)
                StopAllCoroutines();
                StartCoroutine(CheckPathEnd());
        }
    }

    void MoveThroughArrayCheckPoints()
    {
        pointInArray += 1;

        if(pointInArray == checkPointAmount)
            pointInArray = 0;
    }
}
