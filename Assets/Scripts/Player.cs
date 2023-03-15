using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {
    
    [SerializeField]
    private float minY, maxY, minX, maxX, speed;

    private bool isDashing = false;
    private int rightDash = 1;
    private float dashTime = 1.2f;

    [SerializeField]
    private PlayerLaser laser;


    private static Player _instance;

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
    }

    /// <summary>
    ///     Shoots a laser
    /// </summary>
    void Shoot() {
        //Create a new variable called pos, which will hold the current position of the player.
        Vector3 pos = transform.position;
        //Add 0.5 to the x-axis position, to make sure the bullet spawns in front of the player.
        pos.z += 0.5f;

        //Spawn the bullet, with the position and rotation variables as parameters.
        Instantiate(laser, pos, transform.rotation);
    }

    // Update is called once per frame
    void Update() {
        // Move the player
        if (!isDashing) {
            MovePlayer();
            if (Input.GetKeyDown(KeyCode.E)) {
                isDashing = true;
                rightDash = 1;
            } else if (Input.GetKeyDown(KeyCode.Q)) {
                isDashing = true;
                rightDash = -1;
            }
        } else { // If the player is dashing
            Dash();
        }
        /* // Check if the player is shooting and if the cooldown is less than or equal to 0

        // Checks if the player is shooting
        if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))) {
            // Start the delay coroutine
            currentCoroutine = StartCoroutine(Delay());
        }

        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Jump")) {
            // Stop the delay coroutine
            StopCoroutine(currentCoroutine);
        }
    }

    IEnumerator Delay() {
        while (true) {
            Shoot();
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Detects if the player has collided with an enemy
    /* void OnTriggerEnter(Collider other) {
        // If the player has collided with an enemy
        if (other.CompareTag("Enemy") || other.CompareTag("laserEnemigo")) {
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

    /// <summary>
    ///     Dash mechanic
    /// </summary>
    void Dash() {
        transform.Translate(new Vector3(rightDash, 0, 0) * Time.deltaTime * speed, Space.World); // Move the player
        transform.Rotate(new Vector3(0, 0, -1 * rightDash) * Time.deltaTime * 305, Space.Self); // Rotate the player
        dashTime -= Time.deltaTime; // Reduce the dash time
        if (dashTime <= 0) {
            isDashing = false;
            dashTime = 1.2f;
        }
        //Check if the player is out of bounds
        if (transform.position.x > maxX && rightDash > 0) {
            isDashing = false;
            dashTime = 1.2f;
        }else if (transform.position.x < minX && rightDash < 0) {
            isDashing = false;
            dashTime = 1.2f;
        }
    }
}
