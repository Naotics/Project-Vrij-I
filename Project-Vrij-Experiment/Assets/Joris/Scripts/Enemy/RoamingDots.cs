using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingDots : MonoBehaviour
{
    [Header("Enemy")]
    public Transform[] Dots;
    public GameObject DotsHolder;
    private int dotAmount;
    private int placeInArray;

    public GameObject EnemyPrefab;
    public float timeTillNextSpawn;

    private float time;

    PlayerBehaviour _Player;

    private void Awake()
    {
        _Player = FindObjectOfType<PlayerBehaviour>();
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

    void Update()
    {
        transform.position = _Player.transform.position;

        WaveLogic();
    }

    void WaveLogic()
    {
        time += Time.deltaTime;
        bool flag = false;

        if (time >= timeTillNextSpawn)
        {
            if (!flag)
            {
                flag = true;
                SpawnEnemy();
            }

            time = 0;
        }
    }

    void SpawnEnemy()
    {
        int randomDot = Random.Range(0, dotAmount);
        placeInArray = randomDot;

        Instantiate(EnemyPrefab, Dots[placeInArray].transform.position, Quaternion.identity);
    }
}
