using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireBolt : MonoBehaviour
{
    public float speed = 12f;
    public Rigidbody2D _this;
    public GameObject prefabExplosion;
    public int damage = 10;
    private int playerLayers; 
    float randomY;

    void Start()
    {
        _this = this.gameObject.GetComponent<Rigidbody2D>();
        playerLayers = LayerMask.NameToLayer("Player");
        _this.velocity = transform.right * speed;

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
