using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CompanionBehaviour : MonoBehaviour
{
    private enum WalkingBehaviour { Idling, Roaming, Walking }
    private WalkingBehaviour walking;
    public enum SwitchDirections { Positive, Negative }
    public SwitchDirections direction;

    [Header("Companion")]
    public Transform[] Dots;
    public GameObject DotsHolder;
    private int dotAmount;
    private int placeInArray;
    private Transform currentDot;

    [Header("CheckPoints")]
    public Transform[] Checkpoint;
    public GameObject CheckpointHolder;
    private int checkPointAmount;
    public int pointInArray;

    [Header("Variables")]
    public bool isPanicked;
    public float walkingSpeed;
    public float panickedSpeed;
    public float SearchingRange;

    private bool isMoving;
    public bool isLatched;
    private bool isTrackingPlayer;
    private bool changeDirections;
    private bool WalkingDelay;
    [HideInInspector] public bool cutscene;

    [HideInInspector] public bool isWalking;

    [Header("Trackables")]
    PlayerBehaviour _Player;
    NavMeshAgent _Companion;
    Animator _Animator;

    private void Awake()
    {
        _Player = FindObjectOfType<PlayerBehaviour>();
        _Companion = GetComponent<NavMeshAgent>();
        _Animator = GetComponentInChildren<Animator>();

        _Companion.speed = walkingSpeed;
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
        _Companion.enabled = false;
    }

    void Update()
    {
        switch (walking)
        {
            case WalkingBehaviour.Idling: Idling(); break;
            case WalkingBehaviour.Roaming: Roaming(); break;
            case WalkingBehaviour.Walking: Walking(); break;
        }
    }

    void Idling()
    {
        isWalking = true;
        _Animator.Play("Idle");

        if (Vector3.Distance(_Player.transform.position, transform.position) < SearchingRange)
        {
            walking = WalkingBehaviour.Walking;
            _Companion.enabled = true;
        }
    }

    void Roaming()
    {
        MoveToClosestDot();
        MoveToNextDot();
        SwitchDirection();

        isWalking = false;

        _Animator.Play("Walk");

        if (!WalkingDelay)
        {
            if (Vector3.Distance(_Player.transform.position, transform.position) < 5)
            {
                walking = WalkingBehaviour.Walking;
                MoveToPlayer();
            }
        }
    }

    void Walking()
    {
        MoveToNextCheckPoint();

        isWalking = true;

        _Animator.Play("Walk");

        if (Vector3.Distance(_Player.transform.position, transform.position) > SearchingRange && !cutscene)
        {
            walking = WalkingBehaviour.Roaming;
            _Companion.ResetPath();
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
            WalkingDelay = true;

            _Companion.speed = panickedSpeed;

            if (isPanicked)
                pointInArray -= 1;

            Vector3 newDestination = GetClosestDot(Dots).transform.position;
            _Companion.SetDestination(newDestination);

            if (_Companion.pathPending)
                StartCoroutine(CheckPathEnd());
        }
    }

    void MoveToNextDot()
    {
        if (!isMoving && isLatched && !isPanicked)
        {
            isMoving = true;

            _Companion.ResetPath();

            RunThroughArrayDots();
            Vector3 newDestination = Dots[placeInArray].transform.position;
            _Companion.SetDestination(newDestination);

            StopAllCoroutines();
            StartCoroutine(CheckPathEnd());
        }
    }

    IEnumerator CheckPathEnd()
    {
        Debug.Log("checkfinish");
        yield return new WaitForSeconds(0.2f);

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
                Debug.Log("Finsihed");
                if (isTrackingPlayer)
                {
                    isTrackingPlayer = false;
                    _Companion.ResetPath();
                    _Companion.stoppingDistance = 0.2f;
                }

                isLatched = true;
                WalkingDelay = false;
                yield return new WaitForSeconds(0.2f);
                isMoving = false;
            }
            yield return null;
        }
    }

    void RunThroughArrayDots()
    {
        switch (direction)
        {
            case SwitchDirections.Positive: Positive(); break;
            case SwitchDirections.Negative: Negative(); break;
        }
    }

    void MoveToPlayer()
    {
        isTrackingPlayer = true;
        _Companion.stoppingDistance = SearchingRange / 2;

        _Companion.ResetPath();

        Vector3 newDestination = _Player.transform.position;
        _Companion.SetDestination(newDestination);

        if (_Companion.pathPending)
        {
            StopAllCoroutines();
            StartCoroutine(CheckPathEnd());
        }
    }

    void MoveToNextCheckPoint()
    {
        if (!isMoving)
        {
            isMoving = true;

            _Companion.speed = walkingSpeed;
            _Companion.ResetPath();

            Vector3 newDestination = Checkpoint[pointInArray].transform.position;
            MoveThroughArrayCheckPoints();
            _Companion.SetDestination(newDestination);

            if (_Companion.pathPending)
            {
                StopAllCoroutines();
                StartCoroutine(CheckPathEnd());
            }
        }
    }

    void MoveThroughArrayCheckPoints()
    {
        Debug.Log("AddOne");
        pointInArray += 1;
    }

    void SwitchDirection()
    {
        if (isPanicked && isLatched)
        {
            isMoving = true;
            isPanicked = false;
            changeDirections = true;

            _Companion.ResetPath();

            switch (direction)
            {
                case SwitchDirections.Positive: Positive(); break;
                case SwitchDirections.Negative: Negative(); break;
            }

            Vector3 newDestination = Dots[placeInArray].transform.position;
            _Companion.SetDestination(newDestination);

            if (_Companion.pathPending)
            {
                StopAllCoroutines();
                StartCoroutine(CheckPathEnd());
            }
        }
    }

    void Positive()
    {
        if (changeDirections)
        {
            if (placeInArray == dotAmount - 1)
                placeInArray = 0;
            else
                placeInArray += 1;

            changeDirections = false;
            direction = SwitchDirections.Negative;
        }
        else
        {
            if (placeInArray == 0)
                placeInArray = dotAmount - 1;
            else
                placeInArray -= 1;
        }
    }

    void Negative()
    {
        if (changeDirections)
        {
            if (placeInArray == 0)
                placeInArray = dotAmount - 1;
            else
                placeInArray -= 1;

            changeDirections = false;
            direction = SwitchDirections.Positive;
        }
        else
        {
            if (placeInArray == dotAmount - 1)
                placeInArray = 0;
            else
                placeInArray += 1;
        }
    }

    public void WhenSeeingEnemy()
    {
        isPanicked = true;
    }
}
