using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Attack : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D _this;
    public GameObject prefabExplosion;

    public GameObject thePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _this = this.gameObject.GetComponent<Rigidbody2D>();
        _this.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            collision.GetComponent<PlayerMovement>().TakeDamage(10, transform);
        }

        if (collision.tag != "Enemy") {
            Destroy(gameObject);
            Instantiate(prefabExplosion, transform.position, transform.rotation);
        }
    }
}
