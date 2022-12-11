using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Icespear : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D _this;

    private int playerLayers; 

    float randomY;
    void Start()
    {
        
        _this = this.gameObject.GetComponent<Rigidbody2D>();
        playerLayers = LayerMask.NameToLayer("Player");
        _this.velocity = transform.right * speed;

        Spell[] supportSpells = SpellController.supportSpellsList;

        var superSupport = Array.Find(
            supportSpells,
            item => item.name == "super"
        );

        if (superSupport != null) {
            transform.localScale += new Vector3(2,2,0);
        }

        randomY = Random.Range(-0.01f, 0.03f);
    }

    void Update() {
        Spell[] supportSpells = SpellController.supportSpellsList;

        var snapshotSupport = Array.Find(
            supportSpells,
            item => item.name == "snapshot"
        );

        if (snapshotSupport != null) {
            gameObject.transform.position += new Vector3(0, randomY, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Enemy") {
            if (hitInfo.GetComponent<EnemyStatus>() != null) {
                hitInfo.GetComponent<EnemyStatus>().TakeDamage(10);
            }
            
            if (hitInfo.GetComponent<MonsterStatus>() != null) {
                hitInfo.GetComponent<MonsterStatus>().TakeDamage(10);
            }
        }

        if(hitInfo.gameObject.layer != playerLayers){
            Destroy(gameObject);
        }
    }
}
