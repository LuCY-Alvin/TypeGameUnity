using System;
using TMPro;
using UnityEngine;

public class FirstBoss : MonoBehaviour
{
    enum Task { Inactive, Running, Success, Failed }
    enum Status { Idle, Dash, Attack, Ult }
    
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

    public PolygonCollider2D enemyCollider;
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
        meteorTimer += Time.deltaTime;

    /* Behaviour */
        switch (status){
            case Status.Idle: Idle(); break;
            case Status.Dash: DashSequence(); break;
            case Status.Attack: AttackSequence(); break;
            case Status.Ult: UltSequence(); break; 
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

    [Header("Dash")]
    [SerializeField] private Transform dashLeftPoint;
    [SerializeField] private Transform dashRightPoint;
    [SerializeField] private float dashSpeed;

    private void DashSequence(){
        if( task == Task.Inactive ){
            float nowX = transform.position.x; 
            isMovingLeft = Mathf.Abs(dashLeftPoint.position.x - nowX) > Mathf.Abs(dashRightPoint.position.x - nowX);
            CheckFace(isMovingLeft ? dashLeftPoint : dashRightPoint );

            anim.SetTrigger("DashCharge");
            anim.SetBool("IsDashing", true);

            task = Task.Running; 
            return;
        }

    /* Running */
    // chargeing
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("dash_charge")){
            task = Task.Running; 
            return;
        }

    // dashing to point 
        if(isMovingLeft && transform.position.x >= dashLeftPoint.position.x){
            CheckFace(dashLeftPoint);
            Dash(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= dashRightPoint.position.x){
            CheckFace(dashRightPoint);
            Dash(1);
            return;
        }

        anim.SetBool("IsDashing", false);
        enemyCollider.gameObject.SetActive(true);
        task = Task.Success;
    }

    private void Dash(int dir){
        transform.position = new Vector3(
            transform.position.x + Time.deltaTime * dir * dashSpeed,
            transform.position.y,
            transform.position.z
        );
    }

    [Header("Attack")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private Transform attackLeftPoint;
    [SerializeField] private Transform attackRightPoint;
    [SerializeField] private Transform attackFirePoint;
    [SerializeField] private float attackCount = 0;
    [SerializeField] private float attackTimes = 3;
    
    private void AttackSequence() {
        if( task == Task.Inactive ){
            int rand = UnityEngine.Random.Range(0, 2);
            isMovingLeft = (rand == 0);

            task = Task.Running; 
            
            return;
        }

    /* Running */
    // move to attack position 
        if(isMovingLeft && transform.position.x >= attackLeftPoint.position.x){
            anim.SetBool("IsRunning", true);
            CheckFace(attackLeftPoint);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= attackRightPoint.position.x){
            anim.SetBool("IsRunning", true);
            CheckFace(attackRightPoint);
            Move(1);
            return;
        }

        anim.SetBool("IsRunning", false);

    // atack
        CheckFace(player.transform);

        if(attackCount < attackTimes){
            anim.SetBool("IsAttacking", true);
            return;
        } else {
            anim.SetBool("IsAttacking", false);
            attackCount = 0;

            task = Task.Success;
            return;
        }
    }
    
    public void Attack(){
        GameObject fireball = Instantiate(attackPrefab, attackFirePoint.position, attackFirePoint.rotation);
        if(player.transform.position.x < transform.position.x) 
            fireball.transform.Rotate(0f, 180f, 0f);

        attackCount += 1;
        // Debug.Log("CastFb " + fireboltCastCount + "/" + fireboltCastTimes );
    }

    
    [Header("Meteor Shower")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private Transform meteorPoint;
    [SerializeField] private Transform[] meteorFirePoint = new Transform[2];
    [SerializeField] private float meteorCooldown;
    [SerializeField] private float meteorHealthPercentage;
    [SerializeField] private float meteorSpellingTime;
    [SerializeField] private float meteorCastingTime;
    
    private int meteorCastCount = 0;
    private int meteorCastTimes = 15;
    private float meteorTimer = Mathf.Infinity;
    private String meteorName = "MeteorShower";
    

    private void UltSequence(){

        if( task == Task.Inactive ){
            float healthP = enemyStatus.GetCurrentHealth() / enemyStatus.GetStartingHealth();
            if(meteorTimer < meteorCooldown || healthP > meteorHealthPercentage){
                idleTimer += idelTime;
                task = Task.Failed;
                return;
            }

            float nowX = transform.position.x; 
            isMovingLeft = transform.position.x > meteorPoint.position.x;

            task = Task.Running; 
            return;
        }

    /* Running */
    // move to spell position 
        if(isMovingLeft && transform.position.x >= meteorPoint.position.x){
            anim.SetBool("IsRunning", true);
            CheckFace(meteorPoint);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= meteorPoint.position.x){
            anim.SetBool("IsRunning", true);
            CheckFace(meteorPoint);
            Move(1);
            return;
        }

        anim.SetBool("IsRunning", false);

    // spelling
        CheckFace(player.transform);
        spellTimer += Time.deltaTime;

        if( spellTimer <= meteorSpellingTime ){
            anim.SetBool("IsSpelling", true);
            spellBuffer = meteorName;
            ShowSpellName();
            return;
        }

    // cast
        anim.SetBool("IsSpelling", false);
        HideSpellName();

        if(meteorCastCount < meteorCastTimes){
            anim.SetBool("IsCasting", true);
            return;
        } else {
            anim.SetBool("IsCasting", false);
            meteorTimer = 0;
            spellTimer = 0;
            spellBuffer = "";
            meteorCastCount = 0;

            task = Task.Success;
            return;
        }
    }

    public void CastMeteor(){
        Vector3 tar = new Vector3(
            UnityEngine.Random.Range(meteorFirePoint[0].position.x, meteorFirePoint[1].position.x),
            meteorFirePoint[0].position.y,
            meteorFirePoint[0].position.z
        );

        GameObject meteor = Instantiate(meteorPrefab, tar, meteorFirePoint[0].rotation);
        meteorCastCount += 1;
        // Debug.Log("CastFb " + fireboltCastCount + "/" + fireboltCastTimes );
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
            anim.SetBool("casting", false);
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

    void OnTriggerEnter2D(Collider2D hitInfo) {
        if(hitInfo.gameObject.tag == "Player" && status == Status.Dash && task == Task.Running){
            enemyCollider.gameObject.GetComponent<EnemyCollider>().HurtPlayer(hitInfo);
            enemyCollider.gameObject.SetActive(false);
        }
    }
}
