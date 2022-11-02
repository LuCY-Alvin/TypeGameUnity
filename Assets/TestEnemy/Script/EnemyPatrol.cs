using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge; 
    [SerializeField] private Transform rightEdge; 

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    
    [Header ("Idel Behavior")]
    [SerializeField] private float idleDuration;
    private float idelTimer;

    [Header ("Enemy Animator")]
    [SerializeField] private Animator anim;

    private void Awake() {
        initScale = enemy.localScale;
    }

    private void Update() {
        if (movingLeft){
            if(enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        } 
        else {
            if(enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void MoveInDirection(int dir){
        idelTimer = 0;
        anim.SetBool("moving", true);

    // Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * dir, initScale.y, initScale.z);

    // Move in that direction
        enemy.position = new Vector3(
            enemy.position.x + Time.deltaTime * dir * speed,
            enemy.position.y,
            enemy.position.z
        );
    }

    private void DirectionChange(){
        anim.SetBool("moving", false);
        idelTimer += Time.deltaTime;

        if(idelTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void OnDisable() {
        anim.SetBool("moving", false);
    }

    public bool IsFacingLeft(){ return movingLeft; }
}
