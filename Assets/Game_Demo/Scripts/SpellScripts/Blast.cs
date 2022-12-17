using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    public Collider2D hitObjectInfo;
    private bool hit = false;
    void Start()
    {
        Destroy(gameObject, 1.5f);
        InvokeRepeating ("HitHandler", 0.5f, 1f); 
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Enemy") {
            hitObjectInfo = hitInfo;
            hit = true;
        }
    }

    void HitHandler() {
        if (hit) {
            hit = false;
            if (hitObjectInfo != null) {
                if (hitObjectInfo.GetComponent<EnemyStatus>() != null) {
                    hitObjectInfo.GetComponent<EnemyStatus>().TakeDamage(20);
                }
                
                if (hitObjectInfo.GetComponent<MonsterStatus>() != null) {
                    StartCoroutine(hitObjectInfo.GetComponent<MonsterStatus>().TakeDamage(20));
                }
            }
        }
    }
}
