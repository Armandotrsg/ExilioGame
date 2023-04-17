using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Life points of the enemy
    [SerializeField]
    private float live = 10f;


    [SerializeField]
    private int damage = 1;

    void OnTriggerEnter(Collider other) {
    // If the enemy collides with the player
        if (other.CompareTag("Player")) {
        // Reduce the player's lives by the damage amount
            other.GetComponent<Player>().lives -= damage;

        // If the player has no more lives
            if (other.GetComponent<Player>().lives <= 0) {
            // Destroy the player
                Destroy(other.gameObject);
            }

        // Destroy the enemy
            Destroy(gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Damage(float damage)
    {
        live = live - damage;
        if(live <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
