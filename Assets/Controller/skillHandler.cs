using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillHandler : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(speed, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {	
    	if (collision.gameObject.tag == "Role") {
    		Destroy(this.gameObject);
    	}
    }
}
