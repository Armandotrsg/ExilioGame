using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private float speed = 2f;
    void Start()
    {
        transform.Rotate(90, 0, 0); // Rotate the bullet
        Destroy(gameObject, 5f); // Destroy the bullet after 5 seconds
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed, Space.Self); // Move the bullet    
    }
}
