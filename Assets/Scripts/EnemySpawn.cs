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
            SpawnAsteroid();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnAsteroid()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.z = Random.Range(-spawnDistance, spawnDistance); 
        
        GameObject Enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        Enemy.transform.LookAt(target);

        Rigidbody EnemyRb = Enemy.GetComponent<Rigidbody>();
        EnemyRb.velocity = Enemy.transform.forward * EnemySpeed;

        Destroy(Enemy, destructionTime);
    }
}
