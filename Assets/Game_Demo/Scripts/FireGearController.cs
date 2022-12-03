using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGearController : MonoBehaviour
{
    public GameObject prefabFlame;
    public Transform firePoint;
    private Coroutine startFlaming;

    // Start is called before the first frame update
    void Start()
    {
        
       startFlaming = StartCoroutine(shootFlames());
    }

    IEnumerator shootFlames()
    {      
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameObject newObject = Instantiate(prefabFlame, firePoint.position + new Vector3((float) -1.5, (float) 0.45, 0), firePoint.rotation) as GameObject;
        }
           
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "Ice_Spear(Clone)")
        {
            StopCoroutine(startFlaming);
        }
    }
}
