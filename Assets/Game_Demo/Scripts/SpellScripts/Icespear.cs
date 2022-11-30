using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icespear : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    private int playerLayers; 

    void Start()
    {
        playerLayers = LayerMask.NameToLayer("Player");
        rb.velocity = transform.right * speed;

        Spell[] supportSpells = SpellController.supportSpellsList;

        var superSupport = Array.Find(
            supportSpells,
            item => item.name == "super"
        );

        if (superSupport != null) {
            transform.localScale += new Vector3(2,2,0);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Enemy") {
            hitInfo.GetComponent<EnemyStatus>().TakeDamage(10);
        }

        if(hitInfo.gameObject.layer != playerLayers){
            Debug.Log("hit: " + hitInfo.name);
            Destroy(gameObject);
        }
    }
}
