using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnRadius = 20f;
    public float spawnRate = 2f;
    public float asteroidSpeed = 10f;

    [SerializeField]
    public float destructionTime;

    private Transform target;
    private float nextSpawnTime;

    void Start()
    {
        // buscar la nave por etiqueta
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // spawnear un asteroide si es tiempo de hacerlo
        if (Time.time >= nextSpawnTime)
        {
            SpawnAsteroid();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnAsteroid()
    {
        // generar un punto aleatorio dentro del radio de spawn
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.z = 0f; // asegurarse de que el asteroide no se genere en el eje z

        // crear el asteroide en la posición generada y rotado hacia la nave
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.transform.LookAt(target);

        // añadirle movimiento al asteroide
        Rigidbody asteroidRb = asteroid.GetComponent<Rigidbody>();
        asteroidRb.velocity = asteroid.transform.forward * asteroidSpeed;

        Destroy(asteroid, destructionTime);
    }
}
