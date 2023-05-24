using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.AI;
using UnityEngine;

public class CompanionMovement : MonoBehaviour
{
    public float SearchingRange;
    public bool isMoving;
    private bool islatched;
    private bool isDancing;
    private bool reachedDestination;
    private int DotAmount;

    Transform currentPotTarget;
    int currentArrayNumber = -1;

    [Header("D.O.T.S")]
    public GameObject DotsHolder;
    public Transform[] Dots;
 
    PlayerMovement _Player;
    NavMeshAgent _Companion;

    private enum StateEnum { Roam, Walking }
    private StateEnum state;

    private void Awake()
    {
        _Player = FindObjectOfType<PlayerMovement>();
        _Companion = FindObjectOfType<NavMeshAgent>();
    }

    private void Start()
    {
        DotAmount = DotsHolder.transform.childCount;

        Dots = new Transform[DotAmount];

        for (int i = 0; i < DotAmount; i++)
            Dots[i] = DotsHolder.transform.GetChild(i).transform;

        Debug.Log("Call Start");
    }

    void Update()
    {
        switch (state)
        {
            case StateEnum.Roam: Roaming(); break;
            case StateEnum.Walking: Walking(); break;
        }

        PathCompleteCheck();
    }

    void Roaming()
    {
        Debug.Log("Call Method Roaming (Update)");

        if (Vector3.Distance(_Player.transform.position, transform.position) < SearchingRange)
            StartCoroutine(PlayerDestination());
        else
            StartCoroutine(RandomDestination());

        if (isDancing)
        {
            Debug.Log("isDancing = true");
            for (int i = 0; i < DotAmount; i++)
            {
                if (Dots[i] == currentPotTarget)
                {
                    currentArrayNumber = i;
                    isDancing = false;
                    Debug.Log("isDancing = false && currenArraynumer = i");
                }
            }
        }
    }

    void RunArray()
    {
        Debug.Log("RunArray calling");
        int RNG = Random.Range(1, 3);
        if (RNG == 1)
        {
            if (currentArrayNumber == 3)
            {
                currentArrayNumber = 0;
            }
            else
                currentArrayNumber += 1;
        }
            
        if (RNG == 2)
        {
            if (currentArrayNumber == 0)
                currentArrayNumber = 3;
            else
            {
                currentArrayNumber -= 1;
            }
        }
    }

    void Walking()
    {

    }

    IEnumerator PlayerDestination()
    {
        if (isMoving)
            yield break;
        
        //isMoving = true;
        _Companion.SetDestination(_Player.transform.position);
    }

    IEnumerator RandomDestination()
    {
        Debug.Log("1");
        if (isMoving)
            yield break;

        isMoving = true;
        Debug.Log("2");

        if (!islatched)
        {
            islatched = true;
            
            Vector3 newDestination = GetClosestDot(Dots).transform.position;
            _Companion.SetDestination(newDestination);
            reachedDestination = false;
            isDancing = true;
            Debug.Log("3");
        } else
        {
            Vector3 newDestination = Dots[currentArrayNumber].transform.position;
            _Companion.SetDestination(newDestination);
            reachedDestination = false;
            isDancing = true;
            Debug.Log("4");
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

        currentPotTarget = bestTarget;
        return bestTarget;
    }

    void PathCompleteCheck()
    {
        if (_Companion.remainingDistance <= _Companion.stoppingDistance && !reachedDestination && isMoving)
        {
            reachedDestination = true;
            isMoving = false;
            RunArray();
            Debug.Log("Reached Destination, RunArray got called");
        }
    }
}
