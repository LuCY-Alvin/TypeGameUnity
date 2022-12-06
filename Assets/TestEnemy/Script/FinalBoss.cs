using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    enum Task { Inactive, Running, Success, Failed }
    enum Status { Idle, Move, Jump }
    
    private Status status;
    private Task task;
    int movementNum = 3;
    
    [Header("Idle")]
    [SerializeField] private float idelTime = 1.0f;
    private float idleTimer = Mathf.Infinity;
    
    [Header("Jump")]
    [SerializeField] private float jumpForceX;
    [SerializeField] private float jumpForceY;
    private bool isGround;


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
    /* Behaviour */
        switch (status){
            case Status.Idle: Idle(); break;
            case Status.Move: Idle(); break;
            case Status.Jump: JumpSequence(); break;
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

    private void MoveSequence(){

    }
    private void Move(){

    }

    private float jumpTimer = 0;
    private float jumpTime = 0.1f;
    private void JumpSequence(){
    /* inactive */
        if( task == Task.Inactive ){
            if(isGround){
                task = Task.Running;

                int direction = (player.transform.position.x < transform.position.x) ? -1 : 1;
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
            CheckFace();
        }

        if(task == Task.Success) jumpTimer = 0;
        return;
    }

    private void Jump(int dir){
        if(isGround == false) return;

        body.AddForce(new Vector2(jumpForceX * dir, jumpForceY));
        anim.SetTrigger("Jump");
    }
    
    private void CheckGround(){
        isGround = baseColl.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void CheckFace(){
        var scale = transform.localScale;
        scale.x = player.transform.position.x < transform.position.x ? -baseScaleX : baseScaleX;
        transform.localScale = scale;
    }
}
