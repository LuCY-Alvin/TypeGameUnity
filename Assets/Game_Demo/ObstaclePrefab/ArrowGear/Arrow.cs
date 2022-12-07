using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed = 20f;
    //public Rigidbody2D rb;
    
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * (-speed);
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Player")
        {
            hitInfo.GetComponent<PlayerMovement>().TakeDamage(5, this.transform);
            Destroy(gameObject);
        }

        if (hitInfo.gameObject.name.Contains("Earth Bump"))
        {
            Destroy(gameObject);
        }
    }
}
