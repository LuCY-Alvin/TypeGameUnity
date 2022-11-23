using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    private int playerLayers; 

    void Start()
    {
        playerLayers = LayerMask.NameToLayer("Player");
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Enemy") {
            hitInfo.GetComponent<EnemyStatus>().TakeDamage(15);
        }

        if(hitInfo.gameObject.layer != playerLayers){
            Debug.Log("hit: " + hitInfo.name);
            Destroy(gameObject);
        }
    }
}
