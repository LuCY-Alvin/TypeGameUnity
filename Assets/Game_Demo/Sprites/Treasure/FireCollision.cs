using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollision : MonoBehaviour
{
    public bool _fireCollided;

    void Start()
    {
        _fireCollided = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_fireCollided)
        {
            Destroy(gameObject, (float)0.3);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("FireObject"))
        {
            _fireCollided = true;
        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("FireObject"))
        {
            _fireCollided = true;
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("FireObject"))
        {
            _fireCollided = false;
        }
    }


}
