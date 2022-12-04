using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject prefabExplosion;

    public int damage = 15;

    private int playerLayers; 

    void Start()
    {
        playerLayers = LayerMask.NameToLayer("Player");
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Enemy") {
            if (hitInfo.GetComponent<EnemyStatus>() != null) {
                hitInfo.GetComponent<EnemyStatus>().TakeDamage(damage);
            }
            
            if (hitInfo.GetComponent<MonsterStatus>() != null) {
                hitInfo.GetComponent<MonsterStatus>().TakeDamage(damage);
            }
        }

        if(hitInfo.gameObject.layer != playerLayers){
            Debug.Log("hit: " + hitInfo.name);
            StartCoroutine(HitHandler());
        }
    }

    IEnumerator HitHandler() {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
        Instantiate(prefabExplosion, transform.position, transform.rotation);
    }
}
