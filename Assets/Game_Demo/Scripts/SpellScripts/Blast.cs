using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f);
        Spell[] supportSpells = SpellController.supportSpellsList;

        var superSupport = Array.Find(
            supportSpells,
            item => item.name == "super"
        );

        if (superSupport != null) {
            transform.localScale += new Vector3(2,2,0);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.gameObject.tag);
        if (hitInfo.gameObject.tag == "Enemy") {
            hitInfo.GetComponent<EnemyStatus>().TakeDamage(20);
        }
    }
}
