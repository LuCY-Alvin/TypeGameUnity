using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Attack : MonoBehaviour
{
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.5f);
        InvokeRepeating ("Action", 0f, 0.1f); 
    }

    // Update is called once per frame
    void Update()
    {
        var step =  speed * Time.deltaTime;
        if (PlayerMovement._playerTransform != null) {
            transform.position = Vector3.MoveTowards(transform.position, PlayerMovement._playerTransform.position, step);
        }
        
    }

    void Action()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            collision.GetComponent<PlayerMovement>().TakeDamage(10, transform);
        }

        if (collision.tag != "Enemy") {
            Destroy(gameObject);
        }
    }
}
