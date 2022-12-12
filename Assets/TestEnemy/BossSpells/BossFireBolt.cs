using System.Collections;
using UnityEngine;

public class BossFireBolt : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject prefabExplosion;

    public int damage = 15;

    private int bossLayer; 

    void Start()
    {
        bossLayer = LayerMask.NameToLayer("Boss");
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Player") {
            if (hitInfo.GetComponent<PlayerMovement>() != null) {
                rb.velocity = transform.right * 0;
                hitInfo.GetComponent<PlayerMovement>().TakeDamage(damage, transform);
            }
        }

        if(hitInfo.gameObject.layer != bossLayer){
            // Debug.Log("boss firebolt hit: " + hitInfo.name);
            StartCoroutine(HitHandler());
        }
    }

    IEnumerator HitHandler() {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
        Instantiate(prefabExplosion, transform.position, transform.rotation);
    }
}
