using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {
    
    [SerializeField]
    private float minY, maxY, minX, maxX, speed;

    /* [SerializeField]
    private Proyectil proyectil; */

    private static Player _instance;

    [SerializeField]
    private Camera cam;

    public static Player Instance {
        get {
            return _instance;
        }
    }

    Coroutine currentCoroutine;

    [SerializeField]
    private int lives = 3;

/*     private LivesGUI livesGUI;
    private ScoreGUI scoreGUI; */

    private int score = 0;

    public int Score {
        get {
            return score;
        }
        set {
            score = value;
        }
    }

    void Awake() {
        
    }

    void Start() {
        /* livesGUI = LivesGUI.Instance;
        livesGUI._texto.text = "Lives: " + lives;
        scoreGUI = ScoreGUI.Instance;
        scoreGUI._texto.text = "Score: " + score; */

        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    ///     Moves the player and makes sure it doesn't go out of bounds
    /// </summary>
    void MovePlayer() {
        //Get the horizontal and vertical input of the user.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Check if the player is out of bounds
        if (transform.position.y > maxY && vertical > 0) {
            vertical = 0;
        } else if (transform.position.y < minY && vertical < 0) {
            vertical = 0;
        }

        if (transform.position.x > maxX && horizontal > 0) {
            horizontal = 0;
        }else if (transform.position.x < minX && horizontal < 0) {
            horizontal = 0;
        }

        //Move the player
        transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * speed, Space.World);
        
        //Rotate the player smoothly according to the input
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-vertical * 27, 0, -horizontal * 25), Time.deltaTime * 10);

        //Smoothly move the camera to the player's position
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(-vertical * 27 / 13, 0, -horizontal * 25 / 13), Time.deltaTime * 10);
    }

    /// <summary>
    ///     Shoots a proyectil
    /// </summary>
   /*  void Shoot() {
        //Create a new variable called pos, which will hold the currentCoroutine position of the player.
        Vector3 pos = transform.position;
        //Create a new variable called rotation, which will hold the currentCoroutine rotation of the player.
        Quaternion rotation = transform.rotation;
        //Set the x-axis and y-axis rotations of the rotation variable to 0 and add 90 to the z axis rotation.
        rotation.eulerAngles = new Vector3(0, 0, 0);
        //Add 0.5 to the x-axis position, to make sure the bullet spawns in front of the player.
        pos.y += 0.5f;
        //Spawn the bullet, with the position and rotation variables as parameters.
        Instantiate(proyectil, pos, rotation);
    } */

    // Update is called once per frame
    void Update() {
        // Move the player
        MovePlayer();
        /* // Check if the player is shooting and if the cooldown is less than or equal to 0
        if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))) {
            // Start the delay coroutine
            currentCoroutine = StartCoroutine(Delay());
        }

        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Jump")) {
            // Stop the delay coroutine
            StopCoroutine(currentCoroutine);
        } */
    }

    /* IEnumerator Delay() {
        while (true) {
            Shoot();
            yield return new WaitForSeconds(0.5f);
        }
    } */

    // Detects if the player has collided with an enemy
    /* void OnTriggerEnter(Collider other) {
        // If the player has collided with an enemy
        if (other.CompareTag("Enemy") || other.CompareTag("ProyectilEnemigo")) {
            // Reduce the player's lives by 1
            lives--;
            // Update the lives GUI
            // livesGUI._texto.text = "Lives: " + lives;

            //Reduce score by 20
            Score -= 20;
            // Update the score GUI
            // scoreGUI._texto.text = "Score: " + Score;
            // If the player has no more lives
            if (lives <= 0) {
                // Destroy the player
                Destroy(gameObject);
            } else { // If the player has more than 0 lives
                // Destroy the enemy
                Destroy(other.gameObject);
            }
        }
    } */
}
