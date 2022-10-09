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
	bool isBTtime = false;
	// bool crouch = false;
	
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.Space) ){
			isBTtime = true;
			castController.StartCast();
		}

		if( Input.GetKeyDown(KeyCode.Return) ){
			isBTtime = false;
			castController.EndCast();
		}

		if( !isBTtime ){
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
			
			if (Input.GetKeyDown(KeyCode.Z))
			{
				jump = true;
				animator.SetBool("IsJumping", true);
			}

			if( Input.GetKeyDown(KeyCode.X)){
				animator.SetTrigger("Combat");
			}
		}
		else{
		// set idle
			horizontalMove = 0; 
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		// TODO: casting sys
			castController.ReadingInput();
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
		// TODO:
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
			// cast
		}
		
	}

	public void OnLanding(){
		animator.SetBool("IsJumping", false);
	}

}
