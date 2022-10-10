using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public Transform firePoint;
    public Transform healPoint;
    public GameObject prefabFirebolt;
    public GameObject prefabHeal;

    public void Heal(){
        Instantiate(prefabHeal, healPoint.position, healPoint.rotation);
    }

    public void Firebolt(){
        Instantiate(prefabFirebolt, firePoint.position, firePoint.rotation);
    }




}
