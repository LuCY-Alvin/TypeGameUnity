using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.gameObject.tag);
        if (hitInfo.gameObject.tag == "Enemy") {
            hitInfo.GetComponent<EnemyStatus>().TakeDamage(20);
        }
    }

}
