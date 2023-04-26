using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemyLaser : MonoBehaviour
{

    [SerializeField]private float speed = 3000f;
    [SerializeField] private int damage;
    private Player player;
    private FirebaseManager firebaseManager;
    [SerializeField]
    private Lives lives;


    // Start is called before the first frame update
    void Start()
    {
        lives = Lives.Instance;
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Check in which direction the bullet moves
        transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * speed, Space.World); // Move the bullet\
    }

    void OnTriggerEnter(Collider other)
    {
        // If the enemy collides with the player
        if (other.CompareTag("Player")) {
            print("Colision");

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

}
