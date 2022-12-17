using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroGearController : MonoBehaviour
{
    public GameObject EntranceLevel3;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("next level"))
        {
            if (PlayerPrefs.GetString("next level") != "Level3")
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {    
        if (c.gameObject.name == "Thunder(Clone)")
        {
            EntranceLevel3.SetActive(true);
        }
    }
}
