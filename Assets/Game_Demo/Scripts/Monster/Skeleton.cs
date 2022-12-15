using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Animator animator;
    private bool inAttack = false;
    private bool fire = false;
    private bool stop = false;
    private int i = 0;
    private int attCount = 0;
    public GameObject thePrefab;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        InvokeRepeating ("Action", 0f, 0.1f); 
    }

    void Action()
    {   
        if (PlayerMovement._playerTransform != null) {
            if (
                Mathf.Abs(PlayerMovement._playerTransform.position.x - transform.position.x) < 10
            ) {
                // Attack
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
                
                if (attCount > 20) {
                    if (!inAttack) {
                        if (!stop) {
                            animator.SetBool("Skeleton_Attack", true);
                            stop = true;
                        }

                        if (attCount > 38) {
                            if (!fire) {
                                Instantiate(thePrefab, transform.position, transform.rotation);
                                fire = true;
                                animator.SetBool("Skeleton_Attack", false);
                            }
                            
                            if (attCount > 47) {
                                inAttack = true;
                                fire = false;
                                attCount = 0;
                                stop = false;
                                
                            }   
                        }
                    } else {
                        inAttack = false;
                        attCount = 0;
                    }
                }
                attCount ++;
            } else {
                animator.SetBool("Skeleton_Attack", false);
            }

            if (!stop) {
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
                        transform.position.x - 0.1f,
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
                        transform.position.x + 0.1f,
                        transform.position.y,
                        transform.position.z
                    );
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator BreakSecond(float seconds) {
        yield return new WaitForSeconds(seconds);
    }
}
