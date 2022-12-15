using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Debug.Log(hitInfo.gameObject.tag);
        if (hitInfo.gameObject.tag == "Player")
        {
            hitInfo.GetComponent<PlayerMovement>().TakeDamage(5, this.transform);
        }
    }
}
