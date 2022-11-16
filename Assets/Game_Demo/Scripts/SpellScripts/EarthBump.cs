using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBump : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Handler());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Handler() {
        yield return new WaitForSeconds(0.6f);

        Destroy(gameObject);
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
