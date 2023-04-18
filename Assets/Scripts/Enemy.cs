using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour
{
    // Life points of the enemy
    [SerializeField]
    private float live = 10f;


    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private Lives lives;

    private FirebaseManager firebaseManager;

    void Start()
    {
        lives = Lives.Instance;
        firebaseManager = FirebaseManager.Instance;
        Assert.IsNotNull(firebaseManager, "FirebaseManager is null");
    }

    void OnTriggerEnter(Collider other) {
    // If the enemy collides with the player
        if (other.CompareTag("Player")) {
        // Reduce the player's lives by the damage amount
            other.GetComponent<Player>().lives -= damage;
            lives._texto.text = "Lives: " + other.GetComponent<Player>().lives;
            

        // If the player has no more lives
            if (other.GetComponent<Player>().lives <= 0) {
                // Save the score and kills to the database
                StartCoroutine(firebaseManager.SaveScore(other.GetComponent<Player>().Score));
                StartCoroutine(firebaseManager.SaveKills(other.GetComponent<Player>().Kills));
                //Save the username of the current player which is the email without the domain
                string username = firebaseManager.auth.CurrentUser.Email.Split('@')[0];
                StartCoroutine(firebaseManager.SaveUsername(username));
            // Destroy the player
                //Destroy(other.gameObject);
            // Load the game over scene
                SceneManager.LoadScene(4);
            }
///
        // Destroy the enemy
            Destroy(gameObject);
        }
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
