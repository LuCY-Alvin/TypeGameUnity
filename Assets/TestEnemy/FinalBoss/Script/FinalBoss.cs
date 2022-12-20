using System;
using TMPro;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    enum Task { Inactive, Running, Success, Failed }
    enum Status { Idle, Jump, Attack, Firebolt, Tornado, Blast }
    
    private Status status;
    private Task task;
    int movementNum = 6;
    
    [Header("Idle")]
    [SerializeField] private float idelTime = 3.0f;
    private float idleTimer = Mathf.Infinity;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForceX;
    [SerializeField] private float jumpForceY;
    private bool isGround;
    private bool isMovingLeft;

    [Header("Spell")]
    [SerializeField] private GameObject prefabUltName;
    [SerializeField] private Canvas bossCanvas;
    [SerializeField] private Transform namePoint;
    private bool isNameShow = false;
    private GameObject ultNameObject;

    private String spellBuffer = "";
    private float spellTimer = 0;

    private float baseScaleX;
    private Rigidbody2D body;
    private BoxCollider2D baseColl;
    public EnemyStatus enemyStatus;
    private Animator anim;
    private GameObject player;

    private void Awake(){
        baseScaleX = transform.localScale.x;
        body = GetComponent<Rigidbody2D>();
        baseColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        status = Status.Idle;
        task = Task.Inactive;
    }

    private void Update(){
    // TODO: Add Skills
    /* Behaviour */
        switch (status){
            case Status.Idle: Idle(); break;
            case Status.Jump: JumpSequence(); break;
            case Status.Attack: AttackSequence(); break;
            case Status.Firebolt: FireboltSequence(); break;
            case Status.Tornado: TornadoSequence(); break;
            case Status.Blast: BlastSequence(); break;
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

        CheckGround();
    }

    private void Idle(){
        idleTimer += Time.deltaTime;

        if(idleTimer <= idelTime) return;

    /* random select movement */ 
        status = (Status)UnityEngine.Random.Range(1, movementNum);
        idleTimer = 0;
    }

    private float jumpTimer = 0;
    private float jumpTime = 0.1f;
    private void JumpSequence(){
    /* inactive */
        if( task == Task.Inactive ){
            if(isGround){
                task = Task.Running;

                int direction = (player.transform.position.x < transform.position.x) ? -1 : 1;
                anim.SetTrigger("Jump");
                Jump(direction);
                return;
            } else {
                task = Task.Failed;
                return;
            }
        }

    /* Running */
        jumpTimer += Time.deltaTime;
        if(jumpTimer < jumpTime){
            task = Task.Running;
        }
        else{
            task = isGround ? Task.Success : Task.Running;
            CheckFace(player.transform);
        }

        if(task == Task.Success) jumpTimer = 0;
        return;
    }

    private void Jump(int dir){
        if(isGround == false) return;

        body.AddForce(new Vector2(jumpForceX * dir, jumpForceY));
    }
    
    private void Move(int dir){
        transform.position = new Vector3(
            transform.position.x + Time.deltaTime * dir * moveSpeed,
            transform.position.y,
            transform.position.z
        );
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
        
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Player_idle")){
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

    [Header("Firebolt")]
    [SerializeField] private GameObject fireboltPrefab;
    [SerializeField] private Transform fireboltLeftPoint;
    [SerializeField] private Transform fireboltRightPoint;
    [SerializeField] private Transform fireboltFirePoint;
    [SerializeField] private float fireBoltSpellingTime;
    private float fireboltCastCount = 0;
    private float fireboltCastTimes = 3;
    private String fireboltName = "DarkFirebolt";
    
    private void FireboltSequence() {
        if( task == Task.Inactive ){
            float nowX = transform.position.x; 
            isMovingLeft = Mathf.Abs(fireboltLeftPoint.position.x - nowX) < Mathf.Abs(fireboltRightPoint.position.x - nowX);

            task = Task.Running; 
            
            return;
        }

    /* Running */
    // move to spell position 
        if(isMovingLeft && transform.position.x >= fireboltLeftPoint.position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(fireboltLeftPoint);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= fireboltRightPoint.position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(fireboltRightPoint);
            Move(1);
            return;
        }

        anim.SetBool("IsMoving", false);

    // spelling
        CheckFace(player.transform);
        spellTimer += Time.deltaTime;

        if( spellTimer <= fireBoltSpellingTime ){
            anim.SetBool("IsSpelling", true);
            spellBuffer = fireboltName;
            ShowSpellName();
            return;
        }

    // cast
        anim.SetBool("IsSpelling", false);
        HideSpellName();

        if(fireboltCastCount < fireboltCastTimes){
            anim.SetBool("IsCastingFirebolt", true);
            return;
        } else {
            anim.SetBool("IsCastingFirebolt", false);
            spellTimer = 0;
            spellBuffer = "";
            fireboltCastCount = 0;

            task = Task.Success;
            return;
        }
    }
    
    public void CastFirebolt(){
        GameObject firebolt = Instantiate(fireboltPrefab, fireboltFirePoint.position, fireboltFirePoint.rotation);
        if(player.transform.position.x < transform.position.x) 
            firebolt.transform.Rotate(0f, 180f, 0f);

        fireboltCastCount += 1;
    }

    [Header("DarkTornado")]
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private Transform tornadoPoint;
    [SerializeField] private float tornadoPercentage;
    [SerializeField] private Transform[] tornadoPosition = new Transform[2];
    [SerializeField] private float tornadoSpellingTime;
    [SerializeField] private float tornadoCooldown;

    private float tornadoTimer;
    private bool isTornadoCasted = false;
    private String tornadoName = "DarkTornado";

    private void TornadoSequence(){
        if( task == Task.Inactive ){
            float healthP = enemyStatus.GetCurrentHealth() / enemyStatus.GetStartingHealth();
            if(tornadoTimer < tornadoCooldown || healthP > tornadoPercentage){
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
        if(isMovingLeft && transform.position.x >= tornadoPosition[0].position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(tornadoPosition[0]);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= tornadoPosition[1].position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(tornadoPosition[1]);
            Move(1);
            return;
        }

        anim.SetBool("IsMoving", false);

    // spelling
        CheckFace(player.transform);
        spellTimer += Time.deltaTime;

        if( spellTimer <= tornadoSpellingTime ){
            anim.SetBool("IsSpelling", true);
            spellBuffer = tornadoName;
            ShowSpellName();
            return;
        }

    // cast
        anim.SetBool("IsSpelling", false);
        HideSpellName();

        if(isTornadoCasted){
            tornadoTimer = 0;
            spellTimer = 0;
            spellBuffer = "";
            isTornadoCasted = false;

            task = Task.Success;
            return;

        } else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("cast_tornado")) {
            anim.SetTrigger("CastTornado");
            return;
        }
    }

    public void CastTornado(){
        for(int i=0; i<7; ++i){
            float offset = i * 5;
            Vector3 tar = new Vector3(
                transform.position.x + (offset * (transform.position.x < player.transform.position.x ? 1 : -1)),
                tornadoPoint.position.y,
                tornadoPoint.position.z
            );

            Instantiate(tornadoPrefab, tar, tornadoPoint.rotation);
        }
        isTornadoCasted = true;
    }
    
    [Header("Blast")]
    [SerializeField] private GameObject blastPrefab;
    [SerializeField] private Transform blastPosition;
    [SerializeField] private float blastPercentage;
    [SerializeField] private float blastSpellingTime;
    [SerializeField] private float blastCooldown;

    private float blastTimer;
    private bool isBlastCasted = false;
    private String blastName = "DarkSuperBlast";

    private void BlastSequence(){
        if( task == Task.Inactive ){
            float healthP = enemyStatus.GetCurrentHealth() / enemyStatus.GetStartingHealth();
            if(blastTimer < blastCooldown || healthP > blastPercentage){
                idleTimer += idelTime;
                task = Task.Failed;
                return;
            }

            isMovingLeft = (blastPosition.position.x < transform.position.x);
            CheckFace(player.transform);

            task = Task.Running; 
            return;
        }

    /* Running */
    // move to spell position
        if(isMovingLeft && transform.position.x >= blastPosition.position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(blastPosition);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= blastPosition.position.x){
            anim.SetBool("IsMoving", true);
            CheckFace(blastPosition);
            Move(1);
            return;
        }

        anim.SetBool("IsMoving", false);

    // spelling
        CheckFace(player.transform);
        spellTimer += Time.deltaTime;

        if( spellTimer <= blastSpellingTime ){
            anim.SetBool("IsSpelling", true);
            spellBuffer = blastName;
            ShowSpellName();
            return;
        }

    // cast
        anim.SetBool("IsSpelling", false);
        HideSpellName();

        if(isBlastCasted){
            blastTimer = 0;
            spellTimer = 0;
            spellBuffer = "";
            isBlastCasted = false;

            task = Task.Success;
            return;

        } else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("cast_blast")) {
            anim.SetTrigger("CastBlast");
            return;
        }
    }

    public void CastBlast(){
        Vector3 tar = player.transform.position;
        tar.y = blastPosition.position.y;
        Instantiate(blastPrefab, tar, player.transform.rotation);
        isBlastCasted = true;
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

    private void CheckGround(){
        isGround = baseColl.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void CheckFace(Transform target){
        var scale = transform.localScale;
        scale.x = target.position.x < transform.position.x ? -baseScaleX : baseScaleX;
        transform.localScale = scale;
    }

    public bool CancelSpell(string spell){
        if(spell.Equals( spellBuffer, System.StringComparison.OrdinalIgnoreCase) ) {
            anim.SetBool("IsSpelling", false);
            anim.SetTrigger("Hurt");
            HideSpellName();
            spellTimer = 0;
            spellBuffer = "";
            task = Task.Failed;
            status = Status.Idle;
            return true;
        } else return false;
    }
    
}

