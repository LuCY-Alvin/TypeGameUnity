using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    [Header ("Status Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int atkDamage;
    [SerializeField] private float range;

    [Header ("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject prefabRange;

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;
    
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private EnemyPatrol enemyPatrol;

    private void Awake() {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update(){
        cooldownTimer += Time.deltaTime;

        // Attack only when player in sight
        if( PlayerInSight() && cooldownTimer >= attackCooldown ){
        // Attack
            cooldownTimer = 0;
            anim.SetTrigger("meleeAttack");

            GameObject smoke = Instantiate(prefabRange, firePoint.position, firePoint.rotation) as GameObject;
            smoke.GetComponent<EnemySmoke>().ShootInDirection( enemyPatrol.IsFacingLeft() ? -1 : 1 );
        }

        if(enemyPatrol != null){
            enemyPatrol.enabled = (!PlayerInSight()) 
                                && (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("move"));
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

        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }
}
