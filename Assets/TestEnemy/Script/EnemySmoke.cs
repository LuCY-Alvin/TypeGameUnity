using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmoke : MonoBehaviour
{
    [Header ("Smoke status")]
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private int damage = 3;
    [SerializeField] private float hurtDuration = 1f;
    [SerializeField] private float smokeDuration = 5f;

    [SerializeField] private Vector3 startScale = new Vector3(1, 1, 1);
    [SerializeField] private Vector3 maxScale = new Vector3(4, 2, 1);

    private float hurtTimer = Mathf.Infinity;
    private float smokeTimer; 

    [SerializeField] private Rigidbody2D rb;
    private int playerLayer;
    private int direction;

    // TODO: smoke持續時間、每秒傷害玩家、範圍擴散
    private void Awake() {
        hurtTimer = 0;
        smokeTimer = 0;

        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void Update() {
        hurtTimer += Time.deltaTime;
        smokeTimer += Time.deltaTime;

    // smoke size 
        float timeScale = smokeTimer / smokeDuration;
        transform.localScale = startScale + (maxScale - startScale) * timeScale;

    // smoke time count
        if(smokeTimer >= smokeDuration){
            Debug.Log("Smoke end");
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D hit) {
        if(hit.gameObject.layer == playerLayer){ // if player in smoke

            if(hurtTimer >= hurtDuration){
                Debug.Log("Smoke hurt player!");
                hurtTimer = 0;
                hit.GetComponent<PlayerMovement>().TakeDamage(damage, this.transform);
            }
        }
        
    }

    public void ShootInDirection(int dir){ 
        rb.velocity = transform.right * speed * dir;
    }

}
