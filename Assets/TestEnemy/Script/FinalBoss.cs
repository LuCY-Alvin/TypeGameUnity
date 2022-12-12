using System;
using TMPro;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    enum Task { Inactive, Running, Success, Failed }
    enum Status { Idle, Jump, Firebolt }
    
    private Status status;
    private Task task;
    int movementNum = 3;
    
    [Header("Idle")]
    [SerializeField] private float idelTime = 1.0f;
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
            case Status.Firebolt: FireboltSequence(); break;
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

// TODO: Firebolt: move to position and spell three huge Firebolt
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
            anim.SetBool("IsRunning", true);
            CheckFace(fireboltLeftPoint);
            Move(-1);
            return;
        }

        if(!isMovingLeft && transform.position.x <= fireboltRightPoint.position.x){
            anim.SetBool("IsRunning", true);
            CheckFace(fireboltRightPoint);
            Move(1);
            return;
        }

        anim.SetBool("IsRunning", false);

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
        // Debug.Log("CastFb " + fireboltCastCount + "/" + fireboltCastTimes );
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

