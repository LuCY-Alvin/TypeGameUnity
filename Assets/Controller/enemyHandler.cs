using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemyHandler : MonoBehaviour
{
    // public float speed;
    public Rigidbody2D _this;
    public GameObject skillObj;
    // Start is called before the first frame update
    void Start()
    {
        _this = this.gameObject.GetComponent<Rigidbody2D>();
        // StartCoroutine(jump());
        InvokeRepeating ("jump", 0, 5); 
        InvokeRepeating ("skill", 0, 3); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void jump()
    {
        _this.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
    }

    void skill()
    {
        Instantiate(skillObj, transform.position, Quaternion.identity);
    }

}
