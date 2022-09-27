using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHandler : MonoBehaviour
{
    public Rigidbody2D _this;
    // Start is called before the first frame update
    void Start()
    {
    	_this = this.gameObject.GetComponent<Rigidbody2D>();

        InvokeRepeating ("left", 2, 4); 
        InvokeRepeating ("right", 0, 4); 
    }

    void left()
    {	
    	_this.velocity = new Vector2(0, 0);
        _this.AddForce(new Vector2(-3, 0), ForceMode2D.Impulse);
    }

    void right()
    {	
    	_this.velocity = new Vector2(0, 0);
        _this.AddForce(new Vector2(3, 0), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
