using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;

    [SerializeField]
    GameObject enemyPrefab;

    public List<EnemyAIManager> enemyAIManagers;

    [SerializeField]
    Transform[] spawnPoints;

    int numberofEnemy;

    int waveCount = 0;

    [SerializeField]
    Transform playerBase;


    private void Awake()
    {
        enemyManager = this;

    }

    private void Start()
    {
        CalculateTheWave();
    }


    void CalculateTheWave()
    {
        waveCount++;

        numberofEnemy = (int)(Mathf.Pow(waveCount, 1.5f) + 5 + Mathf.Sin(waveCount));

        int randomPoint = UnityEngine.Random.Range(0, spawnPoints.Length);

        Transform randomSpawnPoints = spawnPoints[randomPoint];


        SpawnEnemy(numberofEnemy, randomSpawnPoints);

    }



    void SpawnEnemy(int _numberofEnem1, Transform _spawnPoint)
    {
        Vector3 offset = UnityEngine.Random.insideUnitCircle * 5;

        for (int i = 0; i < _numberofEnem1; i++)
        {

            offset = UnityEngine.Random.insideUnitCircle * 5;

            GameObject newEnemy = Instantiate(enemyPrefab, _spawnPoint.position + offset, quaternion.identity);
            
            enemyAIManagers.Add(newEnemy.GetComponent<EnemyAIManager>());

            StartCoroutine(newEnemy.GetComponent<EnemyAIManager>().SetupSpawn( UnityEngine.Random.Range(10,15), playerBase));
        }


    }






}
