using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroGearController : MonoBehaviour
{
    public GameObject EntranceLevel3;
    public GameObject guide;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("next level"))
        {
            if (PlayerPrefs.GetString("next level") != "Level3")
            {
                gameObject.SetActive(false);
            }
            else
            {
                guide.SetActive(false);
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
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
            Invoke("smoothGuideCamera", 1f);
            
        }
    }

    void smoothGuideCamera()
    {
        GameObject.Find("Usual Camera").GetComponent<PlayerCamera>().smoothGuideCamera();
    }
}
