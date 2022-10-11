using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmear : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    private int enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayers = LayerMask.NameToLayer("Enemy");
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void onTriggerEnter2D(Collider2D hitinfo)
    {
        if(hitinfo.gameObject.layer != enemyLayers)
        {
            Debug.Log("hit:" + hitinfo.name);
            Destroy(gameObject);
        }
    }
}
