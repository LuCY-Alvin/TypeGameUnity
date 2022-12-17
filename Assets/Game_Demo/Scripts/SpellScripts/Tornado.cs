using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float speed = 0.01f;
    public Rigidbody2D rb;
    float lockPos = 0;
    bool right = true;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2.25f);

        if (transform.right.x < 0) {
            right = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(lockPos, lockPos, lockPos);
        if (right) {
            transform.position += new Vector3(0.01f, 0, 0);
        } else {
            transform.position -= new Vector3(0.01f, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Enemy") {
            if (hitInfo.GetComponent<EnemyStatus>() != null) {
                hitInfo.GetComponent<EnemyStatus>().TakeDamage(5);
            }
            
            if (hitInfo.GetComponent<MonsterStatus>() != null) {
                StartCoroutine(hitInfo.GetComponent<MonsterStatus>().TakeDamage(5));
            }
        }
    }

}
