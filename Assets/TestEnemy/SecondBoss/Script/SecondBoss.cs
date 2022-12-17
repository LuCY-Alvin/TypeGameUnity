using System;
using TMPro;
using UnityEngine;

public class SecondBoss : MonoBehaviour
{
    enum Task { Inactive, Running, Success, Failed }
    enum Status { Idle, Teleport, Attack, Lightning}
    
    private Status status;
    private Task task;
    int movementNum = 4;
    
    [Header("Idle")]
    [SerializeField] private float idelTime = 3.0f;
    private float idleTimer = 0f;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private bool isMovingLeft;

    [Header("Spell")]
    [SerializeField] private GameObject prefabUltName;
    [SerializeField] private Canvas bossCanvas;
    [SerializeField] private Transform namePoint;
    private bool isNameShow = false;
    private GameObject ultNameObject;
    private String spellBuffer = "";
    private float spellTimer = 0;

    public BoxCollider2D enemyCollider;
    public EnemyStatus enemyStatus;
    private float baseScaleX;
    private Rigidbody2D body;
    private Animator anim;
    private GameObject player;

    private void Awake(){
        baseScaleX = transform.localScale.x;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        status = Status.Idle;
        task = Task.Inactive;
    }

    private void Update(){
        lightTimer += Time.deltaTime;

    /* Behaviour */
        switch (status){
            case Status.Idle: Idle(); break;
            case Status.Teleport: TeleportSequence(); break;
            case Status.Attack: AttackSequence(); break;
            case Status.Lightning: LightningSequence(); break;
        }

    /* Task Check */
        switch(task){
            case Task.Inactive:
            case Task.Running: break;
            case Task.Success:
            case Task.Failed: 
                task = Task.Inactive;
                status = Status.Idle; 
                break;
        }
    }

    private void Idle(){
        idleTimer += Time.deltaTime;

        if(idleTimer <= idelTime) return;

    /* random select movement */ 
        status = (Status)UnityEngine.Random.Range(1, movementNum);
        idleTimer = 0;
    }
    
    private void Move(int dir){
        transform.position = new Vector3(
            transform.position.x + Time.deltaTime * dir * moveSpeed,
            transform.position.y,
            transform.position.z
        );
    }

    [Header("Teleport")]
    [SerializeField] private GameObject teleportOutPrefab;
    [SerializeField] private GameObject teleportInPrefab;
    [SerializeField] private GameObject darkBoltPrefab;

    private Vector3 teleportPoint;
    private float teleportScale;
    private GameObject teleportOutEffect, teleportInEffect, darkBoltEffect;
    private float teleportTimer = 0f, teleportTime = 1.2f;

    private void TeleportSequence(){
        if( task == Task.Inactive ){
            teleportPoint = player.transform.position;
            teleportPoint.y = 104.24f;
            CheckFace(player.transform);
            teleportOutEffect = Instantiate(teleportOutPrefab, transform.position, transform.rotation) as GameObject;
            task = Task.Running;
        }

    /* Running */
    // TeleportOut
        if(teleportTimer < teleportTime){
            teleportTimer += Time.deltaTime;
            teleportScale = Mathf.Abs(transform.localScale.x);

            if(teleportScale > 0.01){
                teleportScale -= Time.deltaTime * (1.5f / 0.2f);
            }
            else{
                teleportScale = 0.01f;
            }
            
            teleportScale *= (transform.localScale.x >= 0 ? 1 : -1);
            transform.localScale = new Vector3(teleportScale, Mathf.Abs(teleportScale), 0);
            if(teleportTimer >= teleportTime){
                darkBoltEffect =  Instantiate(darkBoltPrefab, teleportPoint, transform.rotation);
                transform.position = teleportPoint;
            }

            return;
        }

    // Dark bolt
        if(darkBoltEffect != null){
            return;
        }

    // TeleportIn
        if(darkBoltEffect==null && teleportInEffect==null){
            teleportInEffect = Instantiate(teleportInPrefab, teleportPoint, transform.rotation) as GameObject;
            return;
        }

        if(teleportInEffect != null){
            teleportScale = Mathf.Abs(transform.localScale.x);

            if(teleportScale < 1.5f){
                teleportScale += Time.deltaTime * (1.5f / 0.2f);
            }
            else{
                teleportScale = 1.5f;
                teleportTimer = 0;
                task = Task.Success;
            }
            
            teleportScale *= (transform.localScale.x >= 0 ? 1 : -1);
            transform.localScale = new Vector3(teleportScale, Mathf.Abs(teleportScale), 0f);
            return;
        }
    }

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask playerLayer;

