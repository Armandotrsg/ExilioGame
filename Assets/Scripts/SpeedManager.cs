using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    private EnemyLaser enemyLaser;

    void Awake()
    {
        enemyLaser = GetComponent<EnemyLaser>();
    }

    void FixedUpdate()
    {
        enemyLaser.speed += 100f;
    }
}
