using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Attack : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D _this;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerMovement._playerTransform != null) {
            float xForce = PlayerMovement._playerTransform.position.x - transform.position.x;
            if (xForce > 20) {
                xForce = 20;
            } else if (xForce < -20) {
                xForce = -20;
            }
            _this.AddForce(new Vector2(xForce, 30), ForceMode2D.Impulse);
        } 

        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.tag);
        if(collision.tag == "Player") {
            collision.GetComponent<PlayerMovement>().TakeDamage(10, transform);
            Destroy(gameObject);
        }
    }
}
