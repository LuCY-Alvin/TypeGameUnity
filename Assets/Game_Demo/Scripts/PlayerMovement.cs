using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	public CharacterController2D controller;
	public TimeController btController;
	public CastController castController;
	public Animator animator;

	public HealthBar _healthBar;
	public GameObject bookBox;
    public Text bookBoxCast;
	public Text bookBoxAffix;
	public Button bookBoxBackBtn;
	public Button bookBoxNextBtn;
	public SpellController _spellController;

	// Status
	[SerializeField] private int atkDamage;
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool _BookOpened = false;
	int pages = 0;
	// bool crouch = false;

	/* combat related */
	public Transform combatPoint;
	public float combatRange = 1.5f;
	public LayerMask enemyLayers;
	public bool isInvincible = false;
	public bool isStiffness = false;
	public bool isInjured = false;

	public static Transform _playerTransform;
    // Update is called once per frame
    void Update () {
		if (isStiffness) {
			return;
		}

		_playerTransform = transform;

		if(TimeController.GetIsBulletTime() == false){

			// CastBook
				if (Input.GetKeyDown(KeyCode.E) && !_BookOpened) {

				List<Spell> activeSpells = new() { };
				List<Spell> supportSpells = new() { };
				List<Spell> functionSpells = new() { };

				foreach (Spell spells in _spellController.spellList.spells)
				{
					if (spells.type == "active")
						activeSpells.Add(spells);
					else if (spells.type == "support" && spells.effect != "function" && !supportSpells.Contains(spells))
						supportSpells.Add(spells);
					else if (spells.type == "support" && spells.effect == "function" && !functionSpells.Contains(spells))
						functionSpells.Add(spells);
				}

				bookBoxCast.text = "Skills\n";
				bookBoxAffix.text = "Affix\n";

				var spell = activeSpells[pages];
				if (spell.effect == "function")
				{
					foreach (Spell functionCast in functionSpells)
						bookBoxAffix.text += "\n" + functionCast.name;
				}
				else if (spell.effect == "buff")
				{
					foreach (Spell buffCast in supportSpells)
						if (buffCast.name == "extend")
							bookBoxAffix.text += "\n" + buffCast.name;
				}
				else if (spell.effect == "attack" || spell.effect == "heal")
				{
					foreach (Spell supportCast in supportSpells)
						if (supportCast.name != "extend")
							bookBoxAffix.text += "\n" + supportCast.name;
				}

				bookBoxCast.text += "\n" + spell.name;
				bookBoxBackBtn.gameObject.SetActive(false);
				bookBoxNextBtn.gameObject.SetActive(true);

				bookBoxBackBtn.onClick.AddListener(back);
				bookBoxNextBtn.onClick.AddListener(next);

				void back()
                {
					if (pages>=1)
					{
						bookBoxNextBtn.gameObject.SetActive(true);
						bookBoxCast.text = "Skills\n";
						bookBoxAffix.text = "Affix\n";

						pages -= 1;
						spell = activeSpells[pages];
						bookBoxCast.text += "\n" + spell.name;

						if (spell.effect == "function")
						{
							foreach (Spell functionCast in functionSpells)
								bookBoxAffix.text += "\n" + functionCast.name;
						}
						else if (spell.effect == "buff")
						{
							foreach (Spell buffCast in supportSpells)
								if (buffCast.name == "extend")
									bookBoxAffix.text += "\n" + buffCast.name;
						}
						else if (spell.effect == "attack" || spell.effect == "heal")
						{
							foreach (Spell supportCast in supportSpells)
								if (supportCast.name != "extend")
									bookBoxAffix.text += "\n" + supportCast.name;
						}

						if (pages == 0)
							bookBoxBackBtn.gameObject.SetActive(false);
					}
                }

				void next()
				{
					if (pages <= activeSpells.Count-2)
					{
						bookBoxBackBtn.gameObject.SetActive(true);
						bookBoxCast.text = "Skills\n";
						bookBoxAffix.text = "Affix\n";

						pages += 1;
						spell = activeSpells[pages];
						bookBoxCast.text += "\n" + spell.name;

						if (spell.effect == "function")
						{
							foreach (Spell functionCast in functionSpells)
								bookBoxAffix.text += "\n" + functionCast.name;
						}
						else if (spell.effect == "buff")
						{
							foreach (Spell buffCast in supportSpells)
								if (buffCast.name == "extend")
									bookBoxAffix.text += "\n" + buffCast.name;
						}
						else if (spell.effect == "attack" || spell.effect == "heal")
						{
							foreach (Spell supportCast in supportSpells)
								if (supportCast.name != "extend")
									bookBoxAffix.text += "\n" + supportCast.name;
						}

						if (pages == activeSpells.Count-1)
							bookBoxNextBtn.gameObject.SetActive(false);
					}
				}

				_BookOpened = true;
				bookBox.SetActive(true);

			} else if (Input.GetKeyDown(KeyCode.E) && _BookOpened)
            {
				pages = 0;
				_BookOpened = false;
				bookBox.SetActive(false);
			}

			// Movement
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

			if( Input.GetKeyDown(KeyCode.Return) ) // Bullet time and Start Cast
			{
				bookBox.SetActive(false);
				animator.SetBool("IsTyping", true);
				btController.StartBulletTime();
				castController.StartCast();
			}

			if (Input.GetKeyDown(KeyCode.Space)) // Jump
			{
				jump = true;
				animator.SetBool("IsJumping", true);
			}

			if(Input.GetKeyDown(KeyCode.X))	// Combat
			{
				animator.SetTrigger("Combat");
				StartCoroutine(SetStiffness(0.5f));
				Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(combatPoint.position, combatRange, enemyLayers);

				foreach (Collider2D enemy in hitEnemies){
					// Debug.Log("hit " + enemy.name);
					var currentMp = _healthBar.GetCurrentMp();
            		_healthBar.SetValue(currentMp + 20, "mp");

					if (enemy.GetComponent<EnemyStatus>() != null) {
						enemy.GetComponent<EnemyStatus>().TakeDamage(atkDamage);
					}
					
					if (enemy.GetComponent<MonsterStatus>() != null) {
						enemy.GetComponent<MonsterStatus>().TakeDamage(atkDamage);
					}
                    // Debug.Log(enemy.name);
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
				animator.SetBool("IsTyping", false);
				btController.EndBulletTime();
				castController.EndCast();

				StartCoroutine(SetStiffness(0.6f));
			}
		}

	}
	
	public void CallSpeedUp (Spell[] supportSpells) {
		StartCoroutine(SpeedUp(supportSpells));
	}

	public IEnumerator SpeedUp(Spell[] supportSpells) {
		Debug.Log("SpeedUp");
		var superSupport = Array.Find(
			supportSpells,
			item => item.name == "super"
		);
		
		runSpeed = 80f;
		if (superSupport != null) {
			runSpeed = 120f;
		}
		yield return new WaitForSeconds(4f);
		runSpeed = 80f;
	}

	public IEnumerator SetStiffness(float time) {
		isStiffness = true;
		yield return new WaitForSeconds(time);

		isStiffness = false;
	}

	public IEnumerator SetInjured(float time) {
		isInjured = true;
		yield return new WaitForSeconds(time);

		isInjured = false;
	}

	public void CallTeleport (Spell[] supportSpells) {
		StartCoroutine(Teleport(supportSpells));
	}

	IEnumerator Teleport(Spell[] supportSpells) {
		Vector3 player = transform.position;
		bool canTeleport = false;
		var leftSupport = Array.Find(
			supportSpells,
			item => item.name == "left"
		);

		int distant = 8;

		if (leftSupport != null) {
			distant = -distant;
			if (player.x + distant >= leftSupport.point) {
				player.x += distant;
				canTeleport = true;
			}
			
		}

		var rightSupport = Array.Find(
			supportSpells,
			item => item.name == "right"
		);

		if (rightSupport != null) {
			if (player.x + distant <= rightSupport.point) {
				player.x += distant;
				canTeleport = true;
			}
		}
		
		if (canTeleport) {
			controller.Move(distant * Time.fixedDeltaTime, false, false);
			yield return new WaitForSeconds(0.5f);
			transform.position = player;
			yield return new WaitForSeconds(0.1f);
		}
		
	}

	void OnCollisionExit2D(Collision2D collisionInfo){
		// Debug.Log(collisionInfo.gameObject.tag);
	}

	void FixedUpdate ()
	{			
		if(TimeController.GetIsBulletTime() == false){
		// Move our character
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
			jump = false;
		}
		else{
			controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
			// TODO: casting animation
			animator.SetBool("IsTyping", true);
		}
		
	}

	public void OnLanding(){
		animator.SetBool("IsJumping", false);
	}

	private void OnDrawGizmosSelected() {
		if(combatPoint == null) return;

		Gizmos.DrawWireSphere(combatPoint.position, combatRange);
	}

	public void TakeDamage(int damage, Transform source)
	{
		if (isInvincible || isInjured) {
			return;
		}

		SetInjured(0.5f);

		float f = 5;

		if (transform.position.x < source.position.x) {
			// On left
			f = -5;
		}

		float constR = 300;

		controller.m_Rigidbody2D.AddForce(new Vector2(f * constR, 20f));

		StartCoroutine(SetStiffness(0.8f));
		
		//hurt animation
		animator.SetTrigger("Injured");

		// Debug.Log("Player takes " + damage + " damage!\n");

		_healthBar.SetValue(_healthBar.GetCurrentHp() - damage, "hp");
	}
}
