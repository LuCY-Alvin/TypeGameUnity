using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    private Animator animator;
    private bool inAttack = false;

    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        InvokeRepeating ("Action", 0f, 0.1f); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Action()
    {   
        if (PlayerMovement._playerTransform != null) {
            if (Mathf.Abs(PlayerMovement._playerTransform.position.x - transform.position.x) < 6) {
                if (transform.position.x > PlayerMovement._playerTransform.position.x) {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        180,
                        0
                    );
                } else {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        0,
                        0
                    );
                }

                if (Mathf.Abs(PlayerMovement._playerTransform.position.x - transform.position.x) < 3) {
                    animator.SetBool("Guard_Attack", true);
                    StartCoroutine(BreakHalfSecond());
                    return;
                } else {
                    animator.SetBool("Guard_Attack", false);
                }

                if (inAttack) return;

                if (transform.position.x > PlayerMovement._playerTransform.position.x) {
                    transform.position = new Vector3(
                        transform.position.x - 0.3f,
                        transform.position.y,
                        transform.position.z
                    );
                } else {
                    transform.position = new Vector3(
                        transform.position.x + 0.3f,
                        transform.position.y,
                        transform.position.z
                    );
                }
            } else {
                i ++;
                if (i >= 40) {
                    i = 0;
                    return;
                }
                if (i < 20) {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        180,
                        0
                    );

                    transform.position = new Vector3(
                        transform.position.x - 0.3f,
                        transform.position.y,
                        transform.position.z
                    );
                } else {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        0,
                        0
                    );

                    transform.position = new Vector3(
                        transform.position.x + 0.3f,
                        transform.position.y,
                        transform.position.z
                    );
                }
                
            }
        }
    }

    public IEnumerator BreakHalfSecond() {
        inAttack = true;
        yield return new WaitForSeconds(1f);
        inAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            collision.GetComponent<PlayerMovement>().TakeDamage(3, transform);
        }
    }
}
