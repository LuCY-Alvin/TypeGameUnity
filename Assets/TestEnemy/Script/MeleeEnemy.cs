using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Status Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int atkDamage;
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;
    
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private PlayerMovement player;

    private EnemyPatrol enemyPatrol;
    private EnemyStatus enemyStatus;

    private void Awake() {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        enemyStatus = GetComponent<EnemyStatus>();
    }

    private void Update(){
        cooldownTimer += Time.deltaTime;

        // Attack only when player in sight
        if( PlayerInSight() && cooldownTimer >= attackCooldown ){
        // Attack
            if(cooldownTimer >= attackCooldown){
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if(enemyPatrol != null){
            enemyPatrol.enabled = !PlayerInSight() && 
                                  (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("move"));
        }
    }

    private bool PlayerInSight(){
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, 
            Vector2.left, 
            0,
            playerLayer
        );

    // get player health
        if(hit.collider != null){
            player = hit.transform.GetComponent<PlayerMovement>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }

    private void DamagePlayer(){
        // If player still in range damage him 
        if( PlayerInSight() ){
            player.TakeDamage(atkDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player")
            collision.GetComponent<PlayerMovement>().TakeDamage(atkDamage);
    }

}
