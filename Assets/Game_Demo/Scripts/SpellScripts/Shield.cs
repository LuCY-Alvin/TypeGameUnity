using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Shield : MonoBehaviour
{
    public PlayerMovement _playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        hiddenShield();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hiddenShield() 
    {
        Vector3 vector = transform.localScale;
        vector.x = 0;
        transform.localScale = vector;
    }
    
    public void CallShield(Spell[] supportSpells)
    {
        StartCoroutine(showShield(supportSpells));
    }

    IEnumerator showShield(Spell[] supportSpells) {
        float time = 2f;

        var extendSupport = Array.Find(
            supportSpells,
            item => item.name == "extend"
        );

        if (extendSupport != null) {
            time = 4f;
        }

        Vector3 vector = transform.localScale;
        vector.x = 1.67f;
        transform.localScale = vector;
        _playerMovement.isInvincible = true;

        yield return new WaitForSeconds(time);

        _playerMovement.isInvincible = false;

        hiddenShield();
    }
}
