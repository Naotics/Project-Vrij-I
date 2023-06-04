using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy")]
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
    PlayerBehaviour _Player;
    CompanionBehaviour _Companion;
    AudioSource _AudioSource;

    private void Awake()
    {
        _Enemy = GetComponent<NavMeshAgent>();
        _Player = FindObjectOfType<PlayerBehaviour>();
        _Companion = FindObjectOfType<CompanionBehaviour>();
        _AudioSource = GetComponent<AudioSource>();

        direction = Random.Range(0, 2);
        DotsHolder = FindObjectOfType<RoamingDots>().transform.GetChild(1).gameObject;
        transform.GetComponentInChildren<Renderer>().material.color = new Color (0,0,0,0);
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

        StartCoroutine(FadeTo(7.0f, 4.0f));
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

            if (_Enemy.pathPending && !isAttacking)
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

        isMoving = false;
    }

    void RunThroughArrayDots()
    {
        if (amountWalked == 3)
        {
            isAttacking = true;
            StartCoroutine(FadeOut());
            StartCoroutine(FadeTo(0.0f, 4.0f));
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _Player.DecreaseSanity();
        }

        if (other.gameObject.tag == "Companion")
        {
            _Companion.WhenSeeingEnemy();
        }
    }

    IEnumerator FadeOut()
    {
        _AudioSource.volume = 0.9f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.8f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.7f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.6f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.5f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.4f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.3f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.2f;
        yield return new WaitForSeconds(0.5f);
        _AudioSource.volume = 0.1f;
        Destroy(gameObject);
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = transform.GetComponentInChildren<Renderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponentInChildren<Renderer>().material.color = newColor;
            yield return null;
        }
    }
}
