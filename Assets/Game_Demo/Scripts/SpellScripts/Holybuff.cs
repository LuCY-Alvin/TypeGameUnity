using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holybuff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        Spell[] supportSpells = SpellController.supportSpellsList;

        var superSupport = Array.Find(
            supportSpells,
            item => item.name == "super"
        );

        if (superSupport != null) {
            transform.localScale += new Vector3(1,1,0);
        }
        Destroy(gameObject, 0.35f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
