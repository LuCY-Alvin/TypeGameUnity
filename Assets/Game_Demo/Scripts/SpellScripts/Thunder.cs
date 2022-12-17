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
        if (hitInfo.gameObject.tag == "Enemy") {
            if (hitInfo.GetComponent<EnemyStatus>() != null) {
                hitInfo.GetComponent<EnemyStatus>().TakeDamage(12);
            }
            
            if (hitInfo.GetComponent<MonsterStatus>() != null) {
                StartCoroutine(hitInfo.GetComponent<MonsterStatus>().TakeDamage(12));
            }
        }
    }

}
