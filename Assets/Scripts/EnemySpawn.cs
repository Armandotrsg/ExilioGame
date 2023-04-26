using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float spawnRadius = 20f;
    public float spawnRate = 2f;
    public float EnemySpeed = 10f;

    [SerializeField]
    public float destructionTime;

    public float spawnDistance = 10f;
    public float spawnZRange = 2f;

    private Transform target;
    private float nextSpawnTime;

    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        float zPos = Mathf.Clamp(target.position.z + Random.Range(-spawnZRange, spawnZRange), transform.position.z + spawnDistance, transform.position.z + spawnDistance + spawnZRange * 2f);
        spawnPosition.z = zPos; 
        
        GameObject enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        enemy.transform.LookAt(target);

        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        enemyRb.velocity = enemy.transform.forward * EnemySpeed;

        Destroy(enemy, destructionTime);

    }
}