    private Vector3 attackPosition;

    private void AttackSequence(){
        if( task == Task.Inactive ){
            attackPosition = player.transform.position;
            isMovingLeft = (attackPosition.x < transform.position.x);
            CheckFace(player.transform);
            task = Task.Running;
        }

    /* Running */
    // move to attack position 
        if(isMovingLeft && transform.position.x-attackRange >= attackPosition.x){
            anim.SetBool("IsMoving", true);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x+attackRange <= attackPosition.x){
            anim.SetBool("IsMoving", true);
            Move(1);
            return;
        }
        anim.SetBool("IsMoving", false);

    // attack
        CheckFace(player.transform);
        anim.SetTrigger("Attack");
        
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("idle")){
            task = Task.Success;
            return;
        }
    }

    public void Attack(){
        Collider2D[] hit =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hit){
            if (player.GetComponent<PlayerMovement>() != null) {
                player.GetComponent<PlayerMovement>().TakeDamage(attackDamage, attackPoint);
            }
        }
    }

	private void OnDrawGizmosSelected() {
		if(attackPoint == null) return;
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

    [Header("DarkLightning")]
    [SerializeField] private GameObject sparkPrefab;
    [SerializeField] private Transform sparkPoint;
    [SerializeField] private float lightHealthPercentage;
    [SerializeField] private Transform[] lightPoint = new Transform[2];
    [SerializeField] private float lightSpellingTime;

    private int lightCastTimes = 7, lightCastCount = 0;
    private float lightTimer, lightCooldown;
    private String lightName = "DarkLightning";

    private void LightningSequence(){
        if( task == Task.Inactive ){
            float healthP = enemyStatus.GetCurrentHealth() / enemyStatus.GetStartingHealth();
            if(lightTimer < lightCooldown || healthP > lightHealthPercentage){
                idleTimer += idelTime;
                task = Task.Failed;
                return;
            }

            int rand = UnityEngine.Random.Range(0, 2);
            isMovingLeft = (rand == 0);

            task = Task.Running; 
            return;
        }

    /* Running */
    // move to spell position 
        if(isMovingLeft && transform.position.x >= lightPoint[0].position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(lightPoint[0]);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= lightPoint[1].position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(lightPoint[1]);
            Move(1);
            return;
        }

        anim.SetBool("IsMoving", false);

    // spelling
        CheckFace(player.transform);
        spellTimer += Time.deltaTime;

        if( spellTimer <= lightSpellingTime ){
            anim.SetBool("IsSpelling", true);
            spellBuffer = lightName;
            ShowSpellName();
            return;
        }

    // cast
        anim.SetBool("IsSpelling", false);
        HideSpellName();

        if(lightCastCount < lightCastTimes){
            anim.SetBool("IsCasting", true);
            return;
        } else {
            anim.SetBool("IsCasting", false);
            lightTimer = 0;
            spellTimer = 0;
            spellBuffer = "";
            lightCastCount = 0;

            task = Task.Success;
            return;
        }
    }

    public void CastLightning(){
        float offset = lightCastCount * 5;
        Vector3 tar = new Vector3(
            transform.position.x + (offset * (transform.position.x < player.transform.position.x ? 1 : -1)),
            sparkPoint.position.y,
            sparkPoint.position.z
        );

        Instantiate(sparkPrefab, tar, sparkPoint.rotation);
        lightCastCount += 1;
    }
    
    private void CheckFace(Transform target){
        var scale = transform.localScale;
        scale.x = target.position.x < transform.position.x ? -baseScaleX : baseScaleX;
        transform.localScale = scale;
    }
    
    private void ShowSpellName(){
        if( !isNameShow ){
            ultNameObject = Instantiate(prefabUltName, bossCanvas.transform) as GameObject;
            if(player.transform.position.x < transform.position.x) ultNameObject.transform.Rotate(0, 180, 0);
            ultNameObject.transform.position = namePoint.position;
            ultNameObject.GetComponent<TMP_Text>().text = spellBuffer;
            isNameShow = true;
        }
    }

    private void HideSpellName(){
        if(isNameShow){
            Destroy(ultNameObject);
            isNameShow = false;
        }
    }

    public bool CancelSpell(string spell){
        if(spell.Equals( spellBuffer, System.StringComparison.OrdinalIgnoreCase) ) {
            anim.SetBool("IsSpelling", false);
            anim.SetTrigger("Hurt");
            enemyStatus.TakeDamage(0);
            HideSpellName();
            spellTimer = 0;
            spellBuffer = "";
            task = Task.Failed;
            status = Status.Idle;
            return true;
        } else return false;
    }

}
