using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    [SerializeField]private float speed = 3000f;
    [SerializeField] private float damage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Check in which direction the bullet moves
        transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * speed, Space.World); // Move the bullet\
    }
}
