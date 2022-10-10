using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public CharacterController2D controller;
	public TimeController btController;
	public CastController castController;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	public bool isBTtime = false;
	// bool crouch = false;

	/* combat related */
	public Transform combatPoint;
	public float combatRange = 1.5f;
	public LayerMask enemyLayers;
	
	// Update is called once per frame
	void Update () {

		if( !isBTtime ){
		// Movement
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

			if( Input.GetKeyDown(KeyCode.Space) ) // Bullet time and Start Cast
			{	
				isBTtime = true;
				castController.StartCast();
			}

			if (Input.GetKeyDown(KeyCode.Z))	// Jump
			{
				jump = true;
				animator.SetBool("IsJumping", true);
			}

			if( Input.GetKeyDown(KeyCode.X))	// Cobbat
			{
				animator.SetTrigger("Combat");
			
				Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(combatPoint.position, combatRange, enemyLayers);

				foreach(Collider2D enemy in hitEnemies){
				// TODO: hit reaction 
					Debug.Log("hit " + enemy.name);
				}
			}
		}
		else{
		// set idle
			horizontalMove = 0; 
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
			
		// Reading cast input
			castController.ReadingInput();		

			if( Input.GetKeyDown(KeyCode.Return) ) // End Bullet time and Cast
			{
				isBTtime = false;
				castController.EndCast();
			}
		}


		// if (Input.GetButtonDown("Crouch"))
		// {
		// 	crouch = true;
		// } else if (Input.GetButtonUp("Crouch"))
		// {
		// 	crouch = false;
		// }

	}

	void FixedUpdate ()
	{
		btController.BulletTime(isBTtime);

		if(!isBTtime){
		// Move our character
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
			jump = false;
		}
		else{
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
		// TODO: casting animation
		
		}
		
	}

	public void OnLanding(){
		animator.SetBool("IsJumping", false);
	}

	private void OnDrawGizmosSelected() {
		if(combatPoint == null) return;

		Gizmos.DrawWireSphere(combatPoint.position, combatRange);
	}

}
