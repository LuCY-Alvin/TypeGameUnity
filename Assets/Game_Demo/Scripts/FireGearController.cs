using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGearController : MonoBehaviour
{
    public GameObject prefabFlame;
    public Transform firePoint1;
    public Transform firePoint2;
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
            GameObject newObject1 = Instantiate(prefabFlame, firePoint1.position + new Vector3((float) -1.3, (float) 0.3, 0), firePoint1.rotation) as GameObject;
            GameObject newObject2 = Instantiate(prefabFlame, firePoint2.position + new Vector3((float) 1.3, (float) 0.3, 0), firePoint2.rotation) as GameObject;
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
