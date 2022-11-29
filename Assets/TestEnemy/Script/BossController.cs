using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossController : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int atkDamage;
    [SerializeField] private float range;
    private float atkTimer = Mathf.Infinity;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Cast parameters")]
    [SerializeField] private float ultCooldown; // TODO: 60, 10, 0.5
    [SerializeField] private float ultCastingTime;
    [SerializeField] private float ultSpellingTime;
    [SerializeField] private float ultHealthPercentage;
    [SerializeField] private Transform[] firePoint = new Transform[2];
    [SerializeField] private GameObject prefabMeteor;
    
    private float ultTimer = Mathf.Infinity;
    private string ultName = "METEOR SHOWER";

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float colliderDistance;
    
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    
    [Header ("Ult Name")]
    [SerializeField] private GameObject prefabUltName;
    [SerializeField] private Canvas bossCanvas;
    [SerializeField] private Transform namePoint;
    private bool isNameShow = false;
    private GameObject ultNameObject;

    private Animator anim;
    private AnimatorStateInfo animState;
    private GameObject player;

    private EnemyStatus status;

    private void Awake() {
        initScale = transform.localScale;
        anim = GetComponent<Animator>();
        status = GetComponent<EnemyStatus>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update(){
        atkTimer += Time.deltaTime;
        ultTimer += Time.deltaTime;
        animState = anim.GetCurrentAnimatorStateInfo(0);
        
        if( status.GetCurrentHealth() <= status.GetStartingHealth() * ultHealthPercentage ){
        // TODO: cast

            if( ultTimer >= ultCooldown && !animState.IsTag("Inv")){
                ultTimer = 0;
                CastUlt();
            }
        }

        if(animState.IsName("cast")) { 
            CastUlt(); 

            if( !isNameShow ){
                ultNameObject = Instantiate(prefabUltName, bossCanvas.transform) as GameObject;
                ultNameObject.transform.position = namePoint.position;
                ultNameObject.GetComponent<TMP_Text>().text = ultName;
                isNameShow = true;
            }
        } 
        else {
            if(isNameShow){
                Destroy(ultNameObject);
                isNameShow = false;
                anim.SetBool("casting", false);
            }
        }


        if(animState.IsName("spell")) { 
            SpellUlt(); 
        }
        
        if( PlayerInSight() && atkTimer >= attackCooldown && !animState.IsTag("Inv")){
        // Attack only when player in sight
            atkTimer = 0;
            anim.SetTrigger("meleeAttack");
        }
        
        if(!PlayerInSight() && (animState.IsName("idle") || animState.IsName("move"))){
        // Chase player
            if (movingLeft){
                if(transform.position.x >= player.transform.position.x)
                    MoveInDirection(-1);
                else
                    DirectionChange();
            } 
            else {
                if(transform.position.x <= player.transform.position.x)
                    MoveInDirection(1);
                else
                    DirectionChange();
            }
        }
        else {
            anim.SetBool("moving", false);
        }
    }

    private void MoveInDirection(int dir){
        anim.SetBool("moving", true);

    // Make enemy face direction
        transform.localScale = new Vector3(Mathf.Abs(initScale.x) * dir, initScale.y, initScale.z);

    // Move in that direction
        transform.position = new Vector3(
            transform.position.x + Time.deltaTime * dir * speed,
            transform.position.y,
            transform.position.z
        );
    }

    private void CastUlt(){
        anim.SetBool("casting", true);
   
        if(ultTimer >= ultCastingTime){
            ultTimer = 0;
            SpellUlt();
        }

    }

    private void SpellUlt(){
        anim.SetBool("spelling", true);
        anim.SetBool("casting", false);
   
        if(ultTimer >= ultSpellingTime){
            anim.SetBool("spelling", false);
            ultTimer = 0;
        }
    }

    private void CallMeteor(){
        Vector3 tar = new Vector3(
            Random.Range(firePoint[0].position.x, firePoint[1].position.x),
            firePoint[0].position.y,
            firePoint[0].position.z
        );

        Instantiate(prefabMeteor, tar, firePoint[0].rotation);
    }


    private void DirectionChange(){
        anim.SetBool("moving", false);
        movingLeft = !movingLeft;
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

    private bool IsRage(){
        return status.GetCurrentHealth() <= status.GetStartingHealth()*ultHealthPercentage;
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
            player.GetComponent<PlayerMovement>().TakeDamage(atkDamage, this.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player")
            collision.GetComponent<PlayerMovement>().TakeDamage(atkDamage, this.transform);
    }

    public bool IsFacingLeft(){ return movingLeft; }

    public bool CancelUlt(string spell){
        if(animState.IsName("cast") && spell.Equals(ultName, System.StringComparison.OrdinalIgnoreCase)) {
            anim.SetTrigger("hurt");
            anim.SetBool("casting", false);
            ultTimer = 0;
            return true;
        } else return false;
    }
}
