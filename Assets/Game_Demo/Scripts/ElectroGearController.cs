using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroGearController : MonoBehaviour
{
    public GameObject EntranceLevel3;
    
    
    void OnTriggerEnter2D(Collider2D c)
    {    
        if (c.gameObject.name == "Thunder(Clone)")
        {
            EntranceLevel3.SetActive(true);
        }
    }
}
