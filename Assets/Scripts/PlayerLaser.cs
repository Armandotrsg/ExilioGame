using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField]private float damage = 10f;

    private Player player;

    private Score score;

    // Start is called before the first frame update
    [SerializeField]private float speed = 2f;
    void Start()
    {
        transform.Rotate(90, 0, 0); // Rotate the bullet
        Destroy(gameObject, 5f); // Destroy the bullet after 5 seconds
        player = Player.Instance; // So player is not null
        score = Score.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed, Space.Self); // Move the bullet    
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")){
            other.GetComponent<Enemy>().Damage(damage);
            Destroy(gameObject);
            player.Score += 10;
            player.Kills += 1;
            score._texto.text = "Score: " + player.Score;
        }
    }
}
