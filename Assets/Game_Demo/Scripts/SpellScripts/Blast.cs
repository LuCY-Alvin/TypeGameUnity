using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.gameObject.tag);
        if (hitInfo.gameObject.tag == "Enemy") {
            hitInfo.GetComponent<EnemyStatus>().TakeDamage(30);
        }
    }
}
