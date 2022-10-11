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

    public void Heal(){
        var cost = 40;
        var currentMp = _healthBar.GetCurrentMp();

        if (cost < currentMp) {
            Instantiate(prefabHeal, healPoint.position, healPoint.rotation);
            var currentHp = _healthBar.GetCurrentHp();
            
            _healthBar.SetValue(currentHp + 30, "hp");
            _healthBar.SetValue(currentMp - cost, "mp");
        }
    }

    public void Firebolt(){
        var cost = 60;
        var currentMp = _healthBar.GetCurrentMp();

        if (cost < currentMp) {
            Instantiate(prefabFirebolt, firePoint.position, firePoint.rotation);
            _healthBar.SetValue(currentMp - cost, "mp");
        }
    }




}
