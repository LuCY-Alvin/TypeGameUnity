using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Requirement]
public class roleHandler : MonoBehaviour
{
    public float speed;
    public Rigidbody2D _this;
    private bool canExtraMove = true;
    private bool canSprint = true;

    // Start is called before the first frame update
    void Start()
    {
    	_this = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canExtraMove) {
        	_this.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
            canExtraMove = false;
        }

        float rotateY = this.gameObject.transform.rotation.y;

        if (Input.GetKey(KeyCode.RightArrow)) {
            if (rotateY != 0) {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    0,
                    transform.eulerAngles.z
                );
            }

        	this.gameObject.transform.position += new Vector3(speed, 0, 0);

        	if (Input.GetKeyDown(KeyCode.C) && canSprint) {
        		_this.AddForce(new Vector2(3, 0), ForceMode2D.Impulse);
                canSprint = false;
        	}
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (rotateY != -180) {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    180,
                    transform.eulerAngles.z
                );
            }
            
        	this.gameObject.transform.position -= new Vector3(speed, 0, 0);

        	if (Input.GetKeyDown(KeyCode.C) && canSprint) {
        		_this.AddForce(new Vector2(-3, 0), ForceMode2D.Impulse);
                canSprint = false;
        	}
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {	

    	print("tag, " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Ground") {
            canExtraMove = true;
            canSprint = true;
            print(canExtraMove);
        }

        if (collision.gameObject.tag == "Ground") {
            canExtraMove = true;
            print(canExtraMove);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("ApplyDamage", 10);
        }
    }
}
