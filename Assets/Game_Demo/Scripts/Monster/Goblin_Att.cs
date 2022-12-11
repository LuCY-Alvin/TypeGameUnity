using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Att : MonoBehaviour
{
    
    public float speed = 3f;
    private Rigidbody2D _this;

    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        _this = this.gameObject.GetComponent<Rigidbody2D>();
        if (PlayerMovement._playerTransform != null) {
            float xForce = PlayerMovement._playerTransform.position.x - transform.position.x;
            if (xForce > 5) {
                xForce = 5;
            } else if (xForce < -5) {
                xForce = -5;
            }
            _this.AddForce(new Vector2(xForce, 3), ForceMode2D.Impulse);
        } 

        Destroy(gameObject, 1.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (count < 200) {
            count ++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player" && count > 199) {
            collision.GetComponent<PlayerMovement>().TakeDamage(10, transform);
            Destroy(gameObject);
        }
    }
}
