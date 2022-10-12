using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public Transform firePoint;
    public Transform healPoint;
    public GameObject prefabFirebolt;
    public GameObject prefabHeal;

    public HealthBar _healthBar;

    public void Heal(string[] list){
        var cost = 40;
        var currentMp = _healthBar.GetCurrentMp();

        if (cost < currentMp) {
            Instantiate(prefabHeal, healPoint.position, healPoint.rotation);
            var currentHp = _healthBar.GetCurrentHp();
            
            _healthBar.SetValue(currentHp + 30, "hp");
            _healthBar.SetValue(currentMp - cost, "mp");
        }
    }

    public void Firebolt(string[] list){
        var cost = 60;
        var currentMp = _healthBar.GetCurrentMp();

        if (cost < currentMp) {
       
            foreach(var item in list)
            {
                print(item.ToString());
            }
            if (Array.IndexOf(list, "multi") >= 0) {
                StartCoroutine(multiFirebolt(list));
            } else {
                GameObject newObject = Instantiate(prefabFirebolt, firePoint.position, firePoint.rotation) as GameObject;
                if (Array.IndexOf(list, "super") >= 0) {
                    newObject.transform.localScale += new Vector3(1,1,0);
                }
            }
        }

        _healthBar.SetValue(currentMp - cost, "mp");
    }

    IEnumerator multiFirebolt(string[] list) {
        float waitTime = 0.2f;
        for (int i = 0; i < 3; i++) {
            GameObject newObject = Instantiate(prefabFirebolt, firePoint.position, firePoint.rotation) as GameObject;
            if (Array.IndexOf(list, "super") >= 0) {
                newObject.transform.localScale += new Vector3(1,1,0);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }


}
