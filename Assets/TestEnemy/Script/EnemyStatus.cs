using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private Animator anim;
    private bool isDead;

    private void Awake() {
        currentHealth = startingHealth;
        isDead = false;
        anim = GetComponent<Animator>();
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage){
    // if last hurting animation not end, wont nurt
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        if( animState.IsTag("Inv") ) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0) {
            anim.SetBool("moving", false);
            anim.SetTrigger("hurt");
        }
        else{
            if(!isDead){
                anim.SetBool("moving", false);
                anim.SetTrigger("die");
    
                foreach (Behaviour component in components)
                    component.enabled = false;

                isDead = true;
            }
        }
    }

    public float GetStartingHealth(){ return startingHealth; }
    public float GetCurrentHealth(){ return currentHealth; }
}
