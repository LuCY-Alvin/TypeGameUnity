using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    //private PlayerMovement playerMovement;
    
    Vector2 start;
    float i = 0;

    void Start()
    {
        start = transform.position;
        //playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        i += 0.01f;
        transform.position = start + Vector2.up * Mathf.PingPong(i, (float) 1.5);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(3, this.transform);
        }
    }

}
